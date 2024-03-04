using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.Profiling;

namespace Multiplayer
{
	public class NetStream
	{
		private static Dictionary<byte[], int> bufferRefCount;

		private static int liveStreams;

		private static int memTrappedInStreams;

		private int memTrappedInThisStream;

		private byte[] buffer;

		private NetStream masterStream;

		private int bufferTierAndFlags = -1;

		private int streamRefCount = 1;

		public const bool forceWorstCase = false;

		private int offsetBits;

		private int currentByte;

		private int currentBit;

		private static Queue<NetStream> streamQueue;

		private static object poolLock;

		private static List<Queue<byte[]>> bufferPool;

		private static int bufferPoolTotal;

		private static int bufferPoolMin;

		private static int bufferPoolMin2;

		private static int bufferPoolMin3;

		private static int freeTicker;

		private const int FreeBufferLimit = 1000;

		private const int FreeBufferSillyLimit = 5000;

		public const int MsgId_WorstCaseBits = 4;

		private static Queue<uint> dynamicIds;

		private static Queue<uint> bigDynamicIds;

		public const uint AuxNetIdRangeMin = 288u;

		public const uint AuxNetIdRangeMax = 1054u;

		public const uint MaxNetId = 1054u;

		public const int NetId_WorstCaseBits = 15;

		public const int FrameId_MaxFrameIdBits = 22;

		public const int FrameId_WorstCaseBits = 25;

		public const int SubStream_WorstCaseOverhead = 38;

		public const int SubStream_WorstCaseZeroLen = 5;

		public const int SubStream_MaxLen = 4194303;

		private static MD5 md5Hash;

		public int position => currentByte * 8 + currentBit - offsetBits;

		~NetStream()
		{
			Interlocked.Decrement(ref liveStreams);
			Interlocked.Add(ref memTrappedInStreams, -memTrappedInThisStream);
			memTrappedInThisStream = 0;
		}

		public void Seek(int pos)
		{
			pos += offsetBits;
			currentByte = pos / 8;
			currentBit = pos % 8;
		}

		private NetStream()
		{
			Interlocked.Increment(ref liveStreams);
		}

		private static NetStream GrabStream()
		{
			NetStream netStream = null;
			if (streamQueue.Count > 0)
			{
				netStream = streamQueue.Dequeue();
				if (netStream.streamRefCount != -99)
				{
					Debug.LogErrorFormat("NetStream recycled from pool has a dodgy ref count: {0} (should be 0)", new object[1] { netStream.streamRefCount });
					throw new ApplicationException("Bad ref count!");
				}
				Interlocked.Exchange(ref netStream.streamRefCount, 1);
			}
			else
			{
				netStream = new NetStream();
			}
			return netStream;
		}

		public static NetStream AllocStream(int sizeBytes = 0)
		{
			lock (poolLock)
			{
				NetStream netStream = GrabStream();
				netStream.Alloc(CalculateTierForSize(sizeBytes));
				netStream.offsetBits = 0;
				netStream.Seek(0);
				return netStream;
			}
		}

		public static NetStream AllocStream(NetStream baseStream, int offsetBits = 0)
		{
			lock (poolLock)
			{
				if (baseStream.buffer == null)
				{
					Debug.LogError((object)"AllocStream(baseStream) called when baseStream.buffer==null");
					throw new ApplicationException("Stop!");
				}
				NetStream netStream = GrabStream();
				netStream.bufferTierAndFlags = -1;
				netStream.masterStream = baseStream.AddRef();
				netStream.buffer = baseStream.buffer;
				netStream.offsetBits = offsetBits;
				netStream.Seek(0);
				return netStream;
			}
		}

		public static NetStream AllocStream(byte[] data, int tier = -1, int offsetBits = 0, bool useRefCountOnBuffer = false, bool forceRegister = false)
		{
			lock (poolLock)
			{
				NetStream netStream = GrabStream();
				if (tier >= 0)
				{
					netStream.bufferTierAndFlags = (tier << 1) | (useRefCountOnBuffer ? 1 : 0);
					if (useRefCountOnBuffer)
					{
						netStream.buffer = ReuseBuffer(data, forceRegister);
					}
					else
					{
						netStream.buffer = data;
					}
				}
				else
				{
					netStream.bufferTierAndFlags = -1;
					netStream.buffer = data;
				}
				netStream.memTrappedInThisStream = netStream.buffer.Length;
				Interlocked.Add(ref memTrappedInStreams, netStream.memTrappedInThisStream);
				netStream.offsetBits = offsetBits;
				netStream.Seek(0);
				return netStream;
			}
		}

		private static void ReleaseStream(NetStream stream)
		{
			lock (poolLock)
			{
				int num = Interlocked.Exchange(ref stream.streamRefCount, -99);
				if (num < 0 || num > 1)
				{
					Debug.LogErrorFormat("NetStream.ReleaseStream() called badly on a stream rather than using Release() : refcount={0} instead of 0", new object[1] { num });
				}
				int num2 = stream.bufferTierAndFlags;
				stream.bufferTierAndFlags = -1;
				if (num2 >= 0)
				{
					ReleaseBuffer(num2 >> 1, stream.buffer, (num2 & 1) != 0);
				}
				stream.buffer = null;
				Interlocked.Add(ref memTrappedInStreams, -stream.memTrappedInThisStream);
				stream.memTrappedInThisStream = 0;
				if (stream.masterStream != null)
				{
					stream.masterStream = stream.masterStream.Release();
				}
				streamQueue.Enqueue(stream);
			}
		}

		public static void TickPools()
		{
			lock (poolLock)
			{
				int num = Mathf.Min(bufferPoolMin, Mathf.Min(bufferPoolMin2, bufferPoolMin3));
				if (num > 1000)
				{
					if (num >= 5000 || ++freeTicker >= 5)
					{
						freeTicker = 0;
						int num2 = num - 5000;
						num2 = Mathf.Max(num2 >> 9, 8);
						int i = 0;
						for (int count = bufferPool.Count; i < count; i++)
						{
							if (bufferPool[i].Count > 0)
							{
								int num3 = Mathf.Min(bufferPool[i].Count, num2);
								num2 -= num3;
								bufferPoolTotal -= num3;
								while (num3-- > 0)
								{
									bufferPool[i].Dequeue();
								}
								if (num2 <= 0)
								{
									break;
								}
							}
						}
					}
					num = streamQueue.Count - 5000;
					if (num > 0)
					{
						int num4 = Mathf.Max(num >> 9, 8);
						while (num4-- > 0)
						{
							streamQueue.Dequeue();
						}
					}
				}
				bufferPoolMin3 = bufferPoolMin2;
				bufferPoolMin2 = bufferPoolMin;
				bufferPoolMin = bufferPoolTotal;
			}
		}

		public static int CalculateSizeForTier(int tier)
		{
			return 1200 << tier;
		}

		public static int CalculateTierForSize(int sizeBytes)
		{
			int i;
			for (i = 0; CalculateSizeForTier(i) < sizeBytes; i++)
			{
			}
			return i;
		}

		public static byte[] ReuseBuffer(byte[] data, bool allowAddNewEntry = true)
		{
			lock (poolLock)
			{
				if (!bufferRefCount.TryGetValue(data, out var value))
				{
					if (!allowAddNewEntry)
					{
						Debug.LogError((object)"Reusing non allocated buffer");
						return data;
					}
					bufferRefCount[data] = 1;
					return data;
				}
				bufferRefCount[data] = value + 1;
				return data;
			}
		}

		public static byte[] AllocateBuffer(int tier, bool applyRefCount = true)
		{
			lock (poolLock)
			{
				byte[] array;
				if (tier < bufferPool.Count && bufferPool[tier].Count > 0)
				{
					array = bufferPool[tier].Dequeue();
					bufferPoolTotal--;
					if (bufferPoolTotal < bufferPoolMin)
					{
						bufferPoolMin = bufferPoolTotal;
					}
				}
				else
				{
					array = new byte[CalculateSizeForTier(tier)];
				}
				if (applyRefCount)
				{
					bufferRefCount[array] = 1;
				}
				return array;
			}
		}

		private static void DumpBufferStats()
		{
			List<string> list = new List<string>();
			DumpBufferStats(list);
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				Debug.Log((object)list[i]);
			}
		}

		public static void DumpBufferStats(List<string> output)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			lock (poolLock)
			{
				foreach (KeyValuePair<byte[], int> item in bufferRefCount)
				{
					int num9 = item.Key.Length;
					int value = item.Value;
					num2 += num9;
					num++;
					if (value > 1)
					{
						num3++;
					}
				}
				num6 = streamQueue.Count;
				num7 = liveStreams;
				num8 = memTrappedInStreams;
				foreach (Queue<byte[]> item2 in bufferPool)
				{
					foreach (byte[] item3 in item2)
					{
						num4++;
						num5 += item3.Length;
					}
				}
			}
			output.Add("NetStream stats:");
			output.Add($"  External ref-counted buffers: num_buffers={num}, total_data={num2:N0}, num_shared={num3}");
			output.Add($"  Buffer ticking: min={bufferPoolMin}/{bufferPoolMin2}/{bufferPoolMin3}, total={bufferPoolTotal}");
			output.Add($"  Buffers free-pool: num_items={num4}, total_data={num5:N0}");
			output.Add($"  Live NetStreams={num7}, total_data_attached={num8:N0}, in_free_pool={num6}");
			output.Add($"  GC total-size={GC.GetTotalMemory(forceFullCollection: false):N0}");
		}

		public static void ReleaseBuffer(int bufferTier, byte[] buffer, bool applyRefCount = true)
		{
			lock (poolLock)
			{
				if (!applyRefCount)
				{
					goto IL_0069;
				}
				if (bufferRefCount.TryGetValue(buffer, out var value))
				{
					if (--value <= 0)
					{
						bufferRefCount.Remove(buffer);
						goto IL_0069;
					}
					bufferRefCount[buffer] = value;
				}
				else if (!NetGame.instance.multithreading)
				{
					Debug.LogError((object)"Releasing buffer without references");
				}
				goto end_IL_000c;
				IL_0069:
				while (bufferTier >= bufferPool.Count)
				{
					bufferPool.Add(new Queue<byte[]>());
				}
				bufferPool[bufferTier].Enqueue(buffer);
				bufferPoolTotal++;
				end_IL_000c:;
			}
		}

		public static void DiscardPools()
		{
			lock (poolLock)
			{
				bufferPool.Clear();
				streamQueue.Clear();
				bufferPoolTotal = (bufferPoolMin = (bufferPoolMin3 = (bufferPoolMin2 = 0)));
				bufferRefCount.Clear();
			}
		}

		private void Alloc(int tier)
		{
			lock (poolLock)
			{
				byte[] array = buffer;
				int num = bufferTierAndFlags;
				buffer = AllocateBuffer(tier, applyRefCount: false);
				bufferTierAndFlags = (tier << 1) | 0;
				Interlocked.Add(ref memTrappedInStreams, buffer.Length - memTrappedInThisStream);
				memTrappedInThisStream = buffer.Length;
				if (array != null)
				{
					Array.Copy(array, buffer, array.Length);
					if (num >= 0)
					{
						ReleaseBuffer(num >> 1, array, (num & 1) != 0);
					}
				}
				if (masterStream != null)
				{
					masterStream = masterStream.Release();
				}
			}
		}

		private void Expand(int targetSize)
		{
			if (bufferTierAndFlags < 0)
			{
				Debug.LogWarning((object)"Expanding non pooled NetStream");
			}
			int tier = CalculateTierForSize(targetSize);
			Alloc(tier);
		}

		public NetStream AddRef()
		{
			if (Interlocked.Increment(ref streamRefCount) <= 1)
			{
				throw new ApplicationException("NetStream.AddRef() called on a dead object");
			}
			return this;
		}

		public NetStream Release()
		{
			try
			{
				int num = Interlocked.Decrement(ref streamRefCount);
				if (num < 0)
				{
					throw new ApplicationException("NetStream.Release() called on a dead object");
				}
				if (num == 0)
				{
					ReleaseStream(this);
				}
			}
			catch (Exception ex)
			{
				Debug.LogException(ex);
			}
			return null;
		}

		private void EnsureFitsBits(int extraBits)
		{
			extraBits -= 8 - currentBit;
			int num = (position + extraBits + 7) / 8;
			if (num > buffer.Length)
			{
				Expand(num);
			}
		}

		private void EnsureFitsBytes(int extraBytes, bool pad)
		{
			int num = extraBytes * 8;
			if (pad)
			{
				num += 8 - currentBit;
			}
			EnsureFitsBits(num);
		}

		private void Advance()
		{
			if (currentBit == 7)
			{
				currentBit = 0;
				currentByte++;
			}
			else
			{
				currentBit++;
			}
		}

		private void Advance(int bits)
		{
			currentBit += bits;
			while (currentBit > 8)
			{
				currentBit -= 8;
				currentByte++;
			}
		}

		public void Write(bool v)
		{
			if (currentBit == 0 && currentByte == buffer.Length)
			{
				Expand(currentByte + 32);
			}
			byte b = (byte)(128 >> currentBit);
			if (v)
			{
				buffer[currentByte] = (byte)(buffer[currentByte] | b);
			}
			else
			{
				buffer[currentByte] = (byte)(buffer[currentByte] & ~b);
			}
			Advance();
		}

		public bool ReadBool()
		{
			if (currentByte >= buffer.Length)
			{
				Debug.LogError((object)"Buffer overrun");
			}
			byte b = (byte)(128 >> currentBit);
			bool result = (buffer[currentByte] & b) != 0;
			Advance();
			return result;
		}

		public void Skip(int bits)
		{
			Advance(bits);
		}

		public void Write(byte b)
		{
			Write((uint)b, (ushort)8);
		}

		public byte ReadByte()
		{
			return (byte)ReadUInt32(8);
		}

		public void Write(int x, ushort bitsmall, ushort bitlarge)
		{
			int num = 1 << bitsmall - 1;
			if (x >= -num && x < num)
			{
				Write(v: true);
				Write(x, bitsmall);
			}
			else
			{
				Write(v: false);
				Write(x, bitlarge);
			}
		}

		public void Write(int x, ushort bitsmall, ushort bitmed, ushort bitlarge)
		{
			int num = 1 << bitsmall - 1;
			if (x >= -num && x < num)
			{
				Write(v: true);
				Write(x, bitsmall);
				return;
			}
			Write(v: false);
			int num2 = 1 << bitmed - 1;
			if (x >= -num2 && x < num2)
			{
				Write(v: true);
				Write(x, bitmed);
			}
			else
			{
				Write(v: false);
				Write(x, bitlarge);
			}
		}

		public void Write(int x, ushort bits)
		{
			Write((uint)x, bits);
		}

		public void Write(uint x, ushort bitfull)
		{
			int num = 8;
			while (bitfull > 0 && num-- > 0)
			{
				if (currentBit == 0 && currentByte == buffer.Length)
				{
					Expand(currentByte + 32);
				}
				byte b = (byte)(255 >> currentBit);
				if (currentBit + bitfull < 8)
				{
					byte b2 = (byte)(x << 8 - currentBit - bitfull);
					b = (byte)(b & (byte)(255 << 8 - currentBit - bitfull));
					buffer[currentByte] = (byte)((buffer[currentByte] & ~b) | (b2 & b));
					currentBit += bitfull;
					bitfull = 0;
				}
				else
				{
					byte b3 = (byte)(x >> bitfull - (8 - currentBit));
					buffer[currentByte] = (byte)((buffer[currentByte] & ~b) | (b3 & b));
					bitfull = (ushort)(bitfull - (8 - currentBit));
					currentBit = 0;
					currentByte++;
				}
			}
		}

		public void Write(uint x, ushort bitsmall, ushort bitlarge)
		{
			int num = 1 << (int)bitsmall;
			if (x < num)
			{
				Write(v: true);
				Write(x, bitsmall);
			}
			else
			{
				Write(v: false);
				Write(x, bitlarge);
			}
		}

		public void Write(uint x, ushort bitsmall, ushort bitmed, ushort bitlarge)
		{
			int num = 1 << (int)bitsmall;
			if (x < num)
			{
				Write(v: true);
				Write(x, bitsmall);
				return;
			}
			Write(v: false);
			int num2 = 1 << (int)bitmed;
			if (x < num2)
			{
				Write(v: true);
				Write(x, bitmed);
			}
			else
			{
				Write(v: false);
				Write(x, bitlarge);
			}
		}

		public static uint PredictWriteLen(uint x, ushort bitsmall, ushort bitmed, ushort bitlarge)
		{
			if (x < 1 << (int)bitsmall)
			{
				return (uint)(1 + bitsmall);
			}
			if (x >= 1 << (int)bitmed)
			{
				return (uint)(2 + bitlarge);
			}
			return (uint)(2 + bitmed);
		}

		public uint ReadUInt32(ushort bitfull)
		{
			if (!NetGame.instance.multithreading)
			{
				Profiler.BeginSample("ReadUInt");
			}
			int num = 8;
			int num2 = 0;
			while (bitfull > 0 && num-- > 0)
			{
				if (currentByte > buffer.Length - 1)
				{
					Debug.LogError((object)"Buffer overrun");
				}
				byte b = buffer[currentByte];
				b = (byte)(b & (byte)(255 >> currentBit));
				if (currentBit + bitfull < 8)
				{
					b = (byte)(b >> 8 - (currentBit + bitfull));
					currentBit += bitfull;
					bitfull = 0;
				}
				else
				{
					bitfull = (ushort)(bitfull - (8 - currentBit));
					currentBit = 0;
					currentByte++;
				}
				num2 += b << (int)bitfull;
			}
			if (!NetGame.instance.multithreading)
			{
				Profiler.EndSample();
			}
			return (uint)num2;
		}

		public int ReadInt32(ushort bitfull)
		{
			int num = (int)ReadUInt32(bitfull);
			int num2 = 1 << bitfull - 1;
			if ((num & num2) != 0)
			{
				num |= -1 << (int)bitfull;
			}
			return num;
		}

		public int ReadInt32(ushort bitsmall, ushort bitlarge)
		{
			if (ReadBool())
			{
				return ReadInt32(bitsmall);
			}
			return ReadInt32(bitlarge);
		}

		public int ReadInt32(ushort bitsmall, ushort bitmed, ushort bitlarge)
		{
			if (ReadBool())
			{
				return ReadInt32(bitsmall);
			}
			if (ReadBool())
			{
				return ReadInt32(bitmed);
			}
			return ReadInt32(bitlarge);
		}

		public uint ReadUInt32(ushort bitsmall, ushort bitlarge)
		{
			if (ReadBool())
			{
				return ReadUInt32(bitsmall);
			}
			return ReadUInt32(bitlarge);
		}

		public uint ReadUInt32(ushort bitsmall, ushort bitmed, ushort bitlarge)
		{
			if (ReadBool())
			{
				return ReadUInt32(bitsmall);
			}
			if (ReadBool())
			{
				return ReadUInt32(bitmed);
			}
			return ReadUInt32(bitlarge);
		}

		public byte[] ToArray()
		{
			int num = (position + 7) / 8;
			byte[] array = new byte[num];
			Array.Copy(buffer, 0, array, 0, num);
			return array;
		}

		public byte[] GetOriginalBuffer()
		{
			return buffer;
		}

		public int UseBuffedSize()
		{
			return (position + 7) / 8;
		}

		public void PadToByte()
		{
			if (currentBit > 0)
			{
				currentBit = 0;
				currentByte++;
			}
		}

		public void WriteArray(byte[] array, ushort lenBits = 8)
		{
			PadToByte();
			EnsureFitsBytes(array.Length + (lenBits + 7) / 8, pad: true);
			int num = array.Length;
			Write(num, lenBits);
			Array.Copy(array, 0, buffer, currentByte, num);
			currentByte += num;
		}

		public byte[] ReadArray(ushort lenBits = 8)
		{
			PadToByte();
			int num = ReadInt32(lenBits);
			byte[] array = new byte[num];
			Array.Copy(buffer, currentByte, array, 0, num);
			currentByte += num;
			return array;
		}

		public void WriteArray2(byte[] array, ushort lenBits = 8)
		{
			int num = array.Length;
			Write(num, lenBits);
			PadToByte();
			EnsureFitsBytes(num, pad: true);
			Array.Copy(array, 0, buffer, currentByte, num);
			currentByte += num;
		}

		public byte[] ReadArray2(ushort lenBits = 8)
		{
			uint num = ReadUInt32(lenBits);
			PadToByte();
			byte[] array = new byte[num];
			Array.Copy(buffer, currentByte, array, 0L, num);
			currentByte += (int)num;
			return array;
		}

		public void WriteFixedArray(byte[] array, int len, int srcOffset = 0)
		{
			PadToByte();
			EnsureFitsBytes(len, pad: true);
			Array.Copy(array, srcOffset, buffer, currentByte, len);
			currentByte += len;
		}

		public byte[] ReadFixedArray(int len)
		{
			PadToByte();
			byte[] array = new byte[len];
			Array.Copy(buffer, currentByte, array, 0, len);
			currentByte += len;
			return array;
		}

		public void WriteMsgId(NetMsgId id)
		{
			Write((int)id, 4);
		}

		public NetMsgId ReadMsgId()
		{
			return (NetMsgId)ReadUInt32(4);
		}

		static NetStream()
		{
			bufferRefCount = new Dictionary<byte[], int>();
			liveStreams = 0;
			memTrappedInStreams = 0;
			streamQueue = new Queue<NetStream>();
			poolLock = new object();
			bufferPool = new List<Queue<byte[]>>();
			bufferPoolTotal = 0;
			bufferPoolMin = 0;
			bufferPoolMin2 = 0;
			bufferPoolMin3 = 0;
			freeTicker = 0;
			dynamicIds = new Queue<uint>();
			bigDynamicIds = new Queue<uint>();
			md5Hash = MD5.Create();
			for (uint num = 8u; num < 32; num++)
			{
				dynamicIds.Enqueue(num);
			}
			for (uint num2 = 32u; num2 < 288; num2++)
			{
				dynamicIds.Enqueue(num2);
			}
		}

		public static uint GetDynamicScopeId()
		{
			if (dynamicIds.Count > 0)
			{
				return dynamicIds.Dequeue();
			}
			if (bigDynamicIds.Count > 0)
			{
				return bigDynamicIds.Dequeue();
			}
			throw new Exception("Ran out of net IDs");
		}

		public static void ReturnDynamicScopeId(uint id)
		{
			if (id < 32)
			{
				dynamicIds.Enqueue(id);
			}
			else
			{
				bigDynamicIds.Enqueue(id);
			}
		}

		public void WriteNetId(uint id)
		{
			if (id >= 31)
			{
				Write(31, 5);
				Write(id - 31, 10);
			}
			else
			{
				Write(id, 5);
			}
		}

		public uint ReadNetId()
		{
			uint num = ReadUInt32(5);
			if (num == 31)
			{
				num = ReadUInt32(10) + 31;
			}
			return num;
		}

		public static uint PredictNetIdSize(uint id)
		{
			if (id < 31)
			{
				return 5u;
			}
			return 15u;
		}

		public void WriteFrameId(int frameId, int streamFrameId)
		{
			if (frameId == 0)
			{
				Write(v: true);
				return;
			}
			Write(v: false);
			Write(frameId - streamFrameId, 5, 9, 22);
		}

		public int ReadFrameId(int streamFrameId)
		{
			if (!ReadBool())
			{
				return (ReadInt32(5, 9, 22) + streamFrameId) & 0x3FFFFF;
			}
			return 0;
		}

		public string ReadString()
		{
			byte[] array = ReadArray(32);
			return Encoding.UTF8.GetString(array, 0, array.Length);
		}

		public void Write(string text)
		{
			byte[] array = new byte[Encoding.UTF8.GetByteCount(text)];
			Encoding.UTF8.GetBytes(text, 0, text.Length, array, 0);
			WriteArray(array, 32);
		}

		public void WriteStream(NetStream stream)
		{
			if (stream == null)
			{
				EnsureFitsBits(5);
				Write(0, 4, 14, 22);
				return;
			}
			uint num = (uint)stream.UseBuffedSize();
			if (num < 16)
			{
				EnsureFitsBits((int)(5 + num * 8));
				Write(num, 4, 14, 22);
				for (int i = 0; i < num; i++)
				{
					Write(stream.buffer[i]);
				}
			}
			else
			{
				Write(num, 4, 14, 22);
				EnsureFitsBytes(stream.UseBuffedSize(), pad: true);
				PadToByte();
				Array.Copy(stream.GetOriginalBuffer(), 0L, buffer, currentByte, num);
				currentByte += (int)num;
			}
		}

		public static uint PredictStreamAdvance(uint streamSizeInBytes, uint bitpos)
		{
			bitpos += PredictWriteLen(streamSizeInBytes, 4, 14, 22);
			if (streamSizeInBytes >= 16)
			{
				bitpos = (uint)((bitpos + 7) & -8);
			}
			bitpos += streamSizeInBytes << 3;
			return bitpos;
		}

		public NetStream ReadStream(bool forceIndependent = false)
		{
			uint num = ReadUInt32(4, 14, 22);
			NetStream netStream = null;
			switch (num)
			{
			case 0u:
				netStream = null;
				break;
			case 1u:
			case 2u:
			case 3u:
			case 4u:
			case 5u:
			case 6u:
			case 7u:
			case 8u:
			case 9u:
			case 10u:
			case 11u:
			case 12u:
			case 13u:
			case 14u:
			case 15u:
				if (forceIndependent)
				{
					netStream = AllocStream((int)num);
					for (int i = 0; i < num; i++)
					{
						netStream.buffer[i] = ReadByte();
					}
				}
				else
				{
					int num3 = position;
					currentByte += (int)num;
					netStream = AllocStream(this, num3);
				}
				break;
			default:
				PadToByte();
				if (forceIndependent)
				{
					netStream = AllocStream((int)num);
					Array.Copy(buffer, currentByte, netStream.buffer, 0L, num);
					currentByte += (int)num;
				}
				else
				{
					int num2 = position;
					currentByte += (int)num;
					netStream = AllocStream(this, num2);
				}
				break;
			}
			return netStream;
		}

		public string GetCRC()
		{
			buffer[currentByte] = (byte)(buffer[currentByte] & ~(255 >> currentBit));
			byte[] array = md5Hash.ComputeHash(buffer, 0, UseBuffedSize());
			if (array == null)
			{
				return "(null)";
			}
			string text = "";
			for (int i = 0; i < array.Length; i++)
			{
				text += array[i].ToString("X2");
			}
			return text;
		}
	}
}

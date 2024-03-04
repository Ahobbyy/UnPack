using System;
using UnityEngine;

namespace Multiplayer
{
	public struct NetQuaternion
	{
		public byte sel;

		public int x;

		public int y;

		public int z;

		public const float quatrange = 0.707107f;

		public ushort bits;

		public NetQuaternion(byte sel, int a, int b, int c, ushort bits)
		{
			this.sel = sel;
			x = a;
			y = b;
			z = c;
			this.bits = bits;
		}

		public NetQuaternion(byte sel, float a, float b, float c, float drop, ushort bits)
		{
			if (drop < 0f)
			{
				a = 0f - a;
				b = 0f - b;
				c = 0f - c;
			}
			this.sel = sel;
			x = NetFloat.Quantize(a, 0.707107f, bits);
			y = NetFloat.Quantize(b, 0.707107f, bits);
			z = NetFloat.Quantize(c, 0.707107f, bits);
			this.bits = bits;
		}

		public static NetQuaternion Quantize(Quaternion q, ushort bits)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Abs(q.x);
			float num2 = Mathf.Abs(q.y);
			float num3 = Mathf.Abs(q.z);
			float num4 = Mathf.Abs(q.w);
			return ((num > num2) ? ((num > num3) ? ((!(num > num4)) ? 3 : 0) : ((!(num3 > num4)) ? 3 : 2)) : ((num2 > num3) ? ((num2 > num4) ? 1 : 3) : ((!(num3 > num4)) ? 3 : 2))) switch
			{
				0 => new NetQuaternion(0, q.y, q.z, q.w, q.x, bits), 
				1 => new NetQuaternion(1, q.x, q.z, q.w, q.y, bits), 
				2 => new NetQuaternion(2, q.x, q.y, q.w, q.z, bits), 
				3 => new NetQuaternion(3, q.x, q.y, q.z, q.w, bits), 
				_ => throw new InvalidOperationException("can't get here"), 
			};
		}

		public Quaternion Dequantize()
		{
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			float num = NetFloat.Dequantize(x, 0.707107f, bits);
			float num2 = NetFloat.Dequantize(y, 0.707107f, bits);
			float num3 = NetFloat.Dequantize(z, 0.707107f, bits);
			float num4 = Mathf.Sqrt(1f - num * num - num2 * num2 - num3 * num3);
			return (Quaternion)(sel switch
			{
				0 => new Quaternion(num4, num, num2, num3), 
				1 => new Quaternion(num, num4, num2, num3), 
				2 => new Quaternion(num, num2, num4, num3), 
				3 => new Quaternion(num, num2, num3, num4), 
				_ => throw new InvalidOperationException("can't get here"), 
			});
		}

		public static NetQuaternionDelta Delta(NetQuaternion from, NetQuaternion to, ushort bitlarge)
		{
			int num = to.x - from.x;
			int num2 = to.y - from.y;
			int num3 = to.z - from.z;
			int num4 = 1 << bitlarge - 1;
			NetQuaternionDelta result;
			if (num < -num4 || num >= num4 || num2 < -num4 || num2 >= num4 || num3 < -num4 || num3 >= num4)
			{
				result = default(NetQuaternionDelta);
				result.isRelative = false;
				result.sel = to.sel;
				result.x = to.x;
				result.y = to.y;
				result.z = to.z;
				return result;
			}
			result = default(NetQuaternionDelta);
			result.isRelative = true;
			result.sel = to.sel;
			result.x = num;
			result.y = num2;
			result.z = num3;
			return result;
		}

		public static NetQuaternionDelta WorstCaseDelta(NetQuaternion from, NetQuaternion to)
		{
			NetQuaternionDelta result = default(NetQuaternionDelta);
			result.isRelative = false;
			result.sel = to.sel;
			result.x = to.x;
			result.y = to.y;
			result.z = to.z;
			return result;
		}

		public static NetQuaternion AddDelta(NetQuaternion from, NetQuaternionDelta delta)
		{
			if (delta.isRelative)
			{
				return new NetQuaternion(delta.sel, from.x + delta.x, from.y + delta.y, from.z + delta.z, from.bits);
			}
			return new NetQuaternion(delta.sel, delta.x, delta.y, delta.z, from.bits);
		}

		public void Write(NetStream stream)
		{
			stream.Write(sel, 2);
			stream.Write(x, bits);
			stream.Write(y, bits);
			stream.Write(z, bits);
		}

		public static NetQuaternion Read(NetStream stream, ushort bits)
		{
			return new NetQuaternion((byte)((uint)stream.ReadInt32(2) & 3u), stream.ReadInt32(bits), stream.ReadInt32(bits), stream.ReadInt32(bits), bits);
		}

		public static bool operator ==(NetQuaternion a, NetQuaternion b)
		{
			if (a.sel == b.sel && a.x == b.x && a.y == b.y)
			{
				return a.z == b.z;
			}
			return false;
		}

		public static bool operator !=(NetQuaternion a, NetQuaternion b)
		{
			return !(a == b);
		}

		public override string ToString()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			Quaternion val = Dequantize();
			return $"({sel},{x},{y},{z}) ({val.x},{val.y},{val.z},{val.w}) {((Quaternion)(ref val)).get_eulerAngles()}";
		}
	}
}

using UnityEngine;

namespace Multiplayer
{
	public struct NetVector3
	{
		public int x;

		public int y;

		public int z;

		public ushort bits;

		public NetVector3(int x, int y, int z, ushort bits)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.bits = bits;
		}

		public static NetVector3 Quantize(Vector3 vec, float range, ushort bits)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			return new NetVector3(NetFloat.Quantize(vec.x, range, bits), NetFloat.Quantize(vec.y, range, bits), NetFloat.Quantize(vec.z, range, bits), bits);
		}

		public static NetVector3 Quantize(Vector3 vec, Vector3 range, ushort bits)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			return new NetVector3(NetFloat.Quantize(vec.x, range.x, bits), NetFloat.Quantize(vec.y, range.y, bits), NetFloat.Quantize(vec.z, range.z, bits), bits);
		}

		public Vector3 Dequantize(float range)
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(NetFloat.Dequantize(x, range, bits), NetFloat.Dequantize(y, range, bits), NetFloat.Dequantize(z, range, bits));
		}

		public Vector3 Dequantize(Vector3 range)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3(NetFloat.Dequantize(x, range.x, bits), NetFloat.Dequantize(y, range.y, bits), NetFloat.Dequantize(z, range.z, bits));
		}

		public static NetVector3Delta Delta(NetVector3 from, NetVector3 to, ushort bitlarge)
		{
			int num = to.x - from.x;
			int num2 = to.y - from.y;
			int num3 = to.z - from.z;
			int num4 = 1 << bitlarge - 1;
			NetVector3Delta result;
			if (num < -num4 || num >= num4 || num2 < -num4 || num2 >= num4 || num3 < -num4 || num3 >= num4)
			{
				result = default(NetVector3Delta);
				result.isRelative = false;
				result.x = to.x;
				result.y = to.y;
				result.z = to.z;
				return result;
			}
			result = default(NetVector3Delta);
			result.isRelative = true;
			result.x = num;
			result.y = num2;
			result.z = num3;
			return result;
		}

		public static NetVector3Delta WorstCaseDelta(NetVector3 from, NetVector3 to)
		{
			NetVector3Delta result = default(NetVector3Delta);
			result.isRelative = false;
			result.x = to.x;
			result.y = to.y;
			result.z = to.z;
			return result;
		}

		public static NetVector3 AddDelta(NetVector3 from, NetVector3Delta delta)
		{
			if (delta.isRelative)
			{
				return new NetVector3(from.x + delta.x, from.y + delta.y, from.z + delta.z, from.bits);
			}
			return new NetVector3(delta.x, delta.y, delta.z, from.bits);
		}

		public void Write(NetStream stream)
		{
			stream.Write(x, bits);
			stream.Write(y, bits);
			stream.Write(z, bits);
		}

		public static NetVector3 Read(NetStream stream, ushort bits)
		{
			return new NetVector3(stream.ReadInt32(bits), stream.ReadInt32(bits), stream.ReadInt32(bits), bits);
		}

		public void Write(NetStream stream, ushort bitsShort)
		{
			stream.Write(x, bitsShort, bits);
			stream.Write(y, bitsShort, bits);
			stream.Write(z, bitsShort, bits);
		}

		public static NetVector3 Read(NetStream stream, ushort bitsShort, ushort bits)
		{
			return new NetVector3(stream.ReadInt32(bitsShort, bits), stream.ReadInt32(bitsShort, bits), stream.ReadInt32(bitsShort, bits), bits);
		}

		public override string ToString()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Dequantize(500f);
			return $"({x},{y},{z}) ({val.x},{val.y}, {val.z})";
		}

		public static bool operator ==(NetVector3 a, NetVector3 b)
		{
			if (a.x == b.x && a.y == b.y)
			{
				return a.z == b.z;
			}
			return false;
		}

		public static bool operator !=(NetVector3 a, NetVector3 b)
		{
			return !(a == b);
		}
	}
}

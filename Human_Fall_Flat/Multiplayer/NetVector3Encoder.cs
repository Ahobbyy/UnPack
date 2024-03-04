using UnityEngine;

namespace Multiplayer
{
	public class NetVector3Encoder
	{
		public Vector3 startPos = Vector3.get_zero();

		public float range;

		public ushort fullBits;

		public ushort deltaSmall;

		public ushort deltaLarge;

		public NetVector3Encoder(float range = 500f, ushort fullBits = 18, ushort deltaSmall = 4, ushort deltaLarge = 8)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			this.range = range;
			this.fullBits = fullBits;
			this.deltaSmall = deltaSmall;
			this.deltaLarge = deltaLarge;
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			return NetVector3Delta.CalculateMaxDeltaSizeInBits(deltaSmall, deltaLarge, fullBits);
		}

		public NetVector3 Quantize(Vector3 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return NetVector3.Quantize(value, range, fullBits);
		}

		public Vector3 Dequantize(NetVector3 value)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return value.Dequantize(range);
		}

		public void CollectState(NetStream stream, Vector3 value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			NetVector3.Quantize(value, range, fullBits).Write(stream);
		}

		public Vector3 ApplyState(NetStream state)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			return NetVector3.Read(state, fullBits).Dequantize(range);
		}

		public Vector3 ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = NetVector3.Read(state0, fullBits).Dequantize(range);
			Vector3 val2 = NetVector3.Read(state1, fullBits).Dequantize(range);
			Vector3 val3 = val - val2;
			if (((Vector3)(ref val3)).get_sqrMagnitude() > 15f)
			{
				val = val2;
			}
			return Vector3.Lerp(val, val2, mix);
		}

		public bool CalculateDelta(NetStream state0, NetStream state1, NetStream delta, bool writeChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			NetVector3 netVector = ((state0 == null) ? NetVector3.Quantize(startPos, range, fullBits) : NetVector3.Read(state0, fullBits));
			NetVector3 netVector2 = NetVector3.Read(state1, fullBits);
			if (netVector == netVector2)
			{
				if (writeChanged)
				{
					delta.Write(v: false);
				}
				return false;
			}
			if (writeChanged)
			{
				delta.Write(v: true);
			}
			NetVector3.Delta(netVector, netVector2, deltaLarge).Write(delta, deltaSmall, deltaLarge, fullBits);
			return true;
		}

		public bool CalculateWorstCaseDelta(NetStream state0, NetStream state1, NetStream delta, bool writeChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			NetVector3 from = ((state0 == null) ? NetVector3.Quantize(startPos, range, fullBits) : NetVector3.Read(state0, fullBits));
			NetVector3 to = ((state1 == null) ? NetVector3.Quantize(startPos, range, fullBits) : NetVector3.Read(state1, fullBits));
			if (writeChanged)
			{
				delta.Write(v: true);
			}
			NetVector3.WorstCaseDelta(from, to).Write(delta, deltaSmall, deltaLarge, fullBits);
			return true;
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result, bool readChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			NetVector3 from = ((state0 == null) ? NetVector3.Quantize(startPos, range, fullBits) : NetVector3.Read(state0, fullBits));
			if (delta == null || (readChanged && !delta.ReadBool()))
			{
				from.Write(result);
				return;
			}
			NetVector3Delta delta2 = NetVector3Delta.Read(delta, deltaSmall, deltaLarge, fullBits);
			NetVector3.AddDelta(from, delta2).Write(result);
		}
	}
}

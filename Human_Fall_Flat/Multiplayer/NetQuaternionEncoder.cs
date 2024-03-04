using UnityEngine;

namespace Multiplayer
{
	public class NetQuaternionEncoder
	{
		public Quaternion startRot = Quaternion.get_identity();

		public ushort fullBits;

		public ushort deltaSmall;

		public ushort deltaLarge;

		public NetQuaternionEncoder(ushort fullBits = 9, ushort deltaSmall = 4, ushort deltaLarge = 6)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			this.fullBits = fullBits;
			this.deltaSmall = deltaSmall;
			this.deltaLarge = deltaLarge;
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			return NetQuaternionDelta.CalculateMaxDeltaSizeInBits(deltaSmall, deltaLarge, fullBits);
		}

		public void CollectState(NetStream stream, Quaternion value)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			NetQuaternion.Quantize(value, fullBits).Write(stream);
		}

		public Quaternion ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			Quaternion val = NetQuaternion.Read(state0, fullBits).Dequantize();
			Quaternion val2 = NetQuaternion.Read(state1, fullBits).Dequantize();
			return Quaternion.Slerp(val, val2, mix);
		}

		public Quaternion ApplyState(NetStream state)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			return NetQuaternion.Read(state, fullBits).Dequantize();
		}

		public bool CalculateDelta(NetStream state0, NetStream state1, NetStream delta, bool writeChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			NetQuaternion netQuaternion = ((state0 == null) ? NetQuaternion.Quantize(startRot, fullBits) : NetQuaternion.Read(state0, fullBits));
			NetQuaternion netQuaternion2 = NetQuaternion.Read(state1, fullBits);
			if (netQuaternion == netQuaternion2)
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
			NetQuaternion.Delta(netQuaternion, netQuaternion2, deltaLarge).Write(delta, deltaSmall, deltaLarge, fullBits);
			return true;
		}

		public bool CalculateWorstCaseDelta(NetStream state0, NetStream state1, NetStream delta, bool writeChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			NetQuaternion from = ((state0 == null) ? NetQuaternion.Quantize(startRot, fullBits) : NetQuaternion.Read(state0, fullBits));
			NetQuaternion to = ((state1 == null) ? NetQuaternion.Quantize(startRot, fullBits) : NetQuaternion.Read(state1, fullBits));
			if (writeChanged)
			{
				delta.Write(v: true);
			}
			NetQuaternion.WorstCaseDelta(from, to).Write(delta, deltaSmall, deltaLarge, fullBits);
			return true;
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result, bool readChanged = true)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			NetQuaternion from = ((state0 == null) ? NetQuaternion.Quantize(startRot, fullBits) : NetQuaternion.Read(state0, fullBits));
			if (delta == null || (readChanged && !delta.ReadBool()))
			{
				from.Write(result);
				return;
			}
			NetQuaternionDelta delta2 = NetQuaternionDelta.Read(delta, deltaSmall, deltaLarge, fullBits);
			NetQuaternion.AddDelta(from, delta2).Write(result);
		}
	}
}

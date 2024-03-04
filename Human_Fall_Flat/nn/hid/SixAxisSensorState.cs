using System.Runtime.InteropServices;
using nn.util;

namespace nn.hid
{
	public struct SixAxisSensorState
	{
		public const float AccelerometerMax = 7f;

		public const float AngularVelocityMax = 5f;

		public long deltaTimeNanoSeconds;

		public long samplingNumber;

		public Float3 acceleration;

		public Float3 angularVelocity;

		public Float3 angle;

		public DirectionState direction;

		public SixAxisSensorAttribute attributes;

		public override string ToString()
		{
			return $"{acceleration} {angularVelocity} {angle} {direction} {attributes} {deltaTimeNanoSeconds} {samplingNumber}";
		}

		public void GetQuaternion(ref float x, ref float y, ref float z, ref float w)
		{
			GetQuaternionImpl(ref this, ref x, ref y, ref z, ref w);
		}

		public void GetQuaternion(ref Float4 quaternion)
		{
			GetQuaternionImpl(ref this, ref quaternion.x, ref quaternion.y, ref quaternion.z, ref quaternion.w);
		}

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSixAxisSensorStateQuaternion")]
		private static extern void GetQuaternionImpl(ref SixAxisSensorState state, ref float pOutX, ref float pOutY, ref float pOutZ, ref float pOutW);
	}
}

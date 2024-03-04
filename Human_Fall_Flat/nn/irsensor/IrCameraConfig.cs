using System.Runtime.InteropServices;

namespace nn.irsensor
{
	public struct IrCameraConfig
	{
		public long exposureTimeNanoSeconds;

		public IrCameraLightTarget lightTarget;

		public int gain;

		[MarshalAs(UnmanagedType.U1)]
		public bool isNegativeImageUsed;

		public override string ToString()
		{
			return $"({exposureTimeNanoSeconds} {lightTarget} {gain} {isNegativeImageUsed})";
		}
	}
}

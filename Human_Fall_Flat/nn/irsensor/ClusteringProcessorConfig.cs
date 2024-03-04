using System.Runtime.InteropServices;

namespace nn.irsensor
{
	public struct ClusteringProcessorConfig
	{
		public IrCameraConfig irCameraConfig;

		public Rect windowOfInterest;

		public int objectPixelCountMin;

		public int objectPixelCountMax;

		public int objectIntensityMin;

		[MarshalAs(UnmanagedType.U1)]
		public bool isExternalLightFilterEnabled;

		public override string ToString()
		{
			return $"({irCameraConfig.ToString()} {windowOfInterest.ToString()} {objectPixelCountMin} {objectPixelCountMax} {objectIntensityMin} {isExternalLightFilterEnabled})";
		}
	}
}

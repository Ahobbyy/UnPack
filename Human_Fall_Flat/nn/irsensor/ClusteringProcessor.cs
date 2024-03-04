using System.Runtime.InteropServices;

namespace nn.irsensor
{
	public static class ClusteringProcessor
	{
		public const int StateCountMax = 5;

		public const int ObjectCountMax = 16;

		public const int ObjectPixelCountMax = 76800;

		public const int OutObjectPixelCountMax = 65535;

		public const long ExposureTimeMinNanoSeconds = 7000L;

		public const long ExposureTimeMaxNanoSeconds = 600000L;

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetClusteringProcessorDefaultConfig")]
		public static extern void GetDefaultConfig(ref ClusteringProcessorConfig pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_RunClusteringProcessor")]
		public static extern void Run(IrCameraHandle handle, ClusteringProcessorConfig config);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetClusteringProcessorState")]
		public static extern Result GetState(ref ClusteringProcessorState pOutValue, IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetClusteringProcessorStates")]
		public static extern Result GetStates([Out] ClusteringProcessorState[] pOutStates, ref int pOutCount, int countMax, IrCameraHandle handle);
	}
}

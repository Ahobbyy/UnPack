using System.Runtime.InteropServices;

namespace nn.irsensor
{
	public static class ImageProcessor
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_StopImageProcessor")]
		public static extern void Stop(IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_StopImageProcessor")]
		public static extern void StopAsync(IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetImageProcessorStatus")]
		public static extern ImageProcessorStatus GetStatus(IrCameraHandle handle);
	}
}

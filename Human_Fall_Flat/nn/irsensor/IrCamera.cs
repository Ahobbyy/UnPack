using System.Runtime.InteropServices;
using nn.hid;

namespace nn.irsensor
{
	public static class IrCamera
	{
		public const int IntensityMax = 255;

		public const int ImageWidth = 320;

		public const int ImageHeight = 240;

		public const int GainMin = 1;

		public const int GainMax = 16;

		public static readonly ErrorRange ResultIrsensorUnavailable = new ErrorRange(205, 110, 120);

		public static readonly ErrorRange ResultIrsensorUnconnected = new ErrorRange(205, 110, 111);

		public static readonly ErrorRange ResultIrsensorUnsupported = new ErrorRange(205, 111, 112);

		public static readonly ErrorRange ResultIrsensorNotReady = new ErrorRange(205, 120, 121);

		public static readonly ErrorRange ResultIrsensorDeviceError = new ErrorRange(205, 122, 140);

		public static readonly ErrorRange ResultIrsensorFirmwareCheckIncompleted = new ErrorRange(205, 150, 151);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetIrCameraHandle")]
		public static extern IrCameraHandle GetHandle(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_Initialize")]
		public static extern void Initialize(IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_Finalize")]
		public static extern void Finalize(IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_GetIrCameraStatus")]
		public static extern IrCameraStatus GetStatus(IrCameraHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_irsensor_CheckFirmwareUpdateNecessity")]
		public static extern Result CheckFirmwareUpdateNecessity(ref bool pOutIsUpdateNeeded, IrCameraHandle handle);
	}
}

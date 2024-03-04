using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class Vibration
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetVibrationDeviceHandles")]
		public static extern int GetDeviceHandles(VibrationDeviceHandle[] pOutValues, int count, NpadId npadId, NpadStyle npadStyle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetVibrationDeviceInfo")]
		public static extern void GetDeviceInfo(ref VibrationDeviceInfo pOutValue, VibrationDeviceHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_InitializeVibrationDevice")]
		public static extern void InitializeDevice(VibrationDeviceHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SendVibrationValue")]
		public static extern void SendValue(VibrationDeviceHandle handle, VibrationValue value);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetActualVibrationValue")]
		public static extern void GetActualValue(ref VibrationValue pOutValue, VibrationDeviceHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_IsVibrationPermitted")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsPermitted();
	}
}

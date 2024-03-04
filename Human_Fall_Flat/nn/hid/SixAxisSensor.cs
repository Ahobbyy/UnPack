using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class SixAxisSensor
	{
		public const int StateCountMax = 16;

		public const int HandleCountMax = 8;

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSixAxisSensorHandles")]
		public static extern int GetHandles(SixAxisSensorHandle[] pOutValues, int count, NpadId npadId, NpadStyle npadStyle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_StartSixAxisSensor")]
		public static extern void Start(SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_StopSixAxisSensor")]
		public static extern void Stop(SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_IsSixAxisSensorAtRest")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsRest(SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSixAxisSensorState")]
		public static extern void GetState(ref SixAxisSensorState pOutValue, SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSixAxisSensorStates")]
		public static extern int GetStates([Out] SixAxisSensorState[] pOutValues, int count, SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_IsSixAxisSensorFusionEnabled")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsFusionEnabled(SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_EnableSixAxisSensorFusion")]
		public static extern void EnableFusion(SixAxisSensorHandle handle, bool enable);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetSixAxisSensorFusionParameters")]
		public static extern void SetFusionParameters(SixAxisSensorHandle handle, float revisePower, float reviseRange);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSixAxisSensorFusionParameters")]
		public static extern void GetFusionParameters(ref float pOutRevisePower, ref float pOutReviseRange, SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_ResetSixAxisSensorFusionParameters")]
		public static extern void ResetFusionParameters(SixAxisSensorHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetGyroscopeZeroDriftMode")]
		public static extern void SetGyroscopeZeroDriftMode(SixAxisSensorHandle handle, GyroscopeZeroDriftMode mode);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetGyroscopeZeroDriftMode")]
		public static extern GyroscopeZeroDriftMode GetGyroscopeZeroDriftMode(SixAxisSensorHandle handle);
	}
}

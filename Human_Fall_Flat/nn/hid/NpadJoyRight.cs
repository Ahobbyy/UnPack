using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadJoyRight
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyRightState")]
		public static extern void GetState(ref NpadJoyRightState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyRightState")]
		public static extern void GetState(ref NpadState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyRightStates")]
		public static extern int GetStates([Out] NpadJoyRightState[] pOutValues, int count, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyRightStates")]
		public static extern int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId);
	}
}

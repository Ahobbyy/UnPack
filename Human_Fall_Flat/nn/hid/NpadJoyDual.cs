using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadJoyDual
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyDualState")]
		public static extern void GetState(ref NpadJoyDualState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyDualState")]
		public static extern void GetState(ref NpadState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyDualStates")]
		public static extern int GetStates([Out] NpadJoyDualState[] pOutValues, int count, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyDualStates")]
		public static extern int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId);
	}
}

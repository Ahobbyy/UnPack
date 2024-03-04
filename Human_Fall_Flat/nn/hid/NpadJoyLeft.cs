using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadJoyLeft
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyLeftState")]
		public static extern void GetState(ref NpadJoyLeftState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyLeftState")]
		public static extern void GetState(ref NpadState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyLeftStates")]
		public static extern int GetStates([Out] NpadJoyLeftState[] pOutValues, int count, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyLeftStates")]
		public static extern int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId);
	}
}

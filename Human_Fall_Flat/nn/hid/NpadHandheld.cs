using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadHandheld
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadHandheldState")]
		public static extern void GetState(ref NpadHandheldState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadHandheldState")]
		public static extern void GetState(ref NpadState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadHandheldStates")]
		public static extern int GetStates([Out] NpadHandheldState[] pOutValues, int count, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadHandheldStates")]
		public static extern int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId);
	}
}

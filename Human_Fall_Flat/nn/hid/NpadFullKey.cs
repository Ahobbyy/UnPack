using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadFullKey
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadFullKeyState")]
		public static extern void GetState(ref NpadFullKeyState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadFullKeyState")]
		public static extern void GetState(ref NpadState pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadFullKeyStates")]
		public static extern int GetStates([Out] NpadFullKeyState[] pOutValues, int count, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadFullKeyStates")]
		public static extern int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId);
	}
}

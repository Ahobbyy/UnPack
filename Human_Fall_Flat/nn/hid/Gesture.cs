using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class Gesture
	{
		public const int PointCountMax = 4;

		public const int StateCountMax = 16;

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_InitializeGesture")]
		public static extern void Initialize();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetGestureStates")]
		public static extern int GetStates([Out] GestureState[] pOutValues, int count);
	}
}

using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class TouchScreen
	{
		public const int TouchCountMax = 16;

		public const int StateCountMax = 16;

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_InitializeTouchScreen")]
		public static extern void Initialize();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState1")]
		public static extern void GetState(ref TouchScreenState1 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState2")]
		public static extern void GetState(ref TouchScreenState2 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState3")]
		public static extern void GetState(ref TouchScreenState3 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState4")]
		public static extern void GetState(ref TouchScreenState4 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState5")]
		public static extern void GetState(ref TouchScreenState5 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState6")]
		public static extern void GetState(ref TouchScreenState6 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState7")]
		public static extern void GetState(ref TouchScreenState7 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState8")]
		public static extern void GetState(ref TouchScreenState8 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState9")]
		public static extern void GetState(ref TouchScreenState9 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState10")]
		public static extern void GetState(ref TouchScreenState10 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState11")]
		public static extern void GetState(ref TouchScreenState11 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState12")]
		public static extern void GetState(ref TouchScreenState12 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState13")]
		public static extern void GetState(ref TouchScreenState13 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState14")]
		public static extern void GetState(ref TouchScreenState14 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState15")]
		public static extern void GetState(ref TouchScreenState15 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenState16")]
		public static extern void GetState(ref TouchScreenState16 pOutValue);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates1")]
		public static extern int GetStates([Out] TouchScreenState1[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates2")]
		public static extern int GetStates([Out] TouchScreenState2[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates3")]
		public static extern int GetStates([Out] TouchScreenState3[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates4")]
		public static extern int GetStates([Out] TouchScreenState4[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates5")]
		public static extern int GetStates([Out] TouchScreenState5[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates6")]
		public static extern int GetStates([Out] TouchScreenState6[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates7")]
		public static extern int GetStates([Out] TouchScreenState7[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates8")]
		public static extern int GetStates([Out] TouchScreenState8[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates9")]
		public static extern int GetStates([Out] TouchScreenState9[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates10")]
		public static extern int GetStates([Out] TouchScreenState10[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates11")]
		public static extern int GetStates([Out] TouchScreenState11[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates12")]
		public static extern int GetStates([Out] TouchScreenState12[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates13")]
		public static extern int GetStates([Out] TouchScreenState13[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates14")]
		public static extern int GetStates([Out] TouchScreenState14[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates15")]
		public static extern int GetStates([Out] TouchScreenState15[] pOutValues, int count);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetTouchScreenStates16")]
		public static extern int GetStates([Out] TouchScreenState16[] pOutValues, int count);
	}
}

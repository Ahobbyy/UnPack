using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class NpadJoy
	{
		public static readonly ErrorRange ResultDualConnected = new ErrorRange(202, 601, 602);

		public static readonly ErrorRange ResultSameJoyTypeConnected = new ErrorRange(202, 602, 603);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyAssignment")]
		public static extern NpadJoyAssignmentMode GetAssignment(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadJoyAssignmentModeSingle")]
		public static extern void SetAssignmentModeSingle(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadJoyAssignmentModeSingle2")]
		public static extern void SetAssignmentModeSingle(NpadId npadId, NpadJoyDeviceType deviceType);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadJoyAssignmentModeDual")]
		public static extern void SetAssignmentModeDual(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_MergeSingleJoyAsDualJoy")]
		public static extern Result MergeSingleAsDual(NpadId npadId1, NpadId npadId2);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SwapNpadAssignment")]
		public static extern void SwapAssignment(NpadId npadId1, NpadId npadId2);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadJoyHoldType")]
		public static extern void SetHoldType(NpadJoyHoldType holdType);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadJoyHoldType")]
		public static extern NpadJoyHoldType GetHoldType();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_StartLrAssignmentMode")]
		public static extern void StartLrAssignmentMode();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_StopLrAssignmentMode")]
		public static extern void StopLrAssignmentMode();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadHandheldActivationMode")]
		public static extern void SetHandheldActivationMode(NpadHandheldActivationMode activationMode);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadHandheldActivationMode")]
		public static extern NpadHandheldActivationMode GetHandheldActivationMode();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetNpadCommunicationMode")]
		public static extern void SetCommunicationMode(NpadCommunicationMode mode);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadCommunicationMode")]
		public static extern NpadCommunicationMode GetCommunicationMode();
	}
}

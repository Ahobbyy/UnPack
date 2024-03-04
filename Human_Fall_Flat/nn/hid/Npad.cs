using System.Runtime.InteropServices;

namespace nn.hid
{
	public static class Npad
	{
		public const int StateCountMax = 16;

		public static readonly ErrorRange ResultColorNotAvailable = new ErrorRange(202, 603, 604);

		public static readonly ErrorRange ResultControllerNotConnected = new ErrorRange(202, 604, 605);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_InitializeNpad")]
		public static extern void Initialize();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetSupportedNpadStyleSet")]
		public static extern void SetSupportedStyleSet(NpadStyle npadStyle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetSupportedNpadStyleSet")]
		public static extern NpadStyle GetSupportedStyleSet();

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_SetSupportedNpadIdType")]
		public static extern void SetSupportedIdType(NpadId[] npadIds, int count);

		public static void SetSupportedIdType(NpadId[] npadIds)
		{
			SetSupportedIdType(npadIds, npadIds.Length);
		}

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_BindNpadStyleSetUpdateEvent")]
		public static extern void BindStyleSetUpdateEvent(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_IsNpadStyleSetUpdated")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsStyleSetUpdated(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_DestroyNpadStyleSetUpdateEvent")]
		public static extern void DestroyStyleSetUpdateEvent(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadStyleSet")]
		public static extern NpadStyle GetStyleSet(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_DisconnectNpad")]
		public static extern void Disconnect(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetPlayerLedPattern")]
		public static extern byte GetPlayerLedPattern(NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadControllerColor")]
		public static extern Result GetControllerColor(ref NpadControllerColor pOutValue, NpadId npadId);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_hid_GetNpadControllerColor2")]
		public static extern Result GetControllerColor(ref NpadControllerColor pOutLeftColor, ref NpadControllerColor pOutRightColor, NpadId npadId);

		public static void GetState(ref NpadState pOutValue, NpadId npadId, NpadStyle npadStyle)
		{
			NpadButton buttons = pOutValue.buttons;
			switch (npadStyle)
			{
			case NpadStyle.FullKey:
				NpadFullKey.GetState(ref pOutValue, npadId);
				break;
			case NpadStyle.Handheld:
				NpadHandheld.GetState(ref pOutValue, npadId);
				break;
			case NpadStyle.JoyDual:
				NpadJoyDual.GetState(ref pOutValue, npadId);
				break;
			case NpadStyle.JoyLeft:
				NpadJoyLeft.GetState(ref pOutValue, npadId);
				break;
			case NpadStyle.JoyRight:
				NpadJoyRight.GetState(ref pOutValue, npadId);
				break;
			}
			pOutValue.preButtons = buttons;
		}

		public static int GetStates([Out] NpadStateArrayItem[] pOutValues, int count, NpadId npadId, NpadStyle npadStyle)
		{
			return npadStyle switch
			{
				NpadStyle.FullKey => NpadFullKey.GetStates(pOutValues, count, npadId), 
				NpadStyle.Handheld => NpadHandheld.GetStates(pOutValues, count, npadId), 
				NpadStyle.JoyDual => NpadJoyDual.GetStates(pOutValues, count, npadId), 
				NpadStyle.JoyLeft => NpadJoyLeft.GetStates(pOutValues, count, npadId), 
				NpadStyle.JoyRight => NpadJoyRight.GetStates(pOutValues, count, npadId), 
				_ => 0, 
			};
		}
	}
}

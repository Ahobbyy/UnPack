using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct ControllerFirmwareUpdateArg
	{
		[MarshalAs(UnmanagedType.U1)]
		public bool enableForceUpdate;

		private byte _padding0;

		private byte _padding1;

		private byte _padding2;

		public void SetDefault()
		{
			enableForceUpdate = false;
		}
	}
}

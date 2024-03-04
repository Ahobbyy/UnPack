using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState16
	{
		public const int TouchCount = 16;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[16];
		}
	}
}

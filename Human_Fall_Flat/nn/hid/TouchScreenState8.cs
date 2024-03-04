using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState8
	{
		public const int TouchCount = 8;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[8];
		}
	}
}

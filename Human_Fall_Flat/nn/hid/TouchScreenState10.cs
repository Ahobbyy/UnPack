using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState10
	{
		public const int TouchCount = 10;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[10];
		}
	}
}

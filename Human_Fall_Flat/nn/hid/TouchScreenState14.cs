using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState14
	{
		public const int TouchCount = 14;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[14];
		}
	}
}

using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState13
	{
		public const int TouchCount = 13;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 13)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[13];
		}
	}
}

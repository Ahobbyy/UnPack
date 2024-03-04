using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState6
	{
		public const int TouchCount = 6;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[6];
		}
	}
}

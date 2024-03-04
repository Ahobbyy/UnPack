using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState4
	{
		public const int TouchCount = 4;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[4];
		}
	}
}

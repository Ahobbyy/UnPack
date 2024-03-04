using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState15
	{
		public const int TouchCount = 15;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 15)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[15];
		}
	}
}

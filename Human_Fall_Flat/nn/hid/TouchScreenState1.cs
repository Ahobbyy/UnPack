using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState1
	{
		public const int TouchCount = 1;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 1)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[1];
		}
	}
}

using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState9
	{
		public const int TouchCount = 9;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 9)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[9];
		}
	}
}

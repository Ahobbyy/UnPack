using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState2
	{
		public const int TouchCount = 2;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[2];
		}
	}
}

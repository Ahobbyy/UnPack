using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState11
	{
		public const int TouchCount = 11;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[11];
		}
	}
}

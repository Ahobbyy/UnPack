using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState12
	{
		public const int TouchCount = 12;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[12];
		}
	}
}

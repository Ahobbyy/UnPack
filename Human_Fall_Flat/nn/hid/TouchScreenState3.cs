using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct TouchScreenState3
	{
		public const int TouchCount = 3;

		public long samplingNumber;

		public int count;

		private int _reserved;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public TouchState[] touches;

		public void SetDefault()
		{
			touches = new TouchState[3];
		}
	}
}

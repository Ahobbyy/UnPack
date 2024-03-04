using System;
using System.Runtime.InteropServices;

namespace nn.hid
{
	public struct VibrationFileParserContext
	{
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public IntPtr[] _storage;
	}
}

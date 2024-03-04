using System;

namespace nn.hid
{
	[Flags]
	public enum NpadStyle
	{
		None = 0x0,
		FullKey = 0x1,
		Handheld = 0x2,
		JoyDual = 0x4,
		JoyLeft = 0x8,
		JoyRight = 0x10,
		Invalid = 0x20
	}
}

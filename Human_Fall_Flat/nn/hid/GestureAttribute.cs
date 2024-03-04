using System;

namespace nn.hid
{
	[Flags]
	public enum GestureAttribute
	{
		IsNewTouch = 0x10,
		IsDoubleTap = 0x100
	}
}

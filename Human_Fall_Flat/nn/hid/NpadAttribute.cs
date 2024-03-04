using System;

namespace nn.hid
{
	[Flags]
	public enum NpadAttribute
	{
		IsConnected = 0x1,
		IsWired = 0x2,
		IsLeftConnected = 0x4,
		IsLeftWired = 0x8,
		IsRightConnected = 0x10,
		IsRightWired = 0x20
	}
}

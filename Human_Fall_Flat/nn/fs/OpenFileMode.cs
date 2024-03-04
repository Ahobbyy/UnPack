using System;

namespace nn.fs
{
	[Flags]
	public enum OpenFileMode
	{
		Read = 0x1,
		Write = 0x2,
		AllowAppend = 0x4
	}
}

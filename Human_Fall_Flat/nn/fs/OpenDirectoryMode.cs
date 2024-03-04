using System;

namespace nn.fs
{
	[Flags]
	public enum OpenDirectoryMode
	{
		Directory = 0x1,
		File = 0x2,
		All = 0x3
	}
}

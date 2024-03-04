using System.Runtime.InteropServices;

namespace nn.account
{
	public struct Nickname
	{
		public const int NameBytesMax = 32;

		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 33)]
		public string name;

		public override string ToString()
		{
			return name;
		}
	}
}

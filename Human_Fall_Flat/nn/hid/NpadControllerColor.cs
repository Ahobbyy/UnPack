using nn.util;

namespace nn.hid
{
	public struct NpadControllerColor
	{
		public Color4u8 main;

		public Color4u8 sub;

		public override string ToString()
		{
			return $"main{main} sub{sub}";
		}
	}
}

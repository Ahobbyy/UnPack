namespace nn.util
{
	public struct Color4u8
	{
		public byte r;

		public byte g;

		public byte b;

		public byte a;

		public void Set(byte r, byte g, byte b, byte a)
		{
			this.r = r;
			this.g = g;
			this.b = b;
			this.a = a;
		}

		public override string ToString()
		{
			return $"({r} {g} {b} {a})";
		}
	}
}

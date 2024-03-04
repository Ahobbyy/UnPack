namespace nn.irsensor
{
	public struct Rect
	{
		public short x;

		public short y;

		public short width;

		public short height;

		public Rect(short x, short y, short width, short height)
		{
			this.x = x;
			this.y = y;
			this.width = width;
			this.height = height;
		}

		public override string ToString()
		{
			return $"(x:{x} y:{y} w:{width} h:{height})";
		}
	}
}

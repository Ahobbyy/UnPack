namespace nn.hid
{
	public struct GesturePoint
	{
		public int x;

		public int y;

		public override string ToString()
		{
			return $"({x} {y})";
		}
	}
}

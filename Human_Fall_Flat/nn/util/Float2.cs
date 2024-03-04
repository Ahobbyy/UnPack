namespace nn.util
{
	public struct Float2
	{
		public float x;

		public float y;

		public Float2(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public void Set(float x, float y)
		{
			this.x = x;
			this.y = y;
		}

		public override string ToString()
		{
			return $"({x} {y})";
		}
	}
}

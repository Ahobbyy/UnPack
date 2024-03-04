namespace nn.util
{
	public struct Float4
	{
		public float x;

		public float y;

		public float z;

		public float w;

		public Float4(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public void Set(float x, float y, float z, float w)
		{
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		public override string ToString()
		{
			return $"({x} {y} {z} {w})";
		}
	}
}

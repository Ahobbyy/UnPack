namespace nn.util
{
	public struct Float3
	{
		public float x;

		public float y;

		public float z;

		public Float3(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public void Set(float x, float y, float z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public override string ToString()
		{
			return $"({x} {y} {z})";
		}
	}
}

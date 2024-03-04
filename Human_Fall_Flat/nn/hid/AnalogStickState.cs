namespace nn.hid
{
	public struct AnalogStickState
	{
		public const int Max = 32767;

		public int x;

		public int y;

		public float fx => (float)x / 32767f;

		public float fy => (float)y / 32767f;

		public override string ToString()
		{
			return string.Format("({0,6} {1,6})", x, y);
		}
	}
}

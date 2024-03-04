namespace nn.hid
{
	public struct TouchState
	{
		public long deltaTimeNanoSeconds;

		public TouchAttribute attributes;

		public int fingerId;

		public int x;

		public int y;

		public int diameterX;

		public int diameterY;

		public int rotationAngle;

		private int _reserved;

		public override string ToString()
		{
			return $"fId:{fingerId} pos:({x} {y}) dia:({diameterX} {diameterY}) rotA:{rotationAngle} attr:{attributes} delta:{deltaTimeNanoSeconds}";
		}
	}
}

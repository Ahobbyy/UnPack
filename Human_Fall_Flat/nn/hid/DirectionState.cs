using nn.util;

namespace nn.hid
{
	public struct DirectionState
	{
		public Float3 x;

		public Float3 y;

		public Float3 z;

		public override string ToString()
		{
			return $"X{x} Y{y} Z{z}";
		}
	}
}

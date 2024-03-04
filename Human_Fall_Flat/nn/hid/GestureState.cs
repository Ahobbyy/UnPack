using System.Runtime.InteropServices;
using nn.util;

namespace nn.hid
{
	public struct GestureState
	{
		public long eventNumber;

		public long contextNumber;

		public int _type;

		public int _direction;

		public int x;

		public int y;

		public int deltaX;

		public int deltaY;

		public Float2 velocity;

		public GestureAttribute attributes;

		public float scale;

		public float rotationAngle;

		public int pointCount;

		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public GesturePoint[] points;

		public GestureType type => (GestureType)_type;

		public GestureDirection direction => (GestureDirection)_direction;

		public bool isDoubleTap => (attributes & GestureAttribute.IsDoubleTap) == GestureAttribute.IsDoubleTap;

		public void SetDefault()
		{
			points = new GesturePoint[4];
		}

		public override string ToString()
		{
			return $"event:{eventNumber} con:{contextNumber} type:{type} dir:{direction} pos:({x} {y}) delta:({deltaX} {deltaY}) vel:{velocity} attr:{attributes} scale:{scale} rotA:{rotationAngle} count:{pointCount} p0:{points[0]} p1:{points[1]} p2:{points[2]} p3:{points[3]}";
		}
	}
}

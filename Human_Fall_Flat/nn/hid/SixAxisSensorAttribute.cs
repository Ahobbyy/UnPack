using System;

namespace nn.hid
{
	[Flags]
	public enum SixAxisSensorAttribute
	{
		IsConnected = 0x1,
		IsInterpolated = 0x2
	}
}

namespace nn.hid
{
	public struct VibrationDeviceInfo
	{
		public VibrationDeviceType deviceType;

		public VibrationDevicePosition position;

		public override string ToString()
		{
			return $"{deviceType} {position}";
		}
	}
}

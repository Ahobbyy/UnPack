namespace nn.hid
{
	public struct DebugPadState
	{
		public long samplingNumber;

		public DebugPadAttribute attributes;

		public DebugPadButton buttons;

		public AnalogStickState analogStickR;

		public AnalogStickState analogStickL;

		public override string ToString()
		{
			return $"L{analogStickL} R{analogStickR} [{buttons}] {attributes} {samplingNumber}";
		}
	}
}

namespace nn.hid
{
	public struct NpadJoyLeftState
	{
		public long samplingNumber;

		public NpadButton buttons;

		public AnalogStickState analogStickL;

		public AnalogStickState analogStickR;

		public NpadAttribute attributes;

		public override string ToString()
		{
			return $"L{analogStickL} R{analogStickR} [{buttons}] {attributes} {samplingNumber}";
		}
	}
}

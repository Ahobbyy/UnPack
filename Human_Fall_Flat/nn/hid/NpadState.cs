namespace nn.hid
{
	public struct NpadState
	{
		public long samplingNumber;

		public NpadButton buttons;

		public AnalogStickState analogStickL;

		public AnalogStickState analogStickR;

		public NpadAttribute attributes;

		public NpadButton preButtons;

		public bool GetButton(NpadButton button)
		{
			return (buttons & button) != 0;
		}

		public bool GetButtonDown(NpadButton button)
		{
			if ((preButtons & button) == (NpadButton)0L)
			{
				return (buttons & button) != 0;
			}
			return false;
		}

		public bool GetButtonUp(NpadButton button)
		{
			if ((preButtons & button) != (NpadButton)0L)
			{
				return (buttons & button) == 0;
			}
			return false;
		}

		public override string ToString()
		{
			return $"L{analogStickL} R{analogStickR} [{buttons}] {attributes} {samplingNumber}";
		}
	}
}

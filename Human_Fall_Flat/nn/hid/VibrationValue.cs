namespace nn.hid
{
	public struct VibrationValue
	{
		public const int FrequencyLowDefault = 160;

		public const int FrequencyHighDefault = 320;

		public float amplitudeLow;

		public float frequencyLow;

		public float amplitudeHigh;

		public float frequencyHigh;

		public static VibrationValue Make()
		{
			return new VibrationValue(0f, 160f, 0f, 320f);
		}

		public static VibrationValue Make(float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			return new VibrationValue(amplitudeLow, frequencyLow, amplitudeHigh, frequencyHigh);
		}

		public VibrationValue(float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			this.amplitudeLow = amplitudeLow;
			this.frequencyLow = frequencyLow;
			this.amplitudeHigh = amplitudeHigh;
			this.frequencyHigh = frequencyHigh;
		}

		public void Set(float amplitudeLow, float frequencyLow, float amplitudeHigh, float frequencyHigh)
		{
			this.amplitudeLow = amplitudeLow;
			this.frequencyLow = frequencyLow;
			this.amplitudeHigh = amplitudeHigh;
			this.frequencyHigh = frequencyHigh;
		}

		public void Clear()
		{
			amplitudeLow = 0f;
			frequencyLow = 160f;
			amplitudeHigh = 0f;
			frequencyHigh = 320f;
		}

		public override string ToString()
		{
			return $"Low({amplitudeLow} {frequencyLow}Hz) High({amplitudeHigh} {frequencyHigh}Hz)";
		}
	}
}

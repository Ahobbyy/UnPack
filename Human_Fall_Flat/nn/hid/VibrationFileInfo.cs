namespace nn.hid
{
	public struct VibrationFileInfo
	{
		public uint metaDataSize;

		public ushort formatId;

		public ushort samplingRate;

		public uint dataSize;

		public int sampleLength;

		public int isLoop;

		public uint loopStartPosition;

		public uint loopEndPosition;

		public uint loopInterval;

		public override string ToString()
		{
			return $"({metaDataSize} {formatId}) SamplingRate:{samplingRate} DataSize:{dataSize} SampleLength:{sampleLength} Loop:{isLoop}({loopStartPosition} - {loopEndPosition}, {loopInterval})";
		}
	}
}

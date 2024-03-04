namespace nn.hid
{
	public struct ControllerSupportResultInfo
	{
		public byte playerCount;

		public NpadId selectedId;

		private byte _padding0;

		private byte _padding1;

		private byte _padding2;

		public override string ToString()
		{
			return $"{playerCount} {selectedId}";
		}
	}
}

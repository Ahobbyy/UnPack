namespace nn.err
{
	public struct ErrorCode
	{
		public uint category;

		public uint number;

		public override string ToString()
		{
			return string.Format("(0x{0,0:X8} 0x{1,0:X8})", category, number);
		}

		public bool IsValid()
		{
			return true;
		}

		public static ErrorCode GetInvalidErrorCode()
		{
			return default(ErrorCode);
		}
	}
}

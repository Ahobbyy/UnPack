namespace nn.fs
{
	public static class Host
	{
		public static readonly ErrorRange ResultSaveDataHostFileSystemCorrupted = new ErrorRange(2, 4441, 4460);

		public static readonly ErrorRange ResultSaveDataHostEntryCorrupted = new ErrorRange(2, 4442, 4443);

		public static readonly ErrorRange ResultSaveDataHostFileDataCorrupted = new ErrorRange(2, 4443, 4444);

		public static readonly ErrorRange ResultSaveDataHostFileCorrupted = new ErrorRange(2, 4444, 4445);

		public static readonly ErrorRange ResultInvalidSaveDataHostHandle = new ErrorRange(2, 4445, 4446);

		public static readonly ErrorRange ResultHostFileSystemCorrupted = new ErrorRange(2, 4701, 4720);

		public static readonly ErrorRange ResultHostEntryCorrupted = new ErrorRange(2, 4702, 4703);

		public static readonly ErrorRange ResultHostFileDataCorrupted = new ErrorRange(2, 4703, 4704);

		public static readonly ErrorRange ResultHostFileCorrupted = new ErrorRange(2, 4704, 4705);

		public static readonly ErrorRange ResultInvalidHostHandle = new ErrorRange(2, 4705, 4706);

		public static Result MountHost(string name, string rootPath)
		{
			return default(Result);
		}

		public static Result MountHostRoot()
		{
			return default(Result);
		}

		public static void UnMountHostRoot()
		{
		}
	}
}

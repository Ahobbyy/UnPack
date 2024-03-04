namespace nn.err
{
	public static class Error
	{
		public static void Show(Result result)
		{
		}

		public static void Show(ErrorCode errorCode)
		{
		}

		public static void Show(Result result, bool suspendUnityThreads)
		{
			Show(result);
		}

		public static void Show(ErrorCode errorCode, bool suspendUnityThreads)
		{
			Show(errorCode);
		}

		public static void ShowUnacceptableApplicationVersion()
		{
		}

		public static void ShowUnacceptableApplicationVersion(bool suspendUnityThreads)
		{
			ShowUnacceptableApplicationVersion();
		}
	}
}

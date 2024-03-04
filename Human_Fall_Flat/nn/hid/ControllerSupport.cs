namespace nn.hid
{
	public static class ControllerSupport
	{
		public const int ExplainTextMaxLength = 32;

		public const int Utf8ByteSize = 4;

		public const int ExplainTextMaxBufferSize = 129;

		public static readonly ErrorRange ResultCanceled = new ErrorRange(202, 3101, 3102);

		public static readonly ErrorRange ResultNotSupportedNpadStyle = new ErrorRange(202, 3102, 3103);

		public static Result Show(ControllerSupportArg showControllerSupportArg)
		{
			return default(Result);
		}

		public static Result Show(ref ControllerSupportResultInfo pOutValue, ControllerSupportArg showControllerSupportArg)
		{
			return default(Result);
		}

		public static void SetExplainText(ref ControllerSupportArg pOutControllerSupportArg, string pStr, NpadId npadId)
		{
		}

		public static Result Show(ControllerSupportArg showControllerSupportArg, bool suspendUnityThreads)
		{
			return Show(showControllerSupportArg);
		}

		public static Result Show(ref ControllerSupportResultInfo pOutValue, ControllerSupportArg showControllerSupportArg, bool suspendUnityThreads)
		{
			return Show(ref pOutValue, showControllerSupportArg);
		}
	}
}

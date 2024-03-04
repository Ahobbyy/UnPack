namespace nn.account
{
	public static class NetworkServiceAccount
	{
		public const int IdTokenLengthMax = 3072;

		public static readonly ErrorRange ResultNetworkServiceAccountUnavailable = new ErrorRange(124, 200, 270);

		public static readonly ErrorRange ResultTokenCacheUnavailable = new ErrorRange(124, 430, 500);

		public static readonly ErrorRange ResultNetworkCommunicationError = new ErrorRange(124, 3000, 8192);

		public static readonly ErrorRange ResultSslService = new ErrorRange(123, 0, 5000);

		public static Result EnsureAvailable(UserHandle handle)
		{
			return default(Result);
		}

		public static Result IsAvailable(ref bool pOut, UserHandle handle)
		{
			pOut = false;
			return default(Result);
		}

		public static Result GetId(ref NetworkServiceAccountId pOutId, UserHandle handle)
		{
			return default(Result);
		}

		public static Result EnsurIdTokenCacheAsync(AsyncContext pOutContext, UserHandle handle)
		{
			return default(Result);
		}

		public static Result LoadIdTokenCache(ref int pOutActualSize, byte[] buffer, UserHandle handle)
		{
			pOutActualSize = 0;
			return default(Result);
		}
	}
}

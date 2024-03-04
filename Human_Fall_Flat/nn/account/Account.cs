namespace nn.account
{
	public static class Account
	{
		public const int UserCountMax = 8;

		public const int ProfileImageBytesMax = 131072;

		public const int SaveDataThumbnailImageBytesMax = 147456;

		public static readonly ErrorRange ResultCancelled = new ErrorRange(124, 0, 1);

		public static readonly ErrorRange ResultCancelledByUser = new ErrorRange(124, 1, 2);

		public static readonly ErrorRange ResultUserNotExist = new ErrorRange(124, 100, 101);

		public static Result GetUserCount(ref int pOutCount)
		{
			pOutCount = 0;
			return default(Result);
		}

		public static Result GetUserExistence(ref bool pOutExistence, Uid user)
		{
			pOutExistence = false;
			return default(Result);
		}

		public static Result ListAllUsers(ref int pOutActualLength, Uid[] outUsers)
		{
			pOutActualLength = 0;
			return default(Result);
		}

		public static Result ListOpenUsers(ref int pOutActualLength, Uid[] outUsers)
		{
			pOutActualLength = 0;
			return default(Result);
		}

		public static Result GetLastOpenedUser(ref Uid pOutUser)
		{
			return default(Result);
		}

		public static Result GetNickname(ref Nickname pOut, Uid user)
		{
			return default(Result);
		}

		public static Result LoadProfileImage(ref long pOutActualSize, byte[] outImage, Uid user)
		{
			return default(Result);
		}

		public static void Initialize()
		{
		}

		public static Result OpenUser(ref UserHandle pOutHandle, Uid user)
		{
			return default(Result);
		}

		public static Result OpenPreselectedUser(ref UserHandle pOutHandle)
		{
			return default(Result);
		}

		public static void CloseUser(UserHandle handle)
		{
		}

		public static Result GetUserId(ref Uid pOut, UserHandle handle)
		{
			return default(Result);
		}

		public static Result StoreSaveDataThumbnailImage(Uid user, byte[] imageBuffer)
		{
			return default(Result);
		}

		public static Result DeleteSaveDataThumbnailImage(Uid user)
		{
			return default(Result);
		}

		public static Result ShowUserSelector(ref Uid pOut, UserSelectionSettings arg)
		{
			return default(Result);
		}

		public static Result ShowUserSelector(ref Uid pOut)
		{
			return default(Result);
		}

		public static Result ShowUserCreator()
		{
			return default(Result);
		}

		public static Result ShowUserSelector(ref Uid pOut, UserSelectionSettings arg, bool suspendUnityThreads)
		{
			return ShowUserSelector(ref pOut, arg);
		}

		public static Result ShowUserSelector(ref Uid pOut, bool suspendUnityThreads)
		{
			return ShowUserSelector(ref pOut);
		}

		public static Result ShowUserCreator(bool suspendUnityThreads)
		{
			return ShowUserCreator();
		}
	}
}

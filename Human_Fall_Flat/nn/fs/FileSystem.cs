using System.Runtime.InteropServices;

namespace nn.fs
{
	public static class FileSystem
	{
		public const int MountNameLengthMax = 15;

		public static readonly ErrorRange ResultHandledByAllProcess = new ErrorRange(2, 0, 1000);

		public static readonly ErrorRange ResultPathNotFound = new ErrorRange(2, 1, 2);

		public static readonly ErrorRange ResultPathAlreadyExists = new ErrorRange(2, 2, 3);

		public static readonly ErrorRange ResultTargetLocked = new ErrorRange(2, 7, 8);

		public static readonly ErrorRange ResultDirectoryNotEmpty = new ErrorRange(2, 8, 9);

		public static readonly ErrorRange ResultDirectoryStatusChanged = new ErrorRange(2, 13, 14);

		public static readonly ErrorRange ResultUsableSpaceNotEnough = new ErrorRange(2, 30, 46);

		public static readonly ErrorRange ResultUnsupportedSdkVersion = new ErrorRange(2, 50, 51);

		public static readonly ErrorRange ResultMountNameAlreadyExists = new ErrorRange(2, 60, 61);

		public static readonly ErrorRange ResultTargetNotFound = new ErrorRange(2, 1002, 1003);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_GetEntryType")]
		public static extern Result GetEntryType(ref EntryType outValue, string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_Unmount")]
		public static extern void Unmount(string name);
	}
}

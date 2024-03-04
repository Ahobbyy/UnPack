using System.Runtime.InteropServices;

namespace nn.fs
{
	public static class Directory
	{
		public const int EntryNameLengthMax = 768;

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_ReadDirectory")]
		public static extern Result Read(ref long outValue, [Out] DirectoryEntry[] entryBuffer, DirectoryHandle handle, long entryBufferCount);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_GetDirectoryEntryCount")]
		public static extern Result GetEntryCount(ref long outValue, DirectoryHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CloseDirectory")]
		public static extern void Close(DirectoryHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CreateDirectory")]
		public static extern Result Create(string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_DeleteDirectory")]
		public static extern Result Delete(string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_DeleteDirectoryRecursively")]
		public static extern Result DeleteRecursively(string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CleanDirectoryRecursively")]
		public static extern Result CleanRecursively(string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_RenameDirectory")]
		public static extern Result Rename(string currentPath, string newPath);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_OpenDirectory")]
		public static extern Result Open(ref DirectoryHandle outValue, string path, OpenDirectoryMode mode);
	}
}

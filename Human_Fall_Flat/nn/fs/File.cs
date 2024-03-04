using System.Runtime.InteropServices;

namespace nn.fs
{
	public static class File
	{
		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_ReadFile0")]
		public static extern Result Read(FileHandle handle, long offset, byte[] buffer, long size, ReadOption option);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_ReadFile1")]
		public static extern Result Read(FileHandle handle, long offset, byte[] buffer, long size);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_ReadFile2")]
		public static extern Result Read(ref long outValue, FileHandle handle, long offset, byte[] buffer, long size, ReadOption option);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_ReadFile3")]
		public static extern Result Read(ref long outValue, FileHandle handle, long offset, byte[] buffer, long size);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_WriteFile")]
		public static extern Result Write(FileHandle handle, long offset, byte[] buffer, long size, WriteOption option);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_FlushFile")]
		public static extern Result Flush(FileHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_SetFileSize")]
		public static extern Result SetSize(FileHandle handle, long size);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_GetFileSize")]
		public static extern Result GetSize(ref long outValue, FileHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_GetFileOpenMode")]
		public static extern OpenFileMode GetOpenMode(FileHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CloseFile")]
		public static extern void Close(FileHandle handle);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CreateFile")]
		public static extern Result Create(string path, long size);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_DeleteFile")]
		public static extern Result Delete(string path);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_RenameFile")]
		public static extern Result Rename(string currentPath, string newPath);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_OpenFile")]
		public static extern Result Open(ref FileHandle outValue, string path, OpenFileMode mode);
	}
}

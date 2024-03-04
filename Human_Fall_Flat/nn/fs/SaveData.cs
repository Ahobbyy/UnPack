using System.Runtime.InteropServices;
using nn.account;

namespace nn.fs
{
	public static class SaveData
	{
		public static readonly ErrorRange ResultUsableSpaceNotEnoughForSaveData = new ErrorRange(2, 31, 32);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_EnsureSaveData")]
		public static extern Result Ensure(Uid user);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_MountSaveData")]
		public static extern Result Mount(string name, Uid user);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_MountSaveDataReadOnly")]
		public static extern Result MountSaveDataReadOnly(string name, ulong applicationId, Uid user);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_IsSaveDataExisting0")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsExisting(Uid user);

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_IsSaveDataExisting1")]
		[return: MarshalAs(UnmanagedType.U1)]
		public static extern bool IsExisting(ulong applicationId, Uid user);

		public static void SetRootPath(string rootPath)
		{
		}

		public static Result MountForDebug(string name)
		{
			return default(Result);
		}

		[DllImport("NintendoSDKPlugin", CallingConvention = CallingConvention.Cdecl, EntryPoint = "nn_fs_CommitSaveData")]
		public static extern Result Commit(string name);
	}
}

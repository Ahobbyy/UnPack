using System.Runtime.InteropServices;

namespace nn.fs
{
	public struct DirectoryEntry
	{
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 769)]
		public string name;

		private byte _reserved0;

		private byte _reserved1;

		private byte _reserved2;

		private sbyte _entryType;

		private byte _reserved3;

		private byte _reserved4;

		private byte _reserved5;

		private long fileSize;

		public EntryType entryType
		{
			get
			{
				return (EntryType)_entryType;
			}
			set
			{
				_entryType = (sbyte)value;
			}
		}

		public override string ToString()
		{
			if (entryType == EntryType.Directory)
			{
				return $"{name}/";
			}
			return $"{name} : {fileSize}";
		}
	}
}

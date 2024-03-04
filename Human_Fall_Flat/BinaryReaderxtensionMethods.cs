using System;
using System.IO;
using System.Text;

internal static class BinaryReaderxtensionMethods
{
	public static string ReadNullTerminatedString(this BinaryReader stream)
	{
		string text = "";
		char c;
		while ((c = stream.ReadChar()) != 0)
		{
			text += c;
		}
		return text;
	}

	public static string ReadLengthPrefixString(this BinaryReader stream)
	{
		int count = stream.ReadInt32();
		byte[] bytes = stream.ReadBytes(count);
		return Encoding.UTF8.GetString(bytes, 0, count);
	}

	public static void Align(this BinaryReader stream, long basePos)
	{
		while (((stream.BaseStream.Position - basePos) & 3) != 0L)
		{
			stream.ReadByte();
		}
	}

	public static uint ReadUInt32BigEndian(this BinaryReader stream)
	{
		byte[] bytes = BitConverter.GetBytes(stream.ReadUInt32());
		Array.Reverse(bytes);
		return BitConverter.ToUInt32(bytes, 0);
	}

	public static ulong ReadUInt64BigEndian(this BinaryReader stream)
	{
		byte[] bytes = BitConverter.GetBytes(stream.ReadUInt64());
		Array.Reverse(bytes);
		return BitConverter.ToUInt64(bytes, 0);
	}
}

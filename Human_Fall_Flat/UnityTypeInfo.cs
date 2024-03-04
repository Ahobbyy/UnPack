using System.IO;

internal class UnityTypeInfo
{
	public ushort m_version;

	public byte m_depth;

	public bool m_isArray;

	public int m_byteSize;

	public int m_index;

	public uint m_metaFlag;

	public string m_type;

	public void Read(BinaryReader reader, long stringPos)
	{
		m_version = reader.ReadUInt16();
		m_depth = reader.ReadByte();
		m_isArray = reader.ReadBoolean();
		uint offset = reader.ReadUInt32();
		reader.ReadUInt32();
		m_byteSize = reader.ReadInt32();
		m_index = reader.ReadInt32();
		m_metaFlag = reader.ReadUInt32();
		m_type = ReadString(reader, stringPos, offset);
	}

	private static string ReadString(BinaryReader stream, long stringPos, uint offset)
	{
		if ((offset & 0x80000000u) == 0)
		{
			long position = stream.BaseStream.Position;
			stream.BaseStream.Position = stringPos + offset;
			string result = stream.ReadNullTerminatedString();
			stream.BaseStream.Position = position;
			return result;
		}
		return (offset & 0x7FFFFFFF) switch
		{
			263u => "MonoBehaviour", 
			277u => "MonoScript", 
			_ => "UnhandledType", 
		};
	}
}

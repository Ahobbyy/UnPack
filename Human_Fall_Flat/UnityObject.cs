using System.IO;

internal class UnityObject
{
	public long m_pathID;

	public int m_dataOffset;

	public int m_dataSize;

	public int m_typeIndex;

	public void Read(BinaryReader reader, long filePos)
	{
		while (((reader.BaseStream.Position - filePos) & 3) != 0L)
		{
			reader.ReadByte();
		}
		m_pathID = reader.ReadInt64();
		m_dataOffset = reader.ReadInt32();
		m_dataSize = reader.ReadInt32();
		m_typeIndex = reader.ReadInt32();
	}
}

using System.Collections.Generic;
using System.IO;

internal class UnitySerializedFile
{
	public ulong m_fileOffset;

	public ulong m_fileSize;

	public uint m_fileFlags;

	public string m_fileName;

	public uint m_headerSize;

	public uint m_length;

	public uint m_generation;

	public uint m_dataOffset;

	public long m_filePos;

	public RTTIEntry[] m_entries;

	public Dictionary<long, UnityObject> m_objects;

	public void ReadHeader(BinaryReader reader)
	{
		m_fileOffset = reader.ReadUInt64BigEndian();
		m_fileSize = reader.ReadUInt64BigEndian();
		m_fileFlags = reader.ReadUInt32BigEndian();
		m_fileName = reader.ReadNullTerminatedString();
	}

	public void Read(long baseFilePosition, BinaryReader reader)
	{
		m_filePos = baseFilePosition + (long)m_fileOffset;
		reader.BaseStream.Position = m_filePos;
		m_headerSize = reader.ReadUInt32BigEndian();
		m_length = reader.ReadUInt32BigEndian();
		m_generation = reader.ReadUInt32BigEndian();
		m_dataOffset = reader.ReadUInt32BigEndian();
	}

	public void ReadObjects(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		m_objects = new Dictionary<long, UnityObject>();
		for (int i = 0; i < num; i++)
		{
			UnityObject unityObject = new UnityObject();
			unityObject.Read(reader, m_filePos);
			m_objects.Add(unityObject.m_pathID, unityObject);
		}
	}

	public void ReadRTTI(BinaryReader reader)
	{
		reader.BaseStream.Position = m_filePos + 20;
		reader.ReadNullTerminatedString();
		reader.ReadUInt32();
		reader.ReadByte();
		uint num = reader.ReadUInt32();
		m_entries = new RTTIEntry[num];
		for (int i = 0; i < num; i++)
		{
			RTTIEntry rTTIEntry = new RTTIEntry();
			rTTIEntry.Read(reader);
			m_entries[i] = rTTIEntry;
		}
	}
}

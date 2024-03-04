using System.IO;

internal class RTTIEntry
{
	public uint m_classID;

	public ushort m_scriptID;

	public void Read(BinaryReader reader)
	{
		m_classID = reader.ReadUInt32();
		reader.ReadByte();
		m_scriptID = reader.ReadUInt16();
		if (m_classID == 114)
		{
			reader.ReadUInt64();
			reader.ReadUInt64();
		}
		reader.ReadUInt64();
		reader.ReadUInt64();
		int num = reader.ReadInt32();
		int num2 = reader.ReadInt32();
		long stringPos = reader.BaseStream.Position + num * 24;
		UnityTypeInfo[] array = new UnityTypeInfo[num];
		for (int i = 0; i < num; i++)
		{
			UnityTypeInfo unityTypeInfo = new UnityTypeInfo();
			unityTypeInfo.Read(reader, stringPos);
			array[i] = unityTypeInfo;
		}
		reader.BaseStream.Position += num2;
	}
}

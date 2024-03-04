using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class AssetBundleParser : MonoBehaviour
{
	public static void ParseAssetBundle(string filename, string outfile)
	{
		if (!File.Exists(filename))
		{
			Debug.LogError((object)("File does not exist: " + filename));
			return;
		}
		Debug.Log((object)("parsing: " + filename));
		byte[] array = File.ReadAllBytes(filename);
		BinaryReader binaryReader = new BinaryReader(File.Open(filename, FileMode.Open));
		binaryReader.ReadNullTerminatedString();
		binaryReader.ReadUInt32BigEndian();
		binaryReader.ReadNullTerminatedString();
		binaryReader.ReadNullTerminatedString();
		binaryReader.ReadUInt64BigEndian();
		uint num = binaryReader.ReadUInt32BigEndian();
		binaryReader.ReadUInt32BigEndian();
		binaryReader.ReadUInt32BigEndian();
		List<UnitySerializedFile> list = new List<UnitySerializedFile>();
		UnitySerializedFile item = new UnitySerializedFile();
		list.Add(item);
		binaryReader.BaseStream.Position += num;
		long position = binaryReader.BaseStream.Position;
		foreach (UnitySerializedFile item2 in list)
		{
			item2.Read(position, binaryReader);
		}
		foreach (UnitySerializedFile item3 in list)
		{
			item3.ReadRTTI(binaryReader);
			item3.ReadObjects(binaryReader);
		}
		UnitySerializedFile unitySerializedFile = list[0];
		byte[] bytes = Encoding.ASCII.GetBytes("Assembly-CSharp.dll");
		byte[] bytes2 = Encoding.ASCII.GetBytes("HumanAPI.dll");
		string text = "HFF-Code-CSharp.dll";
		string[] array2 = new string[6] { "Checkpoint", "FallTrigger", "HumanBase", "Level", "LevelObject", "LevelPassTrigger" };
		foreach (KeyValuePair<long, UnityObject> @object in unitySerializedFile.m_objects)
		{
			if (unitySerializedFile.m_entries[@object.Value.m_typeIndex].m_classID != 115)
			{
				continue;
			}
			binaryReader.BaseStream.Position = unitySerializedFile.m_filePos + unitySerializedFile.m_dataOffset + @object.Value.m_dataOffset;
			binaryReader.ReadLengthPrefixString();
			binaryReader.Align(unitySerializedFile.m_filePos);
			binaryReader.ReadInt32();
			binaryReader.ReadUInt32();
			binaryReader.ReadUInt32();
			binaryReader.ReadUInt32();
			binaryReader.ReadUInt32();
			string value = binaryReader.ReadLengthPrefixString();
			binaryReader.Align(unitySerializedFile.m_filePos);
			string strA = binaryReader.ReadLengthPrefixString();
			binaryReader.Align(unitySerializedFile.m_filePos);
			binaryReader.ReadInt32();
			int num2 = (int)binaryReader.BaseStream.Position;
			binaryReader.BaseStream.Position -= 4L;
			string strA2 = binaryReader.ReadLengthPrefixString();
			binaryReader.Align(unitySerializedFile.m_filePos);
			if (string.Compare(strA2, text, ignoreCase: true) == 0)
			{
				Array.Clear(array, num2, text.Length);
				if (string.Compare(strA, "HumanAPI") == 0 && Array.IndexOf(array2, value) >= 0)
				{
					Array.Copy(bytes2, 0, array, num2, bytes2.Length);
					array[num2 - 4] = 12;
				}
				else
				{
					Array.Copy(bytes, 0, array, num2, bytes.Length);
				}
			}
		}
		binaryReader.Close();
		File.WriteAllBytes(outfile, array);
	}

	public AssetBundleParser()
		: this()
	{
	}
}

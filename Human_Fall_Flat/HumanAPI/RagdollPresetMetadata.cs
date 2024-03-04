using System;
using System.IO;
using System.Security.Cryptography;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class RagdollPresetMetadata : WorkshopItemMetadata
	{
		public RagdollPresetPartMetadata main;

		public RagdollPresetPartMetadata head;

		public RagdollPresetPartMetadata upperBody;

		public RagdollPresetPartMetadata lowerBody;

		private byte[] md5;

		private byte[] serialized;

		public RagdollPresetPartMetadata GetPart(WorkshopItemType part)
		{
			return part switch
			{
				WorkshopItemType.ModelFull => main, 
				WorkshopItemType.ModelHead => head, 
				WorkshopItemType.ModelUpperBody => upperBody, 
				WorkshopItemType.ModelLowerBody => lowerBody, 
				_ => throw new Exception("Invalid part"), 
			};
		}

		public void SetPart(WorkshopItemType part, RagdollPresetPartMetadata meta)
		{
			switch (part)
			{
			case WorkshopItemType.ModelFull:
				main = meta;
				break;
			case WorkshopItemType.ModelHead:
				head = meta;
				break;
			case WorkshopItemType.ModelUpperBody:
				upperBody = meta;
				break;
			case WorkshopItemType.ModelLowerBody:
				lowerBody = meta;
				break;
			default:
				throw new Exception("Invalid part");
			}
		}

		public void SetColor(WorkshopItemType part, int channel, Color color)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			RagdollPresetPartMetadata part2 = GetPart(part);
			switch (channel)
			{
			case 1:
				part2.color1 = HexConverter.ColorToHex(Color32.op_Implicit(color));
				break;
			case 2:
				part2.color2 = HexConverter.ColorToHex(Color32.op_Implicit(color));
				break;
			case 3:
				part2.color3 = HexConverter.ColorToHex(Color32.op_Implicit(color));
				break;
			}
		}

		public Color GetColor(WorkshopItemType part, int channel)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			RagdollPresetPartMetadata part2 = GetPart(part);
			return (Color)(channel switch
			{
				1 => Color32.op_Implicit(HexConverter.HexToColor(part2.color1, default(Color))), 
				2 => Color32.op_Implicit(HexConverter.HexToColor(part2.color2, default(Color))), 
				3 => Color32.op_Implicit(HexConverter.HexToColor(part2.color3, default(Color))), 
				_ => default(Color), 
			});
		}

		public void ClearColors()
		{
			if (main != null)
			{
				main.color1 = (main.color2 = (main.color3 = null));
			}
			if (head != null)
			{
				head.color1 = (head.color2 = (head.color3 = null));
			}
			if (upperBody != null)
			{
				upperBody.color1 = (upperBody.color2 = (upperBody.color3 = null));
			}
			if (lowerBody != null)
			{
				lowerBody.color1 = (lowerBody.color2 = (lowerBody.color3 = null));
			}
		}

		public byte[] GetSerialized()
		{
			if (serialized == null)
			{
				serialized = Serialize();
			}
			return serialized;
		}

		public byte[] GetCRC()
		{
			if (md5 == null)
			{
				using MD5 mD = MD5.Create();
				byte[] buffer = GetSerialized();
				md5 = mD.ComputeHash(buffer);
			}
			return md5;
		}

		public static string FormatCRC(byte[] crc)
		{
			if (crc == null)
			{
				return "(null)";
			}
			string text = "";
			for (int i = 0; i < crc.Length; i++)
			{
				text += crc[i].ToString("X2");
			}
			return text;
		}

		private void SerializePart(WorkshopItemType part, NetStream stream)
		{
			byte[] array = null;
			RagdollPresetPartMetadata part2 = GetPart(part);
			if (part2 != null)
			{
				array = part2.bytes;
				if (array == null && !string.IsNullOrEmpty(folder))
				{
					array = FileTools.ReadAllBytes(FileTools.Combine(folder, part.ToString() + ".png"));
				}
			}
			if (array != null)
			{
				stream.Write(v: true);
				stream.WriteArray(array, 32);
			}
			else
			{
				stream.Write(v: false);
			}
		}

		public byte[] Serialize()
		{
			NetStream netStream = NetStream.AllocStream();
			try
			{
				string text = JsonUtility.ToJson((object)this);
				netStream.Write(text);
				SerializePart(WorkshopItemType.ModelFull, netStream);
				SerializePart(WorkshopItemType.ModelHead, netStream);
				SerializePart(WorkshopItemType.ModelUpperBody, netStream);
				SerializePart(WorkshopItemType.ModelLowerBody, netStream);
				return netStream.ToArray();
			}
			finally
			{
				if (netStream != null)
				{
					netStream = netStream.Release();
				}
			}
		}

		private void DeserializePart(WorkshopItemType part, NetStream stream)
		{
			if (stream.ReadBool())
			{
				byte[] array = (GetPart(part).bytes = stream.ReadArray(32));
			}
		}

		public static RagdollPresetMetadata Deserialize(byte[] data)
		{
			NetStream netStream = NetStream.AllocStream(data);
			try
			{
				string text = netStream.ReadString();
				RagdollPresetMetadata ragdollPresetMetadata = JsonUtility.FromJson<RagdollPresetMetadata>(text);
				if (ragdollPresetMetadata == null)
				{
					Debug.LogErrorFormat("Unable to deserialize metadata {0}", new object[1] { text });
					return null;
				}
				ragdollPresetMetadata.folder = null;
				ragdollPresetMetadata.serialized = data;
				ragdollPresetMetadata.DeserializePart(WorkshopItemType.ModelFull, netStream);
				ragdollPresetMetadata.DeserializePart(WorkshopItemType.ModelHead, netStream);
				ragdollPresetMetadata.DeserializePart(WorkshopItemType.ModelUpperBody, netStream);
				ragdollPresetMetadata.DeserializePart(WorkshopItemType.ModelLowerBody, netStream);
				return ragdollPresetMetadata;
			}
			finally
			{
				if (netStream != null)
				{
					netStream = netStream.Release();
				}
			}
		}

		public void SaveNetSkin(uint localCoopIndex, string id)
		{
			FileTools.WriteAllBytes(Path.Combine(Application.get_persistentDataPath(), "net/" + id.ToLower() + localCoopIndex), GetSerialized());
		}

		public static RagdollPresetMetadata LoadNetSkin(uint localCoopIndex, string id)
		{
			byte[] array = FileTools.ReadAllBytes(Path.Combine(Application.get_persistentDataPath(), "net/" + id.ToLower() + localCoopIndex));
			if (array == null)
			{
				return null;
			}
			return Deserialize(array);
		}

		public bool CheckCRC(byte[] skinCRC)
		{
			byte[] cRC = GetCRC();
			if (cRC.Length != skinCRC.Length)
			{
				return false;
			}
			for (int i = 0; i < cRC.Length; i++)
			{
				if (cRC[i] != skinCRC[i])
				{
					return false;
				}
			}
			return true;
		}
	}
}

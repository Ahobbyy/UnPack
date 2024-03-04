using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public static class RuntimeParams
{
	public class SimpleExprEvaluator
	{
		protected HashSet<string> m_Defines;

		protected StringBuilder m_sb = new StringBuilder();

		public SimpleExprEvaluator(BuildTargetGroup group, bool editor)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			string[] collection = PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';');
			m_Defines = new HashSet<string>(collection);
			m_Defines.Add("UNITY_" + ((object)(BuildTargetGroup)(ref group)).ToString().ToUpperInvariant());
			if (editor)
			{
				m_Defines.Add("UNITY_EDITOR");
			}
			m_Defines.Add("true");
			m_Defines.Remove("false");
		}

		public bool Evaluate(string arg, bool exception = true)
		{
			int num = RawEvaluate(arg);
			if (num < 0)
			{
				string text = "Bad expression in RuntimeParams: \"" + arg + "\"";
				if (exception)
				{
					throw new ApplicationException(text);
				}
				Debug.LogError((object)text);
			}
			return num > 0;
		}

		public int RawEvaluate(string arg)
		{
			StringBuilder sb = m_sb;
			sb.Length = 0;
			int length = arg.Length;
			int i = 0;
			List<int> list = new List<int>();
			while (i < length)
			{
				char c = arg[i++];
				if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
				{
					switch (c)
					{
					case '_':
						break;
					case '0':
					case '1':
						sb.Append(c);
						continue;
					case '!':
					case '&':
					case '*':
					case '+':
					case '|':
						if (i < length && arg[i] == c)
						{
							i++;
							if (c == '!')
							{
								continue;
							}
						}
						switch (c)
						{
						case '*':
							c = '&';
							break;
						case '+':
							c = '|';
							break;
						}
						sb.Append(c);
						continue;
					case '(':
						list.Add(sb.Length);
						continue;
					case ')':
					{
						if (list.Count == 0)
						{
							return -1;
						}
						int begin = list[list.Count - 1];
						list.RemoveAt(list.Count - 1);
						if (Parse(sb, begin) >= 0)
						{
							continue;
						}
						return -1;
					}
					default:
						return -1;
					case ' ':
						continue;
					}
				}
				int num = i - 1;
				for (; i < length; i++)
				{
					c = arg[i];
					if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z'))
					{
						switch (c)
						{
						case '0':
						case '1':
						case '2':
						case '3':
						case '4':
						case '5':
						case '6':
						case '7':
						case '8':
						case '9':
						case '_':
							continue;
						}
						break;
					}
				}
				string item = arg.Substring(num, i - num);
				sb.Append(m_Defines.Contains(item) ? '1' : '0');
			}
			if (list.Count != 0)
			{
				return -1;
			}
			return Parse(sb, 0);
		}

		private static int Parse(StringBuilder sb, int begin)
		{
			int num = begin;
			int num2 = sb.Length - 1;
			int length = begin;
			while (num < num2)
			{
				char c = sb[num++];
				if (c == '!')
				{
					c = sb[num++];
					if (c != '0' && c != '1')
					{
						return -1;
					}
					c = ((c == '0') ? '1' : '0');
				}
				sb[length++] = c;
			}
			while (num < sb.Length)
			{
				sb[length++] = sb[num++];
			}
			sb.Length = length;
			for (int i = 0; i < 2; i++)
			{
				char c2 = ((i != 0) ? '|' : '&');
				num = begin + 1;
				num2 = sb.Length - 1;
				length = begin + 1;
				while (num < num2)
				{
					char c3 = sb[num++];
					if (c3 == c2)
					{
						char c4 = sb[length - 1];
						char c5 = sb[num++];
						if (c4 != '0' && c4 != '1')
						{
							return -1;
						}
						if (c5 != '0' && c5 != '1')
						{
							return -1;
						}
						bool flag = c4 != '0';
						bool flag2 = c5 != '0';
						c3 = ((c2 != '&') ? ((flag || flag2) ? '1' : '0') : ((flag && flag2) ? '1' : '0'));
						length--;
					}
					sb[length++] = c3;
				}
				while (num < sb.Length)
				{
					sb[length++] = sb[num++];
				}
				sb.Length = length;
			}
			if (sb.Length != begin + 1)
			{
				return -1;
			}
			return sb[begin] switch
			{
				'0' => 0, 
				'1' => 1, 
				_ => -1, 
			};
		}
	}

	private static Dictionary<string, string> m_Params;

	public const string ResourceName = "Curve/AssetHashes";

	public const string EditorFilename = "Assets/Resources/Curve/AssetHashes.bytes";

	private static void InternalInit()
	{
		m_Params = new Dictionary<string, string>();
		if (Application.get_isEditor() && EditorApplication.get_isPlaying())
		{
			ImportXml_InEditor((BuildTargetGroup)1, playInEditor: true);
			Load();
		}
	}

	public static void Init()
	{
		if (m_Params == null)
		{
			InternalInit();
		}
	}

	public static bool CheckForKey(string key)
	{
		if (m_Params == null)
		{
			InternalInit();
		}
		return m_Params.ContainsKey(key);
	}

	public static string GetString(string key, string defaultValue = null)
	{
		if (m_Params == null)
		{
			InternalInit();
		}
		if (!m_Params.TryGetValue(key, out var value))
		{
			return defaultValue;
		}
		return value;
	}

	public static int GetInt(string key, int defaultValue = 0)
	{
		string @string = GetString(key);
		if (@string == null)
		{
			return defaultValue;
		}
		if (!int.TryParse(@string, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static uint GetUInt(string key, uint defaultValue = 0u)
	{
		string @string = GetString(key);
		if (@string == null)
		{
			return defaultValue;
		}
		if (!uint.TryParse(@string, out var result))
		{
			return defaultValue;
		}
		return result;
	}

	public static bool GetBool(string key, bool defaultValue = false)
	{
		return GetInt(key, defaultValue ? 1 : 0) != 0;
	}

	public static void Set(string key, string value)
	{
		if (m_Params == null)
		{
			InternalInit();
		}
		m_Params[key] = value;
	}

	public static void Set(string key, int value)
	{
		Set(key, value.ToString());
	}

	public static void Set(string key, uint value)
	{
		Set(key, value.ToString());
	}

	public static void Set(string key, bool value)
	{
		Set(key, value ? "1" : "0");
	}

	public static void DeleteKey(string key)
	{
		if (m_Params != null)
		{
			m_Params.Remove(key);
		}
	}

	public static void Clear()
	{
		if (m_Params != null)
		{
			m_Params.Clear();
		}
	}

	public static void Load()
	{
		try
		{
			TextAsset val = Resources.Load<TextAsset>("Curve/AssetHashes");
			if ((Object)(object)val != (Object)null)
			{
				byte[] bytes = val.get_bytes();
				byte[] array = new byte[bytes.Length];
				int i = 0;
				for (int num = array.Length; i < num; i++)
				{
					array[i] = (byte)((uint)(bytes[i] ^ (205 + i * 47)) & 0xFFu);
				}
				using (BinaryReader binaryReader = new BinaryReader(new MemoryStream(array)))
				{
					int num2 = binaryReader.ReadInt32();
					while (num2-- > 0)
					{
						string key = binaryReader.ReadString();
						string value = binaryReader.ReadString();
						m_Params[key] = value;
					}
				}
				Resources.UnloadAsset((Object)(object)val);
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
		Dump();
	}

	public static void Dump()
	{
		string text = "RuntimeParams:";
		if (m_Params != null)
		{
			foreach (KeyValuePair<string, string> param in m_Params)
			{
				text = $"{text}\n  {param.Key}:\"{param.Value}\"";
			}
		}
		Debug.Log((object)text);
	}

	public static void Save()
	{
		Init();
		MemoryStream memoryStream = new MemoryStream(4096);
		using (BinaryWriter binaryWriter = new BinaryWriter(memoryStream))
		{
			binaryWriter.Write(m_Params.Count);
			foreach (KeyValuePair<string, string> param in m_Params)
			{
				binaryWriter.Write(param.Key);
				binaryWriter.Write(param.Value);
			}
			binaryWriter.Close();
		}
		byte[] array = memoryStream.ToArray();
		byte[] array2 = new byte[(array.Length + 31) & -32];
		int i = 0;
		for (int num = array2.Length; i < num; i++)
		{
			array2[i] = (byte)((uint)(-i * 111) & 0xFFu);
		}
		int j = 0;
		for (int num2 = array.Length; j < num2; j++)
		{
			array2[j] = (byte)((uint)(array[j] ^ (205 + j * 47)) & 0xFFu);
		}
		Directory.CreateDirectory(Path.GetDirectoryName("Assets/Resources/Curve/AssetHashes.bytes"));
		EditorNonVCWrite("Assets/Resources/Curve/AssetHashes.bytes", array2);
	}

	public static Dictionary<string, string> CopyData()
	{
		Init();
		return new Dictionary<string, string>(m_Params);
	}

	public static void DumpVCAssetState(Asset vcasset)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		if (vcasset == null)
		{
			Debug.Log((object)"No vc asset");
			return;
		}
		Debug.LogFormat("vcasset fullName:{0}\npath:{6}\nstate:{8}\nisFolder:{1}\nisInCurrentProject:{2}\nisMeta:{3}\nlocked:{4}\nname:{5}\nreadOnly:{7}", new object[9]
		{
			vcasset.get_fullName(),
			vcasset.get_isFolder(),
			vcasset.get_isInCurrentProject(),
			vcasset.get_isMeta(),
			vcasset.get_locked(),
			vcasset.get_name(),
			vcasset.get_path(),
			vcasset.get_readOnly(),
			vcasset.get_state()
		});
	}

	public static void GetRidOfFromPerforce(string filename, bool keepLocal)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Asset assetByPath = Provider.GetAssetByPath(filename);
		if (assetByPath != null && assetByPath.get_isInCurrentProject())
		{
			RevertMode val = (RevertMode)(keepLocal ? 2 : 0);
			if (Provider.RevertIsValid(assetByPath, val))
			{
				Provider.Revert(assetByPath, val).Wait();
			}
			if (!keepLocal && Provider.get_isActive())
			{
				Provider.Delete(filename).Wait();
			}
		}
	}

	public static void EditorNonVCWrite(string filename, byte[] data)
	{
		try
		{
			GetRidOfFromPerforce(filename, keepLocal: false);
			string text = "Assets/Resources/Curve/AssetHashes.bytes.meta";
			string contents = "fileFormatVersion: 2\nguid: fc804f43799834c468ad37a4aafaa276\nTextScriptImporter:\n  externalObjects: {}\n  userData: \n  assetBundleName: \n  assetBundleVariant: \n";
			File.WriteAllBytes("Assets/Resources/Curve/AssetHashes.bytes", data);
			File.WriteAllText(text, contents);
			DateTime lastWriteTimeUtc = File.GetLastWriteTimeUtc("Assets/Resources/Curve/AssetHashes.bytes");
			DateTime lastWriteTimeUtc2 = File.GetLastWriteTimeUtc(text);
			AssetDatabase.ImportAsset("Assets/Resources/Curve/AssetHashes.bytes", (ImportAssetOptions)8);
			Provider.Status(filename).Wait();
			GetRidOfFromPerforce(filename, keepLocal: false);
			if (!File.Exists("Assets/Resources/Curve/AssetHashes.bytes"))
			{
				Debug.LogFormat("Rewriting {0}", new object[1] { "Assets/Resources/Curve/AssetHashes.bytes" });
				File.WriteAllBytes("Assets/Resources/Curve/AssetHashes.bytes", data);
				File.SetLastWriteTimeUtc("Assets/Resources/Curve/AssetHashes.bytes", lastWriteTimeUtc);
			}
			else
			{
				Debug.LogFormat("NOT Rewriting {0}", new object[1] { "Assets/Resources/Curve/AssetHashes.bytes" });
			}
			if (!File.Exists(text))
			{
				Debug.LogFormat("Rewriting {0}", new object[1] { text });
				File.WriteAllText(text, contents);
				File.SetLastWriteTimeUtc(text, lastWriteTimeUtc2);
			}
			else
			{
				Debug.LogFormat("NOT Rewriting {0}", new object[1] { text });
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	public static void DeleteResource()
	{
		try
		{
			AssetDatabase.DeleteAsset("Assets/Resources/Curve/AssetHashes.bytes");
			GetRidOfFromPerforce("Assets/Resources/Curve/AssetHashes.bytes", keepLocal: false);
			if (File.Exists("Assets/Resources/Curve/AssetHashes.bytes"))
			{
				File.Delete("Assets/Resources/Curve/AssetHashes.bytes");
			}
			string path = "Assets/Resources/Curve/AssetHashes.bytes.meta";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
		}
		catch (Exception)
		{
		}
	}

	public static void WriteAsXml(string path)
	{
		try
		{
			Init();
			XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
			xmlWriterSettings.Indent = true;
			using StreamWriter streamWriter = File.CreateText(path);
			using (XmlWriter xmlWriter = XmlWriter.Create(streamWriter, xmlWriterSettings))
			{
				xmlWriter.WriteStartElement("RuntimeParams");
				foreach (KeyValuePair<string, string> param in m_Params)
				{
					xmlWriter.WriteStartElement("Param");
					xmlWriter.WriteAttributeString("key", param.Key);
					xmlWriter.WriteAttributeString("value", param.Value);
					xmlWriter.WriteEndElement();
				}
				xmlWriter.WriteEndElement();
				xmlWriter.Close();
			}
			streamWriter.Close();
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	public static void ImportXml(string path, SimpleExprEvaluator exprEval, bool doDeleteKey, bool dontOverwrite)
	{
		try
		{
			if (dontOverwrite)
			{
				doDeleteKey = false;
			}
			Init();
			XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
			xmlReaderSettings.IgnoreComments = true;
			xmlReaderSettings.IgnoreProcessingInstructions = true;
			xmlReaderSettings.IgnoreWhitespace = true;
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			HashSet<string> hashSet = new HashSet<string>();
			using (XmlReader xmlReader = XmlReader.Create(path, xmlReaderSettings))
			{
				while (xmlReader.Read())
				{
					if (xmlReader.IsStartElement("Param"))
					{
						string attribute = xmlReader.GetAttribute("key");
						string attribute2 = xmlReader.GetAttribute("value");
						bool flag = true;
						string attribute3 = xmlReader.GetAttribute("condition");
						if (!string.IsNullOrEmpty(attribute3))
						{
							flag = exprEval.Evaluate(attribute3, exception: false);
						}
						hashSet.Add(attribute);
						if (flag)
						{
							dictionary[attribute] = attribute2;
						}
					}
				}
			}
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (!dontOverwrite || !CheckForKey(item.Key))
				{
					Set(item.Key, item.Value);
				}
			}
			if (!doDeleteKey)
			{
				return;
			}
			foreach (string item2 in hashSet)
			{
				if (!dictionary.ContainsKey(item2))
				{
					DeleteKey(item2);
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogException(ex);
		}
	}

	public static void ImportXml_InEditor(BuildTargetGroup group, bool playInEditor)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Invalid comparison between Unknown and I4
		string text = "ProjectSettings/BaseRuntimeParams.xml";
		if (File.Exists(text))
		{
			SimpleExprEvaluator exprEval = new SimpleExprEvaluator(group, playInEditor);
			ImportXml(text, exprEval, doDeleteKey: false, !playInEditor);
		}
		else if ((int)group == 19)
		{
			Debug.LogErrorFormat("File {0} was not found - This contains necessary data", new object[1] { text });
		}
	}
}

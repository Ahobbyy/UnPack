using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

namespace UnityEditor.Rendering.PostProcessing
{
	internal sealed class CubeLutAssetImporter : AssetPostprocessor
	{
		private static List<string> s_Excluded = new List<string> { "Linear to sRGB r1", "Linear to Unity Log r1", "sRGB to Linear r1", "sRGB to Unity Log r1", "Unity Log to Linear r1", "Unity Log to sRGB r1" };

		private static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
		{
			foreach (string path in imported)
			{
				string extension = Path.GetExtension(path);
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
				if (!string.IsNullOrEmpty(extension) && !s_Excluded.Contains(fileNameWithoutExtension))
				{
					extension = extension.ToLowerInvariant();
					if (extension.Equals(".cube"))
					{
						ImportCubeLut(path);
					}
				}
			}
		}

		private static void ImportCubeLut(string path)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0243: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Expected O, but got Unknown
			string path2 = path.Substring(7);
			path2 = Path.Combine(Application.get_dataPath(), path2);
			string[] array = File.ReadAllLines(path2);
			int num = 0;
			int result = -1;
			int num2 = -1;
			List<Color> list = new List<Color>();
			Color domain = Color.get_black();
			Color domain2 = Color.get_white();
			while (true)
			{
				if (num >= array.Length)
				{
					if (list.Count != num2)
					{
						Debug.LogError((object)"Premature end of file");
					}
					break;
				}
				string text = FilterLine(array[num]);
				if (!string.IsNullOrEmpty(text) && !text.StartsWith("TITLE"))
				{
					if (text.StartsWith("LUT_3D_SIZE"))
					{
						if (!int.TryParse(text.Substring(11).TrimStart(), out result))
						{
							Debug.LogError((object)("Invalid data on line " + num));
							break;
						}
						if (result < 2 || result > 256)
						{
							Debug.LogError((object)"LUT size out of range");
							break;
						}
						num2 = result * result * result;
					}
					else if (text.StartsWith("DOMAIN_MIN"))
					{
						if (!ParseDomain(num, text, ref domain))
						{
							break;
						}
					}
					else if (text.StartsWith("DOMAIN_MAX"))
					{
						if (!ParseDomain(num, text, ref domain2))
						{
							break;
						}
					}
					else
					{
						string[] array2 = text.Split();
						if (array2.Length != 3)
						{
							Debug.LogError((object)("Invalid data on line " + num));
							break;
						}
						Color black = Color.get_black();
						for (int i = 0; i < 3; i++)
						{
							if (!float.TryParse(array2[i], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result2))
							{
								Debug.LogError((object)("Invalid data on line " + num));
								break;
							}
							((Color)(ref black)).set_Item(i, result2);
						}
						list.Add(black);
					}
				}
				num++;
			}
			if (num2 != list.Count)
			{
				Debug.LogError((object)("Wrong table size - Expected " + num2 + " elements, got " + list.Count));
				return;
			}
			string text2 = Path.ChangeExtension(path, ".asset");
			Texture3D val = AssetDatabase.LoadAssetAtPath<Texture3D>(text2);
			if ((Object)(object)val != (Object)null)
			{
				val.SetPixels(list.ToArray(), 0);
				val.Apply();
			}
			else
			{
				Texture3D val2 = new Texture3D(result, result, result, (TextureFormat)17, false);
				((Texture)val2).set_anisoLevel(0);
				((Texture)val2).set_filterMode((FilterMode)1);
				((Texture)val2).set_wrapMode((TextureWrapMode)1);
				val = val2;
				val.SetPixels(list.ToArray(), 0);
				val.Apply();
				AssetDatabase.CreateAsset((Object)(object)val, text2);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		private static string FilterLine(string line)
		{
			StringBuilder stringBuilder = new StringBuilder();
			line = line.TrimStart().TrimEnd();
			int length = line.Length;
			for (int i = 0; i < length; i++)
			{
				char c = line[i];
				if (c == '#')
				{
					break;
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		private static bool ParseDomain(int i, string line, ref Color domain)
		{
			string[] array = line.Substring(10).TrimStart().Split();
			if (array.Length != 3)
			{
				Debug.LogError((object)("Invalid data on line " + i));
				return false;
			}
			for (int j = 0; j < 3; j++)
			{
				if (!float.TryParse(array[j], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var result))
				{
					Debug.LogError((object)("Invalid data on line " + i));
					return false;
				}
				((Color)(ref domain)).set_Item(j, result);
			}
			return true;
		}

		public CubeLutAssetImporter()
			: this()
		{
		}
	}
}

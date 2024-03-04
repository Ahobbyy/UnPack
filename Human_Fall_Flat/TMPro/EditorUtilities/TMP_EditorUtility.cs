using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class TMP_EditorUtility
	{
		[SerializeField]
		private static string m_PackagePath;

		[SerializeField]
		private static string m_PackageFullPath;

		private static string folderPath = "Not Found";

		private static EditorWindow Gameview;

		private static bool isInitialized = false;

		public static string packageRelativePath
		{
			get
			{
				if (string.IsNullOrEmpty(m_PackagePath))
				{
					m_PackagePath = GetPackageRelativePath();
				}
				return m_PackagePath;
			}
		}

		public static string packageFullPath
		{
			get
			{
				if (string.IsNullOrEmpty(m_PackageFullPath))
				{
					m_PackageFullPath = GetPackageFullPath();
				}
				return m_PackageFullPath;
			}
		}

		private static void GetGameview()
		{
			Gameview = EditorWindow.GetWindow(typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView"));
		}

		public static void RepaintAll()
		{
			if (!isInitialized)
			{
				GetGameview();
				isInitialized = true;
			}
			SceneView.RepaintAll();
			Gameview.Repaint();
		}

		public static T CreateAsset<T>(string name) where T : ScriptableObject
		{
			string assetPath = AssetDatabase.GetAssetPath(Selection.get_activeObject());
			assetPath = ((assetPath.Length == 0) ? ("Assets/" + name + ".asset") : ((!Directory.Exists(assetPath)) ? (Path.GetDirectoryName(assetPath) + "/" + name + ".asset") : (assetPath + "/" + name + ".asset")));
			T val = ScriptableObject.CreateInstance<T>();
			AssetDatabase.CreateAsset((Object)(object)val, AssetDatabase.GenerateUniqueAssetPath(assetPath));
			EditorUtility.FocusProjectWindow();
			Selection.set_activeObject((Object)(object)val);
			return val;
		}

		public static Material[] FindMaterialReferences(TMP_FontAsset fontAsset)
		{
			List<Material> list = new List<Material>();
			Material material = fontAsset.material;
			list.Add(material);
			string[] array = AssetDatabase.FindAssets("t:Material " + ((Object)fontAsset).get_name().Split(' ')[0]);
			for (int i = 0; i < array.Length; i++)
			{
				Material val = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath(array[i]));
				if (val.HasProperty(ShaderUtilities.ID_MainTex) && (Object)(object)val.GetTexture(ShaderUtilities.ID_MainTex) != (Object)null && (Object)(object)material.GetTexture(ShaderUtilities.ID_MainTex) != (Object)null && ((Object)val.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() == ((Object)material.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() && !list.Contains(val))
				{
					list.Add(val);
				}
			}
			return list.ToArray();
		}

		public static TMP_FontAsset FindMatchingFontAsset(Material mat)
		{
			if ((Object)(object)mat.GetTexture(ShaderUtilities.ID_MainTex) == (Object)null)
			{
				return null;
			}
			string[] dependencies = AssetDatabase.GetDependencies(AssetDatabase.GetAssetPath((Object)(object)mat), false);
			for (int i = 0; i < dependencies.Length; i++)
			{
				TMP_FontAsset tMP_FontAsset = AssetDatabase.LoadAssetAtPath<TMP_FontAsset>(dependencies[i]);
				if ((Object)(object)tMP_FontAsset != (Object)null)
				{
					return tMP_FontAsset;
				}
			}
			return null;
		}

		private static string GetPackageRelativePath()
		{
			string fullPath = Path.GetFullPath("Packages/com.unity.textmeshpro");
			if (Directory.Exists(fullPath))
			{
				return "Packages/com.unity.textmeshpro";
			}
			fullPath = Path.GetFullPath("Assets/..");
			if (Directory.Exists(fullPath))
			{
				if (Directory.Exists(fullPath + "/Assets/Packages/com.unity.TextMeshPro/Editor Resources"))
				{
					return "Assets/Packages/com.unity.TextMeshPro";
				}
				if (Directory.Exists(fullPath + "/Assets/TextMesh Pro/Editor Resources"))
				{
					return "Assets/TextMesh Pro";
				}
				fullPath = ValidateLocation(Directory.GetDirectories(fullPath, "TextMesh Pro", SearchOption.AllDirectories), fullPath);
				if (fullPath != null)
				{
					return fullPath;
				}
			}
			return null;
		}

		private static string GetPackageFullPath()
		{
			string fullPath = Path.GetFullPath("Packages/com.unity.textmeshpro");
			if (Directory.Exists(fullPath))
			{
				return fullPath;
			}
			fullPath = Path.GetFullPath("Assets/..");
			if (Directory.Exists(fullPath))
			{
				if (Directory.Exists(fullPath + "/Assets/Packages/com.unity.TextMeshPro/Editor Resources"))
				{
					return fullPath + "/Assets/Packages/com.unity.TextMeshPro";
				}
				if (Directory.Exists(fullPath + "/Assets/TextMesh Pro/Editor Resources"))
				{
					return fullPath + "/Assets/TextMesh Pro";
				}
				string text = ValidateLocation(Directory.GetDirectories(fullPath, "TextMesh Pro", SearchOption.AllDirectories), fullPath);
				if (text != null)
				{
					return fullPath + text;
				}
			}
			return null;
		}

		private static string ValidateLocation(string[] paths, string projectPath)
		{
			for (int i = 0; i < paths.Length; i++)
			{
				if (Directory.Exists(paths[i] + "/Editor Resources"))
				{
					folderPath = paths[i].Replace(projectPath + "\\", "");
					return folderPath;
				}
			}
			return null;
		}

		public static string GetDecimalCharacterSequence(int[] characterSet)
		{
			string text = string.Empty;
			int num = characterSet.Length;
			int num2 = characterSet[0];
			int num3 = num2;
			for (int i = 1; i < num; i++)
			{
				if (characterSet[i - 1] + 1 == characterSet[i])
				{
					num3 = characterSet[i];
					continue;
				}
				text = ((num2 != num3) ? (text + num2 + "-" + num3 + ",") : (text + num2 + ","));
				num2 = (num3 = characterSet[i]);
			}
			if (num2 == num3)
			{
				return text + num2;
			}
			return text + num2 + "-" + num3;
		}

		public static string GetUnicodeCharacterSequence(int[] characterSet)
		{
			string text = string.Empty;
			int num = characterSet.Length;
			int num2 = characterSet[0];
			int num3 = num2;
			for (int i = 1; i < num; i++)
			{
				if (characterSet[i - 1] + 1 == characterSet[i])
				{
					num3 = characterSet[i];
					continue;
				}
				text = ((num2 != num3) ? (text + num2.ToString("X2") + "-" + num3.ToString("X2") + ",") : (text + num2.ToString("X2") + ","));
				num2 = (num3 = characterSet[i]);
			}
			if (num2 == num3)
			{
				return text + num2.ToString("X2");
			}
			return text + num2.ToString("X2") + "-" + num3.ToString("X2");
		}

		public static void DrawBox(Rect rect, float thickness, Color color)
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() - thickness, ((Rect)(ref rect)).get_y() + thickness, ((Rect)(ref rect)).get_width() + thickness * 2f, thickness), color);
			EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() - thickness, ((Rect)(ref rect)).get_y() + thickness, thickness, ((Rect)(ref rect)).get_height() - thickness * 2f), color);
			EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() - thickness, ((Rect)(ref rect)).get_y() + ((Rect)(ref rect)).get_height() - thickness * 2f, ((Rect)(ref rect)).get_width() + thickness * 2f, thickness), color);
			EditorGUI.DrawRect(new Rect(((Rect)(ref rect)).get_x() + ((Rect)(ref rect)).get_width(), ((Rect)(ref rect)).get_y() + thickness, thickness, ((Rect)(ref rect)).get_height() - thickness * 2f), color);
		}

		public static int GetHorizontalAlignmentGridValue(int value)
		{
			if ((value & 1) == 1)
			{
				return 0;
			}
			if ((value & 2) == 2)
			{
				return 1;
			}
			if ((value & 4) == 4)
			{
				return 2;
			}
			if ((value & 8) == 8)
			{
				return 3;
			}
			if ((value & 0x10) == 16)
			{
				return 4;
			}
			if ((value & 0x20) == 32)
			{
				return 5;
			}
			return 0;
		}

		public static int GetVerticalAlignmentGridValue(int value)
		{
			if ((value & 0x100) == 256)
			{
				return 0;
			}
			if ((value & 0x200) == 512)
			{
				return 1;
			}
			if ((value & 0x400) == 1024)
			{
				return 2;
			}
			if ((value & 0x800) == 2048)
			{
				return 3;
			}
			if ((value & 0x1000) == 4096)
			{
				return 4;
			}
			if ((value & 0x2000) == 8192)
			{
				return 5;
			}
			return 0;
		}
	}
}

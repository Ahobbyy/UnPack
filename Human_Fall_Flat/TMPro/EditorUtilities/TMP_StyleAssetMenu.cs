using System.IO;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class TMP_StyleAssetMenu
	{
		[MenuItem("Assets/Create/TextMeshPro/Style Sheet", false, 120)]
		public static void CreateTextMeshProObjectPerform()
		{
			string text;
			if (Selection.get_assetGUIDs().Length == 0)
			{
				text = "Assets";
			}
			else
			{
				text = AssetDatabase.GUIDToAssetPath(Selection.get_assetGUIDs()[0]);
				if (Path.GetExtension(text) != "")
				{
					text = Path.GetDirectoryName(text);
				}
			}
			string text2 = AssetDatabase.GenerateUniqueAssetPath(text + "/TMP StyleSheet.asset");
			TMP_StyleSheet tMP_StyleSheet = ScriptableObject.CreateInstance<TMP_StyleSheet>();
			AssetDatabase.CreateAsset((Object)(object)tMP_StyleSheet, text2);
			EditorUtility.SetDirty((Object)(object)tMP_StyleSheet);
			AssetDatabase.SaveAssets();
		}
	}
}

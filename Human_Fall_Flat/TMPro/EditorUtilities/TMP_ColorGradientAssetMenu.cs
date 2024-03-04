using System.IO;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	public static class TMP_ColorGradientAssetMenu
	{
		[MenuItem("Assets/Create/TextMeshPro/Color Gradient", false, 110)]
		public static void CreateColorGradient(MenuCommand context)
		{
			string text = ((Selection.get_assetGUIDs().Length != 0) ? AssetDatabase.GUIDToAssetPath(Selection.get_assetGUIDs()[0]) : "Assets/New TMP Color Gradient.asset");
			text = ((!Directory.Exists(text)) ? (Path.GetDirectoryName(text) + "/New TMP Color Gradient.asset") : (text + "/New TMP Color Gradient.asset"));
			text = AssetDatabase.GenerateUniqueAssetPath(text);
			TMP_ColorGradient tMP_ColorGradient = ScriptableObject.CreateInstance<TMP_ColorGradient>();
			AssetDatabase.CreateAsset((Object)(object)tMP_ColorGradient, text);
			AssetDatabase.SaveAssets();
			AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath((Object)(object)tMP_ColorGradient));
			EditorUtility.FocusProjectWindow();
			Selection.set_activeObject((Object)(object)tMP_ColorGradient);
		}
	}
}

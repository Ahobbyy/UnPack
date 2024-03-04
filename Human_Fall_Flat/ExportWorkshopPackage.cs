using System.Collections.Generic;
using System.Text;
using UnityEditor;
using UnityEngine;

public class ExportWorkshopPackage
{
	private const int kMaxAssetsToExport = 5000;

	private static readonly string[] kExportAssetScene = new string[10] { "Assets/WorkShop/Scenes/Levels/BeginnerLevel.unity", "Assets/WorkShop/Scenes/Levels/IntermediateLevel 1.unity", "Assets/WorkShop/Scenes/Levels/WorkshopLobby.unity", "Assets/WorkShop/Scenes/Levels/LevelTemplate.unity", "Assets/WorkShop/Scenes/Levels/LobbyTemplate.unity", "Assets/WorkshopContent/Examples/AaronNodeTest.unity", "Assets/Prefabs/Resources.prefab", "Assets/Prefabs/Game.prefab", "Assets/Prefabs/GameCamera.prefab", "Assets/Sprites/hff_chinese_logo.png" };

	private static List<string> kKeepAssets = new List<string> { "Assets/Sprites/hff_chinese_logo.png", "Assets/Sprites/HFFLogo_jp.png", "Assets/Sprites/LogoNewText.png" };

	private static string[] forcedAssetsNames = new string[10] { "Assets/WorkShop", "Assets/WorkShop/Scenes/Levels/PrefabTestBeds", "Assets/TextMesh Pro", "Assets/PP", "Assets/ProCore", "Assets/Fonts", "Assets/HFF-Code-CSharp.dll", "Assets/Extensions/NativeAudio", "Assets/Plugins", "Assets/Audio" };

	private static string[] removeAssetsWithPath = new string[11]
	{
		"Assets/Plugins/Switch", "Assets/Music", "Assets/Animations", "Assets/LevelImages", "Assets/LobbyImages", "Assets/Prefabs/Menu", "Assets/SkinTextures", "Assets/Sprites", "Assets/Sumo", "Assets/Videos",
		"Assets/Scripts"
	};

	[MenuItem("Human/Export Workshop Package")]
	private static void Export()
	{
		string text = PlayerSettings.get_productName() + ".unitypackage";
		HashSet<string> hashSet = new HashSet<string>();
		HashSet<string> hashSet2 = new HashSet<string>();
		List<string> list = new List<string>(5000);
		Debug.Log((object)("Exporting ... " + text + " " + SystemInfo.get_deviceName()));
		string[] array = AssetDatabase.FindAssets("", forcedAssetsNames);
		for (int i = 0; i < array.Length; i++)
		{
			string item = AssetDatabase.GUIDToAssetPath(array[i]);
			hashSet2.Add(item);
		}
		array = kExportAssetScene;
		for (int i = 0; i < array.Length; i++)
		{
			string[] dependencies = AssetDatabase.GetDependencies(array[i]);
			hashSet.UnionWith(dependencies);
		}
		foreach (string item2 in hashSet2)
		{
			string[] dependencies2 = AssetDatabase.GetDependencies(item2);
			if (dependencies2.Length != 0)
			{
				hashSet.UnionWith(dependencies2);
			}
		}
		hashSet2.Clear();
		foreach (string item3 in hashSet)
		{
			array = removeAssetsWithPath;
			foreach (string value in array)
			{
				if (item3.StartsWith(value))
				{
					list.Add(item3);
					break;
				}
			}
		}
		foreach (string item4 in list)
		{
			if (!kKeepAssets.Contains(item4))
			{
				hashSet.Remove(item4);
			}
		}
		list.Clear();
		string[] array2 = new string[hashSet.Count];
		hashSet.CopyTo(array2);
		AssetDatabase.ExportPackage(array2, text, (ExportPackageOptions)1);
		Debug.Log((object)"Exporting done.");
	}

	private static void DumpStrings(string[] stringArray)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (string value in stringArray)
		{
			stringBuilder.AppendLine(value);
		}
		Debug.Log((object)(stringBuilder.ToString() ?? ""));
	}

	[MenuItem("Human/Export Workshop Package", true)]
	private static bool ValidateExport()
	{
		return SystemInfo.get_deviceName() == "42AGREEN-D1";
	}

	[MenuItem("Human/About")]
	private static void WorkshopPackageAbout()
	{
		EditorUtility.DisplayDialog("About", "Version 1.3", "Ok");
	}
}

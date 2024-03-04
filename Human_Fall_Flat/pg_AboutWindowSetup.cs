using System;
using UnityEditor;

public class pg_AboutWindowSetup : AssetPostprocessor
{
	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		string[] array = Array.FindAll(importedAssets, (string name) => name.Contains("pc_AboutEntry") && !name.EndsWith(".meta"));
		for (int i = 0; i < array.Length && !pg_AboutWindow.Init(array[i], fromMenu: false); i++)
		{
		}
	}

	public pg_AboutWindowSetup()
		: this()
	{
	}
}

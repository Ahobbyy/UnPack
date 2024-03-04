using System.IO;
using UnityEditor;
using UnityEngine;

public class ReimportBlendFiles
{
	[MenuItem("Human/d3t/Re-import all .blend files")]
	private static void ReimportAllBlendFiles()
	{
		FileInfo[] files = new DirectoryInfo("Assets").GetFiles("*.blend", SearchOption.AllDirectories);
		foreach (FileInfo fileInfo in files)
		{
			Debug.Log((object)("Reimporting " + fileInfo.Name));
			AssetDatabase.ImportAsset("Assets" + fileInfo.FullName.Remove(0, Application.get_dataPath().Length), (ImportAssetOptions)1);
		}
		AssetDatabase.Refresh((ImportAssetOptions)8192);
	}
}

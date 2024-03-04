using System.IO;
using UnityEditor;
using UnityEngine;

namespace HashcodeDigital.Editor
{
	public class BlenderUtils
	{
		[MenuItem("Hashcode/Reimport All Blender Files")]
		private static void ReimportAllBlenderFiles()
		{
			string[] files = Directory.GetFiles(Application.get_dataPath() + "/", "*.blend", SearchOption.AllDirectories);
			string directoryName = Path.GetDirectoryName(Application.get_dataPath());
			Debug.Log((object)directoryName);
			using ProgressBarScope progressBarScope = new ProgressBarScope("Reimporting Assets", files.Length);
			string[] array = files;
			foreach (string obj in array)
			{
				progressBarScope.Tick();
				string text = obj.Remove(0, directoryName.Length + 1);
				Debug.Log((object)text);
				AssetDatabase.ImportAsset(text);
			}
		}
	}
}

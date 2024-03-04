using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	[InitializeOnLoad]
	public static class pg_IconUtility
	{
		private const string ICON_FOLDER_PATH = "ProGridsToggles";

		private static string iconFolderPath;

		static pg_IconUtility()
		{
			iconFolderPath = "Assets/ProCore/ProGrids/GUI/ProGridsToggles/";
			if (!Directory.Exists(iconFolderPath))
			{
				string path = FindFolder("ProGridsToggles");
				if (Directory.Exists(path))
				{
					iconFolderPath = path;
				}
			}
		}

		private static string FindFolder(string folder)
		{
			string searchPattern = folder.Replace("\\", "/").Substring(folder.LastIndexOf('/') + 1);
			string[] directories = Directory.GetDirectories("Assets/", searchPattern, SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				string text = directories[i].Replace("\\", "/");
				if (text.Contains(folder))
				{
					if (!text.EndsWith("/"))
					{
						text += "/";
					}
					return text;
				}
			}
			Debug.LogError((object)"Could not locate ProGrids/GUI/ProGridsToggles folder.  The ProGrids folder may be moved, but the contents of ProGrids must remain unmodified.");
			return "";
		}

		public static Texture2D LoadIcon(string iconName)
		{
			string path = $"{iconFolderPath}{iconName}";
			if (!File.Exists(path))
			{
				Debug.LogError((object)("ProGrids failed to locate menu image: " + iconName + ".\nThis can happen if the GUI folder is moved or deleted.  Deleting and re-importing ProGrids will fix this error."));
				return null;
			}
			return pg_IconUtility.LoadAssetAtPath<Texture2D>(path);
		}

		private static T LoadAssetAtPath<T>(string path) where T : Object
		{
			return (T)(object)AssetDatabase.LoadAssetAtPath(path, typeof(T));
		}
	}
}

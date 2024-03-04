using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class RedistCopy
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
	{
	}

	private static void CopyFile(string filename, string outputfilename, string pathToFile, string pathToBuiltProject)
	{
		string text = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), pathToFile), filename);
		string text2 = Path.Combine(Path.GetDirectoryName(pathToBuiltProject), outputfilename);
		if (!File.Exists(text))
		{
			Debug.LogWarning((object)string.Format("[Steamworks.NET] Could not copy {0} into the project root. {0} could not be found in '{1}'. Place {0} from the redist into the project root manually.", filename, pathToFile));
			return;
		}
		if (File.Exists(text2) && File.GetLastWriteTime(text) == File.GetLastWriteTime(text2))
		{
			FileInfo fileInfo = new FileInfo(text);
			FileInfo fileInfo2 = new FileInfo(text2);
			if (fileInfo.Length == fileInfo2.Length)
			{
				return;
			}
		}
		File.Copy(text, text2, overwrite: true);
		File.SetAttributes(text2, File.GetAttributes(text2) & ~FileAttributes.ReadOnly);
		if (!File.Exists(text2))
		{
			Debug.LogWarning((object)string.Format("[Steamworks.NET] Could not copy {0} into the built project. File.Copy() Failed. Place {0} from the redist folder into the output dir manually.", filename));
		}
	}
}

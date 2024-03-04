using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class RedistInstall
{
	static RedistInstall()
	{
		CopyFile("Assets/Plugins/Steamworks.NET/redist", "steam_appid.txt", bCheckDifference: false);
		SetPlatformSettings();
	}

	private static void CopyFile(string path, string filename, bool bCheckDifference)
	{
		string currentDirectory = Directory.GetCurrentDirectory();
		string text = Path.Combine(Path.Combine(currentDirectory, path), filename);
		string text2 = Path.Combine(currentDirectory, filename);
		if (!File.Exists(text))
		{
			Debug.LogWarning((object)string.Format("[Steamworks.NET] Could not copy {0} into the project root. {0} could not be found in '{1}'. Place {0} from the Steamworks SDK in the project root manually.", filename, Path.Combine(currentDirectory, path)));
			return;
		}
		if (File.Exists(text2))
		{
			if (!bCheckDifference)
			{
				return;
			}
			if (File.GetLastWriteTime(text) == File.GetLastWriteTime(text2))
			{
				FileInfo fileInfo = new FileInfo(text);
				FileInfo fileInfo2 = new FileInfo(text2);
				if (fileInfo.Length == fileInfo2.Length)
				{
					return;
				}
			}
			Debug.Log((object)$"[Steamworks.NET] {filename} in the project root differs from the Steamworks.NET redistributable. Updating.... Please relaunch Unity.");
		}
		else
		{
			Debug.Log((object)$"[Steamworks.NET] {filename} is not present in the project root. Copying...");
		}
		File.Copy(text, text2, overwrite: true);
		File.SetAttributes(text2, File.GetAttributes(text2) & ~FileAttributes.ReadOnly);
		if (File.Exists(text2))
		{
			Debug.Log((object)$"[Steamworks.NET] Successfully copied {filename} into the project root. Please relaunch Unity.");
		}
		else
		{
			Debug.LogWarning((object)string.Format("[Steamworks.NET] Could not copy {0} into the project root. File.Copy() Failed. Please copy {0} into the project root manually.", Path.Combine(path, filename)));
		}
	}

	private static void SetPlatformSettings()
	{
		PluginImporter[] allImporters = PluginImporter.GetAllImporters();
		foreach (PluginImporter val in allImporters)
		{
			if ((Object)(object)val == (Object)null || Path.IsPathRooted(((AssetImporter)val).get_assetPath()))
			{
				continue;
			}
			bool flag = false;
			switch (Path.GetFileName(((AssetImporter)val).get_assetPath()))
			{
			case "libsteam_api.dylib":
				flag |= ResetPluginSettings(val, "AnyCPU", "OSX");
				flag |= SetCompatibleWithOSX(val);
				break;
			case "libsteam_api.so":
				if (((AssetImporter)val).get_assetPath().Contains("x86_64"))
				{
					flag |= ResetPluginSettings(val, "x86_64", "Linux");
					flag |= SetCompatibleWithLinux(val, (BuildTarget)24);
				}
				else
				{
					flag |= ResetPluginSettings(val, "x86", "Linux");
					flag |= SetCompatibleWithLinux(val, (BuildTarget)17);
				}
				break;
			case "steam_api.dll":
			case "steam_api64.dll":
				if (((AssetImporter)val).get_assetPath().Contains("x86_64"))
				{
					flag |= ResetPluginSettings(val, "x86_64", "Windows");
					flag |= SetCompatibleWithWindows(val, (BuildTarget)19);
				}
				else
				{
					flag |= ResetPluginSettings(val, "x86", "Windows");
					flag |= SetCompatibleWithWindows(val, (BuildTarget)5);
				}
				break;
			}
			if (flag)
			{
				((AssetImporter)val).SaveAndReimport();
			}
		}
	}

	private static bool ResetPluginSettings(PluginImporter plugin, string CPU, string OS)
	{
		bool result = false;
		if (plugin.GetCompatibleWithAnyPlatform())
		{
			plugin.SetCompatibleWithAnyPlatform(false);
			result = true;
		}
		if (!plugin.GetCompatibleWithEditor())
		{
			plugin.SetCompatibleWithEditor(true);
			result = true;
		}
		if (plugin.GetEditorData("CPU") != CPU)
		{
			plugin.SetEditorData("CPU", CPU);
			result = true;
		}
		if (plugin.GetEditorData("OS") != OS)
		{
			plugin.SetEditorData("OS", OS);
			result = true;
		}
		return result;
	}

	private static bool SetCompatibleWithOSX(PluginImporter plugin)
	{
		return false | SetCompatibleWithPlatform(plugin, (BuildTarget)4, enable: true) | SetCompatibleWithPlatform(plugin, (BuildTarget)27, enable: true) | SetCompatibleWithPlatform(plugin, (BuildTarget)2, enable: true) | SetCompatibleWithPlatform(plugin, (BuildTarget)17, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)24, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)25, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)5, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)19, enable: false);
	}

	private static bool SetCompatibleWithLinux(PluginImporter plugin, BuildTarget platform)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Invalid comparison between Unknown and I4
		bool flag = false;
		if ((int)platform == 17)
		{
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)17, enable: true);
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)24, enable: false);
		}
		else
		{
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)17, enable: false);
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)24, enable: true);
		}
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)25, enable: true);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)4, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)27, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)2, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)5, enable: false);
		return flag | SetCompatibleWithPlatform(plugin, (BuildTarget)19, enable: false);
	}

	private static bool SetCompatibleWithWindows(PluginImporter plugin, BuildTarget platform)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0004: Invalid comparison between Unknown and I4
		bool flag = false;
		if ((int)platform == 5)
		{
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)5, enable: true);
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)19, enable: false);
		}
		else
		{
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)5, enable: false);
			flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)19, enable: true);
		}
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)24, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)17, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)25, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)4, enable: false);
		flag |= SetCompatibleWithPlatform(plugin, (BuildTarget)27, enable: false);
		return flag | SetCompatibleWithPlatform(plugin, (BuildTarget)2, enable: false);
	}

	private static bool SetCompatibleWithEditor(PluginImporter plugin)
	{
		return false | SetCompatibleWithPlatform(plugin, (BuildTarget)24, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)17, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)25, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)4, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)27, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)2, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)5, enable: false) | SetCompatibleWithPlatform(plugin, (BuildTarget)19, enable: false);
	}

	private static bool SetCompatibleWithPlatform(PluginImporter plugin, BuildTarget platform, bool enable)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		if (plugin.GetCompatibleWithPlatform(platform) == enable)
		{
			return false;
		}
		plugin.SetCompatibleWithPlatform(platform, enable);
		return true;
	}
}

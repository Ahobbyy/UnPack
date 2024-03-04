using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public class PiaPluginInstall
{
	public static void InstallPiaPlugin(string platform, string buildType)
	{
		Debug.Log((object)"***** PLUGIN INSTALL START *****");
		string currentDirectory = Directory.GetCurrentDirectory();
		string text = "";
		string text2 = "";
		string text3 = "";
		text = ((!(platform == "iPhoneOS64")) ? (currentDirectory + "\\Assets") : (currentDirectory + "/Assets"));
		switch (platform)
		{
		case "CTR":
			text2 = buildType + " " + text;
			Debug.Log((object)("CTR_PIA_UNITY_ROOT = " + Environment.GetEnvironmentVariable("CTR_PIA_UNITY_ROOT")));
			text3 = Environment.GetEnvironmentVariable("CTR_PIA_UNITY_ROOT") + "/PiaPlugin/Install_CTR.bat";
			break;
		case "Android":
			text2 = buildType + " " + text + " " + platform;
			Debug.Log((object)("ANDROID_PIA_UNITY_ROOT = " + Environment.GetEnvironmentVariable("ANDROID_PIA_UNITY_ROOT")));
			text3 = Environment.GetEnvironmentVariable("ANDROID_PIA_UNITY_ROOT") + "/PiaPlugin/Install_Android.bat";
			break;
		case "iPhoneOS64":
			text2 = buildType + " " + text + " " + platform;
			Debug.Log((object)("IOS_PIA_UNITY_ROOT = " + Environment.GetEnvironmentVariable("IOS_PIA_UNITY_ROOT")));
			text3 = Environment.GetEnvironmentVariable("IOS_PIA_UNITY_ROOT") + "/PiaPlugin/Install_iOS.sh";
			break;
		default:
			text2 = buildType + " \"" + text + "\" " + platform;
			text3 = Directory.GetParent(Environment.GetEnvironmentVariable("NINTENDO_SDK_ROOT"))?.ToString() + "\\NintendoSDK-PiaUnity\\PiaPlugin\\Install_NX.bat";
			break;
		}
		Process process = new Process();
		if (platform == "iPhoneOS64")
		{
			process.StartInfo.FileName = "sh";
			process.StartInfo.Arguments = text3 + " " + text2;
			process.StartInfo.UseShellExecute = false;
			process.StartInfo.RedirectStandardOutput = true;
			process.Start();
			StringBuilder stringBuilder = new StringBuilder();
			string text4 = null;
			while ((text4 = process.StandardOutput.ReadLine()) != null)
			{
				stringBuilder.AppendLine(text4);
			}
			process.WaitForExit();
			if (process.ExitCode == 0)
			{
				stringBuilder.Insert(0, "<color=aqua>plugin install succeeded.</color>\n");
			}
			else
			{
				stringBuilder.Insert(0, "<color=red>plugin install failed.</color>\n");
			}
			Debug.Log((object)stringBuilder);
		}
		else
		{
			process.StartInfo.FileName = text3;
			process.StartInfo.Arguments = text2;
			process.StartInfo.WindowStyle = (Environment.CommandLine.Contains("-batchmode") ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal);
			process.Start();
			process.WaitForExit(6000);
		}
		Debug.Log((object)"***** PLUGIN INSTALL END *****");
	}
}

using System;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.VersionControl;
using UnityEngine;

public class PreBuildEvents : IPreprocessBuild, IOrderedCallback
{
	[Serializable]
	public struct VersionNumber
	{
		public int majorVersion;

		public int minorVersion;

		public int build;

		public int revision;
	}

	public static string BuildVersionString;

	public int callbackOrder => 0;

	public void OnPreprocessBuild(BuildTarget target, string path)
	{
		SetVersionNumber();
	}

	public static void SetVersionNumber(string paramFilePath = "")
	{
		VersionNumber versionNumber = default(VersionNumber);
		int.TryParse(PlayerSettings.get_bundleVersion().Split('.')[0], out versionNumber.majorVersion);
		int.TryParse(PlayerSettings.get_bundleVersion().Split('.')[1], out versionNumber.minorVersion);
		DateTime dateTime = new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
		DateTime now = DateTime.Now;
		versionNumber.build = (now - dateTime).Days;
		versionNumber.revision = (int)(now - DateTime.Today.Date).TotalSeconds / 2;
		BuildVersionString = $"v{versionNumber.majorVersion}{versionNumber.minorVersion}{versionNumber.build}{versionNumber.revision}";
		BuildVersionString = BuildVersionString.Substring(0, 10);
		TextAsset val = Resources.Load<TextAsset>("Curve/build_version_number");
		Asset assetByPath = Provider.GetAssetByPath(AssetDatabase.GetAssetPath((Object)(object)val));
		bool flag = Provider.CheckoutIsValid(assetByPath);
		bool flag2 = Provider.IsOpenForEdit(assetByPath);
		if (flag || flag2)
		{
			if (flag)
			{
				Debug.Log((object)("Checking out asset - " + assetByPath.get_fullName()));
				Provider.Checkout(assetByPath, (CheckoutMode)3).Wait();
			}
			else
			{
				File.SetAttributes(AssetDatabase.GetAssetPath((Object)(object)val), FileAttributes.Normal);
			}
			XmlSerializer xmlSerializer = new XmlSerializer(versionNumber.GetType());
			StreamWriter textWriter = new StreamWriter(AssetDatabase.GetAssetPath((Object)(object)val));
			xmlSerializer.Serialize(textWriter, versionNumber);
			AssetDatabase.Refresh((ImportAssetOptions)8);
		}
		else
		{
			Debug.Log((object)("Check out fail - " + assetByPath.get_fullName()));
		}
	}
}

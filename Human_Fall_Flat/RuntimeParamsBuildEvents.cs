using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;

public class RuntimeParamsBuildEvents : IPreprocessBuild, IOrderedCallback, IPostprocessBuild
{
	public int callbackOrder => 100;

	public void OnPreprocessBuild(BuildTarget target, string path)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		RuntimeParams.ImportXml_InEditor(BuildPipeline.GetBuildTargetGroup(target), playInEditor: false);
		RuntimeParams.Save();
		RuntimeParams.WriteAsXml("Temp/RuntimeParamsForRef.xml");
	}

	public void OnPostprocessBuild(BuildTarget target, string path)
	{
		bool flag = false;
		try
		{
			flag = File.Exists("Assets/Resources/Curve/AssetHashes.bytes");
		}
		catch (Exception)
		{
		}
		if (!flag)
		{
			Debug.LogWarningFormat("File {0} was not found after build - possibly a VC problem", new object[1] { "Assets/Resources/Curve/AssetHashes.bytes" });
		}
		RuntimeParams.Clear();
		RuntimeParams.DeleteResource();
	}
}

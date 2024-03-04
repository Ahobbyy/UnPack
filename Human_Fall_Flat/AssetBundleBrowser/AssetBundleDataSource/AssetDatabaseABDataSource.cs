using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleDataSource
{
	internal class AssetDatabaseABDataSource : ABDataSource
	{
		public string Name => "Default";

		public string ProviderName => "Built-in";

		public bool CanSpecifyBuildTarget => true;

		public bool CanSpecifyBuildOutputDirectory => true;

		public bool CanSpecifyBuildOptions => true;

		public static List<ABDataSource> CreateDataSources()
		{
			AssetDatabaseABDataSource item = new AssetDatabaseABDataSource();
			return new List<ABDataSource> { item };
		}

		public string[] GetAssetPathsFromAssetBundle(string assetBundleName)
		{
			return AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
		}

		public string GetAssetBundleName(string assetPath)
		{
			AssetImporter atPath = AssetImporter.GetAtPath(assetPath);
			if ((Object)(object)atPath == (Object)null)
			{
				return string.Empty;
			}
			string text = atPath.get_assetBundleName();
			if (atPath.get_assetBundleVariant().Length > 0)
			{
				text = text + "." + atPath.get_assetBundleVariant();
			}
			return text;
		}

		public string GetImplicitAssetBundleName(string assetPath)
		{
			return AssetDatabase.GetImplicitAssetBundleName(assetPath);
		}

		public string[] GetAllAssetBundleNames()
		{
			return AssetDatabase.GetAllAssetBundleNames();
		}

		public bool IsReadOnly()
		{
			return false;
		}

		public void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName)
		{
			AssetImporter.GetAtPath(assetPath).SetAssetBundleNameAndVariant(bundleName, variantName);
		}

		public void RemoveUnusedAssetBundleNames()
		{
			AssetDatabase.RemoveUnusedAssetBundleNames();
		}

		public bool BuildAssetBundles(ABBuildInfo info)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			if (info == null)
			{
				Debug.Log((object)"Error in build");
				return false;
			}
			AssetBundleManifest val = BuildPipeline.BuildAssetBundles(info.outputDirectory, info.options, info.buildTarget);
			if ((Object)(object)val == (Object)null)
			{
				Debug.Log((object)"Error in build");
				return false;
			}
			string[] allAssetBundles = val.GetAllAssetBundles();
			foreach (string obj in allAssetBundles)
			{
				if (info.onBuild != null)
				{
					info.onBuild(obj);
				}
			}
			return true;
		}
	}
}

using System;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class AssetBundleRecord
	{
		internal string path { get; private set; }

		internal AssetBundle bundle { get; private set; }

		internal AssetBundleRecord(string path, AssetBundle bundle)
		{
			if (string.IsNullOrEmpty(path) || (Object)null == (Object)(object)bundle)
			{
				throw new ArgumentException($"AssetBundleRecord encountered invalid parameters path={path}, bundle={bundle}");
			}
			this.path = path;
			this.bundle = bundle;
		}
	}
}

namespace AssetBundleBrowser.AssetBundleDataSource
{
	public interface ABDataSource
	{
		string Name { get; }

		string ProviderName { get; }

		bool CanSpecifyBuildTarget { get; }

		bool CanSpecifyBuildOutputDirectory { get; }

		bool CanSpecifyBuildOptions { get; }

		string[] GetAssetPathsFromAssetBundle(string assetBundleName);

		string GetAssetBundleName(string assetPath);

		string GetImplicitAssetBundleName(string assetPath);

		string[] GetAllAssetBundleNames();

		bool IsReadOnly();

		void SetAssetBundleNameAndVariant(string assetPath, string bundleName, string variantName);

		void RemoveUnusedAssetBundleNames();

		bool BuildAssetBundles(ABBuildInfo info);
	}
}

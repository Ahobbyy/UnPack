using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetBundleBrowser.AssetBundleDataSource;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleModel
{
	public static class Model
	{
		internal class ABMoveData
		{
			internal string assetName;

			internal string bundleName;

			internal string variantName;

			internal ABMoveData(string asset, string bundle, string variant)
			{
				assetName = asset;
				bundleName = bundle;
				variantName = variant;
			}

			internal void Apply()
			{
				if (!DataSource.IsReadOnly())
				{
					DataSource.SetAssetBundleNameAndVariant(assetName, bundleName, variantName);
				}
			}
		}

		private const string k_NewBundleBaseName = "newbundle";

		private const string k_NewVariantBaseName = "newvariant";

		internal static Color k_LightGrey = Color.get_grey() * 1.5f;

		private static ABDataSource s_DataSource;

		private static BundleFolderConcreteInfo s_RootLevelBundles = new BundleFolderConcreteInfo("", null);

		private static List<ABMoveData> s_MoveData = new List<ABMoveData>();

		private static List<BundleInfo> s_BundlesToUpdate = new List<BundleInfo>();

		private static Dictionary<string, AssetInfo> s_GlobalAssetList = new Dictionary<string, AssetInfo>();

		private static Dictionary<string, HashSet<string>> s_DependencyTracker = new Dictionary<string, HashSet<string>>();

		private static bool s_InErrorState = false;

		private const string k_DefaultEmptyMessage = "Drag assets here or right-click to begin creating bundles.";

		private const string k_ProblemEmptyMessage = "There was a problem parsing the list of bundles. See console.";

		private static string s_EmptyMessageString;

		private static Texture2D s_folderIcon = null;

		private static Texture2D s_bundleIcon = null;

		private static Texture2D s_sceneIcon = null;

		public static ABDataSource DataSource
		{
			get
			{
				if (s_DataSource == null)
				{
					s_DataSource = new AssetDatabaseABDataSource();
				}
				return s_DataSource;
			}
			set
			{
				s_DataSource = value;
			}
		}

		public static bool Update()
		{
			bool result = false;
			ExecuteAssetMove(forceAct: false);
			int count = s_BundlesToUpdate.Count;
			if (count > 0)
			{
				s_BundlesToUpdate[count - 1].Update();
				s_BundlesToUpdate.RemoveAll((BundleInfo item) => item.doneUpdating);
				if (s_BundlesToUpdate.Count == 0)
				{
					result = true;
					{
						foreach (BundleInfo child in s_RootLevelBundles.GetChildList())
						{
							child.RefreshDupeAssetWarning();
						}
						return result;
					}
				}
			}
			return result;
		}

		internal static void ForceReloadData(TreeView tree)
		{
			s_InErrorState = false;
			Rebuild();
			tree.Reload();
			bool flag = s_BundlesToUpdate.Count == 0;
			EditorUtility.DisplayProgressBar("Updating Bundles", "", 0f);
			int count = s_BundlesToUpdate.Count;
			while (!flag && !s_InErrorState)
			{
				int count2 = s_BundlesToUpdate.Count;
				EditorUtility.DisplayProgressBar("Updating Bundles", s_BundlesToUpdate[count2 - 1].displayName, (float)(count - count2) / (float)count);
				flag = Update();
			}
			EditorUtility.ClearProgressBar();
		}

		public static void Rebuild()
		{
			s_RootLevelBundles = new BundleFolderConcreteInfo("", null);
			s_MoveData = new List<ABMoveData>();
			s_BundlesToUpdate = new List<BundleInfo>();
			s_GlobalAssetList = new Dictionary<string, AssetInfo>();
			Refresh();
		}

		internal static void AddBundlesToUpdate(IEnumerable<BundleInfo> bundles)
		{
			foreach (BundleInfo bundle in bundles)
			{
				bundle.ForceNeedUpdate();
				s_BundlesToUpdate.Add(bundle);
			}
		}

		internal static void Refresh()
		{
			s_EmptyMessageString = "There was a problem parsing the list of bundles. See console.";
			if (s_InErrorState)
			{
				return;
			}
			string[] array = ValidateBundleList();
			if (array != null)
			{
				s_EmptyMessageString = "Drag assets here or right-click to begin creating bundles.";
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					AddBundleToModel(array2[i]);
				}
				AddBundlesToUpdate(s_RootLevelBundles.GetChildList());
			}
			if (s_InErrorState)
			{
				s_RootLevelBundles = new BundleFolderConcreteInfo("", null);
				s_EmptyMessageString = "There was a problem parsing the list of bundles. See console.";
			}
		}

		internal static string[] ValidateBundleList()
		{
			string[] allAssetBundleNames = DataSource.GetAllAssetBundleNames();
			bool flag = true;
			HashSet<string> hashSet = new HashSet<string>();
			int num = 0;
			bool flag2 = false;
			while (num < allAssetBundleNames.Length)
			{
				string text = allAssetBundleNames[num];
				if (!hashSet.Add(text))
				{
					LogError("Two bundles share the same name: " + text);
					flag = false;
				}
				int num2 = text.LastIndexOf('.');
				if (num2 > -1)
				{
					string text2 = text.Substring(0, num2);
					if (text2.LastIndexOf('.') > -1)
					{
						if (!flag2)
						{
							if (!DataSource.IsReadOnly())
							{
								DataSource.RemoveUnusedAssetBundleNames();
							}
							num = 0;
							hashSet.Clear();
							allAssetBundleNames = DataSource.GetAllAssetBundleNames();
							flag2 = true;
							continue;
						}
						LogError(string.Concat(string.Concat("Bundle name '" + text2 + "' contains a period.", "  Internally Unity keeps 'bundleName' and 'variantName' separate, but externally treat them as 'bundleName.variantName'."), "  If a bundleName contains a period, the build will (probably) succeed, but this tool cannot tell which portion is bundle and which portion is variant."));
						flag = false;
					}
					if (allAssetBundleNames.Contains(text2))
					{
						if (!flag2)
						{
							if (!DataSource.IsReadOnly())
							{
								DataSource.RemoveUnusedAssetBundleNames();
							}
							num = 0;
							hashSet.Clear();
							allAssetBundleNames = DataSource.GetAllAssetBundleNames();
							flag2 = true;
							continue;
						}
						flag = false;
						LogError(string.Concat("Bundle name '" + text2 + "' exists without a variant as well as with variant '" + text.Substring(num2 + 1) + "'.", " That is an illegal state that will not build and must be cleaned up."));
					}
				}
				num++;
			}
			if (flag)
			{
				return allAssetBundleNames;
			}
			return null;
		}

		internal static bool BundleListIsEmpty()
		{
			return s_RootLevelBundles.GetChildList().Count() == 0;
		}

		internal static string GetEmptyMessage()
		{
			return s_EmptyMessageString;
		}

		internal static BundleInfo CreateEmptyBundle(BundleFolderInfo folder = null, string newName = null)
		{
			if (folder is BundleVariantFolderInfo)
			{
				return CreateEmptyVariant(folder as BundleVariantFolderInfo);
			}
			folder = ((folder == null) ? s_RootLevelBundles : folder);
			string uniqueName = GetUniqueName(folder, newName);
			BundleNameData nameData = new BundleNameData(folder.m_Name.bundleName, uniqueName);
			return AddBundleToFolder(folder, nameData);
		}

		internal static BundleInfo CreateEmptyVariant(BundleVariantFolderInfo folder)
		{
			string uniqueName = GetUniqueName(folder, "newvariant");
			BundleNameData nameData = new BundleNameData(folder.m_Name.bundleName + "." + uniqueName);
			return AddBundleToFolder(folder.parent, nameData);
		}

		internal static BundleFolderInfo CreateEmptyBundleFolder(BundleFolderConcreteInfo folder = null)
		{
			folder = ((folder == null) ? s_RootLevelBundles : folder);
			string name = GetUniqueName(folder) + "/dummy";
			BundleNameData nameData = new BundleNameData(folder.m_Name.bundleName, name);
			return AddFoldersToBundle(s_RootLevelBundles, nameData);
		}

		private static BundleInfo AddBundleToModel(string name)
		{
			if (name == null)
			{
				return null;
			}
			BundleNameData nameData = new BundleNameData(name);
			return AddBundleToFolder(AddFoldersToBundle(s_RootLevelBundles, nameData), nameData);
		}

		private static BundleFolderConcreteInfo AddFoldersToBundle(BundleFolderInfo root, BundleNameData nameData)
		{
			BundleInfo bundleInfo = root;
			BundleFolderConcreteInfo bundleFolderConcreteInfo = bundleInfo as BundleFolderConcreteInfo;
			int count = nameData.pathTokens.Count;
			for (int i = 0; i < count; i++)
			{
				if (bundleFolderConcreteInfo != null)
				{
					bundleInfo = bundleFolderConcreteInfo.GetChild(nameData.pathTokens[i]);
					if (bundleInfo == null)
					{
						bundleInfo = new BundleFolderConcreteInfo(nameData.pathTokens, i + 1, bundleFolderConcreteInfo);
						bundleFolderConcreteInfo.AddChild(bundleInfo);
					}
					bundleFolderConcreteInfo = bundleInfo as BundleFolderConcreteInfo;
					if (bundleFolderConcreteInfo == null)
					{
						s_InErrorState = true;
						LogFolderAndBundleNameConflict(bundleInfo.m_Name.fullNativeName);
						break;
					}
				}
			}
			return bundleInfo as BundleFolderConcreteInfo;
		}

		private static void LogFolderAndBundleNameConflict(string name)
		{
			LogError(string.Concat(string.Concat(string.Concat("Bundle '" + name, "' has a name conflict with a bundle-folder."), "Display of bundle data and building of bundles will not work."), "\nDetails: If you name a bundle 'x/y', then the result of your build will be a bundle named 'y' in a folder named 'x'.  You thus cannot also have a bundle named 'x' at the same level as the folder named 'x'."));
		}

		private static BundleInfo AddBundleToFolder(BundleFolderInfo root, BundleNameData nameData)
		{
			BundleInfo bundleInfo = root.GetChild(nameData.shortName);
			if (!string.IsNullOrEmpty(nameData.variant))
			{
				if (bundleInfo == null)
				{
					bundleInfo = new BundleVariantFolderInfo(nameData.bundleName, root);
					root.AddChild(bundleInfo);
				}
				BundleVariantFolderInfo bundleVariantFolderInfo = bundleInfo as BundleVariantFolderInfo;
				if (bundleVariantFolderInfo == null)
				{
					LogError(string.Concat(string.Concat(string.Concat("Bundle named " + nameData.shortName, " exists both as a standard bundle, and a bundle with variants.  "), "This message is not supported for display or actual bundle building.  "), "You must manually fix bundle naming in the inspector."));
					return null;
				}
				bundleInfo = bundleVariantFolderInfo.GetChild(nameData.variant);
				if (bundleInfo == null)
				{
					bundleInfo = new BundleVariantDataInfo(nameData.fullNativeName, bundleVariantFolderInfo);
					bundleVariantFolderInfo.AddChild(bundleInfo);
				}
			}
			else if (bundleInfo == null)
			{
				bundleInfo = new BundleDataInfo(nameData.fullNativeName, root);
				root.AddChild(bundleInfo);
			}
			else if (!(bundleInfo is BundleDataInfo))
			{
				s_InErrorState = true;
				LogFolderAndBundleNameConflict(nameData.fullNativeName);
			}
			return bundleInfo;
		}

		private static string GetUniqueName(BundleFolderInfo folder, string suggestedName = null)
		{
			suggestedName = ((suggestedName == null) ? "newbundle" : suggestedName);
			string text = suggestedName;
			int num = 1;
			bool flag = folder.GetChild(text) != null;
			while (flag)
			{
				text = suggestedName + num;
				num++;
				flag = folder.GetChild(text) != null;
			}
			return text;
		}

		internal static BundleTreeItem CreateBundleTreeView()
		{
			return s_RootLevelBundles.CreateTreeView(-1);
		}

		internal static AssetTreeItem CreateAssetListTreeView(IEnumerable<BundleInfo> selectedBundles)
		{
			AssetTreeItem assetTreeItem = new AssetTreeItem();
			if (selectedBundles != null)
			{
				foreach (BundleInfo selectedBundle in selectedBundles)
				{
					selectedBundle.AddAssetsToNode(assetTreeItem);
				}
				return assetTreeItem;
			}
			return assetTreeItem;
		}

		internal static bool HandleBundleRename(BundleTreeItem item, string newName)
		{
			BundleNameData bundleNameData = new BundleNameData(item.bundle.m_Name.fullNativeName);
			int num = newName.LastIndexOf('.');
			int num2 = newName.LastIndexOf('/');
			int num3 = newName.LastIndexOf('\\');
			if (num == 0 || num2 == 0 || num3 == 0)
			{
				return false;
			}
			bool result = item.bundle.HandleRename(newName, 0);
			if (num > 0 || num2 > 0 || num3 > 0)
			{
				item.bundle.parent.HandleChildRename(newName, string.Empty);
			}
			ExecuteAssetMove();
			if (FindBundle(bundleNameData) != null)
			{
				Debug.LogError((object)string.Concat("Failed to rename bundle named: " + bundleNameData.fullNativeName, ".  Most likely this is due to the bundle being assigned to a folder in your Assets directory, AND that folder is either empty or only contains assets that are explicitly assigned elsewhere."));
			}
			return result;
		}

		internal static void HandleBundleReparent(IEnumerable<BundleInfo> bundles, BundleFolderInfo parent)
		{
			parent = ((parent == null) ? s_RootLevelBundles : parent);
			foreach (BundleInfo bundle in bundles)
			{
				bundle.HandleReparent(parent.m_Name.bundleName, parent);
			}
			ExecuteAssetMove();
		}

		internal static void HandleBundleMerge(IEnumerable<BundleInfo> bundles, BundleDataInfo target)
		{
			foreach (BundleInfo bundle in bundles)
			{
				bundle.HandleDelete(isRootOfDelete: true, target.m_Name.bundleName, target.m_Name.variant);
			}
			ExecuteAssetMove();
		}

		internal static void HandleBundleDelete(IEnumerable<BundleInfo> bundles)
		{
			List<BundleNameData> list = new List<BundleNameData>();
			foreach (BundleInfo bundle in bundles)
			{
				list.Add(bundle.m_Name);
				bundle.HandleDelete(isRootOfDelete: true);
			}
			ExecuteAssetMove();
			foreach (BundleNameData item in list)
			{
				if (FindBundle(item) != null)
				{
					Debug.LogError((object)string.Concat("Failed to delete bundle named: " + item.fullNativeName, ".  Most likely this is due to the bundle being assigned to a folder in your Assets directory, AND that folder is either empty or only contains assets that are explicitly assigned elsewhere."));
				}
			}
		}

		internal static BundleInfo FindBundle(BundleNameData name)
		{
			BundleInfo child = s_RootLevelBundles;
			foreach (string pathToken in name.pathTokens)
			{
				if (child is BundleFolderInfo)
				{
					child = (child as BundleFolderInfo).GetChild(pathToken);
					if (child == null)
					{
						return null;
					}
					continue;
				}
				return null;
			}
			if (child is BundleFolderInfo)
			{
				child = (child as BundleFolderInfo).GetChild(name.shortName);
				if (child is BundleVariantFolderInfo)
				{
					child = (child as BundleVariantFolderInfo).GetChild(name.variant);
				}
				return child;
			}
			return null;
		}

		internal static BundleInfo HandleDedupeBundles(IEnumerable<BundleInfo> bundles, bool onlyOverlappedAssets)
		{
			BundleInfo bundleInfo = CreateEmptyBundle();
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			bool flag = s_BundlesToUpdate.Count == 0;
			while (!flag)
			{
				flag = Update();
			}
			foreach (BundleInfo bundle in bundles)
			{
				foreach (AssetInfo dependency in bundle.GetDependencies())
				{
					if (onlyOverlappedAssets)
					{
						if (!hashSet2.Add(dependency.fullAssetName))
						{
							hashSet.Add(dependency.fullAssetName);
						}
					}
					else if (dependency.IsMessageSet(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles))
					{
						hashSet.Add(dependency.fullAssetName);
					}
				}
			}
			if (hashSet.Count == 0)
			{
				return null;
			}
			MoveAssetToBundle(hashSet, bundleInfo.m_Name.bundleName, string.Empty);
			ExecuteAssetMove();
			return bundleInfo;
		}

		internal static BundleInfo HandleConvertToVariant(BundleDataInfo bundle)
		{
			bundle.HandleDelete(isRootOfDelete: true, bundle.m_Name.bundleName, "newvariant");
			ExecuteAssetMove();
			BundleVariantFolderInfo bundleVariantFolderInfo = bundle.parent.GetChild(bundle.m_Name.shortName) as BundleVariantFolderInfo;
			if (bundleVariantFolderInfo != null)
			{
				return bundleVariantFolderInfo.GetChild("newvariant");
			}
			BundleVariantFolderInfo bundleVariantFolderInfo2 = new BundleVariantFolderInfo(bundle.m_Name.bundleName, bundle.parent);
			BundleVariantDataInfo bundleVariantDataInfo = new BundleVariantDataInfo(bundle.m_Name.bundleName + ".newvariant", bundleVariantFolderInfo2);
			bundle.parent.AddChild(bundleVariantFolderInfo2);
			bundleVariantFolderInfo2.AddChild(bundleVariantDataInfo);
			return bundleVariantDataInfo;
		}

		internal static void MoveAssetToBundle(AssetInfo asset, string bundleName, string variant)
		{
			s_MoveData.Add(new ABMoveData(asset.fullAssetName, bundleName, variant));
		}

		internal static void MoveAssetToBundle(string assetName, string bundleName, string variant)
		{
			s_MoveData.Add(new ABMoveData(assetName, bundleName, variant));
		}

		internal static void MoveAssetToBundle(IEnumerable<AssetInfo> assets, string bundleName, string variant)
		{
			foreach (AssetInfo asset in assets)
			{
				MoveAssetToBundle(asset, bundleName, variant);
			}
		}

		internal static void MoveAssetToBundle(IEnumerable<string> assetNames, string bundleName, string variant)
		{
			foreach (string assetName in assetNames)
			{
				MoveAssetToBundle(assetName, bundleName, variant);
			}
		}

		internal static void ExecuteAssetMove(bool forceAct = true)
		{
			int count = s_MoveData.Count;
			if (!forceAct)
			{
				return;
			}
			if (count > 0)
			{
				bool @bool = EditorPrefs.GetBool("kAutoRefresh");
				EditorPrefs.SetBool("kAutoRefresh", false);
				EditorUtility.DisplayProgressBar("Moving assets to bundles", "", 0f);
				for (int i = 0; i < count; i++)
				{
					EditorUtility.DisplayProgressBar("Moving assets to bundle " + s_MoveData[i].bundleName, Path.GetFileNameWithoutExtension(s_MoveData[i].assetName), (float)i / (float)count);
					s_MoveData[i].Apply();
				}
				EditorUtility.ClearProgressBar();
				EditorPrefs.SetBool("kAutoRefresh", @bool);
				s_MoveData.Clear();
			}
			if (!DataSource.IsReadOnly())
			{
				DataSource.RemoveUnusedAssetBundleNames();
			}
			Refresh();
		}

		internal static AssetInfo CreateAsset(string name, AssetInfo parent)
		{
			if (ValidateAsset(name))
			{
				string bundleName = GetBundleName(name);
				return CreateAsset(name, bundleName, parent);
			}
			return null;
		}

		internal static AssetInfo CreateAsset(string name, string bundleName)
		{
			if (ValidateAsset(name))
			{
				return CreateAsset(name, bundleName, null);
			}
			return null;
		}

		private static AssetInfo CreateAsset(string name, string bundleName, AssetInfo parent)
		{
			if (!string.IsNullOrEmpty(bundleName))
			{
				return new AssetInfo(name, bundleName);
			}
			AssetInfo value = null;
			if (!s_GlobalAssetList.TryGetValue(name, out value))
			{
				value = new AssetInfo(name, string.Empty);
				s_GlobalAssetList.Add(name, value);
			}
			value.AddParent(parent.displayName);
			return value;
		}

		internal static bool ValidateAsset(string name)
		{
			if (!name.StartsWith("Assets/"))
			{
				return false;
			}
			switch (Path.GetExtension(name))
			{
			case ".dll":
			case ".cs":
			case ".meta":
			case ".js":
			case ".boo":
				return false;
			default:
				return true;
			}
		}

		internal static string GetBundleName(string asset)
		{
			return DataSource.GetAssetBundleName(asset);
		}

		internal static int RegisterAsset(AssetInfo asset, string bundle)
		{
			if (s_DependencyTracker.ContainsKey(asset.fullAssetName))
			{
				s_DependencyTracker[asset.fullAssetName].Add(bundle);
				int count = s_DependencyTracker[asset.fullAssetName].Count;
				if (count > 1)
				{
					asset.SetMessageFlag(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles, on: true);
				}
				return count;
			}
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add(bundle);
			s_DependencyTracker.Add(asset.fullAssetName, hashSet);
			return 1;
		}

		internal static void UnRegisterAsset(AssetInfo asset, string bundle)
		{
			if (s_DependencyTracker != null && asset != null && s_DependencyTracker.ContainsKey(asset.fullAssetName))
			{
				s_DependencyTracker[asset.fullAssetName].Remove(bundle);
				switch (s_DependencyTracker[asset.fullAssetName].Count)
				{
				case 0:
					s_DependencyTracker.Remove(asset.fullAssetName);
					break;
				case 1:
					asset.SetMessageFlag(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles, on: false);
					break;
				}
			}
		}

		internal static IEnumerable<string> CheckDependencyTracker(AssetInfo asset)
		{
			if (s_DependencyTracker.ContainsKey(asset.fullAssetName))
			{
				return s_DependencyTracker[asset.fullAssetName];
			}
			return new HashSet<string>();
		}

		internal static void LogError(string message)
		{
			Debug.LogError((object)("AssetBundleBrowser: " + message));
		}

		internal static void LogWarning(string message)
		{
			Debug.LogWarning((object)("AssetBundleBrowser: " + message));
		}

		internal static Texture2D GetFolderIcon()
		{
			if ((Object)(object)s_folderIcon == (Object)null)
			{
				FindBundleIcons();
			}
			return s_folderIcon;
		}

		internal static Texture2D GetBundleIcon()
		{
			if ((Object)(object)s_bundleIcon == (Object)null)
			{
				FindBundleIcons();
			}
			return s_bundleIcon;
		}

		internal static Texture2D GetSceneIcon()
		{
			if ((Object)(object)s_sceneIcon == (Object)null)
			{
				FindBundleIcons();
			}
			return s_sceneIcon;
		}

		private static void FindBundleIcons()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Expected O, but got Unknown
			s_folderIcon = EditorGUIUtility.FindTexture("Folder Icon");
			if (Directory.Exists(Path.GetFullPath("Packages/com.unity.assetbundlebrowser")))
			{
				s_bundleIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Packages/com.unity.assetbundlebrowser/Editor/Icons/ABundleBrowserIconY1756Basic.png", typeof(Texture2D));
				s_sceneIcon = (Texture2D)AssetDatabase.LoadAssetAtPath("Packages/com.unity.assetbundlebrowser/Editor/Icons/ABundleBrowserIconY1756Scene.png", typeof(Texture2D));
			}
		}
	}
}

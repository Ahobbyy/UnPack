using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class BundleDataInfo : BundleInfo
	{
		protected List<AssetInfo> m_ConcreteAssets;

		protected List<AssetInfo> m_DependentAssets;

		protected HashSet<string> m_BundleDependencies;

		protected int m_ConcreteCounter;

		protected int m_DependentCounter;

		protected bool m_IsSceneBundle;

		protected long m_TotalSize;

		internal bool isSceneBundle => m_IsSceneBundle;

		internal BundleDataInfo(string name, BundleFolderInfo parent)
			: base(name, parent)
		{
			m_ConcreteAssets = new List<AssetInfo>();
			m_DependentAssets = new List<AssetInfo>();
			m_BundleDependencies = new HashSet<string>();
			m_ConcreteCounter = 0;
			m_DependentCounter = 0;
		}

		~BundleDataInfo()
		{
			foreach (AssetInfo dependentAsset in m_DependentAssets)
			{
				Model.UnRegisterAsset(dependentAsset, m_Name.fullNativeName);
			}
		}

		internal override bool HandleRename(string newName, int reverseDepth)
		{
			RefreshAssetList();
			if (!base.HandleRename(newName, reverseDepth))
			{
				return false;
			}
			Model.MoveAssetToBundle(m_ConcreteAssets, m_Name.bundleName, m_Name.variant);
			return true;
		}

		internal override void HandleDelete(bool isRootOfDelete, string forcedNewName = "", string forcedNewVariant = "")
		{
			RefreshAssetList();
			base.HandleDelete(isRootOfDelete);
			Model.MoveAssetToBundle(m_ConcreteAssets, forcedNewName, forcedNewVariant);
		}

		internal string TotalSize()
		{
			if (m_TotalSize == 0L)
			{
				return "--";
			}
			return EditorUtility.FormatBytes(m_TotalSize);
		}

		internal override void RefreshAssetList()
		{
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles, on: false);
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.SceneBundleConflict, on: false);
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.DependencySceneConflict, on: false);
			m_ConcreteAssets.Clear();
			m_TotalSize = 0L;
			m_IsSceneBundle = false;
			foreach (AssetInfo dependentAsset in m_DependentAssets)
			{
				Model.UnRegisterAsset(dependentAsset, m_Name.fullNativeName);
			}
			m_DependentAssets.Clear();
			m_BundleDependencies.Clear();
			bool flag = false;
			bool flag2 = false;
			string[] assetPathsFromAssetBundle = Model.DataSource.GetAssetPathsFromAssetBundle(m_Name.fullNativeName);
			foreach (string text in assetPathsFromAssetBundle)
			{
				if (AssetDatabase.GetMainAssetTypeAtPath(text) == typeof(SceneAsset))
				{
					m_IsSceneBundle = true;
					if (flag)
					{
						flag2 = true;
					}
				}
				else
				{
					flag = true;
					if (m_IsSceneBundle)
					{
						flag2 = true;
					}
				}
				string bundleName = Model.GetBundleName(text);
				if (string.IsNullOrEmpty(bundleName))
				{
					string text2 = text;
					while (!string.IsNullOrEmpty(text2) && text2 != "Assets" && string.IsNullOrEmpty(bundleName))
					{
						text2 = text2.Substring(0, text2.LastIndexOf('/'));
						bundleName = Model.GetBundleName(text2);
					}
					if (string.IsNullOrEmpty(bundleName))
					{
						continue;
					}
					AssetInfo folderAsset = Model.CreateAsset(text2, bundleName);
					folderAsset.isFolder = true;
					if (m_ConcreteAssets.FindIndex((AssetInfo a) => a.displayName == folderAsset.displayName) == -1)
					{
						m_ConcreteAssets.Add(folderAsset);
					}
					m_DependentAssets.Add(Model.CreateAsset(text, folderAsset));
					if (m_DependentAssets != null && m_DependentAssets.Count > 0)
					{
						AssetInfo assetInfo = m_DependentAssets.Last();
						if (assetInfo != null)
						{
							m_TotalSize += assetInfo.fileSize;
						}
					}
					continue;
				}
				AssetInfo assetInfo2 = Model.CreateAsset(text, m_Name.fullNativeName);
				if (assetInfo2 != null)
				{
					m_ConcreteAssets.Add(assetInfo2);
					m_TotalSize += m_ConcreteAssets.Last().fileSize;
					if (AssetDatabase.GetMainAssetTypeAtPath(text) == typeof(SceneAsset))
					{
						m_IsSceneBundle = true;
						m_ConcreteAssets.Last().isScene = true;
					}
				}
			}
			if (flag2)
			{
				foreach (AssetInfo concreteAsset in m_ConcreteAssets)
				{
					if (concreteAsset.isFolder)
					{
						concreteAsset.SetMessageFlag(MessageSystem.MessageFlag.DependencySceneConflict, on: true);
						m_BundleMessages.SetFlag(MessageSystem.MessageFlag.DependencySceneConflict, on: true);
					}
					else
					{
						concreteAsset.SetMessageFlag(MessageSystem.MessageFlag.SceneBundleConflict, on: true);
						m_BundleMessages.SetFlag(MessageSystem.MessageFlag.SceneBundleConflict, on: true);
					}
				}
			}
			m_ConcreteCounter = 0;
			m_DependentCounter = 0;
			m_Dirty = true;
		}

		internal override void AddAssetsToNode(AssetTreeItem node)
		{
			foreach (AssetInfo concreteAsset in m_ConcreteAssets)
			{
				((TreeViewItem)node).AddChild((TreeViewItem)(object)new AssetTreeItem(concreteAsset));
			}
			foreach (AssetInfo dependentAsset in m_DependentAssets)
			{
				if (!node.ContainsChild(dependentAsset))
				{
					((TreeViewItem)node).AddChild((TreeViewItem)(object)new AssetTreeItem(dependentAsset));
				}
			}
			m_Dirty = false;
		}

		internal HashSet<string> GetBundleDependencies()
		{
			return m_BundleDependencies;
		}

		internal override void Update()
		{
			int count = m_DependentAssets.Count;
			int count2 = m_BundleDependencies.Count;
			if (m_ConcreteCounter < m_ConcreteAssets.Count)
			{
				GatherDependencies(m_ConcreteAssets[m_ConcreteCounter]);
				m_ConcreteCounter++;
				m_DoneUpdating = false;
			}
			else if (m_DependentCounter < m_DependentAssets.Count)
			{
				GatherDependencies(m_DependentAssets[m_DependentCounter], m_Name.fullNativeName);
				m_DependentCounter++;
				m_DoneUpdating = false;
			}
			else
			{
				m_DoneUpdating = true;
			}
			m_Dirty = count != m_DependentAssets.Count || count2 != m_BundleDependencies.Count;
			if (m_Dirty || m_DoneUpdating)
			{
				RefreshMessages();
			}
		}

		private void GatherDependencies(AssetInfo asset, string parentBundle = "")
		{
			if (string.IsNullOrEmpty(parentBundle))
			{
				parentBundle = asset.bundleName;
			}
			if (asset == null)
			{
				return;
			}
			List<AssetInfo> dependencies = asset.GetDependencies();
			if (dependencies == null)
			{
				return;
			}
			foreach (AssetInfo item in dependencies)
			{
				if (item == asset || m_ConcreteAssets.Contains(item) || m_DependentAssets.Contains(item))
				{
					continue;
				}
				string implicitAssetBundleName = Model.DataSource.GetImplicitAssetBundleName(item.fullAssetName);
				if (string.IsNullOrEmpty(implicitAssetBundleName))
				{
					m_DependentAssets.Add(item);
					m_TotalSize += item.fileSize;
					if (Model.RegisterAsset(item, parentBundle) > 1)
					{
						SetDuplicateWarning();
					}
				}
				else if (implicitAssetBundleName != m_Name.fullNativeName)
				{
					m_BundleDependencies.Add(implicitAssetBundleName);
				}
			}
		}

		internal override bool RefreshDupeAssetWarning()
		{
			foreach (AssetInfo dependentAsset in m_DependentAssets)
			{
				if (dependentAsset != null && dependentAsset.IsMessageSet(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles))
				{
					SetDuplicateWarning();
					return true;
				}
			}
			return false;
		}

		internal bool IsEmpty()
		{
			return m_ConcreteAssets.Count == 0;
		}

		internal override bool RefreshEmptyStatus()
		{
			bool flag = IsEmpty();
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.EmptyBundle, flag);
			return flag;
		}

		protected void SetDuplicateWarning()
		{
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles, on: true);
			m_Dirty = true;
		}

		internal override BundleTreeItem CreateTreeView(int depth)
		{
			RefreshAssetList();
			RefreshMessages();
			if (isSceneBundle)
			{
				return new BundleTreeItem(this, depth, Model.GetSceneIcon());
			}
			return new BundleTreeItem(this, depth, Model.GetBundleIcon());
		}

		internal override void HandleReparent(string parentName, BundleFolderInfo newParent = null)
		{
			RefreshAssetList();
			string text = (string.IsNullOrEmpty(parentName) ? "" : (parentName + "/"));
			text += m_Name.shortName;
			if (text == m_Name.bundleName)
			{
				return;
			}
			if (newParent != null && newParent.GetChild(text) != null)
			{
				Model.LogWarning("An item named '" + text + "' already exists at this level in hierarchy.  If your desire is to merge bundles, drag one on top of the other.");
				return;
			}
			foreach (AssetInfo concreteAsset in m_ConcreteAssets)
			{
				Model.MoveAssetToBundle(concreteAsset, text, m_Name.variant);
			}
			if (newParent != null)
			{
				m_Parent.HandleChildRename(m_Name.shortName, string.Empty);
				m_Parent = newParent;
				m_Parent.AddChild(this);
			}
			m_Name.SetBundleName(text, m_Name.variant);
		}

		internal override List<AssetInfo> GetDependencies()
		{
			return m_DependentAssets;
		}

		internal override bool DoesItemMatchSearch(string search)
		{
			foreach (AssetInfo concreteAsset in m_ConcreteAssets)
			{
				if (concreteAsset.displayName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			foreach (AssetInfo dependentAsset in m_DependentAssets)
			{
				if (dependentAsset.displayName.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					return true;
				}
			}
			return false;
		}
	}
}

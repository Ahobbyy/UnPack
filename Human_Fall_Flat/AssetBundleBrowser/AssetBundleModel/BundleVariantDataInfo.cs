using System.Collections.Generic;
using System.Linq;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class BundleVariantDataInfo : BundleDataInfo
	{
		protected List<AssetInfo> m_FolderIncludeAssets = new List<AssetInfo>();

		internal override string displayName => m_Name.variant;

		internal BundleVariantDataInfo(string name, BundleFolderInfo parent)
			: base(name, parent)
		{
		}

		internal override void Update()
		{
			base.Update();
			(m_Parent as BundleVariantFolderInfo).ValidateVariants();
		}

		internal override void RefreshAssetList()
		{
			m_FolderIncludeAssets.Clear();
			base.RefreshAssetList();
			if (m_DependentAssets.Count > 0)
			{
				m_FolderIncludeAssets = new List<AssetInfo>(m_DependentAssets);
			}
		}

		internal bool IsSceneVariant()
		{
			RefreshAssetList();
			return base.isSceneBundle;
		}

		internal override bool HandleRename(string newName, int reverseDepth)
		{
			switch (reverseDepth)
			{
			case 0:
				RefreshAssetList();
				if (!m_Parent.HandleChildRename(m_Name.variant, newName))
				{
					return false;
				}
				m_Name.variant = newName;
				Model.MoveAssetToBundle(m_ConcreteAssets, m_Name.bundleName, m_Name.variant);
				break;
			case 1:
				RefreshAssetList();
				m_Name.PartialNameChange(newName + "." + m_Name.variant, 0);
				Model.MoveAssetToBundle(m_ConcreteAssets, m_Name.bundleName, m_Name.variant);
				break;
			default:
				return base.HandleRename(newName, reverseDepth - 1);
			}
			return true;
		}

		internal override void HandleDelete(bool isRootOfDelete, string forcedNewName = "", string forcedNewVariant = "")
		{
			RefreshAssetList();
			if (isRootOfDelete)
			{
				m_Parent.HandleChildRename(m_Name.variant, string.Empty);
			}
			Model.MoveAssetToBundle(m_ConcreteAssets, forcedNewName, forcedNewVariant);
		}

		internal bool FindContentMismatch(BundleVariantDataInfo other)
		{
			bool result = false;
			if (m_FolderIncludeAssets.Count != 0 || other.m_FolderIncludeAssets.Count != 0)
			{
				HashSet<string> hashSet = new HashSet<string>();
				HashSet<string> hashSet2 = new HashSet<string>(other.m_FolderIncludeAssets.Select((AssetInfo x) => x.displayName));
				foreach (AssetInfo folderIncludeAsset in m_FolderIncludeAssets)
				{
					if (!hashSet2.Remove(folderIncludeAsset.displayName))
					{
						hashSet.Add(folderIncludeAsset.displayName);
					}
				}
				if (hashSet.Count > 0)
				{
					m_BundleMessages.SetFlag(MessageSystem.MessageFlag.VariantBundleMismatch, on: true);
					result = true;
				}
				if (hashSet2.Count > 0)
				{
					other.m_BundleMessages.SetFlag(MessageSystem.MessageFlag.VariantBundleMismatch, on: true);
					result = true;
				}
			}
			else
			{
				HashSet<string> hashSet3 = new HashSet<string>();
				HashSet<string> hashSet4 = new HashSet<string>(other.m_ConcreteAssets.Select((AssetInfo x) => x.displayName));
				foreach (AssetInfo concreteAsset in m_ConcreteAssets)
				{
					if (!hashSet4.Remove(concreteAsset.displayName))
					{
						hashSet3.Add(concreteAsset.displayName);
					}
				}
				if (hashSet3.Count > 0)
				{
					m_BundleMessages.SetFlag(MessageSystem.MessageFlag.VariantBundleMismatch, on: true);
					result = true;
				}
				if (hashSet4.Count > 0)
				{
					other.m_BundleMessages.SetFlag(MessageSystem.MessageFlag.VariantBundleMismatch, on: true);
					result = true;
				}
			}
			return result;
		}
	}
}

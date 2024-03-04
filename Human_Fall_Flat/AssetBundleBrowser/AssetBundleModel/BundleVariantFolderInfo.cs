using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class BundleVariantFolderInfo : BundleFolderInfo
	{
		private bool m_validated;

		internal BundleVariantFolderInfo(string name, BundleFolderInfo parent)
			: base(name, parent)
		{
		}

		internal override void AddChild(BundleInfo info)
		{
			m_Children.Add(info.m_Name.variant, info);
		}

		internal override void Update()
		{
			m_validated = false;
			base.Update();
			if (!m_validated)
			{
				ValidateVariants();
			}
		}

		internal void ValidateVariants()
		{
			m_validated = true;
			bool flag = false;
			if (m_Children.Count > 1)
			{
				BundleVariantDataInfo bundleVariantDataInfo = null;
				foreach (KeyValuePair<string, BundleInfo> child in m_Children)
				{
					BundleVariantDataInfo bundleVariantDataInfo2 = child.Value as BundleVariantDataInfo;
					bundleVariantDataInfo2.SetMessageFlag(MessageSystem.MessageFlag.VariantBundleMismatch, on: false);
					if (bundleVariantDataInfo == null)
					{
						bundleVariantDataInfo = bundleVariantDataInfo2;
					}
					else
					{
						flag |= bundleVariantDataInfo.FindContentMismatch(bundleVariantDataInfo2);
					}
				}
			}
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.VariantBundleMismatch, flag);
		}

		internal override BundleTreeItem CreateTreeView(int depth)
		{
			RefreshMessages();
			Texture2D val = null;
			val = ((m_Children.Count <= 0 || !(m_Children.First().Value as BundleVariantDataInfo).IsSceneVariant()) ? Model.GetBundleIcon() : Model.GetSceneIcon());
			BundleTreeItem bundleTreeItem = new BundleTreeItem(this, depth, val);
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				((TreeViewItem)bundleTreeItem).AddChild((TreeViewItem)(object)child.Value.CreateTreeView(depth + 1));
			}
			return bundleTreeItem;
		}

		internal override void HandleReparent(string parentName, BundleFolderInfo newParent = null)
		{
			string text = (string.IsNullOrEmpty(parentName) ? "" : (parentName + "/"));
			text += displayName;
			if (text == m_Name.bundleName)
			{
				return;
			}
			if (newParent != null && newParent.GetChild(text) != null)
			{
				Model.LogWarning("An item named '" + text + "' already exists at this level in hierarchy.  If your desire is to merge bundles, drag one on top of the other.");
				return;
			}
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.HandleReparent(parentName);
			}
			if (newParent != null)
			{
				m_Parent.HandleChildRename(m_Name.shortName, string.Empty);
				m_Parent = newParent;
				m_Parent.AddChild(this);
			}
			m_Name.SetBundleName(text, string.Empty);
		}

		internal override bool HandleChildRename(string oldName, string newName)
		{
			bool result = base.HandleChildRename(oldName, newName);
			if (m_Children.Count == 0)
			{
				HandleDelete(isRootOfDelete: true);
			}
			return result;
		}
	}
}

using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class BundleFolderConcreteInfo : BundleFolderInfo
	{
		internal BundleFolderConcreteInfo(string name, BundleFolderInfo parent)
			: base(name, parent)
		{
		}

		internal BundleFolderConcreteInfo(List<string> path, int depth, BundleFolderInfo parent)
			: base(path, depth, parent)
		{
		}

		internal override void AddChild(BundleInfo info)
		{
			m_Children.Add(info.displayName, info);
		}

		internal override BundleTreeItem CreateTreeView(int depth)
		{
			RefreshMessages();
			BundleTreeItem bundleTreeItem = new BundleTreeItem(this, depth, Model.GetFolderIcon());
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
				child.Value.HandleReparent(text);
			}
			if (newParent != null)
			{
				m_Parent.HandleChildRename(m_Name.shortName, string.Empty);
				m_Parent = newParent;
				m_Parent.AddChild(this);
			}
			m_Name.SetBundleName(text, m_Name.variant);
		}
	}
}

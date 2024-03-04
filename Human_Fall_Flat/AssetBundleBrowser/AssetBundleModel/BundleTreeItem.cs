using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal sealed class BundleTreeItem : TreeViewItem
	{
		private BundleInfo m_Bundle;

		internal BundleInfo bundle => m_Bundle;

		public override string displayName
		{
			get
			{
				if (!AssetBundleBrowserMain.instance.m_ManageTab.hasSearch)
				{
					return m_Bundle.displayName;
				}
				return m_Bundle.m_Name.fullNativeName;
			}
		}

		internal BundleTreeItem(BundleInfo b, int depth, Texture2D iconTexture)
			: this(b.nameHashCode, depth, b.displayName)
		{
			m_Bundle = b;
			((TreeViewItem)this).set_icon(iconTexture);
			((TreeViewItem)this).set_children(new List<TreeViewItem>());
		}

		internal MessageSystem.Message BundleMessage()
		{
			return m_Bundle.HighestMessage();
		}
	}
}

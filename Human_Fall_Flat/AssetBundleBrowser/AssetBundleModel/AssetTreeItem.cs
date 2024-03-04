using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal sealed class AssetTreeItem : TreeViewItem
	{
		private AssetInfo m_asset;

		private Color m_color = new Color(0f, 0f, 0f, 0f);

		internal AssetInfo asset => m_asset;

		internal Color itemColor
		{
			get
			{
				//IL_0021: Unknown result type (might be due to invalid IL or missing references)
				//IL_0026: Unknown result type (might be due to invalid IL or missing references)
				//IL_002c: Unknown result type (might be due to invalid IL or missing references)
				if (m_color.a == 0f && m_asset != null)
				{
					m_color = m_asset.GetColor();
				}
				return m_color;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_color = value;
			}
		}

		internal AssetTreeItem()
			: this(-1, -1)
		{
		}//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)


		internal AssetTreeItem(AssetInfo a)
			: this(a?.fullAssetName.GetHashCode() ?? Random.Range(int.MinValue, int.MaxValue), 0, (a != null) ? a.displayName : "failed")
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			m_asset = a;
			if (a != null)
			{
				_003F val = this;
				Texture cachedIcon = AssetDatabase.GetCachedIcon(a.fullAssetName);
				((TreeViewItem)val).set_icon((Texture2D)(object)((cachedIcon is Texture2D) ? cachedIcon : null));
			}
		}

		internal Texture2D MessageIcon()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return MessageSystem.GetIcon(HighestMessageLevel());
		}

		internal MessageType HighestMessageLevel()
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			if (m_asset == null)
			{
				return (MessageType)3;
			}
			return m_asset.HighestMessageLevel();
		}

		internal bool ContainsChild(AssetInfo asset)
		{
			bool result = false;
			if (((TreeViewItem)this).get_children() == null)
			{
				return result;
			}
			if (asset == null)
			{
				return false;
			}
			foreach (TreeViewItem child in ((TreeViewItem)this).get_children())
			{
				AssetTreeItem assetTreeItem = child as AssetTreeItem;
				if (assetTreeItem != null && assetTreeItem.asset != null && assetTreeItem.asset.fullAssetName == asset.fullAssetName)
				{
					return true;
				}
			}
			return result;
		}
	}
}

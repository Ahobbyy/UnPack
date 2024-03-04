using System.Collections.Generic;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal abstract class BundleFolderInfo : BundleInfo
	{
		protected Dictionary<string, BundleInfo> m_Children;

		internal override bool doneUpdating
		{
			get
			{
				foreach (KeyValuePair<string, BundleInfo> child in m_Children)
				{
					m_DoneUpdating &= child.Value.doneUpdating;
				}
				return base.doneUpdating;
			}
		}

		internal BundleFolderInfo(string name, BundleFolderInfo parent)
			: base(name, parent)
		{
			m_Children = new Dictionary<string, BundleInfo>();
		}

		internal BundleFolderInfo(List<string> path, int depth, BundleFolderInfo parent)
			: base("", parent)
		{
			m_Children = new Dictionary<string, BundleInfo>();
			m_Name = new BundleNameData("");
			m_Name.pathTokens = path.GetRange(0, depth);
		}

		internal BundleInfo GetChild(string name)
		{
			if (name == null)
			{
				return null;
			}
			BundleInfo value = null;
			if (m_Children.TryGetValue(name, out value))
			{
				return value;
			}
			return null;
		}

		internal Dictionary<string, BundleInfo>.ValueCollection GetChildList()
		{
			return m_Children.Values;
		}

		internal abstract void AddChild(BundleInfo info);

		internal override bool HandleRename(string newName, int reverseDepth)
		{
			if (!base.HandleRename(newName, reverseDepth))
			{
				return false;
			}
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.HandleRename(newName, reverseDepth + 1);
			}
			return true;
		}

		internal override void HandleDelete(bool isRootOfDelete, string forcedNewName = "", string forcedNewVariant = "")
		{
			base.HandleDelete(isRootOfDelete);
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.HandleDelete(isRootOfDelete: false, forcedNewName, forcedNewVariant);
			}
			m_Children.Clear();
		}

		internal override bool DoesItemMatchSearch(string search)
		{
			return false;
		}

		protected override void RefreshMessages()
		{
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.ErrorInChildren, on: false);
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				if (child.Value.IsMessageSet(MessageSystem.MessageFlag.Error))
				{
					m_BundleMessages.SetFlag(MessageSystem.MessageFlag.ErrorInChildren, on: true);
					break;
				}
			}
			base.RefreshMessages();
		}

		internal override bool RefreshEmptyStatus()
		{
			bool flag = m_Children.Count == 0;
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				flag |= child.Value.RefreshEmptyStatus();
			}
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.EmptyFolder, flag);
			return flag;
		}

		internal override void RefreshAssetList()
		{
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.RefreshAssetList();
			}
		}

		internal override bool RefreshDupeAssetWarning()
		{
			bool flag = false;
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				flag |= child.Value.RefreshDupeAssetWarning();
			}
			m_BundleMessages.SetFlag(MessageSystem.MessageFlag.WarningInChildren, flag);
			return flag;
		}

		internal override void AddAssetsToNode(AssetTreeItem node)
		{
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.AddAssetsToNode(node);
			}
			m_Dirty = false;
		}

		internal virtual bool HandleChildRename(string oldName, string newName)
		{
			if (!string.IsNullOrEmpty(newName) && m_Children.ContainsKey(newName))
			{
				Model.LogWarning("Attempting to name an item '" + newName + "' which matches existing name at this level in hierarchy.  If your desire is to merge bundles, drag one on top of the other.");
				return false;
			}
			BundleInfo value = null;
			if (m_Children.TryGetValue(oldName, out value))
			{
				m_Children.Remove(oldName);
				if (!string.IsNullOrEmpty(newName))
				{
					m_Children.Add(newName, value);
				}
			}
			return true;
		}

		internal override void Update()
		{
			m_Dirty = false;
			m_DoneUpdating = true;
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				child.Value.Update();
				m_Dirty |= child.Value.dirty;
				m_DoneUpdating &= child.Value.doneUpdating;
			}
			if (m_Dirty || m_DoneUpdating)
			{
				RefreshMessages();
			}
		}

		internal override List<AssetInfo> GetDependencies()
		{
			List<AssetInfo> list = new List<AssetInfo>();
			foreach (KeyValuePair<string, BundleInfo> child in m_Children)
			{
				list.AddRange(child.Value.GetDependencies());
			}
			return list;
		}
	}
}

using System.Collections.Generic;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal abstract class BundleInfo
	{
		protected BundleFolderInfo m_Parent;

		protected bool m_DoneUpdating;

		protected bool m_Dirty;

		internal BundleNameData m_Name;

		protected MessageSystem.MessageState m_BundleMessages = new MessageSystem.MessageState();

		protected MessageSystem.Message m_CachedHighMessage;

		internal BundleFolderInfo parent => m_Parent;

		internal virtual string displayName => m_Name.shortName;

		internal virtual int nameHashCode => m_Name.GetHashCode();

		internal virtual bool doneUpdating => m_DoneUpdating;

		internal virtual bool dirty => m_Dirty;

		internal BundleInfo(string name, BundleFolderInfo parent)
		{
			m_Name = new BundleNameData(name);
			m_Parent = parent;
		}

		internal abstract BundleTreeItem CreateTreeView(int depth);

		protected virtual void RefreshMessages()
		{
			RefreshEmptyStatus();
			RefreshDupeAssetWarning();
			MessageSystem.MessageFlag lookup = m_BundleMessages.HighestMessageFlag();
			m_CachedHighMessage = MessageSystem.GetMessage(lookup);
		}

		internal abstract bool RefreshEmptyStatus();

		internal abstract bool RefreshDupeAssetWarning();

		internal virtual MessageSystem.Message HighestMessage()
		{
			if (m_CachedHighMessage == null)
			{
				RefreshMessages();
			}
			return m_CachedHighMessage;
		}

		internal bool IsMessageSet(MessageSystem.MessageFlag flag)
		{
			return m_BundleMessages.IsSet(flag);
		}

		internal void SetMessageFlag(MessageSystem.MessageFlag flag, bool on)
		{
			m_BundleMessages.SetFlag(flag, on);
		}

		internal List<MessageSystem.Message> GetMessages()
		{
			return m_BundleMessages.GetMessages();
		}

		internal bool HasMessages()
		{
			return m_BundleMessages.HasMessages();
		}

		internal virtual bool HandleRename(string newName, int reverseDepth)
		{
			if (reverseDepth == 0 && !m_Parent.HandleChildRename(m_Name.shortName, newName))
			{
				return false;
			}
			m_Name.PartialNameChange(newName, reverseDepth);
			return true;
		}

		internal virtual void HandleDelete(bool isRootOfDelete, string forcedNewName = "", string forcedNewVariant = "")
		{
			if (isRootOfDelete)
			{
				m_Parent.HandleChildRename(m_Name.shortName, string.Empty);
			}
		}

		internal abstract void RefreshAssetList();

		internal abstract void AddAssetsToNode(AssetTreeItem node);

		internal abstract void Update();

		internal void ForceNeedUpdate()
		{
			m_DoneUpdating = false;
			m_Dirty = true;
		}

		internal abstract void HandleReparent(string parentName, BundleFolderInfo newParent = null);

		internal abstract List<AssetInfo> GetDependencies();

		internal abstract bool DoesItemMatchSearch(string search);
	}
}

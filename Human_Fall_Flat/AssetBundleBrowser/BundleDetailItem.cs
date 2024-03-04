using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace AssetBundleBrowser
{
	internal class BundleDetailItem : TreeViewItem
	{
		internal MessageType MessageLevel { get; set; }

		internal BundleDetailItem(int id, int depth, string displayName, MessageType type)
			: this(id, depth, displayName)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			MessageLevel = type;
		}
	}
}

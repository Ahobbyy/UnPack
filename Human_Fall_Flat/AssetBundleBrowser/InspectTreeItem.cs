using UnityEditor.IMGUI.Controls;

namespace AssetBundleBrowser
{
	internal class InspectTreeItem : TreeViewItem
	{
		internal string bundlePath { get; private set; }

		internal InspectTreeItem(string path, int depth)
			: this(path.GetHashCode(), depth, path)
		{
			bundlePath = path;
		}

		internal InspectTreeItem(string path, int depth, string prettyName)
			: this(path.GetHashCode(), depth, prettyName)
		{
			bundlePath = path;
		}
	}
}

using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class InspectBundleTree : TreeView
	{
		private AssetBundleInspectTab m_InspectTab;

		internal InspectBundleTree(TreeViewState s, AssetBundleInspectTab parent)
			: this(s)
		{
			m_InspectTab = parent;
			((TreeView)this).set_showBorder(true);
		}

		protected override TreeViewItem BuildRoot()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Expected O, but got Unknown
			TreeViewItem val = new TreeViewItem(-1, -1);
			val.set_children(new List<TreeViewItem>());
			if (m_InspectTab == null)
			{
				Debug.Log((object)"Unknown problem in AssetBundle Browser Inspect tab.  Restart Browser and try again, or file ticket on github.");
				return val;
			}
			foreach (KeyValuePair<string, List<string>> bundle in m_InspectTab.BundleList)
			{
				if (string.IsNullOrEmpty(bundle.Key))
				{
					foreach (string item in bundle.Value)
					{
						val.AddChild((TreeViewItem)(object)new InspectTreeItem(item, 0));
					}
					continue;
				}
				TreeViewItem val2 = new TreeViewItem(bundle.Key.GetHashCode(), 0, bundle.Key);
				foreach (string item2 in bundle.Value)
				{
					string prettyName = item2;
					if (item2.StartsWith(bundle.Key))
					{
						prettyName = item2.Remove(0, bundle.Key.Length + 1);
					}
					val2.AddChild((TreeViewItem)(object)new InspectTreeItem(item2, 1, prettyName));
				}
				val.AddChild(val2);
			}
			return val;
		}

		public override void OnGUI(Rect rect)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			((TreeView)this).OnGUI(rect);
			if ((int)Event.get_current().get_type() == 0 && Event.get_current().get_button() == 0 && ((Rect)(ref rect)).Contains(Event.get_current().get_mousePosition()))
			{
				((TreeView)this).SetSelection((IList<int>)new int[0], (TreeViewSelectionOptions)1);
			}
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			((TreeView)this).RowGUI(args);
			if (args.item.get_depth() != 0)
			{
				return;
			}
			int num = 16;
			if (!GUI.Button(new Rect(((Rect)(ref args.rowRect)).get_xMax() - (float)num, ((Rect)(ref args.rowRect)).get_y(), (float)num, ((Rect)(ref args.rowRect)).get_height()), "-"))
			{
				return;
			}
			if (((TreeView)this).GetSelection().Contains(args.item.get_id()))
			{
				foreach (int item in ((TreeView)this).GetSelection())
				{
					TreeViewItem val = ((TreeView)this).FindItem(item, ((TreeView)this).get_rootItem());
					if (val.get_depth() == 0)
					{
						RemoveItem(val);
					}
				}
			}
			else
			{
				RemoveItem(args.item);
			}
			m_InspectTab.RefreshBundles();
		}

		private void RemoveItem(TreeViewItem item)
		{
			InspectTreeItem inspectTreeItem = item as InspectTreeItem;
			if (inspectTreeItem != null)
			{
				m_InspectTab.RemoveBundlePath(inspectTreeItem.bundlePath);
			}
			else
			{
				m_InspectTab.RemoveBundleFolder(item.get_displayName());
			}
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			((TreeView)this).SelectionChanged(selectedIds);
			if (selectedIds == null)
			{
				return;
			}
			if (selectedIds.Count > 0)
			{
				m_InspectTab.SetBundleItem((from tvi in ((TreeView)this).FindRows(selectedIds)
					select tvi as InspectTreeItem).ToList());
			}
			else
			{
				m_InspectTab.SetBundleItem(null);
			}
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return true;
		}
	}
}

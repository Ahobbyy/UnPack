using System.Collections.Generic;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class BundleDetailList : TreeView
	{
		private HashSet<BundleDataInfo> m_Selecteditems;

		private Rect m_TotalRect;

		private const float k_DoubleIndent = 32f;

		private const string k_SizeHeader = "Size: ";

		private const string k_DependencyHeader = "Dependent On:";

		private const string k_DependencyEmpty = "Dependent On: - None";

		private const string k_MessageHeader = "Messages:";

		private const string k_MessageEmpty = "Messages: - None";

		internal BundleDetailList(TreeViewState state)
			: this(state)
		{
			m_Selecteditems = new HashSet<BundleDataInfo>();
			((TreeView)this).set_showBorder(true);
		}

		internal void Update()
		{
			bool flag = false;
			foreach (BundleDataInfo selecteditem in m_Selecteditems)
			{
				flag |= selecteditem.dirty;
			}
			if (flag)
			{
				((TreeView)this).Reload();
				((TreeView)this).ExpandAll();
			}
		}

		protected override TreeViewItem BuildRoot()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Expected O, but got Unknown
			TreeViewItem val = new TreeViewItem(-1, -1);
			val.set_children(new List<TreeViewItem>());
			if (m_Selecteditems != null)
			{
				foreach (BundleDataInfo selecteditem in m_Selecteditems)
				{
					val.AddChild(AppendBundleToTree(selecteditem));
				}
				return val;
			}
			return val;
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			if (args.item is BundleDetailItem)
			{
				EditorGUI.HelpBox(new Rect(((Rect)(ref args.rowRect)).get_x() + 32f, ((Rect)(ref args.rowRect)).get_y(), ((Rect)(ref args.rowRect)).get_width() - 32f, ((Rect)(ref args.rowRect)).get_height()), args.item.get_displayName(), (args.item as BundleDetailItem).MessageLevel);
				return;
			}
			Color color = GUI.get_color();
			if (args.item.get_depth() == 1 && (args.item.get_displayName() == "Messages: - None" || args.item.get_displayName() == "Dependent On: - None"))
			{
				GUI.set_color(Model.k_LightGrey);
			}
			((TreeView)this).RowGUI(args);
			GUI.set_color(color);
		}

		public override void OnGUI(Rect rect)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			m_TotalRect = rect;
			((TreeView)this).OnGUI(rect);
		}

		protected override float GetCustomRowHeight(int row, TreeViewItem item)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if (item is BundleDetailItem)
			{
				return DefaultStyles.backgroundEven.CalcHeight(new GUIContent(item.get_displayName()), ((Rect)(ref m_TotalRect)).get_width()) + 3f;
			}
			return ((TreeView)this).GetCustomRowHeight(row, item);
		}

		internal static TreeViewItem AppendBundleToTree(BundleDataInfo bundle)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Expected O, but got Unknown
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Expected O, but got Unknown
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Expected O, but got Unknown
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Expected O, but got Unknown
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Expected O, but got Unknown
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			string fullNativeName = bundle.m_Name.fullNativeName;
			TreeViewItem val = new TreeViewItem(fullNativeName.GetHashCode(), 0, fullNativeName);
			string text = fullNativeName + "Size: ";
			TreeViewItem val2 = new TreeViewItem(text.GetHashCode(), 1, "Size: " + bundle.TotalSize());
			text = fullNativeName + "Dependent On:";
			TreeViewItem val3 = new TreeViewItem(text.GetHashCode(), 1, "Dependent On: - None");
			if (bundle.GetBundleDependencies().Count > 0)
			{
				val3.set_displayName("Dependent On:");
				foreach (string bundleDependency in bundle.GetBundleDependencies())
				{
					text = fullNativeName + bundleDependency;
					val3.AddChild(new TreeViewItem(text.GetHashCode(), 2, bundleDependency));
				}
			}
			text = fullNativeName + "Messages:";
			TreeViewItem val4 = new TreeViewItem(text.GetHashCode(), 1, "Messages: - None");
			if (bundle.HasMessages())
			{
				val4.set_displayName("Messages:");
				foreach (MessageSystem.Message message in bundle.GetMessages())
				{
					text = fullNativeName + message.message;
					val4.AddChild((TreeViewItem)(object)new BundleDetailItem(text.GetHashCode(), 2, message.message, message.severity));
				}
			}
			val.AddChild(val2);
			val.AddChild(val3);
			val.AddChild(val4);
			return val;
		}

		internal void SetItems(IEnumerable<BundleInfo> items)
		{
			m_Selecteditems.Clear();
			foreach (BundleInfo item in items)
			{
				CollectBundles(item);
			}
			((TreeView)this).SetSelection((IList<int>)new List<int>());
			((TreeView)this).Reload();
			((TreeView)this).ExpandAll();
		}

		internal void CollectBundles(BundleInfo bundle)
		{
			BundleDataInfo bundleDataInfo = bundle as BundleDataInfo;
			if (bundleDataInfo != null)
			{
				m_Selecteditems.Add(bundleDataInfo);
				return;
			}
			foreach (BundleInfo child in (bundle as BundleFolderInfo).GetChildList())
			{
				CollectBundles(child);
			}
		}
	}
}

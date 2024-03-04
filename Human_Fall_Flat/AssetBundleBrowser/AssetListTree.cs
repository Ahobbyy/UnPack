using System.Collections.Generic;
using System.Linq;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class AssetListTree : TreeView
	{
		private enum MyColumns
		{
			Asset,
			Bundle,
			Size,
			Message
		}

		internal enum SortOption
		{
			Asset,
			Bundle,
			Size,
			Message
		}

		private List<BundleInfo> m_SourceBundles = new List<BundleInfo>();

		private AssetBundleManageTab m_Controller;

		private List<Object> m_EmptyObjectList = new List<Object>();

		private SortOption[] m_SortOptions = new SortOption[4]
		{
			SortOption.Asset,
			SortOption.Bundle,
			SortOption.Size,
			SortOption.Message
		};

		internal static MultiColumnHeaderState CreateDefaultMultiColumnHeaderState()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Expected O, but got Unknown
			return new MultiColumnHeaderState(GetColumns());
		}

		private static Column[] GetColumns()
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Expected O, but got Unknown
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Expected O, but got Unknown
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Expected O, but got Unknown
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Expected O, but got Unknown
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Expected O, but got Unknown
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Expected O, but got Unknown
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			Column[] obj = new Column[4]
			{
				new Column(),
				new Column(),
				new Column(),
				new Column()
			};
			obj[0].headerContent = new GUIContent("Asset", "Short name of asset. For full name select asset and see message below");
			obj[0].minWidth = 50f;
			obj[0].width = 100f;
			obj[0].maxWidth = 300f;
			obj[0].headerTextAlignment = (TextAlignment)0;
			obj[0].canSort = true;
			obj[0].autoResize = true;
			obj[1].headerContent = new GUIContent("Bundle", "Bundle name. 'auto' means asset was pulled in due to dependency");
			obj[1].minWidth = 50f;
			obj[1].width = 100f;
			obj[1].maxWidth = 300f;
			obj[1].headerTextAlignment = (TextAlignment)0;
			obj[1].canSort = true;
			obj[1].autoResize = true;
			obj[2].headerContent = new GUIContent("Size", "Size on disk");
			obj[2].minWidth = 30f;
			obj[2].width = 75f;
			obj[2].maxWidth = 100f;
			obj[2].headerTextAlignment = (TextAlignment)0;
			obj[2].canSort = true;
			obj[2].autoResize = true;
			obj[3].headerContent = new GUIContent("!", "Errors, Warnings, or Info");
			obj[3].minWidth = 16f;
			obj[3].width = 16f;
			obj[3].maxWidth = 16f;
			obj[3].headerTextAlignment = (TextAlignment)0;
			obj[3].canSort = true;
			obj[3].autoResize = false;
			return (Column[])(object)obj;
		}

		internal AssetListTree(TreeViewState state, MultiColumnHeaderState mchs, AssetBundleManageTab ctrl)
			: this(state, new MultiColumnHeader(mchs))
		{
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Expected O, but got Unknown
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Expected O, but got Unknown
			m_Controller = ctrl;
			((TreeView)this).set_showBorder(true);
			((TreeView)this).set_showAlternatingRowBackgrounds(true);
			((TreeView)this).get_multiColumnHeader().add_sortingChanged(new HeaderCallback(OnSortingChanged));
		}

		internal void Update()
		{
			bool flag = false;
			foreach (BundleInfo sourceBundle in m_SourceBundles)
			{
				flag |= sourceBundle.dirty;
			}
			if (flag)
			{
				((TreeView)this).Reload();
			}
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

		protected override IList<TreeViewItem> BuildRows(TreeViewItem root)
		{
			IList<TreeViewItem> list = ((TreeView)this).BuildRows(root);
			SortIfNeeded(root, list);
			return list;
		}

		internal void SetSelectedBundles(IEnumerable<BundleInfo> bundles)
		{
			m_Controller.SetSelectedItems(null);
			m_SourceBundles = bundles.ToList();
			((TreeView)this).SetSelection((IList<int>)new List<int>());
			((TreeView)this).Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			return (TreeViewItem)(object)Model.CreateAssetListTreeView(m_SourceBundles);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < ((RowGUIArgs)(ref args)).GetNumVisibleColumns(); i++)
			{
				CellGUI(((RowGUIArgs)(ref args)).GetCellRect(i), args.item as AssetTreeItem, ((RowGUIArgs)(ref args)).GetColumn(i), ref args);
			}
		}

		private void CellGUI(Rect cellRect, AssetTreeItem item, int column, ref RowGUIArgs args)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_014e: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			Color color = GUI.get_color();
			((TreeView)this).CenterRectUsingSingleLineHeight(ref cellRect);
			if (column != 3)
			{
				GUI.set_color(item.itemColor);
			}
			switch (column)
			{
			case 0:
			{
				Rect val2 = default(Rect);
				((Rect)(ref val2))._002Ector(((Rect)(ref cellRect)).get_x() + 1f, ((Rect)(ref cellRect)).get_y() + 1f, ((Rect)(ref cellRect)).get_height() - 2f, ((Rect)(ref cellRect)).get_height() - 2f);
				if ((Object)(object)((TreeViewItem)item).get_icon() != (Object)null)
				{
					GUI.DrawTexture(val2, (Texture)(object)((TreeViewItem)item).get_icon(), (ScaleMode)2);
				}
				DefaultGUI.Label(new Rect(((Rect)(ref cellRect)).get_x() + ((Rect)(ref val2)).get_xMax() + 1f, ((Rect)(ref cellRect)).get_y(), ((Rect)(ref cellRect)).get_width() - ((Rect)(ref val2)).get_width(), ((Rect)(ref cellRect)).get_height()), ((TreeViewItem)item).get_displayName(), args.selected, args.focused);
				break;
			}
			case 1:
				DefaultGUI.Label(cellRect, item.asset.bundleName, args.selected, args.focused);
				break;
			case 2:
				DefaultGUI.Label(cellRect, item.asset.GetSizeString(), args.selected, args.focused);
				break;
			case 3:
			{
				Texture2D val = item.MessageIcon();
				if ((Object)(object)val != (Object)null)
				{
					GUI.DrawTexture(new Rect(((Rect)(ref cellRect)).get_x(), ((Rect)(ref cellRect)).get_y(), ((Rect)(ref cellRect)).get_height(), ((Rect)(ref cellRect)).get_height()), (Texture)(object)val, (ScaleMode)2);
				}
				break;
			}
			}
			GUI.set_color(color);
		}

		protected override void DoubleClickedItem(int id)
		{
			AssetTreeItem assetTreeItem = ((TreeView)this).FindItem(id, ((TreeView)this).get_rootItem()) as AssetTreeItem;
			if (assetTreeItem != null)
			{
				Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetTreeItem.asset.fullAssetName);
				EditorGUIUtility.PingObject(obj);
				Selection.set_activeObject(obj);
			}
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			if (selectedIds == null)
			{
				return;
			}
			List<Object> list = new List<Object>();
			List<AssetInfo> list2 = new List<AssetInfo>();
			foreach (int selectedId in selectedIds)
			{
				AssetTreeItem assetTreeItem = ((TreeView)this).FindItem(selectedId, ((TreeView)this).get_rootItem()) as AssetTreeItem;
				if (assetTreeItem != null)
				{
					Object val = AssetDatabase.LoadAssetAtPath<Object>(assetTreeItem.asset.fullAssetName);
					list.Add(val);
					Selection.set_activeObject(val);
					list2.Add(assetTreeItem.asset);
				}
			}
			m_Controller.SetSelectedItems(list2);
			Selection.set_objects(list.ToArray());
		}

		protected override bool CanBeParent(TreeViewItem item)
		{
			return false;
		}

		protected override bool CanStartDrag(CanStartDragArgs args)
		{
			args.draggedItemIDs = ((TreeView)this).GetSelection();
			return true;
		}

		protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			DragAndDrop.PrepareStartDrag();
			DragAndDrop.set_objectReferences(m_EmptyObjectList.ToArray());
			DragAndDrop.set_paths(new List<AssetTreeItem>(args.draggedItemIDs.Select((int id) => ((TreeView)this).FindItem(id, ((TreeView)this).get_rootItem()) as AssetTreeItem)).Select((AssetTreeItem a) => a.asset.fullAssetName).ToArray());
			DragAndDrop.SetGenericData("AssetListTreeSource", (object)this);
			DragAndDrop.StartDrag("AssetListTree");
		}

		protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			if (IsValidDragDrop())
			{
				if (args.performDrop)
				{
					Model.MoveAssetToBundle(DragAndDrop.get_paths(), m_SourceBundles[0].m_Name.bundleName, m_SourceBundles[0].m_Name.variant);
					Model.ExecuteAssetMove();
					foreach (BundleInfo sourceBundle in m_SourceBundles)
					{
						sourceBundle.RefreshAssetList();
					}
					m_Controller.UpdateSelectedBundles(m_SourceBundles);
				}
				return (DragAndDropVisualMode)1;
			}
			return (DragAndDropVisualMode)32;
		}

		protected bool IsValidDragDrop()
		{
			if (Model.DataSource.IsReadOnly())
			{
				return false;
			}
			if (m_SourceBundles.Count == 0 || m_SourceBundles.Count > 1)
			{
				return false;
			}
			if (DragAndDrop.get_paths() == null || DragAndDrop.get_paths().Length == 0)
			{
				return false;
			}
			if (m_SourceBundles[0] is BundleFolderInfo)
			{
				return false;
			}
			BundleDataInfo bundleDataInfo = m_SourceBundles[0] as BundleDataInfo;
			if (bundleDataInfo == null)
			{
				return false;
			}
			if (DragAndDrop.GetGenericData("AssetListTreeSource") is AssetListTree)
			{
				return false;
			}
			if (bundleDataInfo.IsEmpty())
			{
				return true;
			}
			if (bundleDataInfo.isSceneBundle)
			{
				string[] paths = DragAndDrop.get_paths();
				foreach (string text in paths)
				{
					if (AssetDatabase.GetMainAssetTypeAtPath(text) != typeof(SceneAsset) && !AssetDatabase.IsValidFolder(text))
					{
						return false;
					}
				}
			}
			else
			{
				string[] paths = DragAndDrop.get_paths();
				for (int i = 0; i < paths.Length; i++)
				{
					if (AssetDatabase.GetMainAssetTypeAtPath(paths[i]) == typeof(SceneAsset))
					{
						return false;
					}
				}
			}
			return true;
		}

		protected override void ContextClickedItem(int id)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Expected O, but got Unknown
			//IL_0080: Expected O, but got Unknown
			if (Model.DataSource.IsReadOnly())
			{
				return;
			}
			List<AssetTreeItem> list = new List<AssetTreeItem>();
			foreach (int item in ((TreeView)this).GetSelection())
			{
				list.Add(((TreeView)this).FindItem(item, ((TreeView)this).get_rootItem()) as AssetTreeItem);
			}
			if (list.Count > 0)
			{
				GenericMenu val = new GenericMenu();
				val.AddItem(new GUIContent("Remove asset(s) from bundle."), false, new MenuFunction2(RemoveAssets), (object)list);
				val.ShowAsContext();
			}
		}

		private void RemoveAssets(object obj)
		{
			List<AssetTreeItem> obj2 = obj as List<AssetTreeItem>;
			List<AssetInfo> list = new List<AssetInfo>();
			foreach (AssetTreeItem item in obj2)
			{
				if (!string.IsNullOrEmpty(item.asset.bundleName))
				{
					list.Add(item.asset);
				}
			}
			Model.MoveAssetToBundle(list, string.Empty, string.Empty);
			Model.ExecuteAssetMove();
			foreach (BundleInfo sourceBundle in m_SourceBundles)
			{
				sourceBundle.RefreshAssetList();
			}
			m_Controller.UpdateSelectedBundles(m_SourceBundles);
		}

		protected override void KeyEvent()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Invalid comparison between Unknown and I4
			if (m_SourceBundles.Count <= 0 || (int)Event.get_current().get_keyCode() != 127 || ((TreeView)this).GetSelection().Count <= 0)
			{
				return;
			}
			List<AssetTreeItem> list = new List<AssetTreeItem>();
			foreach (int item in ((TreeView)this).GetSelection())
			{
				list.Add(((TreeView)this).FindItem(item, ((TreeView)this).get_rootItem()) as AssetTreeItem);
			}
			RemoveAssets(list);
		}

		private void OnSortingChanged(MultiColumnHeader multiColumnHeader)
		{
			SortIfNeeded(((TreeView)this).get_rootItem(), ((TreeView)this).GetRows());
		}

		private void SortIfNeeded(TreeViewItem root, IList<TreeViewItem> rows)
		{
			if (rows.Count > 1 && ((TreeView)this).get_multiColumnHeader().get_sortedColumnIndex() != -1)
			{
				SortByColumn();
				rows.Clear();
				for (int i = 0; i < root.get_children().Count; i++)
				{
					rows.Add(root.get_children()[i]);
				}
				((TreeView)this).Repaint();
			}
		}

		private void SortByColumn()
		{
			int[] sortedColumns = ((TreeView)this).get_multiColumnHeader().get_state().get_sortedColumns();
			if (sortedColumns.Length == 0)
			{
				return;
			}
			List<AssetTreeItem> list = new List<AssetTreeItem>();
			foreach (TreeViewItem child in ((TreeView)this).get_rootItem().get_children())
			{
				list.Add(child as AssetTreeItem);
			}
			IOrderedEnumerable<AssetTreeItem> source = InitialOrder(list, sortedColumns);
			((TreeView)this).get_rootItem().set_children(source.Cast<TreeViewItem>().ToList());
		}

		private IOrderedEnumerable<AssetTreeItem> InitialOrder(IEnumerable<AssetTreeItem> myTypes, int[] columnList)
		{
			SortOption sortOption = m_SortOptions[columnList[0]];
			bool ascending = ((TreeView)this).get_multiColumnHeader().IsSortedAscending(columnList[0]);
			return sortOption switch
			{
				SortOption.Asset => myTypes.Order((AssetTreeItem l) => ((TreeViewItem)l).get_displayName(), ascending), 
				SortOption.Size => myTypes.Order((AssetTreeItem l) => l.asset.fileSize, ascending), 
				SortOption.Message => myTypes.Order((AssetTreeItem l) => l.HighestMessageLevel(), ascending), 
				_ => myTypes.Order((AssetTreeItem l) => l.asset.bundleName, ascending), 
			};
		}

		private void ReloadAndSelect(IList<int> hashCodes)
		{
			((TreeView)this).Reload();
			((TreeView)this).SetSelection(hashCodes);
			((TreeView)this).SelectionChanged(hashCodes);
		}
	}
}

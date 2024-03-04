using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class AssetBundleTree : TreeView
	{
		private class DragAndDropData
		{
			internal bool hasBundleFolder;

			internal bool hasScene;

			internal bool hasNonScene;

			internal bool hasVariantChild;

			internal List<BundleInfo> draggedNodes;

			internal BundleTreeItem targetNode;

			internal DragAndDropArgs args;

			internal string[] paths;

			internal DragAndDropData(DragAndDropArgs a)
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_0008: Unknown result type (might be due to invalid IL or missing references)
				args = a;
				draggedNodes = DragAndDrop.GetGenericData("AssetBundleModel.BundleInfo") as List<BundleInfo>;
				targetNode = args.parentItem as BundleTreeItem;
				paths = DragAndDrop.get_paths();
				if (draggedNodes != null)
				{
					foreach (BundleInfo draggedNode in draggedNodes)
					{
						if (draggedNode is BundleFolderInfo)
						{
							hasBundleFolder = true;
							continue;
						}
						BundleDataInfo bundleDataInfo = draggedNode as BundleDataInfo;
						if (bundleDataInfo != null)
						{
							if (bundleDataInfo.isSceneBundle)
							{
								hasScene = true;
							}
							else
							{
								hasNonScene = true;
							}
							if (bundleDataInfo is BundleVariantDataInfo)
							{
								hasVariantChild = true;
							}
						}
					}
				}
				else
				{
					if (DragAndDrop.get_paths() == null)
					{
						return;
					}
					string[] array = DragAndDrop.get_paths();
					for (int i = 0; i < array.Length; i++)
					{
						if (AssetDatabase.GetMainAssetTypeAtPath(array[i]) == typeof(SceneAsset))
						{
							hasScene = true;
						}
						else
						{
							hasNonScene = true;
						}
					}
				}
			}
		}

		private AssetBundleManageTab m_Controller;

		private bool m_ContextOnItem;

		private List<Object> m_EmptyObjectList = new List<Object>();

		private string[] dragToNewSpacePaths;

		private BundleFolderInfo dragToNewSpaceRoot;

		internal AssetBundleTree(TreeViewState state, AssetBundleManageTab ctrl)
			: this(state)
		{
			Model.Rebuild();
			m_Controller = ctrl;
			((TreeView)this).set_showBorder(true);
		}

		protected override bool CanMultiSelect(TreeViewItem item)
		{
			return true;
		}

		protected override bool CanRename(TreeViewItem item)
		{
			if (item != null)
			{
				return item.get_displayName().Length > 0;
			}
			return false;
		}

		protected override bool DoesItemMatchSearch(TreeViewItem item, string search)
		{
			return (item as BundleTreeItem).bundle.DoesItemMatchSearch(search);
		}

		protected override void RowGUI(RowGUIArgs args)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			BundleTreeItem bundleTreeItem = args.item as BundleTreeItem;
			if ((Object)(object)args.item.get_icon() == (Object)null)
			{
				((TreeView)this).set_extraSpaceBeforeIconAndLabel(16f);
			}
			else
			{
				((TreeView)this).set_extraSpaceBeforeIconAndLabel(0f);
			}
			Color color = GUI.get_color();
			if (bundleTreeItem.bundle is BundleVariantFolderInfo)
			{
				GUI.set_color(Model.k_LightGrey);
			}
			((TreeView)this).RowGUI(args);
			GUI.set_color(color);
			MessageSystem.Message message = bundleTreeItem.BundleMessage();
			if ((int)message.severity != 0)
			{
				float height = ((Rect)(ref args.rowRect)).get_height();
				float xMax = ((Rect)(ref args.rowRect)).get_xMax();
				GUI.Label(new Rect(xMax - height, ((Rect)(ref args.rowRect)).get_yMin(), height, height), new GUIContent((Texture)(object)message.icon, message.message));
			}
		}

		protected override void RenameEnded(RenameEndedArgs args)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			((TreeView)this).RenameEnded(args);
			if (args.newName.Length > 0 && args.newName != args.originalName)
			{
				args.newName = args.newName.ToLower();
				args.acceptedRename = true;
				BundleTreeItem bundleTreeItem = ((TreeView)this).FindItem(args.itemID, ((TreeView)this).get_rootItem()) as BundleTreeItem;
				args.acceptedRename = Model.HandleBundleRename(bundleTreeItem, args.newName);
				ReloadAndSelect(bundleTreeItem.bundle.nameHashCode, rename: false);
			}
			else
			{
				args.acceptedRename = false;
			}
		}

		protected override TreeViewItem BuildRoot()
		{
			Model.Refresh();
			return (TreeViewItem)(object)Model.CreateBundleTreeView();
		}

		protected override void SelectionChanged(IList<int> selectedIds)
		{
			List<BundleInfo> list = new List<BundleInfo>();
			if (selectedIds != null)
			{
				foreach (int selectedId in selectedIds)
				{
					BundleTreeItem bundleTreeItem = ((TreeView)this).FindItem(selectedId, ((TreeView)this).get_rootItem()) as BundleTreeItem;
					if (bundleTreeItem != null && bundleTreeItem.bundle != null)
					{
						bundleTreeItem.bundle.RefreshAssetList();
						list.Add(bundleTreeItem.bundle);
					}
				}
			}
			m_Controller.UpdateSelectedBundles(list);
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

		protected override void ContextClicked()
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Expected O, but got Unknown
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Expected O, but got Unknown
			//IL_0046: Expected O, but got Unknown
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Expected O, but got Unknown
			//IL_0064: Expected O, but got Unknown
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_0082: Expected O, but got Unknown
			if (m_ContextOnItem)
			{
				m_ContextOnItem = false;
				return;
			}
			List<BundleTreeItem> list = new List<BundleTreeItem>();
			GenericMenu val = new GenericMenu();
			if (!Model.DataSource.IsReadOnly())
			{
				val.AddItem(new GUIContent("Add new bundle"), false, new MenuFunction2(CreateNewBundle), (object)list);
				val.AddItem(new GUIContent("Add new folder"), false, new MenuFunction2(CreateFolder), (object)list);
			}
			val.AddItem(new GUIContent("Reload all data"), false, new MenuFunction2(ForceReloadData), (object)list);
			val.ShowAsContext();
		}

		protected override void ContextClickedItem(int id)
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_009e: Expected O, but got Unknown
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_00bc: Expected O, but got Unknown
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Expected O, but got Unknown
			//IL_00da: Expected O, but got Unknown
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Expected O, but got Unknown
			//IL_00f8: Expected O, but got Unknown
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Expected O, but got Unknown
			//IL_012e: Expected O, but got Unknown
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			//IL_014c: Expected O, but got Unknown
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Expected O, but got Unknown
			//IL_016a: Expected O, but got Unknown
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Expected O, but got Unknown
			//IL_01a0: Expected O, but got Unknown
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Expected O, but got Unknown
			//IL_01be: Expected O, but got Unknown
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Expected O, but got Unknown
			//IL_01dc: Expected O, but got Unknown
			//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Expected O, but got Unknown
			//IL_01fc: Expected O, but got Unknown
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0227: Unknown result type (might be due to invalid IL or missing references)
			//IL_0232: Expected O, but got Unknown
			//IL_0232: Expected O, but got Unknown
			//IL_0238: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_0250: Expected O, but got Unknown
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_0274: Unknown result type (might be due to invalid IL or missing references)
			//IL_027f: Expected O, but got Unknown
			//IL_027f: Expected O, but got Unknown
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029d: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Expected O, but got Unknown
			//IL_02a8: Expected O, but got Unknown
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c6: Expected O, but got Unknown
			//IL_02c6: Expected O, but got Unknown
			//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fd: Expected O, but got Unknown
			//IL_02fd: Expected O, but got Unknown
			if (Model.DataSource.IsReadOnly())
			{
				return;
			}
			m_ContextOnItem = true;
			List<BundleTreeItem> list = new List<BundleTreeItem>();
			foreach (int item in ((TreeView)this).GetSelection())
			{
				list.Add(((TreeView)this).FindItem(item, ((TreeView)this).get_rootItem()) as BundleTreeItem);
			}
			GenericMenu val = new GenericMenu();
			if (list.Count == 1)
			{
				if (list[0].bundle is BundleFolderConcreteInfo)
				{
					val.AddItem(new GUIContent("Add Child/New Bundle"), false, new MenuFunction2(CreateNewBundle), (object)list);
					val.AddItem(new GUIContent("Add Child/New Folder"), false, new MenuFunction2(CreateFolder), (object)list);
					val.AddItem(new GUIContent("Add Sibling/New Bundle"), false, new MenuFunction2(CreateNewSiblingBundle), (object)list);
					val.AddItem(new GUIContent("Add Sibling/New Folder"), false, new MenuFunction2(CreateNewSiblingFolder), (object)list);
				}
				else if (list[0].bundle is BundleVariantFolderInfo)
				{
					val.AddItem(new GUIContent("Add Child/New Variant"), false, new MenuFunction2(CreateNewVariant), (object)list);
					val.AddItem(new GUIContent("Add Sibling/New Bundle"), false, new MenuFunction2(CreateNewSiblingBundle), (object)list);
					val.AddItem(new GUIContent("Add Sibling/New Folder"), false, new MenuFunction2(CreateNewSiblingFolder), (object)list);
				}
				else if (!(list[0].bundle is BundleVariantDataInfo))
				{
					val.AddItem(new GUIContent("Add Sibling/New Bundle"), false, new MenuFunction2(CreateNewSiblingBundle), (object)list);
					val.AddItem(new GUIContent("Add Sibling/New Folder"), false, new MenuFunction2(CreateNewSiblingFolder), (object)list);
					val.AddItem(new GUIContent("Convert to variant"), false, new MenuFunction2(ConvertToVariant), (object)list);
				}
				else
				{
					val.AddItem(new GUIContent("Add Sibling/New Variant"), false, new MenuFunction2(CreateNewSiblingVariant), (object)list);
				}
				if (list[0].bundle.IsMessageSet(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles))
				{
					val.AddItem(new GUIContent("Move duplicates to new bundle"), false, new MenuFunction2(DedupeAllBundles), (object)list);
				}
				val.AddItem(new GUIContent("Rename"), false, new MenuFunction2(RenameBundle), (object)list);
				val.AddItem(new GUIContent("Delete " + ((TreeViewItem)list[0]).get_displayName()), false, new MenuFunction2(DeleteBundles), (object)list);
			}
			else if (list.Count > 1)
			{
				val.AddItem(new GUIContent("Move duplicates shared by selected"), false, new MenuFunction2(DedupeOverlappedBundles), (object)list);
				val.AddItem(new GUIContent("Move duplicates existing in any selected"), false, new MenuFunction2(DedupeAllBundles), (object)list);
				val.AddItem(new GUIContent("Delete " + list.Count + " selected bundles"), false, new MenuFunction2(DeleteBundles), (object)list);
			}
			val.ShowAsContext();
		}

		private void ForceReloadData(object context)
		{
			Model.ForceReloadData((TreeView)(object)this);
		}

		private void CreateNewSiblingFolder(object context)
		{
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				BundleFolderConcreteInfo bundleFolderConcreteInfo = null;
				bundleFolderConcreteInfo = list[0].bundle.parent as BundleFolderConcreteInfo;
				CreateFolderUnderParent(bundleFolderConcreteInfo);
			}
			else
			{
				Debug.LogError((object)"could not add 'sibling' with no bundles selected");
			}
		}

		private void CreateFolder(object context)
		{
			BundleFolderConcreteInfo folder = null;
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				folder = list[0].bundle as BundleFolderConcreteInfo;
			}
			CreateFolderUnderParent(folder);
		}

		private void CreateFolderUnderParent(BundleFolderConcreteInfo folder)
		{
			BundleFolderInfo bundleFolderInfo = Model.CreateEmptyBundleFolder(folder);
			ReloadAndSelect(bundleFolderInfo.nameHashCode, rename: true);
		}

		private void RenameBundle(object context)
		{
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				((TreeView)this).BeginRename(((TreeView)this).FindItem(list[0].bundle.nameHashCode, ((TreeView)this).get_rootItem()));
			}
		}

		private void CreateNewSiblingBundle(object context)
		{
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				BundleFolderConcreteInfo bundleFolderConcreteInfo = null;
				bundleFolderConcreteInfo = list[0].bundle.parent as BundleFolderConcreteInfo;
				CreateBundleUnderParent(bundleFolderConcreteInfo);
			}
			else
			{
				Debug.LogError((object)"could not add 'sibling' with no bundles selected");
			}
		}

		private void CreateNewBundle(object context)
		{
			BundleFolderConcreteInfo folder = null;
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				folder = list[0].bundle as BundleFolderConcreteInfo;
			}
			CreateBundleUnderParent(folder);
		}

		private void CreateBundleUnderParent(BundleFolderInfo folder)
		{
			BundleInfo bundleInfo = Model.CreateEmptyBundle(folder);
			ReloadAndSelect(bundleInfo.nameHashCode, rename: true);
		}

		private void CreateNewSiblingVariant(object context)
		{
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count > 0)
			{
				BundleVariantFolderInfo bundleVariantFolderInfo = null;
				bundleVariantFolderInfo = list[0].bundle.parent as BundleVariantFolderInfo;
				CreateVariantUnderParent(bundleVariantFolderInfo);
			}
			else
			{
				Debug.LogError((object)"could not add 'sibling' with no bundles selected");
			}
		}

		private void CreateNewVariant(object context)
		{
			BundleVariantFolderInfo bundleVariantFolderInfo = null;
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list != null && list.Count == 1)
			{
				bundleVariantFolderInfo = list[0].bundle as BundleVariantFolderInfo;
				CreateVariantUnderParent(bundleVariantFolderInfo);
			}
		}

		private void CreateVariantUnderParent(BundleVariantFolderInfo folder)
		{
			if (folder != null)
			{
				BundleInfo bundleInfo = Model.CreateEmptyVariant(folder);
				ReloadAndSelect(bundleInfo.nameHashCode, rename: true);
			}
		}

		private void ConvertToVariant(object context)
		{
			List<BundleTreeItem> list = context as List<BundleTreeItem>;
			if (list.Count == 1)
			{
				BundleInfo bundleInfo = Model.HandleConvertToVariant(list[0].bundle as BundleDataInfo);
				int hashCode = 0;
				if (bundleInfo != null)
				{
					hashCode = bundleInfo.nameHashCode;
				}
				ReloadAndSelect(hashCode, rename: true);
			}
		}

		private void DedupeOverlappedBundles(object context)
		{
			DedupeBundles(context, onlyOverlappedAssets: true);
		}

		private void DedupeAllBundles(object context)
		{
			DedupeBundles(context, onlyOverlappedAssets: false);
		}

		private void DedupeBundles(object context, bool onlyOverlappedAssets)
		{
			BundleInfo bundleInfo = Model.HandleDedupeBundles((context as List<BundleTreeItem>).Select((BundleTreeItem item) => item.bundle), onlyOverlappedAssets);
			if (bundleInfo != null)
			{
				List<int> list = new List<int>();
				list.Add(bundleInfo.nameHashCode);
				ReloadAndSelect(list);
			}
			else if (onlyOverlappedAssets)
			{
				Debug.LogWarning((object)"There were no duplicated assets that existed across all selected bundles.");
			}
			else
			{
				Debug.LogWarning((object)"No duplicate assets found after refreshing bundle contents.");
			}
		}

		private void DeleteBundles(object b)
		{
			Model.HandleBundleDelete((b as List<BundleTreeItem>).Select((BundleTreeItem item) => item.bundle));
			ReloadAndSelect(new List<int>());
		}

		protected override void KeyEvent()
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Invalid comparison between Unknown and I4
			if ((int)Event.get_current().get_keyCode() != 127 || ((TreeView)this).GetSelection().Count <= 0)
			{
				return;
			}
			List<BundleTreeItem> list = new List<BundleTreeItem>();
			foreach (int item in ((TreeView)this).GetSelection())
			{
				list.Add(((TreeView)this).FindItem(item, ((TreeView)this).get_rootItem()) as BundleTreeItem);
			}
			DeleteBundles(list);
		}

		protected override DragAndDropVisualMode HandleDragAndDrop(DragAndDropArgs args)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected I4, but got Unknown
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			DragAndDropVisualMode result = (DragAndDropVisualMode)0;
			DragAndDropData dragAndDropData = new DragAndDropData(args);
			if (Model.DataSource.IsReadOnly())
			{
				return (DragAndDropVisualMode)32;
			}
			if ((dragAndDropData.hasScene && dragAndDropData.hasNonScene) || dragAndDropData.hasVariantChild)
			{
				return (DragAndDropVisualMode)32;
			}
			DragAndDropPosition dragAndDropPosition = args.dragAndDropPosition;
			switch ((int)dragAndDropPosition)
			{
			case 0:
				result = HandleDragDropUpon(dragAndDropData);
				break;
			case 1:
				result = HandleDragDropBetween(dragAndDropData);
				break;
			case 2:
				if (dragAndDropData.draggedNodes != null)
				{
					result = (DragAndDropVisualMode)1;
					if (dragAndDropData.args.performDrop)
					{
						Model.HandleBundleReparent(dragAndDropData.draggedNodes, null);
						((TreeView)this).Reload();
					}
				}
				else if (dragAndDropData.paths != null)
				{
					result = (DragAndDropVisualMode)1;
					if (dragAndDropData.args.performDrop)
					{
						DragPathsToNewSpace(dragAndDropData.paths, null);
					}
				}
				break;
			}
			return result;
		}

		private DragAndDropVisualMode HandleDragDropUpon(DragAndDropData data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			DragAndDropVisualMode result = (DragAndDropVisualMode)1;
			BundleDataInfo bundleDataInfo = data.targetNode.bundle as BundleDataInfo;
			if (bundleDataInfo != null)
			{
				if (bundleDataInfo.isSceneBundle)
				{
					if (data.hasNonScene)
					{
						return (DragAndDropVisualMode)32;
					}
				}
				else
				{
					if (data.hasBundleFolder)
					{
						return (DragAndDropVisualMode)32;
					}
					if (data.hasScene && !bundleDataInfo.IsEmpty())
					{
						return (DragAndDropVisualMode)32;
					}
				}
				if (data.args.performDrop)
				{
					if (data.draggedNodes != null)
					{
						Model.HandleBundleMerge(data.draggedNodes, bundleDataInfo);
						ReloadAndSelect(bundleDataInfo.nameHashCode, rename: false);
					}
					else if (data.paths != null)
					{
						Model.MoveAssetToBundle(data.paths, bundleDataInfo.m_Name.bundleName, bundleDataInfo.m_Name.variant);
						Model.ExecuteAssetMove();
						ReloadAndSelect(bundleDataInfo.nameHashCode, rename: false);
					}
				}
			}
			else
			{
				BundleFolderInfo bundleFolderInfo = data.targetNode.bundle as BundleFolderInfo;
				if (bundleFolderInfo != null)
				{
					if (data.args.performDrop)
					{
						if (data.draggedNodes != null)
						{
							Model.HandleBundleReparent(data.draggedNodes, bundleFolderInfo);
							((TreeView)this).Reload();
						}
						else if (data.paths != null)
						{
							DragPathsToNewSpace(data.paths, bundleFolderInfo);
						}
					}
				}
				else
				{
					result = (DragAndDropVisualMode)32;
				}
			}
			return result;
		}

		private DragAndDropVisualMode HandleDragDropBetween(DragAndDropData data)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			DragAndDropVisualMode result = (DragAndDropVisualMode)1;
			BundleTreeItem bundleTreeItem = data.args.parentItem as BundleTreeItem;
			if (bundleTreeItem != null)
			{
				if (bundleTreeItem.bundle is BundleVariantFolderInfo)
				{
					return (DragAndDropVisualMode)32;
				}
				if (data.args.performDrop)
				{
					BundleFolderConcreteInfo bundleFolderConcreteInfo = bundleTreeItem.bundle as BundleFolderConcreteInfo;
					if (bundleFolderConcreteInfo != null)
					{
						if (data.draggedNodes != null)
						{
							Model.HandleBundleReparent(data.draggedNodes, bundleFolderConcreteInfo);
							((TreeView)this).Reload();
						}
						else if (data.paths != null)
						{
							DragPathsToNewSpace(data.paths, bundleFolderConcreteInfo);
						}
					}
				}
			}
			return result;
		}

		private void DragPathsAsOneBundle()
		{
			BundleInfo bundleInfo = Model.CreateEmptyBundle(dragToNewSpaceRoot);
			Model.MoveAssetToBundle(dragToNewSpacePaths, bundleInfo.m_Name.bundleName, bundleInfo.m_Name.variant);
			Model.ExecuteAssetMove();
			ReloadAndSelect(bundleInfo.nameHashCode, rename: true);
		}

		private void DragPathsAsManyBundles()
		{
			List<int> list = new List<int>();
			string[] array = dragToNewSpacePaths;
			foreach (string text in array)
			{
				BundleInfo bundleInfo = Model.CreateEmptyBundle(dragToNewSpaceRoot, Path.GetFileNameWithoutExtension(text).ToLower());
				Model.MoveAssetToBundle(text, bundleInfo.m_Name.bundleName, bundleInfo.m_Name.variant);
				list.Add(bundleInfo.nameHashCode);
			}
			Model.ExecuteAssetMove();
			ReloadAndSelect(list);
		}

		private void DragPathsToNewSpace(string[] paths, BundleFolderInfo root)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Expected O, but got Unknown
			//IL_0036: Expected O, but got Unknown
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_0073: Expected O, but got Unknown
			dragToNewSpacePaths = paths;
			dragToNewSpaceRoot = root;
			if (paths.Length > 1)
			{
				GenericMenu val = new GenericMenu();
				val.AddItem(new GUIContent("Create 1 Bundle"), false, new MenuFunction(DragPathsAsOneBundle));
				string text = "Create ";
				text += paths.Length;
				text += " Bundles";
				val.AddItem(new GUIContent(text), false, new MenuFunction(DragPathsAsManyBundles));
				val.ShowAsContext();
			}
			else
			{
				DragPathsAsManyBundles();
			}
		}

		protected override void SetupDragAndDrop(SetupDragAndDropArgs args)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			if (args.draggedItemIDs == null)
			{
				return;
			}
			DragAndDrop.PrepareStartDrag();
			List<BundleInfo> list = new List<BundleInfo>();
			foreach (int draggedItemID in args.draggedItemIDs)
			{
				BundleTreeItem bundleTreeItem = ((TreeView)this).FindItem(draggedItemID, ((TreeView)this).get_rootItem()) as BundleTreeItem;
				list.Add(bundleTreeItem.bundle);
			}
			DragAndDrop.set_paths((string[])null);
			DragAndDrop.set_objectReferences(m_EmptyObjectList.ToArray());
			DragAndDrop.SetGenericData("AssetBundleModel.BundleInfo", (object)list);
			DragAndDrop.set_visualMode((DragAndDropVisualMode)1);
			DragAndDrop.StartDrag("AssetBundleTree");
		}

		protected override bool CanStartDrag(CanStartDragArgs args)
		{
			return true;
		}

		internal void Refresh()
		{
			IList<int> selection = ((TreeView)this).GetSelection();
			((TreeView)this).Reload();
			((TreeView)this).SelectionChanged(selection);
		}

		private void ReloadAndSelect(int hashCode, bool rename)
		{
			List<int> list = new List<int>();
			list.Add(hashCode);
			ReloadAndSelect(list);
			if (rename)
			{
				((TreeView)this).BeginRename(((TreeView)this).FindItem(hashCode, ((TreeView)this).get_rootItem()), 0.25f);
			}
		}

		private void ReloadAndSelect(IList<int> hashCodes)
		{
			((TreeView)this).Reload();
			((TreeView)this).SetSelection(hashCodes, (TreeViewSelectionOptions)2);
			((TreeView)this).SelectionChanged(hashCodes);
		}
	}
}

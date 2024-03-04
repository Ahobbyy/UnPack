using System;
using System.Collections.Generic;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	[Serializable]
	internal class AssetBundleManageTab
	{
		[SerializeField]
		private TreeViewState m_BundleTreeState;

		[SerializeField]
		private TreeViewState m_AssetListState;

		[SerializeField]
		private MultiColumnHeaderState m_AssetListMCHState;

		[SerializeField]
		private TreeViewState m_BundleDetailState;

		private Rect m_Position;

		private AssetBundleTree m_BundleTree;

		private AssetListTree m_AssetList;

		private MessageList m_MessageList;

		private BundleDetailList m_DetailsList;

		private bool m_ResizingHorizontalSplitter;

		private bool m_ResizingVerticalSplitterRight;

		private bool m_ResizingVerticalSplitterLeft;

		private Rect m_HorizontalSplitterRect;

		private Rect m_VerticalSplitterRectRight;

		private Rect m_VerticalSplitterRectLeft;

		[SerializeField]
		private float m_HorizontalSplitterPercent;

		[SerializeField]
		private float m_VerticalSplitterPercentRight;

		[SerializeField]
		private float m_VerticalSplitterPercentLeft;

		private const float k_SplitterWidth = 3f;

		private static float s_UpdateDelay;

		private SearchField m_searchField;

		private EditorWindow m_Parent;

		public bool hasSearch => ((TreeView)m_BundleTree).get_hasSearch();

		internal AssetBundleManageTab()
		{
			m_HorizontalSplitterPercent = 0.4f;
			m_VerticalSplitterPercentRight = 0.7f;
			m_VerticalSplitterPercentLeft = 0.85f;
		}

		internal void OnEnable(Rect pos, EditorWindow parent)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Expected O, but got Unknown
			m_Parent = parent;
			m_Position = pos;
			m_HorizontalSplitterRect = new Rect((float)(int)(((Rect)(ref m_Position)).get_x() + ((Rect)(ref m_Position)).get_width() * m_HorizontalSplitterPercent), ((Rect)(ref m_Position)).get_y(), 3f, ((Rect)(ref m_Position)).get_height());
			m_VerticalSplitterRectRight = new Rect(((Rect)(ref m_HorizontalSplitterRect)).get_x(), (float)(int)(((Rect)(ref m_Position)).get_y() + ((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentRight), ((Rect)(ref m_Position)).get_width() - ((Rect)(ref m_HorizontalSplitterRect)).get_width() - 3f, 3f);
			m_VerticalSplitterRectLeft = new Rect(((Rect)(ref m_Position)).get_x(), (float)(int)(((Rect)(ref m_Position)).get_y() + ((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentLeft), ((Rect)(ref m_HorizontalSplitterRect)).get_width() - 3f, 3f);
			m_searchField = new SearchField();
		}

		internal void Update()
		{
			float realtimeSinceStartup = Time.get_realtimeSinceStartup();
			if (realtimeSinceStartup - s_UpdateDelay > 0.1f || s_UpdateDelay > realtimeSinceStartup)
			{
				s_UpdateDelay = realtimeSinceStartup - 0.001f;
				if (Model.Update())
				{
					m_Parent.Repaint();
				}
				if (m_DetailsList != null)
				{
					m_DetailsList.Update();
				}
				if (m_AssetList != null)
				{
					m_AssetList.Update();
				}
			}
		}

		internal void ForceReloadData()
		{
			UpdateSelectedBundles(new List<BundleInfo>());
			SetSelectedItems(new List<AssetInfo>());
			((TreeView)m_BundleTree).SetSelection((IList<int>)new int[0]);
			Model.ForceReloadData((TreeView)(object)m_BundleTree);
			m_Parent.Repaint();
		}

		internal void OnGUI(Rect pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Expected O, but got Unknown
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Expected O, but got Unknown
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Expected O, but got Unknown
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0182: Expected O, but got Unknown
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0210: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			m_Position = pos;
			if (m_BundleTree == null)
			{
				if (m_AssetListState == null)
				{
					m_AssetListState = new TreeViewState();
				}
				MultiColumnHeaderState val = AssetListTree.CreateDefaultMultiColumnHeaderState();
				if (MultiColumnHeaderState.CanOverwriteSerializedFields(m_AssetListMCHState, val))
				{
					MultiColumnHeaderState.OverwriteSerializedFields(m_AssetListMCHState, val);
				}
				m_AssetListMCHState = val;
				m_AssetList = new AssetListTree(m_AssetListState, m_AssetListMCHState, this);
				((TreeView)m_AssetList).Reload();
				m_MessageList = new MessageList();
				if (m_BundleDetailState == null)
				{
					m_BundleDetailState = new TreeViewState();
				}
				m_DetailsList = new BundleDetailList(m_BundleDetailState);
				((TreeView)m_DetailsList).Reload();
				if (m_BundleTreeState == null)
				{
					m_BundleTreeState = new TreeViewState();
				}
				m_BundleTree = new AssetBundleTree(m_BundleTreeState, this);
				m_BundleTree.Refresh();
				m_Parent.Repaint();
			}
			HandleHorizontalResize();
			HandleVerticalResize();
			if (Model.BundleListIsEmpty())
			{
				((TreeView)m_BundleTree).OnGUI(m_Position);
				GUIStyle val2 = new GUIStyle(GUI.get_skin().get_label());
				val2.set_alignment((TextAnchor)4);
				val2.set_wordWrap(true);
				GUI.Label(new Rect(((Rect)(ref m_Position)).get_x() + 1f, ((Rect)(ref m_Position)).get_y() + 1f, ((Rect)(ref m_Position)).get_width() - 2f, ((Rect)(ref m_Position)).get_height() - 2f), new GUIContent(Model.GetEmptyMessage()), val2);
				return;
			}
			Rect val3 = default(Rect);
			((Rect)(ref val3))._002Ector(((Rect)(ref m_Position)).get_x(), ((Rect)(ref m_Position)).get_y(), ((Rect)(ref m_HorizontalSplitterRect)).get_x(), ((Rect)(ref m_VerticalSplitterRectLeft)).get_y() - ((Rect)(ref m_Position)).get_y());
			((TreeView)m_BundleTree).OnGUI(val3);
			((TreeView)m_DetailsList).OnGUI(new Rect(((Rect)(ref val3)).get_x(), ((Rect)(ref val3)).get_y() + ((Rect)(ref val3)).get_height() + 3f, ((Rect)(ref val3)).get_width(), ((Rect)(ref m_Position)).get_height() - ((Rect)(ref val3)).get_height() - 6f));
			float num = ((Rect)(ref m_HorizontalSplitterRect)).get_x() + 3f;
			float num2 = ((Rect)(ref m_VerticalSplitterRectRight)).get_width() - 6f;
			float num3 = 20f;
			float num4 = ((Rect)(ref m_Position)).get_y() + num3;
			float num5 = ((Rect)(ref m_VerticalSplitterRectRight)).get_y() - num4;
			OnGUISearchBar(new Rect(num, ((Rect)(ref m_Position)).get_y(), num2, num3));
			((TreeView)m_AssetList).OnGUI(new Rect(num, num4, num2, num5));
			m_MessageList.OnGUI(new Rect(num, num4 + num5 + 3f, num2, ((Rect)(ref m_Position)).get_height() - num5 - 6f));
			if (m_ResizingHorizontalSplitter || m_ResizingVerticalSplitterRight || m_ResizingVerticalSplitterLeft)
			{
				m_Parent.Repaint();
			}
		}

		private void OnGUISearchBar(Rect rect)
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			((TreeView)m_BundleTree).set_searchString(m_searchField.OnGUI(rect, ((TreeView)m_BundleTree).get_searchString()));
			((TreeView)m_AssetList).set_searchString(((TreeView)m_BundleTree).get_searchString());
		}

		private void HandleHorizontalResize()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Invalid comparison between Unknown and I4
			((Rect)(ref m_HorizontalSplitterRect)).set_x((float)(int)(((Rect)(ref m_Position)).get_width() * m_HorizontalSplitterPercent));
			((Rect)(ref m_HorizontalSplitterRect)).set_height(((Rect)(ref m_Position)).get_height());
			EditorGUIUtility.AddCursorRect(m_HorizontalSplitterRect, (MouseCursor)3);
			if ((int)Event.get_current().get_type() == 0 && ((Rect)(ref m_HorizontalSplitterRect)).Contains(Event.get_current().get_mousePosition()))
			{
				m_ResizingHorizontalSplitter = true;
			}
			if (m_ResizingHorizontalSplitter)
			{
				m_HorizontalSplitterPercent = Mathf.Clamp(Event.get_current().get_mousePosition().x / ((Rect)(ref m_Position)).get_width(), 0.1f, 0.9f);
				((Rect)(ref m_HorizontalSplitterRect)).set_x((float)(int)(((Rect)(ref m_Position)).get_width() * m_HorizontalSplitterPercent));
			}
			if ((int)Event.get_current().get_type() == 1)
			{
				m_ResizingHorizontalSplitter = false;
			}
		}

		private void HandleVerticalResize()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Invalid comparison between Unknown and I4
			((Rect)(ref m_VerticalSplitterRectRight)).set_x(((Rect)(ref m_HorizontalSplitterRect)).get_x());
			((Rect)(ref m_VerticalSplitterRectRight)).set_y((float)(int)(((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentRight));
			((Rect)(ref m_VerticalSplitterRectRight)).set_width(((Rect)(ref m_Position)).get_width() - ((Rect)(ref m_HorizontalSplitterRect)).get_x());
			((Rect)(ref m_VerticalSplitterRectLeft)).set_y((float)(int)(((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentLeft));
			((Rect)(ref m_VerticalSplitterRectLeft)).set_width(((Rect)(ref m_VerticalSplitterRectRight)).get_width());
			EditorGUIUtility.AddCursorRect(m_VerticalSplitterRectRight, (MouseCursor)2);
			if ((int)Event.get_current().get_type() == 0 && ((Rect)(ref m_VerticalSplitterRectRight)).Contains(Event.get_current().get_mousePosition()))
			{
				m_ResizingVerticalSplitterRight = true;
			}
			EditorGUIUtility.AddCursorRect(m_VerticalSplitterRectLeft, (MouseCursor)2);
			if ((int)Event.get_current().get_type() == 0 && ((Rect)(ref m_VerticalSplitterRectLeft)).Contains(Event.get_current().get_mousePosition()))
			{
				m_ResizingVerticalSplitterLeft = true;
			}
			if (m_ResizingVerticalSplitterRight)
			{
				m_VerticalSplitterPercentRight = Mathf.Clamp(Event.get_current().get_mousePosition().y / ((Rect)(ref m_HorizontalSplitterRect)).get_height(), 0.2f, 0.98f);
				((Rect)(ref m_VerticalSplitterRectRight)).set_y((float)(int)(((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentRight));
			}
			else if (m_ResizingVerticalSplitterLeft)
			{
				m_VerticalSplitterPercentLeft = Mathf.Clamp(Event.get_current().get_mousePosition().y / ((Rect)(ref m_HorizontalSplitterRect)).get_height(), 0.25f, 0.98f);
				((Rect)(ref m_VerticalSplitterRectLeft)).set_y((float)(int)(((Rect)(ref m_HorizontalSplitterRect)).get_height() * m_VerticalSplitterPercentLeft));
			}
			if ((int)Event.get_current().get_type() == 1)
			{
				m_ResizingVerticalSplitterRight = false;
				m_ResizingVerticalSplitterLeft = false;
			}
		}

		internal void UpdateSelectedBundles(IEnumerable<BundleInfo> bundles)
		{
			Model.AddBundlesToUpdate(bundles);
			m_AssetList.SetSelectedBundles(bundles);
			m_DetailsList.SetItems(bundles);
			m_MessageList.SetItems(null);
		}

		internal void SetSelectedItems(IEnumerable<AssetInfo> items)
		{
			m_MessageList.SetItems(items);
		}
	}
}

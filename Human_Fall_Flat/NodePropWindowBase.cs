using System;
using System.Collections.Generic;
using HumanAPI;
using UnityEditor;
using UnityEngine;

public class NodePropWindowBase
{
	private class EditorState
	{
		public Editor propEditor;

		public bool expandedProps;

		public bool dead;
	}

	private Vector2 scrollPos;

	private bool ShowAllProps;

	private bool PendingScrollTo;

	private GUIStyle style_Highlight;

	private GUIStyle style_ScrollBg;

	private Node oldActiveNode;

	private Dictionary<Node, EditorState> map = new Dictionary<Node, EditorState>();

	private List<Node> dying = new List<Node>();

	public bool DrawProps(List<Node> nodes, Node activeNode, bool forceSelectionChanged, float boundsW)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Expected O, but got Unknown
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Expected O, but got Unknown
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Expected O, but got Unknown
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Invalid comparison between Unknown and I4
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0249: Expected O, but got Unknown
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Expected O, but got Unknown
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Expected O, but got Unknown
		//IL_027d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0382: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		foreach (KeyValuePair<Node, EditorState> item in map)
		{
			item.Value.dead = true;
		}
		bool result = false;
		if ((Object)(object)oldActiveNode != (Object)(object)activeNode)
		{
			oldActiveNode = activeNode;
			forceSelectionChanged = true;
		}
		bool showAllProps = ShowAllProps;
		ShowAllProps = EditorGUILayout.ToggleLeft("Show all nodes", ShowAllProps, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (showAllProps != ShowAllProps)
		{
			forceSelectionChanged = true;
		}
		EditorGUILayout.Separator();
		bool hierarchyMode = EditorGUIUtility.get_hierarchyMode();
		float labelWidth = EditorGUIUtility.get_labelWidth();
		float fieldWidth = EditorGUIUtility.get_fieldWidth();
		EditorGUIUtility.set_hierarchyMode(true);
		float num = 50f;
		EditorGUIUtility.set_labelWidth(Mathf.Max((boundsW - num) * 0.45f - 40f, 120f));
		EditorGUIUtility.set_fieldWidth(num);
		if (style_ScrollBg == null)
		{
			style_ScrollBg = new GUIStyle(EditorStyles.get_textArea());
			style_ScrollBg.set_margin(new RectOffset());
			style_ScrollBg.set_padding(new RectOffset());
		}
		scrollPos = GUILayout.BeginScrollView(scrollPos, style_ScrollBg, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(boundsW) });
		foreach (Node node in nodes)
		{
			if ((Object)(object)node == (Object)null)
			{
				continue;
			}
			EditorState value = null;
			if (!map.TryGetValue(node, out value))
			{
				value = new EditorState();
				map[node] = value;
			}
			value.dead = false;
			if (!ShowAllProps && (Object)(object)node != (Object)(object)activeNode)
			{
				value.expandedProps = false;
				continue;
			}
			Editor.CreateCachedEditor((Object)(object)node, (Type)null, ref value.propEditor);
			if ((Object)(object)value.propEditor == (Object)null)
			{
				continue;
			}
			bool flag = false;
			if ((Object)(object)activeNode == (Object)(object)node)
			{
				if (forceSelectionChanged)
				{
					forceSelectionChanged = false;
					PendingScrollTo = true;
					value.expandedProps = true;
				}
				if (ShowAllProps)
				{
					flag = true;
				}
			}
			bool flag2 = PendingScrollTo && (int)Event.get_current().get_type() == 7 && (Object)(object)activeNode == (Object)(object)node;
			Rect val = default(Rect);
			Rect val2 = default(Rect);
			Color backgroundColor = GUI.get_backgroundColor();
			if (flag)
			{
				if (style_Highlight == null)
				{
					style_Highlight = new GUIStyle(EditorStyles.get_textArea());
					style_Highlight.set_margin(new RectOffset());
					style_Highlight.set_padding(new RectOffset());
				}
				GUI.set_backgroundColor(new Color(1f, 0.5f, 0f, 1f));
				GUILayout.BeginVertical(style_Highlight, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUI.set_backgroundColor(backgroundColor);
			}
			ChangeCheckScope val3 = new ChangeCheckScope();
			try
			{
				value.expandedProps = EditorGUILayout.InspectorTitlebar(value.expandedProps, (Object)(object)node);
				if (flag2)
				{
					val = GUILayoutUtility.GetLastRect();
					val2 = val;
				}
				if (value.expandedProps)
				{
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
					value.propEditor.DrawDefaultInspector();
					if (flag2)
					{
						val2 = GUILayoutUtility.GetLastRect();
					}
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				}
				if (val3.get_changed())
				{
					result = true;
				}
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			if (flag)
			{
				GUILayout.EndVertical();
			}
			if (flag2)
			{
				PendingScrollTo = false;
				GUI.ScrollTo(Rect.MinMaxRect(Mathf.Min(((Rect)(ref val)).get_xMin(), ((Rect)(ref val2)).get_xMin()), Mathf.Min(((Rect)(ref val)).get_yMin(), ((Rect)(ref val2)).get_yMin()), Mathf.Max(((Rect)(ref val)).get_xMax(), ((Rect)(ref val2)).get_xMax()), Mathf.Max(((Rect)(ref val)).get_yMax(), ((Rect)(ref val2)).get_yMax()) + 10f));
				GUI.ScrollTo(val);
			}
		}
		EditorGUILayout.Separator();
		GUILayout.EndScrollView();
		EditorGUIUtility.set_labelWidth(labelWidth);
		EditorGUIUtility.set_fieldWidth(fieldWidth);
		EditorGUIUtility.set_hierarchyMode(hierarchyMode);
		foreach (KeyValuePair<Node, EditorState> item2 in map)
		{
			if (item2.Value.dead)
			{
				dying.Add(item2.Key);
			}
		}
		int i = 0;
		for (int count = dying.Count; i < count; i++)
		{
			map.Remove(dying[i]);
		}
		dying.Clear();
		return result;
	}
}

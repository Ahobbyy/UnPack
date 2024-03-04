using System;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using ProBuilder2.Interface;
using UnityEditor;
using UnityEngine;

internal class EditorCallbackViewer : EditorWindow
{
	private List<string> logs = new List<string>();

	private Vector2 scroll = Vector2.get_zero();

	private bool collapse = true;

	private static Color logBackgroundColor
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (!EditorGUIUtility.get_isProSkin())
			{
				return new Color(0.8f, 0.8f, 0.8f, 1f);
			}
			return new Color(0.15f, 0.15f, 0.15f, 0.5f);
		}
	}

	private static Color disabledColor
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (!EditorGUIUtility.get_isProSkin())
			{
				return new Color(0.8f, 0.8f, 0.8f, 1f);
			}
			return new Color(0.3f, 0.3f, 0.3f, 0.5f);
		}
	}

	[MenuItem("Tools/ProBuilder/API Examples/Log Callbacks Window")]
	private static void MenuInitEditorCallbackViewer()
	{
		((EditorWindow)EditorWindow.GetWindow<EditorCallbackViewer>(true, "ProBuilder Callbacks", true)).Show();
	}

	private void OnEnable()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		pb_Editor.AddOnEditLevelChangedListener((Action<int>)OnEditLevelChanged);
		pb_EditorUtility.AddOnObjectCreatedListener(new OnObjectCreated(OnProBuilderObjectCreated));
		pb_Editor.add_OnSelectionUpdate(new OnSelectionUpdateEventHandler(OnSelectionUpdate));
		pb_Editor.add_OnVertexMovementBegin(new OnVertexMovementBeginEventHandler(OnVertexMovementBegin));
		pb_Editor.add_OnVertexMovementFinish(new OnVertexMovementFinishedEventHandler(OnVertexMovementFinish));
		pb_EditorUtility.AddOnMeshCompiledListener(new OnMeshCompiled(OnMeshCompiled));
	}

	private void OnDisable()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Expected O, but got Unknown
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Expected O, but got Unknown
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Expected O, but got Unknown
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		pb_Editor.RemoveOnEditLevelChangedListener((Action<int>)OnEditLevelChanged);
		pb_EditorUtility.RemoveOnObjectCreatedListener(new OnObjectCreated(OnProBuilderObjectCreated));
		pb_EditorUtility.RemoveOnMeshCompiledListener(new OnMeshCompiled(OnMeshCompiled));
		pb_Editor.remove_OnSelectionUpdate(new OnSelectionUpdateEventHandler(OnSelectionUpdate));
		pb_Editor.remove_OnVertexMovementBegin(new OnVertexMovementBeginEventHandler(OnVertexMovementBegin));
		pb_Editor.remove_OnVertexMovementFinish(new OnVertexMovementFinishedEventHandler(OnVertexMovementFinish));
	}

	private void OnProBuilderObjectCreated(pb_Object pb)
	{
		AddLog("Instantiated new ProBuilder Object: " + ((Object)pb).get_name());
	}

	private void OnEditLevelChanged(int editLevel)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		EditLevel val = (EditLevel)editLevel;
		AddLog("Edit Level Changed: " + ((object)(EditLevel)(ref val)).ToString());
	}

	private void OnSelectionUpdate(pb_Object[] selection)
	{
		AddLog("Selection Updated: " + $"{((selection != null) ? selection.Length : 0)} objects and {selection?.Sum((pb_Object x) => x.get_SelectedTriangleCount()) ?? 0} vertices selected.");
	}

	private void OnVertexMovementBegin(pb_Object[] selection)
	{
		AddLog("Began Moving Vertices");
	}

	private void OnVertexMovementFinish(pb_Object[] selection)
	{
		AddLog("Finished Moving Vertices");
	}

	private void OnMeshCompiled(pb_Object pb, Mesh mesh)
	{
		AddLog($"Mesh {((Object)pb).get_name()} rebuilt");
	}

	private void AddLog(string summary)
	{
		logs.Add(summary);
		((EditorWindow)this).Repaint();
	}

	private void OnGUI()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		GUILayout.BeginHorizontal(EditorStyles.get_toolbar(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.FlexibleSpace();
		GUI.set_backgroundColor(collapse ? disabledColor : Color.get_white());
		if (GUILayout.Button("Collapse", EditorStyles.get_toolbarButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			collapse = !collapse;
		}
		GUI.set_backgroundColor(Color.get_white());
		if (GUILayout.Button("Clear", EditorStyles.get_toolbarButton(), (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			logs.Clear();
		}
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Callback Log", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.EndHorizontal();
		Rect lastRect = GUILayoutUtility.GetLastRect();
		((Rect)(ref lastRect)).set_x(0f);
		((Rect)(ref lastRect)).set_y(((Rect)(ref lastRect)).get_y() + ((Rect)(ref lastRect)).get_height() + 6f);
		Rect position = ((EditorWindow)this).get_position();
		((Rect)(ref lastRect)).set_width(((Rect)(ref position)).get_width());
		position = ((EditorWindow)this).get_position();
		((Rect)(ref lastRect)).set_height(((Rect)(ref position)).get_height());
		GUILayout.Space(4f);
		pb_EditorGUIUtility.DrawSolidColor(lastRect, logBackgroundColor);
		scroll = GUILayout.BeginScrollView(scroll, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		int count = logs.Count;
		int num = Math.Max(0, count - 1024);
		for (int num2 = count - 1; num2 >= num; num2--)
		{
			if (!collapse || num2 <= 0 || num2 >= count - 1 || !logs[num2].Equals(logs[num2 - 1]) || !logs[num2].Equals(logs[num2 + 1]))
			{
				GUILayout.Label(string.Format("{0,3}: {1}", num2, logs[num2]), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			}
		}
		GUILayout.EndScrollView();
	}

	public EditorCallbackViewer()
		: this()
	{
	}//IL_000c: Unknown result type (might be due to invalid IL or missing references)
	//IL_0011: Unknown result type (might be due to invalid IL or missing references)

}

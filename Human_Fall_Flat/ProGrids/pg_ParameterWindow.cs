using System;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	public class pg_ParameterWindow : EditorWindow
	{
		public pg_Editor editor;

		private GUIContent gc_predictiveGrid = new GUIContent("Predictive Grid", "If enabled, the grid will automatically render at the optimal axis based on movement.");

		private GUIContent gc_snapAsGroup = new GUIContent("Snap as Group", "If enabled, selected objects will keep their relative offsets when moving.  If disabled, every object in the selection is snapped to grid independently.");

		private void OnGUI()
		{
			GUILayout.Label("Snap Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			float snapIncrement = editor.GetSnapIncrement();
			EditorGUI.BeginChangeCheck();
			snapIncrement = EditorGUILayout.FloatField("Snap Value", snapIncrement, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				editor.SetSnapIncrement(snapIncrement);
			}
			EditorGUI.BeginChangeCheck();
			int @int = EditorPrefs.GetInt("pg_MajorLineIncrement", 10);
			@int = EditorGUILayout.IntField("Major Line Increment", @int, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			@int = ((@int < 2) ? 2 : ((@int > 128) ? 128 : @int));
			if (EditorGUI.EndChangeCheck())
			{
				EditorPrefs.SetInt("pg_MajorLineIncrement", @int);
				pg_GridRenderer.majorLineIncrement = @int;
				pg_Editor.ForceRepaint();
			}
			editor.ScaleSnapEnabled = EditorGUILayout.Toggle("Snap On Scale", editor.ScaleSnapEnabled, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			SnapUnit snapUnit = (EditorPrefs.HasKey("pg_GridUnit") ? ((SnapUnit)EditorPrefs.GetInt("pg_GridUnit")) : SnapUnit.Meter);
			bool snapAsGroup = editor.snapAsGroup;
			snapAsGroup = EditorGUILayout.Toggle(gc_snapAsGroup, snapAsGroup, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (snapAsGroup != editor.snapAsGroup)
			{
				editor.snapAsGroup = snapAsGroup;
			}
			EditorGUI.BeginChangeCheck();
			snapUnit = (SnapUnit)(object)EditorGUILayout.EnumPopup("Grid Units", (Enum)snapUnit, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUI.BeginChangeCheck();
			editor.angleValue = EditorGUILayout.Slider("Angle", editor.angleValue, 0f, 180f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				SceneView.RepaintAll();
			}
			if (EditorGUI.EndChangeCheck())
			{
				EditorPrefs.SetInt("pg_GridUnit", (int)snapUnit);
				editor.LoadPreferences();
			}
			bool predictiveGrid = editor.predictiveGrid;
			predictiveGrid = EditorGUILayout.Toggle(gc_predictiveGrid, predictiveGrid, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (predictiveGrid != editor.predictiveGrid)
			{
				editor.predictiveGrid = predictiveGrid;
				EditorPrefs.SetBool("pg_PredictiveGrid", predictiveGrid);
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button("Done", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				((EditorWindow)this).Close();
			}
		}

		public pg_ParameterWindow()
			: this()
		{
		}//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown

	}
}

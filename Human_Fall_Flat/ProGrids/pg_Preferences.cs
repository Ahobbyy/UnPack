using System;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	public class pg_Preferences
	{
		private static Color _gridColorX;

		private static Color _gridColorY;

		private static Color _gridColorZ;

		private static float _alphaBump;

		private static bool _scaleSnapEnabled;

		private static int _snapMethod;

		private static float _BracketIncreaseValue;

		private static SnapUnit _GridUnits;

		private static bool _syncUnitySnap;

		private static KeyCode _IncreaseGridSize = (KeyCode)61;

		private static KeyCode _DecreaseGridSize = (KeyCode)45;

		private static KeyCode _NudgePerspectiveBackward = (KeyCode)91;

		private static KeyCode _NudgePerspectiveForward = (KeyCode)93;

		private static KeyCode _NudgePerspectiveReset = (KeyCode)48;

		private static KeyCode _CyclePerspective = (KeyCode)92;

		public static Color GRID_COLOR_X = new Color(0.9f, 0.46f, 0.46f, 0.15f);

		public static Color GRID_COLOR_Y = new Color(0.46f, 0.9f, 0.46f, 0.15f);

		public static Color GRID_COLOR_Z = new Color(0.46f, 0.46f, 0.9f, 0.15f);

		public static float ALPHA_BUMP = 0.25f;

		public static bool USE_AXIS_CONSTRAINTS = false;

		public static bool SHOW_GRID = true;

		private static string[] SnapMethod = new string[2] { "Snap on Selected Axis", "Snap on All Axes" };

		private static int[] SnapVals = new int[2] { 1, 0 };

		private static bool prefsLoaded = false;

		[PreferenceItem("ProGrids")]
		public static void PreferencesGUI()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Expected O, but got Unknown
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0206: Unknown result type (might be due to invalid IL or missing references)
			if (!prefsLoaded)
			{
				prefsLoaded = LoadPreferences();
			}
			GUILayout.Label("Grid Colors per Axis", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_gridColorX = EditorGUILayout.ColorField("X Axis", _gridColorX, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_gridColorY = EditorGUILayout.ColorField("Y Axis", _gridColorY, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_gridColorZ = EditorGUILayout.ColorField("Z Axis", _gridColorZ, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_alphaBump = EditorGUILayout.Slider(new GUIContent("Tenth Line Alpha", "Every 10th line will have it's alpha value bumped by this amount."), _alphaBump, 0f, 1f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_GridUnits = (SnapUnit)(object)EditorGUILayout.EnumPopup("Grid Units", (Enum)_GridUnits, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_scaleSnapEnabled = EditorGUILayout.Toggle("Snap On Scale", _scaleSnapEnabled, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_snapMethod = EditorGUILayout.IntPopup("Snap Method", _snapMethod, SnapMethod, SnapVals, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_syncUnitySnap = EditorGUILayout.Toggle("Sync w/ Unity Snap", _syncUnitySnap, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Shortcuts", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_IncreaseGridSize = (KeyCode)(object)EditorGUILayout.EnumPopup("Increase Grid Size", (Enum)(object)_IncreaseGridSize, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_DecreaseGridSize = (KeyCode)(object)EditorGUILayout.EnumPopup("Decrease Grid Size", (Enum)(object)_DecreaseGridSize, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_NudgePerspectiveBackward = (KeyCode)(object)EditorGUILayout.EnumPopup("Nudge Perspective Backward", (Enum)(object)_NudgePerspectiveBackward, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_NudgePerspectiveForward = (KeyCode)(object)EditorGUILayout.EnumPopup("Nudge Perspective Forward", (Enum)(object)_NudgePerspectiveForward, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_NudgePerspectiveReset = (KeyCode)(object)EditorGUILayout.EnumPopup("Nudge Perspective Reset", (Enum)(object)_NudgePerspectiveReset, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			_CyclePerspective = (KeyCode)(object)EditorGUILayout.EnumPopup("Cycle Perspective", (Enum)(object)_CyclePerspective, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Reset", (GUILayoutOption[])(object)new GUILayoutOption[0]) && EditorUtility.DisplayDialog("Delete ProGrids editor preferences?", "Are you sure you want to delete these?, this action cannot be undone.", "Yes", "No"))
			{
				ResetPrefs();
			}
			if (GUI.get_changed())
			{
				SetPreferences();
			}
		}

		public static bool LoadPreferences()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			_scaleSnapEnabled = EditorPrefs.HasKey("scaleSnapEnabled") && EditorPrefs.GetBool("scaleSnapEnabled");
			_gridColorX = (EditorPrefs.HasKey("gridColorX") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorX")) : GRID_COLOR_X);
			_gridColorY = (EditorPrefs.HasKey("gridColorY") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorY")) : GRID_COLOR_Y);
			_gridColorZ = (EditorPrefs.HasKey("gridColorZ") ? pg_Util.ColorWithString(EditorPrefs.GetString("gridColorZ")) : GRID_COLOR_Z);
			_alphaBump = (EditorPrefs.HasKey("pg_alphaBump") ? EditorPrefs.GetFloat("pg_alphaBump") : ALPHA_BUMP);
			_snapMethod = Convert.ToInt32(EditorPrefs.HasKey("pgUseAxisConstraints") ? EditorPrefs.GetBool("pgUseAxisConstraints") : USE_AXIS_CONSTRAINTS);
			_BracketIncreaseValue = (EditorPrefs.HasKey("pgBracketIncreaseValue") ? EditorPrefs.GetFloat("pgBracketIncreaseValue") : 0.25f);
			_GridUnits = (EditorPrefs.HasKey("pg_GridUnit") ? ((SnapUnit)EditorPrefs.GetInt("pg_GridUnit")) : SnapUnit.Meter);
			_syncUnitySnap = EditorPrefs.GetBool("pg_SyncUnitySnap", true);
			_IncreaseGridSize = (KeyCode)(EditorPrefs.HasKey("pg_Editor::IncreaseGridSize") ? EditorPrefs.GetInt("pg_Editor::IncreaseGridSize") : 61);
			_DecreaseGridSize = (KeyCode)(EditorPrefs.HasKey("pg_Editor::DecreaseGridSize") ? EditorPrefs.GetInt("pg_Editor::DecreaseGridSize") : 45);
			_NudgePerspectiveBackward = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveBackward") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveBackward") : 91);
			_NudgePerspectiveForward = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveForward") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveForward") : 93);
			_NudgePerspectiveReset = (KeyCode)(EditorPrefs.HasKey("pg_Editor::NudgePerspectiveReset") ? EditorPrefs.GetInt("pg_Editor::NudgePerspectiveReset") : 48);
			_CyclePerspective = (KeyCode)(EditorPrefs.HasKey("pg_Editor::CyclePerspective") ? EditorPrefs.GetInt("pg_Editor::CyclePerspective") : 92);
			return true;
		}

		public static void SetPreferences()
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected I4, but got Unknown
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Expected I4, but got Unknown
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Expected I4, but got Unknown
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Expected I4, but got Unknown
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Expected I4, but got Unknown
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Expected I4, but got Unknown
			EditorPrefs.SetBool("scaleSnapEnabled", _scaleSnapEnabled);
			EditorPrefs.SetString("gridColorX", ((Color)(ref _gridColorX)).ToString("f3"));
			EditorPrefs.SetString("gridColorY", ((Color)(ref _gridColorY)).ToString("f3"));
			EditorPrefs.SetString("gridColorZ", ((Color)(ref _gridColorZ)).ToString("f3"));
			EditorPrefs.SetFloat("pg_alphaBump", _alphaBump);
			EditorPrefs.SetBool("pgUseAxisConstraints", Convert.ToBoolean(_snapMethod));
			EditorPrefs.SetFloat("pgBracketIncreaseValue", _BracketIncreaseValue);
			EditorPrefs.SetInt("pg_GridUnit", (int)_GridUnits);
			EditorPrefs.SetBool("pg_SyncUnitySnap", _syncUnitySnap);
			EditorPrefs.SetInt("pg_Editor::IncreaseGridSize", (int)_IncreaseGridSize);
			EditorPrefs.SetInt("pg_Editor::DecreaseGridSize", (int)_DecreaseGridSize);
			EditorPrefs.SetInt("pg_Editor::NudgePerspectiveBackward", (int)_NudgePerspectiveBackward);
			EditorPrefs.SetInt("pg_Editor::NudgePerspectiveForward", (int)_NudgePerspectiveForward);
			EditorPrefs.SetInt("pg_Editor::NudgePerspectiveReset", (int)_NudgePerspectiveReset);
			EditorPrefs.SetInt("pg_Editor::CyclePerspective", (int)_CyclePerspective);
			if ((Object)(object)pg_Editor.instance != (Object)null)
			{
				pg_Editor.instance.LoadPreferences();
			}
		}

		public static void ResetPrefs()
		{
			EditorPrefs.DeleteKey("scaleSnapEnabled");
			EditorPrefs.DeleteKey("gridColorX");
			EditorPrefs.DeleteKey("gridColorY");
			EditorPrefs.DeleteKey("gridColorZ");
			EditorPrefs.DeleteKey("pg_alphaBump");
			EditorPrefs.DeleteKey("pgUseAxisConstraints");
			EditorPrefs.DeleteKey("pgBracketIncreaseValue");
			EditorPrefs.DeleteKey("pg_GridUnit");
			EditorPrefs.DeleteKey("showgrid");
			EditorPrefs.DeleteKey("pgSnapMultiplier");
			EditorPrefs.DeleteKey("pg_SyncUnitySnap");
			EditorPrefs.DeleteKey("pg_Editor::IncreaseGridSize");
			EditorPrefs.DeleteKey("pg_Editor::DecreaseGridSize");
			EditorPrefs.DeleteKey("pg_Editor::NudgePerspectiveBackward");
			EditorPrefs.DeleteKey("pg_Editor::NudgePerspectiveForward");
			EditorPrefs.DeleteKey("pg_Editor::NudgePerspectiveReset");
			EditorPrefs.DeleteKey("pg_Editor::CyclePerspective");
			LoadPreferences();
		}
	}
}

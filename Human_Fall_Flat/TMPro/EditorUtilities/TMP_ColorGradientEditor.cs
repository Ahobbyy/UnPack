using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_ColorGradient))]
	public class TMP_ColorGradientEditor : Editor
	{
		private SerializedProperty topLeftColor;

		private SerializedProperty topRightColor;

		private SerializedProperty bottomLeftColor;

		private SerializedProperty bottomRightColor;

		private void OnEnable()
		{
			TMP_UIStyleManager.GetUIStyles();
			topLeftColor = ((Editor)this).get_serializedObject().FindProperty("topLeft");
			topRightColor = ((Editor)this).get_serializedObject().FindProperty("topRight");
			bottomLeftColor = ((Editor)this).get_serializedObject().FindProperty("bottomLeft");
			bottomRightColor = ((Editor)this).get_serializedObject().FindProperty("bottomRight");
		}

		public override void OnInspectorGUI()
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Expected O, but got Unknown
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Expected O, but got Unknown
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Expected O, but got Unknown
			((Editor)this).get_serializedObject().Update();
			GUILayout.Label("<b>TextMeshPro - Color Gradient Preset</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.BeginVertical(TMP_UIStyleManager.SquareAreaBox85G, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(topLeftColor, new GUIContent("Top Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(topRightColor, new GUIContent("Top Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(bottomLeftColor, new GUIContent("Bottom Left"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(bottomRightColor, new GUIContent("Bottom Right"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			if (((Editor)this).get_serializedObject().ApplyModifiedProperties())
			{
				TMPro_EventManager.ON_COLOR_GRAIDENT_PROPERTY_CHANGED(((Editor)this).get_target() as TMP_ColorGradient);
			}
		}

		public TMP_ColorGradientEditor()
			: this()
		{
		}
	}
}

using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TextContainer))]
	[CanEditMultipleObjects]
	public class TMPro_TextContainerEditor : Editor
	{
		private SerializedProperty anchorPosition_prop;

		private SerializedProperty pivot_prop;

		private SerializedProperty rectangle_prop;

		private SerializedProperty margins_prop;

		private TextContainer m_textContainer;

		private void OnEnable()
		{
			anchorPosition_prop = ((Editor)this).get_serializedObject().FindProperty("m_anchorPosition");
			pivot_prop = ((Editor)this).get_serializedObject().FindProperty("m_pivot");
			rectangle_prop = ((Editor)this).get_serializedObject().FindProperty("m_rect");
			margins_prop = ((Editor)this).get_serializedObject().FindProperty("m_margins");
			m_textContainer = (TextContainer)(object)((Editor)this).get_target();
			TMP_UIStyleManager.GetUIStyles();
		}

		private void OnDisable()
		{
		}

		public override void OnInspectorGUI()
		{
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Expected O, but got Unknown
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			((Editor)this).get_serializedObject().Update();
			GUILayout.Label("<b>TEXT CONTAINER</b>", TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(23f) });
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(anchorPosition_prop, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (anchorPosition_prop.get_enumValueIndex() == 9)
			{
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				EditorGUILayout.PropertyField(pivot_prop, new GUIContent("Pivot Position"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
			}
			DrawDimensionProperty(rectangle_prop, "Dimensions");
			DrawMaginProperty(margins_prop, "Margins");
			if (EditorGUI.EndChangeCheck())
			{
				if (anchorPosition_prop.get_enumValueIndex() != 9)
				{
					pivot_prop.set_vector2Value(GetAnchorPosition(anchorPosition_prop.get_enumValueIndex()));
				}
				m_textContainer.hasChanged = true;
			}
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		private void DrawDimensionProperty(SerializedProperty property, string label)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 18f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width(), 18f);
			float num = ((Rect)(ref controlRect)).get_width() + 3f;
			((Rect)(ref val)).set_width(labelWidth);
			GUI.Label(val, label);
			Rect rectValue = property.get_rectValue();
			float num2 = (num - labelWidth) / 4f;
			((Rect)(ref val)).set_width(num2 - 5f);
			((Rect)(ref val)).set_x(labelWidth + 15f);
			GUI.Label(val, "Width");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			((Rect)(ref rectValue)).set_width(EditorGUI.FloatField(val, GUIContent.none, ((Rect)(ref rectValue)).get_width()));
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Height");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			((Rect)(ref rectValue)).set_height(EditorGUI.FloatField(val, GUIContent.none, ((Rect)(ref rectValue)).get_height()));
			property.set_rectValue(rectValue);
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
		}

		private void DrawMaginProperty(SerializedProperty property, string label)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			float labelWidth = EditorGUIUtility.get_labelWidth();
			float fieldWidth = EditorGUIUtility.get_fieldWidth();
			Rect controlRect = EditorGUILayout.GetControlRect(false, 36f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Rect val = default(Rect);
			((Rect)(ref val))._002Ector(((Rect)(ref controlRect)).get_x(), ((Rect)(ref controlRect)).get_y() + 2f, ((Rect)(ref controlRect)).get_width(), 18f);
			float num = ((Rect)(ref controlRect)).get_width() + 3f;
			((Rect)(ref val)).set_width(labelWidth);
			GUI.Label(val, label);
			Vector4 zero = Vector4.get_zero();
			zero.x = property.FindPropertyRelative("x").get_floatValue();
			zero.y = property.FindPropertyRelative("y").get_floatValue();
			zero.z = property.FindPropertyRelative("z").get_floatValue();
			zero.w = property.FindPropertyRelative("w").get_floatValue();
			float num2 = (num - labelWidth) / 4f;
			((Rect)(ref val)).set_width(num2 - 5f);
			((Rect)(ref val)).set_x(labelWidth + 15f);
			GUI.Label(val, "Left");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Top");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Right");
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			GUI.Label(val, "Bottom");
			((Rect)(ref val)).set_y(((Rect)(ref val)).get_y() + 18f);
			((Rect)(ref val)).set_x(labelWidth + 15f);
			zero.x = EditorGUI.FloatField(val, GUIContent.none, zero.x);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			zero.y = EditorGUI.FloatField(val, GUIContent.none, zero.y);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			zero.z = EditorGUI.FloatField(val, GUIContent.none, zero.z);
			((Rect)(ref val)).set_x(((Rect)(ref val)).get_x() + num2);
			zero.w = EditorGUI.FloatField(val, GUIContent.none, zero.w);
			property.FindPropertyRelative("x").set_floatValue(zero.x);
			property.FindPropertyRelative("y").set_floatValue(zero.y);
			property.FindPropertyRelative("z").set_floatValue(zero.z);
			property.FindPropertyRelative("w").set_floatValue(zero.w);
			EditorGUIUtility.set_labelWidth(labelWidth);
			EditorGUIUtility.set_fieldWidth(fieldWidth);
		}

		private Vector2 GetAnchorPosition(int index)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			Vector2 zero = Vector2.get_zero();
			switch (index)
			{
			case 0:
				((Vector2)(ref zero))._002Ector(0f, 1f);
				break;
			case 1:
				((Vector2)(ref zero))._002Ector(0.5f, 1f);
				break;
			case 2:
				((Vector2)(ref zero))._002Ector(1f, 1f);
				break;
			case 3:
				((Vector2)(ref zero))._002Ector(0f, 0.5f);
				break;
			case 4:
				((Vector2)(ref zero))._002Ector(0.5f, 0.5f);
				break;
			case 5:
				((Vector2)(ref zero))._002Ector(1f, 0.5f);
				break;
			case 6:
				((Vector2)(ref zero))._002Ector(0f, 0f);
				break;
			case 7:
				((Vector2)(ref zero))._002Ector(0.5f, 0f);
				break;
			case 8:
				((Vector2)(ref zero))._002Ector(1f, 0f);
				break;
			}
			return zero;
		}

		public TMPro_TextContainerEditor()
			: this()
		{
		}
	}
}

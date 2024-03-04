using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(TMP_Style))]
	public class StyleDrawer : PropertyDrawer
	{
		public static readonly float height = 95f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return height;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			SerializedProperty val = property.FindPropertyRelative("m_Name");
			SerializedProperty val2 = property.FindPropertyRelative("m_HashCode");
			SerializedProperty val3 = property.FindPropertyRelative("m_OpeningDefinition");
			SerializedProperty val4 = property.FindPropertyRelative("m_ClosingDefinition");
			SerializedProperty val5 = property.FindPropertyRelative("m_OpeningTagArray");
			SerializedProperty val6 = property.FindPropertyRelative("m_ClosingTagArray");
			EditorGUIUtility.set_labelWidth(90f);
			((Rect)(ref position)).set_height(EditorGUIUtility.get_singleLineHeight() + EditorGUIUtility.get_standardVerticalSpacing());
			float num = ((Rect)(ref position)).get_height() + 2f;
			EditorGUI.BeginChangeCheck();
			Rect val7 = default(Rect);
			((Rect)(ref val7))._002Ector(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width() / 2f + 5f, ((Rect)(ref position)).get_height());
			EditorGUI.PropertyField(val7, val);
			if (EditorGUI.EndChangeCheck())
			{
				val2.set_intValue(TMP_TextUtilities.GetSimpleHashCode(val.get_stringValue()));
				property.get_serializedObject().ApplyModifiedProperties();
				TMP_StyleSheet.RefreshStyles();
			}
			Rect val8 = default(Rect);
			((Rect)(ref val8))._002Ector(((Rect)(ref val7)).get_x() + ((Rect)(ref val7)).get_width() + 5f, ((Rect)(ref position)).get_y(), 65f, ((Rect)(ref position)).get_height());
			GUI.Label(val8, "HashCode");
			GUI.set_enabled(false);
			((Rect)(ref val8)).set_x(((Rect)(ref val8)).get_x() + 65f);
			((Rect)(ref val8)).set_width(((Rect)(ref position)).get_width() / 2f - 75f);
			EditorGUI.PropertyField(val8, val2, GUIContent.none);
			GUI.set_enabled(true);
			EditorGUI.BeginChangeCheck();
			((Rect)(ref position)).set_y(((Rect)(ref position)).get_y() + num);
			GUI.Label(position, "Opening Tags");
			Rect val9 = default(Rect);
			((Rect)(ref val9))._002Ector(108f, ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width() - 86f, 35f);
			val3.set_stringValue(EditorGUI.TextArea(val9, val3.get_stringValue()));
			if (EditorGUI.EndChangeCheck())
			{
				int length = val3.get_stringValue().Length;
				if (val5.get_arraySize() != length)
				{
					val5.set_arraySize(length);
				}
				for (int i = 0; i < length; i++)
				{
					val5.GetArrayElementAtIndex(i).set_intValue((int)val3.get_stringValue()[i]);
				}
			}
			EditorGUI.BeginChangeCheck();
			((Rect)(ref position)).set_y(((Rect)(ref position)).get_y() + 38f);
			GUI.Label(position, "Closing Tags");
			Rect val10 = default(Rect);
			((Rect)(ref val10))._002Ector(108f, ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width() - 86f, 35f);
			val4.set_stringValue(EditorGUI.TextArea(val10, val4.get_stringValue()));
			if (EditorGUI.EndChangeCheck())
			{
				int length2 = val4.get_stringValue().Length;
				if (val6.get_arraySize() != length2)
				{
					val6.set_arraySize(length2);
				}
				for (int j = 0; j < length2; j++)
				{
					val6.GetArrayElementAtIndex(j).set_intValue((int)val4.get_stringValue()[j]);
				}
			}
		}

		public StyleDrawer()
			: this()
		{
		}
	}
}

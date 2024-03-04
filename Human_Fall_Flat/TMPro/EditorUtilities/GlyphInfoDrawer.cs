using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(TMP_Glyph))]
	public class GlyphInfoDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Expected O, but got Unknown
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Expected O, but got Unknown
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Expected O, but got Unknown
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0250: Expected O, but got Unknown
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_028c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0296: Expected O, but got Unknown
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_02dd: Expected O, but got Unknown
			//IL_030e: Unknown result type (might be due to invalid IL or missing references)
			//IL_031a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0324: Expected O, but got Unknown
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_0361: Unknown result type (might be due to invalid IL or missing references)
			//IL_036b: Expected O, but got Unknown
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b2: Expected O, but got Unknown
			//IL_03e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Expected O, but got Unknown
			SerializedProperty val = property.FindPropertyRelative("id");
			SerializedProperty val2 = property.FindPropertyRelative("x");
			SerializedProperty val3 = property.FindPropertyRelative("y");
			SerializedProperty val4 = property.FindPropertyRelative("width");
			SerializedProperty val5 = property.FindPropertyRelative("height");
			SerializedProperty val6 = property.FindPropertyRelative("xOffset");
			SerializedProperty val7 = property.FindPropertyRelative("yOffset");
			SerializedProperty val8 = property.FindPropertyRelative("xAdvance");
			SerializedProperty val9 = property.FindPropertyRelative("scale");
			Rect rect = GUILayoutUtility.GetRect(((Rect)(ref position)).get_width(), 48f);
			((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() - 15f);
			EditorGUIUtility.set_labelWidth(40f);
			EditorGUIUtility.set_fieldWidth(45f);
			EditorGUI.LabelField(new Rect(((Rect)(ref rect)).get_x() + 5f, ((Rect)(ref rect)).get_y(), 80f, 18f), new GUIContent("Ascii: <color=#FFFF80>" + val.get_intValue() + "</color>"), TMP_UIStyleManager.Label);
			EditorGUI.LabelField(new Rect(((Rect)(ref rect)).get_x() + 90f, ((Rect)(ref rect)).get_y(), 80f, 18f), new GUIContent("Hex: <color=#FFFF80>" + val.get_intValue().ToString("X") + "</color>"), TMP_UIStyleManager.Label);
			EditorGUI.LabelField(new Rect(((Rect)(ref rect)).get_x() + 170f, ((Rect)(ref rect)).get_y(), 80f, 18f), "Char: [ <color=#FFFF80>" + (char)val.get_intValue() + "</color> ]", TMP_UIStyleManager.Label);
			EditorGUIUtility.set_labelWidth(35f);
			EditorGUIUtility.set_fieldWidth(10f);
			float num = (((Rect)(ref rect)).get_width() - 5f) / 4f;
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 0f, ((Rect)(ref rect)).get_y() + 22f, num - 5f, 18f), val2, new GUIContent("X:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 1f, ((Rect)(ref rect)).get_y() + 22f, num - 5f, 18f), val3, new GUIContent("Y:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 2f, ((Rect)(ref rect)).get_y() + 22f, num - 5f, 18f), val4, new GUIContent("W:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 3f, ((Rect)(ref rect)).get_y() + 22f, num - 5f, 18f), val5, new GUIContent("H:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 0f, ((Rect)(ref rect)).get_y() + 44f, num - 5f, 18f), val6, new GUIContent("OX:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 1f, ((Rect)(ref rect)).get_y() + 44f, num - 5f, 18f), val7, new GUIContent("OY:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 2f, ((Rect)(ref rect)).get_y() + 44f, num - 5f, 18f), val8, new GUIContent("ADV:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref rect)).get_x() + 5f + num * 3f, ((Rect)(ref rect)).get_y() + 44f, num - 5f, 18f), val9, new GUIContent("SF:"));
		}

		public GlyphInfoDrawer()
			: this()
		{
		}
	}
}

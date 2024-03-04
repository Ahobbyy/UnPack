using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(KerningPair))]
	public class KerningPairDrawer : PropertyDrawer
	{
		private bool isEditingEnabled;

		private string char_left;

		private string char_right;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Expected O, but got Unknown
			SerializedProperty val = property.FindPropertyRelative("AscII_Left");
			SerializedProperty val2 = property.FindPropertyRelative("AscII_Right");
			SerializedProperty val3 = property.FindPropertyRelative("XadvanceOffset");
			((Rect)(ref position)).set_yMin(((Rect)(ref position)).get_yMin() + 4f);
			((Rect)(ref position)).set_yMax(((Rect)(ref position)).get_yMax() + 4f);
			((Rect)(ref position)).set_height(((Rect)(ref position)).get_height() - 2f);
			float num = ((Rect)(ref position)).get_width() / 3f;
			float num2 = 5f;
			isEditingEnabled = ((label != GUIContent.none) ? true : false);
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(isEditingEnabled);
			Rect val4 = default(Rect);
			((Rect)(ref val4))._002Ector(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y(), 25f, ((Rect)(ref position)).get_height());
			char_left = EditorGUI.TextArea(val4, ((char)val.get_intValue()).ToString() ?? "");
			if (GUI.get_changed() && char_left != "")
			{
				GUI.set_changed(false);
				val.set_intValue((int)char_left[0]);
			}
			EditorGUI.PropertyField(new Rect(((Rect)(ref position)).get_x() + ((Rect)(ref val4)).get_width() + num2, ((Rect)(ref position)).get_y(), num - ((Rect)(ref val4)).get_width() - 10f, ((Rect)(ref position)).get_height()), val, GUIContent.none);
			((Rect)(ref val4))._002Ector(((Rect)(ref position)).get_x() + num * 1f, ((Rect)(ref position)).get_y(), 25f, ((Rect)(ref position)).get_height());
			char_right = EditorGUI.TextArea(val4, ((char)val2.get_intValue()).ToString() ?? "");
			if (GUI.get_changed() && char_right != "")
			{
				GUI.set_changed(false);
				val2.set_intValue((int)char_right[0]);
			}
			EditorGUI.PropertyField(new Rect(((Rect)(ref position)).get_x() + num * 1f + ((Rect)(ref val4)).get_width() + num2, ((Rect)(ref position)).get_y(), num - ((Rect)(ref val4)).get_width() - 10f, ((Rect)(ref position)).get_height()), val2, GUIContent.none);
			GUI.set_enabled(true);
			Rect val5 = new Rect(((Rect)(ref position)).get_x() + num * 2f, ((Rect)(ref position)).get_y(), num, ((Rect)(ref position)).get_height());
			EditorGUIUtility.set_labelWidth(40f);
			EditorGUIUtility.set_fieldWidth(45f);
			EditorGUI.PropertyField(val5, val3, new GUIContent("Offset"));
			GUILayout.EndHorizontal();
		}

		public KerningPairDrawer()
			: this()
		{
		}
	}
}

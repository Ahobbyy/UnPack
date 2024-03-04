using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(TMP_Sprite))]
	public class SpriteInfoDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_0193: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_028f: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a4: Expected O, but got Unknown
			//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Expected O, but got Unknown
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0495: Expected O, but got Unknown
			//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_04d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04dc: Expected O, but got Unknown
			//IL_050d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0519: Unknown result type (might be due to invalid IL or missing references)
			//IL_0523: Expected O, but got Unknown
			//IL_0554: Unknown result type (might be due to invalid IL or missing references)
			//IL_0560: Unknown result type (might be due to invalid IL or missing references)
			//IL_056a: Expected O, but got Unknown
			//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_05ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_05b6: Expected O, but got Unknown
			//IL_05e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_05f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_05fd: Expected O, but got Unknown
			//IL_062e: Unknown result type (might be due to invalid IL or missing references)
			//IL_063a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0644: Expected O, but got Unknown
			//IL_0675: Unknown result type (might be due to invalid IL or missing references)
			//IL_0681: Unknown result type (might be due to invalid IL or missing references)
			//IL_068b: Expected O, but got Unknown
			SerializedProperty val = property.FindPropertyRelative("id");
			SerializedProperty val2 = property.FindPropertyRelative("name");
			SerializedProperty val3 = property.FindPropertyRelative("hashCode");
			SerializedProperty val4 = property.FindPropertyRelative("unicode");
			SerializedProperty val5 = property.FindPropertyRelative("x");
			SerializedProperty val6 = property.FindPropertyRelative("y");
			SerializedProperty val7 = property.FindPropertyRelative("width");
			SerializedProperty val8 = property.FindPropertyRelative("height");
			SerializedProperty val9 = property.FindPropertyRelative("xOffset");
			SerializedProperty val10 = property.FindPropertyRelative("yOffset");
			SerializedProperty val11 = property.FindPropertyRelative("xAdvance");
			SerializedProperty val12 = property.FindPropertyRelative("scale");
			SerializedProperty val13 = property.FindPropertyRelative("sprite");
			Texture spriteSheet = (property.get_serializedObject().get_targetObject() as TMP_SpriteAsset).spriteSheet;
			if ((Object)(object)spriteSheet == (Object)null)
			{
				Debug.LogWarning((object)("Please assign a valid Sprite Atlas texture to the [" + property.get_serializedObject().get_targetObject().get_name() + "] Sprite Asset."), property.get_serializedObject().get_targetObject());
				return;
			}
			Vector2 val14 = default(Vector2);
			((Vector2)(ref val14))._002Ector(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y());
			Vector2 val15 = default(Vector2);
			((Vector2)(ref val15))._002Ector(65f, 65f);
			if (val7.get_floatValue() >= val8.get_floatValue())
			{
				val15.y = val8.get_floatValue() * val15.x / val7.get_floatValue();
				val14.y += (val15.x - val15.y) / 2f;
			}
			else
			{
				val15.x = val7.get_floatValue() * val15.y / val8.get_floatValue();
				val14.x += (val15.y - val15.x) / 2f;
			}
			Rect val16 = default(Rect);
			((Rect)(ref val16))._002Ector(val5.get_floatValue() / (float)spriteSheet.get_width(), val6.get_floatValue() / (float)spriteSheet.get_height(), val7.get_floatValue() / (float)spriteSheet.get_width(), val8.get_floatValue() / (float)spriteSheet.get_height());
			GUI.DrawTextureWithTexCoords(new Rect(val14.x + 5f, val14.y + 2.5f, val15.x, val15.y), spriteSheet, val16, true);
			Rect val17 = default(Rect);
			((Rect)(ref val17))._002Ector(((Rect)(ref position)).get_x(), ((Rect)(ref position)).get_y(), ((Rect)(ref position)).get_width(), 49f);
			((Rect)(ref val17)).set_x(((Rect)(ref val17)).get_x() + 70f);
			bool enabled = GUI.get_enabled();
			GUI.set_enabled(false);
			EditorGUIUtility.set_labelWidth(30f);
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f, ((Rect)(ref val17)).get_y(), 65f, 18f), val, new GUIContent("ID:"));
			GUI.set_enabled(enabled);
			EditorGUI.BeginChangeCheck();
			EditorGUIUtility.set_labelWidth(55f);
			GUI.SetNextControlName("Unicode Input");
			string s = EditorGUI.TextField(new Rect(((Rect)(ref val17)).get_x() + 75f, ((Rect)(ref val17)).get_y(), 105f, 18f), "Unicode:", val4.get_intValue().ToString("X"));
			if (GUI.GetNameOfFocusedControl() == "Unicode Input")
			{
				char character = Event.get_current().get_character();
				if ((character < '0' || character > '9') && (character < 'a' || character > 'f') && (character < 'A' || character > 'F'))
				{
					Event.get_current().set_character('\0');
				}
				if (EditorGUI.EndChangeCheck())
				{
					val4.set_intValue(TMP_TextUtilities.StringToInt(s));
					property.get_serializedObject().ApplyModifiedProperties();
					(property.get_serializedObject().get_targetObject() as TMP_SpriteAsset).UpdateLookupTables();
				}
			}
			EditorGUIUtility.set_labelWidth(45f);
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 185f, ((Rect)(ref val17)).get_y(), ((Rect)(ref val17)).get_width() - 260f, 18f), val2, new GUIContent("Name: " + val2.get_stringValue()));
			if (EditorGUI.EndChangeCheck())
			{
				Object objectReferenceValue = val13.get_objectReferenceValue();
				Sprite val18 = (Sprite)(object)((objectReferenceValue is Sprite) ? objectReferenceValue : null);
				if ((Object)(object)val18 != (Object)null)
				{
					((Object)val18).set_name(val2.get_stringValue());
				}
				val3.set_intValue(TMP_TextUtilities.GetSimpleHashCode(val2.get_stringValue()));
				property.get_serializedObject().ApplyModifiedProperties();
			}
			EditorGUIUtility.set_labelWidth(30f);
			EditorGUIUtility.set_fieldWidth(10f);
			float num = (((Rect)(ref val17)).get_width() - 75f) / 4f;
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 0f, ((Rect)(ref val17)).get_y() + 22f, num - 5f, 18f), val5, new GUIContent("X:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 1f, ((Rect)(ref val17)).get_y() + 22f, num - 5f, 18f), val6, new GUIContent("Y:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 2f, ((Rect)(ref val17)).get_y() + 22f, num - 5f, 18f), val7, new GUIContent("W:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 3f, ((Rect)(ref val17)).get_y() + 22f, num - 5f, 18f), val8, new GUIContent("H:"));
			EditorGUI.BeginChangeCheck();
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 0f, ((Rect)(ref val17)).get_y() + 44f, num - 5f, 18f), val9, new GUIContent("OX:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 1f, ((Rect)(ref val17)).get_y() + 44f, num - 5f, 18f), val10, new GUIContent("OY:"));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 2f, ((Rect)(ref val17)).get_y() + 44f, num - 5f, 18f), val11, new GUIContent("Adv."));
			EditorGUI.PropertyField(new Rect(((Rect)(ref val17)).get_x() + 5f + num * 3f, ((Rect)(ref val17)).get_y() + 44f, num - 5f, 18f), val12, new GUIContent("SF."));
			EditorGUI.EndChangeCheck();
		}

		public SpriteInfoDrawer()
			: this()
		{
		}
	}
}

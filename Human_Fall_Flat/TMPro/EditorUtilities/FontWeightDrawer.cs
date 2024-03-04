using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(TMP_FontWeights))]
	public class FontWeightDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			SerializedProperty val = property.FindPropertyRelative("regularTypeface");
			SerializedProperty val2 = property.FindPropertyRelative("italicTypeface");
			float width = ((Rect)(ref position)).get_width();
			((Rect)(ref position)).set_width(125f);
			EditorGUI.LabelField(position, label);
			if (label.get_text()[0] == '4')
			{
				GUI.set_enabled(false);
			}
			((Rect)(ref position)).set_x(140f);
			((Rect)(ref position)).set_width((width - 140f) / 2f);
			EditorGUI.PropertyField(position, val, GUIContent.none);
			GUI.set_enabled(true);
			((Rect)(ref position)).set_x(((Rect)(ref position)).get_x() + (((Rect)(ref position)).get_width() + 17f));
			EditorGUI.PropertyField(position, val2, GUIContent.none);
		}

		public FontWeightDrawer()
			: this()
		{
		}
	}
}

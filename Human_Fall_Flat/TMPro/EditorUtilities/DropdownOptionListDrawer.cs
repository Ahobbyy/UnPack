using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomPropertyDrawer(typeof(TMP_Dropdown.OptionDataList), true)]
	internal class DropdownOptionListDrawer : PropertyDrawer
	{
		private ReorderableList m_ReorderableList;

		private void Init(SerializedProperty property)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Expected O, but got Unknown
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Expected O, but got Unknown
			if (m_ReorderableList == null)
			{
				SerializedProperty val = property.FindPropertyRelative("m_Options");
				m_ReorderableList = new ReorderableList(property.get_serializedObject(), val);
				m_ReorderableList.drawElementCallback = new ElementCallbackDelegate(DrawOptionData);
				m_ReorderableList.drawHeaderCallback = new HeaderCallbackDelegate(DrawHeader);
				ReorderableList reorderableList = m_ReorderableList;
				reorderableList.elementHeight += 16f;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			Init(property);
			m_ReorderableList.DoList(position);
		}

		private void DrawHeader(Rect rect)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			GUI.Label(rect, "Options");
		}

		private void DrawOptionData(Rect rect, int index, bool isActive, bool isFocused)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			SerializedProperty arrayElementAtIndex = m_ReorderableList.get_serializedProperty().GetArrayElementAtIndex(index);
			SerializedProperty val = arrayElementAtIndex.FindPropertyRelative("m_Text");
			SerializedProperty val2 = arrayElementAtIndex.FindPropertyRelative("m_Image");
			rect = new RectOffset(0, 0, -1, -3).Add(rect);
			((Rect)(ref rect)).set_height(EditorGUIUtility.get_singleLineHeight());
			EditorGUI.PropertyField(rect, val, GUIContent.none);
			((Rect)(ref rect)).set_y(((Rect)(ref rect)).get_y() + EditorGUIUtility.get_singleLineHeight());
			EditorGUI.PropertyField(rect, val2, GUIContent.none);
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Init(property);
			return m_ReorderableList.GetHeight();
		}

		public DropdownOptionListDrawer()
			: this()
		{
		}
	}
}

using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_Dropdown), true)]
	[CanEditMultipleObjects]
	public class DropdownEditor : SelectableEditor
	{
		private SerializedProperty m_Template;

		private SerializedProperty m_CaptionText;

		private SerializedProperty m_CaptionImage;

		private SerializedProperty m_ItemText;

		private SerializedProperty m_ItemImage;

		private SerializedProperty m_OnSelectionChanged;

		private SerializedProperty m_Value;

		private SerializedProperty m_Options;

		protected override void OnEnable()
		{
			((SelectableEditor)this).OnEnable();
			m_Template = ((Editor)this).get_serializedObject().FindProperty("m_Template");
			m_CaptionText = ((Editor)this).get_serializedObject().FindProperty("m_CaptionText");
			m_CaptionImage = ((Editor)this).get_serializedObject().FindProperty("m_CaptionImage");
			m_ItemText = ((Editor)this).get_serializedObject().FindProperty("m_ItemText");
			m_ItemImage = ((Editor)this).get_serializedObject().FindProperty("m_ItemImage");
			m_OnSelectionChanged = ((Editor)this).get_serializedObject().FindProperty("m_OnValueChanged");
			m_Value = ((Editor)this).get_serializedObject().FindProperty("m_Value");
			m_Options = ((Editor)this).get_serializedObject().FindProperty("m_Options");
		}

		public override void OnInspectorGUI()
		{
			((SelectableEditor)this).OnInspectorGUI();
			EditorGUILayout.Space();
			((Editor)this).get_serializedObject().Update();
			EditorGUILayout.PropertyField(m_Template, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_CaptionText, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_CaptionImage, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_ItemText, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_ItemImage, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_Value, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_Options, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PropertyField(m_OnSelectionChanged, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		public DropdownEditor()
			: this()
		{
		}
	}
}

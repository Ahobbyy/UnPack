using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

[CustomEditor(typeof(MenuSelector))]
public class MenuSelectorEditor : ButtonEditor
{
	public override void OnInspectorGUI()
	{
		((ButtonEditor)this).OnInspectorGUI();
		((Editor)this).get_serializedObject().Update();
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("options"), true, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("optionLabels"), true, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("selectedIndex"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("valueLabel"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("leftArrow"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("rightArrow"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("onValueChanged"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	public MenuSelectorEditor()
		: this()
	{
	}
}

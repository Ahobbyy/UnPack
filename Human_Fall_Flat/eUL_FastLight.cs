using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UL_FastLight))]
[CanEditMultipleObjects]
public class eUL_FastLight : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).get_serializedObject().Update();
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("range"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("color"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("intensity"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	public eUL_FastLight()
		: this()
	{
	}
}

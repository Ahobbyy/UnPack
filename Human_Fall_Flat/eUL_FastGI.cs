using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UL_FastGI))]
[CanEditMultipleObjects]
public class eUL_FastGI : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).get_serializedObject().Update();
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("expand"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("intensity"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	public eUL_FastGI()
		: this()
	{
	}
}

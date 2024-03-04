using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UL_Manager))]
[CanEditMultipleObjects]
public class eUL_Manager : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).get_serializedObject().Update();
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("layersToRayTrace"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("showDebugRays"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("showDebugGUI"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	public eUL_Manager()
		: this()
	{
	}
}

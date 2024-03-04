using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UL_RayTracedGI))]
[CanEditMultipleObjects]
public class eUL_RayTracedGI : Editor
{
	public override void OnInspectorGUI()
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Invalid comparison between Unknown and I4
		UL_RayTracedGI uL_RayTracedGI = (UL_RayTracedGI)(object)((Editor)this).get_target();
		((Editor)this).get_serializedObject().Update();
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("intensity"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("raysMatrixSize"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		Light baseLight = uL_RayTracedGI.BaseLight;
		if (baseLight != null && (int)baseLight.get_type() == 1)
		{
			EditorGUILayout.PropertyField(((Editor)this).get_serializedObject().FindProperty("raysMatrixScale"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}
		if (((Behaviour)uL_RayTracedGI).get_enabled())
		{
			if (GUILayout.Button("Generate Fast Lights", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				uL_RayTracedGI.CreateFastLights();
			}
		}
		else if (GUILayout.Button("Destroy Fast Lights", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			uL_RayTracedGI.DestroyFastLights();
		}
		((Editor)this).get_serializedObject().ApplyModifiedProperties();
	}

	public eUL_RayTracedGI()
		: this()
	{
	}
}

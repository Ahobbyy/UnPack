using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CalculateAttenuation))]
[CanEditMultipleObjects]
public class CalculateAttenuationEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("Generate Attenuation", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			Object[] targets = ((Editor)this).get_targets();
			for (int i = 0; i < targets.Length; i++)
			{
				(targets[i] as CalculateAttenuation).Generate();
			}
		}
	}

	public CalculateAttenuationEditor()
		: this()
	{
	}
}

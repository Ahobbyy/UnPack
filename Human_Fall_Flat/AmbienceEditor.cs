using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Ambience))]
public class AmbienceEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("Show Editor", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			AmbienceWindow.Init(((Editor)this).get_target() as Ambience);
		}
	}

	public AmbienceEditor()
		: this()
	{
	}
}

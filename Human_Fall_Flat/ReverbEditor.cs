using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Reverb))]
public class ReverbEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("Show Editor", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			ReverbWindow.Init(((Editor)this).get_target() as Reverb);
		}
	}

	public ReverbEditor()
		: this()
	{
	}
}

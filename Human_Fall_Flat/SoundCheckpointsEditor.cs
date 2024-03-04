using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundCheckpoints))]
public class SoundCheckpointsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		((Editor)this).DrawDefaultInspector();
		SoundCheckpoints soundCheckpoints = (SoundCheckpoints)(object)((Editor)this).get_target();
		EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUILayout.Button("Go Previous Checkpoint", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			soundCheckpoints.GoPreviousCheckpoint();
		}
		if (GUILayout.Button("Go Next Checkpoint", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			soundCheckpoints.GoNextCheckpoint();
		}
		EditorGUILayout.EndHorizontal();
	}

	public SoundCheckpointsEditor()
		: this()
	{
	}
}

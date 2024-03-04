using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NodeGraph))]
public class NodeGraphEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("Show Graph", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			NodeWindow.Init(((Editor)this).get_target() as NodeGraph);
		}
	}

	public NodeGraphEditor()
		: this()
	{
	}
}

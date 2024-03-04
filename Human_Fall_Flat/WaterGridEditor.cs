using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(WaterGrid))]
public class WaterGridEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("Scan", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			(((Editor)this).get_target() as WaterGrid).Rebuild();
		}
	}

	public WaterGridEditor()
		: this()
	{
	}
}

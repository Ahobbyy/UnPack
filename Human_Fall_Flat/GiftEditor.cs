using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Gift))]
public class GiftEditor : Editor
{
	public override void OnInspectorGUI()
	{
		((Editor)this).DrawDefaultInspector();
		if (GUILayout.Button("RandomizeModel", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			GiftGroupEditor.RandomizeGift(((Component)(((Editor)this).get_target() as Gift)).GetComponentInParent<GiftGroup>(), ((Editor)this).get_target() as Gift);
		}
	}

	public GiftEditor()
		: this()
	{
	}
}

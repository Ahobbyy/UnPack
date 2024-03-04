using UnityEditor;
using UnityEngine;

public class SelectParent : EditorWindow
{
	[MenuItem("Edit/Select parent &g")]
	private static void SelectParentOfObject()
	{
		if ((Object)(object)Selection.get_activeGameObject() != (Object)null)
		{
			Selection.set_activeGameObject(((Component)Selection.get_activeGameObject().get_transform().get_root()).get_gameObject());
		}
	}

	public SelectParent()
		: this()
	{
	}
}

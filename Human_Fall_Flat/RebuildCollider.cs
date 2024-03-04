using UnityEditor;
using UnityEngine;

public static class RebuildCollider
{
	[MenuItem("GameObject/Rebuild Collider", false, -1)]
	private static void Rebuild()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Object[] objects = Selection.get_objects();
		foreach (Object obj in objects)
		{
			BoxCollider component = ((GameObject)((obj is GameObject) ? obj : null)).GetComponent<BoxCollider>();
			if (!((Object)(object)component == (Object)null))
			{
				BoxCollider val = ((Component)component).get_gameObject().AddComponent<BoxCollider>();
				component.set_size(val.get_size());
				component.set_center(val.get_center());
				Object.DestroyImmediate((Object)(object)val);
			}
		}
	}
}

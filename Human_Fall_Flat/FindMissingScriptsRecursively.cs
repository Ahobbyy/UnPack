using UnityEditor;
using UnityEngine;

public class FindMissingScriptsRecursively : EditorWindow
{
	private static int go_count;

	private static int components_count;

	private static int missing_count;

	[MenuItem("Lab42Tools/Find Missing Scripts Recursively")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMissingScriptsRecursively));
	}

	public void OnGUI()
	{
		if (GUILayout.Button("Find Missing Scripts in selected GameObjects", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			FindInSelected();
		}
	}

	private static void FindInSelected()
	{
		GameObject[] gameObjects = Selection.get_gameObjects();
		go_count = 0;
		components_count = 0;
		missing_count = 0;
		GameObject[] array = gameObjects;
		for (int i = 0; i < array.Length; i++)
		{
			FindInGO(array[i]);
		}
		Debug.Log((object)$"Searched {go_count} GameObjects, {components_count} components, found {missing_count} missing");
	}

	private static void FindInGO(GameObject g)
	{
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		go_count++;
		Component[] components = g.GetComponents<Component>();
		for (int i = 0; i < components.Length; i++)
		{
			components_count++;
			if ((Object)(object)components[i] == (Object)null)
			{
				missing_count++;
				string text = ((Object)g).get_name();
				Transform val = g.get_transform();
				while ((Object)(object)val.get_parent() != (Object)null)
				{
					text = ((Object)val.get_parent()).get_name() + "/" + text;
					val = val.get_parent();
				}
				Debug.Log((object)(text + " has an empty script attached in position: " + i), (Object)(object)g);
			}
		}
		foreach (Transform item in g.get_transform())
		{
			FindInGO(((Component)item).get_gameObject());
		}
	}

	public FindMissingScriptsRecursively()
		: this()
	{
	}
}

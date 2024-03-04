using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FindMaterials : EditorWindow
{
	public static string materialToFind = "";

	private static Dictionary<string, int> materialSorted;

	private static string matString;

	[MenuItem("Window/Find Materials")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMaterials));
	}

	public void OnGUI()
	{
		GUILayout.Label("Instructions -", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Select Gameobject(s) in hierarchy, to search for materials under it(them). You can select multiple of them.", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Enter the name of the material you are looking for. Leave it empty if you want all the materials to be listed.", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(25f);
		materialToFind = EditorGUILayout.TextField("Material to find - ", materialToFind, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(10f);
		if (GUILayout.Button("Search and log", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			FindInSelected();
		}
	}

	private static void FindInSelected()
	{
		materialSorted = new Dictionary<string, int>();
		matString = "";
		GameObject[] gameObjects = Selection.get_gameObjects();
		for (int i = 0; i < gameObjects.Length; i++)
		{
			FindInGO(gameObjects[i]);
		}
		matString = $"Found {materialSorted.Count} unique materials:\n";
		foreach (KeyValuePair<string, int> item in materialSorted)
		{
			matString += $"{item.Key} - {item.Value} instances\n";
		}
		Debug.Log((object)matString);
	}

	private static void FindInGO(GameObject g)
	{
		Renderer[] componentsInChildren = g.GetComponentsInChildren<Renderer>();
		foreach (Renderer val in componentsInChildren)
		{
			Material[] sharedMaterials = val.get_sharedMaterials();
			foreach (Material val2 in sharedMaterials)
			{
				if (!((Object)(object)val2 == (Object)null) && (string.IsNullOrEmpty(materialToFind) || ((Object)val2).get_name().ToLower().Contains(materialToFind.ToLower())))
				{
					Debug.Log((object)("Found material " + materialToFind + " on " + ((Object)((Component)val).get_gameObject()).get_name()));
					if (!materialSorted.ContainsKey(((Object)val2).get_name()))
					{
						materialSorted[((Object)val2).get_name()] = 0;
					}
					materialSorted[((Object)val2).get_name()]++;
				}
			}
		}
	}

	public FindMaterials()
		: this()
	{
	}
}

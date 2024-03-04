using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class FindMaterialsInProject : EditorWindow
{
	public static string folderPath = "";

	private static Dictionary<string, List<string>> materialSorted;

	private static string logMsg;

	[MenuItem("Window/Find Materials in project")]
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(FindMaterialsInProject));
	}

	public void OnGUI()
	{
		GUILayout.Label("Instructions -", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Enter the path with-in which we have to search! The path looks like this Assets/ContestLevel/LabAssets", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Note that there is no '/' before assets or at the end of the path", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(10f);
		GUILayout.Label("How does this work -", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("Am collecting materials' shader name, albedo color, emission color, gloss, metallic, texture and comparing them with other materials. ", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Label("We are not comparing any other properties, so some manual intervention would be needed before we strip the materials", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(25f);
		folderPath = EditorGUILayout.TextField("Path - ", folderPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		GUILayout.Space(10f);
		if (GUILayout.Button("Search and log", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			FindInSelected();
		}
	}

	private static string GetColor(Color col)
	{
		return col.r + "-" + col.g + "-" + col.b + "-" + col.a;
	}

	private static string GetMaterialProperties(Material mat)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		string text = "";
		text += ((Object)mat.get_shader()).get_name();
		text = text + "," + GetColor(mat.get_color());
		if (mat.HasProperty("_EmissionColor"))
		{
			Color color = mat.GetColor("_EmissionColor");
			bool flag = color.r + color.g + color.b > 0f;
			text = text + "," + (flag ? "Yes" : "No");
			text = text + "," + GetColor(color);
		}
		else
		{
			text += ",None";
			text += ",None";
		}
		text = text + "," + (((Object)(object)mat.get_mainTexture() != (Object)null) ? ((Object)mat.get_mainTexture()).get_name() : "None");
		text = text + "," + (mat.HasProperty("_GlossMapScale") ? mat.GetFloat("_GlossMapScale").ToString() : "None");
		text = text + "," + (mat.HasProperty("_Glossiness") ? mat.GetFloat("_Glossiness").ToString() : "None");
		return text + "," + (mat.HasProperty("_Metallic") ? mat.GetFloat("_Metallic").ToString() : "None");
	}

	private static void FindInSelected()
	{
		materialSorted = new Dictionary<string, List<string>>();
		string text = "";
		Debug.Log((object)("Searching in path - " + folderPath));
		string[] array = AssetDatabase.FindAssets("t:material", new string[1] { folderPath });
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = AssetDatabase.GUIDToAssetPath(array[i]);
			Material val = AssetDatabase.LoadAssetAtPath<Material>(text2);
			if ((Object)(object)val == (Object)null)
			{
				text = text + "Unable to load material-" + text2 + "\n";
				continue;
			}
			string materialProperties = GetMaterialProperties(val);
			if (!materialSorted.ContainsKey(materialProperties))
			{
				materialSorted[materialProperties] = new List<string>();
			}
			materialSorted[materialProperties].Add(text2 + "|" + ((Object)val).get_name());
		}
		Debug.Log((object)text);
		Debug.Log((object)"File exported to MaterialsAtAPath.csv");
		text = "";
		text += "Comment, shader Name, albedo, emissive? ,emissive color, texture, Glossy scale, smoothness, metallic, material name, path\n";
		bool flag = false;
		foreach (KeyValuePair<string, List<string>> item in materialSorted)
		{
			flag = true;
			foreach (string item2 in item.Value)
			{
				string[] array2 = item2.Split('|');
				Debug.Assert(array2.Length == 2);
				text = ((!flag) ? (text + "Same as above,") : (text + "New Set,"));
				text = text + item.Key + "," + array2[1] + "," + array2[0] + "\n";
				flag = false;
			}
		}
		File.WriteAllText("MaterialsAtAPath.csv", text);
	}

	public FindMaterialsInProject()
		: this()
	{
	}
}

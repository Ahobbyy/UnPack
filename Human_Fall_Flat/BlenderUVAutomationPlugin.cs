using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BlenderUVAutomationPlugin : EditorWindow
{
	private static string logMsg;

	private static EditorWindow window;

	private string folderPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_BLENDER_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_BLENDER_PATH", value);
		}
	}

	private string scriptPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_SCRIPT", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_SCRIPT", value);
		}
	}

	private string materialPositionMappingCSVPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_MATERIAL_POS_MAP_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_MATERIAL_POS_MAP_PATH", value);
		}
	}

	private string excludeMaterialsCSVPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_EXCLUDE_MATERIALS_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_EXCLUDE_MATERIALS_PATH", value);
		}
	}

	private string duplicateMaterialCSVPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_DUPLICATE_MATERIALS_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_DUPLICATE_MATERIALS_PATH", value);
		}
	}

	private string fbxInputPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_FBX_INPUT_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_FBX_INPUT_PATH", value);
		}
	}

	private string fbxOutputPath
	{
		get
		{
			return EditorPrefs.GetString("BPY_FBX_OUTPUT_PATH", "");
		}
		set
		{
			EditorPrefs.SetString("BPY_FBX_OUTPUT_PATH", value);
		}
	}

	private int rowDivisionCount
	{
		get
		{
			return EditorPrefs.GetInt("BPY_ROW_DIVISION_COUNT", 1);
		}
		set
		{
			EditorPrefs.SetInt("BPY_ROW_DIVISION_COUNT", value);
		}
	}

	[MenuItem("Window/Blender python integration")]
	public static void ShowWindow()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		window = EditorWindow.GetWindow(typeof(BlenderUVAutomationPlugin));
		window.set_minSize(new Vector2(800f, 400f));
	}

	public void OnGUI()
	{
		if (!((Object)(object)window == (Object)null))
		{
			GUILayout.Label("Instructions -", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Label("Enter all the paths here and run. Make sure you do not have \\ or / at the end of the path. ", (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(25f);
			EditorGUIUtility.set_labelWidth(400f);
			folderPath = EditorGUILayout.TextField("Blender 2.8(or later) path - ", folderPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			scriptPath = EditorGUILayout.TextField("Python script file path - ", scriptPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			materialPositionMappingCSVPath = EditorGUILayout.TextField("Material position map file path - ", materialPositionMappingCSVPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			excludeMaterialsCSVPath = EditorGUILayout.TextField("Exclude material map file path - ", excludeMaterialsCSVPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			duplicateMaterialCSVPath = EditorGUILayout.TextField("duplicate material Path - ", duplicateMaterialCSVPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			fbxInputPath = EditorGUILayout.TextField("FBX input Path - ", fbxInputPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			fbxOutputPath = EditorGUILayout.TextField("FBX output Path(optional) - ", fbxOutputPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			rowDivisionCount = int.Parse(EditorGUILayout.TextField("Atlas swatch count in a Row - ", rowDivisionCount.ToString(), (GUILayoutOption[])(object)new GUILayoutOption[0]));
			GUILayout.Space(10f);
			if (GUILayout.Button("Run Script", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				string text = "\"" + Path.Combine(folderPath, "blender.exe") + "\"";
				string text2 = (string.IsNullOrEmpty(fbxOutputPath) ? $"--background --python \"{scriptPath}\" -- --materialMapping=\"{materialPositionMappingCSVPath}\" --excludedMaterials=\"{excludeMaterialsCSVPath}\" --duplicateMaterials=\"{duplicateMaterialCSVPath}\" --filename=\"{fbxInputPath}\" --quaddivisioncount={rowDivisionCount}" : $"--background --python \"{scriptPath}\" -- --materialMapping=\"{materialPositionMappingCSVPath}\" --excludedMaterials=\"{excludeMaterialsCSVPath}\" --duplicateMaterials=\"{duplicateMaterialCSVPath}\" --filename=\"{fbxInputPath}\" --outfilename=\"{fbxOutputPath}\" --quaddivisioncount={rowDivisionCount}");
				Debug.Log((object)("Executing->" + text + " " + text2));
				Execute(text, text2);
			}
		}
	}

	private static void Execute(string command, string arg)
	{
		Process.Start(new ProcessStartInfo(command)
		{
			WindowStyle = ProcessWindowStyle.Normal,
			Arguments = arg
		});
	}

	public BlenderUVAutomationPlugin()
		: this()
	{
	}
}

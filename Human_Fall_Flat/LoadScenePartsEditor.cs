using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(LoadSceneParts))]
public class LoadScenePartsEditor : Editor
{
	public override void OnInspectorGUI()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		((Editor)this).DrawDefaultInspector();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		LoadSceneParts loadSceneParts = (LoadSceneParts)(object)((Editor)this).get_target();
		string[] array = new string[EditorSceneManager.get_loadedSceneCount() - 1];
		for (int i = 1; i < EditorSceneManager.get_loadedSceneCount(); i++)
		{
			int num = i - 1;
			Scene sceneAt = SceneManager.GetSceneAt(i);
			array[num] = ((Scene)(ref sceneAt)).get_name();
		}
		EditorGUILayout.LabelField("Scene to save", (GUILayoutOption[])(object)new GUILayoutOption[0]);
		loadSceneParts.selectedScene = EditorGUILayout.Popup(loadSceneParts.selectedScene, array, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (GUILayout.Button("Save", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			loadSceneParts.SaveSelectedScene();
		}
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.Space();
		if (GUILayout.Button("Load Scenes", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			loadSceneParts.LoadScenes();
		}
	}

	public LoadScenePartsEditor()
		: this()
	{
	}
}

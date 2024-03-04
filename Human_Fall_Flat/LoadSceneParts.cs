using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
[DefaultExecutionOrder(-4)]
public class LoadSceneParts : MonoBehaviour
{
	[HideInInspector]
	public int selectedScene;

	public Object[] scenes;

	public void LoadScenes()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Object[] array = scenes;
		for (int i = 0; i < array.Length; i++)
		{
			EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(array[i]), (OpenSceneMode)1);
		}
	}

	public static void SaveScene()
	{
		Object.FindObjectOfType<LoadSceneParts>().SaveSelectedScene();
	}

	public void SaveSelectedScene()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		EditorSceneManager.SaveScene(SceneManager.GetSceneAt(selectedScene + 1));
	}

	public LoadSceneParts()
		: this()
	{
	}
}

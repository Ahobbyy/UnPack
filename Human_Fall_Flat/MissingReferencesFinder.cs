using System.Linq;
using UnityEditor;
using UnityEngine;

public class MissingReferencesFinder : MonoBehaviour
{
	private const string MENU_ROOT = "Tools/Missing References/";

	[MenuItem("Tools/Missing References/Search in scene", false, 50)]
	public static void FindMissingReferencesInCurrentScene()
	{
		GameObject[] sceneObjects = GetSceneObjects();
		FindMissingReferences(EditorApplication.get_currentScene(), sceneObjects);
	}

	[MenuItem("Tools/Missing References/Search in all scenes", false, 51)]
	public static void MissingSpritesInAllScenes()
	{
		foreach (EditorBuildSettingsScene item in from s in EditorBuildSettings.get_scenes()
			where s.get_enabled()
			select s)
		{
			EditorApplication.OpenScene(item.get_path());
			FindMissingReferencesInCurrentScene();
		}
	}

	[MenuItem("Tools/Missing References/Search in assets", false, 52)]
	public static void MissingSpritesInAssets()
	{
		GameObject[] objects = (from a in (from path in AssetDatabase.GetAllAssetPaths()
				where path.StartsWith("Assets/")
				select path).ToArray().Select(delegate(string a)
			{
				Object obj = AssetDatabase.LoadAssetAtPath(a, typeof(GameObject));
				return (GameObject)(object)((obj is GameObject) ? obj : null);
			})
			where (Object)(object)a != (Object)null
			select a).ToArray();
		FindMissingReferences("Project", objects);
	}

	private static void FindMissingReferences(string context, GameObject[] objects)
	{
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Invalid comparison between Unknown and I4
		foreach (GameObject val in objects)
		{
			Component[] components = val.GetComponents<Component>();
			foreach (Component val2 in components)
			{
				if (!Object.op_Implicit((Object)(object)val2))
				{
					Debug.LogError((object)("Missing Component in GO: " + GetFullPath(val)), (Object)(object)val);
					continue;
				}
				SerializedProperty iterator = new SerializedObject((Object)(object)val2).GetIterator();
				while (iterator.NextVisible(true))
				{
					if ((int)iterator.get_propertyType() == 5 && iterator.get_objectReferenceValue() == (Object)null && iterator.get_objectReferenceInstanceIDValue() != 0)
					{
						ShowError(context, val, ((object)val2).GetType().Name, ObjectNames.NicifyVariableName(iterator.get_name()));
					}
				}
			}
		}
	}

	private static GameObject[] GetSceneObjects()
	{
		return (from go in Resources.FindObjectsOfTypeAll<GameObject>()
			where string.IsNullOrEmpty(AssetDatabase.GetAssetPath((Object)(object)go)) && (int)((Object)go).get_hideFlags() == 0
			select go).ToArray();
	}

	private static void ShowError(string context, GameObject go, string componentName, string propertyName)
	{
		Debug.LogError((object)string.Format("Missing Ref in: [{3}]{0}. Component: {1}, Property: {2}", GetFullPath(go), componentName, propertyName, context), (Object)(object)go);
	}

	private static string GetFullPath(GameObject go)
	{
		if (!((Object)(object)go.get_transform().get_parent() == (Object)null))
		{
			return GetFullPath(((Component)go.get_transform().get_parent()).get_gameObject()) + "/" + ((Object)go).get_name();
		}
		return ((Object)go).get_name();
	}

	public MissingReferencesFinder()
		: this()
	{
	}
}

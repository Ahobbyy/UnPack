using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PrefabSampler
{
	public class PrefabsCheck : EditorWindow
	{
		private static string assetsDirectory = "Assets/";

		private const string ASSETS_DIR = "ASSETS_CHECK_DIR";

		[MenuItem("Window/Prefab Sampler/Prefab Sampler Check")]
		private static void Inititalize()
		{
			if (PlayerPrefs.HasKey("ASSETS_CHECK_DIR"))
			{
				assetsDirectory = PlayerPrefs.GetString("ASSETS_CHECK_DIR");
			}
			((EditorWindow)EditorWindow.GetWindow<PrefabsCheck>(true, "Prefab Check Tool")).get_titleContent().set_text("Prefab Check Tool");
		}

		private void OnGUI()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Prefab Folder to Check:", assetsDirectory, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Change Prefab Directory", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }))
			{
				string path = EditorUtility.OpenFolderPanel("Prefab Destination Folder", "Assets/", "");
				path = PrefabSamplerConfigData.GetValidPath(path);
				if (assetsDirectory != path)
				{
					assetsDirectory = path;
					PlayerPrefs.SetString("ASSETS_CHECK_DIR", assetsDirectory);
				}
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Check Prefabs", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }))
			{
				CheckPrefabs();
			}
			EditorGUILayout.EndVertical();
		}

		public static void CheckPrefabs()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Expected O, but got Unknown
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Invalid comparison between Unknown and I4
			List<Object> list = new List<Object>();
			Object[] array = Object.FindObjectsOfType(typeof(GameObject));
			for (int i = 0; i < array.Length; i++)
			{
				GameObject val = (GameObject)array[i];
				if ((int)PrefabUtility.GetPrefabType((Object)(object)val) == 3 && (Object)(object)PrefabUtility.FindPrefabRoot(val) == (Object)(object)val)
				{
					list.Add(PrefabUtility.GetPrefabParent((Object)(object)val));
				}
			}
			string[] array2 = AssetDatabase.FindAssets("t: GameObject", new string[1] { assetsDirectory });
			for (int i = 0; i < array2.Length; i++)
			{
				GameObject val2 = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(array2[i]));
				if (!list.Contains((Object)(object)val2))
				{
					Debug.LogError((object)(((Object)val2).get_name() + " not found in the scene!"));
				}
			}
		}

		public PrefabsCheck()
			: this()
		{
		}
	}
}

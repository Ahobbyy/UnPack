using UnityEditor;
using UnityEngine;

namespace PrefabSampler
{
	public class PrefabSamplerEditor : EditorWindow
	{
		private PrefabSamplerConfigEditor editorConfig;

		[MenuItem("Window/Prefab Sampler/Prefab Sampler")]
		private static void Initialize()
		{
			((EditorWindow)EditorWindow.GetWindow<PrefabSamplerEditor>(false, "Prefab Sampler")).get_titleContent().set_text("Prefab Sampler");
		}

		private void OnGUI()
		{
			DrawGUI();
		}

		private void DrawGUI()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			if ((Object)(object)PrefabSamplerConfigData.EditorData != (Object)null)
			{
				EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				string text = ((Selection.get_gameObjects().Length != 0) ? (((Object)Selection.get_gameObjects()[0]).get_name() + PrefabSamplerConfigData.EditorData.AppendName) : "");
				EditorGUILayout.TextField("Asset name: ", text, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
				EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (GUILayout.Button("SAMPLE PREFAB", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }) && Selection.get_gameObjects().Length != 0)
				{
					PrefabSamplerTool.SamplePrefab(Selection.get_gameObjects());
				}
				if (GUILayout.Toggle((Object)(object)editorConfig != (Object)null, (Texture)(object)PrefabSamplerConfigData.EditorData.ConfigButton, GUIStyle.op_Implicit("Button"), (GUILayoutOption[])(object)new GUILayoutOption[2]
				{
					GUILayout.Width(30f),
					GUILayout.Height(30f)
				}))
				{
					editorConfig = EditorWindow.GetWindow<PrefabSamplerConfigEditor>(true, "Prefab Sampler Config");
					((EditorWindow)editorConfig).get_titleContent().set_text("Prefab Sampler Config");
				}
				else if ((Object)(object)editorConfig != (Object)null)
				{
					((EditorWindow)editorConfig).Close();
				}
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
				EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.LabelField("Prefab Folder:", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.LabelField(PrefabSamplerConfigData.EditorData.DestinationFolder, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.EndVertical();
			}
		}

		public PrefabSamplerEditor()
			: this()
		{
		}
	}
}

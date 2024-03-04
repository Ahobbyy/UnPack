using System;
using UnityEditor;
using UnityEngine;

namespace PrefabSampler
{
	public class PrefabSamplerConfigEditor : EditorWindow
	{
		[MenuItem("Window/Prefab Sampler/Prefab Sampler Config")]
		private static void Initialize()
		{
			((EditorWindow)EditorWindow.GetWindow<PrefabSamplerConfigEditor>(true, "Prefab Sampler Config")).get_titleContent().set_text("Prefab Sampler Config");
		}

		private void OnGUI()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0316: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			if ((Object)(object)PrefabSamplerConfigData.EditorData == (Object)null)
			{
				EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.Button("Click here to initialize!", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) });
				return;
			}
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Set New Prefab Destination Folder", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Change Prefab Destination Folder", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Height(30f) }))
			{
				string destinationFolder = EditorUtility.OpenFolderPanel("Prefab Destination Folder", "Assets/", "");
				PrefabSamplerConfigData.EditorData.DestinationFolder = destinationFolder;
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Prefab Folder:", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField(PrefabSamplerConfigData.EditorData.DestinationFolder, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Fix Mesh Pivot", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			PrefabSamplerConfigData.EditorData.FixMeshPosition = EditorGUILayout.Toggle("Fix pivot ", PrefabSamplerConfigData.EditorData.FixMeshPosition, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (PrefabSamplerConfigData.EditorData.FixMeshPosition)
			{
				PrefabSamplerConfigData.EditorData.PivotType = (MeshPivotType)(object)EditorGUILayout.EnumPopup("Position", (Enum)PrefabSamplerConfigData.EditorData.PivotType, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Custom Append Label", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			string text = EditorGUILayout.TextField("Append to Prefab Name", PrefabSamplerConfigData.EditorData.AppendName, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (PrefabSamplerConfigData.EditorData.AppendName != text)
			{
				PrefabSamplerConfigData.EditorData.AppendName = text;
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Add dependant objects from Node Grapth:", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag = EditorGUILayout.Toggle("Add dependent objects:", PrefabSamplerConfigData.EditorData.IncludeNodeGraphObjects, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (PrefabSamplerConfigData.EditorData.IncludeNodeGraphObjects != flag)
			{
				PrefabSamplerConfigData.EditorData.IncludeNodeGraphObjects = flag;
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Align parent GO with children", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag2 = EditorGUILayout.Toggle("Align:", PrefabSamplerConfigData.EditorData.AllignWithChildren, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (PrefabSamplerConfigData.EditorData.AllignWithChildren != flag2)
			{
				PrefabSamplerConfigData.EditorData.AllignWithChildren = flag2;
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.BeginVertical(GUIStyle.op_Implicit("box"), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.LabelField("Scaling settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag3 = EditorGUILayout.Toggle("Fix Scale:", PrefabSamplerConfigData.EditorData.FixScale, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (PrefabSamplerConfigData.EditorData.FixScale != flag3)
			{
				PrefabSamplerConfigData.EditorData.FixScale = flag3;
				EditorUtility.SetDirty((Object)(object)PrefabSamplerConfigData.EditorData);
			}
			EditorGUILayout.EndVertical();
		}

		public PrefabSamplerConfigEditor()
			: this()
		{
		}
	}
}

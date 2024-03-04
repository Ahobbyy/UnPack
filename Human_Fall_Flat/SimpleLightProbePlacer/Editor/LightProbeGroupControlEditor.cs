using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleLightProbePlacer.Editor
{
	[CustomEditor(typeof(LightProbeGroupControl))]
	public class LightProbeGroupControlEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			LightProbeGroupControl lightProbeGroupControl = (LightProbeGroupControl)(object)((Editor)this).get_target();
			if (GUILayout.Button("Delete All Light Probes", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Undo.RecordObject((Object)(object)lightProbeGroupControl.LightProbeGroup, "Light Probe Group - delete all");
				lightProbeGroupControl.DeleteAll();
			}
			if ((Object)(object)lightProbeGroupControl.LightProbeGroup != (Object)null)
			{
				EditorGUILayout.HelpBox($"Light Probes count: {lightProbeGroupControl.LightProbeGroup.get_probePositions().Length}\nMerged Probes: {lightProbeGroupControl.MergedProbes}", (MessageType)1);
			}
			if (GUILayout.Button("Create Light Probes", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Undo.RecordObject((Object)(object)lightProbeGroupControl.LightProbeGroup, "Light Probe Group - create");
				lightProbeGroupControl.Create();
			}
			GUILayout.Space(10f);
			if (GUILayout.Button("Merge Closest Light Probes", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Undo.RecordObject((Object)(object)lightProbeGroupControl.LightProbeGroup, "Light Probe Group - merge");
				lightProbeGroupControl.Merge();
			}
			EditorGUI.BeginChangeCheck();
			float mergeDistance = EditorGUILayout.Slider("Merge distance", lightProbeGroupControl.MergeDistance, 0f, 10f, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(20f);
			EditorGUILayout.LabelField("Point Light Settings", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool usePointLights = EditorGUILayout.Toggle("Use Point Lights", lightProbeGroupControl.UsePointLights, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(lightProbeGroupControl.UsePointLights);
			float pointLightRange = EditorGUILayout.FloatField("Range", lightProbeGroupControl.PointLightRange, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUI.set_enabled(true);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject((Object)(object)lightProbeGroupControl, "Light Probe Group Control changes");
				lightProbeGroupControl.MergeDistance = mergeDistance;
				lightProbeGroupControl.UsePointLights = usePointLights;
				lightProbeGroupControl.PointLightRange = pointLightRange;
				EditorUtility.SetDirty(((Editor)this).get_target());
			}
		}

		[MenuItem("GameObject/Light/Light Probe Group Control")]
		private static void CreateLightProbeGroupControl(MenuCommand menuCommand)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0032: Expected O, but got Unknown
			GameObject val = new GameObject("Light Probe Group Control");
			val.AddComponent<LightProbeGroupControl>();
			object obj = (object)val;
			Object context = menuCommand.context;
			GameObjectUtility.SetParentAndAlign((GameObject)obj, (GameObject)(object)((context is GameObject) ? context : null));
			Undo.RegisterCreatedObjectUndo((Object)val, "Create Light Probe Group Control");
			Selection.set_activeGameObject(val);
		}

		[DrawGizmo(/*Could not decode attribute arguments.*/)]
		private static void DrawGizmoPointLight(Light light, GizmoType gizmoType)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Invalid comparison between Unknown and I4
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			LightProbeGroupControl lightProbeGroupControl = Object.FindObjectOfType<LightProbeGroupControl>();
			if (!((Object)(object)lightProbeGroupControl == (Object)null) && lightProbeGroupControl.UsePointLights && (int)light.get_type() == 2)
			{
				List<Vector3> list = LightProbeGroupControl.CreatePositionsAround(((Component)light).get_transform(), lightProbeGroupControl.PointLightRange);
				for (int i = 0; i < list.Count; i++)
				{
					Gizmos.DrawIcon(list[i], "NONE", false);
				}
			}
		}

		public LightProbeGroupControlEditor()
			: this()
		{
		}
	}
}

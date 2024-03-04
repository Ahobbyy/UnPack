using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace SimpleLightProbePlacer.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(LightProbeVolume))]
	public class LightProbeVolumeEditor : Editor
	{
		public override void OnInspectorGUI()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_0167: Unknown result type (might be due to invalid IL or missing references)
			LightProbeVolume lightProbeVolume = (LightProbeVolume)(object)((Editor)this).get_target();
			EditorGUI.BeginChangeCheck();
			GUILayout.Space(10f);
			EditorGUILayout.LabelField("Volume", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Vector3 origin = EditorGUILayout.Vector3Field("Origin", lightProbeVolume.Origin, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			Vector3 size = EditorGUILayout.Vector3Field("Size", lightProbeVolume.Size, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			GUILayout.Space(10f);
			EditorGUILayout.LabelField("Density", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			LightProbeVolumeType type = (LightProbeVolumeType)(object)EditorGUILayout.EnumPopup("Density Type", (Enum)lightProbeVolume.Type, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			float num = ((lightProbeVolume.Type == LightProbeVolumeType.Fixed) ? 1f : 0.1f);
			float num2 = ((lightProbeVolume.Type == LightProbeVolumeType.Fixed) ? 100 : 50);
			Vector3 density = lightProbeVolume.Density;
			density.x = EditorGUILayout.Slider("DensityX", lightProbeVolume.Density.x, num, num2, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			density.y = EditorGUILayout.Slider("DensityY", lightProbeVolume.Density.y, num, num2, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			density.z = EditorGUILayout.Slider("DensityZ", lightProbeVolume.Density.z, num, num2, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(((Editor)this).get_target(), "Light Probe Volume changes");
				lightProbeVolume.Density = density;
				lightProbeVolume.Type = type;
				lightProbeVolume.Volume = new Volume(origin, size);
				EditorUtility.SetDirty(((Editor)this).get_target());
			}
		}

		private void OnSceneGUI()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			LightProbeVolume lightProbeVolume = (LightProbeVolume)(object)((Editor)this).get_target();
			Volume volume = TransformVolume.EditorVolumeControl(lightProbeVolume, 0.1f, LightProbeVolume.EditorColor);
			if (volume != lightProbeVolume.Volume)
			{
				Undo.RecordObject(((Editor)this).get_target(), "Light Probe Volume changes");
				lightProbeVolume.Volume = volume;
				EditorUtility.SetDirty(((Editor)this).get_target());
			}
		}

		[DrawGizmo(/*Could not decode attribute arguments.*/)]
		private static void DrawGizmoVolume(LightProbeVolume volume, GizmoType gizmoType)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Invalid comparison between Unknown and I4
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			Color editorColor = LightProbeVolume.EditorColor;
			Gizmos.set_color(editorColor);
			Gizmos.set_matrix(Matrix4x4.TRS(((Component)volume).get_transform().get_position(), ((Component)volume).get_transform().get_rotation(), Vector3.get_one()));
			Gizmos.DrawWireCube(volume.Origin, volume.Size);
			if ((int)gizmoType == 28)
			{
				editorColor.a = 0.25f;
				Gizmos.set_color(editorColor);
				Gizmos.DrawCube(volume.Origin, volume.Size);
				List<Vector3> list = volume.CreatePositions();
				for (int i = 0; i < list.Count; i++)
				{
					Gizmos.DrawIcon(list[i], "NONE", false);
				}
			}
		}

		[MenuItem("GameObject/Light/Light Probe Volume")]
		private static void CreateLightProbeVolume(MenuCommand menuCommand)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Expected O, but got Unknown
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Expected O, but got Unknown
			//IL_0032: Expected O, but got Unknown
			GameObject val = new GameObject("Light Probe Volume");
			val.AddComponent<LightProbeVolume>();
			object obj = (object)val;
			Object context = menuCommand.context;
			GameObjectUtility.SetParentAndAlign((GameObject)obj, (GameObject)(object)((context is GameObject) ? context : null));
			Undo.RegisterCreatedObjectUndo((Object)val, "Create Light Probe Volume");
			Selection.set_activeGameObject(val);
		}

		public LightProbeVolumeEditor()
			: this()
		{
		}
	}
}

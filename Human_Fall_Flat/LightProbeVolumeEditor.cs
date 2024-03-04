using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LightProbeVolume))]
public class LightProbeVolumeEditor : Editor
{
	private static int mask = 1;

	private static float offset = 0.1f;

	private static float offsetMult = 0.1f;

	private static Vector3 offsetVector;

	private static bool m_editMode = false;

	private static int m_count = 0;

	private LightProbeGroup group;

	private RaycastHit hitInfo;

	private void OnSceneGUI()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Invalid comparison between Unknown and I4
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Invalid comparison between Unknown and I4
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Invalid comparison between Unknown and I4
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Invalid comparison between Unknown and I4
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Invalid comparison between Unknown and I4
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Invalid comparison between Unknown and I4
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Invalid comparison between Unknown and I4
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Invalid comparison between Unknown and I4
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Invalid comparison between Unknown and I4
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Invalid comparison between Unknown and I4
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0223: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Invalid comparison between Unknown and I4
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_027d: Invalid comparison between Unknown and I4
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Invalid comparison between Unknown and I4
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		if (!m_editMode)
		{
			return;
		}
		Ray val = HandleUtility.GUIPointToWorldRay(Event.get_current().get_mousePosition());
		if ((int)Event.get_current().get_type() == 4)
		{
			if ((int)Event.get_current().get_keyCode() == 113)
			{
				offsetMult = 1f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 119)
			{
				offsetMult = 2f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 101)
			{
				offsetMult = 3f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 114)
			{
				offsetMult = 4f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 116)
			{
				offsetMult = 5f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 121)
			{
				offsetMult = 6f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 117)
			{
				offsetMult = 7f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 105)
			{
				offsetMult = 8f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 111)
			{
				offsetMult = 9f;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 112)
			{
				offsetMult = 10f;
				Event.get_current().Use();
			}
		}
		else if ((int)Event.get_current().get_type() == 5)
		{
			offsetMult = 1f;
			offsetVector = Vector3.get_zero();
		}
		if (Physics.Raycast(val, ref hitInfo, 1000f, mask))
		{
			(((Editor)this).get_target() as LightProbeVolume).hitInfo = hitInfo;
			Vector3 val2 = offsetVector * offsetMult;
			if (val2 == Vector3.get_zero())
			{
				val2 = ((RaycastHit)(ref hitInfo)).get_normal() * offset * offsetMult;
			}
			(((Editor)this).get_target() as LightProbeVolume).offset = val2;
			if ((int)Event.get_current().get_keyCode() == 122)
			{
				Vector3[] array = group.get_probePositions();
				Array.Resize(ref array, array.Length - 1);
				group.set_probePositions(array);
				EditorUtility.SetDirty((Object)(object)group);
				m_count--;
				Event.get_current().Use();
			}
			if ((int)Event.get_current().get_keyCode() == 97 && (int)Event.get_current().get_type() == 4)
			{
				Vector3[] array2 = group.get_probePositions();
				Array.Resize(ref array2, array2.Length + 1);
				array2[array2.Length - 1] = ((Component)group).get_transform().InverseTransformPoint(((RaycastHit)(ref hitInfo)).get_point() + val2);
				group.set_probePositions(array2);
				EditorUtility.SetDirty((Object)(object)group);
				m_count++;
				Event.get_current().Use();
			}
		}
	}

	public override void OnInspectorGUI()
	{
		mask = EditorGUILayout.IntField("Mask", mask, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		offset = EditorGUILayout.FloatField("Offset", offset, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		if (m_editMode)
		{
			if (GUILayout.Button("Disable Editing", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_editMode = false;
			}
		}
		else if (GUILayout.Button("Enable Editing", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			m_editMode = true;
			group = ((Component)(((Editor)this).get_target() as LightProbeVolume)).GetComponent<LightProbeGroup>();
			Debug.Log((object)group);
		}
		if (GUILayout.Button("Reset", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			m_count = 0;
		}
	}

	public LightProbeVolumeEditor()
		: this()
	{
	}
}

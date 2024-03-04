using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PullJoint), true)]
[CanEditMultipleObjects]
public class PullJointEditor : Editor
{
	private void OnSceneGUI()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0172: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		PullJoint pullJoint = ((Editor)this).get_target() as PullJoint;
		if (!((Object)(object)pullJoint.anchor == (Object)null))
		{
			Vector3 val = (((Object)(object)pullJoint.anchorPoint != (Object)null) ? pullJoint.anchorPoint.get_position() : pullJoint.anchor.get_position());
			Vector3 val2 = (((Object)(object)pullJoint.hook != (Object)null) ? pullJoint.hook.get_position() : ((Component)pullJoint).get_transform().get_position());
			Vector3 val3 = val2 - val;
			Vector3 normalized = ((Vector3)(ref val3)).get_normalized();
			Quaternion val4 = Quaternion.LookRotation(normalized);
			Vector3 val5 = val4 * Vector3.get_up();
			Vector3 val6 = val4 * Vector3.get_right();
			int num = 15;
			float num2 = (pullJoint.maxValue - pullJoint.minValue) / (float)num;
			Color val7 = default(Color);
			for (int i = 0; i <= num; i++)
			{
				((Color)(ref val7))._002Ector(1f, 1f, 1f, 0.2f + (float)i * 0.02f);
				Handles.DrawSolidRectangleWithOutline((Vector3[])(object)new Vector3[4]
				{
					val2 + val6 + val5 + (pullJoint.minValue + num2 * (float)i) * normalized,
					val2 - val6 + val5 + (pullJoint.minValue + num2 * (float)i) * normalized,
					val2 - val6 - val5 + (pullJoint.minValue + num2 * (float)i) * normalized,
					val2 + val6 - val5 + (pullJoint.minValue + num2 * (float)i) * normalized
				}, val7, val7);
			}
		}
	}

	public PullJointEditor()
		: this()
	{
	}
}

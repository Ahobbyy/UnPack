using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LinearJoint), true)]
[CanEditMultipleObjects]
public class LinearJointEditor : Editor
{
	private void OnSceneGUI()
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		LinearJoint linearJoint = ((Editor)this).get_target() as LinearJoint;
		Transform val = (((Object)(object)linearJoint.axis != (Object)null) ? linearJoint.axis : ((Component)linearJoint).get_transform());
		int num = 15;
		float num2 = (linearJoint.maxValue - linearJoint.minValue) / (float)num;
		Color val2 = default(Color);
		for (int i = 0; i <= num; i++)
		{
			((Color)(ref val2))._002Ector(1f, 1f, 1f, 0.2f + (float)i * 0.02f);
			Handles.DrawSolidRectangleWithOutline((Vector3[])(object)new Vector3[4]
			{
				val.get_position() + val.get_right() + val.get_up() + (linearJoint.minValue + num2 * (float)i) * val.get_forward(),
				val.get_position() - val.get_right() + val.get_up() + (linearJoint.minValue + num2 * (float)i) * val.get_forward(),
				val.get_position() - val.get_right() - val.get_up() + (linearJoint.minValue + num2 * (float)i) * val.get_forward(),
				val.get_position() + val.get_right() - val.get_up() + (linearJoint.minValue + num2 * (float)i) * val.get_forward()
			}, val2, val2);
		}
	}

	public LinearJointEditor()
		: this()
	{
	}
}

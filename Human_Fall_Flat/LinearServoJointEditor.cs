using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LinearServoJoint))]
public class LinearServoJointEditor : Editor
{
	private void OnSceneGUI()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		LinearServoJoint linearServoJoint = ((Editor)this).get_target() as LinearServoJoint;
		int num = 5;
		float num2 = (linearServoJoint.maxValue - linearServoJoint.minValue) / (float)num;
		Color val = default(Color);
		for (int i = 0; i <= num; i++)
		{
			((Color)(ref val))._002Ector(1f, 1f, 1f, 0.2f + (float)i * 0.05f);
			Handles.DrawSolidRectangleWithOutline((Vector3[])(object)new Vector3[4]
			{
				((Component)linearServoJoint).get_transform().get_position() + ((Component)linearServoJoint).get_transform().get_right() + ((Component)linearServoJoint).get_transform().get_up() + (linearServoJoint.minValue + num2 * (float)i) * ((Component)linearServoJoint).get_transform().get_forward(),
				((Component)linearServoJoint).get_transform().get_position() - ((Component)linearServoJoint).get_transform().get_right() + ((Component)linearServoJoint).get_transform().get_up() + (linearServoJoint.minValue + num2 * (float)i) * ((Component)linearServoJoint).get_transform().get_forward(),
				((Component)linearServoJoint).get_transform().get_position() - ((Component)linearServoJoint).get_transform().get_right() - ((Component)linearServoJoint).get_transform().get_up() + (linearServoJoint.minValue + num2 * (float)i) * ((Component)linearServoJoint).get_transform().get_forward(),
				((Component)linearServoJoint).get_transform().get_position() + ((Component)linearServoJoint).get_transform().get_right() - ((Component)linearServoJoint).get_transform().get_up() + (linearServoJoint.minValue + num2 * (float)i) * ((Component)linearServoJoint).get_transform().get_forward()
			}, val, val);
		}
	}

	public LinearServoJointEditor()
		: this()
	{
	}
}

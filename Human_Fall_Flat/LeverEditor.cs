using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Lever))]
public class LeverEditor : Editor
{
	private void OnSceneGUI()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		Lever lever = ((Editor)this).get_target() as Lever;
		if (!((Object)(object)lever.angularJoint != (Object)null))
		{
			ConfigurableJoint joint = lever.joint;
			Vector3 val = ((Component)joint).get_transform().TransformVector(((Joint)joint).get_axis());
			int num = 5;
			float num2 = (lever.toAngle - lever.fromAngle) / (float)num;
			for (int i = 0; i < num; i++)
			{
				Handles.set_color(new Color(1f, 1f, 1f, 0.2f + (float)i * 0.05f));
				Handles.DrawSolidArc(((Component)joint).get_transform().TransformPoint(((Joint)joint).get_anchor()), val, ((Component)lever).get_transform().TransformVector(joint.get_secondaryAxis()).Rotate(val, lever.fromAngle + num2 * (float)i), num2, 2f);
			}
		}
	}

	public LeverEditor()
		: this()
	{
	}
}

using HumanAPI;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AngularJoint), true)]
[CanEditMultipleObjects]
public class AngularJointEditor : Editor
{
	private void OnSceneGUI()
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		AngularJoint angularJoint = ((Editor)this).get_target() as AngularJoint;
		Transform val = (((Object)(object)angularJoint.axis != (Object)null) ? angularJoint.axis : ((Component)angularJoint).get_transform());
		int num = 15;
		float num2 = (angularJoint.useLimits ? angularJoint.minValue : 0f);
		float num3 = ((angularJoint.useLimits ? angularJoint.maxValue : 360f) - num2) / (float)num;
		for (int i = 0; i < num; i++)
		{
			Handles.set_color(new Color(1f, 1f, 1f, 0.2f + (float)i * 0.02f));
			Handles.DrawSolidArc(val.get_position(), val.get_right(), val.get_forward().Rotate(val.get_right(), num2 + num3 * (float)i), num3, 2f);
		}
	}

	public AngularJointEditor()
		: this()
	{
	}
}

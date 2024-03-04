using UnityEngine;

public static class HingeJointExtensions
{
	public static void SetSpring(this HingeJoint joint, JointSpring spring)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		JointSpring spring2 = joint.get_spring();
		if (spring2.damper != spring.damper || spring2.spring != spring.spring || spring2.targetPosition != spring.targetPosition)
		{
			joint.set_spring(spring);
		}
	}

	public static void SetLimits(this HingeJoint joint, JointLimits limits)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		JointLimits limits2 = joint.get_limits();
		if (((JointLimits)(ref limits2)).get_min() != ((JointLimits)(ref limits)).get_min() || ((JointLimits)(ref limits2)).get_max() != ((JointLimits)(ref limits)).get_max())
		{
			joint.set_limits(limits);
		}
	}
}

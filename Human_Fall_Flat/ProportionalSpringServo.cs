using UnityEngine;

public class ProportionalSpringServo : ProportionalServoMotor
{
	private ConfigurableJoint[] joints;

	private Rigidbody body;

	protected override void OnEnable()
	{
		base.OnEnable();
		body = ((Component)this).GetComponent<Rigidbody>();
		joints = ((Component)this).GetComponents<ConfigurableJoint>();
	}

	protected override void TargetValueChanged(float targetValue)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)body == (Object)null))
		{
			for (int i = 0; i < joints.Length; i++)
			{
				ConfigurableJoint obj = joints[i];
				SoftJointLimit linearLimit = obj.get_linearLimit();
				((SoftJointLimit)(ref linearLimit)).set_limit(targetValue);
				obj.set_linearLimit(linearLimit);
			}
			body.WakeUp();
		}
	}
}

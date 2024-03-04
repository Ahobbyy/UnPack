using UnityEngine;

public class ProportionalAngularServo : ProportionalServoMotor
{
	public Vector3 axis;

	private ConfigurableJoint joint;

	private Rigidbody body;

	private Quaternion startRot;

	protected override void OnEnable()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		body = ((Component)this).GetComponent<Rigidbody>();
		joint = ((Component)this).GetComponent<ConfigurableJoint>();
		if (body.get_isKinematic())
		{
			startRot = body.get_rotation();
		}
	}

	protected override void TargetValueChanged(float targetValue)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)body == (Object)null))
		{
			if (body.get_isKinematic())
			{
				Quaternion val = startRot * Quaternion.AngleAxis(targetValue, axis);
				body.MoveRotation(val);
			}
			else
			{
				body.WakeUp();
				joint.set_targetRotation(Quaternion.AngleAxis(targetValue, axis));
			}
		}
	}
}

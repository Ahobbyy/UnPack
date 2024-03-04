using UnityEngine;

public class ProportionalLinearServo : ProportionalServoMotor
{
	public Vector3 direction;

	public float maxOffset = 0.3f;

	private float maxForce;

	private ConfigurableJoint joint;

	private Rigidbody body;

	private Vector3 startPos;

	protected override void OnEnable()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		body = ((Component)this).GetComponent<Rigidbody>();
		joint = ((Component)this).GetComponent<ConfigurableJoint>();
		if (body.get_isKinematic())
		{
			startPos = ((Component)this).get_transform().get_localPosition();
			return;
		}
		direction = ((Joint)joint).get_axis();
		JointDrive xDrive = joint.get_xDrive();
		maxForce = ((JointDrive)(ref xDrive)).get_maximumForce();
	}

	protected override void TargetValueChanged(float targetValue)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)body == (Object)null))
		{
			if (body.get_isKinematic())
			{
				Vector3 val = ((Component)this).get_transform().get_parent().TransformPoint(startPos + targetValue * direction);
				body.MovePosition(val);
			}
			else
			{
				body.WakeUp();
				joint.set_targetPosition(new Vector3(targetValue, 0f, 0f));
			}
		}
	}

	protected override void PowerChanged(float powerPercent)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		base.PowerChanged(powerPercent);
		JointDrive xDrive = joint.get_xDrive();
		((JointDrive)(ref xDrive)).set_maximumForce(powerPercent * maxForce);
		joint.set_xDrive(xDrive);
		body.WakeUp();
	}
}

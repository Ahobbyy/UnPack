using UnityEngine;

public class HingeMotorOnSignal : SignalTweenBase
{
	public float speedOn = 1f;

	private HingeJoint joint;

	protected override void OnEnable()
	{
		base.OnEnable();
		((Component)this).GetComponent<Rigidbody>().set_maxAngularVelocity(50f);
	}

	public override void OnValueChanged(float value)
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		base.OnValueChanged(value);
		if ((Object)(object)joint == (Object)null)
		{
			joint = ((Component)this).GetComponent<HingeJoint>();
		}
		float num = speedOn * value;
		JointMotor motor = joint.get_motor();
		((JointMotor)(ref motor)).set_targetVelocity(num);
		joint.set_useMotor(num != 0f);
		joint.set_motor(motor);
	}
}

using HumanAPI;
using UnityEngine;

public class ArmJoint : AngularJoint
{
	public float phase = 0.5f;

	protected override void UpdateLimitJoint()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f - Mathf.Lerp(minValue, maxValue, phase) + centerValue;
		ConfigurableJoint obj = joint;
		SoftJointLimit val = default(SoftJointLimit);
		((SoftJointLimit)(ref val)).set_limit(num);
		obj.set_lowAngularXLimit(val);
		ConfigurableJoint obj2 = joint;
		val = default(SoftJointLimit);
		((SoftJointLimit)(ref val)).set_limit(num + 0.01f);
		obj2.set_highAngularXLimit(val);
		((Component)body).GetComponent<Rigidbody>().WakeUp();
	}
}

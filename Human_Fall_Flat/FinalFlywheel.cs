using HumanAPI;
using UnityEngine;

public class FinalFlywheel : Node, IReset
{
	public NodeInput input;

	public NodeOutput currentVelocity;

	public NodeOutput disconnected;

	public float disconnectForce;

	public float accMultiplier = 0.1f;

	public float deAccMultiplier = 0.1f;

	private ConfigurableJoint joint;

	private Rigidbody rb;

	private float targetVelocity;

	public Vector3 rotateDirection = Vector3.get_right();

	private void Awake()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		joint = ((Component)this).GetComponent<ConfigurableJoint>();
		rb = ((Component)this).GetComponent<Rigidbody>();
		Rigidbody obj = rb;
		Vector3 zero;
		rb.set_velocity(zero = Vector3.get_zero());
		obj.set_angularVelocity(zero);
	}

	private void FixedUpdate()
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (disconnected.value > 0f)
		{
			targetVelocity = Mathf.Lerp(targetVelocity, 0f, Time.get_fixedDeltaTime() * deAccMultiplier);
		}
		else
		{
			targetVelocity = Mathf.Lerp(targetVelocity, input.value, Time.get_fixedDeltaTime() * accMultiplier);
		}
		if (targetVelocity != currentVelocity.value)
		{
			currentVelocity.SetValue(targetVelocity);
		}
		rb.set_angularVelocity(rotateDirection * targetVelocity);
		if (targetVelocity > disconnectForce)
		{
			DisconnectJoint();
		}
	}

	private void DisconnectJoint()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		if (!(disconnected.value > 0f))
		{
			disconnected.SetValue(1f);
			ConfigurableJoint obj = joint;
			ConfigurableJoint obj2 = joint;
			ConfigurableJoint obj3 = joint;
			ConfigurableJointMotion val = (ConfigurableJointMotion)2;
			obj3.set_xMotion((ConfigurableJointMotion)2);
			ConfigurableJointMotion zMotion;
			obj2.set_yMotion(zMotion = val);
			obj.set_zMotion(zMotion);
		}
	}

	private void OnDrawGizmosSelected()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Debug.DrawLine(((Component)this).get_transform().get_position(), ((Component)this).get_transform().get_position() + rotateDirection);
	}

	void IReset.ResetState(int checkpoint, int subObjectives)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		ConfigurableJoint obj = joint;
		ConfigurableJoint obj2 = joint;
		ConfigurableJoint obj3 = joint;
		ConfigurableJointMotion val = (ConfigurableJointMotion)0;
		obj3.set_xMotion((ConfigurableJointMotion)0);
		ConfigurableJointMotion zMotion;
		obj2.set_yMotion(zMotion = val);
		obj.set_zMotion(zMotion);
		disconnected.SetValue(0f);
		targetVelocity = 0f;
	}
}

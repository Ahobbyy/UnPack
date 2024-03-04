using UnityEngine;

public class ControlJointLimit : MonoBehaviour, IControllable
{
	public float min;

	public float max = 1f;

	private ConfigurableJoint joint;

	private Rigidbody body;

	private void OnEnable()
	{
		joint = ((Component)this).GetComponent<ConfigurableJoint>();
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	public void SetControlValue(float v)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (body.IsSleeping())
		{
			body.WakeUp();
		}
		if ((Object)(object)joint != (Object)null)
		{
			ConfigurableJoint obj = joint;
			SoftJointLimit linearLimit = default(SoftJointLimit);
			((SoftJointLimit)(ref linearLimit)).set_limit(Mathf.Lerp(min, max, v));
			obj.set_linearLimit(linearLimit);
		}
	}

	public ControlJointLimit()
		: this()
	{
	}
}

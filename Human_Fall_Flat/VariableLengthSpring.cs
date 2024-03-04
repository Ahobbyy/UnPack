using UnityEngine;

public class VariableLengthSpring : MonoBehaviour
{
	private void FixedUpdate()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		ConfigurableJoint[] componentsInChildren = ((Component)this).GetComponentsInChildren<ConfigurableJoint>();
		foreach (ConfigurableJoint obj in componentsInChildren)
		{
			SoftJointLimit linearLimit = default(SoftJointLimit);
			((SoftJointLimit)(ref linearLimit)).set_limit(Mathf.Sin(Time.get_time()) * 1.5f + 1.5f);
			obj.set_linearLimit(linearLimit);
		}
	}

	public VariableLengthSpring()
		: this()
	{
	}
}

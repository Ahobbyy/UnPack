using UnityEngine;

public class RotateWhenStepped : MonoBehaviour
{
	public Vector3 rotationNotStepped;

	public Vector3 rotationStepped;

	private ConfigurableJoint joint;

	private void OnEnable()
	{
		joint = ((Component)this).GetComponent<ConfigurableJoint>();
	}

	private void Update()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		if (GroundManager.IsStandingAny(((Component)this).get_gameObject()))
		{
			joint.set_targetRotation(Quaternion.Euler(rotationStepped));
		}
		else
		{
			joint.set_targetRotation(Quaternion.Euler(rotationNotStepped));
		}
	}

	public RotateWhenStepped()
		: this()
	{
	}
}

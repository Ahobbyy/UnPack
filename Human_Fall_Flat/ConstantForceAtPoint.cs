using UnityEngine;

public class ConstantForceAtPoint : MonoBehaviour
{
	private Rigidbody body;

	public Vector3 force;

	private void Start()
	{
		body = ((Component)this).GetComponentInParent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (!body.IsSleeping())
		{
			body.AddForceAtPosition(force, ((Component)this).get_transform().get_position());
		}
	}

	public ConstantForceAtPoint()
		: this()
	{
	}
}

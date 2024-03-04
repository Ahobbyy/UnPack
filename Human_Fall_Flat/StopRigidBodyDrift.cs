using UnityEngine;

public class StopRigidBodyDrift : MonoBehaviour
{
	private Rigidbody rb;

	private float initX;

	private float initY;

	private float initZ;

	private void Start()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		rb.set_centerOfMass(Vector3.get_zero());
		rb.set_inertiaTensorRotation(Quaternion.get_identity());
	}

	public StopRigidBodyDrift()
		: this()
	{
	}
}

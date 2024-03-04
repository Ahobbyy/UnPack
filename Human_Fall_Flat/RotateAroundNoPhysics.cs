using UnityEngine;

public class RotateAroundNoPhysics : MonoBehaviour
{
	private Rigidbody rigidBody;

	public Vector3 velocity;

	private void Start()
	{
		rigidBody = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = Quaternion.Euler(velocity * Time.get_deltaTime());
		rigidBody.MoveRotation(rigidBody.get_rotation() * val);
	}

	public RotateAroundNoPhysics()
		: this()
	{
	}
}

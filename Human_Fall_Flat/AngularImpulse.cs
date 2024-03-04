using UnityEngine;

public class AngularImpulse : MonoBehaviour
{
	private enum axisImpulse
	{
		x,
		y,
		z
	}

	[SerializeField]
	private Rigidbody rigidbody;

	[SerializeField]
	private float angularVelocityThreshold = 0.1f;

	[SerializeField]
	private float yMax = 2f;

	[SerializeField]
	private float multiplyFactor = 10f;

	[SerializeField]
	private Vector3 constantForceVector;

	[SerializeField]
	private Vector3 targetLocalPosition;

	[SerializeField]
	private float speed = 1f;

	[SerializeField]
	private bool debug;

	private bool rotatingClockWise;

	private bool rotatingCounterClockWise;

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 angularVelocity = rigidbody.get_angularVelocity();
		if (((Vector3)(ref angularVelocity)).get_magnitude() > angularVelocityThreshold)
		{
			rigidbody.set_isKinematic(true);
		}
	}

	public AngularImpulse()
		: this()
	{
	}
}

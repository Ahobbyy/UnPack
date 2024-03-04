using UnityEngine;

public static class RigidbodyExtensions
{
	public static void ResetDynamics(this Rigidbody body)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero;
		body.set_angularVelocity(zero = Vector3.get_zero());
		body.set_velocity(zero);
	}
}

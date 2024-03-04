using UnityEngine;

public class LimitVelocity : MonoBehaviour
{
	public float maxSpeed = 100f;

	private Rigidbody body;

	private void Start()
	{
		body = ((Component)this).GetComponent<Rigidbody>();
		if ((Object)(object)body == (Object)null)
		{
			((Behaviour)this).set_enabled(false);
		}
	}

	private void Update()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body != (Object)null)
		{
			Vector3 velocity = body.get_velocity();
			if (((Vector3)(ref velocity)).get_magnitude() > maxSpeed)
			{
				body.set_velocity(Vector3.ClampMagnitude(body.get_velocity(), maxSpeed));
			}
		}
	}

	public LimitVelocity()
		: this()
	{
	}
}

using UnityEngine;

public class Pushing : MonoBehaviour
{
	public float multiplier = 5f;

	public Vector3 direction;

	public float tresholdLow = 400f;

	public float maxForce = 1000f;

	public Rigidbody negativeBody;

	public Rigidbody positiveBody;

	public float negativeForce;

	public float positiveForce;

	public Vector3 negativePoint;

	public Vector3 positivePoint;

	public void OnCollisionEnter(Collision collision)
	{
		HandleCollision(collision);
	}

	public void OnCollisionStay(Collision collision)
	{
		HandleCollision(collision);
	}

	private void HandleCollision(Collision collision)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0 && collision.get_gameObject().get_layer() == 0)
		{
			Vector3 impulse = collision.GetImpulse();
			float num = Vector3.Dot(impulse, direction) / Time.get_fixedDeltaTime();
			if (Vector3.Dot(((ContactPoint)(ref collision.get_contacts()[0])).get_normal(), impulse) > 0f)
			{
				num *= -1f;
			}
			num *= Mathf.InverseLerp(0.4f, 0.7f, Mathf.Abs(Vector3.Dot(((Vector3)(ref impulse)).get_normalized(), direction)));
			if (num < negativeForce)
			{
				negativeForce = num;
				negativeBody = collision.get_rigidbody();
				negativePoint = collision.GetPoint();
			}
			else if (num > positiveForce)
			{
				positiveForce = num;
				positiveBody = collision.get_rigidbody();
				positivePoint = collision.GetPoint();
			}
		}
	}

	public void FixedUpdate()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		if (negativeForce < 0f - tresholdLow && positiveForce > tresholdLow)
		{
			Vector3 velocity;
			if ((Object)(object)negativeBody != (Object)null && !negativeBody.get_isKinematic())
			{
				velocity = negativeBody.get_velocity();
				float num = Mathf.InverseLerp(2f, 0f, ((Vector3)(ref velocity)).get_magnitude());
				negativeBody.AddForceAtPosition(num * direction * Mathf.Clamp(negativeForce * multiplier, 0f - maxForce, 0f), negativePoint, (ForceMode)0);
			}
			if ((Object)(object)positiveBody != (Object)null && !positiveBody.get_isKinematic())
			{
				velocity = positiveBody.get_velocity();
				float num2 = Mathf.InverseLerp(2f, 0f, ((Vector3)(ref velocity)).get_magnitude());
				positiveBody.AddForceAtPosition(num2 * direction * Mathf.Clamp(positiveForce * multiplier, 0f, maxForce), positivePoint, (ForceMode)0);
			}
		}
		positiveForce = (negativeForce = 0f);
	}

	public Pushing()
		: this()
	{
	}
}

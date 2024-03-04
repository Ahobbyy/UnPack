using UnityEngine;

public class Tire : MonoBehaviour
{
	private HingeJoint joint;

	private Rigidbody body;

	private void OnEnable()
	{
		joint = ((Component)this).GetComponent<HingeJoint>();
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	public void OnCollisionEnter(Collision collision)
	{
		OnCollisionStay(collision);
	}

	public void OnCollisionStay(Collision collision)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length != 0)
		{
			float num = Mathf.Abs(Vector3.Dot(collision.GetImpulse(), Vector3.get_up()));
			Transform transform = ((Component)this).get_transform();
			Vector3 axis = ((Joint)joint).get_axis();
			Vector3 val = transform.TransformDirection(((Vector3)(ref axis)).get_normalized());
			Vector3 point = collision.GetPoint();
			float num2 = Vector3.Dot(body.GetPointVelocity(point), val);
			if (!(Mathf.Abs(num2) < 0.01f))
			{
				Vector3 val2 = num2 * val;
				Vector3 val3 = (0f - num) * val2;
				body.AddForceAtPosition(val3, point, (ForceMode)1);
			}
		}
	}

	public Tire()
		: this()
	{
	}
}

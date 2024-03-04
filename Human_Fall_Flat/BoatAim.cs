using UnityEngine;

public class BoatAim : MonoBehaviour
{
	public Rigidbody boat;

	public Rigidbody paddle1;

	public Rigidbody paddle2;

	public Vector3 alignAxis;

	public float strength = 5f;

	public float maxTorque = 1000f;

	private void FixedUpdate()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			if (human.grabManager.IsGrabbed(((Component)paddle1).get_gameObject()) && human.grabManager.IsGrabbed(((Component)paddle2).get_gameObject()))
			{
				Vector3 val = ((Component)this).get_transform().TransformDirection(alignAxis);
				Vector3 val2 = Quaternion.Euler(0f, human.controls.targetYawAngle, 0f) * Vector3.get_forward();
				float num = Math2d.SignedAngle(val2.To2D(), val.To2D());
				num *= Vector3.Dot(val.ZeroY(), val2);
				float num2 = Mathf.Abs((paddle1.get_angularVelocity() - boat.get_angularVelocity()).y) + Mathf.Abs((paddle2.get_angularVelocity() - boat.get_angularVelocity()).y);
				boat.AddTorque(Vector3.get_up() * Mathf.Clamp(num * strength * num2, 0f - maxTorque, maxTorque));
			}
		}
	}

	public BoatAim()
		: this()
	{
	}
}

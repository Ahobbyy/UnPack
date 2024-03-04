using UnityEngine;

public class FireCorrectVelocity : MonoBehaviour
{
	private Rigidbody rb;

	public float speed = 40f;

	private void Start()
	{
		rb = ((Component)this).GetComponent<Rigidbody>();
	}

	public void Fire()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if (Object.op_Implicit((Object)(object)rb))
		{
			rb.set_angularVelocity(Vector3.get_zero());
			rb.set_velocity(((Component)this).get_transform().get_forward() * speed);
		}
	}

	public FireCorrectVelocity()
		: this()
	{
	}
}

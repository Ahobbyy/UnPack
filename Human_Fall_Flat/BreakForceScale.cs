using UnityEngine;

public class BreakForceScale : MonoBehaviour
{
	public Joint joint;

	public float scaleFactor = 0.1f;

	public Rigidbody contactBody;

	private int contacts;

	private float initialBreakForce;

	private void Start()
	{
		initialBreakForce = joint.get_breakForce();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!((Object)(object)joint == (Object)null) && (Object)(object)collision.get_rigidbody() == (Object)(object)contactBody)
		{
			contacts++;
			if (contacts == 1)
			{
				joint.set_breakForce(initialBreakForce * scaleFactor);
			}
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (!((Object)(object)joint == (Object)null) && (Object)(object)collision.get_rigidbody() == (Object)(object)contactBody)
		{
			contacts--;
			if (contacts == 0)
			{
				joint.set_breakForce(initialBreakForce);
			}
		}
	}

	public BreakForceScale()
		: this()
	{
	}
}

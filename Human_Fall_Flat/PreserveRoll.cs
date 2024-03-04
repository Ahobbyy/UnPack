using UnityEngine;

public class PreserveRoll : MonoBehaviour
{
	private Rigidbody body;

	private void OnEnable()
	{
		body = ((Component)this).GetComponent<Rigidbody>();
		body.set_maxAngularVelocity(20f);
	}

	public PreserveRoll()
		: this()
	{
	}
}

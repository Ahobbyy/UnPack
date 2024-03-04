using UnityEngine;

public class LimitAngularVelocity : MonoBehaviour
{
	public float maxAngular;

	private void Start()
	{
		((Component)this).GetComponent<Rigidbody>().set_maxAngularVelocity(maxAngular);
	}

	public LimitAngularVelocity()
		: this()
	{
	}
}

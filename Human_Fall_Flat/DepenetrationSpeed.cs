using UnityEngine;

public class DepenetrationSpeed : MonoBehaviour
{
	public float max = 1000f;

	private void OnEnable()
	{
		((Component)this).GetComponent<Rigidbody>().set_maxDepenetrationVelocity(max);
	}

	public DepenetrationSpeed()
		: this()
	{
	}
}

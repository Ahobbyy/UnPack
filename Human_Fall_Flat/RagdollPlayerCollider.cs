using UnityEngine;

public class RagdollPlayerCollider : MonoBehaviour
{
	public float ragdollDuration = 0.5f;

	public void OnCollisionEnter(Collision collision)
	{
		Rigidbody rigidbody = collision.get_rigidbody();
		if ((Object)(object)rigidbody != (Object)null)
		{
			Human componentInParent = ((Component)rigidbody).GetComponentInParent<Human>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				componentInParent.MakeUnconscious(ragdollDuration);
			}
		}
	}

	public RagdollPlayerCollider()
		: this()
	{
	}
}

using UnityEngine;

public class ForceArea : MonoBehaviour
{
	public Vector3 forceDirection;

	public float forceMultiplier = 1f;

	public Transform[] ignoreParents;

	public void OnEnable()
	{
		Collider component = ((Component)this).GetComponent<Collider>();
		for (int i = 0; i < ignoreParents.Length; i++)
		{
			Collider[] componentsInChildren = ((Component)ignoreParents[i]).GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				Physics.IgnoreCollision(component, componentsInChildren[j]);
			}
		}
	}

	public void OnTriggerStay(Collider other)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody componentInParent = ((Component)other).GetComponentInParent<Rigidbody>();
		if (!((Object)(object)componentInParent == (Object)null) && !componentInParent.get_isKinematic())
		{
			componentInParent.AddForce(forceDirection * forceMultiplier);
		}
	}

	public ForceArea()
		: this()
	{
	}
}

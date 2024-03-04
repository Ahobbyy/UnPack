using UnityEngine;

public class ShrinkColliderOnImpact : MonoBehaviour
{
	public string tag;

	private Vector3 size;

	private BoxCollider collider;

	private float scale = 1f;

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		collider = ((Component)this).GetComponent<BoxCollider>();
		size = collider.get_size();
	}

	public void OnCollisionEnter(Collision collision)
	{
	}

	private void FixedUpdate()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		if (scale != 1f)
		{
			scale = Mathf.MoveTowards(scale, 1f, 0.01f);
			collider.set_size(size * scale);
		}
	}

	public ShrinkColliderOnImpact()
		: this()
	{
	}
}

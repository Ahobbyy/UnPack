using UnityEngine;

public class Projectile : MonoBehaviour, IReset
{
	public float explodeForce = 1000f;

	private bool explode;

	private bool exploded;

	public void OnCollisionEnter(Collision collision)
	{
		if (!exploded && string.Equals(collision.get_gameObject().get_tag(), "Explodable"))
		{
			explode = true;
		}
	}

	private void FixedUpdate()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (!explode)
		{
			return;
		}
		exploded = true;
		explode = false;
		Collider[] array = Physics.OverlapSphere(((Component)this).get_transform().get_position(), 3f);
		foreach (Collider val in array)
		{
			if (string.Equals(((Component)val).get_gameObject().get_tag(), "Explodable"))
			{
				Rigidbody component = ((Component)val).GetComponent<Rigidbody>();
				if (!((Object)(object)component == (Object)null))
				{
					component.AddExplosionForce(explodeForce, ((Component)this).get_transform().get_position(), 3f, 0.5f, (ForceMode)1);
				}
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		explode = (exploded = false);
	}

	public Projectile()
		: this()
	{
	}
}

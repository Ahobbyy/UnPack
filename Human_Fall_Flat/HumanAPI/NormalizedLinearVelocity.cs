using UnityEngine;

namespace HumanAPI
{
	public class NormalizedLinearVelocity : Node
	{
		public NodeOutput value;

		[SerializeField]
		private float maxVelocity = 10f;

		[SerializeField]
		private bool needsCollision = true;

		private Rigidbody rb;

		private float normalizedVelocity;

		private float previousVelocity;

		private bool isColliding;

		protected override void OnEnable()
		{
			if ((Object)(object)rb == (Object)null)
			{
				rb = ((Component)this).GetComponent<Rigidbody>();
			}
			base.OnEnable();
		}

		private void FixedUpdate()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 velocity = rb.get_velocity();
			normalizedVelocity = Mathf.Clamp(((Vector3)(ref velocity)).get_magnitude() / maxVelocity, 0f, 1f);
			if (!(Mathf.Abs(normalizedVelocity - previousVelocity) < 0.01f))
			{
				previousVelocity = normalizedVelocity;
				if (needsCollision && !isColliding)
				{
					value.SetValue(0f);
				}
				else
				{
					value.SetValue(normalizedVelocity);
				}
			}
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (((Component)collision.get_collider()).get_gameObject().get_layer() != 8 && ((Component)collision.get_collider()).get_gameObject().get_layer() != 9)
			{
				isColliding = true;
			}
		}

		private void OnCollisionStay(Collision collision)
		{
			if (((Component)collision.get_collider()).get_gameObject().get_layer() != 8 && ((Component)collision.get_collider()).get_gameObject().get_layer() != 9)
			{
				isColliding = true;
			}
		}

		private void OnCollisionExit(Collision collision)
		{
			if (((Component)collision.get_collider()).get_gameObject().get_layer() != 8 && ((Component)collision.get_collider()).get_gameObject().get_layer() != 9)
			{
				isColliding = false;
			}
		}
	}
}

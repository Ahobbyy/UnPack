using UnityEngine;

namespace HumanAPI
{
	public class NormalizedAngularVelocity : Node
	{
		public NodeOutput value;

		[SerializeField]
		private float maxVelocity = 10f;

		private Rigidbody rb;

		private float normalizedVelocity;

		private float previousVelocity;

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
			Vector3 angularVelocity = rb.get_angularVelocity();
			normalizedVelocity = Mathf.Clamp(((Vector3)(ref angularVelocity)).get_magnitude() / maxVelocity, 0f, 1f);
			if (!(Mathf.Abs(normalizedVelocity - previousVelocity) < 0.05f))
			{
				previousVelocity = normalizedVelocity;
				value.SetValue(normalizedVelocity);
			}
		}
	}
}

using UnityEngine;

namespace HumanAPI
{
	public class SignalTranslation : Node
	{
		public NodeOutput value;

		public Rigidbody body;

		private Vector3 initialPosition;

		public float distance = 1f;

		public bool clampOutput = true;

		protected override void OnEnable()
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			base.OnEnable();
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			if ((Object)(object)body != (Object)null)
			{
				initialPosition = ((Component)body).get_transform().get_position();
			}
			base.OnEnable();
		}

		private void FixedUpdate()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)body != (Object)null)
			{
				Vector3 val = ((Component)body).get_transform().get_position() - initialPosition;
				float num = ((Vector3)(ref val)).get_magnitude() / distance;
				if (clampOutput)
				{
					num = Mathf.Clamp(num, 0f, 1f);
				}
				value.SetValue(num);
			}
		}
	}
}

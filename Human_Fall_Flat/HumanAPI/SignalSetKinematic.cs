using UnityEngine;

namespace HumanAPI
{
	public class SignalSetKinematic : Node
	{
		public NodeInput input;

		public Rigidbody body;

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
		}

		public override void Process()
		{
			base.Process();
			if ((Object)(object)body != (Object)null)
			{
				body.set_isKinematic(input.value > 0.5f);
			}
		}
	}
}

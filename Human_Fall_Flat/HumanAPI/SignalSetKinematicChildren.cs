using UnityEngine;

namespace HumanAPI
{
	public class SignalSetKinematicChildren : Node
	{
		public NodeInput input;

		private Rigidbody[] rigidbodies;

		protected override void OnEnable()
		{
			base.OnEnable();
			rigidbodies = ((Component)this).GetComponentsInChildren<Rigidbody>();
		}

		public override void Process()
		{
			base.Process();
			if (rigidbodies != null)
			{
				Rigidbody[] array = rigidbodies;
				for (int i = 0; i < array.Length; i++)
				{
					array[i].set_isKinematic(input.value > 0.5f);
				}
			}
		}
	}
}

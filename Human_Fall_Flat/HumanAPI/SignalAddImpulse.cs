using UnityEngine;

namespace HumanAPI
{
	public class SignalAddImpulse : Node
	{
		public NodeInput input;

		public Rigidbody body;

		public Vector3 force;

		public Vector3 torque;

		public bool applyImpulseToChildren;

		private Rigidbody[] childrenRigidbodies;

		private bool appliedImpulse;

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((Object)(object)body == (Object)null && !applyImpulseToChildren)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			if (applyImpulseToChildren)
			{
				childrenRigidbodies = ((Component)this).GetComponentsInChildren<Rigidbody>();
			}
		}

		public override void Process()
		{
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			if (!(input.value > 0.5f) || appliedImpulse)
			{
				return;
			}
			if (applyImpulseToChildren)
			{
				childrenRigidbodies = ((Component)((Component)this).get_transform()).GetComponentsInChildren<Rigidbody>();
				for (int i = 0; i < childrenRigidbodies.Length; i++)
				{
					childrenRigidbodies[i].AddForce(force, (ForceMode)1);
					childrenRigidbodies[i].AddTorque(torque, (ForceMode)1);
				}
				appliedImpulse = true;
			}
			else if ((Object)(object)body != (Object)null)
			{
				body.AddForce(force, (ForceMode)1);
				body.AddTorque(torque, (ForceMode)1);
				appliedImpulse = true;
			}
		}
	}
}

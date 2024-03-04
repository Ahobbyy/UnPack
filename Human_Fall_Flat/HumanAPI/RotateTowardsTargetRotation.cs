using UnityEngine;

namespace HumanAPI
{
	public class RotateTowardsTargetRotation : Node
	{
		public NodeInput active;

		private Quaternion targetRot;

		private Rigidbody rb;

		public float rotSpeed = 1f;

		private void Start()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			rb = ((Component)this).GetComponent<Rigidbody>();
			targetRot = rb.get_rotation();
		}

		public override void Process()
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			if (active.value == 1f)
			{
				targetRot = rb.get_rotation();
			}
		}

		private void FixedUpdate()
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (active.value == 1f)
			{
				rb.MoveRotation(Quaternion.Lerp(rb.get_rotation(), targetRot, rotSpeed * Time.get_fixedDeltaTime()));
			}
		}
	}
}

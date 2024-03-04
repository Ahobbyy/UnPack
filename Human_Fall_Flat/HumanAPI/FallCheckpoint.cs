using UnityEngine;

namespace HumanAPI
{
	public class FallCheckpoint : Checkpoint, IReset
	{
		public Rigidbody fallingObject;

		public float triggerYPos = 5f;

		private bool triggered;

		private Transform fallTransform;

		private void Awake()
		{
			if ((Object)(object)fallingObject != (Object)null)
			{
				fallTransform = ((Component)fallingObject).get_transform();
			}
		}

		private void FixedUpdate()
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			if (!triggered && !((Object)(object)fallTransform == (Object)null) && fallTransform.get_position().y < triggerYPos)
			{
				triggered = true;
				Pass();
			}
		}

		void IReset.ResetState(int checkpoint, int subObjectives)
		{
			if (checkpoint <= number)
			{
				triggered = false;
			}
		}
	}
}

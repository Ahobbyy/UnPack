using UnityEngine;

namespace HumanAPI
{
	public class SteamExplosion : Node, IReset
	{
		[Tooltip("Input: Source signal")]
		public NodeInput input;

		[Tooltip("output: Signal when boiler has exploded ")]
		public NodeOutput explodedSignal;

		[SerializeField]
		private bool exploded;

		[SerializeField]
		private Rigidbody rb;

		[SerializeField]
		private Vector3 force;

		[SerializeField]
		private float threshold;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private void Start()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Start "));
			}
			rb = ((Component)this).GetComponent<Rigidbody>();
		}

		public override void Process()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			if (input.value >= threshold && !exploded)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " EXPLOSION "));
				}
				exploded = true;
				rb.set_isKinematic(false);
				rb.AddForce(force);
				explodedSignal.SetValue(1f);
			}
		}

		void IReset.ResetState(int checkpoint, int subObjectives)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			exploded = false;
			rb.set_isKinematic(true);
			explodedSignal.SetValue(0f);
		}
	}
}

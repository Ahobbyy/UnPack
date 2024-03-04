using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Signals/SignalSmooth", 10)]
	public class SignalSmooth : Node
	{
		public float maxSpeed = 1f;

		public bool ease = true;

		public NodeInput input;

		public NodeOutput output;

		private float phase;

		protected void Awake()
		{
			priority = NodePriority.Update;
			phase = input.value;
		}

		public override void Process()
		{
			base.Process();
			if (SignalManager.skipTransitions)
			{
				phase = input.value;
				float value = (ease ? Ease.easeInOutQuad(0f, 1f, phase) : phase);
				output.SetValue(value);
			}
		}

		private void Update()
		{
			float value = input.value;
			phase = Mathf.MoveTowards(phase, value, maxSpeed * Time.get_deltaTime());
			float value2 = (ease ? Ease.easeInOutQuad(0f, 1f, phase) : phase);
			output.SetValue(value2);
		}
	}
}

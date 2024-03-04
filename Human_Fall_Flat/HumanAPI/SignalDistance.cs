using UnityEngine;

namespace HumanAPI
{
	public class SignalDistance : Node
	{
		public NodeOutput distance;

		public Transform transform1;

		public Transform transform2;

		private void Update()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			NodeOutput nodeOutput = distance;
			Vector3 val = transform1.get_position() - transform2.get_position();
			nodeOutput.SetValue(((Vector3)(ref val)).get_magnitude());
		}
	}
}

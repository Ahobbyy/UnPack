using UnityEngine;

namespace HumanAPI
{
	public class DisplacementSensor : Node
	{
		public NodeOutput value;

		public Transform relativeTo;

		private Vector3 startPos;

		private void Awake()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			startPos = ((Component)this).get_transform().get_position();
			if ((Object)(object)relativeTo != (Object)null)
			{
				startPos = relativeTo.InverseTransformPoint(startPos);
			}
		}

		private void FixedUpdate()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val;
			if (Object.op_Implicit((Object)(object)relativeTo))
			{
				NodeOutput nodeOutput = value;
				val = relativeTo.TransformPoint(startPos) - ((Component)this).get_transform().get_position();
				nodeOutput.SetValue(((Vector3)(ref val)).get_magnitude());
			}
			else
			{
				NodeOutput nodeOutput2 = value;
				val = startPos - ((Component)this).get_transform().get_position();
				nodeOutput2.SetValue(((Vector3)(ref val)).get_magnitude());
			}
		}
	}
}

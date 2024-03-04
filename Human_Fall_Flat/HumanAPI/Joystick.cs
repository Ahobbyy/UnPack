using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Joystick", 10)]
	public class Joystick : Node
	{
		public NodeOutput horizontal;

		public NodeOutput vertical;

		public ConfigurableJoint joint;

		[Space]
		public Vector3 verticalAxis = Vector3.get_right();

		public Vector3 horizontalAxis = Vector3.get_forward();

		private Quaternion initialLocalRotation;

		private float angle;

		private Vector3 axis;

		protected virtual void Awake()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			initialLocalRotation = ((Joint)(object)joint).ReadInitialRotation();
			((Joint)joint).set_axis(Vector3.get_right());
			Vector3 val = verticalAxis + horizontalAxis;
			if (val.x != 0f)
			{
				joint.set_angularXMotion((ConfigurableJointMotion)1);
			}
			if (val.y != 0f)
			{
				joint.set_angularYMotion((ConfigurableJointMotion)1);
			}
			if (val.z != 0f)
			{
				joint.set_angularZMotion((ConfigurableJointMotion)1);
			}
		}

		private void FixedUpdate()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			Quaternion val = Quaternion.Inverse(((Component)joint).get_transform().get_localRotation()) * initialLocalRotation;
			((Quaternion)(ref val)).ToAngleAxis(ref angle, ref axis);
			if (angle < 25f)
			{
				SafeSetValue(horizontal, 0f);
				SafeSetValue(vertical, 0f);
			}
			else
			{
				AngleToValue(axis, verticalAxis, vertical);
				AngleToValue(axis, horizontalAxis, horizontal);
			}
		}

		private void AngleToValue(Vector3 currentValue, Vector3 axis, NodeOutput output)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			((Vector3)(ref axis)).Scale(currentValue);
			float num = axis.x + axis.y + axis.z;
			SafeSetValue(output, (Mathf.Abs(num) > 0.5f) ? Mathf.Sign(num) : 0f);
		}

		private void SafeSetValue(NodeOutput output, float value)
		{
			output?.SetValue(value);
		}
	}
}

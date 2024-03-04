using UnityEngine;

namespace HumanAPI
{
	public class LinearServo : ServoBase
	{
		public Vector3 axis;

		public Rigidbody body;

		private Transform bodyTransform;

		private ConfigurableJoint joint;

		private Vector3 initialConnectedBodyPos;

		protected override void Awake()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			joint = ((Component)body).GetComponent<ConfigurableJoint>();
			bodyTransform = ((Component)body).get_transform();
			isKinematic = body.get_isKinematic();
			initialConnectedBodyPos = GetConnectedAnchorPos();
			if ((Object)(object)joint != (Object)null)
			{
				float num = maxValue - minValue;
				joint.SetXMotionAnchorsAndLimits(num / 2f - (initialValue - minValue), num);
				ConfigurableJoint obj = joint;
				JointDrive xDrive = default(JointDrive);
				((JointDrive)(ref xDrive)).set_maximumForce(maxForce);
				obj.set_xDrive(xDrive);
			}
			base.Awake();
		}

		private Vector3 GetConnectedAnchorPos()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			if (isKinematic)
			{
				return bodyTransform.InverseTransformPoint(bodyTransform.get_parent().get_position());
			}
			axis = ((Joint)joint).get_axis();
			if ((Object)(object)((Joint)joint).get_connectedBody() == (Object)null)
			{
				return bodyTransform.InverseTransformPoint(Vector3.get_zero());
			}
			return bodyTransform.InverseTransformPoint(((Joint)joint).get_connectedBody().get_position());
		}

		protected override float GetActualPosition()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return Vector3.Dot(GetConnectedAnchorPos() - initialConnectedBodyPos, -axis) - initialValue;
		}

		protected override void SetStatic(float pos)
		{
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = bodyTransform.InverseTransformPoint(bodyTransform.get_parent().get_position());
			Vector3 val2 = initialConnectedBodyPos - axis * pos;
			Vector3 val3 = bodyTransform.TransformDirection(val - val2);
			Vector3 val4 = body.get_position() + val3;
			body.MovePosition(val4);
			bodyTransform.set_position(val4);
		}

		protected override void SetJoint(float pos, float spring, float damper)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			joint.set_targetPosition(-new Vector3(pos - (maxValue - minValue) / 2f, 0f, 0f));
			JointDrive xDrive = joint.get_xDrive();
			((JointDrive)(ref xDrive)).set_positionSpring(spring);
			((JointDrive)(ref xDrive)).set_positionDamper(damper);
			joint.set_xDrive(xDrive);
			if (SignalManager.skipTransitions)
			{
				joint.ApplyXMotionTarget();
			}
			else
			{
				body.WakeUp();
			}
		}
	}
}

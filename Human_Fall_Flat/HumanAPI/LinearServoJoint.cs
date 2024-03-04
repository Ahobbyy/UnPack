using System;
using UnityEngine;

namespace HumanAPI
{
	public class LinearServoJoint : ServoBase
	{
		public Rigidbody body;

		public Rigidbody connectedBody;

		private ConfigurableJoint joint;

		private Vector3 connectedAxis;

		private Vector3 connectedCenter;

		private Vector3 localForward;

		private Vector3 connectedOffset;

		protected override void Awake()
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0162: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			isKinematic = body.get_isKinematic();
			connectedAxis = ((Component)this).get_transform().get_forward();
			connectedCenter = ((Component)this).get_transform().get_position();
			connectedOffset = ((Component)body).get_transform().get_position() - connectedCenter;
			if ((Object)(object)connectedBody != (Object)null)
			{
				connectedAxis = ((Component)connectedBody).get_transform().InverseTransformDirection(connectedAxis);
				connectedCenter = ((Component)connectedBody).get_transform().InverseTransformPoint(connectedCenter);
				connectedOffset = ((Component)connectedBody).get_transform().InverseTransformVector(connectedOffset);
			}
			Vector3 anchor = ((Component)body).get_transform().InverseTransformPoint(((Component)this).get_transform().get_position());
			localForward = ((Component)body).get_transform().InverseTransformDirection(((Component)this).get_transform().get_forward());
			Vector3 secondaryAxis = ((Component)body).get_transform().InverseTransformDirection(((Component)this).get_transform().get_right());
			if (!isKinematic)
			{
				float num = (maxValue + minValue) / 2f;
				SetPosition(num);
				joint = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
				((Joint)joint).set_axis(localForward);
				joint.set_secondaryAxis(secondaryAxis);
				((Joint)joint).set_anchor(anchor);
				((Joint)joint).set_connectedBody(connectedBody);
				((Joint)joint).set_autoConfigureConnectedAnchor(false);
				joint.set_xMotion((ConfigurableJointMotion)1);
				joint.set_yMotion((ConfigurableJointMotion)0);
				joint.set_zMotion((ConfigurableJointMotion)0);
				joint.set_angularXMotion((ConfigurableJointMotion)0);
				joint.set_angularYMotion((ConfigurableJointMotion)0);
				joint.set_angularZMotion((ConfigurableJointMotion)0);
				ConfigurableJoint obj = joint;
				SoftJointLimit linearLimit = default(SoftJointLimit);
				((SoftJointLimit)(ref linearLimit)).set_limit((maxValue - minValue) / 2f);
				obj.set_linearLimit(linearLimit);
				float num2 = (float)Math.PI / 180f;
				float positionSpring = maxForce / num2;
				float positionDamper = maxForce / (maxSpeed * ((float)Math.PI / 180f));
				ConfigurableJoint obj2 = joint;
				JointDrive xDrive = default(JointDrive);
				((JointDrive)(ref xDrive)).set_maximumForce(maxForce);
				((JointDrive)(ref xDrive)).set_positionSpring(positionSpring);
				((JointDrive)(ref xDrive)).set_positionDamper(positionDamper);
				obj2.set_xDrive(xDrive);
			}
			base.Awake();
		}

		protected override float GetActualPosition()
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)connectedBody != (Object)null)
			{
				return Vector3.Dot(((Component)connectedBody).get_transform().InverseTransformPoint(((Component)body).get_transform().get_position()) - connectedCenter, connectedAxis);
			}
			return Vector3.Dot(((Component)body).get_transform().get_position() - connectedCenter, connectedAxis);
		}

		private void SetPosition(float pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = connectedCenter + connectedAxis * pos;
			if ((Object)(object)connectedBody != (Object)null)
			{
				val = ((Component)connectedBody).get_transform().TransformPoint(val);
			}
			body.MovePosition(val);
			if (!body.get_isKinematic())
			{
				((Component)body).get_transform().set_position(val);
			}
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

		protected override void SetStatic(float pos)
		{
			SetPosition(pos);
		}
	}
}

using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class AngularJoint : JointImplementation
	{
		public Transform axis;

		public bool disableOnClient;

		private bool useLimitJoint;

		private Vector3 connectedAxis;

		private Vector3 connectedCenter;

		private Vector3 connectedForward;

		private Vector3 localForward;

		private float lastKnownAngle;

		protected Joint limitJoint;

		private bool limitMin;

		protected float limitUpdateValue;

		public override void CreateMainJoint()
		{
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			EnsureInitialized();
			if ((Object)(object)axis == (Object)null)
			{
				axis = ((Component)this).get_transform();
			}
			if (!base.isKinematic)
			{
				rigid.set_maxAngularVelocity(100f);
			}
			connectedAxis = axis.get_right();
			connectedForward = axis.get_forward();
			connectedCenter = axis.get_position();
			if ((Object)(object)anchorTransform != (Object)null)
			{
				connectedAxis = anchorTransform.InverseTransformDirection(connectedAxis);
				connectedForward = anchorTransform.InverseTransformDirection(connectedForward);
				connectedCenter = anchorTransform.InverseTransformPoint(connectedCenter);
			}
			Vector3 val = body.InverseTransformPoint(axis.get_position());
			body.InverseTransformDirection(axis.get_up());
			localForward = body.InverseTransformDirection(axis.get_forward());
			Vector3 val2 = body.InverseTransformDirection(axis.get_right());
			centerValue = 0f;
			if (!base.isKinematic)
			{
				useLimitJoint = useLimits && maxValue - minValue > 270f;
				if (useLimits && !useLimitJoint)
				{
					centerValue = (maxValue + minValue) / 2f;
					SetValue(centerValue);
				}
				joint = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
				((Joint)joint).set_autoConfigureConnectedAnchor(false);
				((Joint)joint).set_axis(val2);
				joint.set_secondaryAxis(localForward);
				((Joint)joint).set_anchor(val);
				((Joint)joint).set_connectedBody(anchorRigid);
				((Joint)joint).set_connectedAnchor(connectedCenter);
				((Joint)joint).set_enableCollision(enableCollision);
				joint.set_xMotion((ConfigurableJointMotion)0);
				joint.set_yMotion((ConfigurableJointMotion)0);
				joint.set_zMotion((ConfigurableJointMotion)0);
				joint.set_angularXMotion((ConfigurableJointMotion)((useLimits && !useLimitJoint) ? 1 : 2));
				joint.set_angularYMotion((ConfigurableJointMotion)0);
				joint.set_angularZMotion((ConfigurableJointMotion)0);
				if (useLimits && !useLimitJoint)
				{
					ConfigurableJoint obj = joint;
					SoftJointLimit val3 = default(SoftJointLimit);
					((SoftJointLimit)(ref val3)).set_limit(minValue - centerValue);
					obj.set_lowAngularXLimit(val3);
					ConfigurableJoint obj2 = joint;
					val3 = default(SoftJointLimit);
					((SoftJointLimit)(ref val3)).set_limit(maxValue - centerValue);
					obj2.set_highAngularXLimit(val3);
					SetValue(0f);
				}
			}
		}

		private void FixedUpdate()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0113: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			if (disableOnClient && NetGame.isClient)
			{
				AngularJoint component = ((Component)this).GetComponent<AngularJoint>();
				if ((Object)(object)component != (Object)null)
				{
					((Behaviour)component).set_enabled(false);
				}
				JointSensor component2 = ((Component)this).GetComponent<JointSensor>();
				if ((Object)(object)component2 != (Object)null)
				{
					((Behaviour)component2).set_enabled(false);
				}
			}
			if ((Object)(object)joint == (Object)null)
			{
				return;
			}
			UpdateLimitJoint();
			JointDrive angularXDrive2;
			if (useSpring)
			{
				Quaternion val = Quaternion.Euler(0f - target + centerValue, 0f, 0f);
				float num = (useTension ? (maxForce / (tensionDist * ((float)Math.PI / 180f))) : spring);
				float num2 = (useTension ? (maxForce / (maxSpeed * ((float)Math.PI / 180f))) : damper);
				JointDrive angularXDrive = joint.get_angularXDrive();
				if (joint.get_targetRotation() != val || ((JointDrive)(ref angularXDrive)).get_maximumForce() != maxForce || ((JointDrive)(ref angularXDrive)).get_positionSpring() != num || ((JointDrive)(ref angularXDrive)).get_positionDamper() != num2)
				{
					ConfigurableJoint obj = joint;
					angularXDrive2 = default(JointDrive);
					((JointDrive)(ref angularXDrive2)).set_maximumForce(maxForce);
					((JointDrive)(ref angularXDrive2)).set_positionSpring(num);
					((JointDrive)(ref angularXDrive2)).set_positionDamper(num2);
					obj.set_angularXDrive(angularXDrive2);
					joint.set_targetRotation(val);
					rigid.WakeUp();
				}
			}
			else
			{
				JointDrive angularXDrive3 = joint.get_angularXDrive();
				if (((JointDrive)(ref angularXDrive3)).get_positionSpring() != spring || ((JointDrive)(ref angularXDrive3)).get_positionDamper() != damper)
				{
					ConfigurableJoint obj2 = joint;
					angularXDrive2 = default(JointDrive);
					((JointDrive)(ref angularXDrive2)).set_maximumForce(maxForce);
					((JointDrive)(ref angularXDrive2)).set_positionSpring(spring);
					((JointDrive)(ref angularXDrive2)).set_positionDamper(damper);
					obj2.set_angularXDrive(angularXDrive2);
				}
			}
		}

		protected virtual void UpdateLimitJoint()
		{
			if (!useLimitJoint)
			{
				return;
			}
			float value = GetValue();
			if (value != limitUpdateValue)
			{
				if (value < minValue + 60f)
				{
					if ((Object)(object)limitJoint != (Object)null && !limitMin)
					{
						Object.Destroy((Object)(object)limitJoint);
					}
					if ((Object)(object)limitJoint == (Object)null)
					{
						limitJoint = CreateLimitJoint(minValue - value, Mathf.Min(minValue - value + 120f, maxValue - value));
						limitMin = true;
					}
				}
				else if (value > maxValue - 60f)
				{
					if ((Object)(object)limitJoint != (Object)null && limitMin)
					{
						Object.Destroy((Object)(object)limitJoint);
					}
					if ((Object)(object)limitJoint == (Object)null)
					{
						limitJoint = CreateLimitJoint(Mathf.Max(maxValue - value - 120f, minValue - value), maxValue - value);
						limitMin = false;
					}
				}
				else if ((Object)(object)limitJoint != (Object)null && value > minValue + 90f && value < maxValue - 90f)
				{
					Object.Destroy((Object)(object)limitJoint);
				}
			}
			limitUpdateValue = value;
		}

		protected Joint CreateLimitJoint(float min, float max)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			ConfigurableJoint obj = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)obj).set_anchor(((Joint)joint).get_anchor());
			((Joint)obj).set_axis(((Joint)joint).get_axis());
			((Joint)obj).set_autoConfigureConnectedAnchor(false);
			((Joint)obj).set_connectedBody(((Joint)joint).get_connectedBody());
			((Joint)obj).set_connectedAnchor(((Joint)joint).get_connectedAnchor());
			ConfigurableJointMotion val = (ConfigurableJointMotion)2;
			obj.set_zMotion((ConfigurableJointMotion)2);
			ConfigurableJointMotion xMotion;
			obj.set_yMotion(xMotion = val);
			obj.set_xMotion(xMotion);
			xMotion = (ConfigurableJointMotion)2;
			obj.set_angularZMotion((ConfigurableJointMotion)2);
			obj.set_angularYMotion(xMotion);
			obj.set_angularXMotion((ConfigurableJointMotion)1);
			SoftJointLimit val2 = default(SoftJointLimit);
			((SoftJointLimit)(ref val2)).set_limit(0f - max);
			obj.set_lowAngularXLimit(val2);
			val2 = default(SoftJointLimit);
			((SoftJointLimit)(ref val2)).set_limit(0f - min);
			obj.set_highAngularXLimit(val2);
			return (Joint)(object)obj;
		}

		public override float GetValue()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			float num;
			for (num = ((!((Object)(object)anchorTransform != (Object)null)) ? Math3d.SignedVectorAngle(connectedForward, body.TransformDirection(localForward), connectedAxis) : Math3d.SignedVectorAngle(connectedForward, anchorTransform.InverseTransformDirection(body.TransformDirection(localForward)), connectedAxis)); num - lastKnownAngle < -180f; num += 360f)
			{
			}
			while (num - lastKnownAngle > 180f)
			{
				num -= 360f;
			}
			lastKnownAngle = num;
			return num;
		}

		public override void SetValue(float angle)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			lastKnownAngle = angle;
			Quaternion val = Quaternion.AngleAxis(angle, connectedAxis) * relativeRotation;
			if ((Object)(object)anchorTransform != (Object)null)
			{
				val = anchorTransform.get_rotation() * val;
			}
			if ((Object)(object)rigid != (Object)null)
			{
				rigid.MoveRotation(val);
				if (!rigid.get_isKinematic())
				{
					((Component)body).get_transform().set_rotation(val);
				}
			}
			else
			{
				body.set_rotation(val);
			}
		}

		public override void ResetState(int checkpoint, int subObjectives)
		{
			if ((Object)(object)limitJoint != (Object)null)
			{
				Object.Destroy((Object)(object)limitJoint);
				limitJoint = null;
			}
			base.ResetState(checkpoint, subObjectives);
		}

		public override void ApplyState(NetStream state)
		{
			base.ApplyState(state);
			UpdateLimitJoint();
		}

		public override void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			base.ApplyLerpedState(state0, state1, mix);
			UpdateLimitJoint();
		}
	}
}

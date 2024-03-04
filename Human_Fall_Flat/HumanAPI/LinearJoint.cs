using UnityEngine;

namespace HumanAPI
{
	public class LinearJoint : JointImplementation
	{
		[Tooltip("Used to get the transform direction")]
		public Transform axis;

		private Vector3 connectedAxis;

		private Vector3 connectedCenter;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		public override void CreateMainJoint()
		{
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0249: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Creating configurable joint "));
			}
			if ((Object)(object)axis == (Object)null)
			{
				axis = ((Component)this).get_transform();
			}
			connectedAxis = axis.get_forward();
			connectedCenter = body.get_position();
			if ((Object)(object)anchorTransform != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " No Transform for the axis "));
				}
				connectedAxis = anchorTransform.InverseTransformDirection(connectedAxis);
				connectedCenter = anchorTransform.InverseTransformPoint(connectedCenter);
			}
			Vector3 zero = Vector3.get_zero();
			Vector3 val = body.InverseTransformDirection(axis.get_forward());
			Vector3 secondaryAxis = body.InverseTransformDirection(axis.get_right());
			centerValue = 0f;
			if (base.isKinematic)
			{
				return;
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Kinematic Not Set "));
			}
			if (useLimits)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Limits Set "));
				}
				centerValue = (maxValue + minValue) / 2f;
			}
			joint = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)joint).set_autoConfigureConnectedAnchor(false);
			((Joint)joint).set_axis(val);
			joint.set_secondaryAxis(secondaryAxis);
			((Joint)joint).set_anchor(zero);
			((Joint)joint).set_connectedAnchor(connectedCenter + connectedAxis * centerValue);
			((Joint)joint).set_connectedBody(anchorRigid);
			((Joint)joint).set_enableCollision(enableCollision);
			joint.set_xMotion((ConfigurableJointMotion)(useLimits ? 1 : 2));
			joint.set_yMotion((ConfigurableJointMotion)0);
			joint.set_zMotion((ConfigurableJointMotion)0);
			joint.set_angularXMotion((ConfigurableJointMotion)0);
			joint.set_angularYMotion((ConfigurableJointMotion)0);
			joint.set_angularZMotion((ConfigurableJointMotion)0);
			if (useLimits)
			{
				ConfigurableJoint obj = joint;
				SoftJointLimit linearLimit = default(SoftJointLimit);
				((SoftJointLimit)(ref linearLimit)).set_limit((maxValue - minValue) / 2f);
				obj.set_linearLimit(linearLimit);
			}
		}

		private void FixedUpdate()
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			EnsureInitialized();
			if (!((Object)(object)joint == (Object)null) && useSpring)
			{
				float num = (useTension ? (maxForce / tensionDist) : spring);
				float num2 = (useTension ? (maxForce / maxSpeed) : damper);
				Vector3 val = -new Vector3(target - centerValue, 0f, 0f);
				JointDrive xDrive = joint.get_xDrive();
				if (joint.get_targetPosition() != val || ((JointDrive)(ref xDrive)).get_maximumForce() != maxForce || ((JointDrive)(ref xDrive)).get_positionSpring() != num || ((JointDrive)(ref xDrive)).get_positionDamper() != num2)
				{
					ConfigurableJoint obj = joint;
					JointDrive xDrive2 = default(JointDrive);
					((JointDrive)(ref xDrive2)).set_maximumForce(maxForce);
					((JointDrive)(ref xDrive2)).set_positionSpring(num);
					((JointDrive)(ref xDrive2)).set_positionDamper(num2);
					obj.set_xDrive(xDrive2);
					joint.set_targetPosition(val);
					rigid.WakeUp();
				}
			}
		}

		public override float GetValue()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Getting value "));
			}
			EnsureInitialized();
			if ((Object)(object)anchorTransform != (Object)null)
			{
				return Vector3.Dot(anchorTransform.InverseTransformPoint(body.get_position()) - connectedCenter, connectedAxis);
			}
			return Vector3.Dot(body.get_position() - connectedCenter, connectedAxis);
		}

		public override void SetValue(float pos)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Setting value "));
			}
			EnsureInitialized();
			Vector3 val = connectedCenter + connectedAxis * pos;
			if ((Object)(object)anchorTransform != (Object)null)
			{
				val = anchorTransform.TransformPoint(val);
			}
			if ((Object)(object)rigid != (Object)null)
			{
				rigid.MovePosition(val);
				if (!rigid.get_isKinematic())
				{
					body.set_position(val);
				}
			}
			else
			{
				body.set_position(val);
			}
		}
	}
}

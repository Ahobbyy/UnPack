using UnityEngine;

namespace HumanAPI
{
	public class PullJoint : JointImplementation
	{
		[Tooltip("Sting extends beweet anchor and hook")]
		public Transform hook;

		[Tooltip("Sting extends beweet anchor and hook")]
		public Transform anchorPoint;

		private Vector3 connectedAxis;

		private Vector3 connectedCenter;

		private Vector3 localPos;

		public override void CreateMainJoint()
		{
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_014d: Unknown result type (might be due to invalid IL or missing references)
			//IL_015e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01da: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)anchorTransform == (Object)null)
			{
				Debug.LogError((object)"Pull joint needs an achor to function");
			}
			if (useSpring)
			{
				Debug.LogError((object)"Pull joint can't use spring");
			}
			if ((Object)(object)hook == (Object)null)
			{
				hook = ((Component)this).get_transform();
			}
			if ((Object)(object)anchorPoint == (Object)null)
			{
				anchorPoint = anchor;
			}
			Vector3 val = hook.get_position() - anchorPoint.get_position();
			centerValue = ((Vector3)(ref val)).get_magnitude();
			val = hook.get_position() - anchorPoint.get_position();
			connectedAxis = ((Vector3)(ref val)).get_normalized();
			connectedCenter = anchorPoint.get_position();
			if ((Object)(object)anchorTransform != (Object)null)
			{
				connectedAxis = anchorTransform.InverseTransformDirection(connectedAxis);
				connectedCenter = anchorTransform.InverseTransformPoint(connectedCenter);
			}
			localPos = body.InverseTransformPoint(hook.get_position());
			if (!base.isKinematic)
			{
				joint = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
				((Joint)joint).set_autoConfigureConnectedAnchor(false);
				((Joint)joint).set_anchor(localPos);
				((Joint)joint).set_connectedAnchor(connectedCenter);
				((Joint)joint).set_connectedBody(anchorRigid);
				((Joint)joint).set_enableCollision(enableCollision);
				joint.set_xMotion((ConfigurableJointMotion)1);
				joint.set_yMotion((ConfigurableJointMotion)1);
				joint.set_zMotion((ConfigurableJointMotion)1);
				joint.set_angularXMotion((ConfigurableJointMotion)2);
				joint.set_angularYMotion((ConfigurableJointMotion)2);
				joint.set_angularZMotion((ConfigurableJointMotion)2);
				ConfigurableJoint obj = joint;
				SoftJointLimit linearLimit = default(SoftJointLimit);
				((SoftJointLimit)(ref linearLimit)).set_limit(centerValue);
				obj.set_linearLimit(linearLimit);
			}
		}

		private void FixedUpdate()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			EnsureInitialized();
			if (!((Object)(object)joint == (Object)null) && useSpring)
			{
				SoftJointLimit linearLimit = joint.get_linearLimit();
				if (((SoftJointLimit)(ref linearLimit)).get_limit() != centerValue + target)
				{
					ConfigurableJoint obj = joint;
					linearLimit = default(SoftJointLimit);
					((SoftJointLimit)(ref linearLimit)).set_limit(centerValue + target);
					obj.set_linearLimit(linearLimit);
					rigid.WakeUp();
				}
			}
		}

		public override float GetValue()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			EnsureInitialized();
			if ((Object)(object)joint != (Object)null)
			{
				SoftJointLimit linearLimit = joint.get_linearLimit();
				return ((SoftJointLimit)(ref linearLimit)).get_limit() - centerValue;
			}
			Vector3 val = body.TransformPoint(localPos);
			if ((Object)(object)anchorTransform != (Object)null)
			{
				return Vector3.Dot(anchorTransform.InverseTransformPoint(val) - connectedCenter, connectedAxis);
			}
			return Vector3.Dot(val - connectedCenter, connectedAxis);
		}

		public override void SetValue(float pos)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			EnsureInitialized();
			if ((Object)(object)joint != (Object)null)
			{
				SoftJointLimit linearLimit = joint.get_linearLimit();
				if (((SoftJointLimit)(ref linearLimit)).get_limit() != centerValue + pos)
				{
					ConfigurableJoint obj = joint;
					linearLimit = default(SoftJointLimit);
					((SoftJointLimit)(ref linearLimit)).set_limit(centerValue + pos);
					obj.set_linearLimit(linearLimit);
					rigid.WakeUp();
				}
				return;
			}
			Vector3 val = connectedCenter + connectedAxis * (centerValue + pos);
			if ((Object)(object)anchorTransform != (Object)null)
			{
				val = anchorTransform.TransformPoint(val);
			}
			val -= body.TransformDirection(localPos);
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

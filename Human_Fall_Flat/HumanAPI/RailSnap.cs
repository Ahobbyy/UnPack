using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class RailSnap : Node
	{
		private ConfigurableJoint joint;

		[Tooltip("The input this node takes from the graph")]
		public NodeInput input;

		[Tooltip("The max Speed of this rail car")]
		public float maxSpeed = 3f;

		[Tooltip("A referece to a rail snap parameter script for storing values")]
		public RailSnapParameters parameters;

		[Tooltip("The current rail the train car should be on ")]
		public Rail currentRail;

		[Tooltip("The current segment of the rail the rail car should be on ")]
		public int currentSegment;

		[Tooltip("The current speed the rail car is doing")]
		[ReadOnly]
		public float currentSpeed;

		private bool seendebugstring1;

		private Rigidbody body;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private Vector3 current;

		private void Awake()
		{
			body = ((Component)this).GetComponentInParent<Rigidbody>();
			if ((Object)(object)parameters == (Object)null)
			{
				parameters = ((Component)this).get_gameObject().AddComponent<RailSnapParameters>();
				parameters.maxSpeed = maxSpeed;
				parameters.accelerationTime = 0f;
				parameters.decelerationTime = 0f;
				parameters.posSpringX = 100000f;
				parameters.posDampX = 10000f;
				parameters.posSpringY = 100000f;
				parameters.posDampY = 10000f;
			}
			CreateJoint();
		}

		private void CreateJoint()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_0143: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			joint = ((Component)body).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)joint).set_axis(Vector3.get_up());
			joint.set_secondaryAxis(Vector3.get_right());
			joint.set_xMotion((ConfigurableJointMotion)2);
			joint.set_yMotion((ConfigurableJointMotion)2);
			joint.set_zMotion((ConfigurableJointMotion)2);
			ConfigurableJoint obj = joint;
			ConfigurableJoint obj2 = joint;
			ConfigurableJoint obj3 = joint;
			ConfigurableJointMotion val = (ConfigurableJointMotion)2;
			obj3.set_angularZMotion((ConfigurableJointMotion)2);
			ConfigurableJointMotion angularXMotion;
			obj2.set_angularYMotion(angularXMotion = val);
			obj.set_angularXMotion(angularXMotion);
			((Joint)joint).set_autoConfigureConnectedAnchor(false);
			((Joint)joint).set_anchor(((Component)body).get_transform().InverseTransformPoint(((Component)this).get_transform().get_position()));
			ConfigurableJoint obj4 = joint;
			JointDrive val2 = default(JointDrive);
			((JointDrive)(ref val2)).set_positionSpring(parameters.posSpringX);
			((JointDrive)(ref val2)).set_positionDamper(parameters.posDampX);
			((JointDrive)(ref val2)).set_maximumForce(float.PositiveInfinity);
			obj4.set_xDrive(val2);
			ConfigurableJoint obj5 = joint;
			val2 = default(JointDrive);
			((JointDrive)(ref val2)).set_positionSpring(parameters.posSpringY);
			((JointDrive)(ref val2)).set_positionDamper(parameters.posDampY);
			((JointDrive)(ref val2)).set_maximumForce(float.PositiveInfinity);
			obj5.set_yDrive(val2);
			current = ((Component)this).get_transform().get_position();
		}

		private void FixedUpdate()
		{
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_018b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			if (NetGame.isClient || ReplayRecorder.isPlaying)
			{
				if ((Object)(object)joint != (Object)null)
				{
					Object.Destroy((Object)(object)joint);
				}
				return;
			}
			if ((Object)(object)joint == (Object)null)
			{
				CreateJoint();
			}
			float num = parameters.maxSpeed * input.value;
			if (currentSpeed < num)
			{
				if (parameters.accelerationTime > 0f)
				{
					currentSpeed += parameters.maxSpeed * (Time.get_fixedDeltaTime() / parameters.accelerationTime);
					currentSpeed = Mathf.Min(currentSpeed, parameters.maxSpeed);
				}
				else
				{
					currentSpeed = num;
				}
			}
			if (currentSpeed > num)
			{
				if (parameters.decelerationTime > 0f)
				{
					currentSpeed -= parameters.maxSpeed * (Time.get_fixedDeltaTime() / parameters.decelerationTime);
					currentSpeed = Mathf.Max(currentSpeed, 0f - parameters.maxSpeed);
				}
				else
				{
					currentSpeed = num;
				}
			}
			if (num == 0f && Mathf.Abs(currentSpeed) < 0.05f)
			{
				currentSpeed = 0f;
			}
			Vector3 val = ((Component)this).get_transform().get_forward() * currentSpeed;
			Vector3 projected = Vector3.get_zero();
			Vector3 position = ((Component)this).get_transform().get_position();
			Vector3 val2 = current;
			Vector3 val3 = position - current;
			current = Vector3.Lerp(val2, position, ((Vector3)(ref val3)).get_magnitude());
			if (Rail.Project(current + val * Time.get_fixedDeltaTime(), ref projected, ref currentRail, ref currentSegment))
			{
				current = projected;
				((Joint)joint).set_connectedAnchor(current);
			}
			else if (!seendebugstring1 && showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + "No track?"));
				seendebugstring1 = true;
			}
		}
	}
}

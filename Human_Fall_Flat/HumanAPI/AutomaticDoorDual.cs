using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Automatic Door Dual", 10)]
	public class AutomaticDoorDual : Node
	{
		public NodeInput input;

		public ConfigurableJoint leftJoint;

		public ConfigurableJoint rightJoint;

		public float doorForce = 1000f;

		public float doorSpeed = 1f;

		public float gap = 2f;

		private Rigidbody leftDoor;

		private Rigidbody rightDoor;

		private float targetGap;

		private float tensionDist = 0.05f;

		protected void Awake()
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			leftDoor = ((Component)leftJoint).GetComponent<Rigidbody>();
			rightDoor = ((Component)rightJoint).GetComponent<Rigidbody>();
			Vector3 val = (((Component)leftJoint).get_transform().get_position() + ((Component)rightJoint).get_transform().get_position()) / 2f;
			ConfigurableJoint obj = leftJoint;
			bool autoConfigureConnectedAnchor;
			((Joint)rightJoint).set_autoConfigureConnectedAnchor(autoConfigureConnectedAnchor = false);
			((Joint)obj).set_autoConfigureConnectedAnchor(autoConfigureConnectedAnchor);
			((Joint)leftJoint).set_anchor(((Component)leftJoint).get_transform().InverseTransformPoint(val));
			((Joint)rightJoint).set_anchor(((Component)rightJoint).get_transform().InverseTransformPoint(val));
			leftJoint.SetXMotionAnchorsAndLimits(gap / 4f, gap / 2f);
			rightJoint.SetXMotionAnchorsAndLimits(gap / 4f, gap / 2f);
		}

		public override void Process()
		{
			base.Process();
			if (SignalManager.skipTransitions)
			{
				targetGap = Mathf.Lerp(0f, gap, input.value);
				ApplyTargetGap();
				leftJoint.ApplyXMotionTarget();
				rightJoint.ApplyXMotionTarget();
			}
		}

		private void FixedUpdate()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			float num = targetGap;
			Vector3 val = ((Component)leftDoor).get_transform().TransformPoint(((Joint)leftJoint).get_anchor()) - ((Component)rightDoor).get_transform().TransformPoint(((Joint)leftJoint).get_anchor());
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			float num2 = Mathf.Lerp(0f, gap, input.value);
			targetGap = Mathf.MoveTowards(magnitude, num2, doorSpeed * Time.get_fixedDeltaTime());
			float num3 = ((Mathf.Abs(input.value) >= 0.5f) ? tensionDist : (0f - tensionDist));
			targetGap += num3 * 2f;
			targetGap = Mathf.Clamp(targetGap, 0f, gap);
			if (num != targetGap)
			{
				ApplyTargetGap();
			}
		}

		private void ApplyTargetGap()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			JointDrive xDrive = leftJoint.get_xDrive();
			((JointDrive)(ref xDrive)).set_positionSpring(doorForce / tensionDist);
			((JointDrive)(ref xDrive)).set_positionDamper(doorForce / doorSpeed);
			leftJoint.set_xDrive(xDrive);
			rightJoint.set_xDrive(xDrive);
			leftJoint.set_targetPosition(new Vector3((0f - targetGap) / 2f + gap / 4f, 0f, 0f));
			rightJoint.set_targetPosition(new Vector3((0f - targetGap) / 2f + gap / 4f, 0f, 0f));
			leftDoor.WakeUp();
			rightDoor.WakeUp();
		}
	}
}

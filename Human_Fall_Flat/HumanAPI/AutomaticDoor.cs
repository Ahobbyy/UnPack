using UnityEngine;

namespace HumanAPI
{
	public class AutomaticDoor : LerpBase
	{
		public ConfigurableJoint slideJoint;

		public float doorForce = 1000f;

		public float doorSpeed = 1f;

		[Range(0f, 0.99f)]
		public float tension = 0.9f;

		protected override void ApplyValue(float value)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			SoftJointLimit linearLimit = slideJoint.get_linearLimit();
			float num = ((SoftJointLimit)(ref linearLimit)).get_limit() * 2f;
			float num2 = num * tension / (1f - tension) + num / 2f;
			slideJoint.set_targetPosition(new Vector3(Mathf.Lerp(num2, 0f - num2, value), 0f, 0f));
			JointDrive xDrive = slideJoint.get_xDrive();
			((JointDrive)(ref xDrive)).set_positionSpring(doorForce / (num2 + num / 2f));
			((JointDrive)(ref xDrive)).set_positionDamper(doorForce / doorSpeed);
			slideJoint.set_xDrive(xDrive);
			((Component)slideJoint).GetComponent<Rigidbody>().WakeUp();
		}
	}
}

using UnityEngine;

namespace HumanAPI
{
	public class AngularServo : ServoBase
	{
		public Vector3 axis;

		public Rigidbody body;

		private Transform bodyTransform;

		private ConfigurableJoint joint;

		private Quaternion invInitialLocalRotation;

		protected override void Awake()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			joint = ((Component)body).GetComponent<ConfigurableJoint>();
			bodyTransform = ((Component)body).get_transform();
			isKinematic = body.get_isKinematic();
			invInitialLocalRotation = ((Joint)(object)joint).ReadInitialRotation();
			if ((Object)(object)joint != (Object)null)
			{
				float num = maxValue - minValue;
				joint.SetXMotionAnchorsAndLimits(num / 2f - (initialValue - minValue), num);
				ConfigurableJoint obj = joint;
				SoftJointLimit val = default(SoftJointLimit);
				((SoftJointLimit)(ref val)).set_limit(minValue - initialValue);
				obj.set_lowAngularXLimit(val);
				ConfigurableJoint obj2 = joint;
				val = default(SoftJointLimit);
				((SoftJointLimit)(ref val)).set_limit(maxValue - initialValue);
				obj2.set_highAngularXLimit(val);
				ConfigurableJoint obj3 = joint;
				JointDrive angularXDrive = default(JointDrive);
				((JointDrive)(ref angularXDrive)).set_maximumForce(maxForce);
				obj3.set_angularXDrive(angularXDrive);
			}
			base.Awake();
		}

		protected override float GetActualPosition()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return ((Joint)(object)joint).GetXAngle(invInitialLocalRotation) - initialValue;
		}

		protected override void SetStatic(float pos)
		{
		}

		protected override void SetJoint(float pos, float spring, float damper)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			joint.SetXAngleTarget(pos);
			JointDrive angularXDrive = joint.get_angularXDrive();
			((JointDrive)(ref angularXDrive)).set_positionSpring(spring);
			((JointDrive)(ref angularXDrive)).set_positionDamper(damper);
			joint.set_angularXDrive(angularXDrive);
			if (SignalManager.skipTransitions)
			{
				((Joint)(object)joint).ApplyXAngle(invInitialLocalRotation, pos);
			}
			else
			{
				body.WakeUp();
			}
		}
	}
}

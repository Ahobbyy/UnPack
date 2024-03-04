using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class Gearbox : Node, IPostReset, IRespawnable
	{
		public float gearChangeTime = 3f;

		public float springSnap = 5000f;

		public float springRelax = 100f;

		public float reverseAngle = -30f;

		public float neutralAngle;

		public float firstAngle = 30f;

		public float secondAngle = 60f;

		public float reverseValue = -0.2f;

		public float firstValue = 0.2f;

		public float secondValue = 1f;

		public HingeJoint steeringWheel;

		public float centeringSpring1st = 100f;

		public float centeringSpring2st = 300f;

		public HingeJoint joint;

		public int gear;

		private float nextChangeIn;

		public NodeOutput output;

		private Quaternion invInitialLocalRotation;

		private int grabbedInGear;

		private bool grabbed;

		protected override void OnEnable()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			base.OnEnable();
			invInitialLocalRotation = Quaternion.Inverse(((Component)joint).get_transform().get_localRotation());
		}

		private float GetJointAngle()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			Quaternion val = invInitialLocalRotation * ((Component)joint).get_transform().get_localRotation();
			float num = default(float);
			Vector3 val2 = default(Vector3);
			((Quaternion)(ref val)).ToAngleAxis(ref num, ref val2);
			if (Vector3.Dot(val2, ((Joint)joint).get_axis()) < 0f)
			{
				return 0f - num;
			}
			return num;
		}

		private void FixedUpdate()
		{
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0169: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)joint == (Object)null)
			{
				return;
			}
			float jointAngle = GetJointAngle();
			bool flag = GrabManager.IsGrabbedAny(((Component)joint).get_gameObject());
			if (!grabbed && flag)
			{
				grabbedInGear = gear;
			}
			grabbed = flag;
			if (grabbed)
			{
				if (jointAngle <= (reverseAngle + neutralAngle) / 2f)
				{
					gear = -1;
				}
				else if (jointAngle <= (neutralAngle + firstAngle) / 2f)
				{
					gear = 0;
				}
				else if (jointAngle <= (firstAngle + secondAngle) / 2f)
				{
					gear = 1;
				}
				else
				{
					gear = 2;
				}
				JointLimits limits = joint.get_limits();
				((JointLimits)(ref limits)).set_min(reverseAngle);
				if (grabbedInGear < 1)
				{
					if (gear == 2)
					{
						gear = 1;
					}
					((JointLimits)(ref limits)).set_max(firstAngle);
				}
				else
				{
					((JointLimits)(ref limits)).set_max(secondAngle);
				}
				joint.SetLimits(limits);
				if ((Object)(object)steeringWheel != (Object)null)
				{
					JointSpring spring = steeringWheel.get_spring();
					spring.spring = ((gear == 2) ? centeringSpring2st : centeringSpring1st);
					steeringWheel.set_spring(spring);
				}
			}
			float value = 0f;
			JointSpring spring2 = joint.get_spring();
			switch (gear)
			{
			case -1:
				spring2.targetPosition = reverseAngle;
				value = reverseValue;
				break;
			case 0:
				spring2.targetPosition = neutralAngle;
				value = 0f;
				break;
			case 1:
				spring2.targetPosition = firstAngle;
				value = firstValue;
				break;
			case 2:
				spring2.targetPosition = secondAngle;
				value = secondValue;
				break;
			}
			joint.SetSpring(spring2);
			output.SetValue(value);
		}

		public void PostResetState(int checkpoint)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			JointSpring spring = joint.get_spring();
			spring.targetPosition = neutralAngle;
			joint.set_spring(spring);
			gear = 0;
		}

		public void Respawn(Vector3 offset)
		{
			PostResetState(0);
		}
	}
}

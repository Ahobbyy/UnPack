using UnityEngine;

namespace HumanAPI
{
	public class GapJoint : JointBase
	{
		public JointBase jointA;

		public JointBase jointB;

		private bool initialized;

		public override void EnsureInitialized()
		{
			//IL_0168: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0209: Unknown result type (might be due to invalid IL or missing references)
			if (!initialized)
			{
				initialized = true;
				jointA.useLimits = (jointB.useLimits = useLimits);
				jointA.minValue = (jointB.minValue = minValue / 2f);
				jointA.maxValue = (jointB.maxValue = maxValue / 2f);
				jointA.maxSpeed = (jointB.maxSpeed = maxSpeed / 2f);
				jointA.maxAcceleration = (jointB.maxAcceleration = maxAcceleration / 2f);
				jointA.useSpring = (jointB.useSpring = useSpring);
				jointA.tensionDist = (jointB.tensionDist = tensionDist / 2f);
				jointA.maxForce = (jointB.maxForce = maxForce / 2f);
				jointA.EnsureInitialized();
				jointB.EnsureInitialized();
				_isKinematic = jointA.isKinematic;
				if (maxValue == 1.7f)
				{
					GameObject val = new GameObject("JamDoorAchievement");
					val.get_transform().SetParent(((Component)this).get_transform(), false);
					val.get_transform().set_localPosition(new Vector3(0f, 1f, -1f));
					val.get_transform().set_localRotation(Quaternion.Euler(90f, 0f, 0f));
					val.get_gameObject().AddComponent<JamDoorAchievement>().door = this;
					BoxCollider obj = val.get_gameObject().AddComponent<BoxCollider>();
					((Collider)obj).set_isTrigger(true);
					obj.set_center(new Vector3(0.25f, 1f, 0f));
					obj.set_size(new Vector3(1f, 2f, 2f));
					val.set_layer(15);
				}
			}
		}

		public override float GetTarget()
		{
			return jointA.GetTarget() + jointB.GetTarget();
		}

		public override float GetValue()
		{
			return jointA.GetValue() + jointB.GetValue();
		}

		public override void SetTarget(float value)
		{
			jointA.SetTarget(value / 2f);
			jointB.SetTarget(value / 2f);
		}

		public override void SetValue(float value)
		{
			jointA.SetValue(value / 2f);
			jointB.SetValue(value / 2f);
		}
	}
}

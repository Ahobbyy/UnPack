using UnityEngine;

namespace HumanAPI
{
	public class SignalAngle : Node
	{
		public NodeOutput output;

		public HingeJoint joint;

		public float fromAngle;

		public float toAngle;

		public bool signedOutput = true;

		private Rigidbody body;

		private Quaternion invInitialLocalRotation;

		public float currentAngle;

		public HingeJoint limitJoint;

		public float currentValue;

		private void Awake()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)joint == (Object)null)
			{
				joint = ((Component)this).GetComponent<HingeJoint>();
			}
			body = ((Component)joint).GetComponent<Rigidbody>();
			invInitialLocalRotation = ((Joint)(object)joint).ReadInitialRotation();
		}

		private void FixedUpdate()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			float num;
			for (num = ((Joint)(object)joint).GetXAngle(invInitialLocalRotation) - currentAngle; num < -180f; num += 360f)
			{
			}
			while (num > 180f)
			{
				num -= 360f;
			}
			currentAngle += num;
			if (num == 0f)
			{
				return;
			}
			if (currentAngle < fromAngle + 60f)
			{
				if ((Object)(object)limitJoint == (Object)null)
				{
					limitJoint = CreateLimitJoint(fromAngle - currentAngle, Mathf.Min(fromAngle - currentAngle + 120f, toAngle - currentAngle));
				}
			}
			else if (currentAngle > toAngle - 60f)
			{
				if ((Object)(object)limitJoint == (Object)null)
				{
					limitJoint = CreateLimitJoint(Mathf.Max(toAngle - currentAngle - 120f, fromAngle - currentAngle), toAngle - currentAngle);
				}
			}
			else if ((Object)(object)limitJoint != (Object)null && currentAngle > fromAngle + 90f && currentAngle < toAngle - 90f)
			{
				Object.Destroy((Object)(object)limitJoint);
			}
			currentAngle = Mathf.Clamp(currentAngle, fromAngle, toAngle);
			currentValue = Mathf.InverseLerp(fromAngle, toAngle, currentAngle);
			output.SetValue(signedOutput ? Mathf.Lerp(-1f, 1f, currentValue) : currentValue);
		}

		private HingeJoint CreateLimitJoint(float min, float max)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			HingeJoint obj = ((Component)this).get_gameObject().AddComponent<HingeJoint>();
			((Joint)obj).set_anchor(((Joint)joint).get_anchor());
			((Joint)obj).set_axis(((Joint)joint).get_axis());
			((Joint)obj).set_autoConfigureConnectedAnchor(false);
			((Joint)obj).set_connectedBody(((Joint)joint).get_connectedBody());
			((Joint)obj).set_connectedAnchor(((Joint)joint).get_connectedAnchor());
			obj.set_useLimits(true);
			JointLimits limits = default(JointLimits);
			((JointLimits)(ref limits)).set_min(min);
			((JointLimits)(ref limits)).set_max(max);
			obj.set_limits(limits);
			return obj;
		}

		public override void Process()
		{
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			base.Process();
			currentValue = (signedOutput ? Mathf.InverseLerp(-1f, 1f, output.value) : output.value);
			currentAngle = Mathf.Lerp(fromAngle, toAngle, currentValue);
			((Joint)(object)joint).ApplyXAngle(invInitialLocalRotation, currentAngle);
		}
	}
}

using UnityEngine;

public class SignalBroadcastAngle : SignalBase
{
	public float fromAngle;

	public float fromDeadAngle;

	public float toDeadAngle;

	public float toAngle;

	public HingeJoint joint;

	public bool snapZero;

	public bool snapPositive;

	public bool snapNegative;

	public float holdZeroSpringWhenNoSnap;

	private float respringBlock;

	public GameObject jointHolder;

	private Quaternion invInitialLocalRotation;

	private void Awake()
	{
		if ((Object)(object)jointHolder == (Object)null)
		{
			jointHolder = ((Component)joint).get_gameObject();
		}
		if (jointHolder.GetComponents<HingeJoint>().Length > 1)
		{
			Debug.LogError((object)"SignalBroadcastAngle has multiple jooints", (Object)(object)this);
		}
	}

	public override void PostResetState(int checkpoint)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		base.PostResetState(checkpoint);
		if ((Object)(object)joint == (Object)null)
		{
			joint = jointHolder.GetComponent<HingeJoint>();
		}
		JointSpring spring = joint.get_spring();
		spring.targetPosition = (fromDeadAngle + toDeadAngle) / 2f;
		joint.set_spring(spring);
		respringBlock = 0.5f;
		CheckHoldZero();
	}

	protected override void OnEnable()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		CheckHoldZero();
		invInitialLocalRotation = Quaternion.Inverse(((Component)joint).get_transform().get_localRotation());
	}

	private void CheckHoldZero()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		if (holdZeroSpringWhenNoSnap != 0f)
		{
			JointSpring spring = joint.get_spring();
			float targetPosition = spring.targetPosition;
			float spring2 = spring.spring;
			if (value == 0f)
			{
				spring.targetPosition = (fromDeadAngle + toDeadAngle) / 2f;
				spring.spring = holdZeroSpringWhenNoSnap;
			}
			else
			{
				spring.spring = 0f;
			}
			if (targetPosition != spring.targetPosition || spring2 != spring.spring)
			{
				joint.set_spring(spring);
			}
		}
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
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)joint == (Object)null)
		{
			return;
		}
		float jointAngle = GetJointAngle();
		float num = 0f;
		if (jointAngle < fromDeadAngle)
		{
			num = 0f - Mathf.InverseLerp(fromDeadAngle, fromAngle, jointAngle);
		}
		if (jointAngle > toDeadAngle)
		{
			num = Mathf.InverseLerp(toDeadAngle, toAngle, jointAngle);
		}
		SetValue(num);
		if (respringBlock > 0f)
		{
			respringBlock -= Time.get_fixedDeltaTime();
			if (respringBlock > 0f)
			{
				return;
			}
		}
		CheckHoldZero();
		JointSpring spring = joint.get_spring();
		float targetPosition = spring.targetPosition;
		if (snapNegative && num < -0.75f)
		{
			spring.targetPosition = fromAngle;
		}
		if (snapPositive && num > 0.75f)
		{
			spring.targetPosition = toAngle;
		}
		if (snapZero && num > -0.5f && num < 0.5f)
		{
			spring.targetPosition = (fromDeadAngle + toDeadAngle) / 2f;
		}
		if (spring.targetPosition != targetPosition)
		{
			joint.set_spring(spring);
		}
	}
}

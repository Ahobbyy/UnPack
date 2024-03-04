using System;
using UnityEngine;

[Serializable]
public class TorsoMuscles
{
	private readonly Human human;

	private readonly Ragdoll ragdoll;

	private readonly HumanMotion2 motion;

	public Vector3 feedbackForce;

	private float timeSinceUnconsious;

	private float timeSinceOffGround;

	private float idleAnimationPhase;

	private float idleAnimationDuration = 3f;

	public float predictTime = 0.001f;

	public float springSpeed = 0.01f;

	public float m = 1f;

	public float headOffset = 0.5f;

	public float headApparentMass = 1f;

	public float headMaxForceY = 40f;

	public float headMaxForceZ = 30f;

	public float chestOffset = 0.5f;

	public float chestApparentMass = 5f;

	public float chestMaxForceY = 20f;

	public float chestMaxForceZ = 20f;

	public float waistOffset = 0.5f;

	public float waistApparentMass = 10f;

	public float waistMaxForceY = 30f;

	public float waistMaxForceZ = 30f;

	public float hipsOffset = 0.5f;

	public float hipsApparentMass = 2f;

	public float hipsMaxForceY = 50f;

	public float hipsMaxForceZ = 50f;

	public float chestAngle = 1f;

	public float waistAngle = 0.7f;

	public float hipsAngle = 0.4f;

	public TorsoMuscles(Human human, Ragdoll ragdoll, HumanMotion2 motion)
	{
		this.human = human;
		this.ragdoll = ragdoll;
		this.motion = motion;
	}

	public void OnFixedUpdate()
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		timeSinceUnconsious += Time.get_fixedDeltaTime();
		timeSinceOffGround += Time.get_fixedDeltaTime();
		if (!human.onGround)
		{
			timeSinceOffGround = 0f;
		}
		feedbackForce = Vector3.get_zero();
		HumanState humanState = human.state;
		if (humanState == HumanState.Fall && (Object)(object)human.grabbedByHuman != (Object)null)
		{
			humanState = HumanState.Idle;
		}
		switch (humanState)
		{
		case HumanState.Idle:
		case HumanState.Slide:
			feedbackForce = IdleAnimation();
			break;
		case HumanState.Walk:
			feedbackForce = StandAnimation();
			break;
		case HumanState.Climb:
			feedbackForce = ClimbAnimation();
			break;
		case HumanState.Jump:
			feedbackForce = JumpAnimation();
			break;
		case HumanState.Fall:
			feedbackForce = FallAnimation();
			break;
		case HumanState.FreeFall:
			feedbackForce = FreeFallAnimation();
			break;
		case HumanState.Unconscious:
			timeSinceUnconsious = 0f;
			break;
		case HumanState.Dead:
		case HumanState.Spawning:
			break;
		}
	}

	private Vector3 IdleAnimation()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		idleAnimationPhase = MathUtils.Wrap(idleAnimationPhase + Time.get_deltaTime() / idleAnimationDuration, 1f);
		float torsoBend = Mathf.Lerp(1f, -0.5f, Mathf.Sin(idleAnimationPhase * (float)Math.PI * 2f) / 2f + 0.5f);
		return ApplyTorsoPose(1f, 1f, torsoBend, 1f);
	}

	private Vector3 StandAnimation()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		return ApplyTorsoPose(1f, 1f, 0f, 1f);
	}

	private Vector3 ClimbAnimation()
	{
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Clamp01((human.controls.targetPitchAngle - 10f) / 60f);
		int num2 = ((((Object)(object)ragdoll.partLeftHand.sensor.grabJoint != (Object)null || human.controls.leftGrab) && ((Object)(object)ragdoll.partRightHand.sensor.grabJoint != (Object)null || human.controls.rightGrab)) ? 1 : 0);
		return ApplyTorsoPose(num2, 1f, 0f, Mathf.Lerp(0.2f, 1f, num));
	}

	private Vector3 JumpAnimation()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		float lift = 1f;
		return ApplyTorsoPose(1f, 1f, 0f, lift);
	}

	private Vector3 FallAnimation()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		float lift = 0.3f;
		return ApplyTorsoPose(0.5f, 1f, 0f, lift);
	}

	private Vector3 FreeFallAnimation()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		human.AddRandomTorque(0.01f);
		float num = Mathf.Sin(Time.get_time() * 3f) * 0.1f;
		float weight = human.weight;
		ragdoll.partHips.rigidbody.SafeAddForce(-Vector3.get_up() * weight * num, (ForceMode)0);
		ragdoll.partChest.rigidbody.SafeAddForce(-Vector3.get_up() * weight * (0f - num), (ForceMode)0);
		return Vector3.get_zero();
	}

	public Vector3 ApplyTorsoPose(float torsoTonus, float headTonus, float torsoBend, float lift)
	{
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0447: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044a: Unknown result type (might be due to invalid IL or missing references)
		//IL_044f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0458: Unknown result type (might be due to invalid IL or missing references)
		//IL_045d: Unknown result type (might be due to invalid IL or missing references)
		lift *= Mathf.Clamp01(timeSinceUnconsious / 3f) * Mathf.Clamp01(timeSinceOffGround * 0.2f + 0.8f);
		torsoTonus *= Mathf.Clamp01(timeSinceUnconsious);
		torsoTonus *= 2f;
		headTonus *= 2f;
		float num = human.weight * 0.8f * lift;
		float num2 = human.controls.targetPitchAngle;
		if (human.hasGrabbed)
		{
			num2 = (num2 + 80f) * 0.5f - 80f;
		}
		HumanMotion2.AlignLook(ragdoll.partHead, Quaternion.Euler(num2, human.controls.targetYawAngle, 0f), 2f * headTonus, 10f * headTonus);
		if (human.onGround || human.state == HumanState.Climb)
		{
			torsoBend *= 40f;
			HumanMotion2.AlignLook(ragdoll.partChest, Quaternion.Euler(human.controls.targetPitchAngle + torsoBend, human.controls.targetYawAngle, 0f), 2f * torsoTonus, 10f * torsoTonus);
			HumanMotion2.AlignLook(ragdoll.partWaist, Quaternion.Euler(human.controls.targetPitchAngle + torsoBend / 2f, human.controls.targetYawAngle, 0f), 1f * torsoTonus, 15f * torsoTonus);
			HumanMotion2.AlignLook(ragdoll.partHips, Quaternion.Euler(human.controls.targetPitchAngle, human.controls.targetYawAngle, 0f), 0.5f * torsoTonus, 20f * torsoTonus);
		}
		float num3 = 0f;
		if (human.targetDirection.y > 0f)
		{
			num3 = human.targetDirection.y * 0.25f;
			if (human.onGround && (Object)(object)human.ragdoll.partLeftHand.sensor.grabBody != (Object)null)
			{
				num3 *= 1.5f;
			}
			if (human.onGround && (Object)(object)human.ragdoll.partRightHand.sensor.grabBody != (Object)null)
			{
				num3 *= 1.5f;
			}
		}
		else
		{
			num3 = 0f - human.targetDirection.y;
		}
		Vector3 val = Mathf.Lerp(0.2f, 0f, num3) * num * headTonus * Vector3.get_up();
		Vector3 val2 = Mathf.Lerp(0.6f, 0f, num3) * num * torsoTonus * Vector3.get_up();
		Vector3 val3 = Mathf.Lerp(0.2f, 0.5f, num3) * num * torsoTonus * Vector3.get_up();
		Vector3 val4 = Mathf.Lerp(0f, 0.5f, num3) * num * torsoTonus * Vector3.get_up();
		if (human.controls.leftGrab)
		{
			UnblockArmBehindTheBack(ragdoll.partLeftHand, -1f);
		}
		if (human.controls.rightGrab)
		{
			UnblockArmBehindTheBack(ragdoll.partRightHand, 1f);
		}
		ragdoll.partHead.rigidbody.SafeAddForce(val, (ForceMode)0);
		ragdoll.partChest.rigidbody.SafeAddForce(val2, (ForceMode)0);
		ragdoll.partWaist.rigidbody.SafeAddForce(val3, (ForceMode)0);
		ragdoll.partHips.rigidbody.SafeAddForce(val4, (ForceMode)0);
		StabilizeHorizontal(ragdoll.partHips.rigidbody, ragdoll.partBall.rigidbody, 1f * lift * Mathf.Lerp(1f, 0.25f, Mathf.Abs(num3)));
		StabilizeHorizontal(ragdoll.partHead.rigidbody, ragdoll.partBall.rigidbody, 0.2f * lift * Mathf.Lerp(1f, 0f, Mathf.Abs(num3)));
		return -(val + val2 + val3 + val4);
	}

	private void UnblockArmBehindTheBack(HumanSegment hand, float direction)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = ragdoll.partHead.collider.get_bounds();
		Vector3 center = ((Bounds)(ref bounds)).get_center();
		bounds = hand.collider.get_bounds();
		Vector3 center2 = ((Bounds)(ref bounds)).get_center();
		Vector3 val = ragdoll.partChest.transform.InverseTransformVector(center2 - center);
		float num = Mathf.InverseLerp(0f, -0.1f, val.z) * Mathf.InverseLerp(0.1f, -0.1f, val.x * direction) * Mathf.InverseLerp(-0.3f, -0.1f, val.y);
		if (num > 0f)
		{
			ragdoll.partHead.rigidbody.SafeAddForce((0f - direction) * num * ragdoll.partChest.transform.get_right() * 200f, (ForceMode)0);
			hand.rigidbody.SafeAddForce(direction * num * ragdoll.partChest.transform.get_right() * 200f, (ForceMode)0);
		}
	}

	private void StabilizeHorizontal(Rigidbody top, Rigidbody bottom, float multiplier)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		float num = 3f;
		Vector3 val = bottom.get_position() + bottom.get_velocity() * Time.get_fixedDeltaTime() - top.get_position() - top.get_velocity() * Time.get_fixedDeltaTime() * num;
		val.y = 0f;
		Vector3 val2 = val * top.get_mass() / Time.get_fixedDeltaTime();
		val2 *= multiplier;
		top.SafeAddForce(val2, (ForceMode)0);
		bottom.SafeAddForce(-val2, (ForceMode)0);
	}
}

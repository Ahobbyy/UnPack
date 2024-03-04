using System;
using HumanAPI;
using UnityEngine;

[Serializable]
public class HandMuscles
{
	public enum TargetingMode
	{
		Shoulder,
		Chest,
		Hips,
		Ball
	}

	private class ScanMem
	{
		public Vector3 pos;

		public Vector3 shoulder;

		public Vector3 hand;

		public float grabTime;

		public float grabAngle;
	}

	public float spring = 10f;

	public float extraUpSpring;

	public float damper;

	public float squareDamper = 0.2f;

	public float maxHorizontalForce = 250f;

	public float maxVertialForce = 500f;

	public float grabSpring = 10f;

	public float grabExtraUpSpring;

	public float grabDamper;

	public float grabSquareDamper = 0.2f;

	public float grabMaxHorizontalForce = 250f;

	public float grabMaxVertialForce = 500f;

	public float maxLiftForce = 500f;

	public float maxPushForce = 200f;

	public float liftDampSqr = 0.1f;

	public float liftDamp = 0.1f;

	private readonly Human human;

	private readonly Ragdoll ragdoll;

	private readonly HumanMotion2 motion;

	public TargetingMode targetingMode;

	public TargetingMode grabTargetingMode = TargetingMode.Ball;

	private ScanMem leftMem = new ScanMem();

	private ScanMem rightMem = new ScanMem();

	public float forwardMultiplier = 10f;

	public float armMass = 20f;

	public float bodyMass = 50f;

	public float maxForce = 300f;

	public float grabMaxForce = 450f;

	public float climbMaxForce = 800f;

	public float gravityForce = 100f;

	public float anisotrophy = 1f;

	public float maxStopForce = 150f;

	public float grabMaxStopForce = 500f;

	public float maxSpeed = 100f;

	public float onAxisAnisotrophy;

	public float offAxisAnisotrophy;

	public const float grabSnap = 0.3f;

	public const float targetHelperSnap = 0.3f;

	public const float targetHelperPull = 0.5f;

	public const float targetSnap = 0.2f;

	public const float targetPull = 0.5f;

	public const float regularSnap = 0.1f;

	public const float regularPull = 0.2f;

	private Collider[] colliders = (Collider[])(object)new Collider[20];

	public HandMuscles(Human human, Ragdoll ragdoll, HumanMotion2 motion)
	{
		this.human = human;
		this.ragdoll = ragdoll;
		this.motion = motion;
	}

	public void OnFixedUpdate()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0268: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0349: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_035f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_0369: Unknown result type (might be due to invalid IL or missing references)
		//IL_036e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_038c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0396: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_0428: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_0483: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0536: Unknown result type (might be due to invalid IL or missing references)
		//IL_053b: Unknown result type (might be due to invalid IL or missing references)
		//IL_054c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0556: Unknown result type (might be due to invalid IL or missing references)
		//IL_055b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0579: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0583: Unknown result type (might be due to invalid IL or missing references)
		//IL_05ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_05f6: Unknown result type (might be due to invalid IL or missing references)
		float targetPitchAngle = human.controls.targetPitchAngle;
		float targetYawAngle = human.controls.targetYawAngle;
		float leftExtend = human.controls.leftExtend;
		float rightExtend = human.controls.rightExtend;
		bool grab = human.controls.leftGrab;
		bool grab2 = human.controls.rightGrab;
		bool onGround = human.onGround;
		Vector3 val = ragdoll.partLeftHand.transform.get_position() - ragdoll.partChest.transform.get_position();
		if (((Vector3)(ref val)).get_sqrMagnitude() > 6f)
		{
			grab = false;
		}
		val = ragdoll.partRightHand.transform.get_position() - ragdoll.partChest.transform.get_position();
		if (((Vector3)(ref val)).get_sqrMagnitude() > 6f)
		{
			grab2 = false;
		}
		Quaternion val2 = Quaternion.Euler(targetPitchAngle, targetYawAngle, 0f);
		Quaternion val3 = Quaternion.Euler(0f, targetYawAngle, 0f);
		Vector3 worldPos = Vector3.get_zero();
		Vector3 worldPos2 = Vector3.get_zero();
		float num = 0f;
		float num2 = 0f;
		if (targetPitchAngle > 0f && onGround)
		{
			num2 = 0.4f * targetPitchAngle / 90f;
		}
		TargetingMode targetingMode = (((Object)(object)ragdoll.partLeftHand.sensor.grabJoint != (Object)null) ? grabTargetingMode : this.targetingMode);
		TargetingMode targetingMode2 = (((Object)(object)ragdoll.partRightHand.sensor.grabJoint != (Object)null) ? grabTargetingMode : this.targetingMode);
		switch (targetingMode)
		{
		case TargetingMode.Shoulder:
			worldPos = ragdoll.partLeftArm.transform.get_position() + val2 * new Vector3(0f, 0f, leftExtend * ragdoll.handLength);
			break;
		case TargetingMode.Chest:
			worldPos = ragdoll.partChest.transform.get_position() + val3 * new Vector3(-0.2f, 0.15f, 0f) + val2 * new Vector3(0f, 0f, leftExtend * ragdoll.handLength);
			break;
		case TargetingMode.Hips:
			if (targetPitchAngle > 0f)
			{
				num = -0.3f * targetPitchAngle / 90f;
			}
			worldPos = ragdoll.partHips.transform.get_position() + val3 * new Vector3(-0.2f, 0.65f + num, num2) + val2 * new Vector3(0f, 0f, leftExtend * ragdoll.handLength);
			break;
		case TargetingMode.Ball:
			if (targetPitchAngle > 0f)
			{
				num = -0.2f * targetPitchAngle / 90f;
			}
			if ((Object)(object)ragdoll.partLeftHand.sensor.grabJoint != (Object)null)
			{
				num2 = (human.isClimbing ? (-0.2f) : 0f);
			}
			worldPos = ragdoll.partBall.transform.get_position() + val3 * new Vector3(-0.2f, 0.7f + num, num2) + val2 * new Vector3(0f, 0f, leftExtend * ragdoll.handLength);
			break;
		}
		switch (targetingMode2)
		{
		case TargetingMode.Shoulder:
			worldPos2 = ragdoll.partRightArm.transform.get_position() + val2 * new Vector3(0f, 0f, rightExtend * ragdoll.handLength);
			break;
		case TargetingMode.Chest:
			worldPos2 = ragdoll.partChest.transform.get_position() + val3 * new Vector3(0.2f, 0.15f, 0f) + val2 * new Vector3(0f, 0f, rightExtend * ragdoll.handLength);
			break;
		case TargetingMode.Hips:
			if (targetPitchAngle > 0f)
			{
				num = -0.3f * targetPitchAngle / 90f;
			}
			worldPos2 = ragdoll.partHips.transform.get_position() + val3 * new Vector3(0.2f, 0.65f + num, num2) + val2 * new Vector3(0f, 0f, rightExtend * ragdoll.handLength);
			break;
		case TargetingMode.Ball:
			if (targetPitchAngle > 0f)
			{
				num = -0.2f * targetPitchAngle / 90f;
			}
			if ((Object)(object)ragdoll.partRightHand.sensor.grabJoint != (Object)null)
			{
				num2 = (human.isClimbing ? (-0.2f) : 0f);
			}
			worldPos2 = ragdoll.partBall.transform.get_position() + val3 * new Vector3(0.2f, 0.7f + num, num2) + val2 * new Vector3(0f, 0f, rightExtend * ragdoll.handLength);
			break;
		}
		ProcessHand(leftMem, ragdoll.partLeftArm, ragdoll.partLeftForearm, ragdoll.partLeftHand, worldPos, leftExtend, grab, motion.legs.legPhase + 0.5f, right: false);
		ProcessHand(rightMem, ragdoll.partRightArm, ragdoll.partRightForearm, ragdoll.partRightHand, worldPos2, rightExtend, grab2, motion.legs.legPhase, right: true);
	}

	private void ProcessHand(ScanMem mem, HumanSegment arm, HumanSegment forearm, HumanSegment hand, Vector3 worldPos, float extend, bool grab, float animationPhase, bool right)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_025d: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		double num = 0.1 + (double)(0.14f * Mathf.Abs(human.controls.targetPitchAngle - mem.grabAngle) / 80f);
		double num2 = num * 2.0;
		if (CheatCodes.climbCheat)
		{
			num2 = (num /= 4.0);
		}
		if (grab && !hand.sensor.grab)
		{
			if ((double)mem.grabTime > num)
			{
				mem.pos = arm.transform.get_position();
			}
			else
			{
				grab = false;
			}
		}
		if (hand.sensor.grab && !grab)
		{
			mem.grabTime = 0f;
			mem.grabAngle = human.controls.targetPitchAngle;
		}
		else
		{
			mem.grabTime += Time.get_fixedDeltaTime();
		}
		hand.sensor.grab = (double)mem.grabTime > num2 && grab;
		if (extend > 0.2f)
		{
			hand.sensor.targetPosition = worldPos;
			mem.shoulder = arm.transform.get_position();
			mem.hand = hand.transform.get_position();
			if ((Object)(object)hand.sensor.grabJoint == (Object)null)
			{
				worldPos = FindTarget(mem, worldPos, out hand.sensor.grabFilter);
			}
			PlaceHand(arm, hand, worldPos, active: true, (Object)(object)hand.sensor.grabJoint != (Object)null, hand.sensor.grabBody);
			if ((Object)(object)hand.sensor.grabBody != (Object)null)
			{
				LiftBody(hand, hand.sensor.grabBody);
			}
			hand.sensor.grabPosition = worldPos;
			return;
		}
		hand.sensor.grabFilter = null;
		if (human.state == HumanState.Walk)
		{
			AnimateHand(arm, forearm, hand, animationPhase, 1f, right);
		}
		else if (human.state == HumanState.FreeFall)
		{
			Vector3 targetDirection = human.targetDirection;
			targetDirection.y = 0f;
			HumanMotion2.AlignToVector(arm, arm.transform.get_up(), -targetDirection, 2f);
			HumanMotion2.AlignToVector(forearm, forearm.transform.get_up(), targetDirection, 2f);
		}
		else
		{
			Vector3 targetDirection2 = human.targetDirection;
			targetDirection2.y = 0f;
			HumanMotion2.AlignToVector(arm, arm.transform.get_up(), -targetDirection2, 20f);
			HumanMotion2.AlignToVector(forearm, forearm.transform.get_up(), targetDirection2, 20f);
		}
	}

	private void AnimateHand(HumanSegment arm, HumanSegment forearm, HumanSegment hand, float phase, float tonus, bool right)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		tonus *= 50f * human.controls.walkSpeed;
		phase -= Mathf.Floor(phase);
		Vector3 val = Quaternion.Euler(0f, human.controls.targetYawAngle, 0f) * Vector3.get_forward();
		Vector3 val2 = Quaternion.Euler(0f, human.controls.targetYawAngle, 0f) * Vector3.get_right();
		if (!right)
		{
			val2 = -val2;
		}
		if (phase < 0.5f)
		{
			HumanMotion2.AlignToVector(arm, arm.transform.get_up(), Vector3.get_down() + val2 / 2f, 3f * tonus);
			HumanMotion2.AlignToVector(forearm, forearm.transform.get_up(), val / 2f - val2, 3f * tonus);
		}
		else
		{
			HumanMotion2.AlignToVector(arm, arm.transform.get_up(), -val + val2 / 2f, 3f * tonus);
			HumanMotion2.AlignToVector(forearm, forearm.transform.get_up(), val + Vector3.get_down(), 3f * tonus);
		}
	}

	private void PlaceHand(HumanSegment arm, HumanSegment hand, Vector3 worldPos, bool active, bool grabbed, Rigidbody grabbedBody)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01af: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0218: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_022b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0302: Unknown result type (might be due to invalid IL or missing references)
		//IL_0307: Unknown result type (might be due to invalid IL or missing references)
		//IL_0309: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0323: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0342: Unknown result type (might be due to invalid IL or missing references)
		//IL_034a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0370: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0391: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_0398: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		if (!active)
		{
			return;
		}
		Rigidbody rigidbody = hand.rigidbody;
		Vector3 worldCenterOfMass = rigidbody.get_worldCenterOfMass();
		Vector3 val = worldPos - worldCenterOfMass;
		new Vector3(0f, val.y, 0f);
		Vector3 velocity = rigidbody.get_velocity() - ragdoll.partBall.rigidbody.get_velocity();
		float num = armMass;
		float num2 = maxForce;
		if (grabbed)
		{
			if ((Object)(object)grabbedBody != (Object)null)
			{
				num += Mathf.Clamp(grabbedBody.get_mass() / 2f, 0f, bodyMass);
				num2 = Mathf.Lerp(grabMaxForce, climbMaxForce, (human.controls.targetPitchAngle - 50f) / 30f);
			}
			else
			{
				num += bodyMass;
				num2 = Mathf.Lerp(grabMaxForce, climbMaxForce, (human.controls.targetPitchAngle - 50f) / 30f);
			}
		}
		float maxAcceleration = num2 / num;
		Vector3 val2 = ConstantAccelerationControl.Solve(val, velocity, maxAcceleration, 0.1f);
		int num3 = 600;
		Vector3 val3 = val2 * num + Vector3.get_up() * gravityForce;
		if ((Object)(object)human.grabbedByHuman != (Object)null && human.grabbedByHuman.state == HumanState.Climb)
		{
			val3 *= 1.7f;
			num3 *= 2;
		}
		if (!grabbed)
		{
			rigidbody.SafeAddForce(val3, (ForceMode)0);
			ragdoll.partHips.rigidbody.SafeAddForce(-val3, (ForceMode)0);
			return;
		}
		Vector3 val4 = human.targetDirection.ZeroY();
		Vector3 normalized = ((Vector3)(ref val4)).get_normalized();
		Vector3 val5 = Mathf.Min(0f, Vector3.Dot(normalized, val3)) * normalized;
		Vector3 val6 = val3 - val5;
		Vector3 val7 = val3.SetX(0f).SetZ(0f);
		Vector3 val8 = -val3 * 0.25f;
		Vector3 val9 = -val3 * 0.75f;
		Vector3 val10 = -val3 * 0.1f - val7 * 0.5f - val6 * 0.25f;
		Vector3 val11 = -val7 * 0.2f - val6 * 0.4f;
		if ((Object)(object)grabbedBody != (Object)null)
		{
			Carryable component = ((Component)grabbedBody).GetComponent<Carryable>();
			if ((Object)(object)component != (Object)null)
			{
				val8 *= component.handForceMultiplier;
				val9 *= component.handForceMultiplier;
			}
		}
		float num4 = ((human.state == HumanState.Climb) ? Mathf.Clamp01((human.controls.targetPitchAngle - 10f) / 60f) : 1f);
		Vector3 val12 = Vector3.Lerp(val10, val8, val.y + 0.5f) * num4;
		Vector3 val13 = Vector3.Lerp(val11, val9, val.y + 0.5f) * num4;
		float num5 = Mathf.Abs(val12.y + val13.y);
		if (num5 > (float)num3)
		{
			val12 *= (float)num3 / num5;
			val13 *= (float)num3 / num5;
		}
		ragdoll.partChest.rigidbody.SafeAddForce(val12, (ForceMode)0);
		ragdoll.partBall.rigidbody.SafeAddForce(val13, (ForceMode)0);
		rigidbody.SafeAddForce(-val12 - val13, (ForceMode)0);
	}

	private void LiftBody(HumanSegment hand, Rigidbody body)
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0178: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0281: Unknown result type (might be due to invalid IL or missing references)
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)human).GetComponent<GroundManager>().IsStanding(((Component)body).get_gameObject()) || ((Component)body).get_tag() == "NoLift")
		{
			return;
		}
		float num = 0.5f + 0.5f * Mathf.InverseLerp(0f, 100f, body.get_mass());
		Vector3 val = (human.targetLiftDirection.ZeroY() * maxPushForce).SetY(Mathf.Max(0f, human.targetLiftDirection.y) * maxLiftForce);
		Vector3 val2 = hand.transform.get_position() - body.get_worldCenterOfMass();
		float magnitude = ((Vector3)(ref val2)).get_magnitude();
		float num2 = num;
		float num3 = 1f;
		float num4 = 1f;
		Carryable component = ((Component)body).GetComponent<Carryable>();
		if ((Object)(object)component != (Object)null)
		{
			num2 *= component.liftForceMultiplier;
			num3 = component.forceHalfDistance;
			num4 = component.damping;
			if (num3 <= 0f)
			{
				throw new InvalidOperationException("halfdistance cant be 0 or less!");
			}
		}
		float num5 = num3 / (num3 + magnitude);
		val *= num2;
		val *= num5;
		body.SafeAddForce(val, (ForceMode)0);
		hand.rigidbody.SafeAddForce(-val * 0.5f, (ForceMode)0);
		ragdoll.partChest.rigidbody.SafeAddForce(-val * 0.5f, (ForceMode)0);
		body.SafeAddTorque(-body.get_angularVelocity() * liftDamp * num4, (ForceMode)5);
		val2 = body.get_angularVelocity();
		Vector3 val3 = -((Vector3)(ref val2)).get_normalized();
		val2 = body.get_angularVelocity();
		body.SafeAddTorque(val3 * ((Vector3)(ref val2)).get_sqrMagnitude() * liftDampSqr * num4, (ForceMode)5);
		if (!((Object)(object)component != (Object)null) || component.aiming == CarryableAiming.None)
		{
			return;
		}
		Vector3 val4 = human.targetLiftDirection;
		if (component.limitAlignToHorizontal)
		{
			val4.y = 0f;
			((Vector3)(ref val4)).Normalize();
		}
		Vector3 val5;
		if (component.aiming != CarryableAiming.ForwardAxis)
		{
			val2 = body.get_worldCenterOfMass() - hand.transform.get_position();
			val5 = ((Vector3)(ref val2)).get_normalized();
		}
		else
		{
			val5 = ((Component)body).get_transform().get_forward();
		}
		Vector3 val6 = val5;
		float aimSpring = component.aimSpring;
		float num6 = ((component.aimTorque < float.PositiveInfinity) ? component.aimTorque : aimSpring);
		if (!component.alwaysForward)
		{
			float num7 = Vector3.Dot(val6, val4);
			if (num7 < 0f)
			{
				val4 = -val4;
				num7 = 0f - num7;
			}
			num6 *= Mathf.Pow(num7, component.aimAnglePower);
		}
		else
		{
			float num8 = Vector3.Dot(val6, val4);
			num8 = 0.5f + num8 / 2f;
			num6 *= Mathf.Pow(num8, component.aimAnglePower);
		}
		if (component.aimDistPower != 0f)
		{
			float num9 = num6;
			val2 = body.get_worldCenterOfMass() - hand.transform.get_position();
			num6 = num9 * Mathf.Pow(((Vector3)(ref val2)).get_magnitude(), component.aimDistPower);
		}
		HumanMotion2.AlignToVector(body, val6, val4, aimSpring, num6);
	}

	private Vector3 FindTarget(ScanMem mem, Vector3 worldPos, out Collider targetCollider)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0158: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0250: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		//IL_027f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0301: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_0318: Unknown result type (might be due to invalid IL or missing references)
		//IL_031a: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_0338: Unknown result type (might be due to invalid IL or missing references)
		//IL_033d: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0376: Unknown result type (might be due to invalid IL or missing references)
		//IL_037b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_038d: Unknown result type (might be due to invalid IL or missing references)
		targetCollider = null;
		Vector3 val = worldPos - mem.shoulder;
		Ray val2 = default(Ray);
		((Ray)(ref val2))._002Ector(mem.shoulder, ((Vector3)(ref val)).get_normalized());
		int num = Physics.OverlapCapsuleNonAlloc(((Ray)(ref val2)).get_origin(), worldPos, 0.5f, colliders, LayerMask.op_Implicit(motion.grabLayers), (QueryTriggerInteraction)1);
		int num2 = 0;
		Vector3 val5;
		while (num2 < num)
		{
			Collider val3 = colliders[num2];
			TargetHelper componentInChildren = ((Component)val3).GetComponentInChildren<TargetHelper>();
			if ((Object)(object)componentInChildren != (Object)null)
			{
				Vector3 val4 = ((Component)componentInChildren).get_transform().get_position() - worldPos;
				val5 = Math3d.ProjectPointOnLineSegment(((Ray)(ref val2)).get_origin(), worldPos, ((Component)componentInChildren).get_transform().get_position()) - ((Component)componentInChildren).get_transform().get_position();
				float magnitude = ((Vector3)(ref val5)).get_magnitude();
				if (magnitude < 0.3f)
				{
					val5 = ((Component)componentInChildren).get_transform().get_position() - mem.hand;
					if (((Vector3)(ref val5)).get_magnitude() < 0.3f)
					{
						worldPos = ((Component)componentInChildren).get_transform().get_position();
						targetCollider = val3;
						goto IL_0122;
					}
				}
				worldPos += val4 * Mathf.InverseLerp(0.5f, 0.3f, magnitude);
				goto IL_0122;
			}
			num2++;
			continue;
			IL_0122:
			return worldPos;
		}
		Vector3 val6 = mem.hand + Vector3.ClampMagnitude(worldPos - mem.hand, 0.3f);
		targetCollider = null;
		Vector3 val7 = val6 - mem.pos;
		Ray val8 = default(Ray);
		((Ray)(ref val8))._002Ector(mem.pos, ((Vector3)(ref val7)).get_normalized());
		Debug.DrawRay(((Ray)(ref val8)).get_origin(), ((Ray)(ref val8)).get_direction() * ((Vector3)(ref val7)).get_magnitude(), Color.get_yellow(), 0.2f);
		float num3 = float.PositiveInfinity;
		Vector3 val9 = val6;
		RaycastHit val10 = default(RaycastHit);
		for (float num4 = 0.05f; num4 <= 0.5f; num4 += 0.05f)
		{
			if (!Physics.SphereCast(val8, num4, ref val10, ((Vector3)(ref val7)).get_magnitude(), LayerMask.op_Implicit(motion.grabLayers), (QueryTriggerInteraction)1))
			{
				continue;
			}
			val5 = val6 - ((RaycastHit)(ref val10)).get_point();
			float magnitude2 = ((Vector3)(ref val5)).get_magnitude();
			magnitude2 += num4 / 10f;
			if (((Component)((RaycastHit)(ref val10)).get_collider()).get_tag() == "Target")
			{
				magnitude2 /= 100f;
			}
			else
			{
				if (num4 > 0.2f)
				{
					continue;
				}
				val5 = worldPos - mem.shoulder;
				Vector3 normalized = ((Vector3)(ref val5)).get_normalized();
				val5 = ((RaycastHit)(ref val10)).get_point() - mem.shoulder;
				Vector3 normalized2 = ((Vector3)(ref val5)).get_normalized();
				if (Vector3.Dot(normalized, normalized2) < 0.7f)
				{
					continue;
				}
			}
			if (magnitude2 < num3)
			{
				num3 = magnitude2;
				val9 = ((RaycastHit)(ref val10)).get_point();
				targetCollider = ((RaycastHit)(ref val10)).get_collider();
			}
		}
		if ((Object)(object)targetCollider != (Object)null)
		{
			Vector3 val11 = val9 - val6;
			val5 = Math3d.ProjectPointOnLineSegment(((Ray)(ref val8)).get_origin(), val6, val9) - val9;
			float magnitude3 = ((Vector3)(ref val5)).get_magnitude();
			if (((Component)targetCollider).get_tag() == "Target")
			{
				if (magnitude3 < 0.2f)
				{
					val5 = mem.hand - val9;
					if (((Vector3)(ref val5)).get_magnitude() < 0.5f)
					{
						worldPos = val9;
						goto IL_0385;
					}
				}
				worldPos = val6 + val11 * Mathf.InverseLerp(0.5f, 0.2f, magnitude3);
				targetCollider = null;
			}
			else if (magnitude3 < 0.1f && ((Vector3)(ref val11)).get_magnitude() < 0.1f)
			{
				worldPos = val9;
			}
			else
			{
				worldPos = val6 + val11 * Mathf.InverseLerp(0.2f, 0.1f, magnitude3);
				targetCollider = null;
			}
		}
		goto IL_0385;
		IL_0385:
		mem.pos = val6;
		return worldPos;
	}
}

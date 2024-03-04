using System;
using UnityEngine;

[Serializable]
public class LegMuscles
{
	private readonly Human human;

	private readonly Ragdoll ragdoll;

	private readonly HumanMotion2 motion;

	private PhysicMaterial ballMaterial;

	private PhysicMaterial footMaterial;

	private float ballFriction;

	private float footFriction;

	private float ballRadius;

	private float stepToAlignOverrideDuration = 0.5f;

	private float stepToAlignOverride;

	public float legPhase;

	private float forwardImpulse;

	private float upImpulse;

	private int framesToApplyJumpImpulse;

	public LegMuscles(Human human, Ragdoll ragdoll, HumanMotion2 motion)
	{
		this.human = human;
		this.ragdoll = ragdoll;
		this.motion = motion;
		ref float val = ref ballRadius;
		Collider collider = ragdoll.partBall.collider;
		val = ((SphereCollider)((collider is SphereCollider) ? collider : null)).get_radius();
		ballMaterial = ragdoll.partBall.collider.get_material();
		footMaterial = ragdoll.partLeftFoot.collider.get_material();
		ballFriction = ballMaterial.get_staticFriction();
		footFriction = footMaterial.get_staticFriction();
	}

	public void OnFixedUpdate(Vector3 torsoFeedback)
	{
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0219: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		stepToAlignOverride -= Time.get_fixedDeltaTime();
		float num = ((human.state == HumanState.Slide) ? 0f : ballFriction);
		if (num != ballMaterial.get_staticFriction())
		{
			PhysicMaterial obj = ballMaterial;
			float staticFriction;
			ballMaterial.set_dynamicFriction(staticFriction = num);
			obj.set_staticFriction(staticFriction);
			PhysicMaterial obj2 = footMaterial;
			footMaterial.set_dynamicFriction(staticFriction = ((human.state == HumanState.Slide) ? 0f : footFriction));
			obj2.set_staticFriction(staticFriction);
			ragdoll.partBall.collider.set_sharedMaterial(ballMaterial);
			ragdoll.partLeftFoot.collider.set_sharedMaterial(footMaterial);
			ragdoll.partRightFoot.collider.set_sharedMaterial(footMaterial);
		}
		switch (human.state)
		{
		case HumanState.Idle:
		{
			int num2 = ((human.controls.leftGrab || human.controls.rightGrab) ? 75 : 90);
			if (Vector2.Angle(VectorExtensions.Rotate(new Vector2(0f, 1f), (0f - human.controls.targetYawAngle) * ((float)Math.PI / 180f)), ragdoll.partHips.transform.get_forward().To2D()) > (float)num2)
			{
				stepToAlignOverride = stepToAlignOverrideDuration;
			}
			if (stepToAlignOverride > 0f)
			{
				RunAnimation(torsoFeedback, 0.5f);
			}
			else
			{
				StandAnimation(torsoFeedback, 1f);
			}
			break;
		}
		case HumanState.Walk:
			RunAnimation(torsoFeedback, 1f);
			break;
		case HumanState.Climb:
			if (human.controls.walkSpeed > 0f)
			{
				RunAnimation(torsoFeedback, 0f);
			}
			else
			{
				StandAnimation(torsoFeedback, 0f);
			}
			break;
		case HumanState.Jump:
			JumpAnimation(torsoFeedback);
			break;
		case HumanState.Slide:
			StandAnimation(torsoFeedback, 1f);
			break;
		case HumanState.Fall:
			NoAnimation(torsoFeedback);
			break;
		case HumanState.FreeFall:
			NoAnimation(torsoFeedback);
			break;
		case HumanState.Unconscious:
			NoAnimation(torsoFeedback);
			break;
		case HumanState.Dead:
			NoAnimation(torsoFeedback);
			break;
		case HumanState.Spawning:
			NoAnimation(torsoFeedback);
			break;
		}
	}

	private void NoAnimation(Vector3 torsoFeedback)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (!CheatCodes.throwCheat || human.state != HumanState.Fall || !((Object)(object)human.grabbedByHuman != (Object)null))
		{
			ragdoll.partHips.rigidbody.SafeAddForce(torsoFeedback, (ForceMode)0);
		}
	}

	private void StandAnimation(Vector3 torsoFeedback, float tonus)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		HumanMotion2.AlignToVector(ragdoll.partLeftThigh, -ragdoll.partLeftThigh.transform.get_up(), Vector3.get_up(), 10f * tonus);
		HumanMotion2.AlignToVector(ragdoll.partRightThigh, -ragdoll.partRightThigh.transform.get_up(), Vector3.get_up(), 10f * tonus);
		HumanMotion2.AlignToVector(ragdoll.partLeftLeg, -ragdoll.partLeftLeg.transform.get_up(), Vector3.get_up(), 10f * tonus);
		HumanMotion2.AlignToVector(ragdoll.partRightLeg, -ragdoll.partRightLeg.transform.get_up(), Vector3.get_up(), 10f * tonus);
		ragdoll.partBall.rigidbody.SafeAddForce(torsoFeedback * 0.2f, (ForceMode)0);
		ragdoll.partLeftFoot.rigidbody.SafeAddForce(torsoFeedback * 0.4f, (ForceMode)0);
		ragdoll.partRightFoot.rigidbody.SafeAddForce(torsoFeedback * 0.4f, (ForceMode)0);
		ragdoll.partBall.rigidbody.set_angularVelocity(Vector3.get_zero());
	}

	private void RunAnimation(Vector3 torsoFeedback, float tonus)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		legPhase = Time.get_realtimeSinceStartup() * 1.5f;
		torsoFeedback += AnimateLeg(ragdoll.partLeftThigh, ragdoll.partLeftLeg, ragdoll.partLeftFoot, legPhase, torsoFeedback, tonus);
		torsoFeedback += AnimateLeg(ragdoll.partRightThigh, ragdoll.partRightLeg, ragdoll.partRightFoot, legPhase + 0.5f, torsoFeedback, tonus);
		ragdoll.partBall.rigidbody.SafeAddForce(torsoFeedback, (ForceMode)0);
		RotateBall();
		AddWalkForce();
	}

	private void JumpAnimation(Vector3 torsoFeedback)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		ragdoll.partHips.rigidbody.SafeAddForce(torsoFeedback, (ForceMode)0);
		if (human.jump)
		{
			float num = 0.75f;
			int num2 = 2;
			float num3 = 2f * num;
			Vector3 gravity = Physics.get_gravity();
			float num4 = Mathf.Sqrt(num3 / ((Vector3)(ref gravity)).get_magnitude());
			float num5 = Mathf.Clamp(human.groundManager.groudSpeed.y, 0f, 100f);
			num5 = Mathf.Pow(num5, 1.2f);
			float num6 = num5;
			gravity = Physics.get_gravity();
			float num7 = (num4 + num6 / ((Vector3)(ref gravity)).get_magnitude()) * human.weight;
			float num8 = human.controls.unsmoothedWalkSpeed * ((float)num2 + num5 / 2f) * human.mass;
			Vector3 momentum = human.momentum;
			float num9 = Vector3.Dot(((Vector3)(ref human.controls.walkDirection)).get_normalized(), momentum);
			if (num9 < 0f)
			{
				num9 = 0f;
			}
			upImpulse = num7 - momentum.y;
			if (upImpulse < 0f)
			{
				upImpulse = 0f;
			}
			forwardImpulse = num8 - num9;
			if (forwardImpulse < 0f)
			{
				forwardImpulse = 0f;
			}
			framesToApplyJumpImpulse = 1;
			if (human.onGround || Time.get_time() - ((Component)human).GetComponent<Ball>().timeSinceLastNonzeroImpulse < 0.2f)
			{
				upImpulse /= framesToApplyJumpImpulse;
				forwardImpulse /= framesToApplyJumpImpulse;
				ApplyJumpImpulses();
				framesToApplyJumpImpulse--;
			}
			human.skipLimiting = true;
			human.jump = false;
		}
		else
		{
			if (framesToApplyJumpImpulse-- > 0)
			{
				ApplyJumpImpulses();
			}
			int num10 = 3;
			int num11 = 500;
			float num12 = human.controls.unsmoothedWalkSpeed * (float)num10 * human.mass;
			Vector3 momentum2 = human.momentum;
			float num13 = Vector3.Dot(((Vector3)(ref human.controls.walkDirection)).get_normalized(), momentum2);
			float num14 = Mathf.Clamp((num12 - num13) / Time.get_fixedDeltaTime(), 0f, (float)num11);
			ragdoll.partChest.rigidbody.SafeAddForce(num14 * ((Vector3)(ref human.controls.walkDirection)).get_normalized(), (ForceMode)0);
		}
	}

	private void ApplyJumpImpulses()
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0115: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0177: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01de: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_020a: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_0235: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0271: Unknown result type (might be due to invalid IL or missing references)
		//IL_028c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_0297: Unknown result type (might be due to invalid IL or missing references)
		//IL_029d: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Unknown result type (might be due to invalid IL or missing references)
		//IL_0350: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_037c: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		for (int i = 0; i < human.groundManager.groundObjects.Count; i++)
		{
			if (human.grabManager.IsGrabbed(human.groundManager.groundObjects[i]))
			{
				num = 0.75f;
			}
		}
		Vector3 val = Vector3.get_up() * upImpulse * num;
		Vector3 val2 = ((Vector3)(ref human.controls.walkDirection)).get_normalized() * forwardImpulse * num;
		ragdoll.partHead.rigidbody.SafeAddForce(val * 0.1f + val2 * 0.1f, (ForceMode)1);
		ragdoll.partChest.rigidbody.SafeAddForce(val * 0.1f + val2 * 0.1f, (ForceMode)1);
		ragdoll.partWaist.rigidbody.SafeAddForce(val * 0.1f + val2 * 0.1f, (ForceMode)1);
		ragdoll.partHips.rigidbody.SafeAddForce(val * 0.1f + val2 * 0.1f, (ForceMode)1);
		ragdoll.partBall.rigidbody.SafeAddForce(val * 0.1f + val2 * 0.1f, (ForceMode)1);
		ragdoll.partLeftThigh.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partRightThigh.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partLeftLeg.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partRightLeg.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partLeftFoot.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partRightFoot.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partLeftArm.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partRightArm.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partLeftForearm.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		ragdoll.partRightForearm.rigidbody.SafeAddForce(val * 0.05f + val2 * 0.05f, (ForceMode)1);
		human.groundManager.DistributeForce(-val / Time.get_fixedDeltaTime(), ragdoll.partBall.rigidbody.get_position());
	}

	private void RotateBall()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		float num = ((human.state == HumanState.Walk) ? 2.5f : 1.2f);
		Vector3 val = default(Vector3);
		((Vector3)(ref val))._002Ector(human.controls.walkDirection.z, 0f, 0f - human.controls.walkDirection.x);
		ragdoll.partBall.rigidbody.set_angularVelocity(num / ballRadius * val);
		Rigidbody rigidbody = ragdoll.partBall.rigidbody;
		Vector3 angularVelocity = ragdoll.partBall.rigidbody.get_angularVelocity();
		rigidbody.set_maxAngularVelocity(((Vector3)(ref angularVelocity)).get_magnitude());
	}

	private void AddWalkForce()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		float num = 300f;
		Vector3 val = human.controls.walkDirection * num;
		ragdoll.partBall.rigidbody.SafeAddForce(val, (ForceMode)0);
		if (human.onGround)
		{
			human.groundManager.DistributeForce(-val, ragdoll.partBall.rigidbody.get_position());
		}
		else if (human.hasGrabbed)
		{
			human.grabManager.DistributeForce(-val * 0.5f);
		}
	}

	private Vector3 AnimateLeg(HumanSegment thigh, HumanSegment leg, HumanSegment foot, float phase, Vector3 torsoFeedback, float tonus)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0167: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0227: Unknown result type (might be due to invalid IL or missing references)
		//IL_022c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_0259: Unknown result type (might be due to invalid IL or missing references)
		//IL_0265: Unknown result type (might be due to invalid IL or missing references)
		tonus *= 20f;
		phase -= Mathf.Floor(phase);
		if (phase < 0.2f)
		{
			HumanMotion2.AlignToVector(thigh, thigh.transform.get_up(), human.controls.walkDirection + Vector3.get_down(), 3f * tonus);
			HumanMotion2.AlignToVector(leg, thigh.transform.get_up(), -human.controls.walkDirection - Vector3.get_up(), tonus);
			Vector3 val = Vector3.get_up() * 20f;
			foot.rigidbody.SafeAddForce(val, (ForceMode)0);
			return -val;
		}
		if (phase < 0.5f)
		{
			HumanMotion2.AlignToVector(thigh, thigh.transform.get_up(), human.controls.walkDirection, 2f * tonus);
			HumanMotion2.AlignToVector(leg, thigh.transform.get_up(), human.controls.walkDirection, 3f * tonus);
		}
		else
		{
			if (phase < 0.7f)
			{
				Vector3 val2 = torsoFeedback * 0.2f;
				foot.rigidbody.SafeAddForce(val2, (ForceMode)0);
				HumanMotion2.AlignToVector(thigh, thigh.transform.get_up(), human.controls.walkDirection + Vector3.get_down(), tonus);
				HumanMotion2.AlignToVector(leg, thigh.transform.get_up(), Vector3.get_down(), tonus);
				return -val2;
			}
			if (phase < 0.9f)
			{
				Vector3 val3 = torsoFeedback * 0.2f;
				foot.rigidbody.SafeAddForce(val3, (ForceMode)0);
				HumanMotion2.AlignToVector(thigh, thigh.transform.get_up(), -human.controls.walkDirection + Vector3.get_down(), tonus);
				HumanMotion2.AlignToVector(leg, thigh.transform.get_up(), -human.controls.walkDirection + Vector3.get_down(), tonus);
				return -val3;
			}
			HumanMotion2.AlignToVector(thigh, thigh.transform.get_up(), -human.controls.walkDirection + Vector3.get_down(), tonus);
			HumanMotion2.AlignToVector(leg, thigh.transform.get_up(), -human.controls.walkDirection, tonus);
		}
		return Vector3.get_zero();
	}
}

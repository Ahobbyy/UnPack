using HumanAPI;
using UnityEngine;

public class Catapult : MonoBehaviour, IPostReset
{
	public enum CatapultState
	{
		Idle,
		Wind,
		Trigger,
		Fire
	}

	public Sound2 ratchetSound;

	public Sound2 releaseSound;

	public CatapultState state;

	public CatapultRope rope;

	public Rigidbody arm;

	public Rigidbody body;

	public Rigidbody windlass;

	public HingeJoint release;

	public Transform springRatchet;

	public Transform springFrame;

	private bool shoot;

	private bool armed;

	public float gearRatio = 12f;

	public float windlassAngle;

	public float topWindlassAngle = 660f;

	public float bottomWindlassAngle = -276f;

	public float currentTooth = -90f;

	private float toothStep = 30f;

	private float speed;

	private float acceleration = -3600f;

	private float oldPullAngle;

	private Vector3 startPos;

	private Quaternion startRot;

	private float initialWindlassAngle;

	private float timeReleased;

	private float timeFired;

	private bool fired;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		startPos = ((Component)this).get_transform().get_position();
		startRot = ((Component)this).get_transform().get_rotation();
		initialWindlassAngle = windlassAngle;
	}

	private void Start()
	{
		LockPull();
		LockTrigger();
		PostResetState(0);
	}

	public void PostResetState(int checkpoint)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		shoot = (armed = false);
		currentTooth = (windlassAngle = initialWindlassAngle);
		SetState(CatapultState.Idle);
		LeaveIdle();
		EnterWind();
		body.set_velocity(Vector3.get_zero());
		body.set_angularVelocity(Vector3.get_zero());
		((Component)this).get_transform().set_position(startPos);
		((Component)this).get_transform().set_rotation(startRot);
		((Component)arm).get_transform().set_rotation(WriteArm());
		((Component)windlass).get_transform().set_rotation(WriteWindlass());
		LeaveWind();
		EnterIdle();
	}

	private void FixedUpdate()
	{
		switch (state)
		{
		case CatapultState.Idle:
			UpdateIdle();
			break;
		case CatapultState.Wind:
			UpdateWind();
			break;
		case CatapultState.Trigger:
			UpdateTrigger();
			break;
		case CatapultState.Fire:
			UpdateFire();
			break;
		}
		rope.catapultWindCount = 1.5f - windlassAngle / 360f;
	}

	private void SetState(CatapultState newState)
	{
		if (newState != state)
		{
			switch (state)
			{
			case CatapultState.Idle:
				LeaveIdle();
				break;
			case CatapultState.Wind:
				LeaveWind();
				break;
			case CatapultState.Trigger:
				LeaveTrigger();
				break;
			case CatapultState.Fire:
				LeaveFire();
				break;
			}
			state = newState;
			switch (state)
			{
			case CatapultState.Idle:
				EnterIdle();
				break;
			case CatapultState.Wind:
				EnterWind();
				break;
			case CatapultState.Trigger:
				EnterTrigger();
				break;
			case CatapultState.Fire:
				EnterFire();
				break;
			}
		}
	}

	private void EnterIdle()
	{
		body.set_isKinematic(false);
		arm.set_isKinematic(false);
		((Joint)((Component)arm).get_gameObject().AddComponent<FixedJoint>()).set_connectedBody(body);
	}

	private void LeaveIdle()
	{
		Object.DestroyImmediate((Object)(object)((Component)arm).GetComponent<FixedJoint>());
		body.set_isKinematic(true);
		arm.set_isKinematic(true);
	}

	private void UpdateIdle()
	{
		if (GrabManager.IsGrabbedAny(((Component)windlass).get_gameObject()))
		{
			SetState(CatapultState.Wind);
		}
		else if (windlassAngle != topWindlassAngle && GrabManager.IsGrabbedAny(((Component)release).get_gameObject()))
		{
			SetState(CatapultState.Trigger);
		}
	}

	private void EnterWind()
	{
		UnlockPull();
		UnlockTrigger();
	}

	private void LeaveWind()
	{
		LockPull();
		LockTrigger();
	}

	private void UpdateWind()
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		if (!GrabManager.IsGrabbedAny(((Component)windlass).get_gameObject()))
		{
			if (timeReleased > 1f)
			{
				SetState(CatapultState.Idle);
				return;
			}
			timeReleased += Time.get_fixedDeltaTime();
		}
		else
		{
			timeReleased = 0f;
		}
		if (armed)
		{
			WriteWindlass();
			WriteArm();
			return;
		}
		ReadWindlass();
		if (windlassAngle < bottomWindlassAngle)
		{
			SetArmed(value: true);
			ratchetSound.PlayOneShot();
		}
		else if (windlassAngle < currentTooth - toothStep)
		{
			float pitch = 0.5f + 0.5f * Mathf.InverseLerp(topWindlassAngle, bottomWindlassAngle, windlassAngle);
			ratchetSound.PlayOneShot(1f, pitch);
			currentTooth -= toothStep;
		}
		WriteArm();
		PullBackWindlass();
		PullDownTrigger();
	}

	private void PullBackWindlass()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		int num = (armed ? 2000 : 30);
		windlass.SafeAddForceAtPosition(-((Component)body).get_transform().get_forward() * (float)num, springRatchet.get_position(), (ForceMode)0);
		body.SafeAddForceAtPosition(((Component)body).get_transform().get_forward() * (float)num, springRatchet.get_position(), (ForceMode)0);
	}

	private void PullDownTrigger()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		int num = ((state == CatapultState.Trigger || state == CatapultState.Wind) ? 50 : 3000);
		((Component)release).GetComponent<Rigidbody>().SafeAddForceAtPosition(-((Component)body).get_transform().get_up() * (float)num, springRatchet.get_position(), (ForceMode)0);
		body.SafeAddForceAtPosition(((Component)body).get_transform().get_up() * (float)num, springRatchet.get_position(), (ForceMode)0);
	}

	private void EnterTrigger()
	{
		state = CatapultState.Trigger;
		UnlockTrigger();
	}

	private void LeaveTrigger()
	{
		LockTrigger();
	}

	private void UpdateTrigger()
	{
		if (!GrabManager.IsGrabbedAny(((Component)release).get_gameObject()))
		{
			SetState(CatapultState.Idle);
		}
		else
		{
			if (!(release.get_angle() < -5f))
			{
				return;
			}
			shoot = true;
			speed = 0f;
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				bool flag = human.groundManager.IsStanding(((Component)arm).get_gameObject());
				if ((Object)(object)human.ragdoll.partLeftHand.sensor.grabBody == (Object)(object)((Component)release).GetComponent<Rigidbody>() && (flag || (Object)(object)human.ragdoll.partRightHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>()))
				{
					human.ragdoll.partLeftHand.sensor.ReleaseGrab(1f);
				}
				if ((Object)(object)human.ragdoll.partRightHand.sensor.grabBody == (Object)(object)((Component)release).GetComponent<Rigidbody>() && (flag || (Object)(object)human.ragdoll.partLeftHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>()))
				{
					human.ragdoll.partRightHand.sensor.ReleaseGrab(1f);
				}
				human.ragdoll.ToggleHeavyArms((Object)(object)human.ragdoll.partLeftHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>(), (Object)(object)human.ragdoll.partRightHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>());
				if (flag)
				{
					StatsAndAchievements.UnlockAchievement(Achievement.ACH_SIEGE_HUMAN_CANNON);
				}
			}
			SetState(CatapultState.Fire);
			releaseSound.PlayOneShot();
		}
	}

	private void EnterFire()
	{
		state = CatapultState.Fire;
		SetArmed(value: false);
		UnlockPull();
		UnlockTrigger();
		fired = false;
	}

	private void LeaveFire()
	{
		LockPull();
		LockTrigger();
	}

	private void UpdateFire()
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		if (fired)
		{
			if (!GrabManager.IsGrabbedAny(((Component)release).get_gameObject()))
			{
				if (timeFired > 1f)
				{
					SetState(CatapultState.Idle);
					return;
				}
				PullDownTrigger();
				timeFired += Time.get_fixedDeltaTime();
			}
			else
			{
				timeFired = 0f;
			}
			WriteArm();
			WriteWindlass();
			return;
		}
		timeFired = 0f;
		bool flag = GrabManager.IsGrabbedAny(((Component)arm).get_gameObject()) || GroundManager.IsStandingAny(((Component)arm).get_gameObject());
		speed += acceleration * Time.get_fixedDeltaTime() * (flag ? 0.9f : 1f);
		windlassAngle -= speed * Time.get_fixedDeltaTime();
		if (windlassAngle > topWindlassAngle)
		{
			currentTooth = (windlassAngle = topWindlassAngle);
		}
		WriteArm();
		WriteWindlass();
		if (windlassAngle != topWindlassAngle)
		{
			return;
		}
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			if ((Object)(object)human.ragdoll.partLeftHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>())
			{
				human.ragdoll.partLeftHand.sensor.ReleaseGrab(0.1f);
			}
			if ((Object)(object)human.ragdoll.partRightHand.sensor.grabBody == (Object)(object)((Component)arm).GetComponent<Rigidbody>())
			{
				human.ragdoll.partRightHand.sensor.ReleaseGrab(0.1f);
			}
			human.ragdoll.ReleaseHeavyArms();
		}
		fired = true;
	}

	private void ReadWindlass()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Quaternion localRotation = ((Component)windlass).get_transform().get_localRotation();
		float num;
		for (num = ((Quaternion)(ref localRotation)).get_eulerAngles().z - windlassAngle; num < -180f; num += 360f)
		{
		}
		while (num > 180f)
		{
			num -= 360f;
		}
		windlassAngle += num;
	}

	private Quaternion WriteWindlass()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = ((Component)windlass).get_transform().get_parent().get_rotation() * Quaternion.Euler(0f, -90f, windlassAngle);
		windlass.MoveRotation(val);
		return val;
	}

	private Quaternion WriteArm()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = ((Component)arm).get_transform().get_parent().get_rotation() * Quaternion.Euler((0f - windlassAngle) / gearRatio, 0f, 0f);
		arm.MoveRotation(val);
		return val;
	}

	private void LockPull()
	{
		((Joint)((Component)windlass).get_gameObject().AddComponent<FixedJoint>()).set_connectedBody(body);
	}

	private void UnlockPull()
	{
		Object.DestroyImmediate((Object)(object)((Component)windlass).GetComponent<FixedJoint>());
	}

	private void LockTrigger()
	{
		((Joint)((Component)release).get_gameObject().AddComponent<FixedJoint>()).set_connectedBody(body);
	}

	private void UnlockTrigger()
	{
		Object.DestroyImmediate((Object)(object)((Component)release).GetComponent<FixedJoint>());
	}

	private void SetArmed(bool value)
	{
		if (armed != value)
		{
			armed = value;
		}
	}

	public Catapult()
		: this()
	{
	}
}

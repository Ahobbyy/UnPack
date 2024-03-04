using System;
using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class Human : HumanBase
{
	public static Human instance;

	public static List<Human> all = new List<Human>();

	public bool spawning;

	private static Human localPlayer = null;

	public Vector3 targetDirection;

	public Vector3 targetLiftDirection;

	public bool jump;

	public bool disableInput;

	public NetPlayer player;

	public Ragdoll ragdoll;

	public HumanControls controls;

	internal GroundManager groundManager;

	internal GrabManager grabManager;

	[NonSerialized]
	public HumanMotion2 motionControl2;

	public HumanState state;

	public bool onGround;

	public float groundAngle;

	public bool hasGrabbed;

	public bool isClimbing;

	public Human grabbedByHuman;

	public float wakeUpTime;

	internal float maxWakeUpTime = 2f;

	public float unconsciousTime;

	private float maxUnconsciousTime = 3f;

	private Vector3 grabStartPosition;

	[NonSerialized]
	public Rigidbody[] rigidbodies;

	private Vector3[] velocities;

	public float weight;

	public float mass;

	private Vector3 lastVelocity;

	private float totalHit;

	private float lastFrameHit;

	private float thisFrameHit;

	private float fallTimer;

	private float groundDelay;

	private float jumpDelay;

	private float slideTimer;

	private float[] groundAngles = new float[60];

	private int groundAnglesIdx;

	private float groundAnglesSum;

	private float lastGroundAngle;

	private uint evtScroll;

	private NetIdentity identity;

	private FixedJoint hook;

	public bool skipLimiting;

	private bool isFallSpeedInitialized;

	private bool isFallSpeedLimited;

	private bool overridenDrag;

	public static Human Localplayer
	{
		get
		{
			if ((Object)(object)localPlayer != (Object)null && localPlayer.IsLocalPlayer)
			{
				return localPlayer;
			}
			foreach (Human item in all)
			{
				if ((Object)(object)item != (Object)null && item.IsLocalPlayer)
				{
					localPlayer = item;
				}
			}
			Debug.Assert((Object)(object)localPlayer != (Object)null);
			return localPlayer;
		}
	}

	public bool IsLocalPlayer
	{
		get
		{
			if ((Object)(object)player != (Object)null)
			{
				return player.isLocalPlayer;
			}
			return false;
		}
	}

	public Vector3 momentum
	{
		get
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Vector3.get_zero();
			for (int i = 0; i < rigidbodies.Length; i++)
			{
				Rigidbody val2 = rigidbodies[i];
				val += val2.get_velocity() * val2.get_mass();
			}
			return val;
		}
	}

	public Vector3 velocity => momentum / mass;

	private void OnEnable()
	{
		all.Add(this);
		instance = this;
		grabManager = ((Component)this).GetComponent<GrabManager>();
		groundManager = ((Component)this).GetComponent<GroundManager>();
		motionControl2 = ((Component)this).GetComponent<HumanMotion2>();
		controls = ((Component)this).GetComponentInParent<HumanControls>();
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	public void Initialize()
	{
		ragdoll = ((Component)this).GetComponentInChildren<Ragdoll>();
		motionControl2.Initialize();
		ServoSound componentInChildren = ((Component)this).GetComponentInChildren<ServoSound>();
		HumanHead humanHead = ((Component)ragdoll.partHead.transform).get_gameObject().AddComponent<HumanHead>();
		humanHead.sounds = componentInChildren;
		humanHead.humanAudio = ((Component)this).GetComponentInChildren<HumanAudio>();
		((Component)componentInChildren).get_transform().SetParent(((Component)humanHead).get_transform(), false);
		InitializeBodies();
	}

	private void InitializeBodies()
	{
		rigidbodies = ((Component)this).GetComponentsInChildren<Rigidbody>();
		velocities = (Vector3[])(object)new Vector3[rigidbodies.Length];
		mass = 0f;
		for (int i = 0; i < rigidbodies.Length; i++)
		{
			Rigidbody val = rigidbodies[i];
			if ((Object)(object)val != (Object)null)
			{
				val.set_maxAngularVelocity(10f);
				mass += val.get_mass();
			}
		}
		weight = mass * 9.81f;
	}

	internal void ReceiveHit(Vector3 impulse)
	{
		thisFrameHit = Mathf.Max(thisFrameHit, ((Vector3)(ref impulse)).get_magnitude());
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0243: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0405: Unknown result type (might be due to invalid IL or missing references)
		//IL_040a: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0430: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_044c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0456: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0474: Unknown result type (might be due to invalid IL or missing references)
		//IL_0479: Unknown result type (might be due to invalid IL or missing references)
		//IL_047b: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_047f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0495: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Unknown result type (might be due to invalid IL or missing references)
		if (thisFrameHit + lastFrameHit > 30f)
		{
			MakeUnconscious();
			ReleaseGrab(3f);
		}
		lastFrameHit = thisFrameHit;
		thisFrameHit = 0f;
		jumpDelay -= Time.get_fixedDeltaTime();
		groundDelay -= Time.get_fixedDeltaTime();
		if (!disableInput)
		{
			ProcessInput();
		}
		LimitFallSpeed();
		Quaternion val = Quaternion.Euler(controls.targetPitchAngle, controls.targetYawAngle, 0f);
		targetDirection = val * Vector3.get_forward();
		targetLiftDirection = Quaternion.Euler(Mathf.Clamp(controls.targetPitchAngle, -70f, 80f), controls.targetYawAngle, 0f) * Vector3.get_forward();
		if (NetGame.isClient || ReplayRecorder.isPlaying)
		{
			return;
		}
		if (state == HumanState.Dead || state == HumanState.Unconscious || state == HumanState.Spawning)
		{
			controls.leftGrab = (controls.rightGrab = false);
			controls.shootingFirework = false;
		}
		groundAngle = 90f;
		groundAngle = Mathf.Min(groundAngle, ragdoll.partBall.sensor.groundAngle);
		groundAngle = Mathf.Min(groundAngle, ragdoll.partLeftFoot.sensor.groundAngle);
		groundAngle = Mathf.Min(groundAngle, ragdoll.partRightFoot.sensor.groundAngle);
		bool num = hasGrabbed;
		onGround = groundDelay <= 0f && groundManager.onGround;
		hasGrabbed = grabManager.hasGrabbed;
		ragdoll.partBall.sensor.groundAngle = (ragdoll.partLeftFoot.sensor.groundAngle = (ragdoll.partRightFoot.sensor.groundAngle = 90f));
		if (hasGrabbed && ((Component)this).get_transform().get_position().y < grabStartPosition.y)
		{
			grabStartPosition = ((Component)this).get_transform().get_position();
		}
		if (hasGrabbed && ((Component)this).get_transform().get_position().y - grabStartPosition.y > 0.5f)
		{
			isClimbing = true;
		}
		else
		{
			isClimbing = false;
		}
		if (num != hasGrabbed && hasGrabbed)
		{
			grabStartPosition = ((Component)this).get_transform().get_position();
		}
		if (state == HumanState.Spawning)
		{
			spawning = true;
			if (onGround)
			{
				MakeUnconscious();
			}
		}
		else
		{
			spawning = false;
		}
		ProcessUnconscious();
		if (state != HumanState.Dead && state != HumanState.Unconscious && state != HumanState.Spawning)
		{
			ProcessFall();
			if (onGround)
			{
				if (controls.jump && jumpDelay <= 0f)
				{
					state = HumanState.Jump;
					jump = true;
					jumpDelay = 0.5f;
					groundDelay = 0.2f;
				}
				else if (controls.walkSpeed > 0f)
				{
					state = HumanState.Walk;
				}
				else
				{
					state = HumanState.Idle;
				}
			}
			else if ((Object)(object)ragdoll.partLeftHand.sensor.grabObject != (Object)null || (Object)(object)ragdoll.partRightHand.sensor.grabObject != (Object)null)
			{
				state = HumanState.Climb;
			}
		}
		if (skipLimiting)
		{
			skipLimiting = false;
			return;
		}
		for (int i = 0; i < rigidbodies.Length; i++)
		{
			Vector3 val2 = velocities[i];
			Vector3 val3 = rigidbodies[i].get_velocity();
			Vector3 val4 = val3 - val2;
			if (Vector3.Dot(val2, val4) < 0f)
			{
				Vector3 normalized = ((Vector3)(ref val2)).get_normalized();
				float magnitude = ((Vector3)(ref val2)).get_magnitude();
				float num2 = Mathf.Clamp(0f - Vector3.Dot(normalized, val4), 0f, magnitude);
				val4 += normalized * num2;
			}
			float num3 = 1000f * Time.get_deltaTime();
			if (((Vector3)(ref val4)).get_magnitude() > num3)
			{
				Vector3 val5 = Vector3.ClampMagnitude(val4, num3);
				val3 -= val4 - val5;
				rigidbodies[i].set_velocity(val3);
			}
			velocities[i] = val3;
		}
	}

	private void ProcessInput()
	{
		if (!NetGame.isClient && !ReplayRecorder.isPlaying)
		{
			if (controls.unconscious)
			{
				MakeUnconscious();
			}
			if (((Behaviour)motionControl2).get_enabled())
			{
				motionControl2.OnFixedUpdate();
			}
		}
	}

	private void PushGroundAngle()
	{
		float num = (lastGroundAngle = ((onGround && groundAngle < 80f) ? groundAngle : lastGroundAngle));
		groundAnglesSum -= groundAngles[groundAnglesIdx];
		groundAnglesSum += num;
		groundAngles[groundAnglesIdx] = num;
		groundAnglesIdx = (groundAnglesIdx + 1) % groundAngles.Length;
	}

	private void ProcessFall()
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		PushGroundAngle();
		bool flag = false;
		if (groundAnglesSum / (float)groundAngles.Length > 45f)
		{
			flag = true;
			slideTimer = 0f;
			onGround = false;
			state = HumanState.Slide;
		}
		else if (state == HumanState.Slide && groundAnglesSum / (float)groundAngles.Length < 37f && ragdoll.partBall.rigidbody.get_velocity().y > -1f)
		{
			slideTimer += Time.get_fixedDeltaTime();
			if (slideTimer < 0.003f)
			{
				onGround = false;
			}
		}
		if (!onGround && !flag)
		{
			if (fallTimer < 5f)
			{
				fallTimer += Time.get_deltaTime();
			}
			if (state == HumanState.Climb)
			{
				fallTimer = 0f;
			}
			if (fallTimer > 3f)
			{
				state = HumanState.FreeFall;
			}
			else if (fallTimer > 1f)
			{
				state = HumanState.Fall;
			}
		}
		else
		{
			fallTimer = 0f;
		}
	}

	private void ProcessUnconscious()
	{
		if (state == HumanState.Unconscious)
		{
			unconsciousTime -= Time.get_fixedDeltaTime();
			if (unconsciousTime <= 0f)
			{
				state = HumanState.Fall;
				wakeUpTime = maxWakeUpTime;
				unconsciousTime = 0f;
			}
		}
		if (wakeUpTime > 0f)
		{
			wakeUpTime -= Time.get_fixedDeltaTime();
			if (wakeUpTime <= 0f)
			{
				wakeUpTime = 0f;
			}
		}
	}

	public void MakeUnconscious(float time)
	{
		unconsciousTime = time;
		state = HumanState.Unconscious;
	}

	public void MakeUnconscious()
	{
		unconsciousTime = maxUnconsciousTime;
		state = HumanState.Unconscious;
	}

	public void Reset()
	{
		groundManager.Reset();
		grabManager.Reset();
		for (int i = 0; i < groundAngles.Length; i++)
		{
			groundAngles[i] = 0f;
		}
		groundAnglesSum = 0f;
	}

	public void SpawnAt(Vector3 pos)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		state = HumanState.Spawning;
		Vector3 val = KillHorizontalVelocity();
		int num = 2;
		if ((Object)(object)Game.currentLevel != (Object)null)
		{
			float maxHumanVelocity = Game.currentLevel.MaxHumanVelocity;
			if (((Vector3)(ref val)).get_magnitude() > maxHumanVelocity)
			{
				ControlVelocity(maxHumanVelocity, killHorizontal: false);
				((Vector3)(ref val))._002Ector(0f, 0f - maxHumanVelocity, 0f);
			}
		}
		Vector3 position = pos - val * (float)num - Physics.get_gravity() * (float)num * (float)num / 2f;
		SetPosition(position);
		if (((Vector3)(ref val)).get_magnitude() < 5f)
		{
			AddRandomTorque(1f);
		}
		Reset();
	}

	public void SpawnAt(Transform spawnPoint, Vector3 offset)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		SpawnAt(offset + spawnPoint.get_position());
	}

	public Vector3 LimitHorizontalVelocity(float max)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody[] array = rigidbodies;
		Vector3 val = velocity;
		Vector3 val2 = val;
		val2.y = 0f;
		if (((Vector3)(ref val2)).get_magnitude() < max)
		{
			return val;
		}
		val2 -= Vector3.ClampMagnitude(val2, max);
		val -= val2;
		foreach (Rigidbody obj in array)
		{
			obj.set_velocity(obj.get_velocity() + -val2);
		}
		return val;
	}

	public Vector3 KillHorizontalVelocity()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody[] array = rigidbodies;
		Vector3 val = velocity;
		Vector3 val2 = val;
		val2.y = 0f;
		val -= val2;
		foreach (Rigidbody obj in array)
		{
			obj.set_velocity(obj.get_velocity() + -val2);
		}
		return val;
	}

	public Vector3 ControlVelocity(float maxVelocity, bool killHorizontal)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Rigidbody[] array = rigidbodies;
		Vector3 val = velocity;
		Vector3 val2 = val;
		val2.y = 0f;
		Vector3 val3 = ((!killHorizontal) ? (Vector3.ClampMagnitude(val, maxVelocity) - val) : (Vector3.ClampMagnitude(val - val2, maxVelocity) - val));
		foreach (Rigidbody obj in array)
		{
			obj.set_velocity(obj.get_velocity() + val3);
		}
		return val;
	}

	public void AddRandomTorque(float multiplier)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector3 torque = Random.get_onUnitSphere() * 100f * multiplier;
		for (int i = 0; i < rigidbodies.Length; i++)
		{
			rigidbodies[i].SafeAddTorque(torque, (ForceMode)2);
		}
	}

	private void Start()
	{
		identity = ((Component)this).GetComponentInParent<NetIdentity>();
		if ((Object)(object)identity != (Object)null)
		{
			evtScroll = identity.RegisterEvent(OnScroll);
		}
	}

	private void OnScroll(NetStream stream)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Vector3 scroll = NetVector3.Read(stream, 12).Dequantize(500f);
		Scroll(scroll);
	}

	public void SetPosition(Vector3 spawnPos)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient && !ReplayRecorder.isPlaying)
		{
			Vector3 scroll = spawnPos - ((Component)this).get_transform().get_position();
			Scroll(scroll);
		}
	}

	private void Scroll(Vector3 scroll)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient && !ReplayRecorder.isPlaying)
		{
			Transform transform = ((Component)this).get_transform();
			transform.set_position(transform.get_position() + scroll);
		}
		if (player.isLocalPlayer)
		{
			CloudSystem.instance.Scroll(scroll);
			player.cameraController.Scroll(scroll);
			for (int i = 0; i < CloudBox.all.Count; i++)
			{
				CloudBox.all[i].FadeIn(1f);
			}
		}
		if ((Object)(object)identity != (Object)null && (NetGame.isServer || ReplayRecorder.isRecording))
		{
			NetStream stream = identity.BeginEvent(evtScroll);
			NetVector3.Quantize(scroll, 500f, 12).Write(stream);
			identity.EndEvent();
		}
	}

	public void ReleaseGrab(float blockTime = 0f)
	{
		ragdoll.partLeftHand.sensor.ReleaseGrab(blockTime);
		ragdoll.partRightHand.sensor.ReleaseGrab(blockTime);
	}

	public void ReleaseGrab(GameObject item, float blockTime = 0f)
	{
		if (ragdoll.partLeftHand.sensor.IsGrabbed(item))
		{
			ragdoll.partLeftHand.sensor.ReleaseGrab(blockTime);
		}
		if (ragdoll.partRightHand.sensor.IsGrabbed(item))
		{
			ragdoll.partRightHand.sensor.ReleaseGrab(blockTime);
		}
	}

	internal void Show()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Object.Destroy((Object)(object)hook);
		SetPosition(new Vector3(0f, 50f, 0f));
	}

	internal void Hide()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		SetPosition(new Vector3(0f, 500f, 0f));
		hook = ((Component)ragdoll.partHead.rigidbody).get_gameObject().AddComponent<FixedJoint>();
	}

	public void SetDrag(float drag, bool external = true)
	{
		if (external || !overridenDrag)
		{
			overridenDrag = external;
			for (int i = 0; i < rigidbodies.Length; i++)
			{
				rigidbodies[i].set_drag(drag);
			}
		}
	}

	public void ResetDrag()
	{
		overridenDrag = false;
		isFallSpeedInitialized = false;
		LimitFallSpeed();
	}

	private void LimitFallSpeed()
	{
		bool flag = Game.instance.state != GameState.PlayingLevel;
		if (isFallSpeedLimited != flag || !isFallSpeedInitialized)
		{
			isFallSpeedInitialized = true;
			isFallSpeedLimited = flag;
			if (flag)
			{
				SetDrag(0.1f, external: false);
			}
			else
			{
				SetDrag(0.05f, external: false);
			}
		}
	}
}

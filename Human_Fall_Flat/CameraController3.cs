using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class CameraController3 : MonoBehaviour
{
	public CameraMode mode;

	public LayerMask wallLayers;

	public Vector3 farTargetOffset;

	public float farRange;

	public float farRangeAction;

	public Vector3 closeTargetOffset;

	public float closeRange;

	public float closeRangeLookUp;

	public Vector3 fpsTargetOffset;

	private float nearClip = 0.05f;

	public bool ignoreWalls;

	public Human human;

	public Camera gameCam;

	public WaterSensor waterSensor;

	private Ragdoll ragdoll;

	public static float fovAdjust = 0f;

	public static float smoothingAmount = 0.5f;

	private List<float> offsetSmoothingFast = new List<float>();

	private List<float> offsetSmoothingSlow = new List<float>();

	private float wallHold = 1000f;

	private float wallHoldTime;

	[NonSerialized]
	public float offset = 4f;

	private Vector3 oldTarget;

	private Vector3 smoothTarget;

	private int oldFrame;

	private Vector3 fixedupdateSmooth;

	private float offsetSpeed;

	public float headPivotAhead;

	private float pitchRange = 30f;

	private float oldPitchSign;

	private float pitchExpandTimer;

	private float cameraPitchAngle;

	private float standPhase;

	private float holdPhase;

	private Vector3[] rayStarts = (Vector3[])(object)new Vector3[4];

	private float cameraTransitionSpeed = 1f;

	private float cameraTransitionPhase;

	private float startFov;

	private Vector3 startOffset;

	private Quaternion startRotation = Quaternion.get_identity();

	private Vector3 startHumanCameraPos;

	private void Start()
	{
		ragdoll = human.ragdoll;
		waterSensor = ((Component)this).get_gameObject().AddComponent<WaterSensor>();
		Shell.RegisterCommand("smooth", OnCameraSmooth, "smooth <smoothing>\r\nSet camera smoothing amount (0-none, 0.5-default, 1-max)");
		Shell.RegisterCommand("fov", OnFOV, "fov <fov_adjust>\r\nSet FOV adjust (-10 - narrow, 0 - default, 30 - wide)");
		Shell.RegisterCommand("hdr", OnHDR, "hdr\r\nToggle high dynamic range");
	}

	private void OnHDR(string param)
	{
		if (!string.IsNullOrEmpty(param))
		{
			param = param.ToLowerInvariant();
			if ("off".Equals(param))
			{
				Options.advancedVideoHDR = 0;
			}
			else if ("on".Equals(param))
			{
				Options.advancedVideoHDR = 1;
			}
		}
		else
		{
			Options.advancedVideoHDR = ((Options.advancedVideoHDR <= 0) ? 1 : 0);
		}
		Options.ApplyAdvancedVideo();
		Shell.Print("HDR " + ((Options.advancedVideoHDR > 0) ? "on" : "off"));
	}

	private static void OnCameraSmooth(string param)
	{
		if (!string.IsNullOrEmpty(param))
		{
			param = param.ToLowerInvariant();
			if (float.TryParse(param, out var result))
			{
				Options.cameraSmoothing = Mathf.Clamp((int)(result * 20f), 0, 40);
			}
			else if ("off".Equals(param))
			{
				Options.cameraSmoothing = 0;
			}
			else if ("on".Equals(param))
			{
				Options.cameraSmoothing = 20;
			}
		}
		Shell.Print("Camera smoothing " + smoothingAmount);
	}

	private static void OnFOV(string param)
	{
		if (!string.IsNullOrEmpty(param) && float.TryParse(param, out var result))
		{
			Options.cameraFov = Mathf.Clamp((int)(result / 2f + 5f), 0, 20);
		}
		Shell.Print("FOV adjust " + fovAdjust);
	}

	public void Scroll(Vector3 offset)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		oldTarget += offset;
		smoothTarget += offset;
		Transform transform = ((Component)this).get_transform();
		transform.set_position(transform.get_position() + offset);
		fixedupdateSmooth += offset;
	}

	private Vector3 SmoothCamera(Vector3 target, float deltaTime)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		if (ReplayRecorder.isPlaying)
		{
			deltaTime = (float)Mathf.Abs(ReplayRecorder.instance.currentFrame - oldFrame) * Time.get_fixedDeltaTime();
			oldFrame = ReplayRecorder.instance.currentFrame;
			if (deltaTime == 0f)
			{
				return smoothTarget;
			}
		}
		if (NetGame.isClient)
		{
			deltaTime = (float)Mathf.Abs(human.player.renderedFrame - oldFrame) * Time.get_fixedDeltaTime();
			oldFrame = human.player.renderedFrame;
			if (deltaTime == 0f)
			{
				return smoothTarget;
			}
		}
		int num = Mathf.RoundToInt(deltaTime / (Time.get_fixedDeltaTime() / 10f));
		if (num < 1)
		{
			num = 1;
		}
		Vector3 val;
		if (smoothingAmount != 0f && deltaTime != 0f && num <= 1000)
		{
			val = target - oldTarget;
			if (!(((Vector3)(ref val)).get_magnitude() > 10f))
			{
				float num2 = deltaTime / (float)num;
				float num3 = offset * 0.1f * smoothingAmount;
				Vector3 val2 = (target - oldTarget) / deltaTime;
				for (int i = 0; i < num; i++)
				{
					oldTarget += val2 * num2;
					Vector3 val3 = oldTarget + Vector3.ClampMagnitude(smoothTarget - oldTarget, num3);
					val = val3 - oldTarget;
					float num4 = Mathf.SmoothStep(0.05f, 2f, ((Vector3)(ref val)).get_magnitude() / num3);
					smoothTarget = Vector3.MoveTowards(val3, oldTarget, num2 * num4);
				}
				deltaTime = 0f;
				return smoothTarget;
			}
		}
		deltaTime = 0f;
		val = (smoothTarget = (oldTarget = target));
		return target;
	}

	public void PostSimulate()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		if (((Behaviour)this).get_enabled() && !NetGame.isClient && !ReplayRecorder.isPlaying)
		{
			fixedupdateSmooth = SmoothCamera(ragdoll.partHead.transform.get_position(), Time.get_fixedDeltaTime());
		}
	}

	public void LateUpdate()
	{
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0226: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0233: Unknown result type (might be due to invalid IL or missing references)
		//IL_0240: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0274: Unknown result type (might be due to invalid IL or missing references)
		//IL_0276: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0285: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		bool flag = !NetGame.isClient && !ReplayRecorder.isPlaying;
		Vector3 targetOffset;
		float yaw;
		float pitch;
		float minDist;
		float fov;
		float camDist;
		float num;
		switch (mode)
		{
		case CameraMode.Far:
			CalculateFarCam(out targetOffset, out yaw, out pitch, out camDist, out minDist, out fov, out num);
			break;
		case CameraMode.Close:
			CalculateCloseCam(out targetOffset, out yaw, out pitch, out camDist, out minDist, out fov, out num);
			break;
		case CameraMode.FirstPerson:
			CalculateFirstPersonCam(out targetOffset, out yaw, out pitch, out camDist, out minDist, out fov, out num);
			break;
		default:
			CalculateCloseCam(out targetOffset, out yaw, out pitch, out camDist, out minDist, out fov, out num);
			break;
		}
		if (fovAdjust != 0f)
		{
			camDist *= Mathf.Tan((float)Math.PI / 180f * fov / 2f) / Mathf.Tan((float)Math.PI / 180f * (fov + fovAdjust) / 2f);
			fov += fovAdjust;
		}
		camDist /= MenuCameraEffects.instance.cameraZoom;
		camDist = Mathf.Max(camDist, minDist);
		if (MenuCameraEffects.instance.creditsAdjust > 0f)
		{
			pitch = Mathf.Lerp(pitch, 90f, MenuCameraEffects.instance.creditsAdjust * 0.7f);
			camDist += MenuCameraEffects.instance.creditsAdjust * 20f;
			fov = Mathf.Lerp(fov, 40f, MenuCameraEffects.instance.creditsAdjust);
		}
		Quaternion val = Quaternion.Euler(pitch, yaw, 0f);
		Vector3 val2 = val * Vector3.get_forward();
		Vector3 val3 = (flag ? fixedupdateSmooth : SmoothCamera(ragdoll.partHead.transform.get_position(), Time.get_unscaledDeltaTime())) + targetOffset;
		float num2 = num;
		Vector3 position = ((Component)gameCam).get_transform().get_position();
		num = num2 * Mathf.Clamp(((Vector3)(ref position)).get_magnitude() / 500f, 1f, 2f);
		gameCam.set_nearClipPlane(num);
		gameCam.set_fieldOfView(fov);
		float num3 = (ignoreWalls ? 10000f : CompensateForWallsNearPlane(val3, val, farRange * 1.2f, minDist));
		offset = SpringArm(offset, camDist, num3, Time.get_unscaledDeltaTime());
		RaycastHit val4 = default(RaycastHit);
		if (num3 < offset && !Physics.SphereCast(val3 - val2 * offset, num * 2f, val2, ref val4, offset - num3, LayerMask.op_Implicit(wallLayers), (QueryTriggerInteraction)1))
		{
			offset = num3;
			offsetSpeed = 0f;
		}
		ApplyCamera(val3, val3 - val2 * offset, val, fov);
	}

	private float SpringArm(float current, float target, float limit, float deltaTime)
	{
		int num = Mathf.RoundToInt(deltaTime / (Time.get_fixedDeltaTime() / 10f));
		if (num < 1)
		{
			num = 1;
		}
		if (deltaTime == 0f || num > 1000)
		{
			return target;
		}
		float stepTime = deltaTime / (float)num;
		if (limit < target)
		{
			target = limit;
			if (target < current)
			{
				offsetSpeed = 0f;
				IntegrateDirect(target, ref current, stepTime, num, 5f, 10f);
				return current;
			}
		}
		if (target < current)
		{
			IntegrateSpring(target, ref current, ref offsetSpeed, stepTime, num, 100f, 10f, 500f);
		}
		else
		{
			IntegrateSpring(target, ref current, ref offsetSpeed, stepTime, num, 2f, 1f, 6f);
		}
		return current;
	}

	public static void SetSmoothing(float v)
	{
		smoothingAmount = v;
	}

	public static void SetFov(float v)
	{
		fovAdjust = (v * 20f - 5f) * 2f;
	}

	private void IntegrateDirect(float target, ref float pos, float stepTime, int steps, float minSpeed, float spring)
	{
		for (int i = 0; i < steps; i++)
		{
			pos = Mathf.MoveTowards(pos, target, (minSpeed + Mathf.Abs(spring * (target - pos))) * stepTime);
		}
	}

	private void IntegrateSpring(float target, ref float pos, ref float speed, float stepTime, int steps, float spring, float damper, float maxForce)
	{
		for (int i = 0; i < steps; i++)
		{
			if (speed * (target - pos) <= 0f)
			{
				speed = 0f;
			}
			speed += Mathf.Clamp(spring * (target - pos), 0f - maxForce, maxForce) * stepTime;
			speed = Mathf.MoveTowards(speed, 0f, Mathf.Abs(speed * damper * stepTime));
			pos = Mathf.MoveTowards(pos, target, Mathf.Abs(speed * stepTime));
		}
	}

	private void CalculateFarCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
	{
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		fov = 70f;
		nearClip = this.nearClip;
		yaw = human.controls.cameraYawAngle;
		cameraPitchAngle = human.controls.cameraPitchAngle;
		float num = 0.5f + 0.5f * Mathf.InverseLerp(0f, 80f, Mathf.Abs(cameraPitchAngle));
		if (human.controls.holding)
		{
			holdPhase = Mathf.MoveTowards(holdPhase, 1f, (human.state == HumanState.Climb) ? 0f : (Time.get_fixedDeltaTime() * Mathf.InverseLerp(0.5f, 0f, human.controls.walkSpeed)));
		}
		else
		{
			holdPhase = Mathf.MoveTowards(holdPhase, 0f, Time.get_fixedDeltaTime() / 0.5f);
		}
		num *= Mathf.Lerp(Mathf.InverseLerp(0.5f, 0f, human.controls.walkSpeed), 1f, holdPhase);
		standPhase = Mathf.MoveTowards(standPhase, num, Time.get_fixedDeltaTime() / 1f);
		pitchRange = Mathf.Lerp(30f, 60f, standPhase);
		int num2 = -50;
		int num3 = 80;
		float num4 = cameraPitchAngle / 80f * 60f + 10f;
		float num5 = cameraPitchAngle / 80f * 20f + 10f;
		pitch = Mathf.Lerp(num5, num4, standPhase);
		Quaternion val = Quaternion.Euler(pitch, yaw, 0f);
		targetOffset = val * farTargetOffset;
		if (human.controls.leftGrab || human.controls.rightGrab)
		{
			camDist = Options.LogMap(pitch, num2, 0f, num3, farRangeAction * 0.6f, farRangeAction, (farRange + farRangeAction) / 2f);
		}
		else
		{
			camDist = Options.LogMap(pitch, num2, 0f, num3, farRangeAction * 0.6f, farRangeAction, farRange);
		}
		fov = Mathf.Lerp(70f, 80f, Mathf.InverseLerp(0f, (float)num2, pitch));
		minDist = 0.025f;
	}

	private void CalculateCloseCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		fov = 70f;
		nearClip = this.nearClip;
		yaw = human.controls.cameraYawAngle;
		pitch = human.controls.cameraPitchAngle + 10f;
		if (pitch < 0f)
		{
			pitch *= 0.8f;
		}
		Quaternion val = Quaternion.Euler(pitch, yaw, 0f);
		targetOffset = val * closeTargetOffset;
		Vector3 val2 = Quaternion.Euler(human.controls.cameraPitchAngle, human.controls.cameraYawAngle, 0f) * Vector3.get_forward();
		camDist = Mathf.Lerp(closeRange, closeRangeLookUp, val2.y);
		minDist = 0.025f;
	}

	private void CalculateFirstPersonCam(out Vector3 targetOffset, out float yaw, out float pitch, out float camDist, out float minDist, out float fov, out float nearClip)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		fov = 70f;
		nearClip = this.nearClip;
		yaw = human.controls.cameraYawAngle;
		pitch = human.controls.cameraPitchAngle + 10f;
		if (pitch < 0f)
		{
			pitch *= 0.8f;
		}
		Quaternion val = Quaternion.Euler(pitch, yaw, 0f);
		targetOffset = val * fpsTargetOffset;
		camDist = 0f;
		minDist = 0f;
	}

	private float CompensateForWallsNearPlane(Vector3 targetPos, Quaternion lookRot, float desiredDist, float minDist)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = lookRot * Vector3.get_forward();
		float nearClipPlane = gameCam.get_nearClipPlane();
		((Component)this).get_transform().get_position();
		((Component)this).get_transform().get_rotation();
		((Component)this).get_transform().set_rotation(lookRot);
		((Component)this).get_transform().set_position(targetPos - val * (gameCam.get_nearClipPlane() + minDist));
		rayStarts[0] = gameCam.ViewportToWorldPoint(new Vector3(0f, 0f, nearClipPlane));
		rayStarts[1] = gameCam.ViewportToWorldPoint(new Vector3(0f, 1f, nearClipPlane));
		rayStarts[2] = gameCam.ViewportToWorldPoint(new Vector3(1f, 0f, nearClipPlane));
		rayStarts[3] = gameCam.ViewportToWorldPoint(new Vector3(1f, 1f, nearClipPlane));
		float num = desiredDist - nearClipPlane;
		RaycastHit val2 = default(RaycastHit);
		for (int i = 0; i < rayStarts.Length; i++)
		{
			if (Physics.Raycast(new Ray(rayStarts[i], -val), ref val2, num, LayerMask.op_Implicit(wallLayers)) && ((RaycastHit)(ref val2)).get_distance() < num)
			{
				num = ((RaycastHit)(ref val2)).get_distance();
			}
		}
		num += nearClipPlane + minDist;
		if (num < minDist * 2f)
		{
			num = minDist * 2f;
		}
		return num;
	}

	public void TransitionFromCurrent(float duration)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (duration == 0f)
		{
			throw new ArgumentException("duration can't be 0", "duration");
		}
		cameraTransitionPhase = 0f;
		cameraTransitionSpeed = 1f / duration;
		startFov = gameCam.get_fieldOfView();
		startOffset = ((Component)this).get_transform().get_position() - ((Component)human).get_transform().get_position();
		startRotation = ((Component)this).get_transform().get_rotation();
		startHumanCameraPos = gameCam.WorldToViewportPoint(((Component)human).get_transform().get_position());
	}

	public void TransitionFrom(GameCamera gameCamera, float focusDist, float duration)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		if (duration == 0f)
		{
			throw new ArgumentException("duration can't be 0", "duration");
		}
		cameraTransitionPhase = 0f;
		cameraTransitionSpeed = 1f / duration;
		startFov = gameCam.get_fieldOfView();
		startOffset = ((Component)gameCamera).get_transform().get_position() - ((Component)human).get_transform().get_position();
		startRotation = ((Component)gameCamera).get_transform().get_rotation();
		startHumanCameraPos = gameCamera.gameCam.WorldToViewportPoint(((Component)human).get_transform().get_position());
	}

	public void ApplyCamera(Vector3 target, Vector3 position, Quaternion rotation, float fov)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		cameraTransitionPhase += cameraTransitionSpeed * Time.get_deltaTime();
		if (cameraTransitionPhase >= 1f)
		{
			((Component)this).get_transform().set_position(position);
			((Component)this).get_transform().set_rotation(rotation);
			gameCam.set_fieldOfView(fov);
		}
		else
		{
			Ease.easeInOutSine(0f, 1f, Mathf.Clamp01(cameraTransitionPhase));
			((Component)this).get_transform().set_rotation(rotation);
			((Component)this).get_transform().set_position(position);
			gameCam.set_fieldOfView(fov);
			Vector3 val = gameCam.WorldToViewportPoint(((Component)human).get_transform().get_position());
			Vector3 val2 = Vector3.Lerp(startHumanCameraPos, val, cameraTransitionPhase);
			((Component)this).get_transform().set_rotation(Quaternion.Lerp(startRotation, rotation, cameraTransitionPhase));
			gameCam.set_fieldOfView(Mathf.Lerp(startFov, fov, cameraTransitionPhase));
			Vector3 val3 = gameCam.ViewportToWorldPoint(val2);
			Transform transform = ((Component)this).get_transform();
			transform.set_position(transform.get_position() + (((Component)human).get_transform().get_position() - val3));
		}
		Vector3 val4;
		if (!human.controls.leftGrab && !human.controls.rightGrab)
		{
			val4 = target - position;
			Mathf.Clamp(((Vector3)(ref val4)).get_magnitude(), 6f, 6f);
		}
		else
		{
			val4 = target - position;
			Mathf.Clamp(((Vector3)(ref val4)).get_magnitude(), 4f, 5f);
		}
	}

	public CameraController3()
		: this()
	{
	}//IL_005a: Unknown result type (might be due to invalid IL or missing references)
	//IL_005f: Unknown result type (might be due to invalid IL or missing references)

}

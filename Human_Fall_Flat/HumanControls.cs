using System.Collections.Generic;
using CurveGame;
using UnityEngine;

public class HumanControls : MonoBehaviour
{
	public bool allowMouse;

	public SteelSeriesHFF steelSeriesHFF;

	private Human humanScript;

	private Game gameScript;

	private const InputAction leftStickX = InputAction.HorizontalAxis0;

	private const InputAction leftStickY = InputAction.VerticalAxis0;

	public static bool freezeMouse;

	private Vector2 keyLookCache;

	public float leftExtend;

	public float rightExtend;

	private float leftExtendCache;

	private float rightExtendCache;

	public bool leftGrab;

	public bool rightGrab;

	public bool unconscious;

	public bool holding;

	public bool jump;

	public bool shootingFirework;

	public VerticalLookMode verticalLookMode;

	public MouseLookMode mouseLookMode;

	public bool mouseControl = true;

	public float cameraPitchAngle;

	public float cameraYawAngle;

	public float targetPitchAngle;

	public float targetYawAngle;

	public Vector3 walkLocalDirection;

	public Vector3 walkDirection;

	public float unsmoothedWalkSpeed;

	public float walkSpeed;

	private List<float> mouseInputX = new List<float>();

	private List<float> mouseInputY = new List<float>();

	private Vector3 stickDirection;

	private Vector3 oldStickDirection;

	private float previousLeftExtend;

	private float previousRightExtend;

	private float shootCooldown;

	public Vector2 calc_joyLook
	{
		get
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			float axis = Input.GetAxis("Joystick Look Horizontal");
			float axis2 = Input.GetAxis("Joystick Look Vertical");
			return new Vector2(axis, axis2);
		}
	}

	public Vector2 calc_keyLook
	{
		get
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			if (allowMouse)
			{
				if (!freezeMouse)
				{
					float axis = Input.GetAxis("Mouse Look Horizontal");
					float axis2 = Input.GetAxis("Mouse Look Vertical");
					keyLookCache = new Vector2(axis, axis2);
				}
				return keyLookCache;
			}
			return Vector2.get_zero();
		}
	}

	public Vector3 calc_joyWalk
	{
		get
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			float axis = Input.GetAxis("Joystick Walk Horizontal");
			float axis2 = Input.GetAxis("Joystick Walk Vertical");
			return new Vector3(axis, 0f, axis2);
		}
	}

	public Vector3 calc_keyWalk
	{
		get
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			if (allowMouse)
			{
				float axis = Input.GetAxis("Keyboard Walk Horizontal");
				float axis2 = Input.GetAxis("Keyboard Walk Vertical");
				return new Vector3(axis, 0f, axis2);
			}
			return Vector3.get_zero();
		}
	}

	public float lookYnormalized
	{
		get
		{
			float axis = Input.GetAxis("Joystick Look Vertical");
			return Mathf.Sign(axis) * Mathf.Pow(Mathf.Abs(axis), 1f);
		}
	}

	public float vScale => 1f;

	private void OnEnable()
	{
		SetAny();
		verticalLookMode = (VerticalLookMode)Options.controllerLookMode;
		mouseLookMode = (MouseLookMode)Options.mouseLookMode;
		steelSeriesHFF = ((Component)this).GetComponent<SteelSeriesHFF>();
		humanScript = ((Component)((Component)this).get_transform().Find("Ball")).GetComponent<Human>();
		gameScript = GameObject.Find("Game(Clone)").GetComponent<Game>();
	}

	private float GetLeftExtend()
	{
		return Input.GetAxis("Left Grab");
	}

	private float GetRightExtend()
	{
		return Input.GetAxis("Right Grab");
	}

	private bool GetUnconscious()
	{
		return Input.GetAxis("Unconscious") > 0.01f;
	}

	public bool ControllerJumpPressed()
	{
		return false;
	}

	public bool ControllerFireworksPressed()
	{
		return false;
	}

	private bool GetJump()
	{
		return Input.GetAxis("Jump") > 0.01f;
	}

	private bool GetFireworks()
	{
		return Input.GetAxis("Shoot Fireworks") > 0.01f;
	}

	public void ReadInput(out float walkForward, out float walkRight, out float cameraPitch, out float cameraYaw, out float leftExtend, out float rightExtend, out bool jump, out bool playDead, out bool shooting)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dc: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = calc_joyLook;
		Vector2 val2 = calc_keyLook;
		Vector3 val3 = calc_joyWalk;
		Vector3 val4 = calc_keyWalk;
		if (((Vector2)(ref val)).get_sqrMagnitude() > ((Vector2)(ref val2)).get_sqrMagnitude())
		{
			mouseControl = false;
		}
		if (((Vector2)(ref val2)).get_sqrMagnitude() > ((Vector2)(ref val)).get_sqrMagnitude())
		{
			mouseControl = true;
		}
		if (((Vector3)(ref val3)).get_sqrMagnitude() > ((Vector3)(ref val4)).get_sqrMagnitude())
		{
			mouseControl = false;
		}
		if (((Vector3)(ref val4)).get_sqrMagnitude() > ((Vector3)(ref val3)).get_sqrMagnitude())
		{
			mouseControl = true;
		}
		cameraPitch = cameraPitchAngle;
		cameraYaw = cameraYawAngle;
		if (mouseControl)
		{
			cameraYaw += Smoothing.SmoothValue(mouseInputX, val2.x);
			cameraPitch -= Smoothing.SmoothValue(mouseInputY, val2.y);
			cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
			if (mouseLookMode == MouseLookMode.SpringBackNoGrab && !leftGrab && !rightGrab)
			{
				int num = 0;
				float num2 = 0.25f;
				cameraPitch = Mathf.Lerp(cameraPitch, (float)num, num2 * 5f * Time.get_fixedDeltaTime());
				cameraPitch = Mathf.MoveTowards(cameraPitch, (float)num, num2 * 30f * Time.get_fixedDeltaTime());
			}
		}
		else
		{
			cameraYaw += val.x * Time.get_deltaTime() * 120f;
			if (verticalLookMode == VerticalLookMode.Relative)
			{
				if (Options.controllerInvert > 0)
				{
					val.y = 0f - val.y;
				}
				cameraPitch -= val.y * Time.get_deltaTime() * 120f * 2f;
				cameraPitch = Mathf.Clamp(cameraPitch, -80f, 80f);
			}
			else
			{
				float num3 = -80f * lookYnormalized;
				bool num4 = num3 * cameraPitch < 0f || Mathf.Abs(num3) > Mathf.Abs(cameraPitch);
				float num5 = ((leftGrab || rightGrab) ? Mathf.Abs(lookYnormalized) : 1f);
				float num6 = ((leftGrab || rightGrab) ? 0.0125f : 0.25f);
				float num7 = (num4 ? num5 : num6);
				cameraPitch = Mathf.Lerp(cameraPitch, num3, num7 * 5f * Time.get_fixedDeltaTime() * vScale);
				cameraPitch = Mathf.MoveTowards(cameraPitch, num3, num7 * 30f * Time.get_fixedDeltaTime() * vScale);
			}
		}
		if (Input.GetKey((KeyCode)260))
		{
			cameraYaw -= 1f;
		}
		if (Input.GetKey((KeyCode)262))
		{
			cameraYaw += 1f;
		}
		if (Input.GetKey((KeyCode)258))
		{
			cameraPitch += 1f;
		}
		if (Input.GetKey((KeyCode)264))
		{
			cameraPitch -= 1f;
		}
		Vector3 val5 = ((((Vector3)(ref val3)).get_sqrMagnitude() > ((Vector3)(ref val4)).get_sqrMagnitude()) ? val3 : val4);
		walkForward = val5.z;
		walkRight = val5.x;
		if (MenuSystem.keyboardState == KeyboardState.None)
		{
			leftExtend = GetLeftExtend();
			rightExtend = GetRightExtend();
			jump = GetJump();
			playDead = GetUnconscious();
			previousLeftExtend = leftExtend;
			previousRightExtend = rightExtend;
			shooting = GetFireworks();
		}
		else
		{
			leftExtend = previousLeftExtend;
			rightExtend = previousRightExtend;
			jump = false;
			playDead = false;
			shooting = false;
		}
	}

	public void HandleInput(float walkForward, float walkRight, float cameraPitch, float cameraYaw, float leftExtend, float rightExtend, bool jump, bool playDead, bool holding, bool shooting)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0197: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		walkLocalDirection = new Vector3(walkRight, 0f, walkForward);
		cameraPitchAngle = cameraPitch;
		cameraYawAngle = cameraYaw;
		this.leftExtend = leftExtend;
		this.rightExtend = rightExtend;
		leftGrab = leftExtend > 0f;
		rightGrab = rightExtend > 0f;
		if ((Object)(object)steelSeriesHFF != (Object)null)
		{
			steelSeriesHFF.SteelSeriesEvent_LeftArm(leftExtend > 0f);
			steelSeriesHFF.SteelSeriesEvent_RightArm(rightExtend > 0f);
			steelSeriesHFF.SteelSeriesEvent_Respawning(humanScript.spawning);
			if (gameScript.passedCheckpoint_ForSteelSeriesEvent)
			{
				gameScript.passedCheckpoint_ForSteelSeriesEvent = false;
				steelSeriesHFF.SteelSeriesEvent_CheckpointHit();
			}
		}
		this.jump = jump;
		unconscious = playDead;
		this.holding = holding;
		shootingFirework = shooting;
		targetPitchAngle = Mathf.MoveTowards(targetPitchAngle, cameraPitchAngle, 180f * Time.get_fixedDeltaTime() / 0.1f);
		targetYawAngle = cameraYawAngle;
		Vector3 val = Quaternion.Euler(0f, cameraYawAngle, 0f) * walkLocalDirection;
		unsmoothedWalkSpeed = ((Vector3)(ref val)).get_magnitude();
		((Vector3)(ref val))._002Ector(FilterAxisAcceleration(oldStickDirection.x, val.x), 0f, FilterAxisAcceleration(oldStickDirection.z, val.z));
		walkSpeed = ((Vector3)(ref val)).get_magnitude();
		if (walkSpeed > 0f)
		{
			walkDirection = val;
		}
		oldStickDirection = val;
	}

	private float FilterAxisAcceleration(float currentValue, float desiredValue)
	{
		float num = Time.get_fixedDeltaTime() / 1f;
		float num2 = 0.2f;
		if (currentValue * desiredValue <= 0f)
		{
			currentValue = 0f;
		}
		if (Mathf.Abs(currentValue) > Mathf.Abs(desiredValue))
		{
			currentValue = desiredValue;
		}
		if (Mathf.Abs(currentValue) < num2)
		{
			num = Mathf.Max(num, num2 - Mathf.Abs(currentValue));
		}
		if (Mathf.Abs(currentValue) > 0.8f)
		{
			num /= 3f;
		}
		return Mathf.MoveTowards(currentValue, desiredValue, num);
	}

	public void SetController()
	{
		verticalLookMode = (VerticalLookMode)Options.controllerLookMode;
		mouseLookMode = (MouseLookMode)Options.mouseLookMode;
		allowMouse = false;
	}

	public void SetMouse()
	{
		verticalLookMode = (VerticalLookMode)Options.controllerLookMode;
		mouseLookMode = (MouseLookMode)Options.mouseLookMode;
		allowMouse = true;
	}

	public void SetAny()
	{
		verticalLookMode = (VerticalLookMode)Options.controllerLookMode;
		mouseLookMode = (MouseLookMode)Options.mouseLookMode;
		allowMouse = true;
	}

	public HumanControls()
		: this()
	{
	}
}

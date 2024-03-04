using HumanAPI;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomizationCameraController : MonoBehaviour
{
	public Transform cameraArm;

	public Transform cameraHolder;

	public Transform bothCharactersCamera;

	public Transform fullCamera;

	public Transform fullCameraFrontal;

	public Transform fullModelCamera;

	public Transform headCamera;

	public Transform upperCamera;

	public Transform lowerCamera;

	private float transitionDuration = 0.5f;

	private float transitionPhase = 1f;

	private Vector3 targetFrom;

	private Vector3 targetTo;

	private Vector3 offsetFrom;

	private Vector3 offsetTo;

	private Quaternion rotateFrom;

	private Quaternion rotateTo;

	private int currentMode;

	public bool uiLock;

	public bool alt;

	public bool ctrl;

	public bool lmb;

	public bool rmb;

	public bool mmb;

	public bool isInGesture;

	public bool navigationEnabled = true;

	public void FocusBoth()
	{
		FocusTransform(bothCharactersCamera);
	}

	public void FocusCharacterFrontal()
	{
		FocusTransform(fullCameraFrontal);
	}

	public void FocusCharacter()
	{
		FocusTransform(fullCamera);
	}

	public void FocusCharacterModel()
	{
		FocusTransform(fullModelCamera);
	}

	public void FocusHead()
	{
		FocusTransform(headCamera);
	}

	public void FocusUpperBody()
	{
		FocusTransform(upperCamera);
	}

	public void FocusLowerBody()
	{
		FocusTransform(lowerCamera);
	}

	public void FocusPart(WorkshopItemType part)
	{
		switch (part)
		{
		case WorkshopItemType.ModelFull:
			FocusCharacterModel();
			break;
		case WorkshopItemType.ModelHead:
			FocusHead();
			break;
		case WorkshopItemType.ModelUpperBody:
			FocusUpperBody();
			break;
		case WorkshopItemType.ModelLowerBody:
			FocusLowerBody();
			break;
		}
	}

	private void FocusTransform(Transform target)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		Transform child = target.GetChild(0);
		offsetFrom = cameraHolder.get_localPosition();
		offsetTo = child.get_localPosition();
		targetFrom = cameraArm.get_position();
		targetTo = target.get_position();
		rotateFrom = cameraArm.get_rotation();
		rotateTo = target.get_rotation();
		transitionPhase = 0f;
	}

	private void Update()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0179: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e0: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val;
		if (transitionPhase < 1f)
		{
			transitionPhase += Time.get_deltaTime() / transitionDuration;
			float num = Ease.easeInOutQuad(0f, 1f, transitionPhase);
			cameraArm.set_rotation(val = Quaternion.Lerp(rotateFrom, rotateTo, num));
			cameraArm.set_position(Vector3.Lerp(targetFrom, targetTo, num));
			cameraHolder.set_localPosition(Vector3.Lerp(offsetFrom, offsetTo, num));
			return;
		}
		if ((Object)(object)EventSystem.get_current() != (Object)null)
		{
			if (Input.GetMouseButtonDown(0) && EventSystem.get_current().IsPointerOverGameObject() && (!ColorWheel.isInside || !ColorWheel.isTransparent))
			{
				uiLock = true;
			}
			if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2))
			{
				uiLock = EventSystem.get_current().IsPointerOverGameObject() && (!ColorWheel.isInside || !ColorWheel.isTransparent);
			}
		}
		if (uiLock && Input.GetMouseButtonUp(0))
		{
			uiLock = false;
		}
		val = cameraArm.get_rotation();
		float num2 = ((Quaternion)(ref val)).get_eulerAngles().x;
		val = cameraArm.get_rotation();
		float num3 = ((Quaternion)(ref val)).get_eulerAngles().y;
		float num4 = ((Component)cameraHolder).get_transform().get_localPosition().x;
		float num5 = ((Component)cameraHolder).get_transform().get_localPosition().y;
		float num6 = 0f - ((Component)cameraHolder).get_transform().get_localPosition().z;
		if (num2 > 180f)
		{
			num2 -= 360f;
		}
		lmb = Input.GetMouseButton(0) && !uiLock;
		rmb = Input.GetMouseButton(1);
		mmb = Input.GetMouseButton(2);
		alt = Input.GetKey((KeyCode)308) || Input.GetKey((KeyCode)307);
		ctrl = Input.GetKey((KeyCode)306) || Input.GetKey((KeyCode)306);
		isInGesture = lmb || rmb || mmb;
		if (!uiLock && (navigationEnabled || alt))
		{
			float y = Input.get_mouseScrollDelta().y;
			num6 *= Mathf.Pow(1.1f, 0f - y);
			num6 = Mathf.Clamp(num6, 0.2f, 5f);
		}
		if (navigationEnabled || alt || mmb || rmb)
		{
			float axis = Input.GetAxis("mouse x");
			float axis2 = Input.GetAxis("mouse y");
			if (axis != 0f || axis2 != 0f)
			{
				if (rmb)
				{
					float num7 = 0.2f;
					num2 -= axis2 * num7;
					num2 = Mathf.Clamp(num2, -89f, 89f);
					num3 += axis * num7;
					if (num3 > 180f)
					{
						num3 -= 360f;
					}
					if (num3 < -180f)
					{
						num3 += 360f;
					}
				}
				if (mmb || ((navigationEnabled || alt) && lmb))
				{
					float num8 = 0.003f;
					num4 -= axis * num8 * num6;
					num5 -= axis2 * num8 * num6;
				}
				num6 = Mathf.Clamp(num6, 0.2f, 5f);
				float num9 = 1f + num6;
				num4 = Mathf.Clamp(num4, 0f - num9, num9);
				num5 = Mathf.Clamp(num5, 0f - (1.2f + num6 * 0.6f), 1f + num6 * 0.6f);
			}
		}
		cameraArm.set_rotation(Quaternion.Euler(num2, num3, 0f));
		((Component)cameraHolder).get_transform().set_localPosition(new Vector3(num4, num5, 0f - num6));
	}

	public CustomizationCameraController()
		: this()
	{
	}
}

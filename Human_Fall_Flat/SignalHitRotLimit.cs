using System.Collections;
using HumanAPI;
using UnityEngine;

public class SignalHitRotLimit : Node
{
	private bool checkSettings;

	public NodeOutput limitReached;

	public NodeInput beginChecking;

	[Tooltip("The amount of rotation to check for")]
	public float rotationTarget;

	[Tooltip("How accuarate the limit should be")]
	public float degreeOfAccuracy = 2f;

	[Tooltip("Check the X Rotation")]
	public bool xRotation;

	[Tooltip("Check the Y Rotation")]
	public bool yRotation;

	[Tooltip("Check the Z Rotation")]
	public bool zRotation;

	[Tooltip("Whether or we need an input to start checking the level of rotation")]
	public bool alwaysCheck;

	private float currentRotation;

	private float repeatedRotationTarget;

	private float currentTarget;

	[Tooltip("Do we need to continually check the roation position")]
	public bool onceOnly = true;

	[Tooltip("Whether to repeat the check")]
	public bool repeatedCheck;

	[Tooltip("Whether or not to see the output from this component")]
	public bool showDebug;

	private bool waiting;

	private void Start()
	{
		CalcEulerSetting();
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Initial current Rotation = " + currentRotation));
			Debug.Log((object)(((Object)this).get_name() + " Initial Target Rotation = " + rotationTarget));
		}
		repeatedRotationTarget = rotationTarget;
		if (!repeatedCheck)
		{
			return;
		}
		if (currentTarget >= 0f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Change Rotation A "));
			}
			rotationTarget = currentRotation + repeatedRotationTarget;
		}
		else if (rotationTarget < 0f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Change Rotatation B "));
			}
			rotationTarget = currentTarget - repeatedRotationTarget;
		}
	}

	private void Update()
	{
		if (!checkSettings && !alwaysCheck)
		{
			return;
		}
		CalcEulerSetting();
		if (repeatedCheck)
		{
			currentTarget = currentRotation - rotationTarget;
			if (currentTarget < 0f)
			{
				currentTarget *= -1f;
			}
		}
		if (currentRotation >= rotationTarget - degreeOfAccuracy && currentRotation <= rotationTarget + degreeOfAccuracy)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Sending the signal now "));
			}
			FireoffSignal();
			if (repeatedCheck)
			{
				rotationTarget = currentRotation + repeatedRotationTarget;
			}
			if (rotationTarget > 360f)
			{
				rotationTarget -= 360f;
			}
		}
	}

	public override void Process()
	{
		if (beginChecking.value > 0.5f || alwaysCheck)
		{
			checkSettings = true;
		}
		else
		{
			checkSettings = false;
		}
	}

	private void FireoffSignal()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Limit Reached "));
		}
		limitReached.SetValue(1f);
		waiting = true;
		((MonoBehaviour)this).StartCoroutine(ResetLimiReached());
	}

	private void CalcEulerSetting()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		Quaternion localRotation;
		if (xRotation)
		{
			localRotation = ((Component)this).get_transform().get_localRotation();
			currentRotation = ((Quaternion)(ref localRotation)).get_eulerAngles().x;
		}
		if (yRotation)
		{
			localRotation = ((Component)this).get_transform().get_localRotation();
			currentRotation = ((Quaternion)(ref localRotation)).get_eulerAngles().y;
		}
		if (zRotation)
		{
			localRotation = ((Component)this).get_transform().get_localRotation();
			currentRotation = ((Quaternion)(ref localRotation)).get_eulerAngles().z;
		}
		if (currentRotation < 0f)
		{
			currentRotation *= -1f;
		}
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Current Rotation = " + currentRotation));
			Debug.Log((object)(((Object)this).get_name() + " Current Rotation Target = " + rotationTarget));
		}
	}

	public IEnumerator ResetLimiReached()
	{
		yield return (object)new WaitForSeconds(0.1f);
		limitReached.SetValue(0f);
		waiting = false;
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Just changed the value back "));
		}
	}
}

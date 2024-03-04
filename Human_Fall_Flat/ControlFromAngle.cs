using System;
using HumanAPI;
using UnityEngine;

public class ControlFromAngle : Node, IReset
{
	public enum Axis
	{
		X,
		Y,
		Z
	}

	public NodeOutput output;

	public GameObject target;

	[Tooltip("A Flag to say whether to use the information for location coming from a parent gameobject")]
	public bool useParentLocation;

	private Vector3 newPosition;

	public Axis axis;

	public float rangeRotations = 1f;

	public float currentAngle;

	public float currentValue;

	private float originalValue;

	private float originalAngle;

	private Rigidbody body;

	public bool allowNegative;

	public bool clampFullRotation = true;

	public bool CalculateAngleCorrectly;

	public bool ResetToZeroOnRestart;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private bool justReset;

	protected override void OnEnable()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Enabled "));
		}
		base.OnEnable();
		body = ((Component)this).GetComponent<Rigidbody>();
		originalAngle = (currentAngle = FromEuler(((Component)this).get_transform()));
		originalValue = currentValue;
		if ((Object)(object)target != (Object)null && target.GetComponent<IControllable>() == null)
		{
			Debug.LogErrorFormat((Object)(object)this, "ControlFromAngle incorrectly set up - target has no IControllable on it", new object[0]);
		}
	}

	private void FixedUpdate()
	{
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		float num = FromEuler(((Component)this).get_transform());
		if (justReset)
		{
			output.SetValue(0f);
			currentAngle = num;
			justReset = false;
			return;
		}
		float num2 = num - currentAngle;
		currentAngle = num;
		for (; num2 < -180f; num2 += 360f)
		{
		}
		while (num2 > 180f)
		{
			num2 -= 360f;
		}
		if (num2 == 0f)
		{
			return;
		}
		currentValue += num2 / 360f / rangeRotations;
		float num3 = (allowNegative ? (-1f) : 0f);
		Vector3 angularVelocity;
		if (currentValue < num3)
		{
			if (clampFullRotation)
			{
				angularVelocity = body.get_angularVelocity();
				if (((Vector3)(ref angularVelocity)).get_magnitude() > 0.01f)
				{
					body.set_angularVelocity(Vector3.get_zero());
					body.MoveRotation(((Component)this).get_transform().get_parent().get_rotation() * Quaternion.Euler(ToEuler((0f - currentValue) * 360f * rangeRotations)) * ((Component)this).get_transform().get_localRotation());
				}
				currentValue = num3;
			}
			else
			{
				currentValue = 1f - (num3 - currentValue);
			}
		}
		if (currentValue > 1f)
		{
			if (clampFullRotation)
			{
				angularVelocity = body.get_angularVelocity();
				if (((Vector3)(ref angularVelocity)).get_magnitude() > 0.01f)
				{
					body.set_angularVelocity(Vector3.get_zero());
					body.MoveRotation(((Component)this).get_transform().get_parent().get_rotation() * Quaternion.Euler(ToEuler((1f - currentValue) * 360f * rangeRotations)) * ((Component)this).get_transform().get_localRotation());
				}
				currentValue = 1f;
			}
			else
			{
				currentValue = (allowNegative ? (-1f) : 0f) + (currentValue - 1f);
			}
		}
		if ((Object)(object)target != (Object)null)
		{
			target.GetComponent<IControllable>()?.SetControlValue(currentValue);
		}
		output.SetValue(currentValue);
	}

	private float FromEuler(Transform transform)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		if (CalculateAngleCorrectly)
		{
			switch (axis)
			{
			case Axis.X:
				return Mathf.Atan2(transform.get_forward().y, transform.get_forward().z) * 57.29578f;
			case Axis.Y:
			case Axis.Z:
				throw new InvalidOperationException();
			}
		}
		else
		{
			Quaternion localRotation = transform.get_localRotation();
			Vector3 eulerAngles = ((Quaternion)(ref localRotation)).get_eulerAngles();
			switch (axis)
			{
			case Axis.X:
				return eulerAngles.x;
			case Axis.Y:
				return eulerAngles.y;
			case Axis.Z:
				return eulerAngles.z;
			}
		}
		throw new InvalidOperationException();
	}

	private Vector3 ToEuler(float angle)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " ToEuler "));
		}
		return (Vector3)(axis switch
		{
			Axis.X => new Vector3(angle, 0f, 0f), 
			Axis.Y => new Vector3(0f, angle), 
			Axis.Z => new Vector3(0f, 0f, angle), 
			_ => throw new InvalidOperationException(), 
		});
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " ResetState "));
		}
		currentAngle = originalAngle;
		currentValue = originalValue;
		if (ResetToZeroOnRestart && checkpoint == 0)
		{
			justReset = true;
		}
	}
}

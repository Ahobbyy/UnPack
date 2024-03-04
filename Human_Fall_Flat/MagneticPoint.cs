using System;
using Multiplayer;
using UnityEngine;

public class MagneticPoint : MonoBehaviour
{
	public float magnetism;

	public float range = 1f;

	public Vector3 magneticPointOffset;

	[Range(0f, 360f)]
	public float angle = 360f;

	[NonSerialized]
	public MagneticBody magneticBody;

	[NonSerialized]
	public bool magnetActive;

	private void Start()
	{
	}

	private void FixedUpdate()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0132: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
		if (ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		if ((Object)(object)magneticBody == (Object)null)
		{
			Debug.LogError((object)("Magnetics need a parent MagneticBody to function: " + ((Object)((Component)this).get_gameObject()).get_name()), (Object)(object)this);
			return;
		}
		Vector3 val = ((Component)this).get_transform().TransformPoint(magneticPointOffset);
		magnetActive = false;
		if (magnetism == 0f || !magneticBody.magnetActive)
		{
			return;
		}
		foreach (MagneticPoint nearbyMagnetic in magneticBody.NearbyMagnetics)
		{
			if ((Object)(object)magneticBody == (Object)(object)nearbyMagnetic.magneticBody)
			{
				continue;
			}
			Vector3 val2 = ((Component)nearbyMagnetic).get_transform().TransformPoint(nearbyMagnetic.magneticPointOffset);
			Vector3 val3 = val - val2;
			if (((Vector3)(ref val3)).get_magnitude() > range)
			{
				continue;
			}
			Vector3 normalized = ((Vector3)(ref val3)).get_normalized();
			float value = Math.Abs(magnetism) + Math.Abs(nearbyMagnetic.magnetism);
			val3 = normalized * Math.Abs(value) * (1f - ((Vector3)(ref val3)).get_magnitude() / range);
			if (!((double)angle < 360.0) || !(57.29578f * Mathf.Acos(Vector3.Dot(-normalized, ((Component)this).get_transform().get_forward())) > angle / 2f))
			{
				magnetActive = true;
				bool flag = false;
				if (Math.Sign(magnetism) == Math.Sign(nearbyMagnetic.magnetism))
				{
					flag = true;
				}
				if (flag)
				{
					nearbyMagnetic.magneticBody.Body.AddForceAtPosition(-val3, val2);
				}
				else if (!magneticBody.disableOnContact || !magneticBody.IsInContact(nearbyMagnetic.magneticBody.Body))
				{
					nearbyMagnetic.magneticBody.Body.AddForceAtPosition(val3, val2);
				}
			}
		}
	}

	public MagneticPoint()
		: this()
	{
	}
}

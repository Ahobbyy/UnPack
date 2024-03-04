using UnityEngine;

public class SignalBroadcastLookAngleOnGrab : SignalBase
{
	public GameObject grabTarget;

	public Transform referenceTransform;

	public Vector3 referenceAxis = Vector3.get_forward();

	public SteerMode steerMode;

	private void Awake()
	{
		if ((Object)(object)grabTarget == (Object)null)
		{
			grabTarget = ((Component)this).get_gameObject();
		}
	}

	public void FixedUpdate()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		Vector3 v = referenceTransform.TransformDirection(referenceAxis);
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			Vector3 v2 = Quaternion.Euler(0f, human.controls.targetYawAngle, 0f) * Vector3.get_forward();
			float num4 = Math2d.SignedAngle(v.To2D(), v2.To2D());
			int num5 = 0;
			num5 = ((-90f < num4 && num4 < 90f) ? 1 : (-1));
			if ((Object)(object)human.ragdoll.partLeftHand.sensor.grabObject == (Object)(object)grabTarget)
			{
				num2 += num4;
				num3 += (float)num5;
				num++;
			}
			if ((Object)(object)human.ragdoll.partRightHand.sensor.grabObject == (Object)(object)grabTarget)
			{
				num2 += num4;
				num3 += (float)num5;
				num++;
			}
		}
		if (steerMode == SteerMode.Steer)
		{
			if (num > 0)
			{
				num2 /= (float)num;
				if (num3 >= 0f)
				{
					SetValue(num2 / (float)num);
				}
				else if (num2 < -90f)
				{
					SetValue(-180f - num2);
				}
				else
				{
					SetValue(180f - num2);
				}
			}
		}
		else
		{
			if (num3 < 2f)
			{
				num3 /= 4f;
			}
			SetValue(num3);
		}
	}
}

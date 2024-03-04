using CurveExtended;
using UnityEngine;

public class CalculateAttenuation : MonoBehaviour
{
	public float falloffStart = 1f;

	public float falloffPower = 0.5f;

	public float lpStart = 2f;

	public float lpPower = 0.5f;

	public float spreadNear = 0.5f;

	public float spreadFar;

	public float spatialNear = 0.5f;

	public float spatialFar = 1f;

	public void Generate()
	{
		AudioSource[] components = ((Component)this).GetComponents<AudioSource>();
		foreach (AudioSource val in components)
		{
			val.set_rolloffMode((AudioRolloffMode)2);
			val.SetCustomCurve((AudioSourceCurveType)0, VolumeFalloff(falloffStart / val.get_maxDistance(), falloffPower));
			val.SetCustomCurve((AudioSourceCurveType)3, Spread(spreadNear, spreadFar, falloffStart / val.get_maxDistance(), falloffPower));
			val.SetCustomCurve((AudioSourceCurveType)1, Spread(spatialNear, spatialFar, falloffStart / val.get_maxDistance(), falloffPower));
		}
		AudioLowPassFilter component = ((Component)this).GetComponent<AudioLowPassFilter>();
		if ((Object)(object)component != (Object)null)
		{
			component.set_customCutoffCurve(LowPassFalloff(lpStart / components[0].get_maxDistance(), lpPower));
		}
	}

	public static AnimationCurve VolumeFalloffFromTo(float near, float far, float falloffStart, float falloff48db, float falloffFocus)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		AnimationCurve val = new AnimationCurve();
		val.AddKey(KeyframeUtil.GetNew(0f, near, TangentMode.Linear));
		if (falloff48db <= falloffStart)
		{
			val.AddKey(KeyframeUtil.GetNew(1f, near, TangentMode.Linear));
		}
		else if (falloffFocus == 1f)
		{
			float num = falloffStart / (falloffStart + falloff48db);
			float num2 = 1f;
			for (int i = 0; i < 16; i++)
			{
				float time = num + (1f - num) * (float)i / 16f;
				val.AddKey(KeyframeUtil.GetNew(time, Mathf.Lerp(far, near, num2), TangentMode.Linear));
				num2 /= Mathf.Sqrt(2f);
			}
			val.AddKey(KeyframeUtil.GetNew(1f, far, TangentMode.Linear));
		}
		else
		{
			float num3 = Mathf.Pow(2f, 1f - falloffFocus);
			float num4 = falloffStart / (falloffStart + falloff48db);
			float num5 = (1f - Mathf.Pow(num3, 8f) * num4) / (1f - Mathf.Pow(num3, 8f));
			float num6 = num4 - num5;
			float num7 = 1f;
			for (int j = 0; j < 16; j++)
			{
				val.AddKey(KeyframeUtil.GetNew(num5 + num6, Mathf.Lerp(far, near, num7), TangentMode.Linear));
				num6 *= Mathf.Sqrt(num3);
				num7 /= Mathf.Sqrt(2f);
			}
			val.AddKey(KeyframeUtil.GetNew(1f, far, TangentMode.Linear));
		}
		val.UpdateAllLinearTangents();
		return val;
	}

	public static AnimationCurve VolumeFalloff(float falloffPoint, float falloffPower)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		AnimationCurve val = new AnimationCurve();
		val.AddKey(KeyframeUtil.GetNew(0f, 1f, TangentMode.Linear));
		if (falloffPoint >= 1f || falloffPower == 1f)
		{
			val.AddKey(KeyframeUtil.GetNew(1f, 1f, TangentMode.Linear));
		}
		else
		{
			for (float num = 0f; num < 10f; num += 0.5f)
			{
				float num2 = falloffPoint * Mathf.Pow(2f, num);
				if (num2 > 1f)
				{
					break;
				}
				float value = Mathf.Pow(falloffPower, num) * Mathf.InverseLerp(1f, falloffPoint, num2);
				val.AddKey(KeyframeUtil.GetNew(num2, value, TangentMode.Linear));
			}
			val.AddKey(KeyframeUtil.GetNew(1f, 0f, TangentMode.Linear));
		}
		val.UpdateAllLinearTangents();
		return val;
	}

	public static AnimationCurve LowPassFalloff(float falloffPoint, float falloffPower)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		AnimationCurve val = new AnimationCurve();
		val.AddKey(KeyframeUtil.GetNew(0f, 1f, TangentMode.Linear));
		if (falloffPoint > 29.9f && falloffPower > 0.99f)
		{
			val.UpdateAllLinearTangents();
			return val;
		}
		if (falloffPoint >= 1f || falloffPower == 1f)
		{
			val.AddKey(KeyframeUtil.GetNew(1f, 1f, TangentMode.Linear));
		}
		else
		{
			_ = 1f / falloffPoint;
			for (float num = 0f; num < 10f; num += 0.5f)
			{
				float num2 = falloffPoint * Mathf.Pow(2f, num);
				if (num2 > 1f)
				{
					break;
				}
				float value = Mathf.Pow(falloffPower, num);
				val.AddKey(KeyframeUtil.GetNew(num2, value, TangentMode.Linear));
			}
		}
		val.UpdateAllLinearTangents();
		return val;
	}

	public static AnimationCurve Spread(float near, float far, float falloffPoint, float falloffPower)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		AnimationCurve val = new AnimationCurve();
		val.AddKey(KeyframeUtil.GetNew(0f, near, TangentMode.Linear));
		if (falloffPoint >= 1f || falloffPower == 1f)
		{
			val.AddKey(KeyframeUtil.GetNew(1f, near, TangentMode.Linear));
		}
		else
		{
			_ = 1f / falloffPoint;
			for (float num = 0f; num < 10f; num += 0.5f)
			{
				float num2 = falloffPoint * Mathf.Pow(2f, num);
				if (num2 >= 1f)
				{
					break;
				}
				float value = Mathf.Lerp(far, near, Mathf.Pow(falloffPower, num));
				val.AddKey(KeyframeUtil.GetNew(num2, value, TangentMode.Linear));
			}
			val.AddKey(KeyframeUtil.GetNew(1f, far, TangentMode.Linear));
		}
		val.UpdateAllLinearTangents();
		return val;
	}

	public CalculateAttenuation()
		: this()
	{
	}
}

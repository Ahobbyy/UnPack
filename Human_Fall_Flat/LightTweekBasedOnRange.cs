using System;
using UnityEngine;

public class LightTweekBasedOnRange : MonoBehaviour
{
	[Serializable]
	public class LightSetting
	{
		public Light light;

		public Transform position1;

		public Transform position2;

		public float intensity1;

		public float intensity2;
	}

	[SerializeField]
	private LightSetting[] lights;

	private void Update()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)Human.Localplayer == (Object)null) && lights != null && lights.Length != 0)
		{
			Vector3 position = ((Component)Human.Localplayer).get_gameObject().get_transform().get_position();
			LightSetting[] array = lights;
			foreach (LightSetting lightSetting in array)
			{
				lightSetting.light.set_intensity(Mathf.Lerp(lightSetting.intensity1, lightSetting.intensity2, GetInterpolationRatio(lightSetting.position1.get_position(), lightSetting.position2.get_position(), position)));
			}
		}
	}

	private float GetInterpolationRatio(Vector3 vA, Vector3 vB, Vector3 vPoint)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = vPoint - vA;
		Vector3 val2 = vB - vA;
		Vector3 normalized = ((Vector3)(ref val2)).get_normalized();
		float num = Vector3.Distance(vA, vB);
		float num2 = Vector3.Dot(normalized, val);
		if (num2 <= 0f)
		{
			return 0f;
		}
		if (num2 >= num)
		{
			return 1f;
		}
		return Mathf.Clamp01(num2 / num);
	}

	public LightTweekBasedOnRange()
		: this()
	{
	}
}

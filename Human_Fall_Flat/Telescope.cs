using HumanAPI;
using UnityEngine;

public class Telescope : Node
{
	public Light rayLight;

	public Light bounceLight;

	public Light sun;

	public float intensityPower;

	public NodeInput focus;

	private float rayLightintensity;

	private float bounceLightintensity;

	public void Awake()
	{
		rayLightintensity = rayLight.get_intensity();
		bounceLightintensity = bounceLight.get_intensity();
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		bool num = Physics.Raycast(((Component)this).get_transform().get_position(), ((Component)this).get_transform().get_forward());
		bool num2 = Physics.Raycast(((Component)this).get_transform().get_position(), -((Component)sun).get_transform().get_forward());
		float num3 = 0f - Vector3.Dot(((Component)this).get_transform().get_forward(), ((Component)sun).get_transform().get_forward());
		float num4 = Mathf.Pow(num3, intensityPower);
		float num5 = 0f;
		if (!num2)
		{
			num5 += num4;
		}
		num5 *= Mathf.Lerp(0.1f, 1f, Mathf.InverseLerp(0.99f, 0.997f, num3));
		if (!num)
		{
			num5 += 0.1f;
		}
		if (num5 > 0f)
		{
			RaycastHit val = default(RaycastHit);
			Physics.Raycast(((Component)rayLight).get_transform().get_position() - ((Component)this).get_transform().get_forward() * 3f, -((Component)this).get_transform().get_forward(), ref val);
			((Component)bounceLight).get_transform().set_position(((RaycastHit)(ref val)).get_point() + ((Component)this).get_transform().get_forward() * 0.2f);
			float num6 = Mathf.InverseLerp(-1f, 1f, focus.value);
			num5 *= Mathf.Lerp(0.2f, 1f, num6 * num6);
			rayLight.set_spotAngle(Mathf.Lerp(20f, 5f, num6 * num6));
		}
		if (rayLight.get_intensity() != rayLightintensity * num5)
		{
			rayLight.set_intensity(rayLightintensity * num5);
		}
		if (bounceLight.get_intensity() != bounceLightintensity * num5)
		{
			bounceLight.set_intensity(bounceLightintensity * num5);
		}
	}
}

using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("UPGEN Lighting/UPGEN Fast GI")]
[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public sealed class UL_FastGI : MonoBehaviour
{
	[Range(1f, 10f)]
	public float expand = 3f;

	[Range(0f, 1f)]
	public float intensity = 0.1f;

	public static readonly List<UL_FastGI> all = new List<UL_FastGI>();

	private Light _light;

	private void OnEnable()
	{
		all.Add(this);
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	internal void GenerateRenderData()
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Invalid comparison between Unknown and I4
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_light == (Object)null)
		{
			_light = ((Component)this).GetComponent<Light>();
			if ((Object)(object)_light == (Object)null)
			{
				return;
			}
		}
		LightType type = _light.get_type();
		Vector3 val;
		if ((int)type != 0)
		{
			if ((int)type != 2)
			{
				return;
			}
			val = ((Component)this).get_transform().get_position();
		}
		else
		{
			val = ((Component)this).get_transform().get_position() + ((Component)this).get_transform().get_forward();
		}
		Vector3 position = val;
		float range = _light.get_range() * expand;
		float num = _light.get_intensity() * intensity;
		Color color = _light.get_color();
		UL_Renderer.Add(position, range, num * ((Color)(ref color)).get_linear());
	}

	public UL_FastGI()
		: this()
	{
	}
}

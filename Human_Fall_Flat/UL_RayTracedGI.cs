using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[AddComponentMenu("UPGEN Lighting/UPGEN RayTraced GI")]
[RequireComponent(typeof(Light))]
[ExecuteInEditMode]
public sealed class UL_RayTracedGI : MonoBehaviour
{
	[Range(0f, 5f)]
	public float intensity = 1f;

	[Range(2f, 15f)]
	public int raysMatrixSize = 7;

	[Range(0.1f, 10f)]
	public float raysMatrixScale = 1f;

	private const float BOUNCED_LIGHT_RANGE = 8f;

	private const float BOUNCED_LIGHT_BOOST = 5f;

	private const float SUN_BOUNCED_LIGHT_BOOST = 3f;

	private const float SUN_FAR_OFFSET = 100f;

	private const float SUN_FAR_OFFSET_DBL = 200f;

	public static readonly List<UL_RayTracedGI> all = new List<UL_RayTracedGI>();

	private Light _light;

	private float _lastUpdateTime;

	private float _lastTime;

	private UL_Rays.Ray[] _rays;

	private Vector2[] _rayMatrix2D;

	private Vector3[] _rayMatrix3D;

	public Light BaseLight
	{
		get
		{
			if (!((Object)(object)_light == (Object)null))
			{
				return _light;
			}
			return _light = ((Component)this).GetComponent<Light>();
		}
	}

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
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected I4, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_017e: Expected I4, but got Unknown
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01df: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022a: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_023b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Unknown result type (might be due to invalid IL or missing references)
		//IL_0295: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0315: Unknown result type (might be due to invalid IL or missing references)
		//IL_031f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0326: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_0368: Unknown result type (might be due to invalid IL or missing references)
		//IL_0392: Unknown result type (might be due to invalid IL or missing references)
		//IL_0397: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_040c: Unknown result type (might be due to invalid IL or missing references)
		//IL_041a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0423: Unknown result type (might be due to invalid IL or missing references)
		//IL_042f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0434: Unknown result type (might be due to invalid IL or missing references)
		//IL_043d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0455: Unknown result type (might be due to invalid IL or missing references)
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0468: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_light == (Object)null)
		{
			_light = ((Component)this).GetComponent<Light>();
			if ((Object)(object)_light == (Object)null)
			{
				return;
			}
		}
		if (!((Behaviour)_light).get_enabled())
		{
			return;
		}
		float num = (float)EditorApplication.get_timeSinceStartup();
		float num2 = num - _lastTime;
		_lastTime = num;
		Vector3 val5;
		if (num - _lastUpdateTime > 0.2f)
		{
			_lastUpdateTime = num;
			UpdateRaysMatrix();
			if (_rays == UL_Rays.EMPTY_RAYS)
			{
				return;
			}
			float num3 = _light.get_intensity() * intensity;
			LightType type = _light.get_type();
			switch ((int)type)
			{
			case 0:
			case 2:
				num3 *= 5f / (float)(raysMatrixSize * raysMatrixSize);
				break;
			case 1:
				num3 *= 3f;
				break;
			}
			Color color = _light.get_color();
			Color lightColor = ((Color)(ref color)).get_linear() * num3;
			if (((Color)(ref lightColor)).get_maxColorComponent() < 0.001f)
			{
				for (int num4 = _rays.Length - 1; num4 >= 0; num4--)
				{
					_rays[num4].hit = false;
				}
				return;
			}
			Vector3 position = ((Component)this).get_transform().get_position();
			float range = _light.get_range();
			int layersToHit = (Object.op_Implicit((Object)(object)UL_Manager.instance) ? LayerMask.op_Implicit(UL_Manager.instance.layersToRayTrace) : (-5));
			type = _light.get_type();
			switch ((int)type)
			{
			case 1:
			{
				float num6 = (float)raysMatrixSize * raysMatrixScale;
				Vector3 val = ((Component)this).get_transform().get_right() * num6;
				Vector3 val2 = ((Component)this).get_transform().get_up() * num6;
				Vector3 forward = ((Component)this).get_transform().get_forward();
				Vector3 val3 = ((Component)this).get_transform().get_position() - forward * 100f;
				for (int num7 = _rayMatrix2D.Length - 1; num7 >= 0; num7--)
				{
					Vector2 val4 = _rayMatrix2D[num7];
					_rays[num7].Trace(val3 + val4.x * val + val4.y * val2, forward, 200f, lightColor, layersToHit);
				}
				break;
			}
			case 0:
			{
				float num8 = Mathf.Tan((float)Math.PI / 180f * _light.get_spotAngle() * 0.4f) * _light.get_range();
				Quaternion rotation = ((Component)this).get_transform().get_rotation();
				Vector3 forward2 = ((Component)this).get_transform().get_forward();
				for (int num9 = _rayMatrix2D.Length - 1; num9 >= 0; num9--)
				{
					UL_Rays.Ray obj = _rays[num9];
					val5 = forward2 * range + rotation * Vector2.op_Implicit(_rayMatrix2D[num9] * num8);
					obj.Trace(position, ((Vector3)(ref val5)).get_normalized(), range, lightColor, layersToHit);
				}
				break;
			}
			case 2:
			{
				for (int num5 = _rayMatrix3D.Length - 1; num5 >= 0; num5--)
				{
					_rays[num5].Trace(position, _rayMatrix3D[num5], range, lightColor, layersToHit);
				}
				break;
			}
			}
		}
		for (int num10 = _rays.Length - 1; num10 >= 0; num10--)
		{
			UL_Rays.Ray ray = _rays[num10];
			val5 = ray.interpolatedPosition - ray.position;
			if (((Vector3)(ref val5)).get_sqrMagnitude() > 9f)
			{
				if (((Color)(ref ray.interpolatedColor)).get_maxColorComponent() > 0.01f)
				{
					ray.interpolatedColor = Color.Lerp(ray.interpolatedColor, Color.get_black(), num2 * 10f);
				}
				else
				{
					ray.interpolatedColor = Color.Lerp(ray.interpolatedColor, ray.hit ? ray.color : Color.get_black(), num2 * 10f);
					ray.interpolatedPosition = ray.position;
				}
				UL_Renderer.Add(ray.interpolatedPosition, 8f, ray.interpolatedColor);
			}
			else
			{
				ray.interpolatedColor = Color.Lerp(ray.interpolatedColor, ray.hit ? ray.color : Color.get_black(), num2 * 5f);
				ray.interpolatedPosition = Vector3.Lerp(ray.interpolatedPosition, ray.position, num2 * 10f);
				UL_Renderer.Add(ray.interpolatedPosition, 8f, ray.interpolatedColor);
			}
		}
	}

	private void UpdateRaysMatrix()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Invalid comparison between Unknown and I4
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Invalid comparison between Unknown and I4
		LightType type = _light.get_type();
		if ((int)type > 1)
		{
			if ((int)type == 2)
			{
				Vector3[] array = UL_RayMatrices.SPHERE[raysMatrixSize - 2];
				if (_rayMatrix3D != array)
				{
					_rayMatrix3D = array;
					_rays = UL_Rays.GenerateRays(array.Length);
				}
			}
			else
			{
				_rays = UL_Rays.EMPTY_RAYS;
			}
		}
		else
		{
			Vector2[] array2 = UL_RayMatrices.GRID[raysMatrixSize - 2];
			if (_rayMatrix2D != array2)
			{
				_rayMatrix2D = array2;
				_rays = UL_Rays.GenerateRays(array2.Length);
			}
		}
	}

	private void BuildFastLight(UL_Rays.Ray ray)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		if (ray.hit)
		{
			GameObject val = new GameObject("Fast Light");
			val.get_transform().SetParent(((Component)this).get_transform(), false);
			val.get_transform().set_position(ray.position);
			UL_FastLight uL_FastLight = val.AddComponent<UL_FastLight>();
			uL_FastLight.intensity = 1f;
			uL_FastLight.range = 8f;
			uL_FastLight.color = ((Color)(ref ray.color)).get_gamma();
			Undo.RegisterCreatedObjectUndo((Object)val, "Create Fast Lights");
		}
	}

	public void CreateFastLights()
	{
		if (((Behaviour)this).get_enabled())
		{
			for (int num = _rays.Length - 1; num >= 0; num--)
			{
				BuildFastLight(_rays[num]);
			}
			Undo.RecordObject((Object)(object)this, "Create Fast Lights");
			((Behaviour)this).set_enabled(false);
		}
	}

	public void DestroyFastLights()
	{
		if (((Behaviour)this).get_enabled())
		{
			return;
		}
		for (int num = ((Component)this).get_transform().get_childCount() - 1; num >= 0; num--)
		{
			Transform child = ((Component)this).get_transform().GetChild(num);
			if (Object.op_Implicit((Object)(object)((Component)child).GetComponent<UL_FastLight>()))
			{
				Undo.DestroyObjectImmediate((Object)(object)((Component)child).get_gameObject());
			}
		}
		Undo.RecordObject((Object)(object)this, "Destroy Fast Lights");
		((Behaviour)this).set_enabled(true);
	}

	public UL_RayTracedGI()
		: this()
	{
	}
}

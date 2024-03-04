using System.Collections.Generic;
using UnityEngine;

public static class UL_Rays
{
	public class Ray
	{
		public bool hit;

		public Vector3 interpolatedPosition;

		public Vector3 position;

		public Color interpolatedColor;

		public Color color;

		private const float HIT_OFFSET = 1f;

		private const float HIT_OFFSET_MAX = 2f;

		private static readonly int _propertyColorId = Shader.PropertyToID("_Color");

		private static readonly int _propertyMainTexId = Shader.PropertyToID("_MainTex");

		private static readonly Dictionary<Renderer, Color> _cachedRendererColor = new Dictionary<Renderer, Color>();

		private static RenderTexture _renderTexture;

		private static Texture2D _tempTexture;

		public void Point(Vector3 pt, Vector3 dir)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			hit = true;
			position = pt + dir;
			color = Color.get_white();
		}

		public void Trace(Vector3 pt, Vector3 dir, float range, Color lightColor, int layersToHit)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			bool flag = hit;
			hit = false;
			RaycastHit val = default(RaycastHit);
			if (!Physics.Raycast(pt, dir, ref val, range * 0.9f, layersToHit))
			{
				return;
			}
			Vector3 point = ((RaycastHit)(ref val)).get_point();
			Vector3 normal = ((RaycastHit)(ref val)).get_normal();
			Vector3 val2 = point + normal * 0.001f;
			float distance = ((RaycastHit)(ref val)).get_distance();
			if (distance < 0.1f)
			{
				return;
			}
			RaycastHit val3 = default(RaycastHit);
			position = (Physics.Raycast(val2, normal, ref val3, 2f, layersToHit) ? (point + 0.5f * ((RaycastHit)(ref val3)).get_distance() * normal) : (point + 1f * normal));
			if (Physics.CheckSphere(position, 0.2f, layersToHit))
			{
				position = (Physics.Raycast(val2, -dir, ref val3, 2f, layersToHit) ? (point - 0.5f * ((RaycastHit)(ref val3)).get_distance() * dir) : (point - 1f * dir));
				if (Physics.CheckSphere(position, 0.1f, layersToHit))
				{
					return;
				}
			}
			float num = distance / range;
			num *= num;
			num = 1f - num;
			color = GetRayHitColor(((Component)((RaycastHit)(ref val)).get_transform()).GetComponent<Renderer>());
			color.r *= color.r * color.r * lightColor.r * num;
			color.g *= color.g * color.g * lightColor.g * num;
			color.b *= color.b * color.b * lightColor.b * num;
			hit = true;
			if (!flag)
			{
				interpolatedColor = color;
				interpolatedPosition = position;
			}
			if (Object.op_Implicit((Object)(object)UL_Manager.instance) && UL_Manager.instance.showDebugRays)
			{
				Debug.DrawLine(point, position, color * 2f, 0.21f);
			}
		}

		private static Color GetRayHitColor(Renderer r)
		{
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected O, but got Unknown
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Expected O, but got Unknown
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0121: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)r == (Object)null)
			{
				return Color.get_white();
			}
			if (_cachedRendererColor.TryGetValue(r, out var value))
			{
				return value;
			}
			Material sharedMaterial = r.get_sharedMaterial();
			if ((Object)(object)sharedMaterial == (Object)null)
			{
				_cachedRendererColor.Add(r, Color.get_white());
				return Color.get_white();
			}
			value = (sharedMaterial.HasProperty(_propertyColorId) ? sharedMaterial.GetColor(_propertyColorId) : Color.get_white());
			if (!sharedMaterial.HasProperty(_propertyMainTexId))
			{
				_cachedRendererColor.Add(r, value);
				return value;
			}
			Texture mainTexture = sharedMaterial.get_mainTexture();
			if ((Object)(object)mainTexture == (Object)null)
			{
				_cachedRendererColor.Add(r, value);
				return value;
			}
			if ((Object)(object)_renderTexture == (Object)null)
			{
				_renderTexture = new RenderTexture(1, 1, 0);
			}
			if ((Object)(object)_tempTexture == (Object)null)
			{
				_tempTexture = new Texture2D(1, 1);
			}
			Graphics.Blit(mainTexture, _renderTexture);
			RenderTexture active = RenderTexture.get_active();
			RenderTexture.set_active(_renderTexture);
			_tempTexture.ReadPixels(new Rect(0f, 0f, 1f, 1f), 0, 0, false);
			_tempTexture.Apply();
			RenderTexture.set_active(active);
			value *= _tempTexture.GetPixel(0, 0);
			_cachedRendererColor.Add(r, value);
			return value;
		}
	}

	public static readonly Ray[] EMPTY_RAYS = new Ray[0];

	public static Ray[] GenerateRays(int count)
	{
		Ray[] array = new Ray[count];
		for (int i = 0; i < count; i++)
		{
			array[i] = new Ray();
		}
		return array;
	}
}

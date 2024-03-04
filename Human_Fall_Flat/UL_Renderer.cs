using System.Collections.Generic;
using UnityEngine;

public static class UL_Renderer
{
	private class Light
	{
		public float score;

		public Vector4 position;

		public Vector4 color;
	}

	private const int MAX_LIGHTS_COUNT = 128;

	private static int _lightsCount;

	private static readonly Vector4[] _lightsPositions = (Vector4[])(object)new Vector4[128];

	private static readonly Color[] _lightsColors = (Color[])(object)new Color[128];

	private static readonly Stack<Light> _lightsPool = new Stack<Light>();

	private static readonly List<Light> _lightsUsed = new List<Light>();

	private static Vector3 _cameraPosition;

	private static Vector3 _cameraForward;

	private const float _cameraFOVAngle = 50f;

	private static readonly float _cameraFOVCos = Mathf.Cos(0.87266463f);

	private static readonly float _cameraFOVSin = Mathf.Sin(0.87266463f);

	private static Texture2D TestTex;

	private static Color[] colors;

	public static bool HasLightsToRender
	{
		get
		{
			if (UL_FastLight.all.Count <= 0 && UL_FastGI.all.Count <= 0)
			{
				return UL_RayTracedGI.all.Count > 0;
			}
			return true;
		}
	}

	public static int RenderedLightsCount => _lightsCount;

	public static int MaxRenderingLightsCount => 128;

	private static int ScoresComparison(Light x, Light y)
	{
		if (x.score < y.score)
		{
			return -1;
		}
		if (x.score > y.score)
		{
			return 1;
		}
		return 0;
	}

	public static void Add(Vector3 position, float range, Color color)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		if (((Color)(ref color)).get_maxColorComponent() < 0.001f)
		{
			return;
		}
		Vector3 val = position - _cameraPosition;
		float num = Vector3.Dot(val, _cameraForward);
		float num2 = _cameraFOVCos * Mathf.Sqrt(Vector3.Dot(val, val) - num * num) - num * _cameraFOVSin;
		if (!(num2 >= 0f) || !(Mathf.Abs(num2) >= range))
		{
			Light light = ((_lightsPool.Count > 0) ? _lightsPool.Pop() : new Light());
			light.score = ((Vector3)(ref val)).get_sqrMagnitude() - (2f - num) * range;
			light.position.x = position.x;
			light.position.y = position.y;
			light.position.z = position.z;
			light.position.w = range;
			light.color.x = color.r;
			light.color.y = color.g;
			light.color.z = color.b;
			_lightsUsed.Add(light);
			if (color.r < 0f || color.g < 0f || color.b < 0f)
			{
				Debug.LogError((object)$"Add color: {color}");
			}
		}
	}

	public static void SetupForCamera(Camera camera, MaterialPropertyBlock properties)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0119: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_0222: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0236: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_028d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0292: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0320: Unknown result type (might be due to invalid IL or missing references)
		CreateTexture();
		Transform transform = ((Component)camera).get_transform();
		_cameraPosition = transform.get_position();
		_cameraForward = transform.get_forward();
		for (int num = UL_FastLight.all.Count - 1; num >= 0; num--)
		{
			UL_FastLight.all[num].GenerateRenderData();
		}
		for (int num2 = UL_FastGI.all.Count - 1; num2 >= 0; num2--)
		{
			UL_FastGI.all[num2].GenerateRenderData();
		}
		for (int num3 = UL_RayTracedGI.all.Count - 1; num3 >= 0; num3--)
		{
			UL_RayTracedGI.all[num3].GenerateRenderData();
		}
		_lightsCount = Mathf.Min(_lightsUsed.Count, 128);
		_lightsUsed.Sort(ScoresComparison);
		for (int num4 = _lightsCount - 1; num4 >= 0; num4--)
		{
			Light light = _lightsUsed[num4];
			_lightsPositions[num4] = light.position;
			_lightsColors[num4] = Color.op_Implicit(light.color);
			colors[num4] = Color.op_Implicit(light.color);
			if (light.color.x < 0f || light.color.y < 0f || light.color.z < 0f || light.color.w < 0f)
			{
				Debug.LogError((object)$"SetupForCamera: {light.color}");
			}
			_lightsPool.Push(light);
		}
		for (int num5 = _lightsUsed.Count - 1; num5 >= _lightsCount; num5--)
		{
			_lightsPool.Push(_lightsUsed[num5]);
		}
		_lightsUsed.Clear();
		properties.SetFloat("_LightsCount", (float)_lightsCount);
		properties.SetVectorArray("_LightsPositions", _lightsPositions);
		TestTex.SetPixels(colors);
		TestTex.Apply();
		Matrix4x4 val;
		if (camera.get_stereoEnabled())
		{
			val = GL.GetGPUProjectionMatrix(camera.GetStereoProjectionMatrix((StereoscopicEye)0), true);
			Matrix4x4 inverse = ((Matrix4x4)(ref val)).get_inverse();
			ref Matrix4x4 reference = ref inverse;
			((Matrix4x4)(ref reference)).set_Item(1, 1, ((Matrix4x4)(ref reference)).get_Item(1, 1) * -1f);
			properties.SetMatrix("_LeftViewFromScreen", inverse);
			val = camera.GetStereoViewMatrix((StereoscopicEye)0);
			properties.SetMatrix("_LeftWorldFromView", ((Matrix4x4)(ref val)).get_inverse());
			val = GL.GetGPUProjectionMatrix(camera.GetStereoProjectionMatrix((StereoscopicEye)1), true);
			Matrix4x4 inverse2 = ((Matrix4x4)(ref val)).get_inverse();
			reference = ref inverse2;
			((Matrix4x4)(ref reference)).set_Item(1, 1, ((Matrix4x4)(ref reference)).get_Item(1, 1) * -1f);
			properties.SetMatrix("_RightViewFromScreen", inverse2);
			val = camera.GetStereoViewMatrix((StereoscopicEye)1);
			properties.SetMatrix("_RightWorldFromView", ((Matrix4x4)(ref val)).get_inverse());
		}
		else
		{
			val = GL.GetGPUProjectionMatrix(camera.get_projectionMatrix(), true);
			Matrix4x4 inverse3 = ((Matrix4x4)(ref val)).get_inverse();
			ref Matrix4x4 reference = ref inverse3;
			((Matrix4x4)(ref reference)).set_Item(1, 1, ((Matrix4x4)(ref reference)).get_Item(1, 1) * -1f);
			properties.SetMatrix("_LeftViewFromScreen", inverse3);
			properties.SetMatrix("_LeftWorldFromView", camera.get_cameraToWorldMatrix());
		}
	}

	private static void CreateTexture()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)TestTex != (Object)null))
		{
			bool flag = false;
			bool flag2 = true;
			TextureFormat val = (TextureFormat)20;
			TestTex = new Texture2D(128, 1, val, flag, flag2);
			((Texture)TestTex).set_filterMode((FilterMode)0);
			((Texture)TestTex).set_wrapMode((TextureWrapMode)1);
			colors = (Color[])(object)new Color[128];
			for (int i = 0; i < 128; i++)
			{
				colors[i] = Color.get_red();
			}
			TestTex.SetPixels(colors);
			TestTex.Apply();
			Shader.SetGlobalTexture("_Colors", (Texture)(object)TestTex);
		}
	}
}

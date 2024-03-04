using Multiplayer;
using UnityEngine;

public class CaveRender : MonoBehaviour
{
	private static float _defaultFogDensity;

	private static Color _defaultFogColor = Color.get_black();

	public static float fogDensityMultiplier = 1f;

	private Color creditsFogColor = new Color(142f / 255f, 154f / 255f, 32f / 51f);

	private Color defaultAmbientColor;

	public Color waterFogColor = Color.get_blue();

	public float waterFogDensity = 0.1f;

	private CaveLighting cave;

	private NetPlayer player;

	private Color fogColor;

	private float fogDensity;

	public static float defaultFogDensity => fogDensityMultiplier * (((Object)(object)Game.currentLevel != (Object)null) ? Game.currentLevel.fogDensity : _defaultFogDensity);

	public static Color defaultFogColor
	{
		get
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)Game.currentLevel != (Object)null))
			{
				return _defaultFogColor;
			}
			return Game.currentLevel.fogColor;
		}
	}

	private void OnEnable()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		player = ((Component)this).GetComponentInParent<NetPlayer>();
		defaultAmbientColor = RenderSettings.get_ambientLight();
		if (defaultFogColor == Color.get_black())
		{
			_defaultFogColor = RenderSettings.get_fogColor();
			_defaultFogDensity = RenderSettings.get_fogDensity();
		}
	}

	private void OnPreCull()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		FogMode val = (FogMode)3;
		fogColor = Color.Lerp(defaultFogColor, creditsFogColor, MenuCameraEffects.instance.creditsAdjust);
		fogDensity = defaultFogDensity;
		if ((Object)(object)player != (Object)null && (Object)(object)player.cameraController.waterSensor != (Object)null && (Object)(object)player.cameraController.waterSensor.waterBody != (Object)null)
		{
			Vector3 velocity;
			float num = player.cameraController.waterSensor.waterBody.SampleDepth(((Component)this).get_transform().get_position(), out velocity) * 10f - 0.5f;
			fogColor = Color.Lerp(fogColor, waterFogColor, num);
			fogDensity = Mathf.Lerp(defaultFogDensity, waterFogDensity, num);
			if (num > 0f)
			{
				RenderSettings.set_fogMode((FogMode)2);
			}
		}
		if ((Object)(object)player != (Object)null)
		{
			cave = CaveLighting.GetCaveForPlayer(player);
			if ((Object)(object)cave != (Object)null)
			{
				fogDensity *= Mathf.Lerp(1f, 0.1f, cave.GetPlaseForPlayer(player));
			}
		}
		if (RenderSettings.get_fogMode() != val)
		{
			RenderSettings.set_fogMode(val);
		}
		if (RenderSettings.get_fogColor() != fogColor)
		{
			RenderSettings.set_fogColor(fogColor);
		}
		if (RenderSettings.get_fogDensity() != fogDensity)
		{
			RenderSettings.set_fogDensity(fogDensity);
		}
		for (int i = 0; i < Human.all.Count; i++)
		{
			if ((Object)(object)Human.all[i].player.nametag != (Object)null)
			{
				Human.all[i].player.nametag.Align(((Component)this).get_transform());
			}
		}
	}

	public CaveRender()
		: this()
	{
	}//IL_0010: Unknown result type (might be due to invalid IL or missing references)
	//IL_0015: Unknown result type (might be due to invalid IL or missing references)
	//IL_001b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Unknown result type (might be due to invalid IL or missing references)

}

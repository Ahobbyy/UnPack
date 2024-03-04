using HumanAPI;
using UnityEngine;

public class CollisionAudioSurfSurfConfig : MonoBehaviour
{
	public SurfaceType surf1;

	public SurfaceType surf2;

	public bool isDefault;

	public CollisionAudioSurfSurfConfig link;

	public CollisionAudioSurfSurfConfig mirror;

	public CollisionAudioHitConfig hit = new CollisionAudioHitConfig();

	public CollisionAudioHitConfig slide = new CollisionAudioHitConfig();

	public CollisionAudioHitMonitor hitMonitor = new CollisionAudioHitMonitor();

	public CollisionAudioHitMonitor slideMonitor = new CollisionAudioHitMonitor();

	public float levelDB;

	public float slideTreshold;

	public float lastSlideAmount;

	public CollisionAudioSurfSurfConfig controlConfig
	{
		get
		{
			if ((Object)(object)mirror != (Object)null)
			{
				return mirror.controlConfig;
			}
			if ((Object)(object)link != (Object)null)
			{
				return link.controlConfig;
			}
			return this;
		}
	}

	public CollisionAudioSurfSurfConfig monitorConfig
	{
		get
		{
			if ((Object)(object)mirror != (Object)null)
			{
				return mirror.monitorConfig;
			}
			return this;
		}
	}

	public bool PlayImpact(CollisionAudioSensor sensor, AudioChannel channel, Vector3 pos, float impulse, float normalVelocity, float tangentVelocity, float volume, float pitch)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		if (controlConfig.slide != null && (Object)(object)controlConfig.slide.sampleLib != (Object)null && tangentVelocity != 0f)
		{
			Vector2 val = new Vector2(normalVelocity, tangentVelocity);
			num = ((Vector2)(ref val)).get_normalized().y;
		}
		monitorConfig.lastSlideAmount = num;
		if (num > controlConfig.slideTreshold)
		{
			return controlConfig.slide.Play(sensor, channel, monitorConfig.slideMonitor, pos, impulse, tangentVelocity, volume, pitch);
		}
		return controlConfig.hit.Play(sensor, channel, monitorConfig.hitMonitor, pos, impulse, normalVelocity, volume, pitch);
	}

	public CollisionAudioSurfSurfConfig()
		: this()
	{
	}
}

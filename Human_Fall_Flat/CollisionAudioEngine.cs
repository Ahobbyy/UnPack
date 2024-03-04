using System;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class CollisionAudioEngine : MonoBehaviour
{
	public float minVelocity = 0.5f;

	public float minImpulse = 5f;

	public float hitDelay = 0.1f;

	public float unitImpulse = 100f;

	public float unitVelocity = 5f;

	public GameObject runtimeConfigs;

	public static CollisionAudioEngine instance;

	public Dictionary<SurfaceType, Dictionary<SurfaceType, CollisionAudioSurfSurfConfig>> map = new Dictionary<SurfaceType, Dictionary<SurfaceType, CollisionAudioSurfSurfConfig>>();

	public SurfaceType soloSurface;

	private Dictionary<ushort, CollisionAudioHitConfig> configIdMap = new Dictionary<ushort, CollisionAudioHitConfig>();

	private void OnEnable()
	{
		instance = this;
		RebuildMap();
	}

	public void RebuildMap()
	{
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		map.Clear();
		CollisionAudioSurfSurfConfig[] componentsInChildren = runtimeConfigs.GetComponentsInChildren<CollisionAudioSurfSurfConfig>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Object.DestroyImmediate((Object)(object)((Component)componentsInChildren[i]).get_gameObject());
		}
		Array values = Enum.GetValues(typeof(SurfaceType));
		for (int j = 0; j < values.Length; j++)
		{
			SurfaceType surfaceType = (SurfaceType)values.GetValue(j);
			if (surfaceType == SurfaceType.Unknown)
			{
				continue;
			}
			Dictionary<SurfaceType, CollisionAudioSurfSurfConfig> dictionary = new Dictionary<SurfaceType, CollisionAudioSurfSurfConfig>();
			map[surfaceType] = dictionary;
			for (int k = 0; k < values.Length; k++)
			{
				SurfaceType surfaceType2 = (SurfaceType)values.GetValue(k);
				if (surfaceType2 != 0)
				{
					dictionary[surfaceType2] = null;
				}
			}
		}
		ushort num = 0;
		configIdMap.Clear();
		CollisionAudioSurfSurfConfig[] componentsInChildren2 = ((Component)this).GetComponentsInChildren<CollisionAudioSurfSurfConfig>();
		foreach (CollisionAudioSurfSurfConfig cfg in componentsInChildren2)
		{
			AddCofigToMap(cfg);
		}
		foreach (CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig in componentsInChildren2)
		{
			if (!collisionAudioSurfSurfConfig.isDefault)
			{
				continue;
			}
			Dictionary<SurfaceType, CollisionAudioSurfSurfConfig> dictionary2 = map[collisionAudioSurfSurfConfig.surf1];
			foreach (SurfaceType item in new List<SurfaceType>(dictionary2.Keys))
			{
				if ((Object)(object)dictionary2[item] != (Object)null)
				{
					CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig2 = dictionary2[item];
					if (collisionAudioSurfSurfConfig2.hit != null)
					{
						collisionAudioSurfSurfConfig2.hit.netId = num++;
						configIdMap[collisionAudioSurfSurfConfig2.hit.netId] = collisionAudioSurfSurfConfig2.hit;
					}
					if (collisionAudioSurfSurfConfig2.slide != null)
					{
						collisionAudioSurfSurfConfig2.slide.netId = num++;
						configIdMap[collisionAudioSurfSurfConfig2.slide.netId] = collisionAudioSurfSurfConfig2.slide;
					}
				}
				if ((Object)(object)dictionary2[item] == (Object)null && item != SurfaceType.RagdollBall)
				{
					CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig3 = new GameObject().AddComponent<CollisionAudioSurfSurfConfig>();
					collisionAudioSurfSurfConfig3.surf1 = collisionAudioSurfSurfConfig.surf1;
					collisionAudioSurfSurfConfig3.surf2 = item;
					collisionAudioSurfSurfConfig3.link = collisionAudioSurfSurfConfig.controlConfig;
					((Object)collisionAudioSurfSurfConfig3).set_name($"{collisionAudioSurfSurfConfig3.surf1}{collisionAudioSurfSurfConfig3.surf2}Cloned{((Object)collisionAudioSurfSurfConfig).get_name()}");
					((Component)collisionAudioSurfSurfConfig3).get_transform().SetParent(runtimeConfigs.get_transform(), false);
					AddCofigToMap(collisionAudioSurfSurfConfig3);
				}
			}
		}
	}

	private void AddCofigToMap(CollisionAudioSurfSurfConfig cfg)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)map[cfg.surf1][cfg.surf2] != (Object)null)
		{
			Debug.LogError((object)("CollisionAudioSurfSurfConfig already defined for " + cfg.surf1.ToString() + " " + cfg.surf2), (Object)(object)cfg);
		}
		map[cfg.surf1][cfg.surf2] = cfg;
		if (cfg.surf1 != cfg.surf2)
		{
			CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig = new GameObject().AddComponent<CollisionAudioSurfSurfConfig>();
			collisionAudioSurfSurfConfig.surf1 = cfg.surf2;
			collisionAudioSurfSurfConfig.surf2 = cfg.surf1;
			collisionAudioSurfSurfConfig.mirror = cfg;
			((Object)collisionAudioSurfSurfConfig).set_name($"{collisionAudioSurfSurfConfig.surf1}{collisionAudioSurfSurfConfig.surf2}Mirror");
			((Component)collisionAudioSurfSurfConfig).get_transform().SetParent(runtimeConfigs.get_transform(), false);
			map[cfg.surf2][cfg.surf1] = collisionAudioSurfSurfConfig;
		}
	}

	public CollisionAudioSurfSurfConfig Resolve(SurfaceType surf1, SurfaceType surf2)
	{
		if (map.TryGetValue(surf1, out var value) && value.TryGetValue(surf2, out var value2))
		{
			return value2;
		}
		return null;
	}

	public CollisionAudioHitConfig GetConfig(ushort libId)
	{
		configIdMap.TryGetValue(libId, out var value);
		return value;
	}

	public CollisionAudioSurfSurfConfig ResolveFinal(SurfaceType surf1, SurfaceType surf2)
	{
		CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig = Resolve(surf1, surf2);
		if ((Object)(object)collisionAudioSurfSurfConfig == (Object)null)
		{
			return null;
		}
		return collisionAudioSurfSurfConfig.controlConfig;
	}

	public bool ReportCollision(CollisionAudioSensor sensor, SurfaceType surf1, SurfaceType surf2, Vector3 pos, float impulse, float normalVelocity, float tangentVelocity, float volume, float pitch)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (surf1 == SurfaceType.RagdollBall || surf2 == SurfaceType.RagdollBall)
		{
			return false;
		}
		if (soloSurface != 0 && surf1 != soloSurface && surf2 != soloSurface)
		{
			return false;
		}
		CollisionAudioSurfSurfConfig collisionAudioSurfSurfConfig = Resolve(surf1, surf2);
		if ((Object)(object)collisionAudioSurfSurfConfig != (Object)null)
		{
			AudioChannel channel = AudioChannel.Physics;
			if (surf1 == SurfaceType.RagdollBody || surf2 == SurfaceType.RagdollBody)
			{
				channel = AudioChannel.Body;
			}
			else if (surf1 == SurfaceType.RagdollFeet || surf2 == SurfaceType.RagdollFeet)
			{
				channel = AudioChannel.Footsteps;
			}
			return collisionAudioSurfSurfConfig.PlayImpact(sensor, channel, pos, impulse, normalVelocity, tangentVelocity, volume, pitch);
		}
		return false;
	}

	public CollisionAudioEngine()
		: this()
	{
	}
}

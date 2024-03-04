using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumanAPI
{
	public class Ambience : MonoBehaviour
	{
		[Serializable]
		public class AmbienceState
		{
			public AmbienceZoneState[] zones;
		}

		[Serializable]
		public class AmbienceZoneState
		{
			public string name;

			public AmbienceZoneSourceState[] sources;

			public float mainVerbLevel;

			public float musicLevel;

			public float ambienceLevel;

			public float effectsLevel;

			public float physicsLevel;

			public float characterLevel;

			public float ambienceFxLevel;
		}

		[Serializable]
		public class AmbienceZoneSourceState
		{
			public string source;

			public float volume;
		}

		public static Ambience instance;

		private List<AmbienceZone> activeZones = new List<AmbienceZone>();

		[NonSerialized]
		public AmbienceZone activeZone;

		public bool useTriggers;

		public AmbienceSource[] sources;

		private AmbienceZone[] zones;

		[NonSerialized]
		public AmbienceZone forcedZone;

		public void OnEnable()
		{
			instance = this;
			sources = ((Component)this).GetComponentsInChildren<AmbienceSource>();
		}

		private void Start()
		{
			SoundManager componentInParent = ((Component)this).GetComponentInParent<SoundManager>();
			if ((Object)(object)componentInParent != (Object)null && (Object)(object)componentInParent.storedState != (Object)null && componentInParent.storedState.ambience != null)
			{
				LoadJson(componentInParent.storedState.ambience);
			}
		}

		public void TransitionToZone(AmbienceZone zone, float duration)
		{
			activeZone = zone;
			for (int i = 0; i < sources.Length; i++)
			{
				float volume = 0f;
				for (int j = 0; j < zone.sources.Length; j++)
				{
					if ((Object)(object)zone.sources[j] == (Object)(object)sources[i])
					{
						volume = zone.volumes[j];
					}
				}
				sources[i].FadeVolume(volume, duration);
			}
			GameAudio.instance.SetAmbienceZoneMix(zone, duration);
		}

		public void EnterZone(AmbienceZone trigger)
		{
			if (!activeZones.Contains(trigger))
			{
				activeZones.Add(trigger);
				CalculateActiveZoneTrigger();
			}
		}

		public void LeaveZone(AmbienceZone trigger)
		{
			if (activeZones.Contains(trigger))
			{
				activeZones.Remove(trigger);
				CalculateActiveZoneTrigger();
			}
		}

		private void CalculateActiveZoneTrigger()
		{
			if (activeZones.Count == 0)
			{
				return;
			}
			AmbienceZone ambienceZone = activeZones[0];
			for (int i = 1; i < activeZones.Count; i++)
			{
				if (activeZones[i].priority < ambienceZone.priority)
				{
					ambienceZone = activeZones[i];
				}
			}
			if (!((Object)(object)activeZone == (Object)(object)ambienceZone))
			{
				TransitionToZone(ambienceZone, ambienceZone.transitionDuration);
			}
		}

		private void CalculateActiveZone()
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			int num = int.MinValue;
			AmbienceZone ambienceZone = null;
			if ((Object)(object)forcedZone != (Object)null)
			{
				ambienceZone = forcedZone;
			}
			else
			{
				Vector3 position = ((Component)Listener.instance).get_transform().get_position();
				if (zones == null)
				{
					zones = ((Component)this).GetComponentsInChildren<AmbienceZone>();
				}
				for (int i = 0; i < zones.Length; i++)
				{
					Bounds bounds = ((Component)zones[i]).GetComponent<Collider>().get_bounds();
					if (((Bounds)(ref bounds)).Contains(position) && zones[i].priority > num)
					{
						ambienceZone = zones[i];
						num = zones[i].priority;
					}
				}
			}
			if (!((Object)(object)ambienceZone == (Object)null) && !((Object)(object)activeZone == (Object)(object)ambienceZone))
			{
				TransitionToZone(ambienceZone, ambienceZone.transitionDuration);
			}
		}

		public static string GetPath()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Scene activeScene = SceneManager.GetActiveScene();
			string name = ((Scene)(ref activeScene)).get_name();
			return "Audio/" + name + "Ambience.txt";
		}

		public void Load()
		{
			try
			{
				string json = File.ReadAllText(GetPath());
				LoadJson(json);
			}
			catch
			{
			}
		}

		public void LoadJson(string json)
		{
			AmbienceState state = JsonUtility.FromJson<AmbienceState>(json);
			LoadJson(state);
		}

		private void LoadJson(AmbienceState state)
		{
			AmbienceSource[] componentsInChildren = ((Component)this).GetComponentsInChildren<AmbienceSource>();
			AmbienceZone[] componentsInChildren2 = ((Component)this).GetComponentsInChildren<AmbienceZone>();
			for (int i = 0; i < state.zones.Length; i++)
			{
				AmbienceZoneState ambienceZoneState = state.zones[i];
				foreach (AmbienceZone ambienceZone in componentsInChildren2)
				{
					if (!(((Object)ambienceZone).get_name() == ambienceZoneState.name))
					{
						continue;
					}
					ambienceZone.mainVerbLevel = ambienceZoneState.mainVerbLevel;
					ambienceZone.musicLevel = ambienceZoneState.musicLevel;
					ambienceZone.ambienceLevel = ambienceZoneState.ambienceLevel;
					ambienceZone.effectsLevel = ambienceZoneState.effectsLevel;
					ambienceZone.physicsLevel = ambienceZoneState.physicsLevel;
					ambienceZone.characterLevel = ambienceZoneState.characterLevel;
					ambienceZone.ambienceFxLevel = ambienceZoneState.ambienceFxLevel;
					if (ambienceZone.sources == null || ambienceZone.sources.Length != ambienceZoneState.sources.Length)
					{
						ambienceZone.sources = new AmbienceSource[ambienceZoneState.sources.Length];
					}
					if (ambienceZone.volumes == null || ambienceZone.volumes.Length != ambienceZoneState.sources.Length)
					{
						ambienceZone.volumes = new float[ambienceZoneState.sources.Length];
					}
					int num = 0;
					for (int k = 0; k < ambienceZoneState.sources.Length; k++)
					{
						AmbienceZoneSourceState ambienceZoneSourceState = ambienceZoneState.sources[k];
						foreach (AmbienceSource ambienceSource in componentsInChildren)
						{
							if (ambienceZoneSourceState.source == ((Object)ambienceSource).get_name())
							{
								ambienceZone.sources[num] = ambienceSource;
								ambienceZone.volumes[num] = ambienceZoneSourceState.volume;
								num++;
							}
						}
					}
					if (num != ambienceZone.sources.Length)
					{
						Array.Resize(ref ambienceZone.sources, num);
						Array.Resize(ref ambienceZone.volumes, num);
					}
				}
			}
		}

		public void ForceZone(AmbienceZone zone)
		{
			forcedZone = zone;
			CalculateActiveZone();
		}

		public void Save()
		{
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Expected O, but got Unknown
			((Component)this).GetComponentsInChildren<AmbienceSource>();
			AmbienceZone[] componentsInChildren = ((Component)this).GetComponentsInChildren<AmbienceZone>();
			AmbienceState ambienceState = new AmbienceState
			{
				zones = new AmbienceZoneState[componentsInChildren.Length]
			};
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				AmbienceZone ambienceZone = componentsInChildren[i];
				ambienceState.zones[i] = new AmbienceZoneState
				{
					name = ((Object)ambienceZone).get_name(),
					sources = new AmbienceZoneSourceState[ambienceZone.volumes.Length],
					mainVerbLevel = ambienceZone.mainVerbLevel,
					musicLevel = ambienceZone.musicLevel,
					ambienceLevel = ambienceZone.ambienceLevel,
					effectsLevel = ambienceZone.effectsLevel,
					physicsLevel = ambienceZone.physicsLevel,
					characterLevel = ambienceZone.characterLevel,
					ambienceFxLevel = ambienceZone.ambienceFxLevel
				};
				for (int j = 0; j < ambienceZone.volumes.Length; j++)
				{
					ambienceState.zones[i].sources[j] = new AmbienceZoneSourceState
					{
						source = ((Object)ambienceZone.sources[j]).get_name(),
						volume = ambienceZone.volumes[j]
					};
				}
			}
			string contents = JsonUtility.ToJson((object)ambienceState, true);
			string path = GetPath();
			Provider.Checkout(new Asset(path), (CheckoutMode)1).Wait();
			File.WriteAllText(path, contents);
		}

		private void Update()
		{
			CalculateActiveZone();
		}

		public Ambience()
			: this()
		{
		}
	}
}

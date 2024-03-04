using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumanAPI
{
	public class SoundManager : MonoBehaviour
	{
		public class SoundGroup
		{
			public string name;

			public List<SoundMaster> sounds = new List<SoundMaster>();
		}

		[Serializable]
		public class SoundManagerState
		{
			public SoundState[] sounds;

			public GrainState[] grains;

			[NonSerialized]
			public Dictionary<string, SoundState> soundStateMap;

			[NonSerialized]
			public Dictionary<string, GrainState> grainStateMap;

			public void Populate()
			{
				if (sounds == null)
				{
					sounds = new SoundState[0];
				}
				if (grains == null)
				{
					grains = new GrainState[0];
				}
				soundStateMap = new Dictionary<string, SoundState>();
				grainStateMap = new Dictionary<string, GrainState>();
				for (int i = 0; i < sounds.Length; i++)
				{
					soundStateMap[sounds[i].id] = sounds[i];
				}
				for (int j = 0; j < grains.Length; j++)
				{
					grainStateMap[grains[j].id] = grains[j];
				}
			}

			public SoundState GetSoundState(string name)
			{
				soundStateMap.TryGetValue(name, out var value);
				return value;
			}

			public GrainState GetGrainState(string name)
			{
				grainStateMap.TryGetValue(name, out var value);
				return value;
			}
		}

		[Serializable]
		public class SoundState
		{
			public string id;

			public string sample;

			public float volume;

			public float pitch;

			public bool useMaster;

			public float maxDistance = 30f;

			public float falloffStart = 1f;

			public float falloffPower = 0.5f;

			public float lpStart = 2f;

			public float lpPower = 0.5f;

			public float spreadNear = 0.5f;

			public float spreadFar;

			public float spatialNear = 0.5f;

			public float spatialFar = 1f;
		}

		[Serializable]
		public class GrainState
		{
			public string id;

			public float slowVolume;

			public float slowPitch;

			public float frequency;

			public float fastJitter;

			public float slowJitter;
		}

		public class SoundMaster
		{
			public SoundManager manager;

			public Sound2 master;

			public List<Sound2> linked = new List<Sound2>();

			public bool isMuted;

			public override string ToString()
			{
				return ((Object)master).get_name();
			}

			public void SetSample(string sampleName)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].SetSample(manager.GetSample(sampleName));
				}
			}

			public void SetUseMaster(bool useMaster)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].useMaster = useMaster;
				}
			}

			public void SetBaseVolume(float volume)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].SetBaseVolume(volume);
				}
			}

			public void SetBasePitch(float pitch)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].SetBasePitch(pitch);
				}
			}

			public void SetMaxDistance(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].maxDistance = value;
				}
			}

			public void SetFalloffStart(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].falloffStart = value;
				}
			}

			public void SetFalloffPower(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].falloffPower = value;
				}
			}

			public void SetSpreadNear(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].spreadNear = value;
				}
			}

			public void SetSpreadFar(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].spreadFar = value;
				}
			}

			public void SetSpatialNear(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].spatialNear = value;
				}
			}

			public void SetSpatialFar(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].spatialFar = value;
				}
			}

			public void SetLpStart(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].lpStart = value;
				}
			}

			public void SetLpPower(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].lpPower = value;
				}
			}

			internal void ApplyAttenuation()
			{
				for (int i = 0; i < linked.Count; i++)
				{
					linked[i].ApplyAttenuation();
				}
			}

			public void SetGrainSlowVolume(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					Grain grain = linked[i] as Grain;
					if (!((Object)(object)grain == (Object)null))
					{
						grain.slowVolume = value;
					}
				}
			}

			public void SetGrainSlowTune(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					Grain grain = linked[i] as Grain;
					if (!((Object)(object)grain == (Object)null))
					{
						grain.slowPitch = value;
					}
				}
			}

			public void SetGrainFrequency(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					Grain grain = linked[i] as Grain;
					if (!((Object)(object)grain == (Object)null))
					{
						grain.frequencyAtMaxIntensity = value;
					}
				}
			}

			public void SetGrainSlowJitter(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					Grain grain = linked[i] as Grain;
					if (!((Object)(object)grain == (Object)null))
					{
						grain.slowJitter = value;
					}
				}
			}

			public void SetGrainFastJitter(float value)
			{
				for (int i = 0; i < linked.Count; i++)
				{
					Grain grain = linked[i] as Grain;
					if (!((Object)(object)grain == (Object)null))
					{
						grain.fastJitter = value;
					}
				}
			}

			public void Mute(bool mute)
			{
				if (isMuted != mute)
				{
					isMuted = mute;
					for (int i = 0; i < linked.Count; i++)
					{
						linked[i].Mute(mute);
					}
				}
			}
		}

		[NonSerialized]
		public List<SoundMaster> sounds;

		[NonSerialized]
		public List<SoundGroup> groups;

		public SoundManagerPrefab storedState;

		public Dictionary<string, SoundMaster> map;

		public SoundManagerType type;

		public static SoundManager main;

		public static SoundManager level;

		public static SoundManager menu;

		public static SoundManager character;

		[NonSerialized]
		public SoundManagerState state;

		public static bool hasSolo;

		public void OnEnable()
		{
			if ((Object)(object)storedState != (Object)null)
			{
				storedState = Object.Instantiate<SoundManagerPrefab>(storedState);
				((Component)storedState).get_transform().SetParent(((Component)this).get_transform(), false);
			}
			switch (type)
			{
			case SoundManagerType.Level:
				level = this;
				break;
			case SoundManagerType.Main:
				main = this;
				break;
			case SoundManagerType.Menu:
				menu = this;
				break;
			case SoundManagerType.Character:
				character = this;
				break;
			}
			if ((Object)(object)storedState != (Object)null)
			{
				storedState.Initialize();
			}
			LoadStoredSounds();
		}

		private void Start()
		{
		}

		private void LoadStoredSounds()
		{
			if ((Object)(object)storedState == (Object)null)
			{
				return;
			}
			Sound2[] componentsInChildren = ((Component)this).GetComponentsInChildren<Sound2>(true);
			foreach (Sound2 sound in componentsInChildren)
			{
				string fullName = sound.fullName;
				SoundState soundState = storedState.GetSoundState(fullName);
				GrainState grainState = storedState.GetGrainState(fullName);
				if (soundState != null)
				{
					Deserialize(sound, soundState, pasteSample: true);
				}
				if (grainState != null)
				{
					Deserialize(sound, grainState);
				}
			}
		}

		private void LoadOverlaySounds()
		{
			if (type != SoundManagerType.Main && main.state == null)
			{
				main.LoadOverlaySounds();
			}
			CollectSounds();
			state = ReadJson(GetPath(type));
			if (state == null)
			{
				state = new SoundManagerState();
				state.Populate();
			}
			if (type != SoundManagerType.Main)
			{
				for (int i = 0; i < sounds.Count; i++)
				{
					LoadOverlaySound(sounds[i]);
				}
			}
		}

		private void CollectSounds()
		{
			Sound2[] componentsInChildren = ((Component)this).GetComponentsInChildren<Sound2>(true);
			sounds = new List<SoundMaster>();
			groups = new List<SoundGroup>();
			map = new Dictionary<string, SoundMaster>();
			foreach (Sound2 sound in componentsInChildren)
			{
				SoundGroup soundGroup = null;
				for (int j = 0; j < groups.Count; j++)
				{
					if (groups[j].name == sound.group)
					{
						soundGroup = groups[j];
						break;
					}
				}
				if (soundGroup == null)
				{
					soundGroup = new SoundGroup
					{
						name = sound.group
					};
					groups.Add(soundGroup);
				}
				string key = sound.group + ":" + ((Object)sound).get_name();
				if (!map.TryGetValue(key, out var value))
				{
					value = new SoundMaster
					{
						master = sound,
						manager = this
					};
					sounds.Add(value);
					map[key] = value;
					soundGroup.sounds.Add(value);
				}
				value.linked.Add(sound);
			}
		}

		public static SoundManagerState ReadJson(string path)
		{
			try
			{
				SoundManagerState soundManagerState = JsonUtility.FromJson<SoundManagerState>(File.ReadAllText(path));
				soundManagerState.Populate();
				return soundManagerState;
			}
			catch
			{
				return null;
			}
		}

		private void LoadOverlaySound(SoundMaster sound)
		{
			string fullName = sound.master.fullName;
			SoundState soundState = state.GetSoundState(fullName);
			GrainState grainState = state.GetGrainState(fullName);
			if (soundState == null || soundState.useMaster)
			{
				SoundState soundState2 = main.state.GetSoundState(fullName);
				if (soundState2 != null)
				{
					soundState = soundState2;
				}
				GrainState grainState2 = main.state.GetGrainState(fullName);
				if (grainState2 != null)
				{
					grainState = grainState2;
				}
			}
			if (soundState == null && (Object)(object)storedState != (Object)null)
			{
				soundState = storedState.GetSoundState(fullName);
				grainState = storedState.GetGrainState(fullName);
			}
			if (soundState != null)
			{
				Deserialize(sound, soundState, pasteSample: true);
			}
			else
			{
				sound.SetSample(sound.master.sample);
			}
			if (grainState != null)
			{
				Deserialize(sound, grainState);
			}
		}

		public SoundMaster GetMaster(Sound2 sound)
		{
			string key = sound.group + ":" + ((Object)sound).get_name();
			map.TryGetValue(key, out var value);
			return value;
		}

		public void ReapplySamples()
		{
			for (int i = 0; i < sounds.Count; i++)
			{
				sounds[i].SetSample(sounds[i].master.sample);
			}
		}

		public void Save(bool saveMaster)
		{
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Expected O, but got Unknown
			List<SoundState> list = new List<SoundState>();
			List<GrainState> list2 = new List<GrainState>();
			if (type == SoundManagerType.Main)
			{
				list = state.soundStateMap.Values.ToList();
				list2 = state.grainStateMap.Values.ToList();
			}
			else
			{
				for (int i = 0; i < sounds.Count; i++)
				{
					Sound2 master = sounds[i].master;
					Grain grain = master as Grain;
					if (master.useMaster && saveMaster)
					{
						main.state.soundStateMap[master.fullName] = Serialize(sounds[i]);
						if ((Object)(object)grain != (Object)null)
						{
							main.state.grainStateMap[master.fullName] = Serialize(grain);
						}
					}
					else
					{
						list.Add(Serialize(sounds[i]));
						if ((Object)(object)grain != (Object)null)
						{
							list2.Add(Serialize(grain));
						}
					}
				}
			}
			string contents = JsonUtility.ToJson((object)new SoundManagerState
			{
				sounds = list.ToArray(),
				grains = list2.ToArray()
			}, true);
			string path = GetPath(type);
			Provider.Checkout(new Asset(path), (CheckoutMode)1).Wait();
			File.WriteAllText(path, contents);
			if (saveMaster)
			{
				main.Save(saveMaster: false);
			}
		}

		public static string GetPath(SoundManagerType type)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			string text;
			switch (type)
			{
			case SoundManagerType.Main:
				text = "Main";
				break;
			case SoundManagerType.Menu:
				text = "Menu";
				break;
			case SoundManagerType.Character:
				text = "Character";
				break;
			default:
			{
				Scene activeScene = SceneManager.GetActiveScene();
				text = ((Scene)(ref activeScene)).get_name();
				break;
			}
			}
			return "Audio/" + text + "Sounds.txt";
		}

		public static GrainState Serialize(Grain grain)
		{
			return new GrainState
			{
				id = grain.group + ":" + ((Object)grain).get_name(),
				slowVolume = grain.slowVolume,
				slowPitch = grain.slowPitch,
				frequency = grain.frequencyAtMaxIntensity,
				fastJitter = grain.fastJitter,
				slowJitter = grain.slowJitter
			};
		}

		public static void Deserialize(SoundMaster master, GrainState grainState)
		{
			master.SetGrainSlowVolume(grainState.slowVolume);
			master.SetGrainSlowTune(grainState.slowPitch);
			master.SetGrainFrequency(grainState.frequency);
			master.SetGrainFastJitter(grainState.fastJitter);
			master.SetGrainSlowJitter(grainState.slowJitter);
		}

		public void Deserialize(Sound2 sound, GrainState grainState)
		{
			Grain grain = sound as Grain;
			if (!((Object)(object)grain == (Object)null))
			{
				grain.slowVolume = grainState.slowVolume;
				grain.slowPitch = grainState.slowPitch;
				grain.frequencyAtMaxIntensity = grainState.frequency;
				grain.fastJitter = grainState.fastJitter;
				grain.slowJitter = grainState.slowJitter;
			}
		}

		public static SoundState Serialize(SoundMaster master)
		{
			Sound2 master2 = master.master;
			return new SoundState
			{
				id = master2.group + ":" + ((Object)master2).get_name(),
				useMaster = master2.useMaster,
				sample = master2.sample,
				volume = master2.baseVolume,
				pitch = master2.basePitch,
				maxDistance = master2.maxDistance,
				falloffStart = master2.falloffStart,
				falloffPower = master2.falloffPower,
				lpStart = master2.lpStart,
				lpPower = master2.lpPower,
				spreadNear = master2.spreadNear,
				spreadFar = master2.spreadFar,
				spatialNear = master2.spatialNear,
				spatialFar = master2.spatialFar
			};
		}

		public static void Deserialize(SoundMaster master, SoundState soundState, bool pasteSample)
		{
			if (pasteSample)
			{
				master.SetSample(soundState.sample);
			}
			master.SetUseMaster(soundState.useMaster);
			master.SetBasePitch(soundState.pitch);
			master.SetBaseVolume(soundState.volume);
			master.SetMaxDistance(soundState.maxDistance);
			master.SetFalloffStart(soundState.falloffStart);
			master.SetFalloffPower(soundState.falloffPower);
			master.SetLpStart(soundState.lpStart);
			master.SetLpPower(soundState.lpPower);
			master.SetSpreadNear(soundState.spreadNear);
			master.SetSpreadFar(soundState.spreadFar);
			master.SetSpatialNear(soundState.spatialNear);
			master.SetSpatialFar(soundState.spatialFar);
			master.ApplyAttenuation();
		}

		public void Deserialize(Sound2 sound, SoundState soundState, bool pasteSample)
		{
			if (pasteSample)
			{
				sound.SetSample(GetSample(soundState.sample));
			}
			sound.useMaster = soundState.useMaster;
			sound.basePitch = soundState.pitch;
			sound.baseVolume = soundState.volume;
			sound.maxDistance = soundState.maxDistance;
			sound.falloffStart = soundState.falloffStart;
			sound.falloffPower = soundState.falloffPower;
			sound.lpStart = soundState.lpStart;
			sound.lpPower = soundState.lpPower;
			sound.spreadNear = soundState.spreadNear;
			sound.spreadFar = soundState.spreadFar;
			sound.spatialNear = soundState.spatialNear;
			sound.spatialFar = soundState.spatialFar;
			sound.ApplyAttenuation();
		}

		public static void SoloSound(SoundMaster master, bool solo)
		{
			hasSolo = solo;
			if (solo)
			{
				for (int i = 0; i < main.sounds.Count; i++)
				{
					main.sounds[i].Mute(mute: true);
				}
				if ((Object)(object)level != (Object)null)
				{
					for (int j = 0; j < level.sounds.Count; j++)
					{
						level.sounds[j].Mute(mute: true);
					}
				}
				if ((Object)(object)menu != (Object)null)
				{
					for (int k = 0; k < menu.sounds.Count; k++)
					{
						menu.sounds[k].Mute(mute: true);
					}
				}
				if ((Object)(object)character != (Object)null)
				{
					for (int l = 0; l < character.sounds.Count; l++)
					{
						character.sounds[l].Mute(mute: true);
					}
				}
				master.Mute(mute: false);
				return;
			}
			for (int m = 0; m < main.sounds.Count; m++)
			{
				main.sounds[m].Mute(mute: false);
			}
			if ((Object)(object)level != (Object)null)
			{
				for (int n = 0; n < level.sounds.Count; n++)
				{
					level.sounds[n].Mute(mute: false);
				}
			}
			if ((Object)(object)menu != (Object)null)
			{
				for (int num = 0; num < menu.sounds.Count; num++)
				{
					menu.sounds[num].Mute(mute: false);
				}
			}
			if ((Object)(object)character != (Object)null)
			{
				for (int num2 = 0; num2 < character.sounds.Count; num2++)
				{
					character.sounds[num2].Mute(mute: false);
				}
			}
		}

		private SoundLibrary.SerializedSample GetSample(string sampleName)
		{
			SoundLibrary.SerializedSample serializedSample = SoundLibrary.GetSample(sampleName);
			if (serializedSample == null && (Object)(object)storedState != (Object)null)
			{
				SoundLibrarySample sample = storedState.GetSample(sampleName);
				if ((Object)(object)sample != (Object)null)
				{
					serializedSample = sample.GetSerialized();
				}
			}
			return serializedSample;
		}

		public void RefreshSampleParameters(SoundLibrary.SerializedSample sample)
		{
			for (int i = 0; i < sounds.Count; i++)
			{
				SoundMaster soundMaster = sounds[i];
				if (soundMaster.master.soundSample != null && soundMaster.master.soundSample.name == sample.name)
				{
					for (int j = 0; j < soundMaster.linked.Count; j++)
					{
						soundMaster.linked[j].RefreshSampleParameters();
					}
				}
			}
		}

		public SoundManager()
			: this()
		{
		}
	}
}

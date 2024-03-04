using System;
using System.IO;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HumanAPI
{
	public class Reverb : MonoBehaviour
	{
		[Serializable]
		public class ReverbState
		{
			public ReverbZoneState[] zones;
		}

		[Serializable]
		public class ReverbZoneState
		{
			public string id;

			public float level;

			public float delay = 0.5f;

			public float diffusion = 0.5f;

			public float lowPass = 22000f;

			public float highPass = 10f;
		}

		public static Reverb instance;

		[NonSerialized]
		public ReverbZone[] zones;

		public void OnEnable()
		{
			instance = this;
			zones = ((Component)this).GetComponentsInChildren<ReverbZone>();
		}

		private void Start()
		{
			SoundManager componentInParent = ((Component)this).GetComponentInParent<SoundManager>();
			if ((Object)(object)componentInParent != (Object)null && (Object)(object)componentInParent.storedState != (Object)null && componentInParent.storedState.reverb != null)
			{
				LoadJson(componentInParent.storedState.reverb);
			}
		}

		public static string GetPath()
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			Scene activeScene = SceneManager.GetActiveScene();
			string name = ((Scene)(ref activeScene)).get_name();
			return "Audio/" + name + "Reverb.txt";
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
			ReverbState state = JsonUtility.FromJson<ReverbState>(json);
			LoadJson(state);
		}

		private void LoadJson(ReverbState state)
		{
			for (int i = 0; i < state.zones.Length; i++)
			{
				ReverbZoneState reverbZoneState = state.zones[i];
				ReverbZone reverbZone = null;
				for (int j = 0; j < zones.Length; j++)
				{
					if (((Object)zones[j]).get_name() == reverbZoneState.id)
					{
						reverbZone = zones[j];
						break;
					}
				}
				if (!((Object)(object)reverbZone == (Object)null))
				{
					Deserialize(reverbZone, reverbZoneState);
				}
			}
		}

		public void Save()
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Expected O, but got Unknown
			ReverbState reverbState = new ReverbState
			{
				zones = new ReverbZoneState[zones.Length]
			};
			for (int i = 0; i < zones.Length; i++)
			{
				reverbState.zones[i] = Serialize(zones[i]);
			}
			string contents = JsonUtility.ToJson((object)reverbState, true);
			string path = GetPath();
			Provider.Checkout(new Asset(path), (CheckoutMode)1).Wait();
			File.WriteAllText(path, contents);
		}

		public static ReverbZoneState Serialize(ReverbZone zone)
		{
			return new ReverbZoneState
			{
				id = ((Object)zone).get_name(),
				level = zone.level,
				delay = zone.delay,
				diffusion = zone.diffusion,
				lowPass = zone.lowPass,
				highPass = zone.highPass
			};
		}

		public static void Deserialize(ReverbZone zone, ReverbZoneState reverbState)
		{
			zone.level = reverbState.level;
			zone.delay = reverbState.delay;
			zone.diffusion = reverbState.diffusion;
			zone.lowPass = reverbState.lowPass;
			zone.highPass = reverbState.highPass;
		}

		public Reverb()
			: this()
		{
		}
	}
}

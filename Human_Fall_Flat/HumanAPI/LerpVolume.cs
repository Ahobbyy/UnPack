using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Lerp Volume", 10)]
	public class LerpVolume : LerpBase
	{
		[Tooltip("Minimum Value to output")]
		public float from;

		[Tooltip("Maximum value to output")]
		public float to;

		private Sound2 sound;

		private AudioSource unitySound;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		protected override void Awake()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Lerp Volume setting vars "));
			}
			sound = ((Component)this).GetComponent<Sound2>();
			if ((Object)(object)sound == (Object)null)
			{
				unitySound = ((Component)this).GetComponent<AudioSource>();
			}
			base.Awake();
		}

		protected override void ApplyValue(float value)
		{
			if ((Object)(object)sound != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Sound Lerp "));
				}
				float volume = Mathf.Lerp(AudioUtils.DBToValue(from), AudioUtils.DBToValue(to), value);
				sound.SetVolume(volume);
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Current Volume = " + volume));
				}
			}
			else if ((Object)(object)unitySound != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Audio Source Lerp "));
				}
				float volume2 = Mathf.Lerp(AudioUtils.DBToValue(from), AudioUtils.DBToValue(to), value);
				unitySound.set_volume(volume2);
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Current Volume = " + volume2));
				}
			}
		}
	}
}

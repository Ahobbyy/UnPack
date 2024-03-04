using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Lerp Volume", 10)]
	public class LerpVolumeAudioSource : LerpBase
	{
		public float from;

		public float to = 1f;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private float currentVolume;

		private AudioSource sound;

		protected override void Awake()
		{
			sound = ((Component)this).GetComponent<AudioSource>();
			if ((Object)(object)sound != (Object)null)
			{
				currentVolume = sound.get_volume();
				if (showDebug && (Object)(object)sound != (Object)null)
				{
					Debug.Log((object)(((Object)this).get_name() + " Found something to play "));
				}
			}
			base.Awake();
		}

		protected override void ApplyValue(float value)
		{
			if (!((Object)(object)sound != (Object)null))
			{
				return;
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Value coming in is = " + value));
			}
			if (!sound.get_isPlaying())
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " The Sound is not Playing "));
				}
				sound.Play();
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Setting Volume to " + value));
			}
			currentVolume = value;
			sound.set_volume(currentVolume);
		}
	}
}

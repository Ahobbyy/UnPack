using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Lerp Pitch", 10)]
	public class LerpPitch : LerpBase
	{
		public float from;

		public float to;

		private Sound2 sound;

		private AudioSource unitySound;

		protected override void Awake()
		{
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
				float pitch = Mathf.Lerp(AudioUtils.CentsToRatio(from), AudioUtils.CentsToRatio(to), value);
				sound.SetPitch(pitch);
			}
			else if ((Object)(object)unitySound != (Object)null)
			{
				float pitch2 = Mathf.Lerp(AudioUtils.CentsToRatio(from), AudioUtils.CentsToRatio(to), value);
				unitySound.set_pitch(pitch2);
			}
		}
	}
}

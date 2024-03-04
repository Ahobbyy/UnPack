using UnityEngine;

namespace HumanAPI
{
	public class SoundControlNode : Node
	{
		public NodeInput input;

		public Sound2 sound;

		public AudioSource unitySound;

		public bool affectVolume = true;

		public float volumeMin;

		public float volumeMax = 1f;

		public bool affectLowPassFilter;

		public float lowPassMin = 22000f;

		public float lowPassMax = 22000f;

		public bool affectPitch;

		public float pitchMin = 0.5f;

		public float pitchMax = 1.5f;

		private void Awake()
		{
			if ((Object)(object)sound == (Object)null)
			{
				sound = ((Component)this).GetComponent<Sound2>();
			}
			else if ((Object)(object)unitySound == (Object)null)
			{
				unitySound = ((Component)this).GetComponentInChildren<AudioSource>();
			}
		}

		private void Update()
		{
			Process();
		}

		public override void Process()
		{
			base.Process();
			if ((Object)(object)sound != (Object)null)
			{
				if (affectVolume)
				{
					float volume = Mathf.Lerp(volumeMin, volumeMax, input.value);
					sound.SetVolume(volume);
				}
				if (affectLowPassFilter)
				{
					float lowPass = Mathf.Lerp(lowPassMin, lowPassMax, input.value);
					sound.SetLowPass(lowPass);
				}
				if (affectPitch)
				{
					float pitch = Mathf.Lerp(pitchMin, pitchMax, input.value);
					sound.SetPitch(pitch);
				}
			}
			else if ((Object)(object)unitySound != (Object)null)
			{
				if (affectVolume)
				{
					float volume2 = Mathf.Lerp(volumeMin, volumeMax, input.value);
					unitySound.set_volume(volume2);
				}
				if (affectLowPassFilter)
				{
					float cutoffFrequency = Mathf.Lerp(lowPassMin, lowPassMax, input.value);
					((Component)unitySound).GetComponent<AudioLowPassFilter>().set_cutoffFrequency(cutoffFrequency);
				}
				if (affectPitch)
				{
					float pitch2 = Mathf.Lerp(pitchMin, pitchMax, input.value);
					unitySound.set_pitch(pitch2);
				}
			}
		}
	}
}

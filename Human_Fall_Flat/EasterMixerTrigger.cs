using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public sealed class EasterMixerTrigger : MonoBehaviour, IReset
{
	[SerializeField]
	private float duckTime;

	[SerializeField]
	private AudioMixer masterMixer;

	private bool isDucking;

	private float originalMusicVolume;

	private float originalEffectsVolume;

	private const float kDuckVolumeDownSpeed = 0.03f;

	private const int kDuckStepsDown = 30;

	private const float kDuckVolumeUpSpeed = 0.03f;

	private const int kDuckStepsUp = 31;

	private void Awake()
	{
		GetCurrentVolumes();
	}

	private void GetCurrentVolumes()
	{
		masterMixer.GetFloat("MusicVolume", ref originalMusicVolume);
		masterMixer.GetFloat("EffectsVolume", ref originalEffectsVolume);
	}

	private void SetVolumes(float adjustVolume = 0f)
	{
		masterMixer.SetFloat("MusicVolume", originalMusicVolume + adjustVolume);
		masterMixer.SetFloat("EffectsVolume", originalEffectsVolume + adjustVolume);
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)((Component)other).GetComponent<HumanHead>() != (Object)null && !isDucking)
		{
			GetCurrentVolumes();
			((MonoBehaviour)this).StartCoroutine(DuckMusic());
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (isDucking)
		{
			SetVolumes();
		}
	}

	private IEnumerator DuckMusic()
	{
		isDucking = true;
		for (int j = 0; j < 30; j++)
		{
			yield return (object)new WaitForSeconds(0.03f);
			SetVolumes(-j / 2);
		}
		yield return (object)new WaitForSeconds(duckTime);
		for (int j = 1; j < 31; j++)
		{
			yield return (object)new WaitForSeconds(0.03f);
			SetVolumes(-15 + j / 2);
		}
		isDucking = false;
	}

	public EasterMixerTrigger()
		: this()
	{
	}
}

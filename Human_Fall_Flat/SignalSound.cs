using HumanAPI;
using UnityEngine;

public class SignalSound : MonoBehaviour
{
	public SignalBase triggerSignal;

	private bool isOn;

	public Sound2 onSound;

	public Sound2 offSound;

	public AudioClip onClip;

	public AudioClip offClip;

	public float onVolume = 1f;

	public float offVolume = 1f;

	public float minDelay = 0.2f;

	private float lastSoundTime;

	public bool playOnOff = true;

	private void OnEnable()
	{
		isOn = triggerSignal.boolValue;
		triggerSignal.onValueChanged += SignalChanged;
	}

	private void OnDisable()
	{
		triggerSignal.onValueChanged -= SignalChanged;
	}

	private void SignalChanged(float val)
	{
		if (triggerSignal.boolValue == isOn)
		{
			return;
		}
		isOn = triggerSignal.boolValue;
		float time = Time.get_time();
		if (lastSoundTime + minDelay > time)
		{
			return;
		}
		if (isOn && ((Object)(object)onClip != (Object)null || (Object)(object)onSound != (Object)null))
		{
			if ((Object)(object)onSound != (Object)null)
			{
				onSound.PlayOneShot();
			}
			else
			{
				((Component)this).GetComponent<AudioSource>().PlayOneShot(onClip, onVolume);
			}
			lastSoundTime = time;
		}
		else if (playOnOff && !isOn && ((Object)(object)offClip != (Object)null || (Object)(object)onClip != (Object)null || (Object)(object)offSound != (Object)null || (Object)(object)onSound != (Object)null))
		{
			if ((Object)(object)offSound != (Object)null)
			{
				offSound.PlayOneShot();
			}
			else if ((Object)(object)onSound != (Object)null)
			{
				onSound.PlayOneShot();
			}
			else
			{
				((Component)this).GetComponent<AudioSource>().PlayOneShot(((Object)(object)offClip != (Object)null) ? offClip : onClip, offVolume);
			}
			lastSoundTime = time;
		}
	}

	public SignalSound()
		: this()
	{
	}
}

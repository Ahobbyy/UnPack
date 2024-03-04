using System;
using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	public class Sound2 : MonoBehaviour
	{
		public string group;

		public string sample;

		public SoundLibrarySample localSample;

		internal SoundLibrary.SerializedSample soundSample;

		public bool useMaster;

		public char currentChoice = 'A';

		public bool startOnLoad;

		public AudioChannel channel = AudioChannel.Effects;

		public float rtVolume = 1f;

		public float rtPitch = 1f;

		public float baseVolume = 1f;

		public float basePitch = 1f;

		[NonSerialized]
		private float mute = 1f;

		public float maxDistance = 30f;

		public float falloffStart = 1f;

		public float falloffPower = 0.5f;

		public float lpStart = 2f;

		public float lpPower = 0.5f;

		public float spreadNear = 1f;

		public float spreadFar;

		public float spatialNear = 0.5f;

		public float spatialFar = 1f;

		private bool isSourcePaused;

		private AudioSource activeSource;

		private AudioSource inactiveSource;

		internal SoundLibrary.SerializedClip activeClip;

		private SoundLibrary.SerializedClip inactiveClip;

		private SoundLibrary.SerializedClip queuedClip;

		private float activeVolume;

		private float inactiveVolume;

		private float activePitch;

		private float inactivePitch;

		private bool activeLoop;

		private bool fadeQueued;

		private float fadeInTime;

		private float fadeInDelay;

		private float fadeInDuration;

		private float fadeOutTime;

		private float fadeOutDuration;

		private float pauseVol = 1f;

		private float pauseSpeed;

		private SoundLibrary.SerializedClip currentClip;

		private float lowPass = 22000f;

		public string fullName => group + ":" + ((Object)this).get_name();

		public string sampleLabel
		{
			get
			{
				if (soundSample == null)
				{
					return "MISS:" + sample;
				}
				return soundSample.category + ":" + sample;
			}
		}

		public bool isPlaying
		{
			get
			{
				if (activeClip == null || ((Object)(object)activeSource == (Object)null && !fadeQueued))
				{
					return false;
				}
				if (isSourcePaused)
				{
					return true;
				}
				if (fadeInDuration != 0f || fadeInDelay != 0f)
				{
					return true;
				}
				if (activeSource.get_loop())
				{
					return true;
				}
				if (activeSource.get_isPlaying() && (Object)(object)activeSource.get_clip() != (Object)null && activeSource.get_clip().get_length() > 10f)
				{
					return true;
				}
				return false;
			}
		}

		private bool processUpdate
		{
			get
			{
				if (fadeInDelay == 0f && fadeInDuration == 0f && fadeOutDuration == 0f)
				{
					return pauseSpeed != 0f;
				}
				return true;
			}
		}

		private void OnDestroy()
		{
			if (soundSample != null)
			{
				soundSample.Unsubscribe(OnSampleLoaded);
			}
		}

		protected virtual void OnSampleLoaded()
		{
			if (isPlaying)
			{
				SoundLibrary.SerializedClip clip = soundSample.GetClip(currentClip, currentChoice, SoundLibrary.SampleContainerChildType.Loop);
				if (clip != null)
				{
					CrossfadeSound(clip, loop: true, 0f, 0.1f);
				}
			}
			else if (startOnLoad)
			{
				Play(forceLoop: true);
			}
		}

		public void SetSample(SoundLibrary.SerializedSample sample)
		{
			if (soundSample != null)
			{
				soundSample.Unsubscribe(OnSampleLoaded);
			}
			this.sample = sample?.name;
			soundSample = sample;
			if (soundSample != null)
			{
				soundSample.Subscribe(OnSampleLoaded);
			}
		}

		public void RefreshSampleParameters()
		{
			if (soundSample != null)
			{
				if (activeClip != null)
				{
					activeVolume = activeClip.volume * soundSample.volume;
					activePitch = activeClip.pitch * soundSample.pitch;
				}
				if (inactiveClip != null)
				{
					inactiveVolume = inactiveClip.volume * soundSample.volume;
					inactivePitch = inactiveClip.pitch * soundSample.pitch;
				}
				ApplyPitch();
				ApplyVolume();
			}
		}

		public void PlayOneShot(float volume = 1f, float pitch = 1f, float delay = 0f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			PlayOneShot(Vector3.get_zero(), volume, pitch, delay);
		}

		public void PlayOneShot(Vector3 pos, float volume = 1f, float pitch = 1f, float delay = 0f)
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (soundSample != null)
			{
				SoundLibrary.SerializedClip clip = soundSample.GetClip(null, currentChoice, SoundLibrary.SampleContainerChildType.None);
				if (clip != null)
				{
					PlayOneShot(clip, pos, volume, pitch, delay);
				}
			}
		}

		public void Switch(char choice, bool crossfade = true)
		{
			if (currentChoice == choice)
			{
				return;
			}
			currentChoice = choice;
			if (soundSample != null && isPlaying && crossfade)
			{
				SoundLibrary.SerializedClip serializedClip = (currentClip = soundSample.GetClip(currentClip, currentChoice, SoundLibrary.SampleContainerChildType.Loop));
				if (serializedClip != null)
				{
					CrossfadeSound(serializedClip, loop: true, 0f, soundSample.crossFade);
				}
			}
		}

		public void Play(bool forceLoop = false)
		{
			startOnLoad = true;
			if (soundSample == null || !soundSample.loaded)
			{
				return;
			}
			SoundLibrary.SerializedClip serializedClip = (currentClip = soundSample.GetClip(null, currentChoice, SoundLibrary.SampleContainerChildType.Start));
			if (currentClip != null)
			{
				PlaySound(currentClip, forceLoop && !soundSample.isLoop);
			}
			if (forceLoop || soundSample.isLoop)
			{
				serializedClip = soundSample.GetClip(currentClip, currentChoice, SoundLibrary.SampleContainerChildType.Loop);
				if (serializedClip != null)
				{
					CrossfadeSound(serializedClip, loop: true, 0f, soundSample.crossFade);
				}
			}
		}

		public void Stop()
		{
			startOnLoad = false;
			if (soundSample != null)
			{
				SoundLibrary.SerializedClip clip = soundSample.GetClip(currentClip, currentChoice, SoundLibrary.SampleContainerChildType.Stop);
				if (clip != null)
				{
					CrossfadeSound(clip, loop: false, 0f, soundSample.crossFade);
				}
				else
				{
					FadeOut(soundSample.crossFade);
				}
			}
		}

		public void PlayClip(AudioClip clip, float volume, float pitch, bool loop)
		{
			soundSample = new SoundLibrary.SerializedSample
			{
				pB = 1f,
				vB = 1f
			};
			PlaySound(new SoundLibrary.SerializedClip
			{
				clip = clip,
				pB = pitch,
				vB = volume
			}, loop);
		}

		public void PlaySound(SoundLibrary.SerializedClip clip, bool loop)
		{
			FadeIn(clip, loop, 0f, 0f);
		}

		public void CrossfadeClip(AudioClip clip, float volume, float pitch, bool loop, float after, float duration, float outDuration, float inDuration)
		{
			soundSample = new SoundLibrary.SerializedSample
			{
				pB = 1f,
				vB = 1f
			};
			CrossfadeSound(new SoundLibrary.SerializedClip
			{
				clip = clip,
				pB = pitch,
				vB = volume
			}, loop, after, duration, outDuration, inDuration);
		}

		public void CrossfadeSound(SoundLibrary.SerializedClip clip, bool loop, float after, float duration)
		{
			FadeOut(duration);
			FadeIn(clip, loop, 0f, duration);
		}

		public void CrossfadeSound(SoundLibrary.SerializedClip clip, bool loop, float after, float duration, float outDuration, float inDuration)
		{
			FadeOut(outDuration);
			FadeIn(clip, loop, duration - inDuration, inDuration);
		}

		public void PlayOneShot(SoundLibrary.SerializedClip clip, Vector3 pos, float volume = 1f, float pitch = 1f, float delay = 0f)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			AudioSource source = AcquireAudioSource(pos);
			source.set_clip(clip.clip);
			source.set_volume(volume * clip.volume * soundSample.volume * baseVolume * rtVolume * pauseVol * mute);
			source.set_pitch(pitch * clip.pitch * soundSample.pitch * basePitch * rtPitch);
			source.set_loop(false);
			if (delay == 0f)
			{
				source.Play();
			}
			else
			{
				source.PlayDelayed(delay);
			}
			ReleaseAudioSource(ref source, waitToFinish: true);
		}

		public AudioSource GetActiveSource()
		{
			return activeSource;
		}

		public void FadeOut(float duration)
		{
			if (activeClip != null)
			{
				if (fadeQueued)
				{
					if (activeClip != null)
					{
						OnClipDeactivated(activeClip);
					}
					fadeQueued = false;
					activeClip = null;
				}
				else
				{
					if (inactiveClip != null)
					{
						ReleaseAudioSource(ref inactiveSource, waitToFinish: false);
					}
					if (inactiveClip != null)
					{
						OnClipDeactivated(inactiveClip);
					}
					inactiveSource = activeSource;
					inactiveClip = activeClip;
					inactivePitch = activePitch;
					inactiveVolume = activeVolume;
					activeClip = null;
					activeSource = null;
					if (fadeInDuration > 0f)
					{
						inactiveVolume *= fadeInTime / fadeInDuration;
					}
					fadeOutTime = 0f;
					fadeOutDuration = duration;
				}
			}
			if (inactiveClip != null)
			{
				if (duration == 0f)
				{
					ReleaseAudioSource(ref inactiveSource, waitToFinish: false);
					OnClipDeactivated(inactiveClip);
					inactiveClip = null;
				}
				else if (fadeOutDuration - fadeOutTime > duration)
				{
					inactiveVolume *= 1f - fadeOutTime / duration;
					fadeOutTime = 0f;
					fadeOutDuration = duration;
				}
			}
			fadeInDelay = 0f;
			fadeInDuration = 0f;
			fadeInTime = 0f;
		}

		public virtual void OnClipDeactivated(SoundLibrary.SerializedClip clip)
		{
		}

		public virtual void OnClipActivating(SoundLibrary.SerializedClip clip)
		{
		}

		public void FadeIn(SoundLibrary.SerializedClip clip, bool loop, float after, float duration)
		{
			if (activeClip != null)
			{
				OnClipDeactivated(activeClip);
			}
			fadeQueued = true;
			activeClip = clip;
			OnClipActivating(activeClip);
			activeVolume = clip.volume * soundSample.volume;
			activePitch = clip.pitch * soundSample.pitch;
			activeLoop = loop;
			fadeInDelay = after;
			fadeInDuration = duration;
			fadeInTime = 0f;
			HandleFadeIn();
		}

		private void HandleFadeIn()
		{
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			fadeInTime += Time.get_unscaledDeltaTime();
			if (fadeQueued && fadeInDelay <= fadeInTime)
			{
				fadeQueued = false;
				fadeInTime -= fadeInDelay;
				fadeInDelay = 0f;
				float num = ((fadeInDuration == 0f) ? 1f : (fadeInTime / fadeInDuration));
				if (num >= 1f)
				{
					fadeInDuration = 0f;
					num = 1f;
				}
				if ((Object)(object)activeSource == (Object)null)
				{
					activeSource = AcquireAudioSource(Vector3.get_zero());
				}
				activeSource.set_clip(activeClip.clip);
				activeSource.set_volume(Mathf.Sqrt(num) * activeVolume * baseVolume * rtVolume * Mathf.Sqrt(pauseVol) * mute);
				activeSource.set_pitch(activePitch * basePitch * rtPitch);
				activeSource.set_loop(activeLoop);
				activeSource.Play();
			}
			else if ((Object)(object)activeSource != (Object)null)
			{
				float num2 = ((fadeInDuration == 0f) ? 1f : (fadeInTime / fadeInDuration));
				if (num2 >= 1f)
				{
					fadeInDuration = 0f;
					num2 = 1f;
				}
				activeSource.set_volume(Mathf.Sqrt(num2) * activeVolume * baseVolume * rtVolume * Mathf.Sqrt(pauseVol) * mute);
				activeSource.set_pitch(activePitch * basePitch * rtPitch);
			}
		}

		private void HandleFadeOut()
		{
			fadeOutTime += Time.get_unscaledDeltaTime();
			if ((Object)(object)inactiveSource == (Object)null)
			{
				return;
			}
			float num = ((fadeOutDuration == 0f) ? 1f : (fadeOutTime / fadeOutDuration));
			if (num >= 1f)
			{
				fadeInDuration = 0f;
				num = 1f;
				ReleaseAudioSource(ref inactiveSource, waitToFinish: false);
				if (inactiveClip != null)
				{
					OnClipDeactivated(inactiveClip);
				}
				inactiveClip = null;
			}
			else
			{
				inactiveSource.set_volume(Mathf.Sqrt(1f - num) * inactiveVolume * baseVolume * rtVolume * Mathf.Sqrt(pauseVol) * mute);
				inactiveSource.set_pitch(inactivePitch * basePitch * rtPitch);
			}
		}

		public void Pause()
		{
			pauseSpeed = -1f;
		}

		public void Resume()
		{
			pauseSpeed = 0.333333343f;
			if (pauseVol == 0f && isSourcePaused)
			{
				isSourcePaused = false;
				activeSource.UnPause();
			}
		}

		private void HandlePause()
		{
			if (pauseSpeed == 0f)
			{
				return;
			}
			pauseVol = Mathf.Clamp01(pauseVol + pauseSpeed * Time.get_unscaledDeltaTime());
			if (pauseVol == 0f)
			{
				ReleaseAudioSource(ref inactiveSource, waitToFinish: false);
				if (inactiveClip != null)
				{
					OnClipDeactivated(inactiveClip);
				}
				inactiveClip = null;
				if ((Object)(object)activeSource != (Object)null && activeSource.get_isPlaying())
				{
					activeSource.Pause();
					isSourcePaused = true;
				}
				pauseSpeed = 0f;
			}
			else if (pauseVol == 1f)
			{
				pauseSpeed = 0f;
			}
		}

		public void StopSound()
		{
			if (((Object)this).get_name() == "Underwater")
			{
				((Object)this).set_name("Underwater");
			}
			FadeOut(0f);
		}

		protected virtual void Update()
		{
			if (processUpdate)
			{
				HandlePause();
				HandleFadeIn();
				HandleFadeOut();
			}
		}

		public void Mute(bool mute)
		{
			this.mute = ((!mute) ? 1 : 0);
			ApplyVolume();
		}

		public void SetPitch(float newPitch)
		{
			if (rtPitch != newPitch)
			{
				rtPitch = newPitch;
				ApplyPitch();
			}
		}

		public void SetBasePitch(float newPitch)
		{
			if (basePitch != newPitch)
			{
				basePitch = newPitch;
				ApplyPitch();
			}
		}

		private void ApplyPitch()
		{
			if (!processUpdate)
			{
				if ((Object)(object)activeSource != (Object)null)
				{
					activeSource.set_pitch(activePitch * basePitch * rtPitch);
				}
				if ((Object)(object)inactiveSource != (Object)null)
				{
					inactiveSource.set_pitch(inactivePitch * basePitch * rtPitch);
				}
			}
		}

		public void SetVolume(float newVolume)
		{
			if (rtVolume != newVolume)
			{
				rtVolume = newVolume;
				ApplyVolume();
			}
		}

		public virtual void SetBaseVolume(float newVolume)
		{
			if (baseVolume != newVolume)
			{
				baseVolume = newVolume;
				ApplyVolume();
			}
		}

		private void ApplyVolume()
		{
			if (!processUpdate)
			{
				if ((Object)(object)activeSource != (Object)null)
				{
					activeSource.set_volume(1f * activeVolume * baseVolume * rtVolume * Mathf.Sqrt(pauseVol) * mute);
				}
				if ((Object)(object)inactiveSource != (Object)null)
				{
					inactiveSource.set_volume(0f * inactiveVolume * baseVolume * rtVolume * Mathf.Sqrt(pauseVol) * mute);
				}
			}
		}

		internal void SetLowPass(float frequency)
		{
			lowPass = frequency;
			SetLowPass(activeSource, lowPass);
			SetLowPass(activeSource, lowPass);
		}

		private void SetLowPass(AudioSource source, float frequency)
		{
			if ((Object)(object)source != (Object)null)
			{
				((Component)source).GetComponent<AudioLowPassFilter>().set_cutoffFrequency(frequency);
			}
		}

		private AudioSource AcquireAudioSource(Vector3 pos)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			AudioSource audioSource = SoundSourcePool.instance.GetAudioSource();
			if (pos == Vector3.get_zero())
			{
				((Component)audioSource).get_transform().SetParent(((Component)this).get_transform(), false);
				((Component)audioSource).get_transform().set_localPosition(Vector3.get_zero());
			}
			else
			{
				((Component)audioSource).get_transform().SetParent((Transform)null, false);
				((Component)audioSource).get_transform().set_localPosition(pos);
			}
			audioSource.set_outputAudioMixerGroup(AudioRouting.GetChannel(channel));
			ApplyAttenuation(audioSource);
			if (lowPass < 22000f)
			{
				SetLowPass(audioSource, lowPass);
			}
			return audioSource;
		}

		private void ReleaseAudioSource(ref AudioSource source, bool waitToFinish)
		{
			if (!((Object)(object)source != (Object)null))
			{
				return;
			}
			if (source.get_isPlaying())
			{
				if (waitToFinish && !source.get_loop())
				{
					SoundSourcePool.instance.ReleaseAudioSourceOnComplete(source);
				}
				else
				{
					source.Stop();
					SoundSourcePool.instance.ReleaseAudioSource(source);
				}
			}
			else
			{
				SoundSourcePool.instance.ReleaseAudioSource(source);
			}
			source = null;
		}

		public void ApplyAttenuation()
		{
			if ((Object)(object)activeSource != (Object)null)
			{
				ApplyAttenuation(activeSource);
			}
			if ((Object)(object)inactiveSource != (Object)null)
			{
				ApplyAttenuation(inactiveSource);
			}
		}

		private void ApplyAttenuation(AudioSource source)
		{
			source.set_maxDistance(maxDistance);
			source.set_rolloffMode((AudioRolloffMode)2);
			source.SetCustomCurve((AudioSourceCurveType)0, CalculateAttenuation.VolumeFalloffFromTo(1f, 0f, falloffStart, source.get_maxDistance(), falloffPower));
			source.SetCustomCurve((AudioSourceCurveType)3, CalculateAttenuation.VolumeFalloffFromTo(spreadNear / 2f, spreadFar / 2f, falloffStart, source.get_maxDistance(), falloffPower));
			source.SetCustomCurve((AudioSourceCurveType)1, CalculateAttenuation.VolumeFalloffFromTo(spatialNear, (spatialNear != 0f) ? 1 : 0, falloffStart, source.get_maxDistance(), falloffPower));
			AudioLowPassFilter component = ((Component)source).GetComponent<AudioLowPassFilter>();
			if ((Object)(object)component != (Object)null)
			{
				component.set_customCutoffCurve(CalculateAttenuation.LowPassFalloff(lpStart / maxDistance, lpPower));
			}
		}

		public void ApplyPreset(BusType type)
		{
			switch (type)
			{
			default:
				_ = 100;
				break;
			case BusType.EffectClose:
				channel = AudioChannel.Effects;
				maxDistance = 30f;
				falloffStart = 1f;
				falloffPower = 0.4f;
				lpStart = 1f;
				lpPower = 0.5f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			case BusType.EffectMedium:
				channel = AudioChannel.Effects;
				maxDistance = 50f;
				falloffStart = 1f;
				falloffPower = 0.5f;
				lpStart = 2f;
				lpPower = 0.6f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			case BusType.EffectFar:
				channel = AudioChannel.Effects;
				maxDistance = 100f;
				falloffStart = 2f;
				falloffPower = 0.8f;
				lpStart = 4f;
				lpPower = 0.8f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			case BusType.AmbienceClose:
				channel = AudioChannel.Ambience;
				maxDistance = 30f;
				falloffStart = 1f;
				falloffPower = 0.5f;
				lpStart = 2f;
				lpPower = 0.5f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			case BusType.AmbienceMedium:
				channel = AudioChannel.Ambience;
				maxDistance = 50f;
				falloffStart = 3f;
				falloffPower = 0.6f;
				lpStart = 3f;
				lpPower = 0.8f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			case BusType.AmbienceFar:
				channel = AudioChannel.Effects;
				maxDistance = 100f;
				falloffStart = 5f;
				falloffPower = 0.7f;
				lpStart = 5f;
				lpPower = 0.8f;
				spreadNear = 0.5f;
				spreadFar = 0f;
				spatialNear = 0.5f;
				spatialFar = 1f;
				break;
			}
		}

		private void OnDrawGizmosSelected()
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (!(this is AmbienceSource))
			{
				Gizmos.set_color((channel == AudioChannel.Ambience) ? new Color(1f, 0f, 0f, 0.3f) : new Color(0.5f, 0.5f, 1f, 0.3f));
				Gizmos.DrawSphere(((Component)this).get_transform().get_position(), 5f);
				drawString(((Object)this).get_name(), ((Component)this).get_transform().get_position() + Vector3.get_up() * 3f, Color.get_white());
			}
		}

		public static void drawString(string text, Vector3 worldPos, Color color)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Expected O, but got Unknown
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_0148: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			Handles.BeginGUI();
			Color color2 = GUI.get_color();
			SceneView currentDrawingSceneView = SceneView.get_currentDrawingSceneView();
			Vector3 val = currentDrawingSceneView.get_camera().WorldToScreenPoint(worldPos);
			if (val.y < 0f || val.y > (float)Screen.get_height() || val.x < 0f || val.x > (float)Screen.get_width() || val.z < 0f)
			{
				GUI.set_color(color2);
				Handles.EndGUI();
				return;
			}
			Vector2 val2 = GUI.get_skin().get_label().CalcSize(new GUIContent(text));
			GUI.set_color(Color.get_black());
			float num = val.x - val2.x / 2f + 1f;
			float num2 = 0f - val.y;
			Rect position = ((EditorWindow)currentDrawingSceneView).get_position();
			GUI.Label(new Rect(num, num2 + ((Rect)(ref position)).get_height() + 1f - val2.y, val2.x, val2.y), text);
			float num3 = val.x - val2.x / 2f - 1f;
			float num4 = 0f - val.y;
			position = ((EditorWindow)currentDrawingSceneView).get_position();
			GUI.Label(new Rect(num3, num4 + ((Rect)(ref position)).get_height() - val2.y, val2.x, val2.y), text);
			float num5 = val.x - val2.x / 2f + 1f;
			float num6 = 0f - val.y;
			position = ((EditorWindow)currentDrawingSceneView).get_position();
			GUI.Label(new Rect(num5, num6 + ((Rect)(ref position)).get_height() - val2.y, val2.x, val2.y), text);
			float num7 = val.x - val2.x / 2f + 2f;
			float num8 = 0f - val.y;
			position = ((EditorWindow)currentDrawingSceneView).get_position();
			GUI.Label(new Rect(num7, num8 + ((Rect)(ref position)).get_height() + 2f - val2.y, val2.x, val2.y), text);
			GUI.set_color(color);
			float num9 = val.x - val2.x / 2f;
			float num10 = 0f - val.y;
			position = ((EditorWindow)currentDrawingSceneView).get_position();
			GUI.Label(new Rect(num9, num10 + ((Rect)(ref position)).get_height() - val2.y, val2.x, val2.y), text);
			GUI.set_color(color2);
			Handles.EndGUI();
		}

		public Sound2()
			: this()
		{
		}
	}
}

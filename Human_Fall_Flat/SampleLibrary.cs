using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class SampleLibrary : MonoBehaviour
{
	public AudioSourcePoolId poolId;

	public float levelDB = -12f;

	public int maxVoices = 3;

	public int maxVoicesPerEntity = 3;

	public float blockTime;

	public bool dontBlockLouder;

	public float rndVolumeMin = 1f;

	public float rndVolumeMax = 1f;

	public float rndPitchMin = 1f;

	public float rndPitchMax = 1f;

	public float compressorTresholdDB;

	public float compressorRate = 8f;

	private float lastSampleTime;

	public float unitImpact = 40f;

	public float unitVelocity = 10f;

	public ImpactVolumeMix volumeMix = ImpactVolumeMix.Lerp;

	public float lerpImpactWeight = 0.5f;

	public bool isSlide;

	public SampleLibrary slideLibrary;

	public AudioClip[] clips;

	public float[] clipsRms;

	private List<float> polyphonyBlockUntil = new List<float>();

	private List<float> playingLoudness = new List<float>();

	private float blockUntil;

	private float volumeAdjust => AudioUtils.DBToValue(levelDB) / clipsRms[0];

	private float compressorTreshold => AudioUtils.DBToValue(compressorTresholdDB);

	private void OnEnable()
	{
	}

	private bool CanPlay(float volume)
	{
		if (volumeAdjust == 0f)
		{
			return false;
		}
		if (volume < 0.001f)
		{
			return false;
		}
		float time = Time.get_time();
		bool flag = true;
		for (int num = polyphonyBlockUntil.Count - 1; num >= 0; num--)
		{
			if (polyphonyBlockUntil[num] <= time)
			{
				polyphonyBlockUntil.RemoveAt(num);
				playingLoudness.RemoveAt(num);
			}
			else if (volume < playingLoudness[num])
			{
				flag = false;
			}
		}
		if (!(dontBlockLouder && flag))
		{
			if (blockUntil > time)
			{
				return false;
			}
			if (polyphonyBlockUntil.Count >= maxVoices)
			{
				return false;
			}
		}
		return true;
	}

	public bool PlayRMS(AudioChannel channel, Vector3 pos, float rms, float pitch)
	{
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		rms *= volumeAdjust;
		rms = ((rndVolumeMin != rndVolumeMax) ? (rms * Random.Range(rndVolumeMin, rndVolumeMax)) : (rms * rndVolumeMin));
		if (!CanPlay(rms))
		{
			return false;
		}
		pitch = ((rndPitchMin != rndPitchMax) ? (pitch * Random.Range(rndPitchMin, rndPitchMax)) : (pitch * rndPitchMin));
		int num = Random.Range(0, clips.Length);
		AudioClip val = clips[num];
		float num2 = rms / clipsRms[num];
		AudioSource obj = AudioSourcePool.Allocate(poolId, null, pos);
		obj.set_pitch(pitch);
		obj.set_outputAudioMixerGroup(AudioRouting.GetChannel(channel));
		obj.set_volume(1f);
		obj.PlayOneShot(val, num2);
		float time = Time.get_time();
		if (blockTime > 0f)
		{
			blockUntil = Mathf.Max(blockUntil, time + blockTime);
		}
		if (maxVoices > 0)
		{
			polyphonyBlockUntil.Add(time + val.get_length());
			playingLoudness.Add(rms);
		}
		return true;
	}

	public SampleLibrary()
		: this()
	{
	}
}

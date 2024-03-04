using System;
using HumanAPI;
using Multiplayer;
using UnityEngine;

[Serializable]
public class CollisionAudioHitConfig
{
	[NonSerialized]
	public ushort netId;

	public SampleLibrary sampleLib;

	public float levelDB;

	public float compTresholdDB;

	public float compRatio = 4f;

	public float velocityComp = 1f;

	public float impulseComp = 10f;

	private float minImpact = 1f;

	public float pitch0 = 1f;

	public float pitch1 = 1f;

	public float pitch0velocity;

	public float pitch1velocity = 100f;

	public bool Play(CollisionAudioSensor sensor, AudioChannel channel, CollisionAudioHitMonitor monitor, Vector3 pos, float impulse, float velocity, float volume, float pitch)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		float num = impulse / CollisionAudioEngine.instance.unitImpulse;
		float num2 = velocity / CollisionAudioEngine.instance.unitVelocity;
		if (num > 1f)
		{
			num = (num - 1f) / impulseComp + 1f;
		}
		if (num2 > 1f)
		{
			num2 = (num2 - 1f) / velocityComp + 1f;
		}
		float num3 = Mathf.InverseLerp(pitch0velocity, pitch1velocity, velocity);
		pitch *= Mathf.Lerp(pitch0, pitch1, num3);
		float num4 = num * num2 * volume;
		if (NetGame.isServer || ReplayRecorder.isRecording)
		{
			sensor.BroadcastCollisionAudio(this, channel, pos, num4, pitch);
		}
		return PlayWithKnownEmit(channel, monitor, pos, num4, pitch);
	}

	public bool PlayWithKnownEmit(AudioChannel channel, CollisionAudioHitMonitor monitor, Vector3 pos, float emit, float pitch)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0294: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)Listener.instance == (Object)null)
		{
			return false;
		}
		Vector3 val = pos - ((Component)Listener.instance).get_transform().get_position();
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		float num = ((magnitude < 2f) ? 1f : (2f / magnitude));
		float num2 = AudioUtils.DBToValue(compTresholdDB) / num;
		float num3 = 1f;
		if (emit > num2)
		{
			float num4 = AudioUtils.ValueToDB(emit);
			num3 = AudioUtils.DBToValue((AudioUtils.ValueToDB(num2) - num4) * (1f - 1f / compRatio));
		}
		float num5 = emit * num3;
		float num6 = num5 * AudioUtils.DBToValue(levelDB);
		if (monitor != null)
		{
			float realtimeSinceStartup = Time.get_realtimeSinceStartup();
			monitor.lastEmit = emit;
			monitor.lastPitch = pitch;
			monitor.lastDist = magnitude;
			monitor.lastAtten = num;
			monitor.lastCompress = num3;
			monitor.lastPreCompress = emit * num;
			monitor.lastPostCompress = num5 * num;
			monitor.lastLevel = num6 * num;
			monitor.lastOutput = num6;
			monitor.lastTime = realtimeSinceStartup;
			float num7 = 5f;
			if (monitor.lastImpact > monitor.peakImpact || monitor.peakImpactTime + num7 < realtimeSinceStartup)
			{
				monitor.peakImpact = monitor.lastImpact;
				monitor.peakImpactTime = realtimeSinceStartup;
			}
			if (monitor.lastVelocity > monitor.peakVelocity || monitor.peakVelocityTime + num7 < realtimeSinceStartup)
			{
				monitor.peakVelocity = monitor.lastVelocity;
				monitor.peakVelocityTime = realtimeSinceStartup;
			}
			if (monitor.lastEmit > monitor.peakEmit || monitor.peakEmitTime + num7 < realtimeSinceStartup)
			{
				monitor.peakEmit = monitor.lastEmit;
				monitor.peakEmitTime = realtimeSinceStartup;
			}
			if (monitor.lastCompress < monitor.peakCompress || monitor.peakCompressTime + num7 < realtimeSinceStartup)
			{
				monitor.peakCompress = monitor.lastCompress;
				monitor.peakCompressTime = realtimeSinceStartup;
			}
			if (monitor.lastPreCompress > monitor.peakPreCompress || monitor.peakPreCompressTime + num7 < realtimeSinceStartup)
			{
				monitor.peakPreCompress = monitor.lastPreCompress;
				monitor.peakPreCompressTime = realtimeSinceStartup;
			}
			if (monitor.lastPostCompress > monitor.peakPostCompress || monitor.peakPostCompressTime + num7 < realtimeSinceStartup)
			{
				monitor.peakPostCompress = monitor.lastPostCompress;
				monitor.peakPostCompressTime = realtimeSinceStartup;
			}
			if (monitor.lastLevel > monitor.peakLevel || monitor.peakLevelTime + num7 < realtimeSinceStartup)
			{
				monitor.peakLevel = monitor.lastLevel;
				monitor.peakLevelTime = realtimeSinceStartup;
			}
			if (monitor.lastOutput > monitor.peakOutput || monitor.peakOutputTime + num7 < realtimeSinceStartup)
			{
				monitor.peakOutput = monitor.lastOutput;
				monitor.peakOutputTime = realtimeSinceStartup;
			}
		}
		return sampleLib.PlayRMS(channel, pos, num6, pitch);
	}
}

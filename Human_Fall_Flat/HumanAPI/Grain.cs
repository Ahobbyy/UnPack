using UnityEngine;

namespace HumanAPI
{
	public class Grain : Sound2
	{
		[Range(0f, 1f)]
		[Tooltip("Used in playing the start and stop sounds at the right location")]
		public float intensity;

		[Range(0f, 1f)]
		[Tooltip("Used in playing the start and stop sounds at the right location")]
		public float minIntensity;

		[Tooltip("Used to track locations within the sound range")]
		public float frequencyAtMaxIntensity = 30f;

		[Tooltip("Used to calc the volume sound plays at")]
		public float slowPitch = 1f;

		[Tooltip("Used to calc the volume sound plays at")]
		internal float slowVolume = 1f;

		[Range(0f, 10f)]
		[Tooltip("Used to track locations within the sound range")]
		public float slowJitter = 5f;

		[Range(0f, 10f)]
		[Tooltip("Used to track locations within the sound range")]
		public float fastJitter = 1.5f;

		private float nextEmit;

		[Tooltip("The sound to play as part of the intro to the main sound")]
		public Sound2 startSound;

		[Tooltip("The sound to play once the main sound has stopped")]
		public Sound2 stopSound;

		[Tooltip("Cooldown timer to prevent the start/stop sound from playing")]
		public float startStopSoundCooldown = 0.2f;

		private float startStopCoolTimer;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private float[] stops = new float[20];

		private float accum;

		private float offset;

		private float prevIntensity;

		private void Start()
		{
			startOnLoad = false;
		}

		protected override void Update()
		{
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_018a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0194: Unknown result type (might be due to invalid IL or missing references)
			base.Update();
			float num = Mathf.Max((intensity - minIntensity) / (1f - minIntensity), 0f);
			if (startStopCoolTimer > 0f)
			{
				startStopCoolTimer -= Time.get_deltaTime();
			}
			if ((Object)(object)startSound != (Object)null && startStopCoolTimer <= 0f && prevIntensity <= minIntensity && intensity > minIntensity)
			{
				startSound.PlayOneShot(((Component)this).get_transform().get_position());
				startStopCoolTimer = startStopSoundCooldown;
			}
			else if ((Object)(object)stopSound != (Object)null && startStopCoolTimer <= 0f && prevIntensity > minIntensity && intensity <= minIntensity)
			{
				stopSound.PlayOneShot(((Component)this).get_transform().get_position());
				startStopCoolTimer = startStopSoundCooldown;
			}
			prevIntensity = intensity;
			accum += Time.get_deltaTime() * num;
			if (!(accum > 0f))
			{
				return;
			}
			for (int i = 0; i < stops.Length; i++)
			{
				if (stops[i] > 0f && stops[i] < accum)
				{
					Vector3 val = ((Component)Listener.instance).get_transform().get_position() - ((Component)this).get_transform().get_position();
					if (((Vector3)(ref val)).get_magnitude() < maxDistance)
					{
						PlayOneShot(Mathf.Lerp(slowVolume, 1f, num), Mathf.Lerp(slowPitch, 1f, num), Time.get_deltaTime() - (accum - stops[i]) / num);
					}
					stops[i] = 0f;
				}
			}
			while (accum > 1f / frequencyAtMaxIntensity)
			{
				accum -= 1f / frequencyAtMaxIntensity;
				for (int j = 0; j < stops.Length; j++)
				{
					if (stops[j] != 0f)
					{
						stops[j] -= 1f / frequencyAtMaxIntensity;
					}
				}
				for (int k = 0; k < stops.Length; k++)
				{
					if (stops[k] == 0f)
					{
						stops[k] = (1f + Random.Range(0f, Mathf.Lerp(slowJitter, fastJitter, num))) / frequencyAtMaxIntensity;
						break;
					}
				}
			}
		}
	}
}

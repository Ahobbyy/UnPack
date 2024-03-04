using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class SoundSourcePool : MonoBehaviour
	{
		public static SoundSourcePool instance;

		private Queue<AudioSource> pool = new Queue<AudioSource>();

		private List<AudioSource> playingSources = new List<AudioSource>();

		private void Awake()
		{
			instance = this;
		}

		public AudioSource GetAudioSource()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			while (pool.Count > 0)
			{
				AudioSource val = pool.Dequeue();
				if ((Object)(object)val != (Object)null)
				{
					return val;
				}
			}
			for (int i = 0; i < playingSources.Count; i++)
			{
				AudioSource val2 = playingSources[i];
				if ((Object)(object)val2 == (Object)null)
				{
					playingSources.RemoveAt(i);
					i--;
				}
				else if (!val2.get_isPlaying())
				{
					playingSources.RemoveAt(i);
					return val2;
				}
			}
			GameObject val3 = new GameObject("pooled audio");
			val3.get_transform().SetParent(((Component)this).get_transform(), false);
			AudioSource result = val3.AddComponent<AudioSource>();
			val3.AddComponent<AudioLowPassFilter>();
			return result;
		}

		public void ReleaseAudioSource(AudioSource source)
		{
			pool.Enqueue(source);
			source.Stop();
			source.set_clip((AudioClip)null);
			source.set_loop(false);
			((Component)source).get_transform().SetParent(((Component)this).get_transform(), false);
		}

		public void ReleaseAudioSourceOnComplete(AudioSource source)
		{
			playingSources.Add(source);
		}

		public SoundSourcePool()
			: this()
		{
		}
	}
}

using System.Collections;
using HumanAPI;
using UnityEngine;

public class IntroDrones : MonoBehaviour
{
	public static IntroDrones instance;

	public Sound2 sound;

	private float dronesStartTime;

	public float dronesTime
	{
		get
		{
			AudioSource activeSource = sound.GetActiveSource();
			float num = (((Object)(object)activeSource != (Object)null) ? activeSource.get_time() : 0f);
			if (num == 0f)
			{
				return Time.get_timeSinceLevelLoad() - dronesStartTime;
			}
			return num;
		}
	}

	private void Start()
	{
		instance = this;
		((Component)this).get_transform().SetParent((Transform)null, false);
		Object.DontDestroyOnLoad((Object)(object)((Component)this).get_gameObject());
	}

	public void Play()
	{
		sound.Play();
		dronesStartTime = Time.get_timeSinceLevelLoad();
		((MonoBehaviour)this).StartCoroutine(DestroyWhenDone());
	}

	private IEnumerator DestroyWhenDone()
	{
		AudioClip clip = sound.soundSample.clips[0].clip;
		while (dronesTime < clip.get_length())
		{
			yield return null;
		}
		instance = null;
		Object.Destroy((Object)(object)((Component)this).get_gameObject());
	}

	public IntroDrones()
		: this()
	{
	}
}

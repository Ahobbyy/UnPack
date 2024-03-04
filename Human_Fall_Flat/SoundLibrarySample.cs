using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class SoundLibrarySample : MonoBehaviour
{
	public string category;

	public float vB = 1f;

	public float vR;

	public float pB = 1f;

	public float pR;

	public float crossFade = 0.1f;

	public AudioClip[] clips;

	private SoundLibrary.SerializedSample serialized;

	public void Load(SoundLibrary.SerializedSample serialized)
	{
		((Object)this).set_name(serialized.name);
		category = serialized.category;
		crossFade = serialized.crossFade;
		vB = serialized.vB;
		vR = serialized.vR;
		pB = serialized.pB;
		pR = serialized.pR;
	}

	public SoundLibrary.SerializedSample GetSerialized()
	{
		if (serialized != null)
		{
			return serialized;
		}
		serialized = new SoundLibrary.SerializedSample
		{
			category = category,
			crossFade = crossFade,
			vB = vB,
			vR = vR,
			pB = pB,
			pR = pR,
			name = ((Object)this).get_name(),
			builtIn = true,
			clips = new List<SoundLibrary.SerializedClip>()
		};
		for (int i = 0; i < clips.Length; i++)
		{
			if ((Object)(object)clips[i] != (Object)null)
			{
				serialized.AddClip(category + "/" + ((Object)clips[i]).get_name(), ((Object)clips[i]).get_name(), null, clips[i]);
			}
			else
			{
				Debug.LogError((object)("Empty clip in sample " + serialized.name));
			}
		}
		serialized.loaded = (Object)(object)SoundSourcePool.instance != (Object)null;
		return serialized;
	}

	public SoundLibrarySample()
		: this()
	{
	}
}

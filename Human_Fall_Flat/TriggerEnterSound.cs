using HumanAPI;
using UnityEngine;

public class TriggerEnterSound : MonoBehaviour
{
	public float volume = 1f;

	public Collider[] acceptedColliders;

	public float minDelay = 0.2f;

	private float lastSoundTime;

	public void OnTriggerEnter(Collider other)
	{
		if (acceptedColliders.Length != 0)
		{
			bool flag = false;
			for (int i = 0; i < acceptedColliders.Length; i++)
			{
				if ((Object)(object)acceptedColliders[i] == (Object)(object)other)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				return;
			}
		}
		float time = Time.get_time();
		if (!(lastSoundTime + minDelay > time))
		{
			((Component)this).GetComponent<Sound2>().PlayOneShot();
			lastSoundTime = time;
		}
	}

	public TriggerEnterSound()
		: this()
	{
	}
}

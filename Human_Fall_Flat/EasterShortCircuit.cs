using System.Collections;
using UnityEngine;

public class EasterShortCircuit : MonoBehaviour
{
	public static EasterShortCircuit instance;

	public int countNeeded = 5;

	public float allowedIdle = 60f;

	private float lastTime;

	private int count;

	private void Awake()
	{
		instance = this;
	}

	public void ShorCircuit()
	{
		float time = Time.get_time();
		if (time - lastTime > allowedIdle)
		{
			count = 1;
		}
		else
		{
			count++;
		}
		lastTime = time;
		if (count >= countNeeded)
		{
			((MonoBehaviour)this).StartCoroutine(Play());
		}
	}

	private IEnumerator Play()
	{
		yield return (object)new WaitForSeconds(3f);
		((Component)this).GetComponent<AudioSource>().Play();
		count = 0;
		countNeeded++;
	}

	public EasterShortCircuit()
		: this()
	{
	}
}

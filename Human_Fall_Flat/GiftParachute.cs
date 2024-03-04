using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class GiftParachute : MonoBehaviour
{
	public GameObject master;

	public Sound2 sound;

	public static GiftParachute instance;

	private Queue<GameObject> queue = new Queue<GameObject>();

	private void Awake()
	{
		instance = this;
		master.SetActive(false);
	}

	public GameObject Allocate()
	{
		GameObject val = ((queue.Count <= 1) ? Object.Instantiate<GameObject>(master, ((Component)this).get_transform(), true) : queue.Dequeue());
		val.SetActive(true);
		return val;
	}

	public void Release(GameObject chute)
	{
		chute.SetActive(false);
		queue.Enqueue(chute);
	}

	public void PlaySound(Vector3 pos)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		sound.PlayOneShot(pos);
	}

	public GiftParachute()
		: this()
	{
	}
}

using System.Collections.Generic;
using UnityEngine;

public class AudioSourcePool : MonoBehaviour
{
	public AudioSourcePoolId poolId;

	public GameObject template;

	public int minItems;

	public int maxItems = 50;

	private List<AudioSource> active = new List<AudioSource>();

	private Queue<GameObject> available = new Queue<GameObject>();

	private static Dictionary<AudioSourcePoolId, AudioSourcePool> pools = new Dictionary<AudioSourcePoolId, AudioSourcePool>();

	private void OnEnable()
	{
		pools.Add(poolId, this);
		for (int i = 1; i < minItems; i++)
		{
			GameObject val = Object.Instantiate<GameObject>(template);
			val.get_transform().SetParent(((Component)this).get_transform(), false);
			available.Enqueue(val);
		}
	}

	private void OnDisable()
	{
		pools.Remove(poolId);
	}

	public static AudioSource Allocate(AudioSourcePoolId id, GameObject parent, Vector3 pos)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		if (pools.TryGetValue(id, out var value))
		{
			return value.Allocate(parent, pos);
		}
		Debug.LogError((object)("No audio pool for " + id));
		return null;
	}

	public AudioSource Allocate(GameObject parent, Vector3 pos)
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		GameObject val = null;
		AudioSource val2 = null;
		if (available.Count > 0)
		{
			val = available.Dequeue();
			val2 = val.GetComponent<AudioSource>();
		}
		else
		{
			for (int i = 0; i < active.Count; i++)
			{
				val2 = active[i];
				if ((Object)(object)val2 == (Object)null)
				{
					active.RemoveAt(i);
					i--;
				}
				else if (!val2.get_isPlaying())
				{
					val = ((Component)val2).get_gameObject();
					active.RemoveAt(i);
					break;
				}
			}
			if ((Object)(object)val == (Object)null)
			{
				if (active.Count >= maxItems)
				{
					Debug.LogError((object)("Allocating too much audiosources!" + ((Object)this).get_name()));
				}
				val = Object.Instantiate<GameObject>(template);
				val.get_transform().SetParent(((Component)this).get_transform(), false);
				val2 = val.GetComponent<AudioSource>();
			}
		}
		val.get_transform().set_position(pos);
		active.Add(val2);
		val.SetActive(true);
		return val2;
	}

	public void Free(AudioSource audiosource)
	{
		if (!active.Contains(audiosource))
		{
			Debug.LogError((object)"Freeing non allocated audiosource!");
			return;
		}
		active.Remove(audiosource);
		GameObject gameObject = ((Component)audiosource).get_gameObject();
		available.Enqueue(gameObject);
		gameObject.SetActive(false);
	}

	public void FreeAll()
	{
		for (int num = active.Count - 1; num >= 0; num--)
		{
			Free(active[num]);
		}
	}

	public AudioSourcePool()
		: this()
	{
	}
}

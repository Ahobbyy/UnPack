using System.Collections.Generic;
using UnityEngine;

public class RaftsCollisionAudio : MonoBehaviour
{
	[SerializeField]
	private GameObject soundMovingPrefab;

	private List<GameObject> rafters;

	private void Awake()
	{
		rafters = new List<GameObject>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!rafters.Contains(((Component)collision.get_collider()).get_gameObject()))
		{
			rafters.Add(((Component)collision.get_collider()).get_gameObject());
			Object.Instantiate<GameObject>(soundMovingPrefab).get_transform().SetParent(((Component)collision.get_collider()).get_gameObject().get_transform(), false);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		Transform val = ((Component)collision.get_collider()).get_gameObject().get_transform().Find(((Object)soundMovingPrefab).get_name() + "(Clone)");
		if ((Object)(object)val != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)val).get_gameObject());
			if (rafters.Contains(((Component)collision.get_collider()).get_gameObject()))
			{
				rafters.Remove(((Component)collision.get_collider()).get_gameObject());
			}
		}
	}

	public RaftsCollisionAudio()
		: this()
	{
	}
}

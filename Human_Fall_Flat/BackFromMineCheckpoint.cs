using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class BackFromMineCheckpoint : Checkpoint, IReset
{
	public int neededCoal = 5;

	public GameObject deliveredCoalContainer;

	public Transform deliveredTruckAlign;

	public Transform truck;

	public Transform[] ignoreParents;

	public List<Collider> delivered = new List<Collider>();

	public Transform validCoalParent;

	protected override void OnEnable()
	{
		base.OnEnable();
		Collider component = ((Component)this).GetComponent<Collider>();
		for (int i = 0; i < ignoreParents.Length; i++)
		{
			Collider[] componentsInChildren = ((Component)ignoreParents[i]).GetComponentsInChildren<Collider>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				Physics.IgnoreCollision(component, componentsInChildren[j]);
			}
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		if (!delivered.Contains(other) && ((Component)other).get_transform().IsChildOf(validCoalParent))
		{
			delivered.Add(other);
		}
		if (delivered.Count >= neededCoal)
		{
			base.OnTriggerEnter(other);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (delivered.Count >= neededCoal)
		{
			base.OnTriggerEnter(other);
		}
	}

	public override void RespawnHere()
	{
		LoadHere();
	}

	public override void LoadHere()
	{
		NetSetActive[] componentsInChildren = deliveredCoalContainer.GetComponentsInChildren<NetSetActive>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetActive(show: true);
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		NetSetActive[] componentsInChildren = deliveredCoalContainer.GetComponentsInChildren<NetSetActive>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetActive(show: false);
		}
	}
}

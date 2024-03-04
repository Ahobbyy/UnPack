using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

public class CaveTrigger : MonoBehaviour, IReset
{
	private float depth;

	private Dictionary<NetPlayer, float> playerPhase = new Dictionary<NetPlayer, float>();

	private CaveLighting lighting;

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		lighting = ((Component)this).GetComponentInParent<CaveLighting>();
		depth = ((Component)this).GetComponent<BoxCollider>().get_size().z;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!(((Component)other).get_tag() != "Player"))
		{
			NetPlayer componentInParent = ((Component)other).GetComponentInParent<NetPlayer>();
			if (!((Object)(object)componentInParent == (Object)null) && componentInParent.isLocalPlayer && !playerPhase.ContainsKey(componentInParent))
			{
				playerPhase[componentInParent] = GetPhase(componentInParent);
				lighting.SetPhase(componentInParent, playerPhase[componentInParent]);
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (((Component)other).get_tag() != "Player")
		{
			return;
		}
		NetPlayer componentInParent = ((Component)other).GetComponentInParent<NetPlayer>();
		if (!((Object)(object)componentInParent == (Object)null) && playerPhase.ContainsKey(componentInParent))
		{
			if (playerPhase[componentInParent] > 0.5f)
			{
				lighting.SetPhase(componentInParent, 1f);
			}
			else
			{
				lighting.SetPhase(componentInParent, 0f);
			}
			playerPhase.Remove(componentInParent);
		}
	}

	private void Update()
	{
		_ = playerPhase.Keys;
		for (int i = 0; i < NetGame.instance.local.players.Count; i++)
		{
			NetPlayer netPlayer = NetGame.instance.local.players[i];
			if (playerPhase.ContainsKey(netPlayer))
			{
				playerPhase[netPlayer] = GetPhase(netPlayer);
				lighting.SetPhase(netPlayer, playerPhase[netPlayer]);
			}
		}
	}

	private float GetPhase(NetPlayer player)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)this).get_transform().InverseTransformPoint(((Component)player.human).get_transform().get_position()).z / depth + 0.5f;
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		playerPhase.Clear();
	}

	public CaveTrigger()
		: this()
	{
	}
}

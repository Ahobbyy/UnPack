using System;
using Multiplayer;
using Steamworks;
using UnityEngine;

public class GiftBase : MonoBehaviour, IGrabbable
{
	public uint giftId;

	[NonSerialized]
	public ulong userId;

	public void OnGrab()
	{
		if (NetGame.isServer)
		{
			Human human = GrabManager.GrabbedBy(((Component)this).get_gameObject());
			userId = ((CSteamID)human.player.host.connection).m_SteamID;
		}
	}

	public void OnRelease()
	{
	}

	public GiftBase()
		: this()
	{
	}
}

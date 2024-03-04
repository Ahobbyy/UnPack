using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Multiplayer;
using Steamworks;
using UnityEngine;

public class ChatManager : MonoBehaviour
{
	public static ChatManager Instance;

	private List<ChatUser> m_chatUsers = new List<ChatUser>();

	private int m_frameCounter;

	private float m_maximumWaitTime = 5f;

	private float m_timeBetweenRefreshCalls = 0.1f;

	private int m_usersJoining;

	private int m_usersLeaving;

	private Dictionary<string, OverheadNameTag> m_nameTags = new Dictionary<string, OverheadNameTag>();

	public static string sRecentlyKickedPlayerName;

	private bool mNeedsRefresh;

	public bool VoiceChatDisabled { get; private set; }

	public event EventHandler<ChatUserEventArgs> ChatUserUpdated;

	public event EventHandler ChatListUpdated;

	private void Awake()
	{
		if ((Object)(object)Instance == (Object)null)
		{
			Instance = this;
			VoiceChatDisabled = false;
		}
		else if ((Object)(object)Instance != (Object)(object)this)
		{
			Object.Destroy((Object)(object)this);
		}
		Object.DontDestroyOnLoad((Object)(object)this);
	}

	private void Update()
	{
		if (!VoiceChatDisabled)
		{
			int num = m_chatUsers.Count * 2;
			if (m_frameCounter < num && m_frameCounter % 2 == 0 && m_chatUsers[m_frameCounter / 2].UpdateStatus())
			{
				OnChatUserUpdated(m_frameCounter / 2);
			}
			m_frameCounter++;
			if (m_frameCounter >= num)
			{
				m_frameCounter = 0;
			}
		}
	}

	public void AddUser()
	{
		m_usersJoining++;
		RefreshUserList();
	}

	public void Refresh()
	{
		RefreshUserList();
	}

	public void RemoveUser()
	{
		m_usersLeaving++;
		RefreshUserList();
	}

	private void RefreshUserList()
	{
		if (mNeedsRefresh)
		{
			OnChatListUpdated();
		}
	}

	protected virtual void OnChatUserUpdated(int index)
	{
		if (this.ChatUserUpdated != null)
		{
			this.ChatUserUpdated(this, new ChatUserEventArgs(index));
		}
		if ((Object)(object)m_nameTags[m_chatUsers[index].XboxUserId] != (Object)null)
		{
			m_nameTags[m_chatUsers[index].XboxUserId].UpdateNameTag(m_chatUsers[index]);
		}
	}

	protected virtual void OnChatListUpdated()
	{
		if (this.ChatListUpdated != null)
		{
			this.ChatListUpdated(this, EventArgs.Empty);
		}
	}

	public ReadOnlyCollection<ChatUser> GetChatUserList()
	{
		return m_chatUsers.AsReadOnly();
	}

	public int GetChatUserCount()
	{
		return m_chatUsers.Count;
	}

	public int GetUsersJoining()
	{
		return m_usersJoining;
	}

	public void ToggleChatUserMuteState(int userIndex, bool value)
	{
	}

	public void ViewPlayerProfile(int requestedUserIndex)
	{
	}

	public void KickPlayer(int userIndex)
	{
		if (userIndex >= 0 && userIndex < m_chatUsers.Count)
		{
			sRecentlyKickedPlayerName = m_chatUsers[userIndex].GamerTag;
		}
		else
		{
			sRecentlyKickedPlayerName = string.Empty;
		}
		KickPlayerInternal(userIndex);
	}

	private void KickPlayerInternal(int userIndex)
	{
		if (userIndex >= m_chatUsers.Count)
		{
			return;
		}
		List<NetPlayer> players = NetGame.instance.players;
		int count = players.Count;
		string xboxUserId = m_chatUsers[userIndex].XboxUserId;
		for (int i = 0; i < count; i++)
		{
			if (players[i].skinUserId.Equals(xboxUserId))
			{
				NetGame.instance.Kick(players[i].host);
				break;
			}
		}
	}

	public ChatUser GetHost()
	{
		return m_chatUsers.Find((ChatUser user) => user.IsHost);
	}

	public ChatUser GetLocalUser()
	{
		return m_chatUsers.Find((ChatUser user) => user.IsLocal);
	}

	private string GetSteamNameTag(NetPlayer player)
	{
		ulong ulSteamID = ulong.Parse(player.skinUserId);
		return SteamFriends.GetFriendPersonaName(new CSteamID(ulSteamID));
	}

	public void AddChatUser(NetPlayer player)
	{
		if (!m_nameTags.ContainsKey(player.skinUserId))
		{
			m_nameTags.Add(player.skinUserId, player.overHeadNameTag);
			ChatUser chatUser = new ChatUser(player.skinUserId);
			chatUser.UpdateGamerTag(GetSteamNameTag(player));
			m_chatUsers.Add(chatUser);
			mNeedsRefresh = true;
			RefreshUserList();
		}
	}

	public void RemoveChatUser(NetPlayer player)
	{
		if (player.localCoopIndex != 0)
		{
			return;
		}
		for (int i = 0; i < m_chatUsers.Count; i++)
		{
			if (m_chatUsers[i].XboxUserId.Equals(player.skinUserId))
			{
				m_chatUsers.RemoveAt(i);
				m_nameTags.Remove(player.skinUserId);
				mNeedsRefresh = true;
				RefreshUserList();
			}
		}
	}

	public void CleanUp()
	{
		m_chatUsers.Clear();
		m_nameTags.Clear();
	}

	public ChatManager()
		: this()
	{
	}
}

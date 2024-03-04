using HumanAPI;
using Multiplayer;
using Steamworks;
using UnityEngine;

public class SteamRichPresence : MonoBehaviour
{
	private const string GAME_MODE_KEY = "steam_display";

	private const string PLAYER_GROUP_KEY = "steam_player_group";

	private const string ROOM_SIZE_KEY = "steam_player_group_size";

	private int levelNumber;

	private WorkshopItemSource levelType;

	private bool verbose = true;

	private AppSate previousState;

	private int cachedOnlinePlayerCount = -1;

	private int cachedMaxPlayerCount = -1;

	private int currentMaxPlayerCount = 8;

	private bool isRegisteredForLobbyUpdates;

	private void Start()
	{
	}

	private void Update()
	{
		AppSate state = App.state;
		if (!((Object)(object)Game.instance == (Object)null) && !((Object)(object)WorkshopRepository.instance == (Object)null) && (previousState != state || (IsOnline(state) && (cachedOnlinePlayerCount != GetOnlinePlayerCount() || cachedMaxPlayerCount != currentMaxPlayerCount))))
		{
			levelNumber = Game.instance.currentLevelNumber;
			levelType = Game.instance.currentLevelType;
			currentMaxPlayerCount = (IsHost(state) ? Options.lobbyMaxPlayers : currentMaxPlayerCount);
			RegisterForLobbyDataIfNecessary(state);
			SetGameModeTitle(state);
			SetDataToGroupFriends(state);
			previousState = state;
			cachedOnlinePlayerCount = GetOnlinePlayerCount();
			cachedMaxPlayerCount = currentMaxPlayerCount;
		}
	}

	private void SetGameModeTitle(AppSate state)
	{
		switch (state)
		{
		case AppSate.Startup:
		case AppSate.Menu:
		case AppSate.Customize:
		case AppSate.LoadLevel:
			SetGameMode("#Menu");
			break;
		case AppSate.PlayLevel:
			if (levelType == WorkshopItemSource.BuiltIn || levelType == WorkshopItemSource.EditorPick)
			{
				WorkshopRepository.instance.levelRepo.GetLevel((ulong)levelNumber, levelType, delegate(WorkshopLevelMetadata metadata)
				{
					if (metadata != null)
					{
						SetLevelName(metadata.title);
						SetGameMode("#Local_Level");
					}
					else
					{
						Debug.LogError((object)$"[Steamworks:RichPresence] Level meta data not found for level number {levelNumber} of type {levelType}");
					}
				});
			}
			else if (levelType == WorkshopItemSource.Subscription || levelType == WorkshopItemSource.LocalWorkshop)
			{
				SetLevelName("Workshop");
				SetGameMode("#Local_Level");
			}
			else
			{
				Debug.LogError((object)$"[Steamworks:RichPresence] Invalid state {levelType} in {state}");
			}
			break;
		case AppSate.ServerHost:
		case AppSate.ClientJoin:
			SetGameMode("#Online_Joining");
			break;
		case AppSate.ServerLoadLobby:
		case AppSate.ServerLobby:
		case AppSate.ClientLoadLobby:
		case AppSate.ClientLobby:
			if ((Object)(object)NetGame.instance == (Object)null)
			{
				Debug.LogError((object)"[Steamworks:RichPresence] Invalid netgame instance");
			}
			else if (Game.multiplayerLobbyLevel < 128)
			{
				string lobbyName = WorkshopRepository.GetLobbyName(Game.multiplayerLobbyLevel);
				if (!string.IsNullOrEmpty(lobbyName))
				{
					SetLevelName(lobbyName);
					SetPlayerCount(GetOnlinePlayerCount(), currentMaxPlayerCount);
					SetGameMode("#Online_Lobby");
				}
				else
				{
					Debug.LogError((object)"[Steamworks:RichPresence] Lobby name not found for Game.multiplayerLobbyLevel");
				}
			}
			else
			{
				SetLevelName("Workshop");
				SetPlayerCount(GetOnlinePlayerCount(), currentMaxPlayerCount);
				SetGameMode("#Online_Lobby");
			}
			break;
		case AppSate.ServerLoadLevel:
		case AppSate.ServerPlayLevel:
		case AppSate.ClientLoadLevel:
		case AppSate.ClientWaitServerLoad:
		case AppSate.ClientPlayLevel:
			if (levelType == WorkshopItemSource.BuiltIn || levelType == WorkshopItemSource.EditorPick)
			{
				WorkshopRepository.instance.levelRepo.GetLevel((ulong)levelNumber, levelType, delegate(WorkshopLevelMetadata metadata)
				{
					if (metadata != null)
					{
						SetLevelName(metadata.title);
						SetPlayerCount(GetOnlinePlayerCount(), currentMaxPlayerCount);
						SetGameMode("#Online_Level");
					}
					else
					{
						Debug.LogError((object)$"[Steamworks:RichPresence] Level meta data not found for level number {levelNumber} of type {levelType}");
					}
				});
			}
			else if (levelType == WorkshopItemSource.Subscription || levelType == WorkshopItemSource.LocalWorkshop)
			{
				SetLevelName("Workshop");
				SetPlayerCount(GetOnlinePlayerCount(), currentMaxPlayerCount);
				SetGameMode("#Online_Level");
			}
			else
			{
				Debug.LogError((object)$"[Steamworks:RichPresence] Invalid state {levelType} in {state}");
			}
			break;
		default:
			Debug.LogError((object)"[Steamworks:RichPresence] App state unclear");
			break;
		}
	}

	private void SetDataToGroupFriends(AppSate state)
	{
		if (IsOnline(state))
		{
			if ((Object)(object)NetGame.instance != (Object)null)
			{
				NetTransportSteam netTransportSteam = (NetTransportSteam)NetGame.instance.transport;
				if ((Object)(object)Human.Localplayer == (Object)null || (Object)(object)Human.Localplayer.player == (Object)null || (Object)(object)netTransportSteam == (Object)null)
				{
					Debug.LogError((object)"[Steamworks:RichPresence] Invalid assumption??");
				}
				else
				{
					SetGroupInfo(netTransportSteam.lobbyID.ToString(), GetOnlinePlayerCount());
				}
			}
			else
			{
				ClearMultiplayerInfo();
			}
		}
		else
		{
			ClearMultiplayerInfo();
		}
	}

	private void RegisterForLobbyDataIfNecessary(AppSate state)
	{
		if (IsOnline(state))
		{
			if (!isRegisteredForLobbyUpdates)
			{
				NetGame.instance.transport.RegisterForLobbyData(OnLobbyDataUpdate);
				isRegisteredForLobbyUpdates = true;
			}
		}
		else if (isRegisteredForLobbyUpdates)
		{
			NetGame.instance.transport.UnregisterForLobbyData(OnLobbyDataUpdate);
			isRegisteredForLobbyUpdates = false;
		}
	}

	private void OnLobbyDataUpdate(object lobbyID, NetTransport.LobbyDisplayInfo dispInfo, bool error)
	{
		if (error)
		{
			Debug.LogError((object)"[Steamworks:RichPresence] Invalid data from Lobby update ??");
		}
		else
		{
			currentMaxPlayerCount = (int)dispInfo.MaxPlayers;
		}
	}

	private bool IsOnline(AppSate state)
	{
		if ((uint)(state - 6) <= 3u || (uint)(state - 11) <= 4u)
		{
			return true;
		}
		return false;
	}

	private bool IsHost(AppSate state)
	{
		if ((uint)(state - 6) <= 3u)
		{
			return true;
		}
		return false;
	}

	private int GetOnlinePlayerCount()
	{
		if (!((Object)(object)NetGame.instance != (Object)null))
		{
			return 0;
		}
		return NetGame.instance.players.Count;
	}

	private void SetLevelName(string localizedValue)
	{
		SteamFriends.SetRichPresence("LevelName", localizedValue);
		if (verbose)
		{
			Debug.Log((object)("[Steamworks:RichPresence] LevelName:" + localizedValue));
		}
	}

	private void SetGameMode(string token)
	{
		SteamFriends.SetRichPresence("steam_display", token);
		if (verbose)
		{
			Debug.Log((object)("[Steamworks:RichPresence] steam_display:" + token));
		}
	}

	private void SetPlayerCount(int current, int max)
	{
		string text = $"({current}/{max})";
		SteamFriends.SetRichPresence("PlayerCount", text);
		if (verbose)
		{
			Debug.Log((object)("[Steamworks:RichPresence] PlayerCount:" + text));
		}
	}

	private void SetGroupInfo(string lobbyID, int playerCount)
	{
		SteamFriends.SetRichPresence("steam_player_group", lobbyID);
		SteamFriends.SetRichPresence("steam_player_group_size", playerCount.ToString());
		if (verbose)
		{
			Debug.Log((object)string.Format("[Steamworks:RichPresence] {0}:{1};{2}:{3}", "steam_player_group", lobbyID, "steam_player_group_size", playerCount));
		}
	}

	private void ClearMultiplayerInfo()
	{
		SteamFriends.SetRichPresence("steam_player_group", null);
		SteamFriends.SetRichPresence("steam_player_group_size", null);
	}

	public SteamRichPresence()
		: this()
	{
	}
}

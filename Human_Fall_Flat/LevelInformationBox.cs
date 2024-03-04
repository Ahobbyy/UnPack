using System.Collections;
using System.Text;
using HumanAPI;
using I2.Loc;
using Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public sealed class LevelInformationBox : MonoBehaviour
{
	private enum InformationStrings
	{
		kEnabled,
		kDisabled,
		kInGame,
		kInLobby,
		kPlayers,
		kLobbyLevel,
		kJoinGameInProgress,
		kLockLevel,
		kInviteOnly,
		kMaxInformationStrings
	}

	public RawImage LevelImage;

	public Text LevelText;

	public Text LaunchedText;

	public Text LobbyNameText;

	public Text MaxPlayersText;

	public Text InviteText;

	public Text JoinGameInProgressText;

	public Text LockLevelText;

	private const int kMaxStringLength = 128;

	private const byte kTriStateBoolFalse = 0;

	private const byte kTriStateBoolTrue = 1;

	private const byte kTriStateBoolUnInit = 2;

	private static readonly string[] sInformationStrings = new string[9] { "MENU/COMMON/Enabled", "MENU/COMMON/Disabled", "MULTIPLAYER/LOBBYINFO.InGame", "MULTIPLAYER/LOBBYINFO.InLobby", "MULTIPLAYER/LOBBYINFO.Players", "MULTIPLAYER/LOBBYINFO.LobbyLevel", "MULTIPLAYER/LOBBYSETTINGS.GameInProgress", "MULTIPLAYER/LOBBYSETTINGS.LockLevel", "MULTIPLAYER/LOBBYSETTINGS.InviteOnly" };

	private static string[] sLocalisedStrings;

	private static StringBuilder sbInformationStrings = new StringBuilder(128);

	private static int sNumberInformationStrings;

	private NetTransport.LobbyDisplayInfo prevDispInfo;

	private void Awake()
	{
		sNumberInformationStrings = 9;
		sLocalisedStrings = new string[sNumberInformationStrings];
		GetInformationStrings();
	}

	private void OnEnable()
	{
		GetInformationStrings();
		ClearDisplay();
	}

	private void OnDisable()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private void GetInformationStrings()
	{
		for (int i = 0; i < sNumberInformationStrings; i++)
		{
			sLocalisedStrings[i] = ScriptLocalization.Get(sInformationStrings[i]);
		}
	}

	private string MakeLobbyTitle(string lobbyTitle)
	{
		lobbyTitle = WorkshopRepository.GetLobbyTitle(lobbyTitle);
		sbInformationStrings.Length = 0;
		sbInformationStrings.AppendFormat("{0}: {1}", sLocalisedStrings[5], lobbyTitle);
		return sbInformationStrings.ToString();
	}

	private string MakePlayers(uint currentPlayers, uint maxPlayers, int stringID)
	{
		sbInformationStrings.Length = 0;
		sbInformationStrings.AppendFormat("{0}: {1}/{2}", sLocalisedStrings[stringID], currentPlayers, maxPlayers);
		return sbInformationStrings.ToString();
	}

	private string MakeFlag(uint state, uint feature, uint mask, int stringID)
	{
		if ((feature & mask) == 0)
		{
			return string.Empty;
		}
		int num = (((state & mask) == 0) ? 1 : 0);
		sbInformationStrings.Length = 0;
		sbInformationStrings.AppendFormat("{0}: {1}", sLocalisedStrings[stringID], sLocalisedStrings[num]);
		return sbInformationStrings.ToString();
	}

	public void ClearDisplay()
	{
		prevDispInfo.InitBlank();
		MaxPlayersText.set_text("");
		JoinGameInProgressText.set_text("");
		InviteText.set_text("");
		LockLevelText.set_text("");
		LaunchedText.set_text("");
		LobbyNameText.set_text("");
		if ((Object)(object)LevelText != (Object)null)
		{
			LevelText.set_text("");
		}
		if ((Object)(object)LevelImage != (Object)null)
		{
			LevelImage.set_texture((Texture)null);
			((Behaviour)LevelImage).set_enabled(false);
		}
	}

	public void UpdateDisplay(NetTransport.ILobbyEntry data)
	{
		NetTransport.LobbyDisplayInfo info = default(NetTransport.LobbyDisplayInfo);
		if (data == null)
		{
			info.InitBlank();
		}
		else
		{
			data.getDisplayInfo(out info);
		}
		UpdateDisplay(info);
	}

	public void UpdateDisplay(NetTransport.LobbyDisplayInfo dispInfo)
	{
		if ((Object)(object)InviteText != (Object)null)
		{
			((Component)InviteText).get_gameObject().SetActive(NetGame.instance.transport.CanSendInvite());
		}
		if (!((Component)this).get_gameObject().get_activeSelf())
		{
			return;
		}
		if (dispInfo.FeaturesMask == 0)
		{
			ClearDisplay();
			((Component)this).get_gameObject().SetActive(false);
			return;
		}
		if ((dispInfo.FeaturesMask & 0x20u) != 0 && (dispInfo.Flags & 0x20) == 0)
		{
			ClearDisplay();
			((Component)this).get_gameObject().SetActive(false);
			return;
		}
		uint num = dispInfo.Compare(ref prevDispInfo);
		prevDispInfo = dispInfo;
		if ((num & 0xC0000000u) != 0)
		{
			if (dispInfo.ShouldDisplayAllAttr(3221225472u))
			{
				MaxPlayersText.set_text(MakePlayers(dispInfo.NumPlayersForDisplay, dispInfo.MaxPlayers, 4));
			}
			else
			{
				MaxPlayersText.set_text(string.Empty);
			}
		}
		if ((num & (true ? 1u : 0u)) != 0)
		{
			JoinGameInProgressText.set_text(MakeFlag(dispInfo.Flags, dispInfo.FeaturesMask, 1u, 6));
		}
		if ((num & 4u) != 0)
		{
			InviteText.set_text(MakeFlag(dispInfo.Flags, dispInfo.FeaturesMask, 4u, 8));
		}
		if ((num & 2u) != 0)
		{
			LockLevelText.set_text(MakeFlag(dispInfo.Flags, dispInfo.FeaturesMask, 2u, 7));
		}
		if ((num & 8u) != 0)
		{
			if ((dispInfo.FeaturesMask & 8u) != 0)
			{
				LaunchedText.set_text(((dispInfo.Flags & 8u) != 0) ? sLocalisedStrings[2] : sLocalisedStrings[3]);
			}
			else
			{
				LaunchedText.set_text(string.Empty);
			}
		}
		if ((num & 0x8000000u) != 0)
		{
			if ((dispInfo.FeaturesMask & 0x8000000u) != 0)
			{
				LobbyNameText.set_text(MakeLobbyTitle(dispInfo.LobbyTitle));
			}
			else
			{
				LobbyNameText.set_text(string.Empty);
			}
		}
		if ((Object)(object)LevelText == (Object)null || (Object)(object)LevelImage == (Object)null || (num & 0x20000000) == 0)
		{
			return;
		}
		if ((dispInfo.FeaturesMask & 0x20000000) == 0)
		{
			LevelText.set_text("");
			LevelImage.set_texture((Texture)null);
			((Behaviour)LevelImage).set_enabled(false);
			return;
		}
		string path;
		switch (dispInfo.LevelType)
		{
		case WorkshopItemSource.BuiltIn:
			WorkshopRepository.instance.LoadBuiltinLevels();
			path = "builtin:" + dispInfo.LevelID;
			break;
		case WorkshopItemSource.EditorPick:
			WorkshopRepository.instance.LoadEditorPickLevels();
			path = "editorpick:" + dispInfo.LevelID;
			break;
		default:
			path = "ws:" + dispInfo.LevelID + "/";
			break;
		}
		WorkshopLevelMetadata item = WorkshopRepository.instance.levelRepo.GetItem(path);
		if (item != null)
		{
			LevelText.set_text(item.title);
			LevelImage.set_texture((Texture)(object)item.thumbnailTexture);
			((Behaviour)LevelImage).set_enabled((Object)(object)LevelImage.get_texture() != (Object)null);
		}
		else if (dispInfo.LevelID == (ulong)(Game.instance.levels.Length - 1))
		{
			LevelText.set_text(ScriptLocalization.Get("LEVEL/" + Game.instance.levels[Game.instance.levels.Length - 1]));
			LevelImage.set_texture((Texture)(object)HFFResources.instance.FindTextureResource("LevelImages/" + Game.instance.levels[Game.instance.levels.Length - 1]));
			((Behaviour)LevelImage).set_enabled(true);
		}
		else
		{
			LevelText.set_text("");
			LevelImage.set_texture((Texture)null);
			((Behaviour)LevelImage).set_enabled(false);
			((MonoBehaviour)this).StartCoroutine(GetNewLevel(dispInfo.LevelID));
		}
	}

	private IEnumerator GetNewLevel(ulong levelID)
	{
		bool loaded = false;
		WorkshopLevelMetadata levelData;
		WorkshopRepository.instance.levelRepo.LoadLevel(levelID, delegate(WorkshopLevelMetadata l)
		{
			levelData = l;
			loaded = true;
			if (levelData != null && (prevDispInfo.FeaturesMask & 0x20000000u) != 0 && prevDispInfo.LevelID == levelID)
			{
				LevelText.set_text(levelData.title);
				LevelImage.set_texture((Texture)(object)levelData.thumbnailTexture);
				((Behaviour)LevelImage).set_enabled((Object)(object)LevelImage.get_texture() != (Object)null);
			}
		});
		while (!loaded)
		{
			yield return null;
		}
	}

	public LevelInformationBox()
		: this()
	{
	}
}

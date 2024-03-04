using HumanAPI;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Multiplayer
{
	public class MultiplayerLobbyMenu : MenuTransition
	{
		public RawImage levelThumbnail;

		public Text levelName;

		public TextMeshProUGUI titleText;

		public TextMeshProUGUI settingsText;

		public TextMeshProUGUI lobbyText;

		public Button joinButton;

		public Button inviteButton;

		public Button startGameButton;

		public Button selectLevelButton;

		public Button settingsButton;

		public Button leaveButton;

		public Button selectLobbyButton;

		public LevelInformationBox informationBox;

		private MultiplayerMenuOptions m_MenuOptions;

		private WorkshopLevelMetadata level;

		private float lobbyRefreshTimer;

		public override void OnGotFocus()
		{
			titleText.text = ScriptLocalization.Get((!NetGame.isServer) ? "MULTIPLAYER/LOBBY.TitleClient" : (NetGame.friendly ? "MULTIPLAYER/LOBBY.TitleServer" : "MULTIPLAYER/LOBBY.TitleServerPublic"));
			bool flag = NetGame.instance.transport.CanSendInvite();
			((Component)inviteButton).get_gameObject().SetActive(flag);
			((Component)selectLevelButton).get_gameObject().SetActive(NetGame.isServer);
			((Component)startGameButton).get_gameObject().SetActive(NetGame.isServer);
			((Component)settingsButton).get_gameObject().SetActive(NetGame.isServer);
			WorkshopRepository.instance.LoadBuiltinLevels();
			RebindLevel();
			RebindSettings();
			defaultElement = (NetGame.isServer ? ((Component)startGameButton).get_gameObject() : (flag ? ((Component)inviteButton).get_gameObject() : ((Component)leaveButton).get_gameObject()));
			base.OnGotFocus();
			if (NetGame.isServer)
			{
				lobbyText.text = string.Format("{0}\n<size=75%>{1}</size>", ScriptLocalization.Get("WORKSHOP/SelectLobbyLevel"), WorkshopRepository.GetLobbyTitle(App.instance.GetLobbyTitle()));
			}
			((Component)selectLobbyButton).get_gameObject().SetActive(NetGame.isServer);
			((Component)informationBox).get_gameObject().SetActive(!NetGame.isServer);
			NetGame.instance.transport.RegisterForLobbyData(OnLobbyDataUpdate);
		}

		public void RebindSettings()
		{
			if (NetGame.isServer)
			{
				_ = NetGame.friendly;
				string text = ScriptLocalization.Get("MULTIPLAYER/LOBBY.SETTINGS") + "\r\n<size=75%>";
				text += string.Format(ScriptLocalization.Get("MULTIPLAYER/LOBBY.MaxPlayers"), Options.lobbyMaxPlayers);
				if (true && Options.lobbyInviteOnly > 0)
				{
					text = text + ", " + ScriptLocalization.Get("MULTIPLAYER/LOBBY.InviteOnly");
				}
				if (Options.lobbyJoinInProgress > 0)
				{
					text = text + ", " + ScriptLocalization.Get("MULTIPLAYER/LOBBY.GameInProgress");
				}
				text += "</size>";
				settingsText.text = text;
			}
		}

		public void RebindLevel()
		{
			if (level != null)
			{
				level.ReleaseThumbnailReference();
			}
			switch (NetGame.instance.currentLevelType)
			{
			case WorkshopItemSource.BuiltIn:
				WorkshopRepository.instance.LoadBuiltinLevels();
				break;
			case WorkshopItemSource.BuiltInLobbies:
				WorkshopRepository.instance.LoadBuiltinLevels(requestLobbies: true);
				break;
			case WorkshopItemSource.EditorPick:
				WorkshopRepository.instance.LoadEditorPickLevels();
				break;
			}
			WorkshopRepository.instance.levelRepo.GetLevel(NetGame.instance.currentLevel, NetGame.instance.currentLevelType, delegate(WorkshopLevelMetadata l)
			{
				level = l;
				bool flag = false;
				if (level != null)
				{
					if (NetGame.isServer && DLC.instance.SupportsDLC() && level.workshopId < 16 && !DLC.instance.LevelIsAvailable((int)level.workshopId))
					{
						flag = true;
					}
					if (!flag)
					{
						BindImage(level.thumbnailTexture);
						levelName.set_text(level.title);
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					BindImage(null);
					levelName.set_text("MISSING");
					NetGame.instance.currentLevel = 0uL;
					if (NetGame.isServer)
					{
						App.instance.ChangeLobbyLevel(0uL, WorkshopItemSource.BuiltIn);
					}
					RebindLevel();
				}
			});
		}

		private void BindImage(Texture2D image)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)image != (Object)null)
			{
				Rect val = ((Graphic)levelThumbnail).get_rectTransform().get_rect();
				float num = 1f;
				float num2 = 1f;
				if ((float)((Texture)image).get_width() / ((Rect)(ref val)).get_width() > (float)((Texture)image).get_height() / ((Rect)(ref val)).get_height())
				{
					num = (float)((Texture)image).get_height() / ((Rect)(ref val)).get_height() / ((float)((Texture)image).get_width() / ((Rect)(ref val)).get_width());
				}
				else
				{
					num2 = (float)((Texture)image).get_width() / ((Rect)(ref val)).get_width() / ((float)((Texture)image).get_height() / ((Rect)(ref val)).get_height());
				}
				levelThumbnail.set_uvRect(new Rect(0.5f - num / 2f, 0.5f - num2 / 2f, num, num2));
			}
			levelThumbnail.set_texture((Texture)(object)image);
		}

		public override void OnLostFocus()
		{
			base.OnLostFocus();
			NetGame.instance.transport.UnregisterForLobbyData(OnLobbyDataUpdate);
			if (level != null)
			{
				level.ReleaseThumbnailReference();
				level = null;
			}
			m_MenuOptions.SetPlayerActionVisibility(flag: false);
		}

		public void SelectLobbyClick()
		{
			if (MenuSystem.CanInvoke)
			{
				LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: true);
				LevelSelectMenu2.instance.ShowLobbies();
				TransitionForward<LevelSelectMenu2>();
			}
		}

		public void SelectLevelClick()
		{
			if (MenuSystem.CanInvoke)
			{
				LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: true);
				LevelSelectMenu2.instance.ShowPickADream();
				switch (NetGame.instance.currentLevelType)
				{
				case WorkshopItemSource.BuiltIn:
					LevelSelectMenu2.selectedPath = "builtin:" + NetGame.instance.currentLevel;
					break;
				case WorkshopItemSource.EditorPick:
					LevelSelectMenu2.selectedPath = "editorpick:" + NetGame.instance.currentLevel;
					break;
				default:
					LevelSelectMenu2.selectedPath = "ws:" + NetGame.instance.currentLevel;
					break;
				}
				TransitionForward<LevelSelectMenu2>();
			}
		}

		public void StartGameClick()
		{
			if (MenuSystem.CanInvoke)
			{
				Options.multiplayerLobbyLevelStore = Game.multiplayerLobbyLevel;
				App.instance.StartGameServer(NetGame.instance.currentLevel, NetGame.instance.currentLevelType);
				NetGame.instance.transport.SetLobbyStatus(status: true);
			}
		}

		public void SettingsClick()
		{
			if (MenuSystem.CanInvoke)
			{
				TransitionForward<MultiplayerLobbySettingsMenu>();
			}
		}

		public void InviteClick()
		{
			if (MenuSystem.CanInvoke)
			{
				NetGame.instance.transport.SendInvite();
			}
		}

		public void LeaveClick()
		{
			if (MenuSystem.CanInvoke && App.state != AppSate.Menu)
			{
				ConfirmMenu.MultiplayerLeaveLobby();
			}
		}

		public void HideUIClick()
		{
			if (MenuSystem.CanInvoke && (Object)(object)MultiplayerLobbyController.instance != (Object)null)
			{
				MultiplayerLobbyController.instance.HideUI();
			}
		}

		public override void ApplyMenuEffects()
		{
			MenuCameraEffects.FadeInLobby();
		}

		public override void OnBack()
		{
			HideUIClick();
		}

		private void Awake()
		{
			m_MenuOptions = Object.FindObjectOfType<MultiplayerMenuOptions>();
		}

		protected override void Update()
		{
			base.Update();
			if (Object.op_Implicit((Object)(object)m_MenuOptions))
			{
				m_MenuOptions.OnButtonUpdate(lastFocusedElement);
				m_MenuOptions.OnUpdate();
			}
			if (NetGame.isClient)
			{
				RefreshLobby();
			}
		}

		private void RefreshLobby()
		{
			lobbyRefreshTimer -= Time.get_deltaTime();
			if (lobbyRefreshTimer < 0f)
			{
				NetGame.instance.transport.RequestLobbyDataRefresh(null, inSession: true);
				lobbyRefreshTimer = NetGame.instance.transport.GetLobbyDataRefreshThrottleTime();
			}
		}

		private void OnLobbyDataUpdate(object lobbyID, NetTransport.LobbyDisplayInfo dispInfo, bool error)
		{
			dispInfo.FeaturesMask &= 3758096383u;
			dispInfo.FeaturesMask &= 4294967263u;
			if (error)
			{
				informationBox.ClearDisplay();
			}
			else
			{
				informationBox.UpdateDisplay(dispInfo);
			}
		}
	}
}

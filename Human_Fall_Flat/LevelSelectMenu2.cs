using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using HumanAPI;
using I2.Loc;
using Multiplayer;
using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectMenu2 : MenuTransition
{
	private enum MetaStrings
	{
		kRecommendedPlayers,
		kLevelType,
		kThemes
	}

	public enum DescriptionProcess
	{
		kNone,
		kSearch,
		kBracketSearch,
		kSkipUntilEndIndex
	}

	public struct TagData
	{
		public string tag;

		public string replaceStart;

		public string replaceEnd;

		public DescriptionProcess mode;
	}

	public static LevelSelectMenu2 instance;

	public AutoNavigation topPanel;

	public RawImage levelThumbnail;

	public TextMeshProUGUI levelName;

	public GameObject levelInfoPanel;

	public GameObject levelImage;

	public GameObject levelDescription;

	public ListView list;

	public GameObject subscribedTitle;

	public GameObject subscribedSubtitle;

	public GameObject customTitle;

	public GameObject customSubtitle;

	public GameObject campaignTitle;

	public GameObject DLCAvailableTitle;

	public GameObject multiplayerTitle;

	public GameObject lobbyTitle;

	public GameObject lobbySubscribedTitle;

	public GameObject editorPicksTitle;

	public GameObject noSubscribedPrompt;

	public GameObject offlinePanel;

	public GameObject noLocalPrompt;

	public TextMeshProUGUI customFolder;

	public TMP_InputField customFolderEditor;

	public GameObject PlayButton;

	public GameObject FindMoreButton;

	public TextMeshProUGUI PlayButtonText;

	public GameObject RestartButton;

	public GameObject ShowPickADreamLevelButton;

	public GameObject ShowSubscribedLevelButton;

	public GameObject ShowLocalLevelButton;

	public GameObject showCustomButton;

	public GameObject showSubscribedButton;

	public GameObject ShowEditorPickLevelButton;

	public GameObject OpenSteamWorkshopButton;

	public GameObject ShowSubscribedLobbiesButton;

	public GameObject ShowBuiltinLobbiesButton;

	public Button PublishButton;

	public GameObject AgreementButton;

	public GameObject InvalidLevelInfoPanel;

	public GameObject workshopMissingThubnail;

	public GameObject workshopMissingDescription;

	public GameObject BackButton;

	public GameObject PageLeftButton;

	public GameObject PageRightButton;

	public GameObject steamProgress;

	public GameObject steamError;

	public TextMeshProUGUI steamErrorCode;

	public GameObject LevelDescriptionPanel;

	public Text LevelDescriptionTitle;

	public Text LevelDescriptionDescription;

	public static string selectedPath = null;

	private bool isMultiplayer;

	public Image itemListImage;

	public Color itemListNormal = new Color(0f, 0f, 0f, 49f / 255f);

	public Color itemListError = new Color(1f, 1f, 1f, 64f / 255f);

	private WorkshopMenuItem selectedMenuItem;

	private bool mLevelError;

	private bool inContinueMode;

	private int checkpointToStart = -1;

	private DLC.DLCBundles levelBundleID;

	private GameObject previousSelectedItem;

	private const int kMaxDescriptionCharacters = 4096;

	private const char kDescriptionSeparator = ':';

	private StringBuilder levelDescriptionBuilder = new StringBuilder(4096);

	private WorkshopLevelMetadata boundLevel;

	private static readonly string[] metaStringsValues = new string[3] { "WORKSHOP/RecommendedPlayers", "WORKSHOP/LevelType", "WORKSHOP/Themes" };

	private const char kBracketStart = '[';

	private const char kBracketEnd = ']';

	private const char kTagEndStart = '/';

	private const char kTagEnd = '=';

	private const int kMaxTagLength = 16;

	private static readonly TagData[] tags;

	private static bool triggerRefresh;

	public static bool InLevelSelectMenu { get; private set; }

	public static LevelSelectMenuMode displayMode { get; set; }

	private string CustomDataPath => PlayerPrefs.GetString("WorkshopRoot", Path.Combine(Application.get_dataPath(), "Workshop"));

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			boundLevel = null;
		}
	}

	private void Awake()
	{
		instance = this;
	}

	private void Start()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 localScale = PageLeftButton.get_transform().get_localScale();
		localScale.x *= -1f;
		PageLeftButton.get_transform().set_localScale(localScale);
		PlayButton.SetActive(false);
		RestartButton.SetActive(false);
	}

	private bool IsLobbyMode()
	{
		LevelSelectMenuMode levelSelectMenuMode = displayMode;
		if ((uint)(levelSelectMenuMode - 4) <= 1u)
		{
			return true;
		}
		return false;
	}

	private void OnInputDeviceChange(MenuSystem.eInputDeviceType deviceType)
	{
		if (deviceType == MenuSystem.eInputDeviceType.Mouse)
		{
			list.transitionDuration = 0.5f;
		}
		else
		{
			list.transitionDuration = 0.25f;
		}
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		MenuSystem menuSystem = MenuSystem.instance;
		menuSystem.InputDeviceChange = (Action<MenuSystem.eInputDeviceType>)Delegate.Combine(menuSystem.InputDeviceChange, new Action<MenuSystem.eInputDeviceType>(OnInputDeviceChange));
		OnInputDeviceChange(MenuSystem.instance.GetCurrentInputDevice());
		ButtonLegendBar.instance.ToogleCarouselLegend(state: true);
		list.onSelect = OnSelect;
		list.onSubmit = OnSubmit;
		list.onPointerClick = OnPointerClick;
		checkpointToStart = -1;
		if (!IsLobbyMode())
		{
			if (NetGame.instance.currentLevelType == WorkshopItemSource.Subscription)
			{
				displayMode = LevelSelectMenuMode.SubscribedWorkshop;
			}
			if (isMultiplayer && displayMode == LevelSelectMenuMode.LocalWorkshop)
			{
				displayMode = LevelSelectMenuMode.SubscribedWorkshop;
			}
		}
		NetGame.instance.transport.RegisterForGameOverlayActivation(OnGameOverlayActivation);
		Rebind();
		triggerRefresh = false;
	}

	private void OnGameOverlayActivation(byte active)
	{
		OnApplicationFocus(active == 0);
	}

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		MenuSystem menuSystem = MenuSystem.instance;
		menuSystem.InputDeviceChange = (Action<MenuSystem.eInputDeviceType>)Delegate.Remove(menuSystem.InputDeviceChange, new Action<MenuSystem.eInputDeviceType>(OnInputDeviceChange));
		ButtonLegendBar.instance.ToogleCarouselLegend(state: false);
		NetGame.instance.transport.RegisterForGameOverlayActivation(null);
		InLevelSelectMenu = false;
		list.Clear();
		if (boundLevel != null)
		{
			boundLevel.ReleaseThumbnailReference();
			boundLevel = null;
		}
	}

	private bool FolderMatch(string folder, string path)
	{
		if (folder[0] == 'l')
		{
			return folder.Equals(path);
		}
		return folder.StartsWith(path);
	}

	private bool CouldShowFindMore()
	{
		if (displayMode == LevelSelectMenuMode.Campaign || (isMultiplayer && !IsLobbyMode()))
		{
			return true;
		}
		return false;
	}

	public void Rebind()
	{
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		bool flag = displayMode == LevelSelectMenuMode.SubscribedWorkshop;
		UpdateTitle();
		DisableLevelContinue();
		bool flag2 = false;
		flag2 = SteamUser.BLoggedOn();
		InLevelSelectMenu = displayMode != 0 && displayMode != LevelSelectMenuMode.LocalWorkshop;
		showCustomButton.SetActive(false);
		showSubscribedButton.SetActive(false);
		subscribedSubtitle.SetActive(false);
		customSubtitle.SetActive(false);
		ShowLocalLevelButton.SetActive(false);
		InvalidLevelInfoPanel.SetActive(false);
		noSubscribedPrompt.SetActive(false);
		offlinePanel.SetActive(false);
		noLocalPrompt.SetActive(false);
		List<WorkshopLevelMetadata> list = null;
		switch (displayMode)
		{
		case LevelSelectMenuMode.Campaign:
			WorkshopRepository.instance.LoadBuiltinLevels(isMultiplayer && IsLobbyMode());
			list = WorkshopRepository.instance.levelRepo.BySource(IsLobbyMode() ? WorkshopItemSource.BuiltInLobbies : WorkshopItemSource.BuiltIn);
			break;
		case LevelSelectMenuMode.EditorPicks:
			WorkshopRepository.instance.LoadEditorPickLevels();
			list = WorkshopRepository.instance.levelRepo.BySource(WorkshopItemSource.EditorPick);
			break;
		case LevelSelectMenuMode.SubscribedWorkshop:
			if (flag2)
			{
				WorkshopRepository.instance.ReloadSubscriptions();
				list = WorkshopRepository.instance.levelRepo.BySource(WorkshopItemSource.Subscription);
			}
			break;
		case LevelSelectMenuMode.LocalWorkshop:
			WorkshopRepository.instance.ReloadLocalLevels();
			list = WorkshopRepository.instance.levelRepo.BySource(WorkshopItemSource.LocalWorkshop);
			break;
		case LevelSelectMenuMode.BuiltInLobbies:
			WorkshopRepository.instance.LoadBuiltinLevels(requestLobbies: true);
			list = WorkshopRepository.instance.levelRepo.BySource(WorkshopItemSource.BuiltInLobbies);
			break;
		case LevelSelectMenuMode.WorkshopLobbies:
			if (flag2)
			{
				WorkshopRepository.instance.ReloadSubscriptions(isLobby: true);
				list = WorkshopRepository.instance.levelRepo.BySource(WorkshopItemSource.SubscriptionLobbies);
			}
			break;
		}
		if (list == null)
		{
			list = new List<WorkshopLevelMetadata>();
		}
		if (!CouldShowFindMore())
		{
			FindMoreButton.SetActive(false);
		}
		customFolder.text = CustomDataPath;
		if (list.Count == 0)
		{
			this.list.Bind(list);
			((Graphic)itemListImage).set_color(itemListError);
			if (!flag2)
			{
				offlinePanel.SetActive(true);
			}
			else if (IsLobbyMode())
			{
				noSubscribedPrompt.SetActive(true);
			}
			else if (displayMode == LevelSelectMenuMode.SubscribedWorkshop)
			{
				noSubscribedPrompt.SetActive(true);
			}
			else
			{
				noLocalPrompt.SetActive(true);
			}
			EventSystem.get_current().SetSelectedGameObject(BackButton.get_gameObject());
			levelImage.SetActive(false);
			LevelDescriptionPanel.SetActive(false);
		}
		else
		{
			((Graphic)itemListImage).set_color(itemListNormal);
			levelInfoPanel.SetActive(true);
			levelImage.SetActive(!flag);
			LevelDescriptionPanel.SetActive(flag);
			this.list.Bind(list);
			if (!string.IsNullOrEmpty(selectedPath))
			{
				for (int i = 0; i < list.Count && !FolderMatch(list[i].folder, selectedPath); i++)
				{
				}
			}
			GameSave.GetLastSave(out var levelNumber, out var _, out var _, out var _);
			WorkshopItemSource lastCheckpointLevelType = GameSave.GetLastCheckpointLevelType();
			bool flag3 = false;
			if (lastCheckpointLevelType switch
			{
				WorkshopItemSource.BuiltIn => displayMode == LevelSelectMenuMode.Campaign, 
				WorkshopItemSource.EditorPick => displayMode == LevelSelectMenuMode.EditorPicks, 
				_ => false, 
			})
			{
				if (levelNumber < this.list.GetNumberItems - 1 && levelNumber != -1)
				{
					this.list.FocusItem(levelNumber);
					if (list.Count > levelNumber && list[levelNumber] != null)
					{
						BindLevel(list[levelNumber]);
					}
				}
				else
				{
					this.list.FocusItem(0);
					if (list.Count > 0 && list[0] != null)
					{
						BindLevel(list[0]);
					}
				}
			}
			else
			{
				this.list.FocusItem(0);
				if (list.Count > 0 && list[0] != null)
				{
					BindLevel(list[0]);
				}
			}
		}
		topPanel.Invalidate();
		EnableShowLevelButtons();
		BindLevelIfNeeded(selectedMenuItem);
		if ((Object)(object)previousSelectedItem != (Object)null)
		{
			((MonoBehaviour)this).StartCoroutine(WaitAndSelect());
		}
	}

	private IEnumerator WaitAndSelect()
	{
		yield return (object)new WaitForSeconds(0.25f);
		if ((Object)(object)previousSelectedItem != (Object)null)
		{
			EventSystem.get_current().SetSelectedGameObject(previousSelectedItem);
			previousSelectedItem = null;
		}
	}

	private void EnableShowLevelButtons()
	{
		bool active = false;
		bool active2 = false;
		bool active3 = false;
		bool active4 = false;
		bool active5 = false;
		bool flag = false;
		bool active6 = false;
		bool active7 = false;
		switch (displayMode)
		{
		case LevelSelectMenuMode.Campaign:
			active2 = true;
			active3 = true;
			active5 = true;
			active4 = !isMultiplayer;
			break;
		case LevelSelectMenuMode.EditorPicks:
			active = true;
			active3 = true;
			active5 = true;
			active4 = !isMultiplayer;
			break;
		case LevelSelectMenuMode.SubscribedWorkshop:
			active = true;
			active2 = true;
			active5 = true;
			active4 = !isMultiplayer;
			break;
		case LevelSelectMenuMode.LocalWorkshop:
			active = true;
			active2 = true;
			active5 = true;
			flag = true;
			break;
		case LevelSelectMenuMode.BuiltInLobbies:
			active6 = true;
			break;
		case LevelSelectMenuMode.WorkshopLobbies:
			active7 = true;
			break;
		}
		PageLeftButton.SetActive(list.isCarousel);
		PageRightButton.SetActive(list.isCarousel);
		ShowPickADreamLevelButton.SetActive(active);
		ShowEditorPickLevelButton.SetActive(active2);
		ShowSubscribedLevelButton.SetActive(active3);
		ShowLocalLevelButton.SetActive(active4);
		OpenSteamWorkshopButton.SetActive(active5);
		ShowSubscribedLobbiesButton.SetActive(active6);
		ShowBuiltinLobbiesButton.SetActive(active7);
		((Component)PublishButton).get_gameObject().SetActive(flag && list.GetNumberItems > 0);
		AgreementButton.SetActive(flag);
	}

	public void PageRight()
	{
		list.PageUp();
	}

	public void PageLeft()
	{
		list.PageDown();
	}

	public void CustomFolder()
	{
		if (MenuSystem.CanInvoke)
		{
			customSubtitle.SetActive(false);
			((Component)customFolderEditor).get_gameObject().SetActive(true);
			customFolderEditor.text = customFolder.text;
			((Selectable)customFolderEditor).Select();
		}
	}

	public void SubmitCustomFolder()
	{
		customSubtitle.SetActive(true);
		((Component)customFolderEditor).get_gameObject().SetActive(false);
		customFolder.text = customFolderEditor.text;
		PlayerPrefs.SetString("WorkshopRoot", customFolder.text);
		Rebind();
	}

	public void OpenWorkshop()
	{
		if (MenuSystem.CanInvoke)
		{
			if (IsLobbyMode())
			{
				WorkshopUpload.ShowLobbyWorkshop();
			}
			else
			{
				WorkshopUpload.ShowLevelWorkshop();
			}
		}
	}

	private void ClearSelectedItem()
	{
		boundLevel = null;
		selectedMenuItem = null;
	}

	public void SetMultiplayerMode(bool inMultiplayer)
	{
		isMultiplayer = inMultiplayer;
	}

	public void ShowPickADream()
	{
		displayMode = LevelSelectMenuMode.Campaign;
		ClearSelectedItem();
		Rebind();
	}

	public void ShowEditorPicks()
	{
		displayMode = LevelSelectMenuMode.EditorPicks;
		ClearSelectedItem();
		Rebind();
		GameSave.SeeLatestLevel();
	}

	public void ShowLobbies()
	{
		if (Game.multiplayerLobbyLevel > 128)
		{
			ShowSubscribedLobbies();
		}
		else
		{
			ShowBuiltinLobbies();
		}
	}

	private void ShowSubscribedLobbies()
	{
		selectedPath = "ws:" + Game.multiplayerLobbyLevel;
		displayMode = LevelSelectMenuMode.WorkshopLobbies;
		ClearSelectedItem();
		Rebind();
	}

	private void ShowBuiltinLobbies()
	{
		selectedPath = "builtinlobby:" + Game.multiplayerLobbyLevel;
		displayMode = LevelSelectMenuMode.BuiltInLobbies;
		ClearSelectedItem();
		Rebind();
	}

	public void ShowSubscribed()
	{
		displayMode = LevelSelectMenuMode.SubscribedWorkshop;
		ClearSelectedItem();
		Rebind();
	}

	public void ShowLocal()
	{
		displayMode = LevelSelectMenuMode.LocalWorkshop;
		ClearSelectedItem();
		Rebind();
	}

	public void WorkshopTermsClick()
	{
		if (MenuSystem.CanInvoke)
		{
			WorkshopUpload.ShowWorkshopAgreement();
		}
	}

	public void WorkshopUploadClick()
	{
		if (MenuSystem.CanInvoke && (Object)(object)selectedMenuItem != (Object)null)
		{
			SteamProgressOverlay.instance.ShowSteamProgress(showProgress: true, null, null);
			((Component)this).get_gameObject().SetActive(false);
			WorkshopUpload.Upload(selectedMenuItem.boundData, selectedMenuItem.boundData.folder, selectedMenuItem.boundData.thumbPath, "", OnPublishOver);
		}
	}

	private void OnPublishOver(WorkshopItemMetadata meta, bool needAgreement, EResult error)
	{
		GameObject currentSelectedGameObject = EventSystem.get_current().get_currentSelectedGameObject();
		if (error == EResult.k_EResultOK)
		{
			SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, null, null);
			((Component)this).get_gameObject().SetActive(true);
			Rebind();
		}
		else
		{
			if (error == EResult.k_EResultFileNotFound)
			{
				meta.workshopId = 0uL;
				meta.Save(meta.folder);
			}
			SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, "", DismissSteamError);
		}
		previousSelectedItem = currentSelectedGameObject;
	}

	public void DismissSteamError()
	{
		SteamProgressOverlay.instance.ShowSteamProgress(showProgress: false, null, null);
		((Component)this).get_gameObject().SetActive(true);
		Rebind();
	}

	public override void OnTansitionedIn()
	{
		base.OnTansitionedIn();
		MenuSystem.instance.FocusOnMouseOver(enable: false);
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public void BackClick()
	{
		if (!MenuSystem.CanInvoke || SteamProgressOverlay.instance.DialogShowing() || SteamProgressOverlay.instance.DialogErrorShowing())
		{
			return;
		}
		LevelSelectMenuMode levelSelectMenuMode = displayMode;
		if ((uint)levelSelectMenuMode <= 5u)
		{
			if (isMultiplayer)
			{
				TransitionBack<MultiplayerLobbyMenu>();
			}
			else
			{
				TransitionBack<PlayMenu>();
			}
		}
		selectedMenuItem = null;
	}

	public override void OnBack()
	{
		BackClick();
	}

	private void OnPointerClick(ListViewItem item, int clickCount)
	{
		if (clickCount > 1)
		{
			PlayClick();
		}
	}

	private void BindLevelIfNeeded(WorkshopMenuItem item)
	{
		if (!((Object)(object)item == (Object)null))
		{
			WorkshopLevelMetadata level = item.level;
			if (level != null && level.folder != null && (boundLevel == null || level.folder != boundLevel.folder))
			{
				BindLevel(level);
			}
		}
	}

	private void OnPointerClick(ListViewItem item, int clickCount, InputButton button)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Invalid comparison between Unknown and I4
		if ((int)button == 0)
		{
			SubmitItemPlay(item);
		}
		else if ((int)button == 1)
		{
			SubmitItemRestart(item);
		}
	}

	private void OnSelect(ListViewItem item)
	{
		SelectItem(item);
	}

	private void OnSubmit(ListViewItem item)
	{
		SubmitItemPlay(item);
	}

	private void SelectItem(ListViewItem item)
	{
		WorkshopMenuItem workshopMenuItem = item as WorkshopMenuItem;
		if ((Object)(object)selectedMenuItem != (Object)null)
		{
			selectedMenuItem.SetActive(active: false);
		}
		selectedMenuItem = workshopMenuItem;
		selectedMenuItem.SetActive(active: true);
		BindLevelIfNeeded(selectedMenuItem);
		bool flag = true;
		if (CouldShowFindMore())
		{
			if (DLC.instance.SupportsDLC())
			{
				levelBundleID = DLC.instance.LevelIsDLC(workshopMenuItem.level.workshopId);
				if (levelBundleID != 0)
				{
					flag = DLC.instance.BundleActive(levelBundleID);
				}
			}
			FindMoreButton.SetActive(!flag);
		}
		UpdateTitle(!flag);
	}

	private void SubmitItemPlay(ListViewItem item)
	{
		if (IsLobbyMode())
		{
			boundLevel = (item as WorkshopMenuItem).level;
			PlayClick();
		}
		else if (MenuSystem.CanInvoke && !BoundIsLobbyLevel())
		{
			WorkshopMenuItem workshopMenuItem = item as WorkshopMenuItem;
			if ((displayMode == LevelSelectMenuMode.Campaign || isMultiplayer) && DLC.instance.SupportsDLC() && !DLC.instance.LevelIsAvailable((int)workshopMenuItem.level.workshopId))
			{
				FindMoreClick();
			}
			else
			{
				Play(workshopMenuItem.level);
			}
		}
	}

	private void SubmitItemRestart(ListViewItem item)
	{
		if (!BoundIsLobbyLevel())
		{
			WorkshopMenuItem workshopMenuItem = item as WorkshopMenuItem;
			if ((displayMode == LevelSelectMenuMode.Campaign || isMultiplayer) && DLC.instance.SupportsDLC() && !DLC.instance.LevelIsAvailable((int)workshopMenuItem.level.workshopId))
			{
				FindMoreClick();
			}
			else
			{
				RestartClick();
			}
		}
	}

	private bool BoundIsLobbyLevel()
	{
		return false;
	}

	public void RestartClick()
	{
		if (!inContinueMode || !MenuSystem.CanInvoke)
		{
			return;
		}
		if (!isMultiplayer)
		{
			switch (boundLevel.levelType)
			{
			case WorkshopItemSource.BuiltIn:
				GameSave.PassCheckpointCampaign((uint)boundLevel.workshopId, -1, 0);
				break;
			case WorkshopItemSource.EditorPick:
				GameSave.PassCheckpointEditorPick((uint)boundLevel.workshopId, -1, 0);
				break;
			case WorkshopItemSource.Subscription:
			case WorkshopItemSource.LocalWorkshop:
				Game.instance.gameProgress.CustomLevelProgress(boundLevel.hash, -1);
				break;
			}
		}
		checkpointToStart = -1;
		PlayClickMain();
	}

	private void PlayClickMain()
	{
		if (IsLobbyMode())
		{
			Game.multiplayerLobbyLevel = boundLevel.workshopId;
			WorkshopRepository.instance.SetLobbyTitle(Game.multiplayerLobbyLevel);
			App.instance.LobbyLoadLevel(boundLevel.workshopId);
		}
		else if (!BoundIsLobbyLevel())
		{
			Play(boundLevel);
		}
	}

	public void PlayClick()
	{
		if (MenuSystem.CanInvoke)
		{
			PlayClickMain();
		}
	}

	private void Play(WorkshopLevelMetadata level)
	{
		if (isMultiplayer)
		{
			App.instance.ChangeLobbyLevel(level.workshopId, level.levelType);
			TransitionForward<MultiplayerLobbyMenu>();
			return;
		}
		switch (displayMode)
		{
		case LevelSelectMenuMode.Campaign:
			Game.instance.workshopLevelIsCustom = false;
			if (level.levelType == WorkshopItemSource.LocalWorkshop)
			{
				App.instance.LaunchCustomLevel(level.folder, level.levelType, (checkpointToStart != -1) ? checkpointToStart : 0, 0);
			}
			else
			{
				App.instance.LaunchSinglePlayer(level.workshopId, level.levelType, (checkpointToStart != -1) ? checkpointToStart : 0, 0);
			}
			break;
		case LevelSelectMenuMode.EditorPicks:
			App.instance.LaunchSinglePlayer(level.workshopId, level.levelType, (checkpointToStart != -1) ? checkpointToStart : 0, 0);
			break;
		case LevelSelectMenuMode.SubscribedWorkshop:
			Game.instance.workshopLevelIsCustom = false;
			App.instance.LaunchCustomLevel(level.folder, level.levelType, (checkpointToStart != -1) ? checkpointToStart : 0, 0);
			break;
		case LevelSelectMenuMode.LocalWorkshop:
			Game.instance.workshopLevelIsCustom = true;
			App.instance.LaunchCustomLevel(level.folder, level.levelType, (checkpointToStart != -1) ? checkpointToStart : 0, 0);
			break;
		}
	}

	public void FindMoreClick()
	{
		DLC.instance.OpenStorePage(levelBundleID);
	}

	private string GetMetaStrings(StringBuilder builder, MetaStrings metaString, string value)
	{
		builder.Length = 0;
		builder.Append(ScriptLocalization.Get(metaStringsValues[(int)metaString]));
		builder.Append(':');
		builder.Append(' ');
		builder.AppendLine(value);
		return builder.ToString();
	}

	private static void IdTag(StringBuilder tag, out DescriptionProcess mode, out string replaceString, out int tagIndex)
	{
		replaceString = "";
		mode = DescriptionProcess.kSearch;
		tagIndex = -1;
		bool flag = tag[0] == '/';
		string value = ((!flag) ? tag.ToString() : tag.ToString(1, tag.Length - 1));
		int num = tags.Length;
		for (int i = 0; i < num; i++)
		{
			TagData tagData = tags[i];
			if (tagData.tag.Equals(value))
			{
				mode = tagData.mode;
				replaceString = (flag ? tagData.replaceEnd : tagData.replaceStart);
				if (mode == DescriptionProcess.kSkipUntilEndIndex)
				{
					tagIndex = i;
				}
				break;
			}
		}
	}

	public static string RichTextProcess(string richText)
	{
		int num = richText.IndexOf('[');
		if (num == -1)
		{
			return richText;
		}
		int length = richText.Length;
		StringBuilder stringBuilder = new StringBuilder(richText, 0, num, length);
		StringBuilder stringBuilder2 = new StringBuilder(16);
		DescriptionProcess mode = DescriptionProcess.kSearch;
		int tagIndex = -1;
		int num2 = -1;
		bool flag = true;
		for (int i = num; i < length; i++)
		{
			char c = richText[i];
			switch (mode)
			{
			case DescriptionProcess.kSearch:
				if (c == '[')
				{
					mode = DescriptionProcess.kBracketSearch;
					stringBuilder2.Length = 0;
					continue;
				}
				break;
			case DescriptionProcess.kBracketSearch:
				if (c == ']' || c == '=')
				{
					IdTag(stringBuilder2, out mode, out var replaceString, out tagIndex);
					if (flag)
					{
						stringBuilder.Append(replaceString);
					}
				}
				else
				{
					stringBuilder2.Append(c);
				}
				continue;
			case DescriptionProcess.kSkipUntilEndIndex:
				if (!flag && tagIndex == num2)
				{
					flag = true;
					mode = DescriptionProcess.kSearch;
					num2 = -1;
					continue;
				}
				num2 = tagIndex;
				tagIndex = -1;
				flag = false;
				mode = DescriptionProcess.kSearch;
				break;
			}
			if (flag)
			{
				stringBuilder.Append(c);
			}
		}
		return stringBuilder.ToString();
	}

	private void BuildMetaAndDescription()
	{
		StringBuilder builder = new StringBuilder();
		levelDescriptionBuilder.Length = 0;
		string metaStrings = GetMetaStrings(builder, MetaStrings.kRecommendedPlayers, WorkshopUpload.TagStrings("WORKSHOP/RecommendedPlayersDesc", boundLevel.playerTags));
		levelDescriptionBuilder.Append(metaStrings);
		metaStrings = GetMetaStrings(builder, MetaStrings.kLevelType, WorkshopUpload.TagStrings("WORKSHOP/LevelTypeDesc", boundLevel.typeTags));
		levelDescriptionBuilder.Append(metaStrings);
		if (!string.IsNullOrEmpty(boundLevel.themeTags))
		{
			metaStrings = GetMetaStrings(builder, MetaStrings.kThemes, boundLevel.themeTags);
			levelDescriptionBuilder.Append(metaStrings);
		}
		levelDescriptionBuilder.Append("\n");
		levelDescriptionBuilder.AppendLine(RichTextProcess(boundLevel.description));
	}

	private void EnableLevelContinue()
	{
		inContinueMode = true;
		ButtonLegendBar.instance.SetContinueMode(inContinueMode: true);
	}

	private void DisableLevelContinue()
	{
		inContinueMode = false;
		ButtonLegendBar.instance.SetContinueMode(inContinueMode: false);
	}

	public void BindLevel(WorkshopLevelMetadata level)
	{
		if (boundLevel != null)
		{
			boundLevel.ReleaseThumbnailReference();
		}
		boundLevel = level;
		if (boundLevel != null)
		{
			selectedPath = boundLevel.folder;
			BindImage(boundLevel.thumbnailTexture);
			levelName.text = boundLevel.title;
			LevelDescriptionTitle.set_text(boundLevel.title);
			BuildMetaAndDescription();
			LevelDescriptionDescription.set_text(levelDescriptionBuilder.ToString());
			if (!isMultiplayer)
			{
				bool flag = false;
				WorkshopItemSource levelType = level.levelType;
				if (levelType - 2 <= WorkshopItemSource.EditorPick)
				{
					checkpointToStart = Game.instance.gameProgress.GetProgressWorkshop(level.hash);
					if (checkpointToStart > 0)
					{
						flag = true;
					}
				}
				else
				{
					flag = false;
				}
				if (flag)
				{
					EnableLevelContinue();
				}
				else
				{
					DisableLevelContinue();
				}
			}
		}
		else
		{
			selectedPath = null;
			BindImage(null);
			levelName.text = "MISSING";
		}
		workshopMissingThubnail.SetActive(false);
		workshopMissingDescription.SetActive(false);
		InvalidLevelInfoPanel.SetActive(false);
		levelImage.SetActive(displayMode != LevelSelectMenuMode.LocalWorkshop);
		LevelDescriptionPanel.SetActive(displayMode == LevelSelectMenuMode.LocalWorkshop);
		((Selectable)PublishButton).set_interactable(true);
		if (displayMode != LevelSelectMenuMode.LocalWorkshop)
		{
			return;
		}
		bool flag2 = (Object)(object)boundLevel.thumbnailTexture == (Object)null;
		bool flag3 = string.IsNullOrEmpty(boundLevel.description);
		bool flag4 = string.IsNullOrEmpty(boundLevel.title);
		mLevelError = flag2 || flag3 || flag4;
		if (mLevelError)
		{
			bool num = flag4 && !flag3 && !flag2;
			workshopMissingThubnail.SetActive(flag2);
			workshopMissingDescription.SetActive(flag3);
			if (!num)
			{
				levelImage.SetActive(false);
				InvalidLevelInfoPanel.SetActive(true);
				LevelDescriptionPanel.SetActive(false);
			}
			((Selectable)PublishButton).set_interactable(false);
		}
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

	public static void TriggerRefresh()
	{
		triggerRefresh = true;
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown((KeyCode)114))
		{
			RestartClick();
		}
		if (Input.GetKeyDown((KeyCode)102))
		{
			PlayClick();
		}
		if (!mLevelError && (Object)(object)PublishButton != (Object)null)
		{
			((Selectable)PublishButton).set_interactable(NetTransportSteam.sSteamServersConnected);
		}
		if (DLC.instance.SupportsDLC() && DLC.instance.Poll())
		{
			TriggerRefresh();
		}
		if (triggerRefresh)
		{
			Debug.Log((object)"triggerRefresh: ");
			triggerRefresh = false;
			Rebind();
		}
	}

	private void UpdateTitle(bool forceDLCTitle = false)
	{
		if (forceDLCTitle)
		{
			DLCAvailableTitle.SetActive(forceDLCTitle);
			campaignTitle.SetActive(false);
			editorPicksTitle.SetActive(false);
			subscribedTitle.SetActive(false);
			multiplayerTitle.SetActive(false);
			customTitle.SetActive(false);
		}
		else
		{
			campaignTitle.SetActive(displayMode == LevelSelectMenuMode.Campaign);
			editorPicksTitle.SetActive(displayMode == LevelSelectMenuMode.EditorPicks);
			subscribedTitle.SetActive(displayMode == LevelSelectMenuMode.SubscribedWorkshop);
			multiplayerTitle.SetActive(false);
			customTitle.SetActive(false);
			editorPicksTitle.GetComponent<Localize>().OnLocalize(Force: true);
			lobbyTitle.SetActive(displayMode == LevelSelectMenuMode.BuiltInLobbies);
			lobbySubscribedTitle.SetActive(displayMode == LevelSelectMenuMode.WorkshopLobbies);
			DLCAvailableTitle.SetActive(false);
		}
	}

	static LevelSelectMenu2()
	{
		TagData[] array = new TagData[6];
		TagData tagData = new TagData
		{
			tag = "h1",
			replaceStart = "<b>",
			replaceEnd = "</b>",
			mode = DescriptionProcess.kSearch
		};
		array[0] = tagData;
		tagData = new TagData
		{
			tag = "b",
			replaceStart = "<b>",
			replaceEnd = "</b>",
			mode = DescriptionProcess.kSearch
		};
		array[1] = tagData;
		tagData = new TagData
		{
			tag = "u",
			replaceStart = "<u>",
			replaceEnd = "</u>",
			mode = DescriptionProcess.kSearch
		};
		array[2] = tagData;
		tagData = new TagData
		{
			tag = "i",
			replaceStart = "<i>",
			replaceEnd = "</i>",
			mode = DescriptionProcess.kSearch
		};
		array[3] = tagData;
		tagData = new TagData
		{
			tag = "img",
			replaceStart = "",
			replaceEnd = "",
			mode = DescriptionProcess.kSkipUntilEndIndex
		};
		array[4] = tagData;
		tagData = new TagData
		{
			tag = "url",
			replaceStart = "",
			replaceEnd = "",
			mode = DescriptionProcess.kSkipUntilEndIndex
		};
		array[5] = tagData;
		tags = array;
		triggerRefresh = false;
	}
}

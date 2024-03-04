using Multiplayer;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MenuTransition
{
	public Button continueButton;

	public Button newGameButton;

	public Button newGameBigButton;

	public GameObject selectLevelButton;

	public Button workshopButton;

	public override void OnGotFocus()
	{
		((Component)workshopButton).get_gameObject().SetActive(false);
		GameSave.GetLastSave(out var levelNumber, out var checkpoint, out var _, out var _);
		bool flag = levelNumber > 0 || checkpoint > 0 || GameSave.GetLastCheckpointLevelType() != WorkshopItemSource.BuiltIn;
		bool active = GameSave.IsLevelSelectEnabled();
		bool flag2 = false;
		if (DLC.instance.SupportsDLC() && flag && !DLC.instance.LevelIsAvailable(levelNumber))
		{
			flag2 = true;
		}
		((Component)newGameBigButton).get_gameObject().SetActive(!flag);
		((Component)newGameButton).get_gameObject().SetActive(flag && flag2);
		((Component)continueButton).get_gameObject().SetActive(flag && !flag2);
		selectLevelButton.SetActive(active);
		defaultElement = ((!flag) ? ((Component)newGameBigButton).get_gameObject() : (flag2 ? ((Component)newGameButton).get_gameObject() : ((Component)continueButton).get_gameObject()));
		((Component)this).GetComponent<AutoNavigation>().Invalidate();
		base.OnGotFocus();
	}

	public void ContinueClick()
	{
		if (MenuSystem.CanInvoke)
		{
			GameSave.GetLastSave(out var levelNumber, out var checkpoint, out var subObjectives, out var _);
			App.instance.LaunchSinglePlayer((ulong)levelNumber, GameSave.GetLastCheckpointLevelType(), checkpoint, subObjectives);
		}
	}

	public void RestartClick()
	{
		GameSave.StartFromBeginning();
		ContinueClick();
	}

	public void WorkshopClick()
	{
		if (MenuSystem.CanInvoke)
		{
			LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: false);
			LevelSelectMenu2.instance.ShowSubscribed();
			TransitionForward<LevelSelectMenu2>();
		}
	}

	public void SelectLevelClick()
	{
		if (MenuSystem.CanInvoke)
		{
			LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: false);
			LevelSelectMenu2.instance.ShowPickADream();
			TransitionForward<LevelSelectMenu2>();
		}
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (NetGame.instance.local.players.Count < 2)
			{
				TransitionBack<SelectPlayersMenu>();
			}
			else
			{
				TransitionBack<SelectPlayersCoopMenu>();
			}
		}
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInMainMenu();
	}

	public override void OnBack()
	{
		BackClick();
	}
}

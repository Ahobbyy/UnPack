using Multiplayer;
using UnityEngine;

public class SelectPlayersMenu : MenuTransition
{
	public GameObject WirelessButton;

	private new void OnEnable()
	{
		base.OnEnable();
		GameObject val = GameObject.Find("FriendsButton/Alpha");
		if ((Object)(object)val != (Object)null)
		{
			val.SetActive(false);
		}
		val = GameObject.Find("OnlineButton/Alpha");
		if ((Object)(object)val != (Object)null)
		{
			val.SetActive(false);
		}
	}

	public void SingleClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (DLC.instance.SupportsDLC())
			{
				DLC.instance.Poll();
			}
			TransitionForward<PlayMenu>();
		}
	}

	public void CoopClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionForward<SelectPlayersCoopMenu>();
		}
	}

	public void FriendsClick()
	{
		if (MenuSystem.CanInvoke)
		{
			NetGame.friendly = true;
			TransitionForward<MultiplayerSelectLobbyMenu>();
		}
	}

	public void WirelessClick()
	{
	}

	public void OnlineClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionForward<MultiplayerSelectLobbyMenu>();
		}
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<MainMenu>();
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

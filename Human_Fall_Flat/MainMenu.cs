using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MenuTransition
{
	public static bool hideLogo;

	public GameObject logo;

	public ButtonGroup menuButtons;

	public VerticalLayoutGroup buttonLayout;

	public Button extrasButton;

	public Button workshopButton;

	private bool transitioning;

	public static bool InMainMenu;

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		transitioning = false;
		InMainMenu = true;
		Physics.set_autoSimulation(true);
		logo.SetActive((Object)(object)StartupExperienceUI.instance == (Object)null || !((Component)StartupExperienceUI.instance).get_gameObject().get_activeInHierarchy());
		if (TutorialRepository.instance.HasAvailableItems() || VideoRepository.instance.HasAvailableItems())
		{
			((Component)extrasButton).get_gameObject().SetActive(true);
		}
		else
		{
			((Component)extrasButton).get_gameObject().SetActive(false);
		}
		if ((Object)(object)workshopButton != (Object)null)
		{
			((Component)workshopButton).get_gameObject().SetActive(false);
		}
		menuButtons.RebuildLinks(makeExplicit: true);
	}

	protected override void Update()
	{
		bool flag = (Object)(object)StartupExperienceUI.instance == (Object)null || !((Component)StartupExperienceUI.instance).get_gameObject().get_activeInHierarchy();
		if (logo.get_activeSelf() != flag)
		{
			Debug.LogError((object)"MainMenu: Emergency fixup of logo visibility");
			logo.SetActive(flag);
		}
		base.Update();
	}

	private void DLCAvailableClick()
	{
	}

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		RemoveStartExperienceLogo();
		InMainMenu = false;
	}

	public void PlayClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			TransitionForward<SelectPlayersMenu>();
		}
	}

	public void WorkshopClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			LevelSelectMenu2.instance.SetMultiplayerMode(inMultiplayer: false);
			LevelSelectMenu2.instance.ShowSubscribed();
			TransitionForward<LevelSelectMenu2>();
		}
	}

	public void MultiplayerClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			TransitionForward<MultiplayerMenu>();
		}
	}

	public void OptionsClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			OptionsMenu.returnToPause = false;
			TransitionForward<OptionsMenu>();
		}
	}

	public void CustomizeClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			if ((Object)(object)GiftService.instance != (Object)null)
			{
				GiftService.instance.RefreshStatus();
			}
			transitioning = true;
			((MonoBehaviour)this).StartCoroutine(TransitionToCustomisation());
		}
	}

	private IEnumerator TransitionToCustomisation()
	{
		RemoveStartExperienceLogo();
		if ((Object)(object)MenuSystem.instance.activeMenu == (Object)(object)this)
		{
			MenuSystem.instance.FocusOnMouseOver(enable: false);
			Transition(-0.99f, 0.3f);
		}
		float dur = 0.5f;
		MenuCameraEffects.FadeToBlack(dur);
		yield return null;
		yield return (object)new WaitForSeconds(dur);
		yield return null;
		transitioning = false;
		MenuSystem.instance.TransitionForward<CustomizationRootMenu>(this, 0f, 10000f);
	}

	public void ExtrasClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			TransitionForward<ExtrasMenu>();
		}
	}

	public void ExitClick()
	{
		if (MenuSystem.CanInvoke && !transitioning)
		{
			TransitionForward<ConfirmQuitMenu>();
		}
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInMainMenu();
	}

	private void RemoveStartExperienceLogo()
	{
		if ((Object)(object)StartupExperienceUI.instance != (Object)null)
		{
			logo.SetActive(true);
			if ((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject() != (Object)null)
			{
				((Component)StartupExperienceUI.instance).get_gameObject().SetActive(false);
			}
			Object.Destroy((Object)(object)((Component)StartupExperienceUI.instance).get_gameObject());
		}
		else
		{
			logo.SetActive(true);
		}
	}

	private void Start()
	{
	}
}

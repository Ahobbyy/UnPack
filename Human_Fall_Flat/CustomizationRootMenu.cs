using System.Collections;
using Multiplayer;
using UnityEngine;

public class CustomizationRootMenu : MenuTransition
{
	private enum StartPhase
	{
		kNone,
		kFadeOutStart,
		kFadeOut,
		kWait,
		kFadeIn,
		kDone
	}

	public ButtonGroup menuButtons;

	private StartPhase mPhase;

	private float mPhaseTimer;

	private const float kFadeInTime = 0.5f;

	private const float kFadeOutTime = 0.25f;

	private WhiteOut mWhiteOut;

	private volatile bool mSceneLoaded;

	private bool goingBack;

	private void NewPhase(StartPhase phase)
	{
		mPhaseTimer = 0f;
		mPhase = phase;
	}

	private void SetupScene()
	{
		if ((Object)(object)CustomizationController.instance == (Object)null)
		{
			goingBack = true;
			App.instance.EnterCustomization(CustomizationControllerLoaded);
		}
		else
		{
			SetupView();
		}
	}

	public override void ApplyMenuEffects()
	{
		if ((Object)(object)CustomizationController.instance != (Object)null)
		{
			MenuCameraEffects.FadeInMainMenu();
		}
	}

	private void OnApplicationFocus(bool hasFocus)
	{
		if (hasFocus)
		{
			OnGotFocus();
		}
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		goingBack = false;
		menuButtons.RebuildLinks();
		SetupScene();
	}

	private void CustomizationControllerLoaded()
	{
		CustomizationController.instance.Initialize();
		SetupView();
		mSceneLoaded = true;
		((MonoBehaviour)this).StartCoroutine(CompleteTransitionIn());
	}

	private IEnumerator CompleteTransitionIn()
	{
		int index = 0;
		while (index < 3)
		{
			yield return null;
			int num = index + 1;
			index = num;
		}
		MenuCameraEffects.FadeFromBlack(0.25f);
		yield return (object)new WaitForSeconds(0.15f);
		Transition(0f, 0.3f);
		goingBack = false;
	}

	private static void SetupView()
	{
		CustomizationController.instance.ShowBoth();
		CustomizationController.instance.cameraController.FocusBoth();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke && !goingBack)
		{
			goingBack = true;
			((MonoBehaviour)this).StartCoroutine(StallFreeBack());
		}
	}

	private IEnumerator StallFreeBack()
	{
		float dur = 0.1f;
		if ((Object)(object)MenuSystem.instance.activeMenu == (Object)(object)this)
		{
			MenuSystem.instance.FocusOnMouseOver(enable: false);
			Transition(0.9999f, 0f);
		}
		MenuCameraEffects.FadeToBlack(dur);
		yield return null;
		yield return (object)new WaitForSeconds(dur);
		yield return null;
		App.instance.LeaveCustomization();
		while (!Game.instance.HasSceneLoaded)
		{
			yield return null;
		}
		int index = 0;
		while (index < 3)
		{
			yield return null;
			int num = index + 1;
			index = num;
		}
		goingBack = false;
		MenuSystem.instance.TransitionBack<MainMenu>(this, 0f);
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void EditClick()
	{
		if (MenuSystem.CanInvoke && !goingBack && !((Object)(object)CustomizationController.instance == (Object)null) && !((Object)(object)CustomizationController.instance.activeCustomization == (Object)null))
		{
			TransitionForward<CustomizationEditMenu>();
			CustomizationController.instance.ShowActive();
		}
	}

	public void TogglePlayerClick()
	{
		if (MenuSystem.CanInvoke && !goingBack && !((Object)(object)CustomizationController.instance == (Object)null) && !((Object)(object)CustomizationController.instance.activeCustomization == (Object)null))
		{
			CustomizationController.instance.SetActivePlayer((CustomizationController.instance.activePlayer + 1) % 2);
			CustomizationController.instance.cameraController.FocusBoth();
		}
	}

	public void SaveClick()
	{
		if (MenuSystem.CanInvoke && !goingBack && !((Object)(object)CustomizationController.instance == (Object)null) && !((Object)(object)CustomizationController.instance.activeCustomization == (Object)null))
		{
			CustomizationPresetMenu.mode = CustomizationPresetMenuMode.Save;
			CustomizationController.instance.ShowActive();
			TransitionForward<CustomizationPresetMenu>();
		}
	}

	public void LoadClick()
	{
		if (MenuSystem.CanInvoke && !goingBack && !((Object)(object)CustomizationController.instance == (Object)null) && !((Object)(object)CustomizationController.instance.activeCustomization == (Object)null))
		{
			CustomizationPresetMenu.mode = CustomizationPresetMenuMode.Load;
			TransitionForward<CustomizationPresetMenu>();
		}
	}
}

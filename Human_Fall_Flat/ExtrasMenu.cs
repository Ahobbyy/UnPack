using UnityEngine;
using UnityEngine.UI;

public class ExtrasMenu : MenuTransition
{
	public Button logButton;

	public Button videoButton;

	public Button backButton;

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		if (VideoRepository.instance.HasAvailableItems())
		{
			((Component)videoButton).get_gameObject().SetActive(true);
			Link((Selectable)(object)logButton, (Selectable)(object)videoButton);
			Link((Selectable)(object)videoButton, (Selectable)(object)backButton);
		}
		else
		{
			((Component)videoButton).get_gameObject().SetActive(false);
			Link((Selectable)(object)logButton, (Selectable)(object)backButton);
		}
		Link((Selectable)(object)backButton, (Selectable)(object)logButton);
	}

	public void TextLogClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionForward<TutorialLogMenu>();
		}
	}

	public void VideoLogClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionForward<VideoLogMenu>();
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
		MenuCameraEffects.FadeInPauseMenu();
	}

	public override void OnBack()
	{
		BackClick();
	}
}

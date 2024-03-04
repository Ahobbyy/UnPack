using UnityEditor;
using UnityEngine;

public class ConfirmQuitMenu : MenuTransition
{
	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public void ConfirmClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (Application.get_isEditor())
			{
				EditorApplication.set_isPlaying(false);
			}
			else
			{
				Application.Quit();
			}
		}
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<MainMenu>();
		}
	}

	public override void OnBack()
	{
		BackClick();
	}
}

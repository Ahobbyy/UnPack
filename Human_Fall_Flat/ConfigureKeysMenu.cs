public class ConfigureKeysMenu : MenuTransition
{
	public KeyBindingButton[] keyButtons;

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
	}

	public void ResetClick()
	{
	}

	public void RebindKeys()
	{
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<KeyboardMouseMenu>();
		}
	}

	public override void OnBack()
	{
		BackClick();
	}
}

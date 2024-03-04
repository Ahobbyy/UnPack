using UnityEngine.UI;

public class AudioMenu : MenuTransition
{
	public MenuSlider masterSlider;

	public MenuSlider fxSlider;

	public MenuSlider musicSlider;

	public MenuSlider voiceSlider;

	public MenuSelector subtitlesSelector;

	public MenuSelector shuffleSelector;

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		((Slider)masterSlider).set_value((float)Options.audioMaster);
		((Slider)fxSlider).set_value((float)Options.audioFX);
		((Slider)musicSlider).set_value((float)Options.audioMusic);
		((Slider)voiceSlider).set_value((float)Options.audioVoice);
		subtitlesSelector.SelectIndex(Options.audioSubtitles);
		shuffleSelector.SelectIndex(Options.audioShuffle);
	}

	public void MasterChanged(float value)
	{
		Options.audioMaster = (int)value;
	}

	public void FXChanged(float value)
	{
		Options.audioFX = (int)value;
	}

	public void MusicChanged(float value)
	{
		Options.audioMusic = (int)value;
	}

	public void VoiceChanged(float value)
	{
		Options.audioVoice = (int)value;
	}

	public void SubtitlesChanged(int value)
	{
		Options.audioSubtitles = value;
	}

	public void ShuffleChanged(int value)
	{
		Options.audioShuffle = value;
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<OptionsMenu>();
		}
	}

	public override void OnBack()
	{
		BackClick();
	}
}

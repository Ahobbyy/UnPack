using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AdvancedVideoMenu : MenuTransition
{
	public MenuSelector presetSelector;

	public MenuSlider cloudSlider;

	public MenuSlider shadowSlider;

	public MenuSlider textureSlider;

	public MenuSelector ambientSelector;

	public MenuSelector antialiasingSelector;

	public MenuSelector vsyncSelector;

	public MenuSelector hdrSelector;

	public MenuSelector bloomSelector;

	public MenuSelector depthSelector;

	public MenuSelector chromaSelector;

	public MenuSelector exposureSelector;

	public Button applyButton;

	private List<LayoutElement> allButtons;

	private List<float> allButtonHeights;

	public static bool hasUnappliedChanges;

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public override void OnGotFocus()
	{
		if (allButtons == null)
		{
			allButtons = new List<LayoutElement>();
			allButtonHeights = new List<float>();
			for (int i = 2; i < ((Component)applyButton).get_transform().get_parent().get_childCount() - 4; i++)
			{
				LayoutElement component = ((Component)((Component)applyButton).get_transform().get_parent().GetChild(i)).GetComponent<LayoutElement>();
				if ((Object)(object)component != (Object)null)
				{
					allButtons.Add(component);
					allButtonHeights.Add(component.get_preferredHeight());
				}
			}
		}
		base.OnGotFocus();
		if (!hasUnappliedChanges)
		{
			HideApply();
			presetSelector.SelectIndex(Options.advancedVideoPreset);
			((Slider)cloudSlider).set_value((float)Options.advancedVideoClouds);
			((Slider)shadowSlider).set_value((float)Options.advancedVideoShadows);
			((Slider)textureSlider).set_value((float)Options.advancedVideoTexture);
			ambientSelector.SelectIndex(Options.advancedVideoAO);
			antialiasingSelector.SelectIndex(Options.advancedVideoAA);
			vsyncSelector.SelectIndex(Options.advancedVideoVsync);
			hdrSelector.SelectIndex(Options.advancedVideoHDR);
			bloomSelector.SelectIndex(Options.advancedVideoBloom);
			depthSelector.SelectIndex(Options.advancedVideoDOF);
			chromaSelector.SelectIndex(Options.advancedVideoChromatic);
			exposureSelector.SelectIndex(Options.advancedVideoExposure);
		}
	}

	public void PresetChanged(int value)
	{
		if (MenuSystem.CanInvoke)
		{
			ShowApply();
			if (value != 0)
			{
				OptionsVideoPreset optionsVideoPreset = OptionsVideoPreset.Create(value);
				((Slider)cloudSlider).set_value((float)optionsVideoPreset.clouds);
				((Slider)shadowSlider).set_value((float)optionsVideoPreset.shadows);
				((Slider)textureSlider).set_value((float)optionsVideoPreset.texture);
				ambientSelector.SelectIndex(optionsVideoPreset.ao);
				antialiasingSelector.SelectIndex(optionsVideoPreset.aa);
				vsyncSelector.SelectIndex(optionsVideoPreset.vsync);
				hdrSelector.SelectIndex(optionsVideoPreset.hdr);
				bloomSelector.SelectIndex(optionsVideoPreset.bloom);
				depthSelector.SelectIndex(optionsVideoPreset.dof);
				chromaSelector.SelectIndex(optionsVideoPreset.chroma);
				exposureSelector.SelectIndex(optionsVideoPreset.exposure);
			}
		}
	}

	public void CloudChanged(float value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void ShadowChanged(float value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void TextureChanged(float value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void AmbientChanged(int value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void AntialiasingChanged(int value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void VsyncChanged(int value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void OptionChanged(int value)
	{
		if (MenuSystem.CanInvoke)
		{
			presetSelector.SelectIndex(0);
			ShowApply();
		}
	}

	public void ApplyClick()
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		if (MenuSystem.CanInvoke)
		{
			Options.advancedVideoPreset = presetSelector.selectedIndex;
			Options.advancedVideoClouds = (int)((Slider)cloudSlider).get_value();
			Options.advancedVideoShadows = (int)((Slider)shadowSlider).get_value();
			Options.advancedVideoTexture = (int)((Slider)textureSlider).get_value();
			Options.advancedVideoAO = ambientSelector.selectedIndex;
			Options.advancedVideoAA = antialiasingSelector.selectedIndex;
			Options.advancedVideoVsync = vsyncSelector.selectedIndex;
			Options.advancedVideoHDR = hdrSelector.selectedIndex;
			Options.advancedVideoBloom = bloomSelector.selectedIndex;
			Options.advancedVideoDOF = depthSelector.selectedIndex;
			Options.advancedVideoChromatic = chromaSelector.selectedIndex;
			Options.advancedVideoExposure = exposureSelector.selectedIndex;
			EventSystem obj = EventSystem.get_current();
			Navigation navigation = ((Selectable)applyButton).get_navigation();
			obj.SetSelectedGameObject(((Component)((Navigation)(ref navigation)).get_selectOnUp()).get_gameObject());
			HideApply();
			Options.ApplyAdvancedVideo();
		}
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (hasUnappliedChanges)
			{
				ConfirmDiscardVideoMenu.confirmTarget = ConfirmDiscardNavigationTarget.Video;
				ConfirmDiscardVideoMenu.backTarget = ConfirmDiscardNavigationTarget.AdvancedVideo;
				TransitionForward<ConfirmDiscardVideoMenu>();
			}
			else
			{
				TransitionBack<VideoMenu>();
			}
		}
	}

	public override void OnBack()
	{
		BackClick();
	}

	private void ShowApply()
	{
		hasUnappliedChanges = true;
		((Component)applyButton).get_gameObject().SetActive(true);
		((Component)applyButton).GetComponentInParent<AutoNavigation>().Invalidate();
	}

	private void HideApply()
	{
		hasUnappliedChanges = false;
		((Component)applyButton).get_gameObject().SetActive(false);
		if ((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() == (Object)(object)((Component)applyButton).get_gameObject())
		{
			EventSystem.get_current().SetSelectedGameObject(defaultElement);
		}
	}
}

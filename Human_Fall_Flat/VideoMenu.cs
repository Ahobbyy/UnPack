using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VideoMenu : MenuTransition
{
	public MenuSlider brightnessSlider;

	public MenuSelector resolutionSelector;

	public MenuSelector fullscreenSelector;

	public Button applyButton;

	private List<Vector2> resolutions;

	private int currentResolutionIndex = -1;

	public static bool hasUnappliedChanges;

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public override void OnGotFocus()
	{
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		base.OnGotFocus();
		if (hasUnappliedChanges)
		{
			return;
		}
		HideApply();
		((Slider)brightnessSlider).set_value((float)Options.videoBrightness);
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector((float)Screen.get_width(), (float)Screen.get_height());
		bool fullScreen = Screen.get_fullScreen();
		Resolution[] array = Screen.get_resolutions();
		resolutions = new List<Vector2>();
		List<string> list = new List<string>();
		float num = float.MaxValue;
		Vector2 val3 = default(Vector2);
		for (int i = 0; i < array.Length; i++)
		{
			Resolution val2 = array[i];
			((Vector2)(ref val3))._002Ector((float)((Resolution)(ref val2)).get_width(), (float)((Resolution)(ref val2)).get_height());
			string item = $"{((Resolution)(ref val2)).get_width()} x {((Resolution)(ref val2)).get_height()}";
			if (!list.Contains(item))
			{
				Vector2 val4 = val - val3;
				float sqrMagnitude = ((Vector2)(ref val4)).get_sqrMagnitude();
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					currentResolutionIndex = list.Count;
				}
				resolutions.Add(new Vector2((float)((Resolution)(ref val2)).get_width(), (float)((Resolution)(ref val2)).get_height()));
				list.Add(item);
			}
		}
		resolutionSelector.options = list.ToArray();
		resolutionSelector.SelectIndex(currentResolutionIndex);
		fullscreenSelector.SelectIndex(fullScreen ? 1 : 0);
	}

	public void BrightnessChanged(float value)
	{
		Options.videoBrightness = (int)value;
	}

	public void ResolutionChanged(int value)
	{
		ShowApply();
	}

	public void FullscreenChanged(int value)
	{
		ShowApply();
	}

	public void AdvancedClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (hasUnappliedChanges)
			{
				ConfirmDiscardVideoMenu.confirmTarget = ConfirmDiscardNavigationTarget.AdvancedVideo;
				ConfirmDiscardVideoMenu.backTarget = ConfirmDiscardNavigationTarget.Video;
				TransitionForward<ConfirmDiscardVideoMenu>();
			}
			else
			{
				TransitionForward<AdvancedVideoMenu>();
			}
		}
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (hasUnappliedChanges)
			{
				ConfirmDiscardVideoMenu.confirmTarget = ConfirmDiscardNavigationTarget.Options;
				ConfirmDiscardVideoMenu.backTarget = ConfirmDiscardNavigationTarget.Video;
				TransitionForward<ConfirmDiscardVideoMenu>();
			}
			else
			{
				TransitionBack<OptionsMenu>();
			}
		}
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void ApplyClick()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (MenuSystem.CanInvoke)
		{
			((MonoBehaviour)this).StartCoroutine(ForceResolution((int)resolutions[resolutionSelector.selectedIndex].x, (int)resolutions[resolutionSelector.selectedIndex].y, fullscreenSelector.selectedIndex > 0));
			EventSystem obj = EventSystem.get_current();
			Navigation navigation = ((Selectable)applyButton).get_navigation();
			obj.SetSelectedGameObject(((Component)((Navigation)(ref navigation)).get_selectOnUp()).get_gameObject());
			HideApply();
		}
	}

	private IEnumerator ForceResolution(int width, int height, bool fullScreen)
	{
		if (Screen.get_fullScreen() == fullScreen)
		{
			Screen.SetResolution(width, height, fullScreen);
		}
		else if (fullScreen)
		{
			Screen.SetResolution(width, height, fullScreen);
			yield return (object)new WaitForEndOfFrame();
			yield return (object)new WaitForEndOfFrame();
			Screen.set_fullScreen(fullScreen);
		}
		else
		{
			Screen.set_fullScreen(fullScreen);
			yield return (object)new WaitForEndOfFrame();
			yield return (object)new WaitForEndOfFrame();
			Screen.SetResolution(width, height, fullScreen);
		}
		Canvas.ForceUpdateCanvases();
	}

	private void ShowApply()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		hasUnappliedChanges = true;
		((Component)applyButton).get_gameObject().SetActive(true);
		Button above = applyButton;
		Navigation navigation = ((Selectable)applyButton).get_navigation();
		Link((Selectable)(object)above, ((Navigation)(ref navigation)).get_selectOnDown());
		navigation = ((Selectable)applyButton).get_navigation();
		Link(((Navigation)(ref navigation)).get_selectOnUp(), (Selectable)(object)applyButton);
	}

	private void HideApply()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		hasUnappliedChanges = false;
		((Component)applyButton).get_gameObject().SetActive(false);
		Navigation navigation = ((Selectable)applyButton).get_navigation();
		Selectable selectOnUp = ((Navigation)(ref navigation)).get_selectOnUp();
		navigation = ((Selectable)applyButton).get_navigation();
		Link(selectOnUp, ((Navigation)(ref navigation)).get_selectOnDown());
		if ((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() == (Object)(object)((Component)applyButton).get_gameObject())
		{
			EventSystem.get_current().SetSelectedGameObject(defaultElement);
		}
	}
}

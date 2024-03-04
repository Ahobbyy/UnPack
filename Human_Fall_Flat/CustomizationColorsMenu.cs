using System.Collections.Generic;
using HumanAPI;
using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomizationColorsMenu : MenuTransition
{
	public Transform channelsContainer;

	public Transform colorsContainer;

	public Transform colorsPanel;

	public CustomizationColorsMenuChannel channelTemplate;

	public CustomizationColorsMenuColor colorTemplate;

	public Color[] colors;

	public Color[] baseColors;

	public CustomizationColorsMenuColor transparent;

	public GameObject title;

	public RectTransform buttons;

	private List<CustomizationColorsMenuChannel> channelButtons = new List<CustomizationColorsMenuChannel>();

	private List<CustomizationColorsMenuColor> colorButtons = new List<CustomizationColorsMenuColor>();

	private CustomizationColorsMenuChannel selectedChannel;

	private Color colorBackup;

	private bool showPalette;

	private bool colorApplied;

	public override void OnTansitionedIn()
	{
		base.OnTansitionedIn();
		MenuSystem.instance.FocusOnMouseOver(enable: false);
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		CustomizationController.instance.cameraController.FocusCharacterModel();
		AddPartButtons(WorkshopItemType.ModelFull, ScriptLocalization.CUSTOMIZATION.PartBody);
		AddPartButtons(WorkshopItemType.ModelHead, ScriptLocalization.CUSTOMIZATION.PartHead);
		AddPartButtons(WorkshopItemType.ModelUpperBody, ScriptLocalization.CUSTOMIZATION.PartUpper);
		AddPartButtons(WorkshopItemType.ModelLowerBody, ScriptLocalization.CUSTOMIZATION.PartLower);
		selectedChannel = channelButtons[0];
		EventSystem.get_current().SetSelectedGameObject(((Component)selectedChannel).get_gameObject());
		AddColorButtons();
	}

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		for (int i = 0; i < channelButtons.Count; i++)
		{
			Object.Destroy((Object)(object)((Component)channelButtons[i]).get_gameObject());
		}
		channelButtons.Clear();
		if ((Object)(object)colorsContainer != (Object)null)
		{
			AutoNavigation component = ((Component)colorsContainer).GetComponent<AutoNavigation>();
			if ((Object)(object)component != (Object)null)
			{
				component.ClearCurrent();
			}
		}
		for (int j = 0; j < colorButtons.Count; j++)
		{
			Object.Destroy((Object)(object)((Component)colorButtons[j]).get_gameObject());
		}
		colorButtons.Clear();
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void BackClick()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if (MenuSystem.CanInvoke)
		{
			if (showPalette)
			{
				ShowColorPalette(show: false);
				CustomizationController.instance.SetColor(selectedChannel.part, selectedChannel.number, colorBackup);
				EventSystem.get_current().SetSelectedGameObject(((Component)selectedChannel).get_gameObject());
				colorApplied = false;
			}
			else
			{
				TransitionBack<CustomizationEditMenu>();
				CustomizationController.instance.Rollback();
			}
		}
	}

	public void ApplyClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<CustomizationEditMenu>();
			CustomizationController.instance.CommitParts();
		}
	}

	private void AddColorButtons()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Unknown result type (might be due to invalid IL or missing references)
		colors = (Color[])(object)new Color[baseColors.Length * 5];
		for (int i = 0; i < baseColors.Length; i++)
		{
			Color[] array = colors;
			int num = i;
			_ = baseColors.Length;
			array[num + 0] = baseColors[i].Desaturate(0.2f).Desaturate(0.4f).Brighten((i == 0) ? 1f : 0.4f);
			colors[i + baseColors.Length] = baseColors[i].Desaturate(0.2f).Desaturate(0.2f).Brighten(0.2f);
			colors[i + 2 * baseColors.Length] = baseColors[i].Desaturate(0.2f);
			colors[i + 3 * baseColors.Length] = baseColors[i].Desaturate(0.2f).Desaturate(0.2f).Darken(0.2f);
			colors[i + 4 * baseColors.Length] = baseColors[i].Desaturate(0.2f).Desaturate(0.4f).Darken((i == 0) ? 1f : 0.4f);
		}
		for (int j = 0; j < colors.Length; j++)
		{
			AddColorButton(colors[j]);
		}
	}

	private void AddColorButton(Color color)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		CustomizationColorsMenuColor component = Object.Instantiate<GameObject>(((Component)colorTemplate).get_gameObject(), colorsContainer).GetComponent<CustomizationColorsMenuColor>();
		((Component)component).get_gameObject().SetActive(true);
		colorButtons.Add(component);
		component.Bind(color);
	}

	private void AddPartButtons(WorkshopItemType part, string prefix)
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		if (RagdollPresetPartMetadata.IsEmpty(CustomizationController.instance.activeCustomization.preset.GetPart(part)))
		{
			return;
		}
		RagdollModel model = CustomizationController.instance.activeCustomization.GetModel(part);
		if (!((Object)(object)model == (Object)null))
		{
			if (model.mask1)
			{
				AddChannelButton(part, 1, string.Format(prefix, 1));
			}
			if (model.mask2)
			{
				AddChannelButton(part, 2, string.Format(prefix, 2));
			}
			if (model.mask3)
			{
				AddChannelButton(part, 3, string.Format(prefix, 3));
			}
			if (channelButtons.Count > 9)
			{
				title.get_gameObject().SetActive(false);
				buttons.set_pivot(new Vector2(0f, 0.3f));
			}
			else
			{
				title.get_gameObject().SetActive(true);
				buttons.set_pivot(new Vector2(0f, 0.4f));
			}
		}
	}

	private void AddChannelButton(WorkshopItemType part, int number, string label)
	{
		CustomizationColorsMenuChannel component = Object.Instantiate<GameObject>(((Component)channelTemplate).get_gameObject(), channelsContainer).GetComponent<CustomizationColorsMenuChannel>();
		((Component)component).get_gameObject().SetActive(true);
		channelButtons.Add(component);
		component.Bind(part, number, label);
	}

	public void HighlightChannel(CustomizationColorsMenuChannel channel)
	{
	}

	public void SelectChannel(CustomizationColorsMenuChannel channel)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)selectedChannel != (Object)null)
		{
			if (colorApplied)
			{
				colorApplied = false;
				CustomizationController.instance.SetColor(selectedChannel.part, selectedChannel.number, colorBackup);
			}
			selectedChannel.SetActive(active: false);
		}
		selectedChannel = channel;
		selectedChannel.SetActive(active: true);
		CustomizationController.instance.cameraController.FocusPart(channel.part);
		ShowColorPalette(show: true);
		colorBackup = CustomizationController.instance.GetColor(selectedChannel.part, selectedChannel.number);
		int num = -1;
		float num2 = (transparent.color - colorBackup).sqrMagnitude();
		for (int i = 0; i < colorButtons.Count; i++)
		{
			float num3 = (colorButtons[i].color - colorBackup).sqrMagnitude();
			if (num3 < num2)
			{
				num2 = num3;
				num = i;
			}
		}
		transparent.SetActive(-1 == num);
		for (int j = 0; j < colorButtons.Count; j++)
		{
			colorButtons[j].SetActive(j == num);
		}
		if (num >= 0)
		{
			EventSystem.get_current().SetSelectedGameObject(((Component)colorButtons[num]).get_gameObject());
		}
		else
		{
			EventSystem.get_current().SetSelectedGameObject(((Component)transparent).get_gameObject());
		}
	}

	public void SelectColor(CustomizationColorsMenuColor color)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		CustomizationController.instance.SetColor(selectedChannel.part, selectedChannel.number, color.color);
		colorApplied = true;
	}

	public void ApplyColor(CustomizationColorsMenuColor color)
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		CustomizationController.instance.SetColor(selectedChannel.part, selectedChannel.number, color.color);
		ShowColorPalette(show: false);
		EventSystem.get_current().SetSelectedGameObject(((Component)selectedChannel).get_gameObject());
		colorApplied = false;
	}

	private void ShowColorPalette(bool show)
	{
		if (showPalette != show)
		{
			showPalette = show;
			((Component)colorsPanel).get_gameObject().SetActive(show);
			selectedChannel.SetActive(show);
		}
	}

	public void LeaveColorPicker()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if (showPalette && colorApplied)
		{
			CustomizationController.instance.SetColor(selectedChannel.part, selectedChannel.number, colorBackup);
			colorApplied = false;
		}
	}
}

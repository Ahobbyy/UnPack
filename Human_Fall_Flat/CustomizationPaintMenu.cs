using System.Collections;
using System.Collections.Generic;
using HumanAPI;
using I2.Loc;
using UnityEngine;

public class CustomizationPaintMenu : MenuTransition
{
	private enum PaintMode
	{
		Blocked,
		Paint,
		Palette,
		Picker
	}

	public Transform maskPanel;

	public Transform channelsContainer;

	public CustomizationColorsMenuChannel channelTemplate;

	public MenuButton maskButton;

	public GameObject maskMarker;

	public MenuButton backfacesButton;

	public GameObject backfacesMarker;

	public float cursorSize = 0.05f;

	public float cursorHardness = 1f;

	public Color color = Color.get_white();

	public PaintCursor cursor;

	public PaintPicker picker;

	public ColorWheel colorPalette;

	private CustomizationCameraController cameraController;

	public static bool dontReload;

	public bool isActive;

	public PaintTool paint;

	public static bool sJustTransitioned;

	private bool skipPaintingUntilMouseUp;

	private bool maskVisible;

	private bool backfaces;

	private List<CustomizationColorsMenuChannel> channelButtons = new List<CustomizationColorsMenuChannel>();

	public float cursorKernel => Mathf.Lerp(0f, cursorSize, cursorHardness);

	public float cursorFalloff => Mathf.Lerp(cursorSize, 0.001f, cursorHardness);

	public override void OnGotFocus()
	{
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		base.OnGotFocus();
		CustomizationController.instance.cameraController.navigationEnabled = false;
		isActive = true;
		if (dontReload)
		{
			dontReload = false;
			return;
		}
		RagdollCustomization activeCustomization = CustomizationController.instance.activeCustomization;
		cameraController = CustomizationController.instance.cameraController;
		paint = ((Component)this).GetComponent<PaintTool>();
		paint.Initialize(activeCustomization);
		cursor.Initialize();
		picker.Initialize();
		cursor.color = (paint.color = (colorPalette.color = color));
		cursor.cursorKernel = (paint.cursorKernel = cursorKernel);
		cursor.cursorFalloff = (paint.cursorFalloff = cursorFalloff);
		maskVisible = false;
		((Component)maskPanel).get_gameObject().SetActive(maskVisible);
		ShowMask(show: true);
	}

	private void ShowMask(bool show)
	{
		ShowMask(WorkshopItemType.ModelFull, show);
		ShowMask(WorkshopItemType.ModelHead, show);
		ShowMask(WorkshopItemType.ModelUpperBody, show);
		ShowMask(WorkshopItemType.ModelLowerBody, show);
	}

	private void ShowMask(WorkshopItemType part, bool show)
	{
		RagdollModel model = CustomizationController.instance.activeCustomization.GetModel(part);
		if (!((Object)(object)model == (Object)null))
		{
			model.ShowMask(show);
			if (show)
			{
				model.SetMask(paint.GetMask(part));
			}
		}
	}

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		CustomizationController.instance.cameraController.navigationEnabled = true;
	}

	public void Teardown()
	{
		ShowMask(show: false);
		cursor.Process(show: false);
		picker.Process(show: false, pick: false);
		paint.Teardown();
		for (int i = 0; i < channelButtons.Count; i++)
		{
			Object.Destroy((Object)(object)((Component)channelButtons[i]).get_gameObject());
		}
		channelButtons.Clear();
	}

	protected override void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		base.Update();
		if (!isActive)
		{
			return;
		}
		if (!cameraController.alt)
		{
			float y = Input.get_mouseScrollDelta().y;
			if (y != 0f)
			{
				float num = Mathf.Pow(1.1f, 0f - y);
				if (cameraController.ctrl)
				{
					cursorHardness = Mathf.Clamp01(cursorHardness + y * 0.1f);
				}
				else
				{
					cursorSize = Mathf.Clamp(cursorSize * num, 0.01f, 0.2f);
				}
				cursor.cursorKernel = (paint.cursorKernel = cursorKernel);
				cursor.cursorFalloff = (paint.cursorFalloff = cursorFalloff);
			}
		}
		PaintMode paintMode = PaintMode.Paint;
		if (colorPalette.isCursorOver)
		{
			paintMode = PaintMode.Palette;
		}
		else if (cameraController.alt || cameraController.uiLock)
		{
			paintMode = PaintMode.Blocked;
		}
		else if (Input.GetKey((KeyCode)99))
		{
			paintMode = PaintMode.Picker;
		}
		if (!cameraController.lmb)
		{
			skipPaintingUntilMouseUp = false;
		}
		switch (paintMode)
		{
		case PaintMode.Paint:
			if (cameraController.lmb && !skipPaintingUntilMouseUp)
			{
				paint.Paint();
				cursor.Process(show: false);
			}
			else
			{
				paint.EndStroke();
				cursor.Process(show: true);
			}
			picker.Process(show: false, pick: false);
			break;
		case PaintMode.Palette:
			paint.EndStroke();
			cursor.Process(show: false);
			picker.Process(show: true, pick: false);
			break;
		case PaintMode.Picker:
			paint.EndStroke();
			cursor.Process(show: false);
			picker.Process(show: true, pick: true);
			break;
		case PaintMode.Blocked:
			paint.EndStroke();
			cursor.Process(show: false);
			picker.Process(show: false, pick: false);
			break;
		}
		if (paintMode == PaintMode.Paint && !paint.inStroke && Game.GetKey((KeyCode)306) && Game.GetKeyDown((KeyCode)122))
		{
			if (Input.GetKey((KeyCode)304))
			{
				paint.Redo();
			}
			else
			{
				paint.Undo();
			}
		}
		else if (paintMode == PaintMode.Paint && !paint.inStroke && Game.GetKey((KeyCode)306) && Game.GetKeyDown((KeyCode)121))
		{
			paint.Redo();
		}
	}

	public void PreviewColor(Color color)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (Game.GetKeyDown((KeyCode)115))
		{
			PickColor(color);
		}
		else
		{
			picker.color = color;
		}
	}

	public void PickColor(Color color)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		picker.color = (cursor.color = (paint.color = (colorPalette.color = color)));
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (paint.inStroke)
			{
				skipPaintingUntilMouseUp = true;
				paint.CancelStroke();
			}
			else if (paint.hasChanges)
			{
				TransitionForward<CustomizationConfirmDiscardMenu>();
				isActive = false;
			}
			else
			{
				TransitionBack<CustomizationEditMenu>();
				isActive = false;
				CustomizationController.instance.Rollback();
				Teardown();
			}
			SetupMaskButton(visible: false);
		}
	}

	public void ConfirmDiscard()
	{
		CustomizationController.instance.Rollback();
		Teardown();
	}

	public void SaveClick()
	{
		if (MenuSystem.CanInvoke)
		{
			((MonoBehaviour)this).StartCoroutine(Save());
		}
	}

	private IEnumerator Save()
	{
		SubtitleManager.instance.SetProgress(ScriptLocalization.TUTORIAL.SAVING, 1f, 1f);
		yield return null;
		paint.Commit();
		CustomizationController.instance.ClearColors();
		CustomizationController.instance.CommitParts();
	}

	private void SetupMaskButton(bool visible)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		maskButton.isOn = visible;
		((Component)maskButton).get_transform().set_localScale(visible ? (Vector3.get_one() * 1.05f) : Vector3.get_one());
		maskMarker.SetActive(visible);
	}

	public void MaskClick()
	{
		if (!MenuSystem.CanInvoke)
		{
			return;
		}
		maskVisible = !maskVisible;
		((Component)maskPanel).get_gameObject().SetActive(maskVisible);
		SetupMaskButton(maskVisible);
		if (maskVisible && channelButtons.Count == 0)
		{
			AddPartButtons(WorkshopItemType.ModelFull, ScriptLocalization.CUSTOMIZATION.PartBody);
			AddPartButtons(WorkshopItemType.ModelHead, ScriptLocalization.CUSTOMIZATION.PartHead);
			AddPartButtons(WorkshopItemType.ModelUpperBody, ScriptLocalization.CUSTOMIZATION.PartUpper);
			AddPartButtons(WorkshopItemType.ModelLowerBody, ScriptLocalization.CUSTOMIZATION.PartLower);
		}
		if (sJustTransitioned)
		{
			for (int i = 0; i < channelButtons.Count; i++)
			{
				channelButtons[i].SetActive(active: true);
			}
			sJustTransitioned = false;
		}
		((Component)maskPanel).GetComponent<AutoNavigation>().ClearCurrent();
	}

	public void BackfacesClick()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		if (MenuSystem.CanInvoke)
		{
			backfaces = !backfaces;
			backfacesButton.isOn = backfaces;
			((Component)backfacesButton).get_transform().set_localScale(backfaces ? (Vector3.get_one() * 1.05f) : Vector3.get_one());
			backfacesMarker.SetActive(backfaces);
			paint.paintBackface = backfaces;
		}
	}

	public void WebcamClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionForward<CustomizationWebcamMenu>();
			isActive = false;
		}
	}

	private void AddPartButtons(WorkshopItemType part, string prefix)
	{
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
		}
	}

	private void AddChannelButton(WorkshopItemType part, int number, string label)
	{
		CustomizationColorsMenuChannel component = Object.Instantiate<GameObject>(((Component)channelTemplate).get_gameObject(), channelsContainer).GetComponent<CustomizationColorsMenuChannel>();
		((Component)component).get_gameObject().SetActive(true);
		channelButtons.Add(component);
		component.Bind(part, number, label);
	}

	public void MaskButtonToggle(CustomizationColorsMenuChannel channel)
	{
		channel.SetActive(!channel.GetActive());
		paint.SetMask(channel.part, channel.number, channel.GetActive());
	}
}

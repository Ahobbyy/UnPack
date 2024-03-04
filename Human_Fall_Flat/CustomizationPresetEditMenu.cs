using System;
using HumanAPI;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationPresetEditMenu : MenuTransition
{
	public TMP_InputField title;

	public TMP_InputField description;

	public GameObject titleArea;

	public GameObject descriptionArea;

	private RagdollCustomization ragdollCustomization;

	public static RagdollPresetMetadata targetPreset;

	public GameObject titleText;

	public GameObject descriptionText;

	public GameObject buttonSave;

	public GameObject buttonBack;

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		EventSystem.get_current().set_sendNavigationEvents(true);
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		EventSystem.get_current().set_sendNavigationEvents(false);
		RagdollPresetMetadata skin = CustomizationController.instance.GetSkin();
		title.text = ((targetPreset != null) ? targetPreset.title : skin.title);
		description.text = ((targetPreset != null) ? targetPreset.description : skin.description);
	}

	public override void OnTansitionedIn()
	{
		base.OnTansitionedIn();
		MenuSystem.instance.FocusOnMouseOver(enable: false);
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			CustomizationPresetMenu.dontReload = true;
			TransitionBack<CustomizationPresetMenu>();
		}
	}

	private bool Up()
	{
		if (Input.GetKeyDown((KeyCode)273))
		{
			return true;
		}
		return false;
	}

	private bool Down()
	{
		if (Input.GetKeyDown((KeyCode)274))
		{
			return true;
		}
		return false;
	}

	private bool Tab()
	{
		if (Input.GetKeyDown((KeyCode)9))
		{
			return true;
		}
		return false;
	}

	private bool Select()
	{
		if (Input.GetKeyDown((KeyCode)271) || Input.GetKeyDown((KeyCode)32) || Input.GetKeyDown((KeyCode)13))
		{
			return true;
		}
		return false;
	}

	protected override void Update()
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		base.Update();
		GameObject currentSelectedGameObject = EventSystem.get_current().get_currentSelectedGameObject();
		Selectable val = null;
		if (Up())
		{
			val = currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnUp();
		}
		if (Down())
		{
			val = currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();
		}
		if ((Object)(object)val != (Object)null)
		{
			EventSystem.get_current().SetSelectedGameObject(((Component)val).get_gameObject());
		}
		Tab();
		val = null;
		if (Select())
		{
			Button component = currentSelectedGameObject.GetComponent<Button>();
			if ((Object)(object)component != (Object)null)
			{
				BaseEventData val2 = new BaseEventData(EventSystem.get_current());
				component.OnSubmit(val2);
			}
		}
	}

	public void SaveClick()
	{
		if (MenuSystem.CanInvoke)
		{
			int length = Math.Min(128, title.text.Length);
			string text = title.text.Substring(0, length);
			int length2 = Math.Min(7999, description.text.Length);
			string text2 = description.text.Substring(0, length2);
			CustomizationSavedMenu.preset = CustomizationController.instance.SavePreset(targetPreset, text, text2);
			TransitionForward<CustomizationSavedMenu>();
		}
	}
}

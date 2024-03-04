using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogOverlay : MonoBehaviour
{
	private static DialogOverlay instance;

	public Image background;

	public GameObject progressRing;

	public TextMeshProUGUI titleText;

	public TextMeshProUGUI descriptionText;

	public TextMeshProUGUI backText;

	public GameObject backButton;

	private static Action onBack;

	private int hideDelay;

	public static int backHideDelay;

	public static bool IsShowing { get; private set; }

	private void Awake()
	{
		instance = this;
		((Component)instance.background).get_gameObject().SetActive(false);
		hideDelay = 0;
	}

	public static bool IsOn()
	{
		if (!((Object)(object)instance != (Object)null))
		{
			return false;
		}
		if (((Component)instance.background).get_gameObject().get_activeSelf())
		{
			return instance.hideDelay == 0;
		}
		return false;
	}

	public static bool IsOnIncludingDelay()
	{
		if (!((Object)(object)instance != (Object)null))
		{
			return false;
		}
		return ((Component)instance.background).get_gameObject().get_activeSelf();
	}

	public static float GetOpacity()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		if (!IsOnIncludingDelay())
		{
			return 0f;
		}
		return ((Graphic)instance.background).get_color().a;
	}

	public static string GetCurrentTitle()
	{
		if (!IsOn() || (Object)(object)instance == (Object)null || (Object)(object)instance.titleText == (Object)null)
		{
			return null;
		}
		return instance.titleText.text;
	}

	public static void DisableCancel()
	{
		onBack = null;
		instance.backButton.SetActive(false);
	}

	public static void Show(float opacity, bool showProgress, string title, string description, string backLabel, Action backAction)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		instance.hideDelay = 0;
		IsShowing = true;
		MenuSystem.keyboardState = KeyboardState.Dialog;
		((Graphic)instance.background).set_color(new Color(0f, 0f, 0f, opacity));
		((Component)instance.background).get_gameObject().SetActive(true);
		((Component)instance).get_transform().SetAsLastSibling();
		instance.progressRing.SetActive(showProgress);
		instance.titleText.text = title;
		if (string.IsNullOrEmpty(description))
		{
			((Component)instance.descriptionText).get_gameObject().SetActive(false);
		}
		else
		{
			((Component)instance.descriptionText).get_gameObject().SetActive(true);
			instance.descriptionText.text = description;
		}
		if (string.IsNullOrEmpty(backLabel))
		{
			instance.backButton.SetActive(false);
			EventSystem.get_current().SetSelectedGameObject(instance.backButton);
			if (Object.op_Implicit((Object)(object)MenuSystem.instance) && Object.op_Implicit((Object)(object)MenuSystem.instance.activeMenu))
			{
				MenuSystem.instance.activeMenu.lastFocusedElement = instance.backButton;
			}
			onBack = backAction;
			backHideDelay = 0;
		}
		else
		{
			instance.backText.text = backLabel;
			onBack = backAction;
			backHideDelay = 0;
			instance.backButton.SetActive(true);
			EventSystem.get_current().SetSelectedGameObject(instance.backButton);
			if (Object.op_Implicit((Object)(object)MenuSystem.instance) && Object.op_Implicit((Object)(object)MenuSystem.instance.activeMenu))
			{
				MenuSystem.instance.activeMenu.lastFocusedElement = instance.backButton;
			}
		}
		ButtonLegendBar.RefreshStatus();
	}

	public void BackClick()
	{
		if (hideDelay <= 0 && MenuSystem.CustomCanInvoke(checkMenuState: false, checkNetworkState: false) && onBack != null)
		{
			Action action = onBack;
			Hide(backHideDelay);
			action();
		}
	}

	public static void Hide(int withDelay = 0)
	{
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		bool activeSelf = ((Component)instance.background).get_gameObject().get_activeSelf();
		IsShowing = false;
		MenuSystem.keyboardState = KeyboardState.None;
		((Component)instance.background).get_gameObject().SetActive(false);
		onBack = null;
		instance.hideDelay = Math.Max(withDelay, instance.hideDelay);
		if (activeSelf && instance.hideDelay > 0)
		{
			((Component)instance.background).get_gameObject().SetActive(true);
			((Graphic)instance.background).set_color(new Color(0f, 0f, 0f, 1f));
			instance.progressRing.SetActive(false);
			instance.titleText.text = string.Empty;
			((Component)instance.descriptionText).get_gameObject().SetActive(false);
			instance.backButton.SetActive(false);
		}
		ButtonLegendBar.RefreshStatus();
	}

	private void Update()
	{
		if (hideDelay > 0 && --hideDelay == 0)
		{
			((Component)instance.background).get_gameObject().SetActive(false);
			ButtonLegendBar.RefreshStatus();
		}
	}

	public DialogOverlay()
		: this()
	{
	}
}

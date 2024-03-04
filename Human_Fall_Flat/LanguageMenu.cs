using I2.Loc;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LanguageMenu : MenuTransition
{
	public Button english;

	public Button french;

	public Button spanish;

	public Button german;

	public Button russian;

	public Button italian;

	public Button chineseSimplified;

	public Button japanese;

	public Button korean;

	public Button brazilianPortuguese;

	public Button turkish;

	public Button thai;

	public Button indonesian;

	public Button polish;

	public Button ukrainian;

	public Button arabic;

	public Button portuguese;

	public Button lithuanian;

	public Button chineseTraditional;

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		switch (LocalizationManager.CurrentLanguage.ToLower())
		{
		case "english":
			EventSystem.get_current().SetSelectedGameObject(((Component)english).get_gameObject());
			break;
		case "french":
			EventSystem.get_current().SetSelectedGameObject(((Component)french).get_gameObject());
			break;
		case "spanish":
			EventSystem.get_current().SetSelectedGameObject(((Component)spanish).get_gameObject());
			break;
		case "german":
			EventSystem.get_current().SetSelectedGameObject(((Component)german).get_gameObject());
			break;
		case "russian":
			EventSystem.get_current().SetSelectedGameObject(((Component)russian).get_gameObject());
			break;
		case "italian":
			EventSystem.get_current().SetSelectedGameObject(((Component)italian).get_gameObject());
			break;
		case "chinese simplified":
			EventSystem.get_current().SetSelectedGameObject(((Component)chineseSimplified).get_gameObject());
			break;
		case "japanese":
			EventSystem.get_current().SetSelectedGameObject(((Component)japanese).get_gameObject());
			break;
		case "korean":
			EventSystem.get_current().SetSelectedGameObject(((Component)korean).get_gameObject());
			break;
		case "brazilian portuguese":
			EventSystem.get_current().SetSelectedGameObject(((Component)brazilianPortuguese).get_gameObject());
			break;
		case "turkish":
			EventSystem.get_current().SetSelectedGameObject(((Component)turkish).get_gameObject());
			break;
		case "thai":
			EventSystem.get_current().SetSelectedGameObject(((Component)thai).get_gameObject());
			break;
		case "indonesian":
			EventSystem.get_current().SetSelectedGameObject(((Component)indonesian).get_gameObject());
			break;
		case "polish":
			EventSystem.get_current().SetSelectedGameObject(((Component)polish).get_gameObject());
			break;
		case "ukrainian":
			EventSystem.get_current().SetSelectedGameObject(((Component)ukrainian).get_gameObject());
			break;
		case "arabic":
			EventSystem.get_current().SetSelectedGameObject(((Component)arabic).get_gameObject());
			break;
		case "portuguese":
			EventSystem.get_current().SetSelectedGameObject(((Component)portuguese).get_gameObject());
			break;
		case "lithuanian":
			EventSystem.get_current().SetSelectedGameObject(((Component)lithuanian).get_gameObject());
			break;
		case "chinese taiwan":
			EventSystem.get_current().SetSelectedGameObject(((Component)chineseTraditional).get_gameObject());
			break;
		}
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	public void English()
	{
		SetLanguage("English");
	}

	public void French()
	{
		SetLanguage("French");
	}

	public void Spanish()
	{
		SetLanguage("Spanish");
	}

	public void German()
	{
		SetLanguage("German");
	}

	public void Russian()
	{
		SetLanguage("Russian");
	}

	public void Italian()
	{
		SetLanguage("Italian");
	}

	public void ChineseSimplified()
	{
		SetLanguage("Chinese Simplified");
	}

	public void Japanese()
	{
		SetLanguage("Japanese");
	}

	public void Korean()
	{
		SetLanguage("Korean");
	}

	public void BrazilianPortuguese()
	{
		SetLanguage("Brazilian Portuguese");
	}

	public void Turkish()
	{
		SetLanguage("Turkish");
	}

	public void Thai()
	{
		SetLanguage("Thai");
	}

	public void Indonesian()
	{
		SetLanguage("Indonesian");
	}

	public void Polish()
	{
		SetLanguage("Polish");
	}

	public void Ukrainian()
	{
		SetLanguage("Ukrainian");
	}

	public void Arabic()
	{
		SetLanguage("Arabic");
	}

	public void Portuguese()
	{
		SetLanguage("Portuguese");
	}

	public void Lithuanian()
	{
		SetLanguage("Lithuanian");
	}

	public void ChineseTraditional()
	{
		SetLanguage("Chinese Taiwan");
	}

	private void SetLanguage(string language)
	{
		if (MenuSystem.CanInvoke)
		{
			LocalizationManager.CurrentLanguage = language;
			LocalizeOnAwake[] array = (LocalizeOnAwake[])(object)Resources.FindObjectsOfTypeAll(typeof(LocalizeOnAwake));
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Localize();
			}
			PlayerPrefs.SetString("Language", language);
			TransitionForward<OptionsMenu>();
		}
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

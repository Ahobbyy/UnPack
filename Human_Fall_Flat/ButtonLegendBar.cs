using System;
using System.Text;
using I2.Loc;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLegendBar : MonoBehaviour
{
	public GameObject LegendBar;

	public SplitScreenNotification splitScreenNotification;

	public MultiplayerMenuOptions multiplayerMenuOptions;

	public LobbySelectMenuOptions lobbySelectMenuOptions;

	private static bool showSplitScreenNotification;

	private static bool showPlayerActions;

	public static ButtonLegendBar instance;

	public GameObject restartObject;

	public TextMeshProUGUI PlayText;

	public TextMeshProUGUI playGlyph;

	public TextMeshProUGUI restartGlyph;

	public TextMeshProUGUI backGlyph;

	public GameObject CarrouselOptionsContainer;

	private float playGlyphInitialFontSize;

	private const float kPlayGlyphFontSizeMultiplyerText = 0.6f;

	private StringBuilder glyphStringbuilder;

	private void Awake()
	{
		instance = this;
		glyphStringbuilder = new StringBuilder();
	}

	private void Start()
	{
		playGlyphInitialFontSize = playGlyph.fontSize;
		ToogleCarouselLegend(state: false);
	}

	private void OnDestroy()
	{
		if ((Object)(object)instance == (Object)(object)this)
		{
			instance = null;
		}
	}

	private void OnEnable()
	{
		MenuSystem menuSystem = MenuSystem.instance;
		menuSystem.InputDeviceChange = (Action<MenuSystem.eInputDeviceType>)Delegate.Combine(menuSystem.InputDeviceChange, new Action<MenuSystem.eInputDeviceType>(OnInputDeviceChanged));
	}

	private void OnDisable()
	{
		MenuSystem menuSystem = MenuSystem.instance;
		menuSystem.InputDeviceChange = (Action<MenuSystem.eInputDeviceType>)Delegate.Remove(menuSystem.InputDeviceChange, new Action<MenuSystem.eInputDeviceType>(OnInputDeviceChanged));
	}

	public static void RefreshStatus()
	{
		if ((Object)(object)instance != (Object)null)
		{
			instance.Update();
		}
	}

	public static void SetShowSplitScreenNotification(bool value)
	{
		showSplitScreenNotification = true;
	}

	public static void SetShowPlayerActions(bool value)
	{
		showPlayerActions = true;
		Debug.Log((object)("Setting player actions - " + value));
	}

	private void Update()
	{
		bool active = PlayerManager.instance.SecondPlayerStatus != PlayerManager.PlayerStatus.Online && !DLCMenu.InDLCMenu;
		((Component)splitScreenNotification).get_gameObject().SetActive(active);
		splitScreenNotification.SplitRefreshEnable(LevelSelectMenu2.InLevelSelectMenu || CustomizationPresetMenu.InCustomizationPresetMenu);
		splitScreenNotification.DLCAvailableEnable(MainMenu.InMainMenu && DLC.instance.SupportsDLC());
		((Component)lobbySelectMenuOptions).get_gameObject().SetActive(MultiplayerSelectLobbyMenu.InLobbySelectMenu);
		((Component)multiplayerMenuOptions).get_gameObject().SetActive(multiplayerMenuOptions.ShouldShow);
		bool flag = false;
		MenuTransition activeMenu = MenuSystem.instance.activeMenu;
		if ((Object)(object)activeMenu != (Object)null && !DialogOverlay.IsOnIncludingDelay() && App.state != AppSate.LoadLevel)
		{
			flag = true;
			if (activeMenu is MultiplayerErrorMenu || activeMenu is ConfirmMenu)
			{
				flag = false;
			}
		}
		if (SteamProgressOverlay.instance.DialogShowing())
		{
			flag = false;
		}
		if (LegendBar.get_activeSelf() != flag)
		{
			LegendBar.SetActive(flag);
		}
	}

	private void OnInputDeviceChanged(MenuSystem.eInputDeviceType deviceType)
	{
		UpdateLegendGlyphs(deviceType);
	}

	public void ToogleCarouselLegend(bool state)
	{
		UpdateLegendGlyphs(MenuSystem.instance.GetCurrentInputDevice());
		CarrouselOptionsContainer.SetActive(state);
	}

	private void UpdateLegendGlyphs(MenuSystem.eInputDeviceType deviceType)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0198: Unknown result type (might be due to invalid IL or missing references)
		switch (deviceType)
		{
		case MenuSystem.eInputDeviceType.Controller:
		{
			TextMeshProUGUI textMeshProUGUI9 = playGlyph;
			TextMeshProUGUI textMeshProUGUI10 = backGlyph;
			float num2 = (restartGlyph.fontSize = playGlyphInitialFontSize);
			float num5 = (textMeshProUGUI9.fontSize = (textMeshProUGUI10.fontSize = num2));
			TextMeshProUGUI textMeshProUGUI11 = playGlyph;
			TextMeshProUGUI textMeshProUGUI12 = backGlyph;
			Color blue;
			((Graphic)restartGlyph).set_color(blue = Color.get_black());
			Color color;
			((Graphic)textMeshProUGUI12).set_color(color = blue);
			((Graphic)textMeshProUGUI11).set_color(color);
			playGlyph.text = "\u20fd\n";
			backGlyph.text = "\u20fe\n";
			restartGlyph.text = "℈\n";
			break;
		}
		case MenuSystem.eInputDeviceType.Keyboard:
		{
			TextMeshProUGUI textMeshProUGUI5 = playGlyph;
			TextMeshProUGUI textMeshProUGUI6 = backGlyph;
			float num2 = (restartGlyph.fontSize = playGlyphInitialFontSize * 0.6f);
			float num5 = (textMeshProUGUI5.fontSize = (textMeshProUGUI6.fontSize = num2));
			TextMeshProUGUI textMeshProUGUI7 = playGlyph;
			TextMeshProUGUI textMeshProUGUI8 = backGlyph;
			Color blue;
			((Graphic)restartGlyph).set_color(blue = Color.get_blue());
			Color color;
			((Graphic)textMeshProUGUI8).set_color(color = blue);
			((Graphic)textMeshProUGUI7).set_color(color);
			playGlyph.text = CreateBracketedKeyGlyph("F");
			backGlyph.text = CreateBracketedKeyGlyph("ESC");
			restartGlyph.text = CreateBracketedKeyGlyph("R");
			break;
		}
		case MenuSystem.eInputDeviceType.Mouse:
		{
			TextMeshProUGUI textMeshProUGUI = playGlyph;
			TextMeshProUGUI textMeshProUGUI2 = backGlyph;
			float num2 = (restartGlyph.fontSize = playGlyphInitialFontSize * 0.6f);
			float num5 = (textMeshProUGUI.fontSize = (textMeshProUGUI2.fontSize = num2));
			TextMeshProUGUI textMeshProUGUI3 = playGlyph;
			TextMeshProUGUI textMeshProUGUI4 = backGlyph;
			Color blue;
			((Graphic)restartGlyph).set_color(blue = Color.get_blue());
			Color color;
			((Graphic)textMeshProUGUI4).set_color(color = blue);
			((Graphic)textMeshProUGUI3).set_color(color);
			playGlyph.text = CreateBracketedKeyGlyph(ScriptLocalization.Get("MENU.LEVELSELECT/LMB"));
			backGlyph.text = CreateBracketedKeyGlyph("ESC");
			restartGlyph.text = CreateBracketedKeyGlyph(ScriptLocalization.Get("MENU.LEVELSELECT/RMB"));
			break;
		}
		}
	}

	private string CreateBracketedKeyGlyph(string glyph)
	{
		glyphStringbuilder.Length = 0;
		glyphStringbuilder.Append("[");
		glyphStringbuilder.Append(glyph);
		glyphStringbuilder.Append("]");
		glyphStringbuilder.Append("  \n");
		return glyphStringbuilder.ToString();
	}

	public void SetContinueMode(bool inContinueMode)
	{
		restartObject.SetActive(inContinueMode);
		PlayText.text = (inContinueMode ? ScriptLocalization.Get("MENU/PLAY/CONTINUE") : ScriptLocalization.Get("MENU.LEVELSELECT/PLAY"));
	}

	public ButtonLegendBar()
		: this()
	{
	}
}

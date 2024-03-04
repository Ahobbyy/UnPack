using System;
using Multiplayer;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuSystem : MonoBehaviour
{
	public enum eInputDeviceType
	{
		Mouse,
		Keyboard,
		Controller
	}

	public GameObject[] pagePrefabs;

	public static MenuSystem instance;

	private EventSystem eventSystem;

	[NonSerialized]
	public MenuTransition activeMenu;

	public MenuSystemState state;

	public GameObject mouseBlocker;

	private bool coopShown;

	public const float defaultFadeOutTime = 0.3f;

	public const float defaultFadeInTime = 0.3f;

	private bool useMenuInput;

	private bool focusOnMouseOver = true;

	private bool mouseMode;

	private bool controllerLastInput;

	public static KeyboardState keyboardState;

	private static float timeSinceLast;

	private eInputDeviceType mDevice = eInputDeviceType.Controller;

	public Action<eInputDeviceType> InputDeviceChange;

	public static bool CanInvoke => CustomCanInvoke();

	public static bool CanInvokeFromGame => CustomCanInvoke(checkMenuState: false);

	private void Awake()
	{
		for (int i = 0; i < pagePrefabs.Length; i++)
		{
			if ((Object)(object)pagePrefabs[i] != (Object)null)
			{
				Object.Instantiate<GameObject>(pagePrefabs[i], ((Component)this).get_transform(), false);
			}
		}
	}

	private void OnEnable()
	{
		state = MenuSystemState.Inactive;
		instance = this;
		eventSystem = Object.FindObjectOfType<EventSystem>();
		MenuTransition[] componentsInChildren = ((Component)this).GetComponentsInChildren<MenuTransition>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			((Component)componentsInChildren[i]).get_gameObject().SetActive(false);
		}
	}

	public void TransitionForward<T>(MenuTransition from, float fadeOutTime = 0.3f, float fadeInTime = 0.3f) where T : MenuTransition
	{
		T menu = GetMenu<T>();
		if (!((Object)(object)menu == (Object)null))
		{
			FadeOutForward(from, fadeOutTime);
			FadeInForward(menu, fadeInTime);
		}
	}

	public void TransitionBack<T>(MenuTransition from, float fadeOutTime = 0.3f, float fadeInTime = 0.3f) where T : MenuTransition
	{
		T menu = GetMenu<T>();
		if (!((Object)(object)menu == (Object)null))
		{
			FadeOutBack(from, fadeOutTime);
			FadeInBack(menu, fadeInTime);
		}
	}

	public T GetMenu<T>() where T : MenuTransition
	{
		T[] componentsInChildren = ((Component)this).GetComponentsInChildren<T>(true);
		if (componentsInChildren.Length == 0)
		{
			throw new InvalidOperationException("Did not find menu" + typeof(T));
		}
		return componentsInChildren[0];
	}

	public void FadeInBack(MenuTransition to, float fadeInTime = 0.3f)
	{
		((Component)to).get_gameObject().SetActive(true);
		to.Transition(-1f, 0f);
		to.Transition(0f, fadeInTime);
		to.ApplyMenuEffects();
		activeMenu = to;
		to.OnGotFocus();
	}

	public void FadeInForward(MenuTransition to, float fadeInTime = 0.3f)
	{
		((Component)to).get_gameObject().SetActive(true);
		to.Transition(1f, 0f);
		to.Transition(0f, fadeInTime);
		to.ApplyMenuEffects();
		activeMenu = to;
		to.OnGotFocus();
	}

	public void MenuTransitionedIn(MenuTransition menu)
	{
		FocusOnMouseOver(enable: true);
		menu.OnTansitionedIn();
	}

	public void FadeOutForward(MenuTransition from, float fadeOutTime = 0.3f)
	{
		FocusOnMouseOver(enable: false);
		from.Transition(-1f, fadeOutTime);
		from.OnLostFocus();
	}

	public void FadeOutActive()
	{
		state = MenuSystemState.Inactive;
		if ((Object)(object)activeMenu != (Object)null)
		{
			activeMenu.FadeOutForward();
		}
	}

	public void FadeOutBack(MenuTransition from, float fadeOutTime = 0.3f)
	{
		FocusOnMouseOver(enable: false);
		from.Transition(1f, fadeOutTime);
		from.OnLostFocus();
	}

	public void EnterMenuInputMode()
	{
		useMenuInput = true;
	}

	public void ExitMenuInputMode()
	{
		useMenuInput = false;
	}

	public void FocusOnMouseOver(bool enable)
	{
		focusOnMouseOver = enable;
	}

	public void ShowPauseMenu()
	{
		state = MenuSystemState.PauseMenu;
		MenuTransition to = ((Game.instance.currentLevelNumber == Game.instance.levelCount) ? ((MenuTransition)GetMenu<CreditsMenu>()) : ((MenuTransition)GetMenu<PauseMenu>()));
		if (!NetGame.isLocal)
		{
			to = GetMenu<MultiplayerPauseMenu>();
		}
		FadeInForward(to);
		EnterMenuInputMode();
		SubtitleManager.instance.Hide();
	}

	public void OnShowingMainMenu()
	{
		state = MenuSystemState.MainMenu;
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human.all[i].Reset();
		}
	}

	internal void ShowMainMenu(bool hideLogo = false, bool hideOldMenu = false)
	{
		App.state = AppSate.Menu;
		if ((Object)(object)activeMenu == (Object)null)
		{
			MainMenu menu = GetMenu<MainMenu>();
			FadeInForward(menu);
		}
		else if (hideOldMenu)
		{
			TransitionForward<MainMenu>(activeMenu, 0f);
		}
		else
		{
			TransitionForward<MainMenu>(activeMenu);
		}
		EnterMenuInputMode();
		SubtitleManager.instance.Hide();
		OnShowingMainMenu();
	}

	public void ShowMainMenu<T>(bool hideOldMenu = false) where T : MenuTransition
	{
		if ((Object)(object)activeMenu == (Object)null)
		{
			T menu = GetMenu<T>();
			FadeInForward(menu);
		}
		else if (hideOldMenu)
		{
			activeMenu.TransitionForward<T>(0f);
		}
		else
		{
			activeMenu.TransitionForward<T>();
		}
		EnterMenuInputMode();
		SubtitleManager.instance.Hide();
		OnShowingMainMenu();
	}

	public void ShowMenu<T>(bool hideOldMenu = false) where T : MenuTransition
	{
		state = MenuSystemState.MainMenu;
		if ((Object)(object)activeMenu == (Object)null)
		{
			T menu = GetMenu<T>();
			FadeInForward(menu);
		}
		else if (hideOldMenu)
		{
			activeMenu.TransitionForward<T>(0f);
		}
		else
		{
			activeMenu.TransitionForward<T>();
		}
		EnterMenuInputMode();
		SubtitleManager.instance.Hide();
	}

	public void HideMenus()
	{
		MenuCameraEffects.FadeOut();
		MenuTransition menuTransition = activeMenu;
		instance.ExitMenus();
		if ((Object)(object)menuTransition != (Object)null)
		{
			menuTransition.FadeOutBack();
		}
	}

	public void ExitMenus()
	{
		if ((Object)(object)activeMenu != (Object)null)
		{
			activeMenu.Transition(1f, 1f);
			activeMenu.Transition(1f, 0f);
		}
		state = MenuSystemState.Inactive;
		activeMenu = null;
		ExitMenuInputMode();
		SubtitleManager.instance.Show();
	}

	private void OnGUI()
	{
		UpdateInputDeviceType();
		BindCursor();
	}

	private void BindCursor(bool force = true)
	{
	}

	public void OnApplicationFocus(bool focus)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (focus)
		{
			BindCursor();
			return;
		}
		Cursor.set_visible(true);
		Cursor.set_lockState((CursorLockMode)0);
		Cursor.SetCursor((Texture2D)null, Vector2.get_zero(), (CursorMode)0);
		mouseBlocker.SetActive(false);
	}

	private void Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		if (state == MenuSystemState.MainMenu && NetGame.instance.players.Count == 1 && !NetGame.isNetStarted)
		{
			Human human = Human.all[0];
			if (((Component)human).get_transform().get_position().y < 50f)
			{
				human.SetPosition(new Vector3(0f, 100f, 0f));
			}
		}
		timeSinceLast += Time.get_unscaledDeltaTime();
		if (!((Object)(object)activeMenu == (Object)null) && keyboardState == KeyboardState.None)
		{
			if ((Object)(object)EventSystem.get_current().get_currentSelectedGameObject() != (Object)null && EventSystem.get_current().get_currentSelectedGameObject().get_activeInHierarchy() && (Object)(object)EventSystem.get_current().get_currentSelectedGameObject().GetComponentInParent<MenuTransition>() == (Object)(object)activeMenu)
			{
				activeMenu.lastFocusedElement = EventSystem.get_current().get_currentSelectedGameObject();
			}
			else if ((Object)(object)activeMenu.lastFocusedElement != (Object)null && activeMenu.lastFocusedElement.get_gameObject().get_activeInHierarchy())
			{
				EventSystem.get_current().SetSelectedGameObject(activeMenu.lastFocusedElement);
			}
		}
	}

	public static bool CustomCanInvoke(bool checkMenuState = true, bool checkNetworkState = true, bool checkTimeSinceLast = true)
	{
		if (checkMenuState && instance.state == MenuSystemState.Inactive)
		{
			return false;
		}
		if (checkNetworkState && NetGame.instance.transport.ShouldInhibitUIExceptCancel())
		{
			return false;
		}
		if (checkTimeSinceLast && timeSinceLast <= 0.1f)
		{
			return false;
		}
		timeSinceLast = 0f;
		return true;
	}

	public eInputDeviceType GetCurrentInputDevice()
	{
		return mDevice;
	}

	private void UpdateInputDeviceType()
	{
		if (mDevice != eInputDeviceType.Controller && ControlerInput())
		{
			SetInputDevice(eInputDeviceType.Controller);
		}
		else if (mDevice != 0 && MouseInput())
		{
			SetInputDevice(eInputDeviceType.Mouse);
		}
		else if (mDevice != eInputDeviceType.Keyboard && KeyboardInput())
		{
			SetInputDevice(eInputDeviceType.Keyboard);
		}
	}

	private void SetInputDevice(eInputDeviceType device)
	{
		mDevice = device;
		InputDeviceChange(mDevice);
	}

	private bool MouseInput()
	{
		if (Event.get_current().get_isMouse())
		{
			return true;
		}
		if (Input.GetAxis("Mouse X") != 0f || Input.GetAxis("Mouse Y") != 0f)
		{
			return true;
		}
		return false;
	}

	private bool KeyboardInput()
	{
		return Event.get_current().get_isKey();
	}

	private bool ControlerInput()
	{
		if (Input.GetKey((KeyCode)330) || Input.GetKey((KeyCode)331) || Input.GetKey((KeyCode)332) || Input.GetKey((KeyCode)333) || Input.GetKey((KeyCode)334) || Input.GetKey((KeyCode)335) || Input.GetKey((KeyCode)336) || Input.GetKey((KeyCode)337) || Input.GetKey((KeyCode)338) || Input.GetKey((KeyCode)339) || Input.GetKey((KeyCode)340) || Input.GetKey((KeyCode)341) || Input.GetKey((KeyCode)342) || Input.GetKey((KeyCode)343) || Input.GetKey((KeyCode)344) || Input.GetKey((KeyCode)345) || Input.GetKey((KeyCode)346) || Input.GetKey((KeyCode)347) || Input.GetKey((KeyCode)348) || Input.GetKey((KeyCode)349))
		{
			return true;
		}
		Input.GetJoystickNames();
		if (Math.Abs(Input.GetAxis("joystick 1 analog 0")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 1")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 2")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 5")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 6")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 7")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 8")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 9")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 10")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 11")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 12")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 13")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 14")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 15")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 16")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 17")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 18")) > 0.5f || Math.Abs(Input.GetAxis("joystick 1 analog 19")) > 0.5f)
		{
			return true;
		}
		return false;
	}

	public MenuSystem()
		: this()
	{
	}
}

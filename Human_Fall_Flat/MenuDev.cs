using Multiplayer;
using UnityEngine;

public class MenuDev : MonoBehaviour
{
	private MenuTransition statupMenu;

	private void Awake()
	{
		MenuTransition[] array = Object.FindObjectsOfType<MenuTransition>();
		for (int i = 0; i < array.Length; i++)
		{
			if (((Component)array[i]).get_gameObject().get_activeSelf())
			{
				statupMenu = array[i];
			}
		}
	}

	private void Start()
	{
		Dependencies.Initialize<App>();
		((Component)((Component)this).GetComponentInChildren<Camera>()).get_gameObject().SetActive(false);
		App.instance.BeginMenuDev();
		if ((Object)(object)statupMenu == (Object)null)
		{
			statupMenu = MenuSystem.instance.GetMenu<MainMenu>();
		}
		MenuSystem.instance.FadeInForward(statupMenu);
		MenuSystem.instance.EnterMenuInputMode();
		SubtitleManager.instance.Hide();
		MenuSystem.instance.OnShowingMainMenu();
	}

	public MenuDev()
		: this()
	{
	}
}

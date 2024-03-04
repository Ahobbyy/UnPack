using System.Collections;
using Multiplayer;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectPlayersCoopMenu : MenuTransition
{
	public Button backButton;

	public Button playButton;

	public Button reconfigureButton;

	public GameObject explanation;

	public TextMeshProUGUI leftPrompt;

	public TextMeshProUGUI rightPrompt;

	public static Coroutine coroutine;

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		coroutine = ((MonoBehaviour)this).StartCoroutine(ConfigureCoop());
	}

	protected override void Update()
	{
		base.Update();
		if (coroutine == null && (Object)(object)MenuSystem.instance.activeMenu == (Object)(object)this && NetGame.instance.local.players.Count < 2)
		{
			coroutine = ((MonoBehaviour)this).StartCoroutine(ConfigureCoop());
		}
	}

	private IEnumerator ConfigureCoop()
	{
		yield return null;
		coroutine = null;
	}

	public void ContinueClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (coroutine != null)
			{
				((MonoBehaviour)this).StopCoroutine(coroutine);
				coroutine = null;
			}
			TransitionForward<PlayMenu>();
		}
	}

	public void ReconfigureClick()
	{
		PlayerManager.SetSingle();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			if (coroutine != null)
			{
				((MonoBehaviour)this).StopCoroutine(coroutine);
				coroutine = null;
			}
			PlayerManager.SetSingle();
			TransitionBack<SelectPlayersMenu>();
		}
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInCoopMenu();
	}

	public override void OnBack()
	{
		BackClick();
	}
}

using System.Collections;
using Multiplayer;
using UnityEngine;

public class Hint : MonoBehaviour
{
	public float delay;

	private HintRemote remote;

	private static Hint activeHint;

	private Coroutine timerCoroutine;

	internal bool wasActivated;

	public bool disabled;

	private void Awake()
	{
		remote = ((Component)this).GetComponentInChildren<HintRemote>();
		if (!NetGame.isClient)
		{
			((Component)remote).GetComponent<NetBody>().SetVisible(visible: false);
		}
	}

	public void TriggerHint()
	{
		if (!wasActivated && !NetGame.isClient)
		{
			if ((Object)(object)activeHint != (Object)null)
			{
				activeHint.StopHint();
			}
			activeHint = this;
			timerCoroutine = ((MonoBehaviour)this).StartCoroutine(HintCoroutine());
		}
	}

	private IEnumerator HintCoroutine()
	{
		wasActivated = true;
		float timer2 = delay;
		timer2 /= 4f;
		while (timer2 > 0f)
		{
			if (Game.instance.state == GameState.PlayingLevel)
			{
				timer2 -= Time.get_deltaTime();
			}
			else if (Game.instance.state != GameState.Paused)
			{
				yield break;
			}
			yield return null;
		}
		if (StillValid())
		{
			((Component)remote).GetComponent<NetBody>().SetVisible(visible: true);
			((Component)remote).GetComponent<NetBody>().Respawn();
			timerCoroutine = null;
		}
	}

	public void StopHint()
	{
		wasActivated = true;
		if (timerCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(timerCoroutine);
		}
		timerCoroutine = null;
	}

	protected virtual bool StillValid()
	{
		return true;
	}

	public Hint()
		: this()
	{
	}
}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuTransition : MonoBehaviour
{
	public GameObject defaultElement;

	public GameObject lastFocusedElement;

	public bool selectLastFocused = true;

	private RectTransform rect;

	private CanvasGroup group;

	private float start;

	private float target;

	private float current;

	private float phase = 1f;

	private float speed = 1f;

	protected virtual void OnEnable()
	{
		rect = ((Component)this).GetComponent<RectTransform>();
		group = ((Component)this).GetComponent<CanvasGroup>();
	}

	public void Transition(float newTarget, float duration)
	{
		if (duration == 0f)
		{
			phase = 1f;
			current = newTarget;
			target = newTarget;
			Apply();
		}
		else
		{
			target = newTarget;
			start = current;
			phase = 0f;
			speed = 1f / duration;
		}
	}

	protected virtual void Update()
	{
		if (phase < 1f)
		{
			phase = Mathf.MoveTowards(phase, 1f, speed * Mathf.Min((speed > 0f) ? 0.333333343f : 1f, Time.get_unscaledDeltaTime()));
			current = Ease.easeInOutQuad(start, target, phase);
			Apply();
			if (phase == 1f && target == 0f)
			{
				MenuSystem.instance.MenuTransitionedIn(this);
			}
		}
		else if (target == 1f || target == -1f)
		{
			((Component)this).get_gameObject().SetActive(false);
		}
	}

	private void Apply()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)group != (Object)null)
		{
			group.set_alpha(1f - Mathf.Abs(current));
		}
		Vector3 localPosition = ((Transform)rect).get_localPosition();
		localPosition.z = current * 2000f;
		((Transform)rect).set_localPosition(localPosition);
		((Transform)rect).set_localRotation(Quaternion.Euler(0f, (0f - current) * 60f, 0f));
	}

	public virtual void OnLostFocus()
	{
	}

	public virtual void OnGotFocus()
	{
		EventSystem.get_current().SetSelectedGameObject((GameObject)null);
		if (!selectLastFocused || (Object)(object)lastFocusedElement == (Object)null || !lastFocusedElement.get_gameObject().get_activeInHierarchy())
		{
			lastFocusedElement = defaultElement;
		}
		EventSystem.get_current().SetSelectedGameObject(lastFocusedElement);
	}

	public virtual void OnBack()
	{
	}

	public virtual void ApplyMenuEffects()
	{
	}

	public void TransitionForward<T>(float fadeOutTime = 0.3f, float fadeInTime = 0.3f) where T : MenuTransition
	{
		MenuSystem.instance.TransitionForward<T>(this, fadeOutTime, fadeInTime);
	}

	public void TransitionBack<T>(float fadeOutTime = 0.3f, float fadeInTime = 0.3f) where T : MenuTransition
	{
		if (!(MenuSystem.instance.activeMenu is SelectPlayersMenu))
		{
			lastFocusedElement = null;
		}
		MenuSystem.instance.TransitionBack<T>(this, fadeOutTime, fadeInTime);
	}

	public void FadeOutForward()
	{
		MenuSystem.instance.FadeOutForward(this);
	}

	public void FadeOutBack()
	{
		MenuSystem.instance.FadeOutBack(this);
	}

	public void Link(Selectable above, Selectable below, bool makeExplicit = false)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Navigation navigation = above.get_navigation();
		((Navigation)(ref navigation)).set_selectOnDown(below);
		if (makeExplicit)
		{
			((Navigation)(ref navigation)).set_mode((Mode)4);
		}
		above.set_navigation(navigation);
		navigation = below.get_navigation();
		((Navigation)(ref navigation)).set_selectOnUp(above);
		if (makeExplicit)
		{
			((Navigation)(ref navigation)).set_mode((Mode)4);
		}
		below.set_navigation(navigation);
	}

	public virtual void OnTansitionedIn()
	{
	}

	public MenuTransition()
		: this()
	{
	}
}

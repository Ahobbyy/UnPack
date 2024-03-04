using HumanAPI;
using UnityEngine;

public class HintText : MonoBehaviour
{
	public Sound2 onSound;

	public Sound2 offSound;

	private RectTransform rect;

	private float transitionSpeed;

	private float transitionPhase;

	private bool inTransition;

	public bool isVisible;

	private void Awake()
	{
		rect = ((Component)this).GetComponent<RectTransform>();
	}

	private void Update()
	{
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		if (inTransition)
		{
			transitionPhase = Mathf.Clamp01(transitionPhase + Time.get_deltaTime() * transitionSpeed);
			float num = Ease.easeInOutQuad(0f, 1f, transitionPhase);
			((Transform)rect).set_localScale(new Vector3(num, 1f, 1f));
			if (transitionPhase == 0f)
			{
				inTransition = false;
				((Component)this).get_gameObject().SetActive(false);
			}
		}
	}

	internal void Show()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		isVisible = true;
		((Component)this).get_gameObject().SetActive(true);
		((Transform)rect).set_localScale(new Vector3(0f, 1f, 1f));
		inTransition = true;
		transitionSpeed = 2f;
		onSound.PlayOneShot();
	}

	internal void Hide()
	{
		isVisible = false;
		offSound.PlayOneShot();
		inTransition = true;
		transitionSpeed = -2f;
	}

	public HintText()
		: this()
	{
	}
}

using System.Collections;
using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
	public static SubtitleManager instance;

	public TextMeshProUGUI subtitleText;

	public TextMeshProUGUI instructionText;

	public TextMeshProUGUI progressText;

	public GameObject progressRing;

	public GameObject recordingLight;

	private float subtitleDismiss;

	private float instructionDismiss;

	private float progressDismiss;

	private float progressRingDismiss;

	private Coroutine dismissCoroutine;

	private bool subtitlesEnabled = true;

	private void OnEnable()
	{
		instance = this;
	}

	private void Start()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		subtitleText.text = "";
		instructionText.text = "";
		progressText.text = "";
		progressRing.SetActive(false);
		recordingLight.SetActive(false);
		((Component)instructionText).GetComponent<RectTransform>().set_anchoredPosition(Vector2.get_zero());
	}

	public void SetSubtitle(string text)
	{
		subtitleText.text = text;
		subtitleDismiss = 2.14748365E+09f;
	}

	public void SetSubtitle(string text, float dismissTime)
	{
		subtitleText.text = text;
		subtitleDismiss = ((dismissTime != 0f) ? dismissTime : 2.14748365E+09f);
		if (dismissCoroutine == null)
		{
			dismissCoroutine = ((MonoBehaviour)this).StartCoroutine(DismissCoroutine());
		}
	}

	public void ClearSubtitle()
	{
		SetSubtitle("");
	}

	public void SetInstruction(string text)
	{
		instructionText.text = text;
	}

	public void SetInstruction(string text, float dismissTime)
	{
		instructionText.text = text;
		instructionDismiss = ((dismissTime != 0f) ? dismissTime : 2.14748365E+09f);
		if (dismissCoroutine == null)
		{
			dismissCoroutine = ((MonoBehaviour)this).StartCoroutine(DismissCoroutine());
		}
	}

	public void ClearInstruction(float dismissTime)
	{
		instructionDismiss = dismissTime;
		if (dismissCoroutine == null)
		{
			dismissCoroutine = ((MonoBehaviour)this).StartCoroutine(DismissCoroutine());
		}
	}

	public void ClearInstruction()
	{
		SetInstruction("");
	}

	public void SetRecording()
	{
		recordingLight.SetActive(true);
	}

	public void ClearRecording()
	{
		recordingLight.SetActive(false);
	}

	public void SetProgress(string text)
	{
		progressText.text = text;
		progressDismiss = 2.14748365E+09f;
		progressRingDismiss = 2.14748365E+09f;
		progressRing.SetActive(true);
	}

	public void SetProgress(string text, float dismissTime, float ringTime)
	{
		progressText.text = text;
		progressDismiss = ((dismissTime != 0f) ? dismissTime : 2.14748365E+09f);
		progressRingDismiss = ((ringTime != 0f) ? ringTime : 2.14748365E+09f);
		progressRing.SetActive(true);
		if (dismissCoroutine == null)
		{
			dismissCoroutine = ((MonoBehaviour)this).StartCoroutine(DismissCoroutine());
		}
	}

	public bool GetRingState()
	{
		return progressRing.get_activeSelf();
	}

	public bool CheckProgressText(string cmpString)
	{
		return progressText.text == cmpString;
	}

	public void ClearProgress()
	{
		SetProgress("");
		progressRing.SetActive(false);
	}

	private IEnumerator DismissCoroutine()
	{
		while (instructionDismiss != 2.14748365E+09f || subtitleDismiss != 2.14748365E+09f || progressDismiss != 2.14748365E+09f)
		{
			if (subtitleDismiss != 2.14748365E+09f)
			{
				subtitleDismiss -= Time.get_deltaTime();
				if (subtitleDismiss <= 0f)
				{
					subtitleDismiss = 2.14748365E+09f;
					ClearSubtitle();
				}
			}
			if (instructionDismiss != 2.14748365E+09f)
			{
				instructionDismiss -= Time.get_deltaTime();
				if (instructionDismiss <= 0f)
				{
					instructionDismiss = 2.14748365E+09f;
					ClearInstruction();
				}
			}
			if (progressDismiss != 2.14748365E+09f)
			{
				progressRingDismiss -= Time.get_deltaTime();
				progressDismiss -= Time.get_deltaTime();
				if (progressRingDismiss <= 0f)
				{
					progressRing.SetActive(false);
				}
				if (progressDismiss <= 0f)
				{
					progressDismiss = 2.14748365E+09f;
					ClearProgress();
				}
			}
			yield return null;
		}
		dismissCoroutine = null;
	}

	public void Hide()
	{
		((Component)instructionText).get_gameObject().SetActive(false);
		((Component)subtitleText).get_gameObject().SetActive(false);
	}

	public void Show()
	{
		((Component)instructionText).get_gameObject().SetActive(true);
		((Component)subtitleText).get_gameObject().SetActive(subtitlesEnabled);
	}

	internal void EnableSubtitles(bool enable)
	{
		subtitlesEnabled = enable;
	}

	public void PlayNarrative(AudioClip clip)
	{
		((Component)this).GetComponent<AudioSource>().PlayOneShot(clip);
	}

	public SubtitleManager()
		: this()
	{
	}
}

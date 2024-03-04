using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReplayUI : MonoBehaviour
{
	public static ReplayUI instance;

	public Image progress;

	public RectTransform[] cameraIndicators;

	public int totalFrames;

	private CanvasGroup group;

	public float lastActivity;

	private void Awake()
	{
		instance = this;
		((Component)this).get_gameObject().SetActive(false);
		HideCameras();
		group = ((Component)this).GetComponent<CanvasGroup>();
	}

	public void Show(int totalFrames)
	{
		if (this.totalFrames != totalFrames)
		{
			lastActivity = Time.get_time();
		}
		this.totalFrames = totalFrames;
		((Component)this).get_gameObject().SetActive(true);
	}

	public void Hide()
	{
		((Component)this).get_gameObject().SetActive(false);
	}

	public void Update()
	{
		float fillAmount = progress.get_fillAmount();
		progress.set_fillAmount(1f * (float)ReplayRecorder.instance.currentFrame / (float)totalFrames);
		if (fillAmount != progress.get_fillAmount())
		{
			lastActivity = Time.get_time();
		}
		float num = Time.get_time() - lastActivity;
		group.set_alpha(Mathf.InverseLerp(1f, 0.5f, num));
	}

	public void SyncCameras(FreeRoamCam.CameraKeyFrame[] keyframes)
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		lastActivity = Time.get_time();
		Vector2 val = default(Vector2);
		for (int i = 0; i < 9; i++)
		{
			FreeRoamCam.CameraKeyFrame cameraKeyFrame = keyframes[i];
			if (totalFrames > 0 && cameraKeyFrame.frame >= 0f)
			{
				((Component)cameraIndicators[i]).get_gameObject().SetActive(true);
				RectTransform obj = cameraIndicators[i];
				RectTransform obj2 = cameraIndicators[i];
				((Vector2)(ref val))._002Ector(1f * cameraKeyFrame.frame / (float)totalFrames, 1f);
				obj2.set_anchorMax(val);
				obj.set_anchorMin(val);
				TextMeshProUGUI componentInChildren = ((Component)cameraIndicators[i]).GetComponentInChildren<TextMeshProUGUI>();
				if (cameraKeyFrame.targetFocus)
				{
					componentInChildren.text = i + 1 + (cameraKeyFrame.humanRelative ? "h" : "t");
				}
				else
				{
					componentInChildren.text = (i + 1).ToString();
				}
			}
			else
			{
				((Component)cameraIndicators[i]).get_gameObject().SetActive(false);
			}
		}
	}

	public void HideCameras()
	{
		lastActivity = Time.get_time();
		for (int i = 0; i < 9; i++)
		{
			((Component)cameraIndicators[i]).get_gameObject().SetActive(false);
		}
	}

	public ReplayUI()
		: this()
	{
	}
}

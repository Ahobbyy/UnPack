using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CustomizationWebcamMenu : MenuTransition
{
	public GameObject prepareUI;

	public MenuButton mirrorButton;

	public GameObject mirrorMarker;

	public TextMeshProUGUI timer;

	[NonSerialized]
	public WebCamTexture webCam;

	public Texture2D fakeTexture;

	private CustomizationPaintMenu paintMenu;

	private int mirror = 1;

	private int desiredCameraIndex;

	private string desiredCameraName;

	public bool isStreaming;

	public override void OnGotFocus()
	{
		paintMenu = MenuSystem.instance.GetMenu<CustomizationPaintMenu>();
		base.OnGotFocus();
		MenuButton component = lastFocusedElement.GetComponent<MenuButton>();
		if ((Object)(object)component != (Object)null)
		{
			((Selectable)component).OnSelect((BaseEventData)null);
		}
		MenuCameraEffects.FadeInWebcamMenu();
		prepareUI.SetActive(true);
		desiredCameraIndex = PlayerPrefs.GetInt("WebCamIdx", 0);
		desiredCameraName = PlayerPrefs.GetString("WebCamName", (string)null);
		mirror = PlayerPrefs.GetInt("WebCamMirror", 1);
		if (mirror == 0)
		{
			mirror = 1;
		}
		StartWebCam();
	}

	public override void OnBack()
	{
		BackClick();
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			paintMenu.paint.CancelStroke();
			Teardown();
			CustomizationPaintMenu.dontReload = true;
			TransitionBack<CustomizationPaintMenu>();
		}
	}

	public void Teardown()
	{
		((Component)timer).get_gameObject().SetActive(false);
		MenuCameraEffects.FadeInMainMenu();
		ReleaseWebCam();
		paintMenu.paint.StopStream();
	}

	public void TakePictureClick()
	{
		if (MenuSystem.CanInvoke)
		{
			((MonoBehaviour)this).StartCoroutine(TakePicture());
		}
	}

	public void SwitchCameraClick()
	{
		if (MenuSystem.CanInvoke && WebCamTexture.get_devices() != null && WebCamTexture.get_devices().Length != 0)
		{
			ReleaseWebCam();
			desiredCameraIndex = (desiredCameraIndex + 1) % WebCamTexture.get_devices().Length;
			desiredCameraName = null;
			StartWebCam();
		}
	}

	public void MirrorToggleClick()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		if (MenuSystem.CanInvoke)
		{
			mirror *= -1;
			mirrorButton.isOn = mirror < 0;
			((Component)mirrorButton).get_transform().set_localScale((mirror < 0) ? (Vector3.get_one() * 1.05f) : Vector3.get_one());
			mirrorMarker.SetActive(mirror < 0);
			PlayerPrefs.SetInt("WebCamMirror", mirror);
		}
	}

	private IEnumerator TakePicture()
	{
		prepareUI.SetActive(false);
		((Component)timer).get_gameObject().SetActive(true);
		timer.text = "5";
		yield return (object)new WaitForSeconds(1f);
		timer.text = "4";
		yield return (object)new WaitForSeconds(1f);
		timer.text = "3";
		yield return (object)new WaitForSeconds(1f);
		timer.text = "2";
		yield return (object)new WaitForSeconds(1f);
		timer.text = "1";
		yield return (object)new WaitForSeconds(1f);
		timer.text = "0";
		StopWebCam();
		yield return (object)new WaitForSeconds(1f);
		((Component)timer).get_gameObject().SetActive(false);
		TransitionForward<CustomizationWebcamDoneMenu>();
	}

	protected override void Update()
	{
		base.Update();
		if (isStreaming)
		{
			paintMenu.paint.Project((Texture)(object)webCam, mirror);
		}
	}

	private void StartWebCam()
	{
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		if ((Object)(object)webCam != (Object)null)
		{
			webCam.Play();
		}
		else
		{
			if (WebCamTexture.get_devices() == null || WebCamTexture.get_devices().Length == 0)
			{
				CustomizeNoDeviceMenu.reason = CustomizeNoDeviceMenuReason.NoWebcam;
				TransitionForward<CustomizeNoDeviceMenu>();
				return;
			}
			if (!string.IsNullOrEmpty(desiredCameraName))
			{
				for (int i = 0; i < WebCamTexture.get_devices().Length; i++)
				{
					if (((WebCamDevice)(ref WebCamTexture.get_devices()[i])).get_name() == desiredCameraName)
					{
						desiredCameraIndex = i;
					}
				}
			}
			for (int j = 0; j < WebCamTexture.get_devices().Length; j++)
			{
				desiredCameraName = ((WebCamDevice)(ref WebCamTexture.get_devices()[desiredCameraIndex])).get_name();
				webCam = new WebCamTexture(desiredCameraName);
				if ((Object)(object)webCam != (Object)null)
				{
					webCam.Play();
					if (webCam.get_isPlaying())
					{
						PlayerPrefs.SetInt("WebCamIdx", desiredCameraIndex);
						PlayerPrefs.SetString("WebCamName", desiredCameraName);
						break;
					}
				}
				desiredCameraIndex++;
			}
		}
		if ((Object)(object)webCam == (Object)null)
		{
			CustomizeNoDeviceMenu.reason = CustomizeNoDeviceMenuReason.NoWebcam;
			TransitionForward<CustomizeNoDeviceMenu>();
		}
		else
		{
			paintMenu.paint.BeginStream();
			isStreaming = true;
		}
	}

	private void StopWebCam()
	{
		isStreaming = false;
		if ((Object)(object)webCam != (Object)null)
		{
			webCam.Pause();
		}
	}

	private void ReleaseWebCam()
	{
		isStreaming = false;
		if ((Object)(object)webCam != (Object)null)
		{
			webCam.Stop();
			webCam = null;
		}
	}
}

using System.Collections.Generic;
using I2.Loc;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoLogMenu : MenuTransition
{
	public RawImage image;

	public AudioSource audioSource;

	private VideoLogButton selectedItem;

	private VideoPlayer video;

	private SrtSubtitles subtitles;

	public TextMeshProUGUI subtitleText;

	private Texture thumbnailTexture;

	private bool videoStarted;

	private SrtSubtitles.SrtLine currentLine;

	private float audiosourceStart;

	public VideoLogButton SelectedItem => selectedItem;

	private float audiosourceTime
	{
		get
		{
			float num = audioSource.get_time();
			if (float.IsInfinity(num) || float.IsNaN(num))
			{
				num = 1f * (float)audioSource.get_timeSamples() / 48000f;
			}
			if (num == 0f)
			{
				return Time.get_unscaledTime() - audiosourceStart;
			}
			return num;
		}
	}

	public override void OnGotFocus()
	{
		base.OnGotFocus();
		VideoLogButton[] componentsInChildren = ((Component)this).GetComponentsInChildren<VideoLogButton>(true);
		List<VideoRepositoryItem> list = VideoRepository.instance.ListAvailableItems();
		videoStarted = false;
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			VideoLogButton videoLogButton = componentsInChildren[i];
			videoLogButton.justGotFocus = false;
			if (i < list.Count)
			{
				((Component)videoLogButton).get_gameObject().SetActive(true);
				videoLogButton.Bind(list[i]);
				Link(((Component)videoLogButton).GetComponent<Selectable>(), ((Component)componentsInChildren[(i + 1) % list.Count]).GetComponent<Selectable>());
			}
			else
			{
				((Component)videoLogButton).get_gameObject().SetActive(false);
			}
		}
		selectedItem = null;
		SelectItem(componentsInChildren[0]);
		subtitleText.text = "";
		Transform val = ((Component)this).get_transform().Find("MenuPanel/Buttons/BottomRow/PlayButton");
		if ((Object)(object)val != (Object)null)
		{
			((Component)val).get_gameObject().SetActive(false);
		}
		Transform val2 = ((Component)this).get_transform().Find("MenuPanel/Buttons/BottomRow/BackButton");
		if ((Object)(object)val2 != (Object)null && list.Count > 0)
		{
			Selectable component = ((Component)val2).GetComponent<Selectable>();
			Link(((Component)componentsInChildren[list.Count - 1]).GetComponent<Selectable>(), component, makeExplicit: true);
			Link(component, ((Component)componentsInChildren[0]).GetComponent<Selectable>(), makeExplicit: true);
		}
	}

	public override void OnLostFocus()
	{
		base.OnLostFocus();
		StopVideo();
	}

	public override void OnTansitionedIn()
	{
		base.OnTansitionedIn();
		MenuSystem.instance.FocusOnMouseOver(enable: false);
	}

	private void OnDisable()
	{
		image.set_texture((Texture)null);
		((Graphic)image).set_material((Material)null);
	}

	public override void ApplyMenuEffects()
	{
		MenuCameraEffects.FadeInPauseMenu();
	}

	private void PrepareCompleted(VideoPlayer source)
	{
		image.set_texture(video.get_texture());
		if ((Object)(object)video != (Object)null)
		{
			video.Play();
			MusicManager.instance.Pause();
			audioSource.Play();
			audiosourceStart = Time.get_unscaledTime();
		}
		videoStarted = true;
	}

	private void LoadMovie()
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected O, but got Unknown
		if ((Object)(object)video == (Object)null)
		{
			video = ((Component)this).get_gameObject().GetComponent<VideoPlayer>();
			if ((Object)(object)video == (Object)null)
			{
				video = ((Component)this).get_gameObject().AddComponent<VideoPlayer>();
			}
		}
		video.set_playOnAwake(false);
		audioSource.set_playOnAwake(false);
		video.set_source((VideoSource)1);
		string url = Application.get_streamingAssetsPath() + "/Videos/" + ((Object)selectedItem.videoRepositoryItem).get_name() + "Video.mp4";
		video.set_url(url);
		video.set_audioOutputMode((VideoAudioOutputMode)1);
		video.set_controlledAudioTrackCount((ushort)1);
		video.EnableAudioTrack((ushort)0, true);
		video.SetTargetAudioSource((ushort)0, audioSource);
		video.add_prepareCompleted(new EventHandler(PrepareCompleted));
		video.Prepare();
	}

	public void PlayClick()
	{
		if (MenuSystem.CanInvoke)
		{
			ScriptLocalization.Get("SUBTITLES/" + ((Object)selectedItem.videoRepositoryItem).get_name());
			TextAsset videoSrt = HFFResources.instance.GetVideoSrt(((Object)selectedItem.videoRepositoryItem).get_name() + "Srt");
			subtitles = new SrtSubtitles();
			subtitles.Load(((Object)selectedItem.videoRepositoryItem).get_name(), videoSrt.get_text());
			videoSrt = null;
			LoadMovie();
		}
	}

	public void SelectItem(VideoLogButton item)
	{
		if (!((Object)(object)item.videoRepositoryItem == (Object)null) && !((Object)(object)selectedItem == (Object)(object)item))
		{
			StopVideo();
			selectedItem = item;
			thumbnailTexture = HFFResources.instance.GetVideoThumb(((Object)item.videoRepositoryItem).get_name() + "Thumb");
			image.set_texture(thumbnailTexture);
			((Graphic)image).set_material((Material)null);
		}
	}

	public void PlayVideo(VideoLogButton item)
	{
		SelectItem(item);
		PlayClick();
	}

	public void StopVideo()
	{
		if ((Object)(object)video != (Object)null)
		{
			video.Stop();
		}
		video = null;
		if ((Object)(object)audioSource != (Object)null)
		{
			audioSource.Stop();
			audioSource.set_clip((AudioClip)null);
		}
		subtitleText.text = "";
		MusicManager.instance.Resume();
		image.set_texture(thumbnailTexture);
		((Graphic)image).set_material((Material)null);
		videoStarted = false;
	}

	public void BackClick()
	{
		if (MenuSystem.CanInvoke)
		{
			TransitionBack<ExtrasMenu>();
		}
	}

	public override void OnBack()
	{
		if ((Object)(object)video != (Object)null)
		{
			VideoLogButton item = selectedItem;
			selectedItem = null;
			SelectItem(item);
		}
		else
		{
			BackClick();
		}
	}

	protected override void Update()
	{
		base.Update();
		if ((Object)(object)video != (Object)null && videoStarted && video.get_isPlaying() && subtitles != null)
		{
			float time = audiosourceTime;
			if (currentLine != null)
			{
				if (!currentLine.ShouldShow(time))
				{
					subtitleText.text = null;
				}
				currentLine = null;
			}
			if (currentLine == null)
			{
				currentLine = subtitles.GetLineToDisplay(time);
				if (currentLine != null && Options.audioSubtitles > 0)
				{
					subtitleText.text = ScriptLocalization.Get("SUBTITLES/" + currentLine.key);
				}
			}
		}
		if ((Object)(object)video != (Object)null && videoStarted && !video.get_isPlaying())
		{
			VideoLogButton item = selectedItem;
			selectedItem = null;
			SelectItem(item);
		}
	}
}

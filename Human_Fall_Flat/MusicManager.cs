using System.Collections;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class MusicManager : Sound2
{
	public ProceduralDroneMix menuDrones;

	public float droneMinTime = 30f;

	public float crossfadeDuration = 3f;

	public float fadeoutDuration = 2f;

	public float fadeinDuration;

	public static MusicManager instance;

	public float[] levels;

	public string[] songList;

	public string[] menuSongList;

	private List<string> recentSongs = new List<string>();

	private List<string> oldSongs = new List<string>();

	private SoundLibrary.SerializedSample lastSample;

	public string currentSong;

	private Coroutine loadCoroutine;

	public float minPause = 10f;

	public float maxPause = 60f;

	private float pause;

	internal bool shuffle = true;

	private void OnEnable()
	{
		instance = this;
		oldSongs.AddRange(songList);
		Shell.RegisterCommand("shuffle", OnShuffle, "shuffle <on|off>\r\nToggle music shuffle\r\n\t<on> - play continuous music shuffling the soundtrack\r\n\t<off> - only play music at scripted locations");
	}

	private void OnShuffle(string param)
	{
		if (!string.IsNullOrEmpty(param))
		{
			param = param.ToLowerInvariant();
			if ("off".Equals(param))
			{
				Options.audioShuffle = 0;
			}
			else if ("on".Equals(param))
			{
				Options.audioShuffle = 1;
			}
		}
		Shell.Print("Music shuffle " + ((Options.audioShuffle > 0) ? "on" : "off"));
	}

	public void Play(SoundLibrary.SerializedSample sample)
	{
		SoundLibrary.SerializedClip clip = sample.GetClip(null, 'A', SoundLibrary.SampleContainerChildType.None);
		if (base.isPlaying)
		{
			if (sample != lastSample)
			{
				lastSample = sample;
				soundSample = sample;
				CrossfadeSound(clip, loop: false, 0f, crossfadeDuration, fadeoutDuration, fadeinDuration);
			}
		}
		else
		{
			lastSample = sample;
			soundSample = sample;
			PlaySound(clip, loop: false);
		}
	}

	public void PlayTriggeredMusic(string resource)
	{
		if (!resource.Equals(currentSong))
		{
			((MonoBehaviour)this).StartCoroutine(Play(resource, duckDrones: false));
		}
	}

	public IEnumerator Play(string resource, bool duckDrones)
	{
		currentSong = resource;
		float decibel = 1f;
		for (int i = 0; i < songList.Length; i++)
		{
			if (songList[i].Equals(resource))
			{
				decibel = levels[i];
			}
		}
		SoundLibrary.SerializedSample soundSample = new SoundLibrary.SerializedSample
		{
			builtIn = true,
			category = "Music",
			name = resource,
			loaded = true,
			vB = AudioUtils.DBToValue(decibel)
		};
		AudioClip audioClip = HFFResources.instance.GetMusicTrack(resource);
		yield return null;
		SoundLibrary.SerializedClip clip = soundSample.AddClip(resource, resource, null, audioClip);
		lastSample = soundSample;
		base.soundSample = soundSample;
		if (base.isPlaying)
		{
			if (resource == "LabDemo")
			{
				fadeoutDuration = 5f;
				crossfadeDuration = 10f;
				fadeinDuration = 5f;
			}
			CrossfadeSound(clip, loop: false, 0f, crossfadeDuration, fadeoutDuration, fadeinDuration);
		}
		else
		{
			PlaySound(clip, loop: false);
		}
		if (duckDrones)
		{
			menuDrones.Duck(this);
		}
		loadCoroutine = null;
	}

	public new void Stop()
	{
		StopSound();
	}

	private void PlayRandom(IList<string> selection, int blockCount, bool duckDrones)
	{
		if (blockCount >= selection.Count)
		{
			blockCount = selection.Count - 1;
		}
		while (recentSongs.Count > blockCount)
		{
			oldSongs.Add(recentSongs[0]);
			recentSongs.RemoveAt(0);
		}
		List<string> list = new List<string>();
		for (int i = 0; i < selection.Count; i++)
		{
			if (oldSongs.Contains(selection[i]))
			{
				list.Add(selection[i]);
			}
		}
		int index = Random.Range(0, list.Count);
		string text = list[index];
		oldSongs.Remove(text);
		recentSongs.Add(text);
		loadCoroutine = ((MonoBehaviour)this).StartCoroutine(Play(text, duckDrones));
	}

	public override void OnClipDeactivated(SoundLibrary.SerializedClip clip)
	{
		if (!((Object)(object)clip.clip == (Object)null))
		{
			base.OnClipDeactivated(clip);
			if (((Object)clip.clip).get_name().Equals(currentSong))
			{
				currentSong = null;
			}
			clip.clip = null;
		}
	}

	protected override void Update()
	{
		bool flag = MenuSystem.instance.state == MenuSystemState.MainMenu || (Object)(object)CreditsText.instance != (Object)null;
		if (flag)
		{
			menuDrones.Play(2f);
		}
		else
		{
			menuDrones.Stop(2f);
		}
		if (loadCoroutine == null && !instance.isPlaying && ((Object)(object)IntroDrones.instance == (Object)null || IntroDrones.instance.dronesTime > droneMinTime))
		{
			if (pause > 0f)
			{
				pause -= Time.get_deltaTime();
			}
			else if (flag)
			{
				PlayRandom(menuSongList, 1, duckDrones: true);
				pause = Random.Range(minPause, maxPause);
			}
			else if (shuffle)
			{
				PlayRandom(songList, 5, duckDrones: false);
				pause = 1f;
			}
		}
		base.Update();
	}
}

using System.Collections;
using HumanAPI;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SignalScriptPlayaRandomSound1 : Node
{
	public bool onlyTriggerOnce;

	[Tooltip("The random array of audio clips to choose from")]
	public AudioClip[] audioClips;

	[Tooltip("Signal this when a sound should be played")]
	public NodeInput beginPlaying;

	[Tooltip("How loud the sound should be played")]
	public NodeInput setVolume;

	private float localVolume;

	[Tooltip("Signal this when a sound has been called for")]
	public NodeOutput soundPlaying;

	private bool hasTriggered;

	private bool isPlaying;

	private int clipArrayEntry;

	private int arrayEnd;

	private int currentClip;

	private AudioClip currentAudioClip;

	private AudioSource m_AudioSource;

	private float currentAudioClipDuration;

	[Tooltip("Whether or not to always play a sound")]
	public bool alwaysPlay;

	public bool onlyPlayWhenInputChanged;

	private float prevInput;

	public bool showDebug;

	private bool beginChecking;

	private void Start()
	{
		arrayEnd = audioClips.Length;
		if (arrayEnd == 0)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + "There is nothing in the array - enabled off "));
			}
			((Behaviour)this).set_enabled(false);
		}
		else if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " There is something in the array " + arrayEnd + " Entries "));
		}
	}

	public override void Process()
	{
		if (onlyTriggerOnce && hasTriggered)
		{
			return;
		}
		if (beginPlaying.value > 0.5f)
		{
			hasTriggered = true;
			if (!beginChecking)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + "  Want to play a sound now "));
				}
				if (!onlyPlayWhenInputChanged || prevInput != beginPlaying.value)
				{
					beginChecking = true;
				}
			}
		}
		prevInput = beginPlaying.value;
	}

	private void Update()
	{
		if (!beginChecking)
		{
			return;
		}
		if (isPlaying)
		{
			if (alwaysPlay)
			{
				PlaySomething();
			}
		}
		else
		{
			PlaySomething();
		}
	}

	private void PlaySomething()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + "Looking for a sound to play "));
			Debug.Log((object)(((Object)this).get_name() + " current Clip = " + currentClip));
			Debug.Log((object)(((Object)this).get_name() + " Array End = " + arrayEnd));
		}
		if (isPlaying)
		{
			return;
		}
		currentClip++;
		if (currentClip == arrayEnd)
		{
			currentClip = 0;
		}
		clipArrayEntry = currentClip;
		currentAudioClip = audioClips[clipArrayEntry];
		if ((Object)(object)currentAudioClip == (Object)null)
		{
			return;
		}
		AudioSource component = ((Component)this).GetComponent<AudioSource>();
		localVolume = setVolume.value;
		if (setVolume.value == 0f)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Restting the volume "));
			}
			localVolume = 1f;
		}
		component.set_clip(currentAudioClip);
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + "Audio clip length : " + component.get_clip().get_length()));
		}
		currentAudioClipDuration = component.get_clip().get_length();
		component.set_volume(localVolume);
		component.Play();
		isPlaying = true;
		soundPlaying.SetValue(1f);
		((MonoBehaviour)this).StartCoroutine(WaitforEnd());
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Finished Waiting "));
		}
	}

	public IEnumerator WaitforEnd()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + "Waiting for the sound to finish "));
		}
		yield return (object)new WaitForSecondsRealtime(currentAudioClipDuration);
		soundPlaying.SetValue(0f);
		isPlaying = false;
		beginChecking = false;
	}
}

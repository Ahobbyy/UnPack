using Multiplayer;
using UnityEngine;

public class NarrativeBlock : MonoBehaviour
{
	public float showTime = 5f;

	public float triggerDelay = 3f;

	public string subtitle;

	public AudioClip voiceover;

	private bool wasPlayed;

	private bool inside;

	private float enterTime;

	private uint evtCollision;

	private NetIdentity identity;

	public void Start()
	{
		identity = ((Component)this).GetComponent<NetIdentity>();
		if ((Object)(object)identity != (Object)null)
		{
			evtCollision = identity.RegisterEvent(OnNarrativePlay);
		}
	}

	private void OnNarrativePlay(NetStream stream)
	{
		Play();
	}

	public void Play()
	{
		if (!wasPlayed && !TutorialScreen.lockedVOAndSubtitles)
		{
			wasPlayed = true;
			SubtitleManager.instance.SetSubtitle(subtitle, showTime);
			SubtitleManager.instance.PlayNarrative(voiceover);
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (!wasPlayed && !(((Component)other).get_tag() != "Player"))
		{
			enterTime = Time.get_time();
			inside = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (!wasPlayed && !(((Component)other).get_tag() != "Player"))
		{
			inside = false;
		}
	}

	private void Update()
	{
		if (!ReplayRecorder.isPlaying && !NetGame.isClient && !wasPlayed && inside && Time.get_time() > enterTime + triggerDelay)
		{
			Play();
			if (Object.op_Implicit((Object)(object)identity))
			{
				identity.BeginEvent(evtCollision);
				identity.EndEvent();
			}
		}
	}

	public void Localize(string text)
	{
		subtitle = text;
	}

	public NarrativeBlock()
		: this()
	{
	}
}

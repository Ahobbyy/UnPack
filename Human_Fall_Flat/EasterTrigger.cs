using Multiplayer;
using UnityEngine;

public class EasterTrigger : MonoBehaviour
{
	public Collider acceptedCollider;

	private uint evtCollision;

	private NetIdentity identity;

	private AudioSource audioSource;

	private float timeToCool;

	public void Start()
	{
		audioSource = ((Component)this).GetComponent<AudioSource>();
		identity = ((Component)this).GetComponent<NetIdentity>();
		if ((Object)(object)identity != (Object)null)
		{
			evtCollision = identity.RegisterEvent(OnPlayEasterEggAudio);
		}
	}

	private void OnPlayEasterEggAudio(NetStream stream)
	{
		if (Object.op_Implicit((Object)(object)audioSource))
		{
			audioSource.Play();
		}
	}

	public void OnTriggerEnter(Collider other)
	{
		if (ReplayRecorder.isPlaying || NetGame.isClient || timeToCool > 0f)
		{
			return;
		}
		bool flag = false;
		if ((Object)(object)acceptedCollider == (Object)null)
		{
			foreach (Human item in Human.all)
			{
				if ((Object)(object)((Component)item).GetComponent<Collider>() == (Object)(object)other)
				{
					flag = true;
				}
			}
		}
		else if ((Object)(object)other == (Object)(object)acceptedCollider)
		{
			flag = true;
		}
		if (flag)
		{
			if (Object.op_Implicit((Object)(object)audioSource))
			{
				audioSource.Play();
			}
			if (Object.op_Implicit((Object)(object)identity))
			{
				identity.BeginEvent(evtCollision);
				identity.EndEvent();
			}
			timeToCool = 60f;
		}
	}

	private void Update()
	{
		if (timeToCool > 0f)
		{
			timeToCool -= Time.get_deltaTime();
		}
	}

	public EasterTrigger()
		: this()
	{
	}
}

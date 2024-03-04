using Multiplayer;
using UnityEngine;

public class EasterTriggerStay : MonoBehaviour
{
	public float time = 2f;

	private float timeGrabbed;

	private Human humanInside;

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
		if (!(timeToCool > 0f))
		{
			HumanHead component = ((Component)other).GetComponent<HumanHead>();
			if ((Object)(object)component != (Object)null)
			{
				humanInside = ((Component)component).GetComponentInParent<Human>();
			}
		}
	}

	public void OnTriggerLeave(Collider other)
	{
		if ((Object)(object)((Component)other).GetComponent<HumanHead>() != (Object)null)
		{
			humanInside = null;
		}
	}

	public void Update()
	{
		if (ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		if (timeToCool > 0f)
		{
			timeToCool -= Time.get_deltaTime();
			return;
		}
		bool flag = false;
		if (Object.op_Implicit((Object)(object)humanInside))
		{
			flag = true;
		}
		if (flag)
		{
			timeGrabbed += Time.get_deltaTime();
			if (timeGrabbed > time)
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
				humanInside = null;
				timeGrabbed = 0f;
			}
		}
		else
		{
			timeGrabbed = 0f;
		}
	}

	public EasterTriggerStay()
		: this()
	{
	}
}

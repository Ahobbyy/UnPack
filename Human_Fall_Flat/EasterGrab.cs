using Multiplayer;
using UnityEngine;

public class EasterGrab : MonoBehaviour
{
	public GameObject toGrab;

	public float time = 10f;

	public float dist;

	private float timeGrabbed;

	private float timeToCool;

	private uint evtCollision;

	private NetIdentity identity;

	private AudioSource audioSource;

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

	private void Update()
	{
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
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
		for (int i = 0; i < Human.all.Count; i++)
		{
			Ragdoll ragdoll = Human.all[i].ragdoll;
			Vector3 val;
			if ((Object)(object)toGrab == (Object)null || (Object)(object)ragdoll.partLeftHand.sensor.grabObject == (Object)(object)toGrab)
			{
				if (dist != 0f)
				{
					val = ragdoll.partLeftHand.transform.get_position() - ((Component)this).get_transform().get_position();
					if (!(((Vector3)(ref val)).get_magnitude() < dist))
					{
						goto IL_00b3;
					}
				}
				flag = true;
			}
			goto IL_00b3;
			IL_00b3:
			if (!((Object)(object)toGrab == (Object)null) && !((Object)(object)ragdoll.partRightHand.sensor.grabObject == (Object)(object)toGrab))
			{
				continue;
			}
			if (dist != 0f)
			{
				val = ragdoll.partRightHand.transform.get_position() - ((Component)this).get_transform().get_position();
				if (!(((Vector3)(ref val)).get_magnitude() < dist))
				{
					continue;
				}
			}
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
				timeToCool = 240f;
				timeGrabbed = 0f;
			}
		}
		else
		{
			timeGrabbed = 0f;
		}
	}

	public EasterGrab()
		: this()
	{
	}
}

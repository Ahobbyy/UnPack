using HumanAPI;
using Multiplayer;
using UnityEngine;

public class HumanAudio : MonoBehaviour
{
	public Sound2 underwater;

	public Sound2 underwaterBubble;

	public Sound2 grab;

	private Transform leftHand;

	private Transform rightHand;

	private uint evtHandGrab;

	private NetIdentity identity;

	public void Start()
	{
		Ragdoll ragdoll = ((Component)this).GetComponentInParent<Human>().ragdoll;
		((Component)ragdoll.partLeftFoot.transform).GetComponent<CollisionAudioSensor>();
		((Component)ragdoll.partRightFoot.transform).GetComponent<CollisionAudioSensor>();
		leftHand = ragdoll.partLeftHand.transform;
		rightHand = ragdoll.partRightHand.transform;
		ragdoll.partLeftHand.sensor.onGrabTap = HandGrabLeft;
		ragdoll.partRightHand.sensor.onGrabTap = HandGrabRight;
		CollisionAudioSensor[] componentsInChildren = ((Component)this).GetComponentsInChildren<CollisionAudioSensor>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].id = -i;
		}
		identity = ((Component)this).GetComponentInParent<NetIdentity>();
		if ((Object)(object)identity != (Object)null)
		{
			evtHandGrab = identity.RegisterEvent(OnHandGrab);
		}
	}

	private void HandGrabLeft(GameObject source, Vector3 pos, PhysicMaterial arg3, Vector3 arg4)
	{
		HandGrab(left: true);
	}

	private void HandGrabRight(GameObject source, Vector3 pos, PhysicMaterial arg3, Vector3 arg4)
	{
		HandGrab(left: false);
	}

	private void HandGrab(bool left)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		grab.PlayOneShot(left ? leftHand.get_position() : rightHand.get_position(), Random.Range(0.5f, 1.5f), Random.Range(0.95f, 1.05f));
		if (NetGame.isServer || ReplayRecorder.isRecording)
		{
			identity.BeginEvent(evtHandGrab).Write(left);
			identity.EndEvent();
		}
	}

	public void OnHandGrab(NetStream stream)
	{
		HandGrab(stream.ReadBool());
	}

	public HumanAudio()
		: this()
	{
	}
}

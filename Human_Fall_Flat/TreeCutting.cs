using System.Collections.Generic;
using HumanAPI;
using Multiplayer;
using UnityEngine;

[RequireComponent(typeof(NetIdentity))]
public class TreeCutting : MonoBehaviour, IReset
{
	private enum HitType
	{
		TooWeak,
		Cut
	}

	public Collider axe;

	public Collider stump;

	public Vector3 pushDirection;

	public Vector3 pushLocation;

	public int cutsRequired = 3;

	public float cutForce = 4f;

	public Sound2 cutSound;

	public GameObject cutFxTemplate;

	public int poolFxItemCount = 5;

	private List<GameObject> poolObjects = new List<GameObject>();

	private bool isCut;

	private bool isOnStump;

	private int cutCount;

	private uint evtHit;

	private NetIdentity identity;

	private NetVector3Encoder posEncoder = new NetVector3Encoder(500f, 18, 4, 8);

	public bool IsCut => isCut;

	private void Start()
	{
		identity = ((Component)this).GetComponent<NetIdentity>();
		evtHit = identity.RegisterEvent(OnHit);
	}

	private void OnEnable()
	{
		if (Object.op_Implicit((Object)(object)cutFxTemplate))
		{
			for (int i = 0; i < poolFxItemCount; i++)
			{
				GameObject val = Object.Instantiate<GameObject>(cutFxTemplate);
				val.get_transform().SetParent(((Component)this).get_transform(), false);
				val.SetActive(false);
				poolObjects.Add(val);
			}
		}
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		if (!isOnStump && (Object)(object)collision.get_collider() == (Object)(object)stump)
		{
			isOnStump = true;
			Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
			if ((Object)(object)component != (Object)null)
			{
				component.set_isKinematic(true);
			}
		}
		if (isCut || !((Object)(object)collision.get_collider() == (Object)(object)axe))
		{
			return;
		}
		Vector3 relativeVelocity = collision.get_relativeVelocity();
		if (((Vector3)(ref relativeVelocity)).get_magnitude() > cutForce)
		{
			cutCount++;
			SendHit(HitType.Cut, collision.GetPoint());
			if (cutCount >= cutsRequired)
			{
				if ((Object)(object)cutSound != (Object)null)
				{
					cutSound.Play();
				}
				isCut = true;
				Rigidbody component2 = ((Component)this).GetComponent<Rigidbody>();
				if ((Object)(object)component2 != (Object)null)
				{
					component2.set_isKinematic(false);
					component2.AddForceAtPosition(pushDirection, pushLocation);
				}
			}
		}
		else
		{
			SendHit(HitType.TooWeak, collision.GetPoint());
		}
	}

	private void OnHit(NetStream stream)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		HitType type = (HitType)stream.ReadUInt32(1);
		Vector3 position = posEncoder.ApplyState(stream);
		DoHit(type, position);
	}

	private void SendHit(HitType type, Vector3 position)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		DoHit(type, position);
		if (NetGame.isServer || ReplayRecorder.isRecording)
		{
			NetStream netStream = identity.BeginEvent(evtHit);
			netStream.Write((uint)type, 1);
			posEncoder.CollectState(netStream, position);
			identity.EndEvent();
		}
	}

	private void DoHit(HitType type, Vector3 position)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		foreach (GameObject poolObject in poolObjects)
		{
			if (!poolObject.get_activeSelf())
			{
				poolObject.get_transform().set_position(position);
				poolObject.SetActive(true);
				break;
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		isOnStump = false;
		isCut = false;
		cutCount = 0;
	}

	public void OnRespawn()
	{
		ResetState(0, 0);
	}

	public TreeCutting()
		: this()
	{
	}
}

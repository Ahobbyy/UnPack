using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

[RequireComponent(typeof(NetIdentity))]
public class MechScriptTreeCutting1 : MonoBehaviour, IReset
{
	private enum HitType
	{
		TooWeak,
		Cut
	}

	[Tooltip("The collider the tree will look for when being chopped down")]
	public Collider axe;

	[Tooltip("The Stump of the tree so this knows to ignore its collision")]
	public Collider stump;

	[Tooltip("The direction the tree is pushed in when cut down")]
	public Vector3 pushDirection;

	[Tooltip("The location the tree is pushed towards when cut down")]
	public Vector3 pushLocation;

	[Tooltip("The amount of cuts needed to make the tree fall")]
	public int cutsRequired = 3;

	[Tooltip("The amount of force needed to make a cut")]
	public float cutForce = 2f;

	[Tooltip("The FX drawn when there is a cut")]
	public GameObject cutFxTemplate;

	[Tooltip("Length of the list of the effects use")]
	public int poolFxItemCount = 5;

	private List<GameObject> poolObjects = new List<GameObject>();

	private bool isCut;

	private bool isOnStump;

	private int cutCount;

	private SignalScriptNode1 graphNode;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private uint evtHit;

	private NetIdentity identity;

	private NetVector3Encoder posEncoder = new NetVector3Encoder(500f, 18, 4, 8);

	public bool IsCut => isCut;

	private void Start()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Started "));
		}
		identity = ((Component)this).GetComponent<NetIdentity>();
		evtHit = identity.RegisterEvent(OnHit);
	}

	private void OnEnable()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Enabled "));
		}
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
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Collision Enter "));
		}
		if (!isOnStump && (Object)(object)collision.get_collider() == (Object)(object)stump)
		{
			isOnStump = true;
			Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
			if ((Object)(object)component != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Tree is not null "));
				}
				component.set_isKinematic(true);
			}
		}
		if (isCut || !((Object)(object)collision.get_collider() == (Object)(object)axe))
		{
			return;
		}
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Hit by the Axe "));
		}
		Vector3 relativeVelocity = collision.get_relativeVelocity();
		if (((Vector3)(ref relativeVelocity)).get_magnitude() > cutForce)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Hit Hard enough "));
			}
			cutCount++;
			SendHit(HitType.Cut, collision.GetPoint());
			if (cutCount < cutsRequired)
			{
				return;
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " No More cuts needed "));
			}
			isCut = true;
			Rigidbody component2 = ((Component)this).GetComponent<Rigidbody>();
			if ((Object)(object)component2 != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Turning off kinematic flag "));
				}
				component2.set_isKinematic(false);
				component2.AddForceAtPosition(pushDirection, pushLocation);
			}
		}
		else
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " More Cuts needed "));
			}
			SendHit(HitType.TooWeak, collision.GetPoint());
		}
	}

	private void OnHit(NetStream stream)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " OnHit "));
		}
		HitType type = (HitType)stream.ReadUInt32(1);
		Vector3 position = posEncoder.ApplyState(stream);
		DoHit(type, position);
	}

	private void SendHit(HitType type, Vector3 position)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " SendHit "));
		}
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
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " DoHit "));
		}
		foreach (GameObject poolObject in poolObjects)
		{
			if (!poolObject.get_activeSelf())
			{
				poolObject.get_transform().set_position(position);
				poolObject.SetActive(true);
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Trying to send signal now "));
				}
				graphNode = ((Component)this).GetComponent<SignalScriptNode1>();
				graphNode.SendSignal(1f);
				break;
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " ResetState "));
		}
		isOnStump = false;
		isCut = false;
		cutCount = 0;
	}

	public void OnRespawn()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " OnRespawn "));
		}
		ResetState(0, 0);
	}

	public MechScriptTreeCutting1()
		: this()
	{
	}
}

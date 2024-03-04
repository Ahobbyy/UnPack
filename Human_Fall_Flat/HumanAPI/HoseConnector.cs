using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class HoseConnector : MonoBehaviour, IPostReset, INetBehavior
	{
		private enum PlugEventType
		{
			PlugPower,
			PlugNoPower,
			UnplugPower,
			UnplugNoPower,
			Short
		}

		public Rigidbody body;

		public Transform alignmentTransform;

		public Transform ropeFixPoint;

		public SpringJoint springIn;

		public SpringJoint springOut;

		public float torque = 0.2f;

		public float spring = 1000f;

		public float damper = 100f;

		public HoseSocket connectedSocket;

		public FixedJoint connectionJoint;

		private float breakDelay = 3f;

		public float breakTreshold = 0.1f;

		public float tensionDistance = 0.1f;

		public float dispacementTolerance = 1f;

		private HoseSocket ignoreSocketConnect;

		private bool canBreak;

		private float breakableIn;

		internal GameObject[] grablist;

		private HoseSocket startSocket;

		private uint evtPlug;

		private NetIdentity identity;

		private void Start()
		{
			alignmentTransform = ((Component)this).get_transform();
			startSocket = connectedSocket;
			if ((Object)(object)startSocket != (Object)null)
			{
				Connect(connectedSocket);
			}
		}

		private void FixedUpdate()
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			if (ReplayRecorder.isPlaying || NetGame.isClient)
			{
				return;
			}
			bool flag = false;
			for (int i = 0; i < grablist.Length; i++)
			{
				if (GrabManager.IsGrabbedAny(grablist[i]))
				{
					flag = true;
				}
			}
			if (flag && (Object)(object)connectedSocket == (Object)null)
			{
				HoseSocket hoseSocket = HoseSocket.Scan(alignmentTransform.get_position());
				if ((Object)(object)hoseSocket != (Object)null && (Object)(object)hoseSocket != (Object)(object)ignoreSocketConnect)
				{
					if (Connect(hoseSocket))
					{
						canBreak = false;
						breakableIn = breakDelay;
						return;
					}
					Collider[] array = Physics.OverlapSphere(((Component)this).get_transform().get_position(), 5f);
					for (int j = 0; j < array.Length; j++)
					{
						Rigidbody componentInParent = ((Component)array[j]).GetComponentInParent<Rigidbody>();
						if ((Object)(object)componentInParent != (Object)null && !componentInParent.get_isKinematic())
						{
							componentInParent.AddExplosionForce(20000f, ((Component)this).get_transform().get_position(), 5f);
							Human componentInParent2 = ((Component)componentInParent).GetComponentInParent<Human>();
							if ((Object)(object)componentInParent2 != (Object)null)
							{
								componentInParent2.MakeUnconscious();
							}
						}
					}
					SendPlug(PlugEventType.Short);
				}
			}
			if (!flag)
			{
				ignoreSocketConnect = null;
				canBreak = true;
			}
			if (breakableIn > 0f)
			{
				breakableIn -= Time.get_fixedDeltaTime();
			}
			else if ((Object)(object)connectedSocket != (Object)null)
			{
				springIn.set_spring(flag ? (spring / 2f) : spring);
				springOut.set_spring(flag ? (spring / 2f) : spring);
				Vector3 val = alignmentTransform.get_position() - alignmentTransform.get_forward() * dispacementTolerance - ((Component)connectedSocket).get_transform().get_position();
				float num = ((Vector3)(ref val)).get_magnitude() - dispacementTolerance;
				if ((Object)(object)springIn != (Object)null && (Object)(object)springOut != (Object)null && num > breakTreshold)
				{
					Disconnect();
				}
			}
		}

		private void Disconnect()
		{
			Object.Destroy((Object)(object)springIn);
			Object.Destroy((Object)(object)springOut);
			ignoreSocketConnect = connectedSocket;
			connectedSocket = null;
		}

		private bool Connect(HoseSocket scan)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			connectedSocket = scan;
			springIn = Attach(scan, alignmentTransform.get_position() + torque * alignmentTransform.get_forward(), ((Component)connectedSocket).get_transform().get_position() + (torque + tensionDistance) * ((Component)connectedSocket).get_transform().get_forward());
			springOut = Attach(scan, alignmentTransform.get_position() - torque * alignmentTransform.get_forward(), ((Component)connectedSocket).get_transform().get_position() - (torque + tensionDistance / 2f) * ((Component)connectedSocket).get_transform().get_forward());
			return true;
		}

		private SpringJoint Attach(HoseSocket socket, Vector3 anchor, Vector3 connectedAnchor)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			SpringJoint val = ((Component)this).get_gameObject().AddComponent<SpringJoint>();
			((Joint)val).set_anchor(((Component)this).get_transform().InverseTransformPoint(anchor));
			((Joint)val).set_autoConfigureConnectedAnchor(false);
			Rigidbody componentInParent = ((Component)socket).GetComponentInParent<Rigidbody>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				((Joint)val).set_connectedBody(componentInParent);
				((Joint)val).set_connectedAnchor(((Component)componentInParent).get_transform().InverseTransformPoint(connectedAnchor));
				((Joint)val).set_enableCollision(true);
			}
			else
			{
				((Joint)val).set_connectedAnchor(connectedAnchor);
			}
			val.set_spring(spring);
			val.set_damper(damper);
			return val;
		}

		public void PostResetState(int checkpoint)
		{
			if ((Object)(object)connectedSocket != (Object)null)
			{
				Disconnect();
			}
			if ((Object)(object)startSocket != (Object)null)
			{
				Connect(startSocket);
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
			this.identity = identity;
			evtPlug = identity.RegisterEvent(OnPlug);
		}

		private void OnPlug(NetStream stream)
		{
			PlugEventType type = (PlugEventType)stream.ReadUInt32(3);
			PlayPlug(type);
		}

		private void SendPlug(PlugEventType type)
		{
			PlayPlug(type);
			if (NetGame.isServer || ReplayRecorder.isRecording)
			{
				identity.BeginEvent(evtPlug).Write((uint)type, 3);
				identity.EndEvent();
			}
		}

		private void PlayPlug(PlugEventType type)
		{
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			switch (type)
			{
			case PlugEventType.PlugPower:
				SparkPool.instance.Emit(10, ((Component)this).get_transform().get_position());
				CircuitSounds.PlugWireCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.PlugNoPower:
				CircuitSounds.PlugWireNoCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.UnplugPower:
				SparkPool.instance.Emit(10, ((Component)this).get_transform().get_position());
				CircuitSounds.UplugWireCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.UnplugNoPower:
				CircuitSounds.UnplugWireNoCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.Short:
				SparkPool.instance.Emit(30, ((Component)this).get_transform().get_position(), 10f);
				CircuitSounds.PlugWireShortCircuit(((Component)this).get_transform().get_position());
				if ((Object)(object)EasterShortCircuit.instance != (Object)null)
				{
					EasterShortCircuit.instance.ShorCircuit();
				}
				break;
			}
		}

		public void CollectState(NetStream stream)
		{
			stream.WriteNetId(((Object)(object)connectedSocket != (Object)null) ? connectedSocket.sceneId : 0u);
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			state0.ReadNetId();
			ApplyState(state1);
		}

		public void ApplyState(NetStream state)
		{
			uint num = state.ReadNetId();
			HoseSocket hoseSocket = ((num != 0) ? HoseSocket.FindById(num) : null);
			if ((Object)(object)connectedSocket != (Object)(object)hoseSocket)
			{
				if ((Object)(object)connectedSocket != (Object)null)
				{
					Disconnect();
				}
				if ((Object)(object)hoseSocket != (Object)null)
				{
					Connect(hoseSocket);
				}
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			uint num = state0?.ReadNetId() ?? 0;
			uint num2 = state1.ReadNetId();
			if (num == num2)
			{
				delta.Write(v: false);
				return;
			}
			delta.Write(v: true);
			delta.WriteNetId(num2);
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			uint id = state0?.ReadNetId() ?? 0;
			if (delta.ReadBool())
			{
				result.WriteNetId(delta.ReadNetId());
			}
			else
			{
				result.WriteNetId(id);
			}
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			return 11;
		}

		public void SetMaster(bool isMaster)
		{
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
		}

		public HoseConnector()
			: this()
		{
		}
	}
}

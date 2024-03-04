using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class PowerPlug : CircuitConnector, IPostReset, INetBehavior
	{
		private enum PlugEventType
		{
			PlugPower,
			PlugNoPower,
			UnplugPower,
			UnplugNoPower,
			Short
		}

		[Tooltip("The rigid body being used as the plug")]
		public Rigidbody body;

		[Tooltip("Alignment location when attached")]
		public Transform alignmentTransform;

		[Tooltip("Alignment location for attached elements")]
		public Transform ropeFixPoint;

		[Tooltip("Joint which provides an inward orientation")]
		public SpringJoint springIn;

		[Tooltip("Joint which provides the outward orientation")]
		public SpringJoint springOut;

		[Tooltip("The torque needed to place and remove the plug")]
		public float torque = 0.2f;

		[Tooltip("Spring used in the connection and disconnection of the plug")]
		public float spring = 1000f;

		[Tooltip("Used when the plug is attached")]
		public float damper = 100f;

		[Tooltip("The socket the plug is connected to")]
		public PowerSocket connectedSocket;

		[Tooltip("The joint the plug is connected to")]
		public FixedJoint connectionJoint;

		private float breakDelay = 3f;

		private float reattachDelay = 2f;

		[Tooltip("The treshold the plug needs to pass in order to disconnect")]
		public float breakTreshold = 0.1f;

		[Tooltip("Allows for a weaker spring")]
		public float tensionDistance = 0.1f;

		[Tooltip("Displacement tolerance checked when connecting")]
		public float dispacementTolerance = 1f;

		private PowerSocket ignoreSocketConnect;

		private bool canBreak;

		private float breakableIn;

		private float reattachableIn;

		internal GameObject[] grablist;

		private PowerSocket startSocket;

		private SignalScriptNode1 sparks;

		[Tooltip("Instance of Circuit Sounds made instantiated on load if none exists")]
		public GameObject instanceofCircuitSounds;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private uint evtPlug;

		private NetIdentity identity;

		private void Start()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Start "));
			}
			alignmentTransform = ((Component)this).get_transform();
			startSocket = connectedSocket;
			if ((Object)(object)startSocket != (Object)null)
			{
				Connect(connectedSocket);
			}
			if (!((Object)(object)CircuitSounds.instance == (Object)null))
			{
				return;
			}
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Trying to make instance "));
			}
			if ((Object)(object)instanceofCircuitSounds != (Object)null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Circuit Sounds gameobject set "));
				}
				Object.Instantiate<GameObject>(instanceofCircuitSounds);
			}
		}

		private void FixedUpdate()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_0220: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			if (ReplayRecorder.isPlaying || NetGame.isClient)
			{
				return;
			}
			bool flag = false;
			if (grablist.Length != 0)
			{
				for (int i = 0; i < grablist.Length; i++)
				{
					if (GrabManager.IsGrabbedAny(grablist[i]))
					{
						flag = true;
					}
				}
			}
			if (flag && (Object)(object)connectedSocket == (Object)null)
			{
				PowerSocket powerSocket = PowerSocket.Scan(alignmentTransform.get_position());
				if ((Object)(object)powerSocket != (Object)null && (Object)(object)powerSocket != (Object)(object)ignoreSocketConnect && (Object)(object)powerSocket.connected == (Object)null)
				{
					if (Connect(powerSocket))
					{
						if (parent.current != 0f)
						{
							SendPlug(PlugEventType.PlugPower);
						}
						else
						{
							SendPlug(PlugEventType.PlugNoPower);
						}
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
				return;
			}
			if (reattachableIn > 0f)
			{
				reattachableIn -= Time.get_fixedDeltaTime();
				if (reattachableIn < 0f)
				{
					ignoreSocketConnect = null;
				}
			}
			if (!((Object)(object)connectedSocket != (Object)null))
			{
				return;
			}
			springIn.set_spring(flag ? (spring / 2f) : spring);
			springOut.set_spring(flag ? (spring / 2f) : spring);
			Vector3 val = alignmentTransform.get_position() - alignmentTransform.get_forward() * dispacementTolerance - ((Component)connectedSocket).get_transform().get_position();
			float num = ((Vector3)(ref val)).get_magnitude() - dispacementTolerance;
			if ((Object)(object)springIn != (Object)null && (Object)(object)springOut != (Object)null && num > breakTreshold)
			{
				if (parent.current != 0f)
				{
					SendPlug(PlugEventType.UnplugPower);
				}
				else
				{
					SendPlug(PlugEventType.UnplugNoPower);
				}
				Disconnect();
				reattachableIn = reattachDelay;
			}
		}

		private void Disconnect()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Disconnect "));
			}
			Circuit.Disconnect(this);
			SignalScriptNode1 component = ((Component)this).GetComponent<SignalScriptNode1>();
			if ((Object)(object)component != (Object)null)
			{
				component.SendSignal(2f);
			}
			Object.Destroy((Object)(object)springIn);
			Object.Destroy((Object)(object)springOut);
			ignoreSocketConnect = connectedSocket;
			connectedSocket = null;
		}

		private bool Connect(PowerSocket scan)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Connect "));
			}
			SignalScriptNode1 component = ((Component)this).GetComponent<SignalScriptNode1>();
			if ((Object)(object)component != (Object)null)
			{
				component.SendSignal(1f);
			}
			if (!Circuit.Connect(this, scan))
			{
				ignoreSocketConnect = scan;
				return false;
			}
			connectedSocket = scan;
			springIn = Attach(scan, alignmentTransform.get_position() + torque * alignmentTransform.get_forward(), ((Component)connectedSocket).get_transform().get_position() + (torque + tensionDistance) * ((Component)connectedSocket).get_transform().get_forward());
			springOut = Attach(scan, alignmentTransform.get_position() - torque * alignmentTransform.get_forward(), ((Component)connectedSocket).get_transform().get_position() - (torque + tensionDistance / 2f) * ((Component)connectedSocket).get_transform().get_forward());
			return true;
		}

		private SpringJoint Attach(PowerSocket socket, Vector3 anchor, Vector3 connectedAnchor)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Attach "));
			}
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
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Post Reset "));
			}
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
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Start Network "));
			}
			this.identity = identity;
			evtPlug = identity.RegisterEvent(OnPlug);
		}

		private void OnPlug(NetStream stream)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " On Plug "));
			}
			PlugEventType type = (PlugEventType)stream.ReadUInt32(3);
			PlayPlug(type);
		}

		private void SendPlug(PlugEventType type)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Send Plug "));
			}
			PlayPlug(type);
			if (NetGame.isServer || ReplayRecorder.isRecording)
			{
				identity.BeginEvent(evtPlug).Write((uint)type, 3);
				identity.EndEvent();
			}
		}

		private void PlayPlug(PlugEventType type)
		{
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			switch (type)
			{
			case PlugEventType.PlugPower:
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Plug Power "));
				}
				SparkPool.instance.Emit(10, ((Component)this).get_transform().get_position());
				CircuitSounds.PlugWireCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.PlugNoPower:
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Plug No Power "));
				}
				CircuitSounds.PlugWireNoCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.UnplugPower:
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Unplug Power "));
				}
				SparkPool.instance.Emit(10, ((Component)this).get_transform().get_position());
				CircuitSounds.UplugWireCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.UnplugNoPower:
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Unplug No Power "));
				}
				CircuitSounds.UnplugWireNoCurrent(((Component)this).get_transform().get_position());
				break;
			case PlugEventType.Short:
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Short "));
				}
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
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Collect State "));
			}
			stream.WriteNetId(((Object)(object)connectedSocket != (Object)null) ? connectedSocket.sceneId : 0u);
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply Lerped State "));
			}
			state0.ReadNetId();
			ApplyState(state1);
		}

		public void ApplyState(NetStream state)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply State "));
			}
			uint num = state.ReadNetId();
			PowerSocket powerSocket = ((num != 0) ? PowerSocket.FindById(num) : null);
			if ((Object)(object)connectedSocket != (Object)(object)powerSocket)
			{
				if ((Object)(object)connectedSocket != (Object)null)
				{
					Disconnect();
				}
				if ((Object)(object)powerSocket != (Object)null)
				{
					Connect(powerSocket);
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
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Add Delta "));
			}
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
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Max Size in Bits "));
			}
			return 11;
		}

		public void SetMaster(bool isMaster)
		{
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
		}
	}
}

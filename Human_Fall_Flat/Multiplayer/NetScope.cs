using System;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

namespace Multiplayer
{
	public class NetScope : MonoBehaviour, IReset
	{
		public class RemoteState
		{
			public int acknowledgedFrame = -1;

			public int acknowledgedEvent = -1;

			public int lastFullStateFrame = -1;

			public int firstFullStateFrame = -1;
		}

		public const bool DefaultAllowSuspend = true;

		public static List<NetScope> all = new List<NetScope>();

		public uint netId;

		private bool netIdDynamic;

		[NonSerialized]
		public bool exitingLevel;

		[NonSerialized]
		public float suppressThrottling;

		[NonSerialized]
		public bool SnapshotCollect;

		private Dictionary<uint, NetIdentity> map = new Dictionary<uint, NetIdentity>();

		private List<NetIdentity> list = new List<NetIdentity>();

		private bool _AllowSuspend = true;

		private bool _IntSuspendCollect;

		public static bool _ExtSuspendCollect = false;

		public static bool DebugSuspendApply = false;

		public static float clientPlayRate = 1f;

		public const bool IsDestroyed_DebugOnly = false;

		private bool isStarted;

		private NetStream eventStream;

		private NetFrames frames = new NetFrames();

		private bool isMasterSet;

		private bool isMaster;

		private Dictionary<uint, RemoteState> remoteStates = new Dictionary<uint, RemoteState>();

		private int numSendsToClients;

		private int lastFrame0idx;

		private int lastBaseFrame;

		private int lastReceivedEventFrame;

		private int lastPlayedEventFrame;

		public const int Scope_DeltaOverheads = 69;

		public int renderedFrame;

		private float fraction;

		private float lastTime;

		private int lagReceiveToRender;

		private int maxLagFrames = 2;

		private int framesWithImprovedLagCount;

		private int couldImproveBy;

		private bool validated;

		public BitRateCounter deltaBps = new BitRateCounter();

		public BitRateCounter eventBps = new BitRateCounter();

		public BitRateCounter ackBps = new BitRateCounter();

		private int worstCaseCached = -1;

		public bool AllowSuspendCollect
		{
			get
			{
				return _AllowSuspend;
			}
			set
			{
				_AllowSuspend = true;
				frames.AllowDiscontinuous = _AllowSuspend;
			}
		}

		public bool SuspendCollect
		{
			get
			{
				if (!_ExtSuspendCollect)
				{
					return _IntSuspendCollect;
				}
				return true;
			}
			set
			{
				_IntSuspendCollect = value;
			}
		}

		public double GetFractionalRenderedFrame()
		{
			return (double)renderedFrame + (double)fraction / (double)Time.get_fixedDeltaTime();
		}

		protected virtual void OnEnable()
		{
			int num = all.Count;
			while (num - 1 > 0 && all[num - 1].netId > netId)
			{
				num--;
			}
			all.Insert(num, this);
		}

		protected virtual void OnDisable()
		{
			all.Remove(this);
		}

		public static NetScope Find(uint id)
		{
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].netId == id)
				{
					return all[i];
				}
			}
			return null;
		}

		public static void ClearAllButPlayers()
		{
		}

		public static void RemoveAllRemoteState(NetHost client, bool setDying)
		{
			for (int i = 0; i < all.Count; i++)
			{
				all[i].RemoveRemoteState(client, setDying);
			}
		}

		public static void ExitingLevel(Level level)
		{
			NetScope[] componentsInChildren = ((Component)level).GetComponentsInChildren<NetScope>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].exitingLevel = true;
			}
		}

		public void StartNetwork(bool repopulate = true)
		{
			isMaster = !NetGame.isClient && !ReplayRecorder.isPlaying;
			isMasterSet = true;
			if (repopulate)
			{
				Populate();
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].StartNetwork();
				list[i].SetMaster(isMaster);
			}
			isStarted = true;
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			if (exitingLevel)
			{
				Debug.LogError((object)"NetScope.ResetState() called after ExitingLevel()");
				return;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].ResetState(checkpoint, subObjectives);
			}
		}

		public virtual void Populate()
		{
			list.Clear();
			map.Clear();
			NetIdentity[] componentsInChildren = ((Component)this).get_gameObject().GetComponentsInChildren<NetIdentity>(true);
			foreach (NetIdentity netIdentity in componentsInChildren)
			{
				if ((Object)(object)((Component)netIdentity).GetComponentInParent<NetScope>() == (Object)(object)this)
				{
					AddIdentity(netIdentity);
				}
			}
		}

		public void AddIdentity(NetIdentity identity)
		{
			identity.scope = this;
			map[identity.sceneId] = identity;
			int num = list.Count;
			while (num - 1 > 0 && list[num - 1].sceneId > identity.sceneId)
			{
				num--;
			}
			list.Insert(num, identity);
		}

		public void RemoveIdentity(NetIdentity identity)
		{
			map.Remove(identity.sceneId);
			list.Remove(identity);
		}

		public NetStream BeginEvent(NetIdentity identity)
		{
			if (eventStream == null)
			{
				eventStream = NetStream.AllocStream();
			}
			eventStream.WriteNetId(identity.sceneId);
			return eventStream;
		}

		public void EndEvent()
		{
		}

		private RemoteState GetRemoteState(uint id)
		{
			RemoteState value = null;
			if (remoteStates.TryGetValue(id, out value))
			{
				return value;
			}
			value = new RemoteState();
			remoteStates[id] = value;
			return value;
		}

		private void RemoveRemoteState(NetHost client, bool setDying)
		{
			lock (remoteStates)
			{
				if (setDying)
				{
					client.isDyingForScopes = true;
				}
				remoteStates.Remove(client.hostId);
			}
		}

		public void Collect()
		{
			bool flag = !NetGame.isClient && !ReplayRecorder.isPlaying;
			if (!isMasterSet || isMaster != flag)
			{
				isMasterSet = true;
				isMaster = flag;
				for (int i = 0; i < list.Count; i++)
				{
					list[i].SetMaster(isMaster);
				}
			}
			if (exitingLevel || (!NetGame.isServer && !ReplayRecorder.isRecording))
			{
				return;
			}
			bool flag2 = !AllowSuspendCollect || !SuspendCollect || SnapshotCollect || ReplayRecorder.isRecording || !NetGame.isServer;
			SnapshotCollect = false;
			NetStream netStream = null;
			NetStream netStream2 = null;
			try
			{
				if (flag2)
				{
					netStream = NetStream.AllocStream();
					for (int j = 0; j < list.Count; j++)
					{
						list[j].CollectState(netStream);
					}
				}
				if (eventStream != null && eventStream.position > 0)
				{
					eventStream.WriteNetId(0u);
					netStream2 = eventStream;
					eventStream = null;
				}
				if (ReplayRecorder.isRecording)
				{
					ReplayRecorder.instance.SubmitFrame(this, netStream, netStream2);
				}
				if (!NetGame.isServer)
				{
					return;
				}
				lock (remoteStates)
				{
					lock (frames.framesLock)
					{
						int num = 0;
						if (frames.frameQueue.Count > 0)
						{
							num = frames.frameQueue[0].frameId;
						}
						int num2 = NetGame.serverFrameId - 60;
						foreach (KeyValuePair<uint, RemoteState> remoteState in remoteStates)
						{
							RemoteState value = remoteState.Value;
							if (value.acknowledgedFrame > 0)
							{
								if (value.acknowledgedFrame < num)
								{
									value.acknowledgedFrame = -1;
								}
								else
								{
									num2 = Mathf.Min(num2, value.acknowledgedFrame);
								}
							}
							if (value.firstFullStateFrame > 0)
							{
								if (value.firstFullStateFrame < num)
								{
									value.firstFullStateFrame = num;
								}
								else
								{
									num2 = Mathf.Min(num2, value.firstFullStateFrame);
								}
							}
						}
						if (frames.frameQueue.Count > 0)
						{
							num2 = Mathf.Min(num2, frames.frameQueue[frames.frameQueue.Count - 1].frameId);
						}
						frames.DropOldEvents(NetGame.serverFrameId - 10);
						if (netStream2 != null)
						{
							frames.PushEvents(NetGame.serverFrameId, netStream2);
						}
						netStream2 = null;
						frames.DropOldStates(num2);
						if (netStream != null)
						{
							frames.PushState(NetGame.serverFrameId, netStream.AddRef());
						}
						frames.LimitHistory();
						if (NetGame.currentLevelInstanceID == 0 && NetGame.isServer && !(this is NetPlayer))
						{
							return;
						}
						bool flag3 = false;
						try
						{
							int frameId = NetGame.serverFrameId;
							if (netStream == null)
							{
								int num3 = frames.frameQueue.Count - 1;
								if (num3 >= 0)
								{
									frameId = frames.frameQueue[num3].frameId;
									netStream = frames.frameQueue[num3].stream.AddRef();
									flag3 = true;
								}
							}
							if (netStream != null)
							{
								NetGame.instance.NotifyClients(this, frameId, NetGame.serverFrameId, netStream);
							}
						}
						finally
						{
							if (flag3 && netStream != null)
							{
								netStream = netStream.Release();
							}
						}
					}
				}
			}
			finally
			{
				if (netStream != null)
				{
					netStream = netStream.Release();
				}
				if (netStream2 != null)
				{
					netStream2 = netStream2.Release();
				}
			}
		}

		public void DebugExamineCollectState(out int highestFrameId, out int lowestAck, out int highestAck, out NetStream frame)
		{
			highestFrameId = -1;
			lowestAck = -1;
			highestAck = -1;
			frame = null;
			lock (remoteStates)
			{
				lock (frames.framesLock)
				{
					int num = frames.frameQueue.Count - 1;
					if (num >= 0)
					{
						highestFrameId = frames.frameQueue[num].frameId;
						frame = frames.frameQueue[num].stream;
						frame.AddRef();
					}
					foreach (KeyValuePair<uint, RemoteState> remoteState in remoteStates)
					{
						RemoteState value = remoteState.Value;
						if (value.acknowledgedFrame <= 0)
						{
							continue;
						}
						if (lowestAck == -1)
						{
							lowestAck = (highestAck = value.acknowledgedFrame);
							continue;
						}
						if (value.acknowledgedFrame < lowestAck)
						{
							lowestAck = value.acknowledgedFrame;
						}
						if (value.acknowledgedFrame > highestAck)
						{
							highestAck = value.acknowledgedFrame;
						}
					}
				}
			}
		}

		public void NotifyClients(int serverFrameId, int timeId, NetStream fullMaster, NetHost conn)
		{
			NetStream netStream = null;
			int num = -1;
			int num2 = -1;
			try
			{
				lock (remoteStates)
				{
					if (conn.isDyingForScopes)
					{
						Debug.LogFormat("Attempt to send delta to client (id {0}) that is disconnecting - caught and rejected", new object[1] { conn.hostId });
						return;
					}
					RemoteState remoteState = GetRemoteState(conn.hostId);
					num = remoteState.acknowledgedFrame;
					if (num > 0)
					{
						lock (frames.framesLock)
						{
							NetStream state = frames.GetState(num);
							if (state != null)
							{
								netStream = NetStream.AllocStream(state);
							}
						}
					}
					if (netStream != null)
					{
						remoteState.lastFullStateFrame = -1;
						remoteState.firstFullStateFrame = -1;
					}
					else
					{
						int num3 = ((!(suppressThrottling <= 0f)) ? 1 : 30);
						if (remoteState.lastFullStateFrame != -1 && timeId - remoteState.lastFullStateFrame < num3)
						{
							return;
						}
						remoteState.lastFullStateFrame = timeId;
						if (remoteState.firstFullStateFrame == -1)
						{
							remoteState.firstFullStateFrame = serverFrameId;
						}
					}
				}
				num2 = conn.GetWriteFrameId(serverFrameId);
				bool flag = true;
				if (AllowSuspendCollect)
				{
					flag = netStream == null || serverFrameId != num;
				}
				if (flag)
				{
					NetStream netStream2 = NetStream.AllocStream(fullMaster);
					try
					{
						netStream2.Seek(0);
						NetStream netStream3 = NetGame.BeginMessage(NetMsgId.Delta);
						try
						{
							netStream3.WriteNetId(netId);
							netStream3.WriteFrameId((netStream != null) ? num : 0, num2);
							netStream3.WriteFrameId(serverFrameId, num2);
							if (num > 0)
							{
								NetGame.instance.clientLatency.ReportLatency(serverFrameId - num - 1);
							}
							for (int i = 0; i < list.Count; i++)
							{
								list[i].CalculateDelta(netStream, netStream2, netStream3);
							}
							NetGame.instance.SendUnreliable(conn, netStream3, num2);
							ReportDeltaBits(netStream3.position);
							lock (remoteStates)
							{
								numSendsToClients++;
							}
						}
						finally
						{
							if (netStream3 != null)
							{
								netStream3 = netStream3.Release();
							}
						}
					}
					finally
					{
						if (netStream2 != null)
						{
							netStream2 = netStream2.Release();
						}
					}
				}
			}
			finally
			{
				if (netStream != null)
				{
					netStream = netStream.Release();
				}
			}
			num = -1;
			lock (remoteStates)
			{
				if (conn.isDyingForScopes)
				{
					Debug.LogFormat("Attempt to send delta to client (id {0}) that is disconnecting - caught and rejected [2]", new object[1] { conn.hostId });
					return;
				}
				num = GetRemoteState(conn.hostId).acknowledgedEvent;
			}
			NetStream netStream4 = NetGame.BeginMessage(NetMsgId.Event);
			try
			{
				netStream4.WriteNetId(netId);
				bool flag2 = false;
				lock (frames.framesLock)
				{
					for (int j = 0; j < frames.eventQueue.Count; j++)
					{
						if (frames.eventQueue[j].frameId > num)
						{
							flag2 = true;
							netStream4.WriteFrameId(frames.eventQueue[j].frameId, num2);
							netStream4.WriteStream(frames.eventQueue[j].stream);
						}
					}
				}
				if (flag2)
				{
					netStream4.WriteFrameId(0, num2);
					NetGame.instance.SendUnreliable(conn, netStream4, num2);
					ReportEvenBits(netStream4.position);
				}
			}
			finally
			{
				if (netStream4 != null)
				{
					netStream4 = netStream4.Release();
				}
			}
		}

		public int DebugGetAndClearNumSends()
		{
			lock (remoteStates)
			{
				int result = numSendsToClients;
				numSendsToClients = 0;
				return result;
			}
		}

		public void AssignNetId(uint id)
		{
			netId = id;
			netIdDynamic = true;
		}

		protected virtual void OnDestroy()
		{
			if (netIdDynamic)
			{
				NetStream.ReturnDynamicScopeId(netId);
			}
		}

		public void OnReceiveDelta(NetStream delta, int containerFrameId)
		{
			if (exitingLevel || !isStarted)
			{
				return;
			}
			int num = delta.ReadFrameId(containerFrameId);
			int num2 = delta.ReadFrameId(containerFrameId);
			if (num > 0)
			{
				NetGame.instance.clientLatency.ReportLatency(num2 - num - 1 + lagReceiveToRender);
			}
			NetGame.instance.clientBuffer.ReportLatency(maxLagFrames);
			bool flag = true;
			NetStream netStream;
			lock (frames.framesLock)
			{
				frames.DropOldStates(Mathf.Min(num, lastFrame0idx));
				lastBaseFrame = num;
				if (frames.TestForState(num2) != -1)
				{
					frames.LimitHistory();
					netStream = null;
					flag = false;
				}
				else
				{
					netStream = frames.GetState(num, rewind: true);
					if (netStream == null && num > 0)
					{
						return;
					}
				}
			}
			if (flag)
			{
				NetStream netStream2 = NetStream.AllocStream();
				for (int i = 0; i < list.Count; i++)
				{
					list[i].AddDelta(netStream, delta, netStream2);
				}
				ReportDeltaBits(delta.position);
				lock (frames.framesLock)
				{
					frames.PushState(num2, netStream2);
					frames.LimitHistory();
				}
			}
			int writeFrameId = NetGame.instance.server.GetWriteFrameId(num2);
			NetStream netStream3 = NetGame.BeginMessage(NetMsgId.Delta);
			try
			{
				netStream3.WriteNetId(netId);
				netStream3.WriteFrameId(num2, writeFrameId);
				NetGame.instance.SendUnreliableToServer(netStream3, writeFrameId);
				ackBps.ReportBits(netStream3.position);
			}
			finally
			{
				if (netStream3 != null)
				{
					netStream3 = netStream3.Release();
				}
			}
		}

		public virtual void PostUpdate()
		{
			if (suppressThrottling > 0f)
			{
				suppressThrottling -= Time.get_deltaTime();
			}
		}

		public virtual void PreFixedUpdate()
		{
		}

		public virtual void PostFixedUpdate()
		{
			deltaBps.FrameComplete();
			eventBps.FrameComplete();
			ackBps.FrameComplete();
		}

		public void OnReceiveEvents(NetStream events, int containerFrameId)
		{
			if (!isStarted)
			{
				return;
			}
			for (int num = events.ReadFrameId(containerFrameId); num > 0; num = events.ReadFrameId(containerFrameId))
			{
				NetStream netStream = events.ReadStream(forceIndependent: true);
				if (num > lastReceivedEventFrame)
				{
					lock (frames.framesLock)
					{
						frames.PushEvents(num, netStream);
					}
					lastReceivedEventFrame = num;
				}
				else
				{
					netStream.Release();
				}
			}
			ReportEvenBits(events.position);
			int writeFrameId = NetGame.instance.server.GetWriteFrameId(containerFrameId);
			NetStream netStream2 = NetGame.BeginMessage(NetMsgId.Event);
			try
			{
				netStream2.WriteNetId(netId);
				netStream2.WriteFrameId(lastReceivedEventFrame, writeFrameId);
				NetGame.instance.SendUnreliableToServer(netStream2, writeFrameId);
				ackBps.ReportBits(netStream2.position);
			}
			finally
			{
				if (netStream2 != null)
				{
					netStream2 = netStream2.Release();
				}
			}
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			int num = 69;
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].CalculateMaxDeltaSizeInBits();
			}
			return num;
		}

		public static int CalculateDeltaPrefixSize(uint netId)
		{
			return (int)(4 + NetStream.PredictNetIdSize(netId) + 50);
		}

		public int CalculateMaxDeltaSizeInBits(uint netId)
		{
			int num = CalculateDeltaPrefixSize(netId);
			for (int i = 0; i < list.Count; i++)
			{
				num += list[i].CalculateMaxDeltaSizeInBits();
			}
			return num;
		}

		public static uint CalcMaxDeltaBitSizeForScope(uint maxSpaceInBytes, uint netId)
		{
			uint num = 4u;
			num += NetStream.PredictNetIdSize(netId);
			num += 50;
			return (maxSpaceInBytes << 3) - num;
		}

		public void OnReceiveAck(NetHost client, NetStream stream, int containerFrameId)
		{
			int num = stream.ReadFrameId(containerFrameId);
			lock (remoteStates)
			{
				if (!client.isDyingForScopes)
				{
					RemoteState remoteState = GetRemoteState(client.hostId);
					remoteState.acknowledgedFrame = Mathf.Max(remoteState.acknowledgedFrame, num);
					if (remoteState.firstFullStateFrame != -1)
					{
						remoteState.firstFullStateFrame = Mathf.Max(remoteState.firstFullStateFrame, num);
					}
				}
			}
		}

		public void OnReceiveEventAck(NetHost client, NetStream stream, int containerFrameId)
		{
			int num = stream.ReadFrameId(containerFrameId);
			lock (remoteStates)
			{
				if (!client.isDyingForScopes)
				{
					RemoteState remoteState = GetRemoteState(client.hostId);
					remoteState.acknowledgedEvent = Mathf.Max(remoteState.acknowledgedEvent, num);
				}
			}
		}

		public virtual void PostSimulate()
		{
			if (!NetGame.isClient)
			{
				return;
			}
			if (clientPlayRate != 1f)
			{
				fraction += Time.get_fixedDeltaTime() * clientPlayRate;
				while (fraction >= Time.get_fixedDeltaTime())
				{
					fraction -= Time.get_fixedDeltaTime();
					renderedFrame++;
				}
			}
			else
			{
				renderedFrame++;
			}
		}

		public void Apply()
		{
			if (exitingLevel || !isStarted)
			{
				return;
			}
			if (ReplayRecorder.isPlaying)
			{
				ReplayRecorder.instance.Play(this);
				return;
			}
			if (!validated)
			{
				validated = true;
				int num = CalculateMaxDeltaSizeInBits(netId);
				int num2 = NetHost.CalcMaxPossibleSizeForContainerContentsTier0() * 8;
				if (num > num2)
				{
					Debug.LogErrorFormat((Object)(object)this, "Net Scope \"{0}\" (netid={1}) might produce a data packet larger than {2} bytes, which could fail to send on some platforms! Estimated worst-case size of this scope is {3} bytes; maximum safe limit is {4} bytes", new object[5]
					{
						((Object)this).get_name(),
						netId,
						NetStream.CalculateSizeForTier(0),
						(num + 7) / 8,
						num2 >> 3
					});
				}
			}
			if (!NetGame.isClient || DebugSuspendApply)
			{
				return;
			}
			lock (frames.framesLock)
			{
				if (frames.frameQueue.Count == 0)
				{
					return;
				}
				int frameId = frames.frameQueue[frames.frameQueue.Count - 1].frameId;
				if (frameId < renderedFrame)
				{
					maxLagFrames = Mathf.Min(30, maxLagFrames + 1);
					framesWithImprovedLagCount = 0;
					couldImproveBy = 30;
					renderedFrame = frameId;
				}
				if (clientPlayRate == 1f && frameId - maxLagFrames > renderedFrame)
				{
					renderedFrame = frameId - maxLagFrames;
				}
				if (renderedFrame + 1 <= frameId)
				{
					framesWithImprovedLagCount++;
					couldImproveBy = Mathf.Min(couldImproveBy, frameId - (renderedFrame + 1));
					if (framesWithImprovedLagCount > 600 && maxLagFrames > 2)
					{
						maxLagFrames--;
						couldImproveBy--;
						if (couldImproveBy == 0)
						{
							framesWithImprovedLagCount = 0;
							couldImproveBy = 30;
						}
					}
				}
				else
				{
					framesWithImprovedLagCount = 0;
					couldImproveBy = 30;
				}
				lagReceiveToRender = frameId - renderedFrame;
			}
			int num3 = RenderState(frames, renderedFrame, fraction / Time.get_fixedDeltaTime());
			int num4 = PlaybackEvents(frames, 0, renderedFrame);
			lock (frames.framesLock)
			{
				frames.DropOldEvents(num4 + 1);
				lastFrame0idx = num3;
				int frameId2 = Mathf.Min(lastBaseFrame, lastFrame0idx - 30);
				frames.DropOldStates(frameId2);
			}
		}

		public int RenderState(NetFrames frames, int renderedFrame, float fraction)
		{
			bool state;
			int frame0id;
			float mix;
			NetStream frame;
			NetStream frame2;
			lock (frames.framesLock)
			{
				state = frames.GetState(renderedFrame, fraction, out frame0id, out frame, out frame2, out mix);
			}
			try
			{
				if (state)
				{
					if (mix <= 0f)
					{
						frame.Seek(0);
						for (int i = 0; i < list.Count; i++)
						{
							list[i].ApplyState(frame);
						}
						return frame0id;
					}
					if (mix >= 1f)
					{
						frame2.Seek(0);
						for (int j = 0; j < list.Count; j++)
						{
							list[j].ApplyState(frame2);
						}
						return frame0id;
					}
					frame.Seek(0);
					frame2.Seek(0);
					for (int k = 0; k < list.Count; k++)
					{
						list[k].ApplyLerpedState(frame, frame2, mix);
					}
					return frame0id;
				}
				return frame0id;
			}
			finally
			{
				if (frame != null)
				{
					frame = frame.Release();
				}
				if (frame2 != null)
				{
					frame2 = frame2.Release();
				}
			}
		}

		public int PlaybackEvents(NetFrames frames, int fromFrame, int toFrame)
		{
			int num = -1;
			int count;
			lock (frames.framesLock)
			{
				count = frames.eventQueue.Count;
			}
			for (int i = 0; i < count; i++)
			{
				FrameState frameState;
				lock (frames.framesLock)
				{
					frameState = frames.eventQueue[i];
				}
				int frameId = frameState.frameId;
				if (frameId > fromFrame && frameId <= toFrame)
				{
					num = Mathf.Max(num, frameId);
					NetStream stream = frameState.stream;
					stream.Seek(0);
					uint num2 = stream.ReadNetId();
					NetIdentity value;
					while (num2 != 0 && (!map.TryGetValue(num2, out value) || value.DeliverEvent(stream)))
					{
						num2 = stream.ReadNetId();
					}
				}
			}
			return num;
		}

		private void ReportDeltaBits(int bits)
		{
			deltaBps.ReportBits(bits);
		}

		private void ReportEvenBits(int bits)
		{
			eventBps.ReportBits(bits);
		}

		private void ReportAckBits(int bits)
		{
			ackBps.ReportBits(bits);
		}

		public void RenderGUI(GUIStyle labelStyle)
		{
			if (worstCaseCached == -1)
			{
				worstCaseCached = CalculateMaxDeltaSizeInBits(netId);
			}
			GUILayout.Label(string.Format("{5}: {0} {1:00.0}kbps aklf={2:000} evlf={3:000} delf={4:000}   worst-case-size={6} bytes/{7} bits", ((Object)this).get_name(), deltaBps.kbps + eventBps.kbps + ackBps.kbps, ackBps.bitsLastFrame, eventBps.bitsLastFrame, deltaBps.bitsLastFrame, netId, worstCaseCached + 7 >> 3, worstCaseCached), labelStyle, (GUILayoutOption[])(object)new GUILayoutOption[0]);
		}

		public NetScope()
			: this()
		{
		}
	}
}

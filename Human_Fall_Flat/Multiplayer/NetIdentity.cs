using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

namespace Multiplayer
{
	public class NetIdentity : MonoBehaviour
	{
		public uint sceneId;

		public const bool IsDestroyed_DebugOnly = false;

		internal NetScope scope;

		private INetBehavior[] behaviors;

		private List<Action<NetStream>> subscriptions = new List<Action<NetStream>>();

		public void StartNetwork()
		{
			behaviors = ((Component)this).GetComponents<INetBehavior>();
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].StartNetwork(this);
			}
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			for (int num = behaviors.Length - 1; num >= 0; num--)
			{
				behaviors[num].ResetState(checkpoint, subObjectives);
			}
			for (int i = 0; i < behaviors.Length; i++)
			{
			}
		}

		internal void SetMaster(bool isMaster)
		{
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].SetMaster(isMaster);
			}
		}

		public void CollectState(NetStream stream)
		{
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].CollectState(stream);
			}
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].ApplyLerpedState(state0, state1, mix);
			}
		}

		public void ApplyState(NetStream state)
		{
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].ApplyState(state);
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (!NetGame.instance.multithreading)
			{
				Profiler.BeginSample("NetIdentity.CalculateDelta", (Object)(object)this);
			}
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].CalculateDelta(state0, state1, delta);
			}
			if (!NetGame.instance.multithreading)
			{
				Profiler.EndSample();
			}
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (!NetGame.instance.multithreading)
			{
				Profiler.BeginSample("NetIdentity.AddDelta", (Object)(object)this);
			}
			for (int i = 0; i < behaviors.Length; i++)
			{
				behaviors[i].AddDelta(state0, delta, result);
			}
			if (!NetGame.instance.multithreading)
			{
				Profiler.EndSample();
			}
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			int num = 0;
			for (int i = 0; i < behaviors.Length; i++)
			{
				num += behaviors[i].CalculateMaxDeltaSizeInBits();
			}
			return num;
		}

		public uint RegisterEvent(Action<NetStream> handler)
		{
			subscriptions.Add(handler);
			return (uint)subscriptions.Count;
		}

		public NetStream BeginEvent(uint eventId)
		{
			if ((Object)(object)scope == (Object)null)
			{
				scope = ((Component)this).GetComponentInParent<NetScope>();
			}
			NetStream netStream = scope.BeginEvent(this);
			netStream.WriteNetId(eventId);
			return netStream;
		}

		public void EndEvent()
		{
			scope.EndEvent();
		}

		public bool DeliverEvent(NetStream stream)
		{
			if ((Object)(object)scope == (Object)null)
			{
				scope = ((Component)this).GetComponentInParent<NetScope>();
			}
			uint num = stream.ReadNetId();
			if (num - 1 < subscriptions.Count)
			{
				subscriptions[(int)(num - 1)](stream);
				return true;
			}
			return false;
		}

		public NetIdentity()
			: this()
		{
		}
	}
}

using System.Collections;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class PowerScriptGenerator1 : Node, INetBehavior
	{
		[Tooltip("Value Output by this generator")]
		public NodeOutput output;

		[Tooltip("Output Hit when there has been a failure")]
		public NodeOutput failed;

		[Tooltip("Smoke particles assocated with this generator")]
		public LerpParticles smoke;

		[Tooltip("The power outlet property of this game object")]
		public PowerOutlet socket;

		[Tooltip("The handle the player needs to pull on to start the generator")]
		public Transform starter;

		[Tooltip("Write_something_here_to_explain_this_thing")]
		public float runTime = 10f;

		[Tooltip("Write_something_here_to_explain_this_thing")]
		public int ignitionsToStart = 3;

		private int remainingIgnitions;

		private float startTreshold = 0.5f;

		private float stopIn;

		private Vector3 initialLocalPos;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private bool isStarting;

		private uint evtFail;

		private NetIdentity identity;

		protected override void OnEnable()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enable "));
			}
			base.OnEnable();
			initialLocalPos = starter.get_localPosition();
			remainingIgnitions = ignitionsToStart;
		}

		private void FixedUpdate()
		{
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			if (ReplayRecorder.isPlaying || NetGame.isClient)
			{
				return;
			}
			if (remainingIgnitions == 0)
			{
				stopIn -= Time.get_fixedDeltaTime();
				if (stopIn <= 0f)
				{
					remainingIgnitions = ignitionsToStart;
					socket.voltage = 0f;
					Circuit.Refresh(socket);
					output.SetValue(0f);
				}
				return;
			}
			Vector3 val = starter.get_localPosition() - initialLocalPos;
			float magnitude = ((Vector3)(ref val)).get_magnitude();
			if (!isStarting && magnitude > startTreshold)
			{
				isStarting = true;
				remainingIgnitions--;
				if (remainingIgnitions == 0)
				{
					socket.voltage = 2f;
					if (Circuit.Refresh(socket))
					{
						stopIn = runTime;
						output.SetValue(1f);
						return;
					}
					remainingIgnitions = ignitionsToStart;
					socket.voltage = 0f;
					Circuit.Refresh(socket);
					output.SetValue(0f);
				}
				else
				{
					FailStart();
				}
			}
			else if (isStarting && magnitude < startTreshold * 0.5f)
			{
				isStarting = false;
			}
		}

		private void FailStart()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Failed Start "));
			}
			if (!ReplayRecorder.isPlaying && !NetGame.isClient)
			{
				identity.BeginEvent(evtFail);
				identity.EndEvent();
			}
			((MonoBehaviour)this).StartCoroutine(EmitBurst());
		}

		private IEnumerator EmitBurst()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Emit Burst "));
			}
			failed.SetValue(1f);
			yield return (object)new WaitForSeconds(0.5f);
			for (int i = 0; i < 10; i++)
			{
				yield return null;
				smoke.Emit(1f);
				smoke.Emit(1f);
				failed.SetValue(0f);
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Start Network "));
			}
			this.identity = identity;
			evtFail = identity.RegisterEvent(OnFail);
		}

		private void OnFail(NetStream stream)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Fail "));
			}
			FailStart();
		}

		public void CollectState(NetStream stream)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Collect State "));
			}
			NetBoolEncoder.CollectState(stream, output.value >= 0.5f);
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply Lerp "));
			}
			ApplyState(NetBoolEncoder.ApplyLerpedState(state0, state1, mix));
		}

		public void ApplyState(NetStream state)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Net State "));
			}
			ApplyState(NetBoolEncoder.ApplyState(state));
		}

		private void ApplyState(bool state)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Apply State "));
			}
			if (state)
			{
				if (output.value < 0.5f)
				{
					output.SetValue(1f);
				}
			}
			else if (output.value > 0.5f)
			{
				output.SetValue(0f);
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc Data "));
			}
			NetBoolEncoder.CalculateDelta(state0, state1, delta);
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Add Delta "));
			}
			NetBoolEncoder.AddDelta(state0, delta, result);
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Calc Max Delta "));
			}
			return NetBoolEncoder.CalculateMaxDeltaSizeInBits();
		}

		public void SetMaster(bool isMaster)
		{
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			remainingIgnitions = ignitionsToStart;
		}
	}
}

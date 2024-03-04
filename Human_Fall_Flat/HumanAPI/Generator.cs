using System.Collections;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class Generator : Node, INetBehavior
	{
		public NodeOutput output;

		public LerpParticles smoke;

		public PowerOutlet socket;

		public AudioClip[] failedStartClips;

		public Sound2 sound;

		public Sound2 failSound;

		public Transform starter;

		public float runTime = 10f;

		public int ignitionsToStart = 3;

		private int remainingIgnitions;

		private float startTreshold = 0.5f;

		private float stopIn;

		private Vector3 initialLocalPos;

		private bool isStarting;

		private uint evtFail;

		private NetIdentity identity;

		protected override void OnEnable()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			base.OnEnable();
			initialLocalPos = starter.get_localPosition();
			remainingIgnitions = ignitionsToStart;
		}

		private void FixedUpdate()
		{
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
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
					sound.Stop();
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
						sound.Play();
						stopIn = runTime;
						output.SetValue(1f);
					}
					else
					{
						remainingIgnitions = ignitionsToStart;
						socket.voltage = 0f;
						Circuit.Refresh(socket);
						sound.Stop();
						output.SetValue(0f);
					}
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
			if (!ReplayRecorder.isPlaying && !NetGame.isClient)
			{
				identity.BeginEvent(evtFail);
				identity.EndEvent();
			}
			failSound.PlayOneShot();
			((MonoBehaviour)this).StartCoroutine(EmitBurst());
		}

		private IEnumerator EmitBurst()
		{
			yield return (object)new WaitForSeconds(0.5f);
			for (int i = 0; i < 10; i++)
			{
				yield return null;
				smoke.Emit(1f);
				smoke.Emit(1f);
			}
		}

		public void StartNetwork(NetIdentity identity)
		{
			this.identity = identity;
			evtFail = identity.RegisterEvent(OnFail);
		}

		private void OnFail(NetStream stream)
		{
			FailStart();
		}

		public void CollectState(NetStream stream)
		{
			NetBoolEncoder.CollectState(stream, output.value >= 0.5f);
		}

		public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
		{
			ApplyState(NetBoolEncoder.ApplyLerpedState(state0, state1, mix));
		}

		public void ApplyState(NetStream state)
		{
			ApplyState(NetBoolEncoder.ApplyState(state));
		}

		private void ApplyState(bool state)
		{
			if (state)
			{
				if (output.value < 0.5f)
				{
					output.SetValue(1f);
					sound.Play();
				}
			}
			else if (output.value > 0.5f)
			{
				output.SetValue(0f);
				sound.Stop();
			}
		}

		public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
		{
			NetBoolEncoder.CalculateDelta(state0, state1, delta);
		}

		public void AddDelta(NetStream state0, NetStream delta, NetStream result)
		{
			NetBoolEncoder.AddDelta(state0, delta, result);
		}

		public int CalculateMaxDeltaSizeInBits()
		{
			return NetBoolEncoder.CalculateMaxDeltaSizeInBits();
		}

		public void SetMaster(bool isMaster)
		{
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			remainingIgnitions = ignitionsToStart;
		}
	}
}

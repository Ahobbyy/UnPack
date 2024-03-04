using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class SignalScriptTriggerSound1 : MonoBehaviour, IReset
	{
		[Tooltip("Whether or not the trigger volume reacts to the player or not")]
		public bool acceptPlayer;

		[Tooltip("An array of colliders the trigger volume reacts to")]
		public Collider[] acceptColliders;

		[Tooltip("Min velocity the thing being looked for needs to be moving at to trigger this sound")]
		public float minVelocity;

		[Tooltip("Max velocity the thing being looked for needs to be moving at to trigger this sound")]
		public float maxVelocity;

		[Tooltip("Min angular velocity the thing being looked for needs to be doing to trigger this sound")]
		public float minAngular;

		[Tooltip("Max angular velocity the thing being looked for needs to be doing to trigger this sound")]
		public float maxAngular;

		[Tooltip("Min Volume the sound can be played at")]
		public float minVolume;

		[Tooltip("Min Delay before the sound plays")]
		public float minDelay = 0.2f;

		[Tooltip("Sound to play")]
		public AudioSource sound;

		public NodeOutput triggered;

		private float lastSoundTime;

		private uint evtImpact;

		private NetIdentity identity;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private void Start()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Started "));
			}
			identity = ((Component)this).GetComponentInParent<NetIdentity>();
			if ((Object)(object)identity != (Object)null)
			{
				evtImpact = identity.RegisterEvent(OnImpact);
			}
		}

		private void BroadcastImpact(float impact)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Broadcast impact "));
			}
			float num = AudioUtils.ValueToDB(impact) + 32f;
			if (!(num < -64f))
			{
				identity.BeginEvent(evtImpact).Write(NetFloat.Quantize(num, 64f, 6), 6);
				identity.EndEvent();
			}
		}

		private void OnImpact(NetStream stream)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Impact "));
			}
			float impact = AudioUtils.DBToValue(NetFloat.Dequantize(stream.ReadInt32(6), 64f, 6) - 32f);
			PlayImpact(impact);
		}

		private void Awake()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Awake "));
			}
			if ((Object)(object)sound == (Object)null)
			{
				sound = ((Component)this).GetComponentInChildren<AudioSource>();
			}
		}

		public void OnTriggerEnter(Collider other)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_019e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Trigger Enter "));
			}
			float num = 1f;
			Vector3 val;
			if (acceptPlayer && ((Component)other).get_tag() == "Player")
			{
				if (maxVelocity > 0f)
				{
					val = ((Component)other).GetComponentInParent<Human>().velocity;
					float magnitude = ((Vector3)(ref val)).get_magnitude();
					if (magnitude < minVelocity)
					{
						return;
					}
					num = Mathf.InverseLerp(minVelocity, maxVelocity, magnitude);
				}
			}
			else
			{
				if (acceptColliders.Length == 0 || !(((Component)other).get_tag() != "Player"))
				{
					return;
				}
				bool flag = false;
				for (int i = 0; i < acceptColliders.Length; i++)
				{
					if ((Object)(object)acceptColliders[i] == (Object)(object)other)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					return;
				}
				if (minVelocity > 0f || maxVelocity > 0f || minAngular > 0f || maxAngular > 0f)
				{
					Rigidbody componentInParent = ((Component)other).GetComponentInParent<Rigidbody>();
					if ((Object)(object)componentInParent == (Object)null)
					{
						return;
					}
					num = 0f;
					if (minVelocity > 0f || maxVelocity > 0f)
					{
						val = componentInParent.get_velocity();
						float magnitude2 = ((Vector3)(ref val)).get_magnitude();
						if (maxVelocity != 0f)
						{
							num = Mathf.InverseLerp(minVelocity, maxVelocity, magnitude2);
						}
						else if (magnitude2 > minVelocity)
						{
							num = 1f;
						}
					}
					if (minAngular > 0f || maxAngular > 0f)
					{
						val = componentInParent.get_angularVelocity();
						float magnitude3 = ((Vector3)(ref val)).get_magnitude();
						if (maxAngular != 0f)
						{
							num = Mathf.Max(num, Mathf.InverseLerp(minAngular, maxAngular, magnitude3));
						}
						else if (magnitude3 > minAngular)
						{
							num = 1f;
						}
					}
					if (num == 0f)
					{
						return;
					}
				}
			}
			if (!(num < minVolume))
			{
				if (NetGame.isServer || ReplayRecorder.isRecording)
				{
					BroadcastImpact(num);
				}
				PlayImpact(num);
			}
		}

		private void PlayImpact(float impact)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Player Impact "));
			}
			float time = Time.get_time();
			if (!(lastSoundTime + minDelay > time))
			{
				sound.set_volume(impact);
				sound.Play();
				lastSoundTime = time;
			}
		}

		public void ResetState(int checkpoint, int subObjectives)
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Reset State "));
			}
			lastSoundTime = 0f;
		}

		public SignalScriptTriggerSound1()
			: this()
		{
		}
	}
}

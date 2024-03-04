using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Collision Audio Sensor", 10)]
	public class CollisionAudioSensor : MonoBehaviour
	{
		private static int idCounter;

		[Tooltip("Override for the counter")]
		public int id;

		private Rigidbody body;

		[Tooltip("Override value for the pitch")]
		public float pitch = 1f;

		[Tooltip("Override for the volume")]
		public float volume = 1f;

		[Tooltip("Use ths in order to print debug info to the Log")]
		public bool showDebug;

		private float nextSoundTime;

		private uint evtCollisionAudio;

		private NetIdentity identity;

		private void Awake()
		{
			id = ++idCounter;
		}

		protected virtual void OnEnable()
		{
		}

		private void OnCollisionEnter(Collision collision)
		{
			ReportCollision(collision);
		}

		private void OnCollisionStay(Collision collision)
		{
			ReportCollision(collision);
		}

		private void ReportCollision(Collision collision)
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			if (NetGame.isClient || ReplayRecorder.isPlaying || collision.get_contacts().Length == 0)
			{
				return;
			}
			float time = Time.get_time();
			if (nextSoundTime > time)
			{
				return;
			}
			Vector3 val = collision.get_relativeVelocity();
			if (((Vector3)(ref val)).get_magnitude() < CollisionAudioEngine.instance.minVelocity)
			{
				return;
			}
			val = collision.get_impulse();
			if (((Vector3)(ref val)).get_magnitude() < CollisionAudioEngine.instance.minImpulse)
			{
				return;
			}
			collision.Analyze(out var pos, out var impulse, out var normalVelocity, out var tangentVelocity, out var mat, out var mat2, out var id, out var volume, out var pitch);
			if (this.id < id)
			{
				SurfaceType surf = SurfaceTypes.Resolve(mat);
				SurfaceType surf2 = SurfaceTypes.Resolve(mat2);
				if (ReportCollision(surf, surf2, pos, impulse, normalVelocity, tangentVelocity, this.volume * volume, this.pitch * pitch))
				{
					nextSoundTime = time + Random.Range(0.5f, 1.5f) * CollisionAudioEngine.instance.hitDelay;
				}
			}
		}

		protected virtual bool ReportCollision(SurfaceType surf1, SurfaceType surf2, Vector3 pos, float impulse, float normalVelocity, float tangentVelocity, float volume, float pitch)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			return CollisionAudioEngine.instance.ReportCollision(this, surf1, surf2, pos, impulse, normalVelocity, tangentVelocity, volume, pitch);
		}

		private void Start()
		{
			identity = ((Component)this).GetComponentInParent<NetIdentity>();
			if ((Object)(object)identity != (Object)null)
			{
				evtCollisionAudio = identity.RegisterEvent(OnReceiveCollisionAudio);
			}
		}

		public void BroadcastCollisionAudio(CollisionAudioHitConfig config, AudioChannel channel, Vector3 pos, float rms, float pitch)
		{
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Broadcast Collision Audio "));
			}
			if (AudioUtils.ValueToDB(rms) + 32f < -64f)
			{
				return;
			}
			if ((Object)(object)identity == (Object)null)
			{
				Debug.LogErrorFormat((Object)(object)this, "No NetIdentity for {0}", new object[1] { ((Object)this).get_name() });
				return;
			}
			NetStream netStream = identity.BeginEvent(evtCollisionAudio);
			netStream.Write(config.netId, 8);
			if (channel == AudioChannel.Footsteps)
			{
				netStream.Write(v: true);
			}
			else
			{
				netStream.Write(v: false);
				if (channel == AudioChannel.Body)
				{
					netStream.Write(v: true);
				}
				else
				{
					netStream.Write(v: false);
				}
			}
			NetVector3.Quantize(pos - ((Component)this).get_transform().get_position(), 100f, 10).Write(netStream, 3);
			netStream.Write(NetFloat.Quantize(AudioUtils.ValueToDB(rms) + 32f, 64f, 6), 6);
			netStream.Write(NetFloat.Quantize(AudioUtils.RatioToCents(pitch), 4800f, 8), 3, 8);
			identity.EndEvent();
		}

		public void OnReceiveCollisionAudio(NetStream stream)
		{
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Recieve Collision Audio "));
			}
			ushort libId = (ushort)stream.ReadUInt32(8);
			AudioChannel channel = ((!stream.ReadBool()) ? (stream.ReadBool() ? AudioChannel.Body : AudioChannel.Physics) : AudioChannel.Footsteps);
			Vector3 val = NetVector3.Read(stream, 3, 10).Dequantize(100f);
			Vector3 pos = ((Component)this).get_transform().get_position() + val;
			float emit = AudioUtils.DBToValue(NetFloat.Dequantize(stream.ReadInt32(6), 64f, 6) - 32f);
			float num = AudioUtils.CentsToRatio(NetFloat.Dequantize(stream.ReadInt32(3, 8), 4800f, 8));
			CollisionAudioHitConfig config = CollisionAudioEngine.instance.GetConfig(libId);
			if (config != null)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " There is no audio engine "));
				}
				config.PlayWithKnownEmit(channel, null, pos, emit, num);
			}
		}

		public CollisionAudioSensor()
			: this()
		{
		}
	}
}

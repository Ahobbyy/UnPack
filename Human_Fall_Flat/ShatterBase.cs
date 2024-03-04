using Multiplayer;
using UnityEngine;

public abstract class ShatterBase : MonoBehaviour, INetBehavior
{
	private struct NetState
	{
		public NetVector3 shatterPos;

		public uint shatterMagnitude;

		public uint shatterSeed;

		public uint netId;

		public void Write(NetStream stream)
		{
			stream.WriteNetId(netId);
			shatterPos.Write(stream);
			stream.Write(shatterMagnitude, 10);
			stream.Write(shatterSeed, 10);
		}

		public static NetState Read(NetStream stream)
		{
			NetState result = default(NetState);
			result.netId = stream.ReadNetId();
			result.shatterPos = NetVector3.Read(stream, 10);
			result.shatterMagnitude = stream.ReadUInt32(10);
			result.shatterSeed = stream.ReadUInt32(10);
			return result;
		}
	}

	protected MeshRenderer renderer;

	protected ShatterAudio audio;

	protected ShatterAudioAudioSource audioSource;

	protected Collider collider;

	public float impulseTreshold = 10f;

	public float breakTreshold = 100f;

	public float accumulatedBreakTreshold = 300f;

	public float humanHardness;

	public bool shattered;

	private Vector3 adjustedImpulse = Vector3.get_zero();

	private Vector3 lastFrameImpact;

	private Vector3 maxImpactPoint;

	private float maxImpact;

	public float accumulatedImpact;

	public float maxMomentum;

	private NetIdentity identity;

	private uint evtCrack;

	private bool isMaster;

	private NetState netState;

	protected virtual void OnEnable()
	{
		if ((Object)(object)((Component)this).GetComponent<NetIdentity>() == (Object)null)
		{
			Debug.LogError((object)"Missing NetIdentity", (Object)(object)this);
		}
		audio = ((Component)this).GetComponent<ShatterAudio>();
		collider = ((Component)this).GetComponent<Collider>();
		audioSource = ((Component)this).GetComponent<ShatterAudioAudioSource>();
		renderer = ((Component)this).GetComponentInChildren<MeshRenderer>();
		if ((Object)(object)renderer == (Object)null)
		{
			Debug.LogError((object)("ShatterBase renderer not found: " + ((Object)((Component)this).get_gameObject()).get_name()));
			((Behaviour)this).set_enabled(false);
		}
		if ((Object)(object)collider == (Object)null)
		{
			Debug.LogError((object)("ShatterBase collider not found: " + ((Object)((Component)this).get_gameObject()).get_name()));
			((Behaviour)this).set_enabled(false);
		}
	}

	public void OnCollisionEnter(Collision collision)
	{
		if (!shattered)
		{
			HandleCollision(collision, enter: false);
		}
	}

	public void OnCollisionStay(Collision collision)
	{
		if (!shattered)
		{
			HandleCollision(collision, enter: false);
		}
	}

	private void FixedUpdate()
	{
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		if (shattered || ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		maxImpact = 0f;
		if (((Vector3)(ref adjustedImpulse)).get_magnitude() > maxMomentum)
		{
			maxMomentum = ((Vector3)(ref adjustedImpulse)).get_magnitude();
		}
		accumulatedImpact += ((Vector3)(ref adjustedImpulse)).get_magnitude();
		if (accumulatedImpact > accumulatedBreakTreshold)
		{
			shattered = true;
		}
		Vector3 val = adjustedImpulse + lastFrameImpact;
		if (((Vector3)(ref val)).get_magnitude() > breakTreshold)
		{
			shattered = true;
		}
		if (shattered)
		{
			if (maxImpactPoint == Vector3.get_zero())
			{
				Bounds bounds = collider.get_bounds();
				maxImpactPoint = ((Bounds)(ref bounds)).get_center();
			}
			ShatterLocal(maxImpactPoint, adjustedImpulse + lastFrameImpact);
		}
		else if (adjustedImpulse != Vector3.get_zero())
		{
			if (NetGame.isServer || ReplayRecorder.isRecording)
			{
				SendCrack(adjustedImpulse, maxImpactPoint);
			}
			PlayCrack(adjustedImpulse, maxImpactPoint);
		}
		lastFrameImpact = adjustedImpulse;
		adjustedImpulse = Vector3.get_zero();
	}

	public void ShatterScript()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		ShatterLocal(Vector3.get_zero(), Vector3.get_zero());
	}

	private void PlayCrack(Vector3 adjustedImpulse, Vector3 maxImpactPoint)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		if (!shattered)
		{
			if ((Object)(object)audio != (Object)null)
			{
				audio.Crack(((Vector3)(ref adjustedImpulse)).get_magnitude(), maxImpactPoint);
			}
			if ((Object)(object)audioSource != (Object)null)
			{
				audioSource.Crack(((Vector3)(ref adjustedImpulse)).get_magnitude(), maxImpactPoint);
			}
			VoronoiTriangulate component = ((Component)this).GetComponent<VoronoiTriangulate>();
			if ((Object)(object)component != (Object)null)
			{
				component.Deform(maxImpactPoint, adjustedImpulse / breakTreshold);
			}
		}
	}

	private void SendCrack(Vector3 adjustedImpulse, Vector3 maxImpactPoint)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		NetStream stream = identity.BeginEvent(evtCrack);
		NetVector3.Quantize(maxImpactPoint - ((Component)this).get_transform().get_position(), 100f, 16).Write(stream, 6);
		NetVector3.Quantize(adjustedImpulse / breakTreshold, 10f, 16).Write(stream, 6);
		identity.EndEvent();
	}

	private void OnCrack(NetStream stream)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = NetVector3.Read(stream, 6, 16).Dequantize(100f) + ((Component)this).get_transform().get_position();
		Vector3 val2 = NetVector3.Read(stream, 6, 16).Dequantize(10f) * breakTreshold;
		PlayCrack(val2, val);
	}

	private void HandleCollision(Collision collision, bool enter)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		if (collision.get_contacts().Length == 0 || ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		Vector3 val = -collision.GetImpulse();
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		if (magnitude < impulseTreshold)
		{
			return;
		}
		float num = 1f;
		Transform val2 = collision.get_transform();
		while ((Object)(object)val2 != (Object)null)
		{
			if (Object.op_Implicit((Object)(object)((Component)val2).GetComponent<Human>()))
			{
				num = humanHardness;
				break;
			}
			ShatterHardness component = ((Component)val2).GetComponent<ShatterHardness>();
			if ((Object)(object)component != (Object)null)
			{
				num = component.hardness;
				break;
			}
			val2 = val2.get_parent();
		}
		if (magnitude * num > maxImpact)
		{
			maxImpact = magnitude * num;
			maxImpactPoint = ((ContactPoint)(ref collision.get_contacts()[0])).get_point();
		}
		adjustedImpulse += val * num;
	}

	protected virtual void Shatter(Vector3 contactPoint, Vector3 adjustedImpulse, float impactMagnitude, uint seed, uint netId)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		((Renderer)renderer).set_enabled(false);
		collider.set_enabled(false);
		if ((Object)(object)audio != (Object)null)
		{
			audio.Shatter(impactMagnitude, contactPoint);
		}
		if ((Object)(object)audioSource != (Object)null)
		{
			audioSource.Shatter(impactMagnitude, contactPoint);
		}
		GrabManager.Release(((Component)this).get_gameObject());
	}

	public virtual void ResetState(int checkpoint, int subObjectives)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((Renderer)renderer).set_enabled(true);
		collider.set_enabled(true);
		lastFrameImpact = (adjustedImpulse = Vector3.get_zero());
		accumulatedImpact = 0f;
		shattered = false;
	}

	public void StartNetwork(NetIdentity identity)
	{
		this.identity = identity;
		evtCrack = identity.RegisterEvent(OnCrack);
	}

	public void SetMaster(bool isMaster)
	{
		this.isMaster = isMaster;
	}

	public void CollectState(NetStream stream)
	{
		stream.Write(shattered);
		if (shattered)
		{
			netState.Write(stream);
		}
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		if (state1.ReadBool())
		{
			NetState.Read(state1);
		}
		ApplyState(state0);
	}

	public void ApplyState(NetStream state)
	{
		if (state.ReadBool())
		{
			netState = NetState.Read(state);
			if (!shattered)
			{
				ShatterNet(netState);
			}
		}
		else if (shattered)
		{
			ResetState(0, 0);
		}
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		bool num = state0?.ReadBool() ?? false;
		bool flag = state1.ReadBool();
		if (num)
		{
			NetState.Read(state0);
		}
		NetState netState = (flag ? NetState.Read(state1) : default(NetState));
		if (num == flag)
		{
			delta.Write(v: false);
			return;
		}
		delta.Write(v: true);
		if (flag)
		{
			netState.Write(delta);
		}
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		bool flag = state0?.ReadBool() ?? false;
		NetState netState = (flag ? NetState.Read(state0) : default(NetState));
		bool flag2 = delta.ReadBool() ^ flag;
		NetState netState2 = ((flag2 && !flag) ? NetState.Read(delta) : netState);
		result.Write(flag2);
		if (flag2)
		{
			netState2.Write(result);
		}
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return 41;
	}

	public void ShatterLocal(Vector3 pos, Vector3 impulse)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (!NetGame.isClient)
		{
			shattered = true;
			pos = collider.ClosestPointOnBounds(pos);
			uint num = (uint)Random.Range(0, 1023);
			netState = new NetState
			{
				shatterPos = QuantizePos(pos),
				shatterMagnitude = (uint)NetFloat.Quantize(((Vector3)(ref impulse)).get_magnitude(), 10000f, 11),
				shatterSeed = num,
				netId = NetStream.GetDynamicScopeId()
			};
			pos = DequantizePos(netState.shatterPos);
			Shatter(pos, impulse, ((Vector3)(ref impulse)).get_magnitude(), num, netState.netId);
		}
	}

	private void ShatterNet(NetState netState)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		shattered = true;
		Vector3 contactPoint = DequantizePos(netState.shatterPos);
		float impactMagnitude = NetFloat.Dequantize((int)netState.shatterMagnitude, 10000f, 11);
		uint shatterSeed = netState.shatterSeed;
		Shatter(contactPoint, Vector3.get_zero(), impactMagnitude, shatterSeed, netState.netId);
	}

	private NetVector3 QuantizePos(Vector3 pos)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		Collider obj = collider;
		BoxCollider val = (BoxCollider)(object)((obj is BoxCollider) ? obj : null);
		if ((Object)(object)val != (Object)null)
		{
			return NetVector3.Quantize(((Component)this).get_transform().InverseTransformPoint(pos) - val.get_center(), val.get_size() / 2f, 10);
		}
		Bounds bounds = collider.get_bounds();
		Vector3 vec = pos - ((Bounds)(ref bounds)).get_center();
		bounds = collider.get_bounds();
		return NetVector3.Quantize(vec, ((Bounds)(ref bounds)).get_extents(), 10);
	}

	private Vector3 DequantizePos(NetVector3 vec)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		Collider obj = collider;
		BoxCollider val = (BoxCollider)(object)((obj is BoxCollider) ? obj : null);
		if ((Object)(object)val != (Object)null)
		{
			return ((Component)this).get_transform().TransformPoint(vec.Dequantize(val.get_size() / 2f) + val.get_center());
		}
		Bounds bounds = collider.get_bounds();
		Vector3 val2 = vec.Dequantize(((Bounds)(ref bounds)).get_extents());
		bounds = collider.get_bounds();
		return val2 + ((Bounds)(ref bounds)).get_center();
	}

	protected ShatterBase()
		: this()
	{
	}//IL_0022: Unknown result type (might be due to invalid IL or missing references)
	//IL_0027: Unknown result type (might be due to invalid IL or missing references)

}

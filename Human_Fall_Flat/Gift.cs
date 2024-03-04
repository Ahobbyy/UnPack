using Multiplayer;
using UnityEngine;

public class Gift : GiftBase, INetBehavior
{
	public enum GiftPhase
	{
		Normal,
		Chute
	}

	private GiftGroup group;

	private NetBody netBody;

	private Rigidbody body;

	private GameObject chute;

	private float chuteHeight = 5f;

	private float chutePhase;

	private float targetHeight;

	public GiftPhase phase;

	private float spawnAfter;

	public static NetFloat encoder = new NetFloat(1f, 12, 4, 8);

	private void Start()
	{
		netBody = ((Component)this).GetComponent<NetBody>();
		netBody.respawn = false;
		body = ((Component)this).GetComponent<Rigidbody>();
		group = ((Component)this).GetComponentInParent<GiftGroup>();
	}

	private void FixedUpdate()
	{
		if (!NetGame.isClient && !ReplayRecorder.isPlaying)
		{
			if (phase == GiftPhase.Normal)
			{
				HandleNormal();
			}
			if (phase == GiftPhase.Chute)
			{
				HandleChute();
			}
		}
	}

	private void HandleNormal()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).get_transform().get_position().y < netBody.despawnHeight + 10f)
		{
			Vector3 position = group.GetRandomSpawn().GetPosition();
			netBody.Respawn(position - netBody.startPos + Vector3.get_up() * (float)Random.Range(20, 30));
			phase = GiftPhase.Chute;
			targetHeight = position.y;
		}
	}

	private void HandleChute()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		float num2 = ((Component)this).get_transform().get_position().y - targetHeight;
		num = ((!(num2 < 3f)) ? Mathf.InverseLerp(15f, 5f, num2) : Mathf.InverseLerp(2f, 3f, num2));
		if (num2 < 2f)
		{
			phase = GiftPhase.Normal;
		}
		else if (body.get_velocity().y < -1f * (2f - num))
		{
			body.SafeAddForce(Vector3.get_up() * 50f * num, (ForceMode)5);
		}
		ApplyChutePhase(num);
	}

	private void ApplyChutePhase(float phase)
	{
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (phase == 0f)
		{
			if ((Object)(object)chute != (Object)null)
			{
				GiftParachute.instance.Release(chute);
				chute = null;
			}
			chutePhase = phase;
			return;
		}
		if ((Object)(object)chute == (Object)null)
		{
			chute = GiftParachute.instance.Allocate();
			GiftParachute.instance.PlaySound(((Component)this).get_transform().get_position());
		}
		chutePhase = phase;
		chute.get_transform().set_position(((Component)this).get_transform().get_position());
		chute.get_transform().set_localScale(new Vector3(chutePhase, chutePhase, Mathf.Lerp(1.2f, 1f, chutePhase)));
	}

	public void StartNetwork(NetIdentity identity)
	{
		giftId = identity.sceneId;
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public void CollectState(NetStream stream)
	{
		encoder.CollectState(stream, chutePhase);
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		float num = encoder.ApplyLerpedState(state0, state1, mix);
		ApplyChutePhase(num);
	}

	public void ApplyState(NetStream state)
	{
		float num = encoder.ApplyState(state);
		ApplyChutePhase(num);
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		encoder.CalculateDelta(state0, state1, delta);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		encoder.AddDelta(state0, delta, result);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return encoder.CalculateMaxDeltaSizeInBits();
	}
}

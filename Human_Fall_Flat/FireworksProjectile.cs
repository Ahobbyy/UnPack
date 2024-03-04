using HumanAPI;
using Multiplayer;
using UnityEngine;

public class FireworksProjectile : MonoBehaviour, INetBehavior
{
	public enum FireworkState
	{
		Inactive,
		Shot,
		Exploded
	}

	public GameObject visual;

	public Sound2 shootSound;

	public Sound2 explodeSound;

	public ParticleSystem flyParticles;

	public ParticleSystem explodeParticles;

	public Light light;

	private Rigidbody body;

	private FireworkState state;

	private float life;

	private const float explodeIn = 2f;

	private const float terminateIn = 5f;

	private void Awake()
	{
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		if (NetGame.isClient || ReplayRecorder.isPlaying)
		{
			return;
		}
		if (state == FireworkState.Shot)
		{
			body.AddForce(((Component)this).get_transform().get_forward(), (ForceMode)5);
			body.AddForce(Vector3.get_up() * 5f, (ForceMode)5);
		}
		if (state == FireworkState.Shot)
		{
			life += Time.get_fixedDeltaTime() / 2f;
			if (life >= 1f)
			{
				Explode();
			}
			else
			{
				Apply(state, life);
			}
		}
		else if (state == FireworkState.Exploded)
		{
			life += Time.get_fixedDeltaTime() / 5f;
			if (life >= 1f)
			{
				Terminate();
			}
			else
			{
				Apply(state, life);
			}
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!NetGame.isClient && !ReplayRecorder.isPlaying && state == FireworkState.Shot)
		{
			Explode();
		}
	}

	public void Shoot(Vector3 pos, Vector3 dir)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_gameObject().SetActive(true);
		((Component)this).get_transform().set_position(pos);
		((Component)this).get_transform().set_rotation(Quaternion.LookRotation(dir));
		body.set_angularVelocity(Vector3.get_zero());
		body.set_velocity(((Component)this).get_transform().get_forward() * 20f);
		Apply(FireworkState.Shot, 0f);
	}

	public void Shoot(Transform muzzle)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		Shoot(muzzle.get_position(), muzzle.get_forward());
	}

	private void Explode()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		Collider[] array = Physics.OverlapSphere(((Component)this).get_transform().get_position(), 2f);
		for (int i = 0; i < array.Length; i++)
		{
			Rigidbody componentInParent = ((Component)array[i]).GetComponentInParent<Rigidbody>();
			if ((Object)(object)componentInParent != (Object)null && !componentInParent.get_isKinematic())
			{
				componentInParent.AddExplosionForce(1500f * Mathf.Pow(componentInParent.get_mass(), 0.25f), ((Component)this).get_transform().get_position(), 8f);
				Human componentInParent2 = ((Component)componentInParent).GetComponentInParent<Human>();
				if ((Object)(object)componentInParent2 != (Object)null)
				{
					componentInParent2.MakeUnconscious(0.5f);
				}
			}
		}
		Apply(FireworkState.Exploded, 0f);
	}

	private void Terminate()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).get_transform().set_position(-200f * Vector3.get_up());
		Fireworks.instance.Kill(this);
		Apply(FireworkState.Inactive, 0f);
	}

	private void Apply(FireworkState newState, float newPhase, bool manageLifetime = false)
	{
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		life = newPhase;
		if (newState != state)
		{
			state = newState;
			if (state == FireworkState.Shot)
			{
				visual.SetActive(true);
				flyParticles.set_enableEmission(true);
				light.set_range(5f);
				if (life <= 0.5f)
				{
					shootSound.PlayOneShot(((Component)Fireworks.instance).get_transform().get_position());
				}
			}
			else if (state == FireworkState.Exploded)
			{
				visual.SetActive(false);
				flyParticles.set_enableEmission(false);
				if (life <= 0.5f)
				{
					explodeParticles.Emit(128);
					explodeSound.PlayOneShot(((Component)this).get_transform().get_position());
				}
			}
		}
		float num = ((state == FireworkState.Exploded) ? Mathf.Lerp(50f, 0f, life * 10f) : 5f);
		if (light.get_range() != num)
		{
			light.set_range(num);
		}
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
	}

	public void CollectState(NetStream stream)
	{
		NetBoolEncoder.CollectState(stream, state == FireworkState.Shot);
		NetBoolEncoder.CollectState(stream, state == FireworkState.Exploded);
		NetSignal.encoder.CollectState(stream, life);
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		bool num = NetBoolEncoder.ApplyLerpedState(state0, state1, mix);
		bool flag = NetBoolEncoder.ApplyLerpedState(state0, state1, mix);
		FireworkState fireworkState = (num ? FireworkState.Shot : (flag ? FireworkState.Exploded : FireworkState.Inactive));
		float newPhase = NetSignal.encoder.ApplyLerpedState(state0, state1, mix);
		if (fireworkState != 0 && state == FireworkState.Inactive)
		{
			Fireworks.instance.MarkUsed(this);
		}
		if (fireworkState == FireworkState.Inactive && state != 0)
		{
			Fireworks.instance.Kill(this);
		}
		Apply(fireworkState, newPhase);
	}

	public void ApplyState(NetStream state)
	{
		bool num = NetBoolEncoder.ApplyState(state);
		bool flag = NetBoolEncoder.ApplyState(state);
		FireworkState fireworkState = (num ? FireworkState.Shot : (flag ? FireworkState.Exploded : FireworkState.Inactive));
		float newPhase = NetSignal.encoder.ApplyState(state);
		if (fireworkState != 0 && this.state == FireworkState.Inactive)
		{
			Fireworks.instance.MarkUsed(this);
		}
		if (fireworkState == FireworkState.Inactive && this.state != 0)
		{
			Fireworks.instance.Kill(this);
		}
		Apply(fireworkState, newPhase);
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		NetBoolEncoder.CalculateDelta(state0, state1, delta);
		NetBoolEncoder.CalculateDelta(state0, state1, delta);
		NetSignal.encoder.CalculateDelta(state0, state1, delta);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		NetBoolEncoder.AddDelta(state0, delta, result);
		NetBoolEncoder.AddDelta(state0, delta, result);
		NetSignal.encoder.AddDelta(state0, delta, result);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		return 2 * NetBoolEncoder.CalculateMaxDeltaSizeInBits() + NetSignal.encoder.CalculateMaxDeltaSizeInBits();
	}

	public FireworksProjectile()
		: this()
	{
	}
}

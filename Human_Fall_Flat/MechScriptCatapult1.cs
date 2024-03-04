using System.Collections;
using HumanAPI;
using Multiplayer;
using UnityEngine;

public class MechScriptCatapult1 : MonoBehaviour, INetBehavior
{
	[Tooltip("Reference to the joint Ratchet of this gameobject")]
	public JointScriptRatchet1 ratchet;

	[Tooltip("Reference to the Angular jount used as the Arm in this gameobject")]
	public AngularJoint arm;

	[Tooltip("Reference to the Angular Joint being used as the release mech for this gameobject")]
	public AngularJoint release;

	[Tooltip("Refence to the Audio source to use as the release sound")]
	public AudioSource releaseSound;

	[Tooltip("Reference to the Catapult Rope being used as the rope in this gameobject")]
	public CatapultRope rope;

	private bool firing;

	private bool hasHuman;

	[Tooltip("The angle the catapult will return to once fired")]
	public float catapultAngle;

	[Tooltip("How fast to move the Arm of the catapult to the firing angle initially")]
	public float initialAcceleration = 90f;

	[Tooltip("Max amount of acceleration for the catapult arm")]
	public float accelerationAcceleration = 270f;

	private float fireTime;

	private float fireStart;

	private float releaseArmIn;

	private float initialMass;

	private float armMass;

	private float currentAudioClipDuration;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private void Awake()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Awake "));
		}
		initialMass = ((Component)this).GetComponent<Rigidbody>().get_mass();
		armMass = ((Component)arm.body).GetComponent<Rigidbody>().get_mass();
	}

	private void LateUpdate()
	{
		rope.catapultWindCount = ratchet.GetValue() / 360f + 1.5f;
	}

	public void FixedUpdate()
	{
		if (ReplayRecorder.isPlaying || NetGame.isClient)
		{
			return;
		}
		bool flag = GrabManager.IsGrabbedAny(((Component)release.body).get_gameObject());
		bool flag2 = GrabManager.IsGrabbedAny(((Component)ratchet.body).get_gameObject());
		if (!firing && flag && !flag2 && release.GetValue() > release.centerValue && catapultAngle < arm.maxValue - 10f)
		{
			hasHuman = false;
			for (int i = 0; i < Human.all.Count; i++)
			{
				Human human = Human.all[i];
				bool num = human.groundManager.IsStanding(((Component)arm.body).get_gameObject());
				bool flag3 = human.grabManager.IsGrabbed(((Component)arm.body).get_gameObject());
				if (num || flag3)
				{
					human.ReleaseGrab(((Component)release.body).get_gameObject());
					hasHuman = true;
					human.ragdoll.ToggleHeavyArms((Object)(object)human.ragdoll.partLeftHand.sensor.grabBody == (Object)(object)((Component)arm.body).GetComponent<Rigidbody>(), (Object)(object)human.ragdoll.partRightHand.sensor.grabBody == (Object)(object)((Component)arm.body).GetComponent<Rigidbody>());
				}
				if (num)
				{
					StatsAndAchievements.UnlockAchievement(Achievement.ACH_SIEGE_HUMAN_CANNON);
				}
			}
			firing = true;
			fireTime = 0f;
			fireStart = catapultAngle;
			if ((Object)(object)releaseSound != (Object)null)
			{
				releaseSound.Play();
			}
			currentAudioClipDuration = releaseSound.get_clip().get_length();
			currentAudioClipDuration -= 0.5f;
			((MonoBehaviour)this).StartCoroutine(Wait());
			ratchet.release = true;
			release.SetTarget(release.maxValue);
		}
		if (firing && catapultAngle == arm.maxValue)
		{
			firing = false;
			((Component)arm.anchor).GetComponent<Rigidbody>().set_isKinematic(false);
			ratchet.release = false;
			release.SetTarget(release.minValue);
			releaseArmIn = 0.12f;
		}
		if (releaseArmIn > 0f)
		{
			releaseArmIn -= Time.get_fixedDeltaTime();
			if (releaseArmIn <= 0f)
			{
				for (int j = 0; j < Human.all.Count; j++)
				{
					Human human2 = Human.all[j];
					human2.ReleaseGrab(((Component)arm.body).get_gameObject(), 0.1f);
					human2.ragdoll.ReleaseHeavyArms();
				}
			}
		}
		if (firing)
		{
			fireTime += Time.get_fixedDeltaTime();
			catapultAngle = Mathf.Clamp(fireStart + initialAcceleration * fireTime * fireTime / 2f + accelerationAcceleration * fireTime * fireTime * fireTime / 3f, arm.minValue, arm.maxValue);
			arm.SetTarget(catapultAngle);
			ratchet.SetValue(Mathf.Lerp(ratchet.minValue, ratchet.maxValue, Mathf.InverseLerp(arm.maxValue, arm.minValue, catapultAngle)));
		}
		else
		{
			catapultAngle = Mathf.Lerp(arm.maxValue, arm.minValue, Mathf.InverseLerp(ratchet.minValue, ratchet.maxValue, ratchet.GetValue()));
			arm.SetTarget(catapultAngle);
		}
		float num2 = 1f;
		if (firing)
		{
			num2 = (hasHuman ? 2f : 1.5f);
		}
		else if (GrabManager.IsGrabbedAny(((Component)this).get_gameObject()) && !flag && !flag2)
		{
			num2 = 0.2f;
		}
		Rigidbody component = ((Component)this).GetComponent<Rigidbody>();
		Rigidbody component2 = ((Component)arm.body).GetComponent<Rigidbody>();
		if (component.get_mass() != num2 * initialMass)
		{
			component.set_mass(num2 * initialMass);
			component2.set_mass(num2 * armMass);
		}
	}

	public void StartNetwork(NetIdentity identity)
	{
	}

	public void SetMaster(bool isMaster)
	{
	}

	public void CollectState(NetStream stream)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Collect State "));
		}
		NetSignal.encoder.CollectState(stream, Mathf.InverseLerp(ratchet.minValue, ratchet.maxValue, ratchet.GetValue()));
		NetBoolEncoder.CollectState(stream, firing);
	}

	private void Apply(float ratchetPhase, bool firing)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Apply Ratchet Phase"));
		}
		catapultAngle = Mathf.Lerp(arm.maxValue, arm.minValue, ratchetPhase);
		if (firing != this.firing)
		{
			this.firing = firing;
			ratchet.release = firing;
			if (firing)
			{
				release.SetTarget(release.maxValue);
				if (catapultAngle < arm.maxValue - 10f && (Object)(object)releaseSound != (Object)null)
				{
					releaseSound.Play();
				}
			}
			else
			{
				ResetState(0, 0);
				release.SetTarget(release.maxValue);
			}
		}
		fireStart = catapultAngle;
		fireTime = 0f;
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Reset State "));
		}
		firing = false;
	}

	public void ApplyState(NetStream state)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Apply State"));
		}
		Apply(NetSignal.encoder.ApplyState(state), NetBoolEncoder.ApplyState(state));
	}

	public void ApplyLerpedState(NetStream state0, NetStream state1, float mix)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Lerped State "));
		}
		Apply(NetSignal.encoder.ApplyLerpedState(state0, state1, mix), NetBoolEncoder.ApplyLerpedState(state0, state1, mix));
	}

	public void CalculateDelta(NetStream state0, NetStream state1, NetStream delta)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Calculate Delta "));
		}
		NetSignal.encoder.CalculateDelta(state0, state1, delta);
		NetBoolEncoder.CalculateDelta(state0, state1, delta);
	}

	public void AddDelta(NetStream state0, NetStream delta, NetStream result)
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Add Delta "));
		}
		NetSignal.encoder.AddDelta(state0, delta, result);
		NetBoolEncoder.AddDelta(state0, delta, result);
	}

	public int CalculateMaxDeltaSizeInBits()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " Calculate Delta Max in Bits "));
		}
		return NetSignal.encoder.CalculateMaxDeltaSizeInBits() + NetBoolEncoder.CalculateMaxDeltaSizeInBits();
	}

	public IEnumerator Wait()
	{
		if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + "Waiting for the sound to finish "));
		}
		yield return (object)new WaitForSecondsRealtime(currentAudioClipDuration);
		SignalScriptNode1 component = ((Component)this).GetComponent<SignalScriptNode1>();
		if ((Object)(object)component != (Object)null)
		{
			component.SendSignal(1f);
		}
	}

	public MechScriptCatapult1()
		: this()
	{
	}
}

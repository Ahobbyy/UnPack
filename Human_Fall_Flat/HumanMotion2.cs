using UnityEngine;

public class HumanMotion2 : MonoBehaviour
{
	private Human human;

	private Ragdoll ragdoll;

	public LayerMask grabLayers;

	public TorsoMuscles torso;

	public LegMuscles legs;

	public HandMuscles hands;

	public void Initialize()
	{
		if (!((Object)(object)human != (Object)null))
		{
			human = ((Component)this).GetComponent<Human>();
			ragdoll = human.ragdoll;
			torso = new TorsoMuscles(human, ragdoll, this);
			legs = new LegMuscles(human, ragdoll, this);
			hands = new HandMuscles(human, ragdoll, this);
		}
	}

	public void OnFixedUpdate()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		hands.OnFixedUpdate();
		torso.OnFixedUpdate();
		legs.OnFixedUpdate(torso.feedbackForce);
	}

	public static void AlignLook(HumanSegment segment, Quaternion targetRotation, float accelerationSpring, float damping)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = targetRotation * Quaternion.Inverse(segment.transform.get_rotation());
		float num = default(float);
		Vector3 val2 = default(Vector3);
		((Quaternion)(ref val)).ToAngleAxis(ref num, ref val2);
		if (num > 180f)
		{
			num -= 360f;
		}
		if (num < -180f)
		{
			num += 360f;
		}
		segment.rigidbody.SafeAddTorque(val2 * num * accelerationSpring - segment.rigidbody.get_angularVelocity() * damping, (ForceMode)5);
	}

	public static void AlignToVector(Rigidbody body, Vector3 alignmentVector, Vector3 targetVector, float spring)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		AlignToVector(body, alignmentVector, targetVector, spring, spring);
	}

	public static void AlignToVector(Rigidbody body, Vector3 alignmentVector, Vector3 targetVector, float spring, float maxTorque)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		float num = 0.1f;
		Vector3 angularVelocity = body.get_angularVelocity();
		Vector3 val = Quaternion.AngleAxis(((Vector3)(ref angularVelocity)).get_magnitude() * 57.29578f * num, body.get_angularVelocity()) * ((Vector3)(ref alignmentVector)).get_normalized();
		Vector3 val2 = Vector3.Cross(((Vector3)(ref val)).get_normalized(), ((Vector3)(ref targetVector)).get_normalized());
		Vector3 val3 = ((Vector3)(ref val2)).get_normalized() * Mathf.Asin(Mathf.Clamp01(((Vector3)(ref val2)).get_magnitude()));
		Vector3 val4 = spring * val3;
		body.SafeAddTorque(Vector3.ClampMagnitude(val4, maxTorque), (ForceMode)0);
	}

	public static void AlignToVector(HumanSegment part, Vector3 alignmentVector, Vector3 targetVector, float spring)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		AlignToVector(part.rigidbody, alignmentVector, targetVector, spring);
	}

	public HumanMotion2()
		: this()
	{
	}
}

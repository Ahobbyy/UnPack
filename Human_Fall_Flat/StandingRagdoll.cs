using UnityEngine;

public class StandingRagdoll : MonoBehaviour
{
	public float standForce = 1000f;

	private Ragdoll ragdoll;

	private void OnEnable()
	{
		ragdoll = ((Component)this).GetComponent<Ragdoll>();
	}

	private void FixedUpdate()
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_010a: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		if (Human.instance.grabManager.hasGrabbed)
		{
			if (Human.instance.controls.walkSpeed > 0f)
			{
				standForce -= Time.get_fixedDeltaTime() * 300f;
			}
		}
		else if (standForce < 1000f)
		{
			standForce = 0f;
		}
		if (standForce != 0f)
		{
			ragdoll.partHead.rigidbody.SafeAddForce(Vector3.get_up() * standForce, (ForceMode)0);
			Vector3 val = ((Component)Human.instance).get_transform().get_position() - ((Component)this).get_transform().get_position();
			if (((Vector3)(ref val)).get_magnitude() < 3f)
			{
				ragdoll.partRightHand.rigidbody.SafeAddForce(new Vector3(-0.2f, 1f, 0f) * standForce * 0.05f, (ForceMode)0);
			}
			ragdoll.partLeftFoot.rigidbody.SafeAddForce(-Vector3.get_up() * standForce / 2f, (ForceMode)0);
			ragdoll.partRightFoot.rigidbody.SafeAddForce(-Vector3.get_up() * standForce / 2f, (ForceMode)0);
		}
	}

	public StandingRagdoll()
		: this()
	{
	}
}

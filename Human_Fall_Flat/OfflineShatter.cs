using UnityEngine;

public class OfflineShatter : ShatterBase
{
	public GameObject shardRoot;

	public float minExplodeImpulse;

	public float maxExplodeImpulse = float.PositiveInfinity;

	public float perShardImpulseFraction = 0.25f;

	public float maxShardVelocity = float.PositiveInfinity;

	protected override void Shatter(Vector3 contactPoint, Vector3 adjustedImpulse, float impactMagnitude, uint seed, uint netId)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		base.Shatter(contactPoint, adjustedImpulse, impactMagnitude, seed, netId);
		shardRoot.SetActive(true);
		Vector3 normalized = ((Vector3)(ref adjustedImpulse)).get_normalized();
		float num = Mathf.Clamp(((Vector3)(ref adjustedImpulse)).get_magnitude(), minExplodeImpulse, maxExplodeImpulse) * perShardImpulseFraction;
		for (int i = 0; i < shardRoot.get_transform().get_childCount(); i++)
		{
			Rigidbody component = ((Component)shardRoot.get_transform().GetChild(i)).GetComponent<Rigidbody>();
			_ = (component.get_position() - contactPoint) * 10f;
			float num2 = Mathf.Clamp(num, 0f, maxShardVelocity * component.get_mass());
			component.SafeAddForceAtPosition(-normalized * num2, (3f * contactPoint + component.get_position()) / 4f, (ForceMode)1);
		}
	}

	public override void ResetState(int checkpoint, int subObjectives)
	{
		base.ResetState(checkpoint, subObjectives);
		shardRoot.SetActive(false);
	}
}

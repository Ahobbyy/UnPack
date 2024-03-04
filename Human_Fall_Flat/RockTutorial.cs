using UnityEngine;

public class RockTutorial : TutorialBlock
{
	public Rigidbody body;

	public float angleDistanceSuccess = 10f;

	private Quaternion bodyStartRot;

	protected override void OnEnable()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		bodyStartRot = body.get_rotation();
	}

	public override bool IsPlayerActivityMakingSense()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		if (GroundManager.IsStandingAny(((Component)body).get_gameObject()))
		{
			Vector3 angularVelocity = body.get_angularVelocity();
			return ((Vector3)(ref angularVelocity)).get_magnitude() > 0.1f;
		}
		return false;
	}

	public override bool CheckInstantSuccess(bool playerInside)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		Quaternion rotation = body.get_rotation();
		return Mathf.Abs(Math2d.NormalizeAngleDeg(((Quaternion)(ref rotation)).get_eulerAngles().z - ((Quaternion)(ref bodyStartRot)).get_eulerAngles().z)) > angleDistanceSuccess;
	}
}

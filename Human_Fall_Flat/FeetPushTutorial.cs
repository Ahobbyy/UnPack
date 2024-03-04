using UnityEngine;

public class FeetPushTutorial : TutorialBlock
{
	public Rigidbody platform;

	public float platformDistanceSuccess = 1f;

	private Vector3 platformStartPos;

	protected override void OnEnable()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		base.OnEnable();
		platformStartPos = platform.get_position();
	}

	public override bool IsPlayerActivityMakingSense()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < Human.all.Count; i++)
		{
			Human human = Human.all[i];
			if (human.hasGrabbed && human.groundManager.IsStanding(((Component)platform).get_gameObject()))
			{
				Vector3 velocity = platform.get_velocity();
				if (((Vector3)(ref velocity)).get_magnitude() > 0.5f)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override bool CheckInstantSuccess(bool playerInside)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (GroundManager.IsStandingAny(((Component)platform).get_gameObject()))
		{
			Vector3 val = platform.get_position() - platformStartPos;
			return ((Vector3)(ref val)).get_magnitude() > platformDistanceSuccess;
		}
		return false;
	}
}

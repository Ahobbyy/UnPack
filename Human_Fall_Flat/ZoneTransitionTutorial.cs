using UnityEngine;

public class ZoneTransitionTutorial : TutorialBlock
{
	public float maxTimeToTarget = 5f;

	private float lastSeenInZone;

	private bool targetReached;

	public override bool IsPlayerActivityMakingSense()
	{
		return false;
	}

	public override bool CheckInstantSuccess(bool playerInside)
	{
		lastSeenInZone = Time.get_time();
		return targetReached;
	}

	public void OnTargetReached()
	{
		if (Time.get_time() - lastSeenInZone < maxTimeToTarget)
		{
			targetReached = true;
		}
	}
}

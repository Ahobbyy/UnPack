using UnityEngine;

public class SurfAchievement : PathAchievementBase
{
	public Transform[] matressParts;

	public override void UnlockAchievement()
	{
		StatsAndAchievements.UnlockAchievement(Achievement.ACH_WATER_SURF);
	}

	protected override bool IsValid(Collider trackedPlayer)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = ((Component)trackedPlayer).get_transform().get_position();
		bool flag = false;
		for (int i = 0; i < matressParts.Length; i++)
		{
			Vector3 val = position - matressParts[i].get_position();
			flag |= (((Vector3)(ref val)).get_sqrMagnitude() < 25f) | (val.y < -1f);
		}
		return flag;
	}
}

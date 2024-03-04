using UnityEngine;

public class FeetHeadAchievement : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)other).get_tag() == "Player")
		{
			Human componentInParent = ((Component)other).GetComponentInParent<Human>();
			if (componentInParent.ragdoll.partHead.transform.get_position().y > componentInParent.ragdoll.partHips.transform.get_position().y)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_LVL_RIVER_FEET);
			}
			else
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_LVL_RIVER_HEAD);
			}
		}
	}

	public FeetHeadAchievement()
		: this()
	{
	}
}

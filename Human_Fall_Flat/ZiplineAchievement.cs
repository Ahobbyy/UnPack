using UnityEngine;

public class ZiplineAchievement : MonoBehaviour
{
	public bool achieved;

	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player" && !achieved)
		{
			achieved = true;
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_SIEGE_ZIPLINE);
		}
	}

	public ZiplineAchievement()
		: this()
	{
	}
}

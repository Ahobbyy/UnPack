using UnityEngine;

public class StatueHeadAchievement : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_INTRO_STATUE_HEAD);
		}
	}

	public StatueHeadAchievement()
		: this()
	{
	}
}

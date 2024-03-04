using UnityEngine;

public class OverlookAchievement : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_AZTEC_OVERLOOK);
		}
	}

	public OverlookAchievement()
		: this()
	{
	}
}

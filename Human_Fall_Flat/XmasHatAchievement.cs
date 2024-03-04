using UnityEngine;

public class XmasHatAchievement : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_XMAS_SNOWMAN_HAT);
		}
	}

	public XmasHatAchievement()
		: this()
	{
	}
}

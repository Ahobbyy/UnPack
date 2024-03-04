using UnityEngine;

public class StealBatteryAchievement : MonoBehaviour
{
	public Collider battery;

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)other == (Object)(object)battery)
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_POWER_STATUE_BATTERY);
		}
	}

	public StealBatteryAchievement()
		: this()
	{
	}
}

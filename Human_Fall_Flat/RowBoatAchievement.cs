using UnityEngine;

public class RowBoatAchievement : MonoBehaviour
{
	public Collider boat;

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)other == (Object)(object)boat)
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_WATER_ROW_BOAT);
		}
	}

	public RowBoatAchievement()
		: this()
	{
	}
}

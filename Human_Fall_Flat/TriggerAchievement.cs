using UnityEngine;

public class TriggerAchievement : MonoBehaviour
{
	public string AchievementName;

	public void AchievementUnlocked()
	{
		if (AchievementName != "")
		{
			Debug.LogFormat("{0} Achievement unlocked!", new object[1] { AchievementName });
		}
		else
		{
			Debug.LogFormat("Unknown achievement unlocked, please check input on {0}.", new object[1] { ((Object)((Component)this).get_gameObject().get_transform()).get_name() });
		}
	}

	public TriggerAchievement()
		: this()
	{
	}
}

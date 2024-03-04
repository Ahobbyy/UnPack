using UnityEngine;

public class FryMeToTheMoonAchievement : MonoBehaviour
{
	public void OnFryMeToTheMoonAchievementCheck(GameObject objectToBeGrabbing)
	{
		if ((Object)(object)objectToBeGrabbing != (Object)null && GrabManager.IsGrabbedAny(objectToBeGrabbing))
		{
			Debug.Log((object)"FRY ME TO THE MOON ACHIEVEMENT COMPLETE");
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_HALLOWEEN_FRY_ME_TO_THE_MOON);
		}
	}

	public FryMeToTheMoonAchievement()
		: this()
	{
	}
}

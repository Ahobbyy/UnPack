using System.Collections.Generic;
using UnityEngine;

public class AchievementBigWheel : MonoBehaviour, IGrabbable, IReset
{
	public int spinsRequired = 3;

	private List<Human> grabbingHumans = new List<Human>();

	private List<int> spinCount = new List<int>();

	private HashSet<Human> FindGrabbingHumans()
	{
		HashSet<Human> hashSet = new HashSet<Human>();
		foreach (Human item in Human.all)
		{
			foreach (GameObject grabbedObject in item.grabManager.grabbedObjects)
			{
				if ((Object)(object)grabbedObject.get_gameObject() == (Object)(object)((Component)this).get_gameObject())
				{
					hashSet.Add(item);
				}
			}
		}
		return hashSet;
	}

	public void OnGrab()
	{
		HashSet<Human> hashSet = FindGrabbingHumans();
		hashSet.ExceptWith(grabbingHumans);
		foreach (Human item in hashSet)
		{
			grabbingHumans.Add(item);
			spinCount.Add(0);
		}
		BlockClimbAchievement();
	}

	public void OnRelease()
	{
		HashSet<Human> hashSet = FindGrabbingHumans();
		for (int i = 0; i < grabbingHumans.Count; i++)
		{
			if (!hashSet.Contains(grabbingHumans[i]))
			{
				grabbingHumans.RemoveAt(i);
				spinCount.RemoveAt(i);
				i--;
			}
		}
		BlockClimbAchievement();
	}

	public void TriggerVolumeEntered(Human human)
	{
		for (int i = 0; i < grabbingHumans.Count; i++)
		{
			if ((Object)(object)grabbingHumans[i] == (Object)(object)human)
			{
				spinCount[i]++;
				if (spinCount[i] == spinsRequired)
				{
					StatsAndAchievements.UnlockAchievement(Achievement.ACH_STEAM_GET_DIZZY);
				}
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		new List<Human>();
		new List<int>();
		BlockClimbAchievement();
	}

	public void BlockClimbAchievement()
	{
		if (grabbingHumans.Count != 0)
		{
			StatsAndAchievements.BlockClimb = true;
		}
		else
		{
			StatsAndAchievements.BlockClimb = false;
		}
	}

	public AchievementBigWheel()
		: this()
	{
	}
}

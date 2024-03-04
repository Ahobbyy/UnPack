using UnityEngine;

public class SeeSawAchievement : MonoBehaviour
{
	public static SeeSawAchievement instance;

	private bool failed;

	private void Start()
	{
		instance = this;
	}

	private void Update()
	{
	}

	public void Fail()
	{
		failed = true;
	}

	public void OnTriggerEnter(Collider other)
	{
		if (failed)
		{
			return;
		}
		foreach (Human item in Human.all)
		{
			if ((Object)(object)((Component)item).GetComponent<Collider>() == (Object)(object)other)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_ICE_NO_ICE_BABY);
				break;
			}
		}
	}

	public SeeSawAchievement()
		: this()
	{
	}
}

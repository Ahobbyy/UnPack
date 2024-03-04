using HumanAPI;
using UnityEngine;

public class LighthouseAchievement : Node
{
	public NodeInput input;

	public float delay = 3f;

	private float timer;

	protected override void OnEnable()
	{
		base.OnEnable();
		timer = delay;
	}

	private void Update()
	{
		if (!(timer >= 0f))
		{
			return;
		}
		if (Mathf.Abs(input.value) >= 0.5f)
		{
			timer -= Time.get_deltaTime();
			if (timer <= 0f)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_WATER_LIGHTHOUSE);
				((Component)this).get_gameObject().SetActive(false);
			}
		}
		else
		{
			timer = delay;
		}
	}
}

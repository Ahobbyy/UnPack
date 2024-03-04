using UnityEngine;

public class BigStackAchievement : MonoBehaviour
{
	public Collider[] boxes;

	public float limit = 2f;

	private bool currentlyStacked;

	private void Update()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = boxes[0].get_bounds();
		for (int i = 1; i < boxes.Length; i++)
		{
			((Bounds)(ref bounds)).Encapsulate(boxes[i].get_bounds());
		}
		if (((Bounds)(ref bounds)).get_size().x <= limit && ((Bounds)(ref bounds)).get_size().z <= limit)
		{
			if (!currentlyStacked)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_CARRY_BIG_STACK);
			}
			currentlyStacked = true;
		}
		else
		{
			currentlyStacked = false;
		}
	}

	public BigStackAchievement()
		: this()
	{
	}
}

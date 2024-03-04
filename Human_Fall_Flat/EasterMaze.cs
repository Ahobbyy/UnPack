using UnityEngine;

public class EasterMaze : PathAchievementBase
{
	public Transform lantern;

	private float timeToCool;

	protected override bool IsValid(Collider trackedPlayer)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!(timeToCool > 0f))
		{
			Bounds bounds = ((Collider)((Component)zoneCollider).GetComponent<BoxCollider>()).get_bounds();
			if (!((Bounds)(ref bounds)).Contains(lantern.get_position()))
			{
				return true;
			}
		}
		return false;
	}

	public override void UnlockAchievement()
	{
		if (!(timeToCool > 0f))
		{
			((Component)this).GetComponent<AudioSource>().Play();
			timeToCool = 60f;
		}
	}

	private void Update()
	{
		if (timeToCool > 0f)
		{
			timeToCool -= Time.get_deltaTime();
		}
	}
}

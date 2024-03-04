using UnityEngine;

public class BellAchievement : MonoBehaviour
{
	public float impulseThreshold = 100f;

	public float velocityThreshold = 5f;

	public void OnCollisionEnter(Collision collision)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)collision.get_gameObject()).get_name().Contains("Rock"))
		{
			return;
		}
		Vector3 val = collision.get_impulse();
		if (!(((Vector3)(ref val)).get_magnitude() > impulseThreshold))
		{
			val = collision.get_rigidbody().get_velocity();
			if (!(((Vector3)(ref val)).get_magnitude() > velocityThreshold))
			{
				return;
			}
		}
		StatsAndAchievements.UnlockAchievement(Achievement.ACH_SIEGE_BELL);
	}

	public BellAchievement()
		: this()
	{
	}
}

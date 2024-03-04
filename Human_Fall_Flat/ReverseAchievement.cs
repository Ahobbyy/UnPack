using UnityEngine;

public class ReverseAchievement : MonoBehaviour
{
	public Collider ship;

	public void OnTriggerEnter(Collider other)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)other == (Object)(object)ship && Vector3.Dot(((Component)other).get_transform().get_up(), ((Component)this).get_transform().get_forward()) > 0f)
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_WATER_REVERSE_GEAR);
		}
	}

	public ReverseAchievement()
		: this()
	{
	}
}

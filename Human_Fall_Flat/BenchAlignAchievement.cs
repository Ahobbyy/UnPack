using UnityEngine;

public class BenchAlignAchievement : MonoBehaviour
{
	private bool awarded;

	private void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (!awarded && ((Component)this).get_transform().get_forward().y > 0.97f && ((Component)this).get_transform().get_up().x < -0.97f)
		{
			StatsAndAchievements.UnlockAchievement(Achievement.ACH_PUSH_BENCH_ALIGN);
			awarded = true;
		}
	}

	public BenchAlignAchievement()
		: this()
	{
	}
}

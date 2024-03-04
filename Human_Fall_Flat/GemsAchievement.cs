using UnityEngine;

public class GemsAchievement : MonoBehaviour
{
	public Transform gemsParent;

	private Transform[] gems;

	public float limit = 5f;

	private bool currentlyStacked;

	private void Awake()
	{
		gems = (Transform[])(object)new Transform[gemsParent.get_childCount()];
		for (int i = 0; i < gemsParent.get_childCount(); i++)
		{
			gems[i] = gemsParent.GetChild(i);
		}
	}

	private void Update()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Bounds val = default(Bounds);
		((Bounds)(ref val))._002Ector(gems[0].get_position(), Vector3.get_zero());
		for (int i = 1; i < gems.Length; i++)
		{
			((Bounds)(ref val)).Encapsulate(gems[i].get_position());
		}
		Vector3 size = ((Bounds)(ref val)).get_size();
		if (((Vector3)(ref size)).get_sqrMagnitude() <= limit * limit)
		{
			if (!currentlyStacked)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_CLIMB_GEMS);
			}
			currentlyStacked = true;
		}
		else
		{
			currentlyStacked = false;
		}
	}

	public GemsAchievement()
		: this()
	{
	}
}

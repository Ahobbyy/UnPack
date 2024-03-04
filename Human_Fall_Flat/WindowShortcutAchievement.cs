using UnityEngine;

public class WindowShortcutAchievement : MonoBehaviour
{
	private Collider trackedCollider;

	private float entryX;

	public void OnTriggerEnter(Collider other)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)other).get_tag() == "Player")
		{
			Vector3 val = ((Component)this).get_transform().InverseTransformPoint(((Component)other).get_transform().get_position());
			entryX = val.x;
			if (entryX < 0f)
			{
				trackedCollider = other;
			}
		}
	}

	public void OnTriggerExit(Collider other)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)other == (Object)(object)trackedCollider)
		{
			if (((Component)this).get_transform().InverseTransformPoint(((Component)other).get_transform().get_position()).x * entryX < 0f)
			{
				StatsAndAchievements.UnlockAchievement(Achievement.ACH_BREAK_WINDOW_SHORTCUT);
			}
			trackedCollider = null;
		}
	}

	public WindowShortcutAchievement()
		: this()
	{
	}
}

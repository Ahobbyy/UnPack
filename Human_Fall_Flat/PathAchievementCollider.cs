using UnityEngine;

public class PathAchievementCollider : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			((Component)this).GetComponentInParent<PathAchievementBase>().PlayerEnter(this, other);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			((Component)this).GetComponentInParent<PathAchievementBase>().PlayerLeave(this, other);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			((Component)this).GetComponentInParent<PathAchievementBase>().PlayerStay(this, other);
		}
	}

	public PathAchievementCollider()
		: this()
	{
	}
}

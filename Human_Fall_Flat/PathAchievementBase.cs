using UnityEngine;

public abstract class PathAchievementBase : MonoBehaviour
{
	private Collider trackedPlayer;

	public PathAchievementCollider entryCollider;

	public PathAchievementCollider zoneCollider;

	public PathAchievementCollider passCollider;

	public void PlayerEnter(PathAchievementCollider collider, Collider player)
	{
		if ((Object)(object)collider == (Object)(object)entryCollider)
		{
			OnEntry(player);
			trackedPlayer = player;
		}
		if ((Object)(object)collider == (Object)(object)passCollider && (Object)(object)trackedPlayer == (Object)(object)player)
		{
			UnlockAchievement();
			trackedPlayer = null;
		}
	}

	public void PlayerLeave(PathAchievementCollider collider, Collider player)
	{
		if ((Object)(object)collider == (Object)(object)zoneCollider && (Object)(object)trackedPlayer == (Object)(object)player)
		{
			trackedPlayer = null;
		}
	}

	public void PlayerStay(PathAchievementCollider collider, Collider player)
	{
		if ((Object)(object)collider == (Object)(object)zoneCollider && (Object)(object)trackedPlayer == (Object)(object)player && !IsValid(trackedPlayer))
		{
			trackedPlayer = null;
		}
	}

	public abstract void UnlockAchievement();

	protected virtual void OnEntry(Collider trackedPlayer)
	{
	}

	protected virtual bool IsValid(Collider trackedPlayer)
	{
		return true;
	}

	protected PathAchievementBase()
		: this()
	{
	}
}

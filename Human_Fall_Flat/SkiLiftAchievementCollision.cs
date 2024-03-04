using UnityEngine;

public class SkiLiftAchievementCollision : MonoBehaviour
{
	public SkiLiftAchievement achievementTracker;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		Human componentInParent = ((Component)collision.get_rigidbody()).GetComponentInParent<Human>();
		if ((Object)(object)componentInParent != (Object)null)
		{
			achievementTracker.AddHuman(componentInParent);
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		Human componentInParent = ((Component)collision.get_rigidbody()).GetComponentInParent<Human>();
		if ((Object)(object)componentInParent != (Object)null)
		{
			achievementTracker.RemoveHuman(componentInParent);
		}
	}

	public SkiLiftAchievementCollision()
		: this()
	{
	}
}

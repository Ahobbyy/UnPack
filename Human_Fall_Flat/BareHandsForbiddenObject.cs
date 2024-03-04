using UnityEngine;

public class BareHandsForbiddenObject : MonoBehaviour
{
	public void OnCollisionEnter(Collision collision)
	{
		VoronoiShatter component = collision.get_gameObject().GetComponent<VoronoiShatter>();
		if (!((Object)(object)component != (Object)null))
		{
			return;
		}
		for (int i = 0; i < BareHandsAchievement.instance.walls.Length; i++)
		{
			if ((Object)(object)BareHandsAchievement.instance.walls[i] == (Object)(object)component)
			{
				BareHandsAchievement.instance.CancelAchievement();
			}
		}
	}

	public BareHandsForbiddenObject()
		: this()
	{
	}
}

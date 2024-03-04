using UnityEngine;

public class ZoneTransitionTarget : MonoBehaviour
{
	public ZoneTransitionTutorial tutorial;

	public void OnTriggerEnter(Collider other)
	{
		if (!(((Component)other).get_tag() != "Player"))
		{
			tutorial.OnTargetReached();
		}
	}

	public ZoneTransitionTarget()
		: this()
	{
	}
}

using HumanAPI;
using UnityEngine;

public class AmbienceZoneTrigger : LevelObject
{
	public AmbienceMix mix;

	public int priority;

	public float transitionDuration = 3f;

	public void OnTriggerEnter(Collider other)
	{
		if (base.active && ((Component)other).get_gameObject().get_tag() == "Player")
		{
			AmbienceManager.instance.EnterZone(this);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (base.active && ((Component)other).get_gameObject().get_tag() == "Player")
		{
			AmbienceManager.instance.LeaveZone(this);
		}
	}
}

using UnityEngine;

public class NarrativeForceTrigger : MonoBehaviour
{
	public NarrativeBlock narrative;

	public void OnTriggerEnter(Collider other)
	{
		if (!(((Component)other).get_tag() != "Player"))
		{
			narrative.Play();
		}
	}

	public NarrativeForceTrigger()
		: this()
	{
	}
}

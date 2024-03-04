using UnityEngine;

public class RockTutorialNonsenseTrigger : MonoBehaviour
{
	public TutorialBlock mainTutorial;

	public bool playerInside;

	private float leaveTime;

	public void OnTriggerEnter(Collider other)
	{
		if (((Behaviour)mainTutorial).get_isActiveAndEnabled() && !(((Component)other).get_tag() != "Player"))
		{
			playerInside = true;
			mainTutorial.ReportNonsense();
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (((Behaviour)mainTutorial).get_isActiveAndEnabled() && !(((Component)other).get_tag() != "Player"))
		{
			playerInside = false;
			mainTutorial.UnreportNonsense();
		}
	}

	public RockTutorialNonsenseTrigger()
		: this()
	{
	}
}

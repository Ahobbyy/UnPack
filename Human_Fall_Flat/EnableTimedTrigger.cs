using UnityEngine;

public class EnableTimedTrigger : MonoBehaviour, IReset
{
	public GameObject triggerToControl;

	public float enableTime;

	private float enableCountdown;

	public void OnTriggerEnter(Collider other)
	{
		if (((Component)other).get_tag() == "Player")
		{
			triggerToControl.SetActive(true);
			enableCountdown = enableTime;
		}
	}

	private void Update()
	{
		if (enableCountdown > 0f)
		{
			enableCountdown -= Time.get_deltaTime();
			if (enableCountdown <= 0f)
			{
				triggerToControl.SetActive(false);
			}
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		triggerToControl.SetActive(false);
	}

	public EnableTimedTrigger()
		: this()
	{
	}
}

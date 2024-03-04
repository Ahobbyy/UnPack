using UnityEngine;

public class ReactorDoor : MonoBehaviour
{
	public bool initiallyClosed = true;

	public Collider closedTrigger;

	private bool isClosed;

	private void Start()
	{
		isClosed = initiallyClosed;
	}

	public bool IsClosed()
	{
		return isClosed;
	}

	public void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)other == (Object)(object)closedTrigger)
		{
			isClosed = true;
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if ((Object)(object)other == (Object)(object)closedTrigger)
		{
			isClosed = false;
		}
	}

	public ReactorDoor()
		: this()
	{
	}
}

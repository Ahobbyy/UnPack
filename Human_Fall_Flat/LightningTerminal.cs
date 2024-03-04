using UnityEngine;

public class LightningTerminal : MonoBehaviour
{
	private int orbContacts;

	public bool isActive => orbContacts > 0;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((Object)(object)((Component)other).get_gameObject().GetComponent<LightningOrb>() != (Object)null)
		{
			orbContacts++;
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if ((Object)(object)((Component)other).get_gameObject().GetComponent<LightningOrb>() != (Object)null)
		{
			orbContacts--;
		}
	}

	public LightningTerminal()
		: this()
	{
	}
}

using UnityEngine;

public class ActiveInDevBuild : MonoBehaviour
{
	public bool activeInDevBuild = true;

	public void Awake()
	{
		((Component)this).get_gameObject().SetActive(activeInDevBuild && Debug.get_isDebugBuild());
	}

	public ActiveInDevBuild()
		: this()
	{
	}
}

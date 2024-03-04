using UnityEngine;

public class SumoStart : MonoBehaviour
{
	private void Start()
	{
		Object[] array = Resources.LoadAll("");
		foreach (Object val in array)
		{
			Debug.Log((object)("object: " + val.get_name() + " : " + ((object)val).GetType().ToString()));
		}
	}

	public SumoStart()
		: this()
	{
	}
}

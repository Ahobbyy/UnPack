using UnityEngine;

public class HandsScript : MonoBehaviour
{
	public GameObject box1;

	public GameObject box2;

	private void Update()
	{
		if (Game.GetKeyDown((KeyCode)256))
		{
			box1.set_tag("Target");
			box2.set_tag("Target");
		}
	}

	public HandsScript()
		: this()
	{
	}
}

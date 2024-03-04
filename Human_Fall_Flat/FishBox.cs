using UnityEngine;

public class FishBox : MonoBehaviour
{
	public bool hasFishOnIt;

	private void OnCollisionStay(Collision collision)
	{
		if (((Object)collision.get_collider()).get_name() == "FishMesh")
		{
			hasFishOnIt = true;
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (((Object)collision.get_collider()).get_name() == "FishMesh")
		{
			hasFishOnIt = true;
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (((Object)collision.get_collider()).get_name() == "FishMesh")
		{
			hasFishOnIt = false;
		}
	}

	private void Update()
	{
		if (hasFishOnIt)
		{
			Debug.Log((object)("<color=green> hasFishOnIt = </color>" + hasFishOnIt));
		}
	}

	public FishBox()
		: this()
	{
	}
}

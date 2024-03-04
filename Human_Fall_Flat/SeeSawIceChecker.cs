using HumanAPI.LightLevel;
using UnityEngine;

public class SeeSawIceChecker : MonoBehaviour
{
	private void Start()
	{
	}

	private void Update()
	{
	}

	private void OnCollisionEnter(Collision collision)
	{
		if ((Object)(object)SeeSawAchievement.instance != (Object)null && (Object)(object)collision.get_gameObject().GetComponentInParent<MeltingObject>() != (Object)null)
		{
			SeeSawAchievement.instance.Fail();
		}
	}

	public SeeSawIceChecker()
		: this()
	{
	}
}

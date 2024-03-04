using UnityEngine;

public class ReleaseGrabTrigger : MonoBehaviour
{
	public void OnTriggerEnter(Collider other)
	{
		CollisionSensor component = ((Component)other).GetComponent<CollisionSensor>();
		if ((Object)(object)component != (Object)null)
		{
			component.BlockGrab(this);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		CollisionSensor component = ((Component)other).GetComponent<CollisionSensor>();
		if ((Object)(object)component != (Object)null)
		{
			component.UnblockBlockGrab();
		}
	}

	public ReleaseGrabTrigger()
		: this()
	{
	}
}

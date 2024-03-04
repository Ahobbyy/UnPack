using HumanAPI;
using UnityEngine;

public class MagneticCrane : Node, IReset
{
	public NodeInput activateMagnet;

	private bool magnetActive;

	private Joint fixJoint;

	public override void Process()
	{
		magnetActive = Mathf.Abs(activateMagnet.value) >= 0.5f;
		if (!magnetActive && (Object)(object)fixJoint != (Object)null)
		{
			Object.Destroy((Object)(object)fixJoint);
		}
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (magnetActive && (Object)(object)fixJoint == (Object)null && (Object)(object)collision.get_gameObject().GetComponent<MagneticCraneTarget>() != (Object)null)
		{
			collision.get_gameObject().GetComponent<Rigidbody>().set_isKinematic(false);
			fixJoint = (Joint)(object)collision.get_gameObject().AddComponent<FixedJoint>();
			fixJoint.set_connectedBody(((Component)this).GetComponent<Rigidbody>());
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		if ((Object)(object)fixJoint != (Object)null)
		{
			Object.Destroy((Object)(object)fixJoint);
		}
	}
}

using UnityEngine;

public class FixFixedJoint : MonoBehaviour
{
	public void ReAddJoint()
	{
		if ((Object)(object)((Component)this).GetComponent<FixedJoint>() == (Object)null)
		{
			FixedJoint obj = ((Component)this).get_gameObject().AddComponent<FixedJoint>();
			((Joint)obj).set_breakForce(float.PositiveInfinity);
			((Joint)obj).set_breakTorque(float.PositiveInfinity);
			((Joint)obj).set_enableCollision(false);
			((Joint)obj).set_massScale(1f);
			((Joint)obj).set_connectedMassScale(1f);
			((Joint)obj).set_enablePreprocessing(true);
		}
	}

	public void MakeJointBreakable()
	{
		FixedJoint component = ((Component)this).GetComponent<FixedJoint>();
		if ((Object)(object)component != (Object)null)
		{
			((Joint)component).set_breakForce(0f);
			((Joint)component).set_breakTorque(0f);
		}
	}

	public FixFixedJoint()
		: this()
	{
	}
}

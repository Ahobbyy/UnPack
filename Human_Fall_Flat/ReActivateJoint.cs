using UnityEngine;

public class ReActivateJoint : MonoBehaviour, IReset
{
	[SerializeField]
	private GameObject iceBlock;

	[SerializeField]
	private GameObject torch;

	void IReset.ResetState(int checkpoint, int subObjectives)
	{
		ConfigurableJoint component = ((Component)this).GetComponent<ConfigurableJoint>();
		if (Object.op_Implicit((Object)(object)component))
		{
			Object.Destroy((Object)(object)component);
		}
		component = torch.AddComponent<ConfigurableJoint>();
		((Joint)component).set_connectedBody(iceBlock.GetComponent<Rigidbody>());
		component.set_xMotion((ConfigurableJointMotion)0);
		component.set_yMotion((ConfigurableJointMotion)0);
		component.set_zMotion((ConfigurableJointMotion)0);
		component.set_angularXMotion((ConfigurableJointMotion)0);
		component.set_angularYMotion((ConfigurableJointMotion)0);
		component.set_angularZMotion((ConfigurableJointMotion)0);
		((Joint)component).set_massScale(1.4f);
	}

	public ReActivateJoint()
		: this()
	{
	}
}

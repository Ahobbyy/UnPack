using HumanAPI;
using UnityEngine;

public class ChainReset : MonoBehaviour, IPostReset
{
	public Rigidbody connectedTo;

	private ConfigurableJoint currentJoint;

	private Vector3 originalAnchor;

	private SoftJointLimit originalZLimit;

	private void Awake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		currentJoint = ((Component)this).GetComponent<ConfigurableJoint>();
		originalAnchor = ((Joint)currentJoint).get_connectedAnchor();
		originalZLimit = currentJoint.get_angularZLimit();
	}

	void IPostReset.PostResetState(int checkpoint)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		Object.DestroyImmediate((Object)(object)currentJoint);
		currentJoint = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
		((Joint)currentJoint).set_autoConfigureConnectedAnchor(false);
		((Joint)currentJoint).set_connectedAnchor(originalAnchor);
		((Joint)currentJoint).set_connectedBody(connectedTo);
		currentJoint.set_xMotion((ConfigurableJointMotion)0);
		currentJoint.set_yMotion((ConfigurableJointMotion)0);
		currentJoint.set_zMotion((ConfigurableJointMotion)0);
		currentJoint.set_angularXMotion((ConfigurableJointMotion)2);
		currentJoint.set_angularYMotion((ConfigurableJointMotion)2);
		currentJoint.set_angularZMotion((ConfigurableJointMotion)1);
		currentJoint.set_angularZLimit(originalZLimit);
	}

	public ChainReset()
		: this()
	{
	}
}

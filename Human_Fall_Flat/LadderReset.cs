using HumanAPI;
using UnityEngine;

public sealed class LadderReset : MonoBehaviour, IPostReset
{
	public class JointList
	{
		public GameObject jointObject;

		public Rigidbody connectedRigidBody;

		public float breakForce;

		public JointList(GameObject jointObject, Rigidbody connectedRigidBody, float breakForce)
		{
			this.jointObject = jointObject;
			this.connectedRigidBody = connectedRigidBody;
			this.breakForce = breakForce;
		}
	}

	private JointList[] jointLists;

	private void Awake()
	{
		FixedJoint[] componentsInChildren = ((Component)this).GetComponentsInChildren<FixedJoint>();
		jointLists = new JointList[componentsInChildren.Length];
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			FixedJoint val = componentsInChildren[i];
			jointLists[i] = new JointList(((Component)val).get_gameObject(), ((Joint)val).get_connectedBody(), ((Joint)val).get_breakForce());
		}
	}

	void IPostReset.PostResetState(int checkpoint)
	{
		JointList[] array = jointLists;
		foreach (JointList jointList in array)
		{
			if ((Object)(object)jointList.jointObject.GetComponent<FixedJoint>() == (Object)null)
			{
				FixedJoint obj = jointList.jointObject.AddComponent<FixedJoint>();
				((Joint)obj).set_connectedBody(jointList.connectedRigidBody);
				((Joint)obj).set_breakForce(jointList.breakForce);
			}
		}
	}

	public LadderReset()
		: this()
	{
	}
}

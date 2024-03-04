using HumanAPI;
using UnityEngine;

public sealed class SnowManReset : MonoBehaviour, IPostEndReset
{
	public sealed class JointStoreData
	{
		public Rigidbody connectedBody;

		public Vector3 scale;

		public float breakForce;

		public float breakTorque;

		public float myMass;
	}

	private JointStoreData[] jointStore;

	private void Awake()
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		int childCount = ((Component)this).get_transform().get_childCount();
		jointStore = new JointStoreData[childCount];
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = ((Component)((Component)this).get_transform().GetChild(i)).get_gameObject();
			FixedJoint component = gameObject.GetComponent<FixedJoint>();
			JointStoreData jointStoreData = new JointStoreData();
			if ((Object)(object)component != (Object)null)
			{
				jointStoreData.connectedBody = ((Joint)component).get_connectedBody();
				jointStoreData.breakForce = ((Joint)component).get_breakForce();
				jointStoreData.breakTorque = ((Joint)component).get_breakTorque();
			}
			jointStoreData.scale = ((Component)component).get_transform().get_localScale();
			Rigidbody component2 = gameObject.GetComponent<Rigidbody>();
			jointStoreData.myMass = component2.get_mass();
			jointStore[i] = jointStoreData;
		}
	}

	void IPostEndReset.PostEndResetState(int checkpoint)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		int childCount = ((Component)this).get_transform().get_childCount();
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = ((Component)((Component)this).get_transform().GetChild(i)).get_gameObject();
			if ((Object)(object)gameObject.GetComponent<FixedJoint>() == (Object)null)
			{
				FixedJoint obj = gameObject.AddComponent<FixedJoint>();
				((Joint)obj).set_connectedBody(jointStore[i].connectedBody);
				((Joint)obj).set_breakForce(jointStore[i].breakForce);
				((Joint)obj).set_breakTorque(jointStore[i].breakTorque);
			}
			gameObject.get_transform().set_localScale(jointStore[i].scale);
			gameObject.GetComponent<Rigidbody>().set_mass(jointStore[i].myMass);
		}
	}

	public SnowManReset()
		: this()
	{
	}
}

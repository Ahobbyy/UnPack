using HumanAPI;
using UnityEngine;

public class ResetChainLink : MonoBehaviour, IPostReset
{
	[SerializeField]
	private GameObject connectedBelow;

	private Rigidbody connectedBody;

	private ConfigurableJointMotion xMotion;

	private ConfigurableJointMotion yMotion;

	private ConfigurableJointMotion zMotion;

	private SoftJointLimit linearLimit;

	private SoftJointLimit lowAngularXLimit;

	private SoftJointLimit highAngularXLimit;

	private SoftJointLimit angularYLimit;

	private SoftJointLimit angularZLimit;

	private JointDrive angularXDrive;

	private JointDrive angularYZDrive;

	private JointDrive slerpDrive;

	private float breakForce;

	private float breakTorque;

	private bool noJoint;

	private void Awake()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		ConfigurableJoint component = ((Component)this).GetComponent<ConfigurableJoint>();
		if (Object.op_Implicit((Object)(object)component))
		{
			connectedBody = ((Joint)component).get_connectedBody();
			xMotion = component.get_xMotion();
			yMotion = component.get_yMotion();
			zMotion = component.get_zMotion();
			lowAngularXLimit = component.get_lowAngularXLimit();
			highAngularXLimit = component.get_highAngularXLimit();
			angularYLimit = component.get_angularYLimit();
			angularZLimit = component.get_angularZLimit();
			angularXDrive = component.get_angularXDrive();
			angularYZDrive = component.get_angularYZDrive();
			slerpDrive = component.get_slerpDrive();
			linearLimit = component.get_linearLimit();
			breakForce = ((Joint)component).get_breakForce();
			breakTorque = ((Joint)component).get_breakTorque();
			if ((Object)(object)connectedBelow != (Object)null)
			{
				component = connectedBelow.GetComponent<ConfigurableJoint>();
				if ((Object)(object)component != (Object)null)
				{
					Rigidbody component2 = ((Component)this).GetComponent<Rigidbody>();
					((Joint)component).set_connectedBody(component2);
				}
			}
		}
		else
		{
			noJoint = true;
		}
	}

	void IPostReset.PostResetState(int checkpoint)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (!noJoint && (Object)(object)((Component)this).GetComponent<ConfigurableJoint>() == (Object)null)
		{
			ConfigurableJoint obj = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
			((Joint)obj).set_connectedBody(connectedBody);
			((Joint)obj).set_anchor(Vector3.get_zero());
			obj.set_xMotion(xMotion);
			obj.set_yMotion(yMotion);
			obj.set_zMotion(zMotion);
			obj.set_linearLimit(linearLimit);
			obj.set_lowAngularXLimit(lowAngularXLimit);
			obj.set_highAngularXLimit(highAngularXLimit);
			obj.set_angularYLimit(angularYLimit);
			obj.set_angularZLimit(angularZLimit);
			obj.set_angularXDrive(angularXDrive);
			obj.set_angularYZDrive(angularYZDrive);
			obj.set_slerpDrive(slerpDrive);
			((Joint)obj).set_breakForce(breakForce);
			((Joint)obj).set_breakTorque(breakTorque);
		}
	}

	public ResetChainLink()
		: this()
	{
	}
}

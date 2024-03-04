using UnityEngine;

public class StringJoint : MonoBehaviour
{
	public Rigidbody connectedBody;

	public Vector3 anchor;

	public bool autoConfigureConnectedAnchor;

	public Vector3 connectedAnchor;

	public bool autoconfigureStringLength;

	public float stringLength;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).get_transform().TransformPoint(anchor);
		if (autoConfigureConnectedAnchor)
		{
			connectedAnchor = ((Component)connectedBody).get_transform().InverseTransformPoint(val);
		}
		Vector3 val2 = ((Component)connectedBody).get_transform().TransformPoint(connectedAnchor);
		if (autoconfigureStringLength)
		{
			Vector3 val3 = val - val2;
			stringLength = ((Vector3)(ref val3)).get_magnitude();
		}
		ConfigurableJoint obj = ((Component)this).get_gameObject().AddComponent<ConfigurableJoint>();
		((Joint)obj).set_anchor(anchor);
		((Joint)obj).set_autoConfigureConnectedAnchor(false);
		((Joint)obj).set_connectedBody(connectedBody);
		((Joint)obj).set_connectedAnchor(connectedAnchor);
		SoftJointLimit val4 = default(SoftJointLimit);
		((SoftJointLimit)(ref val4)).set_limit(stringLength);
		SoftJointLimit linearLimit = val4;
		obj.set_linearLimit(linearLimit);
		ConfigurableJointMotion val5 = (ConfigurableJointMotion)1;
		obj.set_zMotion((ConfigurableJointMotion)1);
		ConfigurableJointMotion xMotion;
		obj.set_yMotion(xMotion = val5);
		obj.set_xMotion(xMotion);
		val5 = (ConfigurableJointMotion)2;
		obj.set_angularZMotion((ConfigurableJointMotion)2);
		obj.set_angularYMotion(xMotion = val5);
		obj.set_angularXMotion(xMotion);
	}

	private void OnDrawGizmosSelected()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_red());
		Gizmos.DrawCube(((Component)this).get_transform().TransformPoint(anchor), Vector3.get_one() * 0.2f);
		Gizmos.set_color(Color.get_red());
		Gizmos.DrawCube(((Component)connectedBody).get_transform().TransformPoint(connectedAnchor), Vector3.get_one() * 0.2f);
	}

	public StringJoint()
		: this()
	{
	}
}

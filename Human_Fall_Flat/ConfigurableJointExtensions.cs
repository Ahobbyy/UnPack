using UnityEngine;

public static class ConfigurableJointExtensions
{
	public static void SetXMotionAnchorsAndLimits(this ConfigurableJoint joint, float centerOffset, float actionRange)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		SoftJointLimit linearLimit = joint.get_linearLimit();
		((SoftJointLimit)(ref linearLimit)).set_limit(actionRange / 2f);
		joint.set_linearLimit(linearLimit);
		joint.set_xMotion((ConfigurableJointMotion)1);
		((Joint)joint).set_autoConfigureConnectedAnchor(false);
		Vector3 val = ((Component)joint).get_transform().TransformPoint(((Joint)joint).get_anchor() + ((Joint)joint).get_axis() * centerOffset);
		if ((Object)(object)((Joint)joint).get_connectedBody() != (Object)null)
		{
			((Joint)joint).set_connectedAnchor(((Component)((Joint)joint).get_connectedBody()).get_transform().InverseTransformPoint(val));
		}
		else
		{
			((Joint)joint).set_connectedAnchor(val);
		}
	}

	public static void ApplyXMotionTarget(this ConfigurableJoint joint)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((!((Object)(object)((Joint)joint).get_connectedBody() != (Object)null)) ? ((Joint)joint).get_connectedAnchor() : ((Component)((Joint)joint).get_connectedBody()).get_transform().TransformPoint(((Joint)joint).get_connectedAnchor()));
		SoftJointLimit linearLimit = joint.get_linearLimit();
		float limit = ((SoftJointLimit)(ref linearLimit)).get_limit();
		float num = joint.get_targetPosition().x;
		if (limit > 0f)
		{
			num = Mathf.Clamp(num, 0f - limit, limit);
		}
		Vector3 val2 = ((Joint)joint).get_anchor() + ((Joint)joint).get_axis() * num;
		Vector3 val3 = ((Component)joint).get_transform().TransformPoint(val2);
		Transform transform = ((Component)joint).get_transform();
		transform.set_position(transform.get_position() + (val - val3));
		Rigidbody component = ((Component)joint).GetComponent<Rigidbody>();
		Vector3 zero;
		component.set_velocity(zero = Vector3.get_zero());
		component.set_angularVelocity(zero);
	}

	public static float GetXAngle(this Joint joint, Quaternion invInitialLocalRotation)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = invInitialLocalRotation * ((Component)joint).get_transform().get_localRotation();
		float num = default(float);
		Vector3 val2 = default(Vector3);
		((Quaternion)(ref val)).ToAngleAxis(ref num, ref val2);
		if (Vector3.Dot(val2, joint.get_axis()) < 0f)
		{
			return 0f - num;
		}
		return num;
	}

	public static void SetXAngleTarget(this ConfigurableJoint joint, float angle)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Quaternion val = Quaternion.AngleAxis(0f - angle, Vector3.get_right());
		if (joint.get_targetRotation() != val)
		{
			joint.set_targetRotation(val);
		}
	}

	public static void ApplyXAngle(this Joint joint, Quaternion invInitialLocalRotation, float angle)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		((Component)joint).get_transform().set_localRotation(Quaternion.Inverse(invInitialLocalRotation) * Quaternion.AngleAxis(angle, joint.get_axis()));
		Rigidbody component = ((Component)joint).GetComponent<Rigidbody>();
		Vector3 zero;
		component.set_velocity(zero = Vector3.get_zero());
		component.set_angularVelocity(zero);
	}

	public static Quaternion ReadInitialRotation(this Joint joint)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		return Quaternion.Inverse(((Component)joint).get_transform().get_localRotation());
	}
}

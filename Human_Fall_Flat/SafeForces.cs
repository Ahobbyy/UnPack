using UnityEngine;

public static class SafeForces
{
	public static void SafeAddForce(this Rigidbody body, Vector3 force, ForceMode mode = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (float.IsNaN(force.x) || float.IsNaN(force.y) || float.IsNaN(force.z) || ((Vector3)(ref force)).get_sqrMagnitude() > 2.5E+11f)
		{
			Vector3 val = force;
			Debug.Log((object)("invalid force " + ((object)(Vector3)(ref val)).ToString() + " for " + ((Object)body).get_name()));
		}
		else
		{
			body.AddForce(force, mode);
		}
	}

	public static void SafeAddForceAtPosition(this Rigidbody body, Vector3 force, Vector3 position, ForceMode mode = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val;
		if (float.IsNaN(force.x) || float.IsNaN(force.y) || float.IsNaN(force.z) || ((Vector3)(ref force)).get_sqrMagnitude() > 2.5E+11f)
		{
			val = force;
			Debug.Log((object)("invalid force " + ((object)(Vector3)(ref val)).ToString() + " for " + ((Object)body).get_name()));
		}
		else if (position.x == float.NaN || position.y == float.NaN || position.z == float.NaN || ((Vector3)(ref position)).get_sqrMagnitude() > 2.5E+11f)
		{
			val = position;
			Debug.Log((object)("invalid position " + ((object)(Vector3)(ref val)).ToString() + " for " + ((Object)body).get_name()));
		}
		else
		{
			body.AddForceAtPosition(force, position, mode);
		}
	}

	public static void SafeAddTorque(this Rigidbody body, Vector3 torque, ForceMode mode = 0)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		if (float.IsNaN(torque.x) || float.IsNaN(torque.y) || float.IsNaN(torque.z) || ((Vector3)(ref torque)).get_sqrMagnitude() > 1E+10f)
		{
			Vector3 val = torque;
			Debug.Log((object)("invalid torque " + ((object)(Vector3)(ref val)).ToString() + " for " + ((Object)body).get_name()));
		}
		else
		{
			body.AddTorque(torque, mode);
		}
	}
}

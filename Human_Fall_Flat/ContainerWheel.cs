using UnityEngine;

public class ContainerWheel : MonoBehaviour
{
	public float staticFriction = 0.2f;

	public float rollingFriction = 0.2f;

	public float connectedMass;

	public float treshold = 1f;

	public Transform mesh;

	public Vector3 upAxis;

	public Vector3 forwardAxis;

	public Rigidbody body;

	private void OnEnable()
	{
		body = ((Component)this).GetComponentInParent<Rigidbody>();
		body.set_maxAngularVelocity(50f);
	}

	public void FixedUpdate()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (rollingFriction != 0f || staticFriction != 0f)
		{
			Vector3 pointVelocity = body.GetPointVelocity(((Component)this).get_transform().get_position());
			float magnitude = ((Vector3)(ref pointVelocity)).get_magnitude();
			if (!(magnitude < 0.01f))
			{
				float num = ((magnitude > treshold) ? rollingFriction : staticFriction);
				float num2 = magnitude * connectedMass / Time.get_fixedDeltaTime();
				body.SafeAddForceAtPosition(-((Vector3)(ref pointVelocity)).get_normalized() * Mathf.Min(num2, num * connectedMass), ((Component)this).get_transform().get_position(), (ForceMode)0);
				Debug.DrawRay(((Component)this).get_transform().get_position(), -((Vector3)(ref pointVelocity)).get_normalized() * Mathf.Min(num2, num * connectedMass) / 100f, Color.get_red());
			}
		}
	}

	private void LateUpdate()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)mesh == (Object)null) && !body.IsSleeping())
		{
			Vector3 val = mesh.get_parent().InverseTransformVector(body.GetPointVelocity(((Component)this).get_transform().get_position()));
			val -= Vector3.Project(val, upAxis);
			Quaternion val2 = Quaternion.FromToRotation(forwardAxis, val);
			Quaternion localRotation = Quaternion.Lerp(mesh.get_localRotation(), val2, ((Vector3)(ref val)).get_magnitude() / 10f);
			mesh.set_localRotation(localRotation);
		}
	}

	public ContainerWheel()
		: this()
	{
	}
}

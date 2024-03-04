using UnityEngine;

namespace HumanAPI
{
	public class SignalAngularVelocity : Node
	{
		public NodeOutput value;

		public float fromVelocity;

		public float fromDeadVelocity;

		public float toDeadVelocity;

		public float toVelocity;

		public Vector3 axis;

		private Vector3 parentAxis;

		public Transform relativeTo;

		private Rigidbody relativeBody;

		public Rigidbody body;

		[ReadOnly]
		public float velocity;

		private Quaternion oldRotation;

		protected override void OnEnable()
		{
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			parentAxis = ((Component)this).get_transform().TransformDirection(axis);
			oldRotation = body.get_rotation();
			if ((Object)(object)relativeTo != (Object)null)
			{
				oldRotation = Quaternion.Inverse(relativeTo.get_rotation()) * oldRotation;
				parentAxis = relativeTo.InverseTransformDirection(parentAxis);
				relativeBody = ((Component)relativeTo).GetComponent<Rigidbody>();
			}
			base.OnEnable();
		}

		private void FixedUpdate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			Quaternion val = body.get_rotation();
			float num = 0f;
			if ((Object)(object)relativeTo != (Object)null)
			{
				val = Quaternion.Inverse(relativeTo.get_rotation()) * val;
				if ((Object)(object)relativeBody != (Object)null)
				{
					velocity = Vector3.Dot(relativeTo.InverseTransformVector(body.get_angularVelocity()) - relativeBody.get_angularVelocity(), parentAxis) * 57.29578f;
				}
				else
				{
					velocity = Vector3.Dot(relativeTo.InverseTransformVector(body.get_angularVelocity()), parentAxis) * 57.29578f;
				}
			}
			else
			{
				velocity = Vector3.Dot(body.get_angularVelocity(), parentAxis) * 57.29578f;
			}
			velocity = Quaternion.Angle(val, oldRotation) / Time.get_fixedDeltaTime();
			oldRotation = val;
			if (velocity < fromDeadVelocity)
			{
				num = 0f - Mathf.InverseLerp(fromDeadVelocity, fromVelocity, velocity);
			}
			if (velocity > toDeadVelocity)
			{
				num = Mathf.InverseLerp(toDeadVelocity, toVelocity, velocity);
			}
			value.SetValue(num);
		}
	}
}

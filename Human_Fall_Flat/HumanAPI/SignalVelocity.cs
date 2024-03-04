using UnityEngine;

namespace HumanAPI
{
	public class SignalVelocity : Node
	{
		[Tooltip("The value output by this node in line with the bodies velocity")]
		public NodeOutput value;

		[Tooltip("Velocity value when coming to a dead stop")]
		public float toDeadVelocity;

		[Tooltip("Velocity value when coming to top speed")]
		public float toVelocity;

		[Tooltip("Body to which the relative velocity will be calculated , if null then world")]
		public Rigidbody relativeBody;

		[Tooltip("Body which the velocity should come from , if null then self")]
		public Rigidbody body;

		[Tooltip("Velocity Var")]
		[ReadOnly]
		public float velocity;

		private Vector3 directionalVelocity;

		[Tooltip("Whether or not to report direction as part of the velocity")]
		public bool directional;

		public bool xDirection;

		public bool yDirection;

		public bool zDirection;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		protected override void OnEnable()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enable "));
			}
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			base.OnEnable();
		}

		private void FixedUpdate()
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			Vector3 val;
			if ((Object)(object)relativeBody != (Object)null)
			{
				val = body.get_velocity() - relativeBody.get_velocity();
				velocity = ((Vector3)(ref val)).get_magnitude();
			}
			else
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Relative Body = null "));
				}
				val = body.get_velocity();
				velocity = ((Vector3)(ref val)).get_magnitude();
				directionalVelocity = body.get_velocity();
			}
			if (velocity > toDeadVelocity)
			{
				num = Mathf.InverseLerp(toDeadVelocity, toVelocity, velocity);
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " velocity > toDeadVelocity "));
					Debug.Log((object)(((Object)this).get_name() + " value = " + num));
					string name = ((Object)this).get_name();
					val = directionalVelocity;
					Debug.Log((object)(name + " directionalVelocity = " + ((object)(Vector3)(ref val)).ToString()));
				}
			}
			if (xDirection && directional)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Use Velocity X Vector Value "));
				}
				num = directionalVelocity.x;
			}
			if (yDirection && directional)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Use Velocity Y Vector Value "));
				}
				num = directionalVelocity.y;
			}
			if (zDirection && directional)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Use Velocity Z Vector Value "));
				}
				num = directionalVelocity.z;
			}
			value.SetValue(num);
		}
	}
}

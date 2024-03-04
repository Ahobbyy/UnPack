using System;
using UnityEngine;

namespace HumanAPI
{
	public class SignalSetAngularVelocity : Node
	{
		[Tooltip("Input for this node ")]
		public NodeInput input;

		[Tooltip("Max Force doesnt appear to be used")]
		public float maxForce = 1f;

		[Tooltip("Used when working out the target velocity")]
		public float maxVelocity = 1f;

		[Tooltip("Used in fixed update to the angular velocity")]
		public bool infiniteTorque;

		[Tooltip("Used in fixed update to the angular velocity")]
		public bool freeSpin;

		[Tooltip("Whether or not the thing should move without a power source")]
		public bool Powered;

		[Tooltip("Var for checking the change in the incoming graph node setting")]
		public SignalBase triggerSignal;

		[Tooltip("Reference to the joint being tracked")]
		public HingeJoint joint;

		[Tooltip("The body object to spin in connection to the motor of the body")]
		public Rigidbody body;

		[Tooltip("Use this to show the prints coming from the script")]
		public bool showDebug;

		protected override void OnEnable()
		{
			base.OnEnable();
			if ((Object)(object)joint == (Object)null)
			{
				joint = ((Component)this).GetComponent<HingeJoint>();
			}
			if ((Object)(object)body == (Object)null)
			{
				body = ((Component)this).GetComponent<Rigidbody>();
			}
			if ((Object)(object)body != (Object)null)
			{
				body.set_maxAngularVelocity(100f);
				if ((Object)(object)triggerSignal != (Object)null)
				{
					triggerSignal.onValueChanged += SignalChanged;
					SignalChanged(triggerSignal.value);
				}
				else if (Powered)
				{
					SignalChanged(1f);
				}
			}
			else if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " There is nothing set within the body var "));
			}
		}

		protected override void OnDisable()
		{
			base.OnDisable();
			if ((Object)(object)triggerSignal != (Object)null)
			{
				triggerSignal.onValueChanged -= SignalChanged;
			}
		}

		private void SignalChanged(float val)
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)joint != (Object)null)
			{
				float num = maxForce * (freeSpin ? Mathf.Abs(val) : 1f);
				joint.set_useMotor(num > 0f);
				HingeJoint obj = joint;
				JointMotor motor = default(JointMotor);
				((JointMotor)(ref motor)).set_force(num);
				((JointMotor)(ref motor)).set_targetVelocity(maxVelocity * val);
				obj.set_motor(motor);
			}
			else if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " There is nothing set within the joint var "));
			}
		}

		public override void Process()
		{
			base.Process();
			SignalChanged(input.value);
		}

		private void FixedUpdate()
		{
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			Vector3 axis;
			if ((Object)(object)triggerSignal != (Object)null)
			{
				if (infiniteTorque && (triggerSignal.value != 0f || !freeSpin))
				{
					Rigidbody obj = body;
					Transform transform = ((Component)this).get_transform();
					axis = ((Joint)joint).get_axis();
					obj.set_angularVelocity(transform.TransformDirection(((Vector3)(ref axis)).get_normalized()) * maxVelocity * ((float)Math.PI / 180f) * triggerSignal.value);
				}
			}
			else if (infiniteTorque && (input.value != 0f || !freeSpin))
			{
				Rigidbody obj2 = body;
				Transform transform2 = ((Component)this).get_transform();
				axis = ((Joint)joint).get_axis();
				obj2.set_angularVelocity(transform2.TransformDirection(((Vector3)(ref axis)).get_normalized()) * maxVelocity * ((float)Math.PI / 180f) * input.value);
			}
		}
	}
}

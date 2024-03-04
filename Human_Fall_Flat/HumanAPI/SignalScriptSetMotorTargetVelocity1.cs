using UnityEngine;

namespace HumanAPI
{
	public class SignalScriptSetMotorTargetVelocity1 : Node
	{
		[Tooltip("The incoming value to do something with")]
		public NodeInput input;

		[Tooltip("The amount to multiply the incoming for in line with the motor velocity")]
		public float multiplier = 1f;

		private HingeJoint localHinge;

		private JointMotor localMotor;

		private float incomingValue;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		private void Start()
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Starting "));
			}
			localHinge = ((Component)this).GetComponent<HingeJoint>();
			localMotor = localHinge.get_motor();
		}

		public override void Process()
		{
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			incomingValue = input.value;
			if (incomingValue == 0f)
			{
				incomingValue = -1f;
			}
			incomingValue *= multiplier;
			((JointMotor)(ref localMotor)).set_targetVelocity(incomingValue);
			localHinge.set_motor(localMotor);
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Changing Velocity to " + incomingValue));
			}
		}

		private void Update()
		{
		}
	}
}

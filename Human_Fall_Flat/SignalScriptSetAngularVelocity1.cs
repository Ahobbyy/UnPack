using HumanAPI;
using UnityEngine;

public class SignalScriptSetAngularVelocity1 : Node
{
	[Tooltip("Input for this node ")]
	public NodeInput input;

	[Tooltip("Max Force doesnt appear to be used")]
	public float maxForce = 1f;

	[Tooltip("Reference to the joint being tracked")]
	public HingeJoint joint;

	[Tooltip("The body object to spin in connection to the motor of the body")]
	public Rigidbody body;

	[Tooltip("This allows the amount of rotation to be increased from the input")]
	public float targetVelocityMultipier = 1f;

	private float incomingSignal;

	[Tooltip("Use this to invett the rotation")]
	public bool inverse;

	[Tooltip("Use this to show the prints coming from the script")]
	public bool showDebug;

	public override void Process()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		incomingSignal = input.value;
		incomingSignal *= targetVelocityMultipier;
		JointMotor motor = joint.get_motor();
		((JointMotor)(ref motor)).set_force(maxForce);
		if (incomingSignal > 0f)
		{
			((JointMotor)(ref motor)).set_targetVelocity(incomingSignal);
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Positive Direction "));
				Debug.Log((object)(((Object)this).get_name() + " incomingSignal = " + incomingSignal));
			}
		}
		if (incomingSignal < 0f)
		{
			((JointMotor)(ref motor)).set_targetVelocity(incomingSignal);
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Negative Direction "));
				Debug.Log((object)(((Object)this).get_name() + " incomingSignal = " + incomingSignal));
			}
		}
		joint.set_motor(motor);
	}
}

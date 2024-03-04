using HumanAPI;
using UnityEngine;

public class SignalBroadcastAngularVelocity : Node
{
	[Tooltip("The output of this node")]
	public NodeOutput output;

	[Tooltip("Top velocity of this element")]
	public float fromVelocity;

	[Tooltip("The Lowest Velocity of this element")]
	public float fromDeadVelocity;

	[Tooltip("Rate at which to change to the dead velocity")]
	public float toDeadVelocity;

	[Tooltip("Rate at which to change to the top velocity of this element")]
	public float toVelocity;

	[Tooltip("Axis on which to move")]
	public Vector3 axis;

	[Tooltip("Use this in order to show the prints coming from the script")]
	public bool showDebug;

	private Vector3 worldAxis;

	[Tooltip("Reference to the thing we are dealing with")]
	public Rigidbody body;

	protected override void OnEnable()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body == (Object)null)
		{
			body = ((Component)this).GetComponent<Rigidbody>();
		}
		worldAxis = ((Component)this).get_transform().TransformDirection(axis);
		base.OnEnable();
	}

	private void FixedUpdate()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body != (Object)null)
		{
			float value = 0f;
			float num = Vector3.Dot(body.get_angularVelocity(), worldAxis) * 57.29578f;
			if (num < fromDeadVelocity)
			{
				value = 0f - Mathf.InverseLerp(fromDeadVelocity, fromVelocity, num);
			}
			if (num > toDeadVelocity)
			{
				value = Mathf.InverseLerp(toDeadVelocity, toVelocity, num);
			}
			output.SetValue(value);
		}
		else if (showDebug)
		{
			Debug.Log((object)(((Object)this).get_name() + " There is nothing set within the body var "));
		}
	}
}

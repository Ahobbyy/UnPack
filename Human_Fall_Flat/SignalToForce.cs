using HumanAPI;
using UnityEngine;

public class SignalToForce : Node, IReset
{
	public NodeInput input;

	public bool oneShot = true;

	private bool appliedForce;

	public Rigidbody body;

	public Vector3 forcePosition;

	public Vector3 forceToApply;

	private void Start()
	{
	}

	private void Update()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body != (Object)null && (double)Mathf.Abs(input.value) >= 0.5 && (!oneShot || !appliedForce))
		{
			body.AddForceAtPosition(forceToApply, forcePosition);
			appliedForce = true;
		}
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		appliedForce = false;
	}
}

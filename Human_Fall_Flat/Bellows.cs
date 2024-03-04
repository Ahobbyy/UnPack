using HumanAPI;
using UnityEngine;

public class Bellows : Node
{
	public NodeOutput output;

	public float velocityOutputScale;

	public Rigidbody topBellow;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void FixedUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		output.SetValue(Mathf.Max(topBellow.get_angularVelocity().x * velocityOutputScale, 0f));
	}
}

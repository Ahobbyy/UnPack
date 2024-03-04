using HumanAPI;
using UnityEngine;

public class BreakOnDisplacement : Node
{
	public float breakDiscplacement = 0.5f;

	public NodeInput startChecking;

	public NodeOutput broken;

	private bool areWeBroken;

	[Tooltip("Use ths in order to print debug info to the Log")]
	public bool showDebug;

	private Vector3 startingPos;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		startingPos = ((Component)this).GetComponent<Rigidbody>().get_position();
	}

	private void Update()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (!areWeBroken)
		{
			return;
		}
		Vector3 val = ((Component)this).GetComponent<Rigidbody>().get_position() - startingPos;
		if (((Vector3)(ref val)).get_magnitude() > breakDiscplacement)
		{
			Joint component = ((Component)this).GetComponent<Joint>();
			if ((Object)(object)component != (Object)null)
			{
				Object.Destroy((Object)(object)component);
			}
			broken.SetValue(1f);
		}
	}

	public override void Process()
	{
		if (startChecking.value > 0.5f)
		{
			areWeBroken = true;
		}
	}
}

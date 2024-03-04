using UnityEngine;

public class SignalOnDisplace : SignalBase
{
	public float breakDiscplacement = 0.5f;

	private Vector3 startingPos;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		startingPos = ((Component)this).GetComponent<Rigidbody>().get_position();
	}

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).GetComponent<Rigidbody>().get_position() - startingPos;
		if (((Vector3)(ref val)).get_magnitude() > breakDiscplacement)
		{
			SetValue(1f);
		}
	}
}

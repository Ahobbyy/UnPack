using HumanAPI;
using UnityEngine;

public class HarborCheckpoint : Checkpoint
{
	public Transform ship;

	public Transform harborShipAlign;

	public Transform harborShipAlignReverse;

	public bool reverse;

	public override void OnTriggerEnter(Collider other)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		base.OnTriggerEnter(other);
		float num = Vector3.Dot(ship.get_up(), harborShipAlign.get_up());
		reverse = num < 0f;
	}

	public override void LoadHere()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (reverse)
		{
			ship.set_rotation(harborShipAlignReverse.get_rotation());
			ship.set_position(harborShipAlignReverse.get_position());
		}
		else
		{
			ship.set_rotation(harborShipAlign.get_rotation());
			ship.set_position(harborShipAlign.get_position());
		}
	}
}

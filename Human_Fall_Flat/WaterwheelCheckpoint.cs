using System.Collections;
using HumanAPI;
using UnityEngine;

public class WaterwheelCheckpoint : Checkpoint
{
	public Breakable damLog;

	public Transform fenceSupport;

	public override void OnTriggerEnter(Collider other)
	{
		if (damLog.broken.value > 0.5f)
		{
			base.OnTriggerEnter(other);
		}
	}

	public override void LoadHere()
	{
		base.LoadHere();
		((MonoBehaviour)this).StartCoroutine(BreakFence());
	}

	private IEnumerator BreakFence()
	{
		yield return null;
		damLog.Shatter();
		Vector3 pos = fenceSupport.get_position() + new Vector3(0f, 0f, -0.5f);
		while (true)
		{
			Vector3 val = fenceSupport.get_position() - pos;
			if (((Vector3)(ref val)).get_magnitude() < 0.2f)
			{
				((Component)fenceSupport).GetComponent<Rigidbody>().MovePosition(pos);
				yield return null;
				continue;
			}
			break;
		}
	}
}

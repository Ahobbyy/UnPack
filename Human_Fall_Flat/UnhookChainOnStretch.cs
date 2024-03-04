using UnityEngine;

public class UnhookChainOnStretch : MonoBehaviour
{
	public Transform chainRoot;

	public float maxLength;

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = chainRoot.get_position() - ((Component)this).get_transform().get_position();
		if (((Vector3)(ref val)).get_sqrMagnitude() > maxLength * maxLength)
		{
			Transform transform = ((Component)this).get_transform();
			transform.set_position(transform.get_position() + ((Vector3)(ref val)).get_normalized() * 0.2f);
		}
	}

	public UnhookChainOnStretch()
		: this()
	{
	}
}

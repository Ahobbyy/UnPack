using UnityEngine;

public class SlidingAnchor : MonoBehaviour
{
	public Transform anchor;

	public Vector3 direction;

	private Vector3 connectedDirection;

	private Joint joint;

	private Vector3 offset;

	private void Start()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		joint = ((Component)this).GetComponent<Joint>();
		Vector3 val = ((Component)joint.get_connectedBody()).get_transform().InverseTransformPoint(anchor.get_position());
		Vector3 up = Vector3.get_up();
		Vector3 val2 = ((Component)this).get_transform().get_position() - anchor.get_position();
		offset = val - up * ((Vector3)(ref val2)).get_magnitude();
		connectedDirection = ((Component)joint.get_connectedBody()).get_transform().InverseTransformDirection(Vector3.get_up());
	}

	private void FixedUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).get_transform().get_position() - anchor.get_position();
		float magnitude = ((Vector3)(ref val)).get_magnitude();
		joint.set_autoConfigureConnectedAnchor(false);
		joint.set_anchor(direction * magnitude);
		joint.set_connectedAnchor(offset + connectedDirection * magnitude);
	}

	public SlidingAnchor()
		: this()
	{
	}
}

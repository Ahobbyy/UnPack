using UnityEngine;

public class FreezeKinematic : MonoBehaviour
{
	private Rigidbody body;

	private Vector3 initialPosition;

	private Quaternion initialRotation;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		initialPosition = ((Component)this).get_transform().get_localPosition();
		initialRotation = ((Component)this).get_transform().get_localRotation();
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	private void FixedUpdate()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body != (Object)null && body.get_isKinematic())
		{
			((Component)this).get_transform().set_localPosition(initialPosition);
			((Component)this).get_transform().set_localRotation(initialRotation);
		}
	}

	public FreezeKinematic()
		: this()
	{
	}
}

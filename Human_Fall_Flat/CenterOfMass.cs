using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
	public Rigidbody body;

	private void Start()
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)body == (Object)null)
		{
			body = ((Component)this).GetComponentInParent<Rigidbody>();
		}
		if ((Object)(object)body != (Object)null)
		{
			body.set_centerOfMass(((Component)body).get_transform().InverseTransformPoint(((Component)this).get_transform().get_position()));
		}
	}

	public CenterOfMass()
		: this()
	{
	}
}

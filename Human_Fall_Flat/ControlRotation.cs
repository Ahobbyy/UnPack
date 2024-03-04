using UnityEngine;

public class ControlRotation : MonoBehaviour, IControllable
{
	public Vector3 axis;

	public float min;

	public float max;

	private Rigidbody body;

	private void Start()
	{
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	public void SetControlValue(float v)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		body.MoveRotation(((Component)this).get_transform().get_parent().get_rotation() * Quaternion.AngleAxis(Mathf.Lerp(min, max, v), axis));
	}

	public ControlRotation()
		: this()
	{
	}
}

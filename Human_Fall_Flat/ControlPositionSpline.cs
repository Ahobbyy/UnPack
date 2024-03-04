using UnityEngine;

public class ControlPositionSpline : MonoBehaviour, IControllable
{
	public SplineComponent spline;

	private Rigidbody body;

	private void Start()
	{
		body = ((Component)this).GetComponent<Rigidbody>();
	}

	public void SetControlValue(float v)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		body.MovePosition(spline.GetPoint(v));
	}

	public ControlPositionSpline()
		: this()
	{
	}
}

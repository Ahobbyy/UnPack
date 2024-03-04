using UnityEngine;

public class SignalTweenRotation : SignalTweenBase
{
	public Vector3 offRot;

	public Vector3 onRot;

	public bool relative;

	public bool eulerInterpolate;

	private Quaternion initialRot;

	private Rigidbody body;

	protected override void OnEnable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		initialRot = ((Component)this).get_transform().get_localRotation();
		body = ((Component)this).GetComponent<Rigidbody>();
		base.OnEnable();
	}

	public override void OnValueChanged(float value)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		base.OnValueChanged(value);
		Quaternion val = (eulerInterpolate ? Quaternion.Euler(Vector3.Lerp(offRot, onRot, value)) : Quaternion.Lerp(Quaternion.Euler(offRot), Quaternion.Euler(onRot), value));
		if (relative)
		{
			val = initialRot * val;
		}
		if ((Object)(object)body != (Object)null)
		{
			body.MoveRotation(((Component)this).get_transform().get_parent().get_rotation() * val);
		}
		else
		{
			((Component)this).get_transform().set_localRotation(val);
		}
	}
}

using UnityEngine;

public class SignalTweenPosition : SignalTweenBase
{
	public Vector3 offPos;

	public Vector3 onPos;

	public bool relative;

	private Vector3 initialPos;

	private Rigidbody body;

	protected override void OnEnable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		initialPos = ((Component)this).get_transform().get_localPosition();
		body = ((Component)this).GetComponent<Rigidbody>();
		base.OnEnable();
	}

	public override void OnValueChanged(float value)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		base.OnValueChanged(value);
		Vector3 val = Vector3.Lerp(offPos, onPos, value);
		if (relative)
		{
			val += initialPos;
		}
		if ((Object)(object)body != (Object)null)
		{
			body.MovePosition(((Component)this).get_transform().get_parent().TransformPoint(val));
		}
		else
		{
			((Component)this).get_transform().set_localPosition(val);
		}
	}
}

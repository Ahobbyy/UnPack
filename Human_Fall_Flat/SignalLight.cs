using UnityEngine;

public class SignalLight : SignalTweenBase
{
	public float intensity = 1f;

	public override void OnValueChanged(float value)
	{
		base.OnValueChanged(value);
		float num = Mathf.Lerp(0f, intensity, value);
		((Component)this).GetComponent<Light>().set_intensity(num);
	}
}

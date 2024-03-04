using UnityEngine;

public class SignalEmission : SignalTweenBase
{
	public Color emissionColor = Color.get_white();

	public float emission = 1f;

	public override void OnValueChanged(float value)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		base.OnValueChanged(value);
		Color val = Color.LerpUnclamped(Color.get_black(), emissionColor * emission, value);
		((Renderer)((Component)this).GetComponent<MeshRenderer>()).get_material().SetColor("_EmissionColor", val);
		DynamicGI.SetEmissive((Renderer)(object)((Component)this).GetComponent<MeshRenderer>(), val);
	}
}

using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class Sail : MonoBehaviour
{
	public SkinnedMeshRenderer skin;

	public float area;

	public Vector3 wind;

	public Rigidbody boom;

	public Rigidbody boat;

	public Transform forcePoint;

	public Sound2 sailSound;

	private List<float> smoothFill = new List<float>();

	private List<float> smoothOpen = new List<float>();

	private Vector3 force;

	private float phaseOpen;

	private float phaseFill;

	private float lastFrameTime;

	private void Update()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.InverseLerp(0f, 2000f, ((Vector3)(ref force)).get_magnitude());
		num = Mathf.Sqrt(num);
		float num2 = Vector3.Dot(((Component)boom).get_transform().get_right(), ((Vector3)(ref force)).get_normalized());
		num2 = Mathf.Clamp(num2 * 100f, -1f, 1f);
		Vector3 val = wind - boom.GetPointVelocity(forcePoint.get_position());
		float num3 = Mathf.Abs(Vector3.Dot(((Component)boom).get_transform().get_right(), ((Vector3)(ref val)).get_normalized()));
		float num4 = Mathf.Sqrt(1f - num3 * num3);
		float time = ReplayRecorder.time;
		float num5 = lastFrameTime - time;
		lastFrameTime = time;
		phaseOpen += num5 * num * num * 100f;
		phaseFill += num5 * num * 5f;
		float value = num3 * num2 * num;
		float value2 = num4 * num2 * num;
		value = Smoothing.SmoothValue(smoothFill, value);
		value2 = Smoothing.SmoothValue(smoothOpen, value2);
		SetSignedShape(0, 150f * value + Mathf.Sin(phaseFill) * Mathf.Lerp(5f, 0f, num * 10f));
		SetSignedShape(2, 150f * value2 + Mathf.Sin(phaseOpen) * Mathf.Lerp(10f, 5f, num));
		if ((Object)(object)sailSound != (Object)null)
		{
			sailSound.SetPitch(Mathf.Lerp(0.4f, 1.2f, num));
			sailSound.SetVolume(Mathf.Lerp(0.2f, 1f, num));
		}
	}

	private void SetSignedShape(int baseIdx, float value)
	{
		skin.SetBlendShapeWeight(baseIdx, Mathf.Clamp(value, 0f, 100f));
		skin.SetBlendShapeWeight(baseIdx + 1, Mathf.Clamp(0f - value, 0f, 100f));
	}

	private void FixedUpdate()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (!boom.IsSleeping())
		{
			Vector3 val = wind - boom.GetPointVelocity(forcePoint.get_position());
			Vector3 val2 = ((Component)boom).get_transform().get_right();
			float num = Vector3.Dot(((Vector3)(ref val)).get_normalized(), val2);
			if (num < 0f)
			{
				num *= -1f;
				val2 *= -1f;
			}
			Vector3 val3 = val2 + ((Component)boat).get_transform().get_up() * 0.25f;
			val2 = ((Vector3)(ref val3)).get_normalized();
			force = area * val2 * num * ((Vector3)(ref val)).get_magnitude() * ((Vector3)(ref val)).get_magnitude();
			boom.AddForceAtPosition(force, forcePoint.get_position());
		}
	}

	public Sail()
		: this()
	{
	}
}

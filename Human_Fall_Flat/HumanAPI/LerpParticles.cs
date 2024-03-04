using UnityEngine;

namespace HumanAPI
{
	public class LerpParticles : LerpBase
	{
		public Color colorOff1 = Color.get_white();

		public Color colorOff2 = Color.get_white();

		public Color colorOn1 = Color.get_white();

		public Color colorOn2 = Color.get_white();

		private float timer;

		private float rate;

		private float sinceLastEmit;

		public float rateOff;

		public float rateOn = 100f;

		private ParticleSystem particles;

		private float storedValue;

		protected override void OnEnable()
		{
			base.OnEnable();
			particles = ((Component)this).GetComponent<ParticleSystem>();
		}

		public void Update()
		{
			if (rate != 0f)
			{
				sinceLastEmit += Time.get_deltaTime();
				while (sinceLastEmit > 1f / rate)
				{
					sinceLastEmit -= 1f / rate;
					Emit(storedValue);
				}
			}
		}

		protected override void ApplyValue(float value)
		{
			storedValue = value;
			rate = Mathf.Lerp(rateOff, rateOn, value);
			if (rate != 0f && sinceLastEmit > 1f / rate)
			{
				Emit(storedValue);
				sinceLastEmit = 0f;
			}
		}

		public void Emit(float storedValue)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			ParticleSystem obj = particles;
			EmitParams val = default(EmitParams);
			((EmitParams)(ref val)).set_startColor(Color32.op_Implicit(Color.Lerp(Color.Lerp(colorOff1, colorOn2, storedValue), Color.Lerp(colorOff1, colorOn2, storedValue), Random.get_value())));
			obj.Emit(val, 1);
		}
	}
}

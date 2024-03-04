using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("Human/Lerp Light Intensity", 10)]
	public class LerpLightIntensity : LerpBase
	{
		public float from;

		public float to = 1f;

		private Light lightComp;

		protected override void Awake()
		{
			lightComp = ((Component)this).GetComponent<Light>();
			base.Awake();
		}

		protected override void ApplyValue(float value)
		{
			float intensity = Mathf.Lerp(from, to, value);
			lightComp.set_intensity(intensity);
		}
	}
}

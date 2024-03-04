using UnityEngine;

namespace HumanAPI
{
	public class SteamGauge : Node
	{
		[Tooltip("Input: Source signal")]
		public NodeInput input;

		[SerializeField]
		private GameObject pin;

		[Range(0f, 100f)]
		[SerializeField]
		private float pressure;

		[SerializeField]
		private float angleMin;

		[SerializeField]
		private float angleMax;

		private Vector3 startRotation;

		private float storedAngle;

		[SerializeField]
		private float speed;

		public float maxValue = 1f;

		private void Start()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			startRotation = pin.get_transform().get_localEulerAngles();
			storedAngle = startRotation.x;
		}

		private void Update()
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Lerp(angleMin, angleMax, input.value / maxValue);
			storedAngle = Mathf.MoveTowards(storedAngle, num, speed * Time.get_deltaTime());
			Quaternion localRotation = Quaternion.Euler(storedAngle, startRotation.y, startRotation.z);
			pin.get_transform().set_localRotation(localRotation);
		}
	}
}

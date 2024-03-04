using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class Sun : LightBase
	{
		public static Sun instance;

		public override Color color => Color.get_white();

		private void Awake()
		{
			instance = this;
		}

		public override Vector3 ClosestPoint(Vector3 point)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return point;
		}

		protected override void ReturnToPool()
		{
		}
	}
}

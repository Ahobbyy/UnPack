using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class FoldableSolarPanel : LightConsume
	{
		public Transform left;

		public Transform right;

		private float dotThreshold;

		protected override void CheckOutput()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			if (Vector3.Dot(left.get_forward(), ((Component)this).get_transform().get_forward()) < dotThreshold)
			{
				num += 0.5f;
			}
			if (Vector3.Dot(right.get_forward(), ((Component)this).get_transform().get_forward()) < dotThreshold)
			{
				num += 0.5f;
			}
			Color litColor = base.LitColor;
			float num2 = (litColor.r + litColor.g + litColor.b) / 3f;
			num2 *= num;
			if (num2 != Intensity.value)
			{
				Intensity.SetValue(num2);
			}
		}
	}
}

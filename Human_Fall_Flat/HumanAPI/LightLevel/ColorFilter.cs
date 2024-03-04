using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class ColorFilter : LightFilter
	{
		public Color color;

		public override int priority => 0;

		public override void ApplyFilter(LightHitInfo info)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			Color val = default(Color);
			val.r = Mathf.Min(info.source.color.r, color.r);
			val.g = Mathf.Min(info.source.color.g, color.g);
			val.b = Mathf.Min(info.source.color.b, color.b);
			Color val2 = val;
			if (consume.debugLog)
			{
				Debug.Log((object)"Color");
			}
			foreach (LightBase output in info.outputs)
			{
				output.color = val2;
			}
		}
	}
}

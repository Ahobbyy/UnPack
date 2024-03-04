using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class ConvexLens : MirrorReflection
	{
		public float focalLength = 10f;

		protected override LightBase CreateLight(Vector3 pos, Vector3 dir)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			LightBeamConvex lightBeamConvex = LightPool.Create<LightBeamConvex>(pos, dir, ((Component)this).get_transform());
			lightBeamConvex.focalLength = focalLength;
			return lightBeamConvex;
		}

		protected override Vector3 GetDirection(LightHitInfo info)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = base.Direction;
			if (Vector3.Dot(info.source.Direction, val) < 0f)
			{
				val = -val;
			}
			return val;
		}
	}
}

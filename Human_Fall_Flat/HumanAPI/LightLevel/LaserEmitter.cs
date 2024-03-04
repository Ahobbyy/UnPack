using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LaserEmitter : LightSource
	{
		public NodeOutput hit;

		public NodeInput reset;

		public override void Process()
		{
			base.Process();
			((LaserBeam)emittedLight).Callibrate();
		}

		public void Hit(bool value)
		{
			float num = (value ? 1 : 0);
			if (hit.value != num)
			{
				hit.SetValue(num);
			}
		}

		protected override LightBase CreateLight()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			LaserBeam laserBeam = LightPool.Create<LaserBeam>(new Ray(((Component)this).get_transform().get_position(), Direction));
			laserBeam.source = this;
			return laserBeam;
		}
	}
}

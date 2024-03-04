using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LaserBeam : LightBeam
	{
		private Collider targetCollider;

		public LaserEmitter source;

		public Color hitColor = Color.get_green();

		public Color defaultColor = Color.get_red();

		public bool resetColorOnExit;

		public override Color color
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return mat.get_color();
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				if (value.r + value.g + value.b == 0f)
				{
					DisableLight();
					return;
				}
				if (value.a == 0f)
				{
					value.a = 1f;
				}
				mat.set_color(value);
			}
		}

		public override float range
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return ((Component)this).get_transform().get_localScale().z;
			}
			protected set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000b: Unknown result type (might be due to invalid IL or missing references)
				//IL_001a: Unknown result type (might be due to invalid IL or missing references)
				Vector3 localScale = ((Component)this).get_transform().get_localScale();
				localScale.z = value;
				((Component)this).get_transform().set_localScale(localScale);
			}
		}

		public void Callibrate()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			targetCollider = hitCollider;
			color = Color.get_red();
		}

		private void Update()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)targetCollider == (Object)null))
			{
				bool flag = (Object)(object)targetCollider != (Object)(object)hitCollider;
				if (flag)
				{
					color = hitColor;
				}
				else if (resetColorOnExit)
				{
					color = defaultColor;
				}
				source.Hit(flag);
			}
		}

		public override void EnableLight()
		{
			base.EnableLight();
			Callibrate();
		}
	}
}

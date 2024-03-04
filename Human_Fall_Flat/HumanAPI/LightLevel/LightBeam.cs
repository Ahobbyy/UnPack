using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightBeam : LightBase
	{
		public float maxBeamDistance = 50f;

		protected Collider hitCollider;

		protected Collider mycollider;

		protected Material mat;

		public Light light;

		public override Color color
		{
			get
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				return light.get_color();
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				//IL_0041: Unknown result type (might be due to invalid IL or missing references)
				//IL_004d: Unknown result type (might be due to invalid IL or missing references)
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
				light.set_color(value);
			}
		}

		public virtual float range
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
				//IL_0020: Unknown result type (might be due to invalid IL or missing references)
				Vector3 localScale = ((Component)this).get_transform().get_localScale();
				localScale.z = value + 0.4f;
				((Component)this).get_transform().set_localScale(localScale);
				light.set_range(value + 0.5f);
			}
		}

		protected virtual void Awake()
		{
			mycollider = ((Component)this).GetComponentInChildren<Collider>();
			MeshRenderer componentInChildren = ((Component)this).GetComponentInChildren<MeshRenderer>();
			mat = ((Renderer)componentInChildren).get_material();
			((Renderer)componentInChildren).set_sharedMaterial(mat);
		}

		private void FixedUpdate()
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)hitCollider != (Object)null)
			{
				Bounds bounds = hitCollider.get_bounds();
				if (!((Bounds)(ref bounds)).Intersects(mycollider.get_bounds()))
				{
					Recalculate();
				}
			}
		}

		protected virtual void Recalculate()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			RaycastHit val = default(RaycastHit);
			if (Physics.Raycast(((Component)this).get_transform().get_position(), Direction, ref val, maxBeamDistance, -5, (QueryTriggerInteraction)1))
			{
				range = Vector3.Distance(((RaycastHit)(ref val)).get_point(), ((Component)this).get_transform().get_position());
			}
			else
			{
				range = maxBeamDistance;
			}
			hitCollider = ((RaycastHit)(ref val)).get_collider();
		}

		public new virtual void SetSize(Bounds b)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			((Component)this).get_transform().set_localScale(((Bounds)(ref b)).get_size());
		}

		public override void Reset()
		{
			base.Reset();
			maxBeamDistance = 50f;
		}

		public override void EnableLight()
		{
			Recalculate();
			base.EnableLight();
		}

		protected override void ReturnToPool()
		{
			LightPool.DestroyLight(this);
		}

		public override void UpdateLight(LightConsume from)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			if (color.r + color.g + color.b == 0f)
			{
				DisableLight();
				return;
			}
			Recalculate();
			base.UpdateLight(from);
		}

		public override void UpdateLight()
		{
			Recalculate();
			base.UpdateLight();
		}

		public override Vector3 ClosestPoint(Vector3 point)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return mycollider.ClosestPoint(point);
		}
	}
}

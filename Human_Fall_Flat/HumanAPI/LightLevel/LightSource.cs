using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightSource : Node
	{
		public bool enableOnStart = true;

		[SerializeField]
		protected Color lightColor = Color.get_white();

		[SerializeField]
		private Vector3 direction = Vector3.get_forward();

		public LightBase emittedLight;

		public bool isOn { get; protected set; }

		public virtual Vector3 Direction
		{
			get
			{
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				//IL_000c: Unknown result type (might be due to invalid IL or missing references)
				return ((Component)this).get_transform().TransformDirection(direction);
			}
			set
			{
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				((Component)this).get_transform().set_forward(value);
			}
		}

		public virtual Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return lightColor;
			}
			set
			{
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				//IL_0007: Unknown result type (might be due to invalid IL or missing references)
				lightColor = color;
			}
		}

		protected virtual void Start()
		{
			if (enableOnStart)
			{
				EnableLight();
			}
		}

		private void Update()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)emittedLight == (Object)null) && (((Component)emittedLight).get_transform().get_position() != ((Component)this).get_transform().get_position() || emittedLight.Direction == Direction))
			{
				((Component)emittedLight).get_transform().set_position(((Component)this).get_transform().get_position());
				emittedLight.Direction = Direction;
				emittedLight.UpdateLight();
			}
		}

		protected virtual LightBase CreateLight()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			return LightPool.Create<LightBeam>(new Ray(((Component)this).get_transform().get_position(), Direction));
		}

		public virtual void EnableLight()
		{
			isOn = true;
			emittedLight = CreateLight();
		}

		public virtual void DisableLight()
		{
			if (isOn)
			{
				isOn = false;
				emittedLight.DisableLight();
			}
		}

		private void OnDrawGizmos()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			Debug.DrawRay(((Component)this).get_transform().get_position(), Direction);
			Gizmos.DrawIcon(((Component)this).get_transform().get_position(), "LightBulb.png", false);
		}
	}
}

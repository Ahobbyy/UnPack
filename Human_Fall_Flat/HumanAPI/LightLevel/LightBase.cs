using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	public abstract class LightBase : Node
	{
		private readonly Color DefaultColor = Color.get_white();

		private readonly float DefaultIntensity = 1f;

		private readonly int maxBounces = 5;

		public float intensity;

		public int bouncesLeft = 5;

		[SerializeField]
		private Vector3 direction = Vector3.get_forward();

		public Dictionary<Transform, Vector3> hitColliders = new Dictionary<Transform, Vector3>();

		private Vector3 lastPos;

		private Quaternion lastRot;

		protected List<LightConsume> consumesHit = new List<LightConsume>();

		public virtual Color color { get; set; }

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

		private void FixedUpdate()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			if (lastPos != ((Component)this).get_transform().get_position() || lastRot != ((Component)this).get_transform().get_rotation())
			{
				lastRot = ((Component)this).get_transform().get_rotation();
				lastPos = ((Component)this).get_transform().get_position();
				UpdateLight();
			}
			foreach (KeyValuePair<Transform, Vector3> hitCollider in hitColliders)
			{
				if (((Component)hitCollider.Key).get_transform().get_position() != hitCollider.Value)
				{
					hitColliders[hitCollider.Key] = hitCollider.Key.get_position();
					UpdateLight();
					break;
				}
			}
		}

		protected void ClearConsumes()
		{
			List<LightConsume> list = new List<LightConsume>(consumesHit);
			if (consumesHit.Count > 0)
			{
				foreach (LightConsume item in list)
				{
					item.RemoveLightSource(this);
				}
			}
			consumesHit.Clear();
		}

		public void AddConsume(LightConsume c)
		{
			if (!consumesHit.Contains(c))
			{
				consumesHit.Add(c);
			}
		}

		public void RemoveConsume(LightConsume c)
		{
			if (consumesHit.Contains(c))
			{
				consumesHit.Remove(c);
			}
		}

		public virtual void Reset()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			color = DefaultColor;
			intensity = DefaultIntensity;
			bouncesLeft = maxBounces;
			ClearConsumes();
		}

		public virtual void EnableLight()
		{
			((Component)this).get_gameObject().SetActive(true);
		}

		public virtual void DisableLight()
		{
			Reset();
			((Component)this).get_gameObject().SetActive(false);
			ReturnToPool();
		}

		protected abstract void ReturnToPool();

		public virtual void SetSize(Bounds b)
		{
		}

		public virtual void UpdateLight(LightConsume from)
		{
			if (consumesHit.Count <= 0)
			{
				return;
			}
			foreach (LightConsume item in consumesHit)
			{
				if ((Object)(object)from != (Object)(object)item)
				{
					item.Recalculate(this);
				}
			}
		}

		public virtual void UpdateLight()
		{
			if (consumesHit.Count <= 0)
			{
				return;
			}
			foreach (LightConsume item in consumesHit)
			{
				item.Recalculate(this);
			}
		}

		public virtual Vector3 ClosestPoint(Vector3 point)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			return ((Component)this).GetComponentInChildren<Collider>().ClosestPoint(point);
		}
	}
}

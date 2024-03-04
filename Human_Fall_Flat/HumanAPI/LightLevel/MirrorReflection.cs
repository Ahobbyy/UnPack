using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class MirrorReflection : LightFilter
	{
		public enum MirrorType
		{
			Reflect,
			Forward,
			Transparent
		}

		public enum ReflectFrom
		{
			Center,
			ContactPoint,
			ClosestPoint
		}

		public bool ignoreBackfacing = true;

		public MirrorType type;

		public ReflectFrom reflectFrom = ReflectFrom.ContactPoint;

		private Dictionary<LightBase, LightBase> reflectedLights = new Dictionary<LightBase, LightBase>();

		[SerializeField]
		private Vector3 _direction = Vector3.get_forward();

		public Vector3 Direction => ((Component)this).get_transform().TransformDirection(_direction);

		public override int priority => 1;

		public override void ApplyFilter(LightHitInfo info)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			_ = info.source.color;
			Vector3 val = info.contactPoint - ((Component)info.source).get_transform().get_position();
			Vector3 normalized = ((Vector3)(ref val)).get_normalized();
			Vector3 direction = Direction;
			if (Vector3.Dot(normalized, ((Vector3)(ref direction)).get_normalized()) <= 0f || !ignoreBackfacing)
			{
				CalculateReflection(info);
			}
			else
			{
				RemoveLight(info);
			}
		}

		private void CalculateReflection(LightHitInfo info)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			Vector3 position = GetPosition(info.contactPoint);
			Vector3 direction = GetDirection(info);
			if (consume.debugLog)
			{
				Debug.Log((object)"reflect");
			}
			LightBase lightBase;
			if (reflectedLights.ContainsKey(info.source))
			{
				lightBase = reflectedLights[info.source];
				if (!((Behaviour)lightBase).get_isActiveAndEnabled())
				{
					RemoveLight(info);
					return;
				}
				((Component)lightBase).get_transform().set_position(position);
				lightBase.Direction = direction;
				lightBase.UpdateLight(consume);
			}
			else
			{
				if (info.source.bouncesLeft == 0)
				{
					return;
				}
				lightBase = CreateLight(position, direction);
				Collider val = ((Component)this).GetComponent<Collider>();
				if ((Object)(object)val == (Object)null)
				{
					val = ((Component)this).GetComponentInParent<Collider>();
				}
				lightBase.SetSize(val.get_bounds());
				lightBase.bouncesLeft = info.source.bouncesLeft - 1;
				reflectedLights.Add(info.source, lightBase);
				info.outputs.Add(lightBase);
				consume.ignoreLights.Add(lightBase);
			}
			lightBase.color = info.source.color;
		}

		protected virtual LightBase CreateLight(Vector3 pos, Vector3 dir)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return LightPool.Create<LightBeam>(pos, dir, ((Component)this).get_transform());
		}

		protected virtual Vector3 GetPosition(Vector3 contactPoint)
		{
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			return (Vector3)(reflectFrom switch
			{
				ReflectFrom.ContactPoint => contactPoint, 
				ReflectFrom.ClosestPoint => ((Component)this).GetComponent<Collider>().ClosestPoint(contactPoint), 
				ReflectFrom.Center => ((Component)this).get_transform().get_position(), 
				_ => ((Component)this).get_transform().get_position(), 
			});
		}

		protected virtual Vector3 GetDirection(LightHitInfo info)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			return (Vector3)(type switch
			{
				MirrorType.Forward => Direction, 
				MirrorType.Reflect => Vector3.Reflect(info.source.Direction, Direction), 
				MirrorType.Transparent => info.source.Direction, 
				_ => info.source.Direction, 
			});
		}

		private void RemoveLight(LightHitInfo info)
		{
			if (reflectedLights.ContainsKey(info.source))
			{
				LightBeam item = (LightBeam)reflectedLights[info.source];
				info.outputs.Remove(item);
				RemoveLight(info.source);
			}
		}

		private void RemoveLight(LightBase source)
		{
			LightBeam lightBeam = (LightBeam)reflectedLights[source];
			consume.ignoreLights.Remove(lightBeam);
			lightBeam.DisableLight();
			reflectedLights.Remove(source);
		}

		protected override void OnLightExit(LightBase source)
		{
			if (reflectedLights.ContainsKey(source))
			{
				RemoveLight(source);
			}
		}

		private void OnDrawGizmos()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			Debug.DrawRay(((Component)this).get_transform().get_position(), Direction);
		}
	}
}

using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class HoseSocket : MonoBehaviour
	{
		public float radius = 1f;

		public float power = 1f;

		public float springAlign = 10f;

		public float springSnap = 10f;

		private static List<HoseSocket> all = new List<HoseSocket>();

		[NonSerialized]
		public uint sceneId;

		private void OnEnable()
		{
			sceneId = ((Component)this).GetComponent<NetIdentity>().sceneId;
			all.Add(this);
		}

		private void OnDisable()
		{
			all.Remove(this);
		}

		public static HoseSocket FindById(uint sceneId)
		{
			for (int i = 0; i < all.Count; i++)
			{
				if (all[i].sceneId == sceneId)
				{
					return all[i];
				}
			}
			return null;
		}

		public static HoseSocket Scan(Vector3 pos)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			HoseSocket result = null;
			float num = 0f;
			for (int i = 0; i < all.Count; i++)
			{
				HoseSocket hoseSocket = all[i];
				Vector3 val = pos - ((Component)hoseSocket).get_transform().get_position();
				float num2 = 1f - ((Vector3)(ref val)).get_magnitude() / hoseSocket.radius;
				if (!(num2 < 0f))
				{
					num2 = Mathf.Pow(num2, hoseSocket.power);
					if (num < num2)
					{
						num = num2;
						result = hoseSocket;
					}
				}
			}
			return result;
		}

		public void OnDrawGizmosSelected()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Gizmos.DrawWireSphere(((Component)this).get_transform().get_position(), radius);
		}

		public void Align(Rigidbody body, Vector3 pos, Vector3 dir)
		{
			//IL_0005: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = pos - ((Component)this).get_transform().get_position();
			float num = 1f - ((Vector3)(ref val)).get_magnitude() / radius;
			if (!(num < 0f))
			{
				num = Mathf.Pow(num, power);
				val = ((Component)this).get_transform().get_position() - pos;
				body.AddForceAtPosition((((Vector3)(ref val)).get_normalized() * springSnap - body.GetPointVelocity(pos) * body.get_mass() * 1f) * num, pos);
				HumanMotion2.AlignToVector(body, dir, ((Component)this).get_transform().get_forward(), springAlign);
			}
		}

		public HoseSocket()
			: this()
		{
		}
	}
}

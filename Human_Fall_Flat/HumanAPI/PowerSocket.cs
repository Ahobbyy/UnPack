using System;
using System.Collections.Generic;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(NetIdentity))]
	public class PowerSocket : CircuitConnector
	{
		[Tooltip("Helps with attaching cables , shows snap range")]
		public float radius = 1f;

		[Tooltip("Power output by this game object")]
		public float power = 1f;

		[Tooltip("Alignment value used in the snap")]
		public float springAlign = 10f;

		[Tooltip("Spring value used in the Snap")]
		public float springSnap = 10f;

		private static List<PowerSocket> all = new List<PowerSocket>();

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		[NonSerialized]
		public uint sceneId;

		private void OnEnable()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enable "));
			}
			sceneId = ((Component)this).GetComponent<NetIdentity>().sceneId;
			all.Add(this);
		}

		private void OnDisable()
		{
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Disable "));
			}
			all.Remove(this);
		}

		public static PowerSocket FindById(uint sceneId)
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

		public static PowerSocket Scan(Vector3 pos)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			PowerSocket result = null;
			float num = 0f;
			for (int i = 0; i < all.Count; i++)
			{
				PowerSocket powerSocket = all[i];
				Vector3 val = pos - ((Component)powerSocket).get_transform().get_position();
				float num2 = 1f - ((Vector3)(ref val)).get_magnitude() / powerSocket.radius;
				if (!(num2 < 0f))
				{
					num2 = Mathf.Pow(num2, powerSocket.power);
					if (num < num2)
					{
						num = num2;
						result = powerSocket;
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
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Align "));
			}
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
	}
}

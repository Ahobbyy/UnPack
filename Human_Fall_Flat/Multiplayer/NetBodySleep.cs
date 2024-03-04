using System;
using UnityEngine;

namespace Multiplayer
{
	public class NetBodySleep
	{
		private Rigidbody body;

		private Quaternion freezeRot;

		private Vector3 freezePos;

		private int framesFrozen;

		private float freezeDrag = 1f;

		private float freezeDragAngular = 0.5f;

		private float sleepTreshold = 0.005f;

		private float dampFrom = 0.005f;

		private float dampTo = 0.05f;

		private float maxDrag = 0.5f;

		private float maxAngularDrag = 0.5f;

		private float drag;

		private float angularDrag;

		private float mass;

		private float momentumOverMass;

		private float energyOverMass;

		private Vector3 inertia;

		private Quaternion inertiaRotation;

		public NetBodySleep(Rigidbody body)
		{
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			this.body = body;
			mass = body.get_mass();
			float num = 5f / mass;
			sleepTreshold *= num;
			dampFrom *= num;
			dampTo *= num;
			sleepTreshold = Mathf.Max(sleepTreshold, 0.001f);
			float num2 = 2f / Mathf.Sqrt(mass);
			freezeDrag = drag + (freezeDrag - drag) * num2;
			freezeDragAngular = angularDrag + (freezeDragAngular - angularDrag) * num2;
			freezeDrag = Mathf.Min(freezeDrag, maxDrag);
			freezeDragAngular = Mathf.Min(freezeDragAngular, maxAngularDrag);
			drag = body.get_drag();
			angularDrag = body.get_angularDrag();
			freezeDrag = Mathf.Max(freezeDrag, drag);
			freezeDragAngular = Mathf.Max(freezeDragAngular, angularDrag);
			NetBodySleepOverride componentInParent = ((Component)body).GetComponentInParent<NetBodySleepOverride>();
			if ((Object)(object)componentInParent != (Object)null)
			{
				sleepTreshold = componentInParent.sleepTreshold;
				dampFrom = componentInParent.dampFrom;
				dampTo = componentInParent.dampTo;
				freezeDrag = componentInParent.freezeDrag;
				freezeDragAngular = componentInParent.freezeDragAngular;
			}
			body.set_sleepThreshold(sleepTreshold);
			inertia = body.get_inertiaTensor();
			inertiaRotation = body.get_inertiaTensorRotation();
		}

		public void HandleSleep()
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0135: Unknown result type (might be due to invalid IL or missing references)
			//IL_013b: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			if (!NetGame.isClient && !body.IsSleeping())
			{
				Vector3 position = body.get_position();
				Quaternion rotation = body.get_rotation();
				Vector3 val = position - freezePos;
				float num = ((Vector3)(ref val)).get_sqrMagnitude() / Time.get_fixedDeltaTime();
				Quaternion val2 = rotation * Quaternion.Inverse(freezeRot);
				float num2 = default(float);
				Vector3 val3 = default(Vector3);
				((Quaternion)(ref val2)).ToAngleAxis(ref num2, ref val3);
				val3 *= num2 / Time.get_fixedDeltaTime() * ((float)Math.PI / 180f);
				Vector3 val4 = ((Component)body).get_transform().InverseTransformVector(val3);
				Vector3 val5 = Quaternion.Inverse(inertiaRotation) * val4;
				val = Vector3.Scale(inertia, val5);
				float sqrMagnitude = ((Vector3)(ref val)).get_sqrMagnitude();
				energyOverMass = num * 0.5f + sqrMagnitude / mass * 0.5f;
				float num3 = Mathf.InverseLerp(dampTo, dampFrom, energyOverMass);
				body.set_drag(Mathf.Lerp(drag, freezeDrag, num3));
				body.set_angularDrag(Mathf.Lerp(angularDrag, freezeDragAngular, num3 * num3));
				freezePos = position;
				freezeRot = rotation;
			}
		}
	}
}

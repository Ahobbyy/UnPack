using System;
using UnityEngine;

namespace HumanAPI
{
	public class CatapultRope : RopeRender
	{
		public Transform target;

		public float windThickness = 0.1f;

		public float windCoreRadius = 0.15f;

		private float totalLen = 6.5f;

		private float windRadius;

		private float windAngle;

		private Vector3 localTargetPos;

		private Vector3 exitWindPos;

		private Vector3 straightNormal;

		private Vector3 straightBinormal;

		private float straightT;

		public float catapultWindCount;

		private float dataWindCount;

		public bool autoWind;

		public bool isDynamic;

		private Vector3 initialWorldVectorX;

		public Rigidbody attachedBody;

		public Rigidbody wheel;

		public bool applyForceToWheel = true;

		public bool applyForceToAttachedBody = true;

		private float lengthChange;

		private Vector3 prevAttachedBodyPos;

		[Tooltip("Use this in order to show the prints coming from the script")]
		public bool showDebug;

		public override void CheckDirty()
		{
			base.CheckDirty();
			if (isDynamic || dataWindCount != catapultWindCount)
			{
				isDirty = true;
			}
		}

		public override void OnEnable()
		{
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			if (showDebug)
			{
				Debug.Log((object)(((Object)this).get_name() + " Enable "));
			}
			if (autoWind)
			{
				totalLen = float.MaxValue;
				initialWorldVectorX = ((Component)this).get_transform().TransformDirection(new Vector3(1f, 0f, 0f));
				windAngle = 0f;
			}
			if ((Object)(object)attachedBody != (Object)null)
			{
				prevAttachedBodyPos = ((Component)attachedBody).get_transform().get_position();
			}
			base.OnEnable();
		}

		public override void ReadData()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0145: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_023f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0255: Unknown result type (might be due to invalid IL or missing references)
			//IL_025a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			if (autoWind)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Auto Wind Compare vectors"));
				}
				float num;
				for (num = Math3d.SignedVectorAngle(((Component)this).get_transform().TransformDirection(new Vector3(1f, 0f, 0f)), normal: ((Component)this).get_transform().TransformDirection(new Vector3(0f, 0f, 1f)), otherVector: initialWorldVectorX); num - windAngle < -180f; num += 360f)
				{
				}
				while (num - windAngle > 180f)
				{
					num -= 360f;
				}
				windAngle = num;
				catapultWindCount = windAngle / 360f;
			}
			dataWindCount = catapultWindCount;
			windRadius = windCoreRadius + radius;
			localTargetPos = ((Component)this).get_transform().InverseTransformPoint(target.get_position());
			Vector2 val = Vector2.op_Implicit(localTargetPos.SetZ(0f));
			float num2 = Mathf.Acos(windRadius / ((Vector2)(ref val)).get_magnitude());
			Vector2 val2 = val.Rotate(0f - num2);
			Vector2 val3 = ((Vector2)(ref val2)).get_normalized() * windRadius;
			float num3 = Math2d.NormalizeAnglePositive(Math2d.SignedAngle(new Vector2(0f, 1f), val3) * ((float)Math.PI / 180f)) / ((float)Math.PI * 2f);
			int num4 = Mathf.RoundToInt(catapultWindCount - num3);
			float num5 = num3 + (float)num4 + 2f;
			exitWindPos = new Vector3(val3.x, val3.y, num5 * windThickness);
			if (autoWind)
			{
				if (showDebug)
				{
					Debug.Log((object)(((Object)this).get_name() + " Auto Wind Calc Total Length"));
				}
				straightT = 0.9f;
				float num6 = num5 * 2f * (float)Math.PI * windRadius / straightT;
				lengthChange = totalLen - num6;
				totalLen = num6;
			}
			else
			{
				straightT = num5 * 2f * (float)Math.PI * windRadius / totalLen;
			}
			straightNormal = Vector2.op_Implicit(((Vector2)(ref val3)).get_normalized());
			val2 = val - val3;
			straightBinormal = Vector3.Cross(Vector2.op_Implicit(((Vector2)(ref val2)).get_normalized()), straightNormal);
		}

		public override void GetPoint(float dist, out Vector3 pos, out Vector3 normal, out Vector3 binormal)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			if (dist < straightT)
			{
				float num = dist * totalLen / windRadius;
				float num2 = Mathf.Sin(num);
				float num3 = Mathf.Cos(num);
				normal = new Vector3(0f - num2, num3, 0f);
				binormal = new Vector3(0f, 0f, -1f);
				pos = normal * windRadius + new Vector3(0f, 0f, num / 2f / (float)Math.PI * windThickness);
			}
			else
			{
				float num4 = (dist - straightT) / (1f - straightT);
				pos = num4 * (localTargetPos - exitWindPos) + exitWindPos;
				normal = straightNormal;
				binormal = straightBinormal;
			}
		}

		public override void LateUpdate()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0088: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0114: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			base.LateUpdate();
			Vector3 val4;
			if (applyForceToWheel && (Object)(object)attachedBody != (Object)null && (Object)(object)wheel != (Object)null)
			{
				if (prevAttachedBodyPos != ((Component)attachedBody).get_transform().get_position())
				{
					Vector3 val = ((Component)this).get_transform().TransformPoint(exitWindPos);
					Vector3 val2 = ((Component)this).get_transform().TransformPoint(localTargetPos) - val;
					Vector3 val3 = ((Component)attachedBody).get_transform().get_position() - prevAttachedBodyPos;
					float num = Vector3.Dot(((Vector3)(ref val3)).get_normalized(), ((Vector3)(ref val2)).get_normalized());
					val4 = ((Component)attachedBody).get_transform().get_position() - prevAttachedBodyPos;
					float num2 = ((Vector3)(ref val4)).get_magnitude() * num;
					if (num2 > 0f)
					{
						float num3 = (float)Math.PI * 2f * (radius + windCoreRadius);
						float num4 = 360f * (num2 / num3);
						Quaternion rotation = ((Component)wheel).get_transform().get_rotation();
						rotation *= Quaternion.Euler(Vector3.get_up() * num4);
						((Component)wheel).GetComponent<Rigidbody>().MoveRotation(rotation);
					}
				}
				prevAttachedBodyPos = ((Component)attachedBody).get_transform().get_position();
			}
			if (applyForceToAttachedBody && (Object)(object)wheel != (Object)null && lengthChange < 0f)
			{
				Vector3 val5 = ((Component)this).get_transform().TransformPoint(exitWindPos);
				Vector3 val6 = ((Component)this).get_transform().TransformPoint(localTargetPos);
				Rigidbody obj = attachedBody;
				Vector3 position = ((Component)attachedBody).get_transform().get_position();
				val4 = val6 - val5;
				obj.MovePosition(position + ((Vector3)(ref val4)).get_normalized() * lengthChange);
			}
		}
	}
}

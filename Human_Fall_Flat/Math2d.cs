using System;
using UnityEngine;

public static class Math2d
{
	public static float SignedAngle(Vector2 from, Vector2 to)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Angle(from, to);
		if (Vector3.Cross(Vector2.op_Implicit(from), Vector2.op_Implicit(to)).z < 0f)
		{
			num = 0f - num;
		}
		return num;
	}

	public static float NormalizeAngle(float angle)
	{
		while (angle < -(float)Math.PI)
		{
			angle += (float)Math.PI * 2f;
		}
		while (angle > (float)Math.PI)
		{
			angle -= (float)Math.PI * 2f;
		}
		return angle;
	}

	public static float NormalizeAngleDeg(float angle)
	{
		while (angle < -180f)
		{
			angle += 360f;
		}
		while (angle > 180f)
		{
			angle -= 360f;
		}
		return angle;
	}

	public static float NormalizeAnglePositive(float angle)
	{
		while (angle > (float)Math.PI * 2f)
		{
			angle -= (float)Math.PI * 2f;
		}
		while (angle < 0f)
		{
			angle += (float)Math.PI * 2f;
		}
		return angle;
	}

	public static float CalculateDistToBisector(Vector2 edgeVector, Vector2 normal, Vector2 startBisector, Vector2 endBisector)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		float num = startBisector.x * endBisector.y - endBisector.x * startBisector.y;
		if (num != 0f)
		{
			float num2 = (edgeVector.x * endBisector.y - endBisector.x * edgeVector.y) / num;
			return (startBisector.x * normal.x + startBisector.y * normal.y) * num2;
		}
		return -10000f;
	}

	public static float CalculateTimeToBisector(Vector2 edgeVector, Vector2 normal, Vector2 startBisector, Vector2 endBisector)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		if (startBisector == Vector2.get_zero())
		{
			if (endBisector == Vector2.get_zero())
			{
				return -10000f;
			}
			return Vector2.Dot(endBisector, -edgeVector) / ((Vector2)(ref endBisector)).get_sqrMagnitude();
		}
		if (endBisector == Vector2.get_zero())
		{
			return Vector2.Dot(startBisector, edgeVector) / ((Vector2)(ref startBisector)).get_sqrMagnitude();
		}
		float num = startBisector.x * endBisector.y - endBisector.x * startBisector.y;
		if (num != 0f)
		{
			return (edgeVector.x * endBisector.y - endBisector.x * edgeVector.y) / num;
		}
		return -10000f;
	}

	public static Vector2 CalculateUnitBisector(Vector2 a, Vector2 b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = a + b;
		float num = val.x * b.x + val.y * b.y;
		if (num != 0f)
		{
			return val / num;
		}
		return a.RotateCW90();
	}

	public static Vector2 CalculateOffsetBisector(Vector2 a, Vector2 b)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		float num = a.x * b.y - b.x * a.y;
		if (num != 0f)
		{
			Vector2 val = a - b;
			float num2 = val.x * b.x + val.y * b.y;
			return a + num2 / num * a.RotateCW90();
		}
		return a.RotateCW90();
	}

	public static bool LineLineIntersection(out Vector2 intersection, Vector2 linePoint1, Vector2 lineVec1, Vector2 linePoint2, Vector2 lineVec2, float parallel_cross_epsilon = 1E-06f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		if (linePoint1 == linePoint2)
		{
			intersection = linePoint1;
			return true;
		}
		float y = lineVec1.y;
		float num = 0f - lineVec1.x;
		float num2 = y * linePoint1.x + num * linePoint1.y;
		float y2 = lineVec2.y;
		float num3 = 0f - lineVec2.x;
		float num4 = y2 * linePoint2.x + num3 * linePoint2.y;
		float num5 = y * num3 - y2 * num;
		if (Mathf.Abs(num5) < parallel_cross_epsilon)
		{
			intersection = Vector2.op_Implicit(Vector3.get_zero());
			return false;
		}
		intersection = new Vector2((num3 * num2 - num * num4) / num5, (y * num4 - y2 * num2) / num5);
		return true;
	}

	public static bool LineLineIntersection(out Vector2 intersection, Vector2 n1, float C1, Vector2 n2, float C2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		float x = n1.x;
		float y = n1.y;
		float x2 = n2.x;
		float y2 = n2.y;
		float num = x * y2 - x2 * y;
		if (Mathf.Abs(num) < 1E-06f)
		{
			intersection = Vector2.op_Implicit(Vector3.get_zero());
			return false;
		}
		intersection = new Vector2((y2 * C1 - y * C2) / num, (x * C2 - x2 * C1) / num);
		return true;
	}

	public static float Cross(Vector2 v1, Vector2 v2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		return v1.x * v2.y - v1.y * v2.x;
	}

	public static bool LineLineIntersection(out float t1, out float t2, Vector2 p1, Vector2 v1, Vector2 p2, Vector2 v2)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = p2 - p1;
		float num = v1.x * v2.y - v2.x * v1.y;
		if (num == 0f)
		{
			Vector2 val2 = v1 - v2;
			float magnitude = ((Vector2)(ref val2)).get_magnitude();
			float num2 = ((magnitude != 0f) ? (((Vector2)(ref val)).get_magnitude() / magnitude) : 0f);
			t1 = (t2 = num2);
			return false;
		}
		t1 = (val.x * v2.y - v2.x * val.y) / num;
		t2 = (val.x * v1.y - v1.x * val.y) / num;
		return true;
	}

	public static int PointOnWhichSideOfLineSegment(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = linePoint2 - linePoint1;
		Vector2 val2 = point - linePoint1;
		if (Vector2.Dot(val2, val) > 0f)
		{
			if (((Vector2)(ref val2)).get_sqrMagnitude() <= ((Vector2)(ref val)).get_sqrMagnitude())
			{
				return 0;
			}
			return 2;
		}
		return 1;
	}

	public static Vector2 ProjectPointOnLine(Vector2 linePoint, Vector2 lineVec, Vector2 point, out float t)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = point - linePoint;
		t = Vector2.Dot(val, lineVec);
		return linePoint + lineVec * t;
	}

	public static Vector2 ProjectPointOnLine(Vector2 linePoint, Vector2 lineVec, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float num = Vector2.Dot(point - linePoint, lineVec);
		return linePoint + lineVec * num;
	}

	public static Vector2 ProjectPointOnLineSegment(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = linePoint2 - linePoint1;
		Vector2 val2 = ProjectPointOnLine(linePoint1, ((Vector2)(ref val)).get_normalized(), point);
		return (Vector2)(PointOnWhichSideOfLineSegment(linePoint1, linePoint2, val2) switch
		{
			0 => val2, 
			1 => linePoint1, 
			2 => linePoint2, 
			_ => Vector2.get_zero(), 
		});
	}

	public static Vector2 ProjectPointOnLineSegment(Vector2 linePoint1, Vector2 linePoint2, Vector2 point, out float t)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = linePoint2 - linePoint1;
		Vector2 result = ProjectPointOnLine(linePoint1, ((Vector2)(ref val)).get_normalized(), point, out t);
		t /= ((Vector2)(ref val)).get_magnitude();
		if (t < 0f)
		{
			return linePoint1;
		}
		if (t > 1f)
		{
			return linePoint2;
		}
		return result;
	}

	public static float PointDistanceToSegment(Vector2 linePoint1, Vector2 linePoint2, Vector2 point)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Vector2.op_Implicit(ProjectPointOnLineSegment(linePoint1, linePoint2, point) - point);
		return ((Vector3)(ref val)).get_magnitude();
	}

	public static float LerpAngle(float from, float to, float t)
	{
		float num = NormalizeAngle(to - from);
		return NormalizeAngle(Mathf.Lerp(from, from + num, t));
	}
}

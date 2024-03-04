using System.Collections.Generic;
using UnityEngine;

public class SplineComponent : MonoBehaviour, ISpline
{
	public bool closed;

	public List<Vector3> points = new List<Vector3>();

	public float? length;

	private SplineIndex uniformIndex;

	public int ControlPointCount => points.Count;

	private SplineIndex Index
	{
		get
		{
			if (uniformIndex == null)
			{
				uniformIndex = new SplineIndex(this);
			}
			return uniformIndex;
		}
	}

	public Vector3 GetNonUniformPoint(float t)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(points.Count switch
		{
			0 => Vector3.get_zero(), 
			1 => ((Component)this).get_transform().TransformPoint(points[0]), 
			2 => ((Component)this).get_transform().TransformPoint(Vector3.Lerp(points[0], points[1], t)), 
			3 => ((Component)this).get_transform().TransformPoint(points[1]), 
			_ => Hermite(t), 
		});
	}

	public void InsertControlPoint(int index, Vector3 position)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		ResetIndex();
		if (index >= points.Count)
		{
			points.Add(position);
		}
		else
		{
			points.Insert(index, position);
		}
	}

	public void RemoveControlPoint(int index)
	{
		ResetIndex();
		points.RemoveAt(index);
	}

	public Vector3 GetControlPoint(int index)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return points[index];
	}

	public void SetControlPoint(int index, Vector3 position)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		ResetIndex();
		points[index] = position;
	}

	private Vector3 Hermite(float t)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		int num = points.Count - ((!closed) ? 3 : 0);
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		float u = t * (float)num - (float)num2;
		Vector3 pointByIndex = GetPointByIndex(num2);
		Vector3 pointByIndex2 = GetPointByIndex(num2 + 1);
		Vector3 pointByIndex3 = GetPointByIndex(num2 + 2);
		Vector3 pointByIndex4 = GetPointByIndex(num2 + 3);
		return ((Component)this).get_transform().TransformPoint(Interpolate(pointByIndex, pointByIndex2, pointByIndex3, pointByIndex4, u));
	}

	private Vector3 GetPointByIndex(int i)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (i < 0)
		{
			i += points.Count;
		}
		return points[i % points.Count];
	}

	public void ResetIndex()
	{
		uniformIndex = null;
		length = null;
	}

	public Vector3 GetRight(float t)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Vector3 point = GetPoint(t - 0.001f);
		Vector3 val = GetPoint(t + 0.001f) - point;
		Vector3 val2 = new Vector3(0f - val.z, 0f, val.x);
		return ((Vector3)(ref val2)).get_normalized();
	}

	public Vector3 GetForward(float t)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Vector3 point = GetPoint(t - 0.001f);
		Vector3 val = GetPoint(t + 0.001f) - point;
		return ((Vector3)(ref val)).get_normalized();
	}

	public Vector3 GetUp(float t)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		Vector3 point = GetPoint(t - 0.001f);
		Vector3 val = GetPoint(t + 0.001f) - point;
		return Vector3.Cross(((Vector3)(ref val)).get_normalized(), GetRight(t));
	}

	public Vector3 GetPoint(float t)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return Index.GetPoint(t);
	}

	public Vector3 GetLeft(float t)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return -GetRight(t);
	}

	public Vector3 GetDown(float t)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return -GetUp(t);
	}

	public Vector3 GetBackward(float t)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return -GetForward(t);
	}

	public float GetLength(float step = 0.001f)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		float num = 0f;
		Vector3 val = GetNonUniformPoint(0f);
		for (float num2 = 0f; num2 < 1f; num2 += step)
		{
			Vector3 nonUniformPoint = GetNonUniformPoint(num2);
			Vector3 val2 = nonUniformPoint - val;
			num += ((Vector3)(ref val2)).get_magnitude();
			val = nonUniformPoint;
		}
		return num;
	}

	public Vector3 GetDistance(float distance)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		if (!length.HasValue)
		{
			length = GetLength();
		}
		return Index.GetPoint(distance / length.Value);
	}

	public Vector3 FindClosest(Vector3 worldPoint)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		float num = float.MaxValue;
		float num2 = 0.0009765625f;
		Vector3 result = Vector3.get_zero();
		for (int i = 0; i <= 1024; i++)
		{
			Vector3 point = GetPoint((float)i * num2);
			Vector3 val = worldPoint - point;
			float sqrMagnitude = ((Vector3)(ref val)).get_sqrMagnitude();
			if (sqrMagnitude < num)
			{
				result = point;
				num = sqrMagnitude;
			}
		}
		return result;
	}

	private void Reset()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		points = new List<Vector3>
		{
			Vector3.get_forward() * 3f,
			Vector3.get_forward() * 6f,
			Vector3.get_forward() * 9f,
			Vector3.get_forward() * 12f
		};
	}

	private void OnValidate()
	{
		if (uniformIndex != null)
		{
			uniformIndex.ReIndex();
		}
	}

	internal static Vector3 Interpolate(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float u)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		return 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
	}

	public void Flatten(List<Vector3> points)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < points.Count; i++)
		{
			points[i] = Vector3.Scale(points[i], new Vector3(1f, 0f, 1f));
		}
	}

	public void CenterAroundOrigin(List<Vector3> points)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Vector3.get_zero();
		for (int i = 0; i < points.Count; i++)
		{
			val += points[i];
		}
		val /= (float)points.Count;
		for (int j = 0; j < points.Count; j++)
		{
			int index = j;
			points[index] -= val;
		}
	}

	public void Reverse(List<Vector3> points)
	{
		points.Reverse();
	}

	public SplineComponent()
		: this()
	{
	}
}

using System.Collections.Generic;
using UnityEngine;

public class SplineIndex
{
	public Vector3[] linearPoints;

	private SplineComponent spline;

	public int ControlPointCount => spline.ControlPointCount;

	public SplineIndex(SplineComponent spline)
	{
		this.spline = spline;
		ReIndex();
	}

	public void ReIndex()
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		float num = 1E-05f;
		float length = spline.GetLength(num);
		List<Vector3> list = new List<Vector3>(Mathf.FloorToInt(length * 2f));
		float num2 = 0f;
		float num3 = Mathf.Pow(length / 1024f, 2f);
		Vector3 val = spline.GetNonUniformPoint(0f);
		list.Add(val);
		while (num2 <= 1f)
		{
			Vector3 nonUniformPoint = spline.GetNonUniformPoint(num2);
			while (true)
			{
				Vector3 val2 = nonUniformPoint - val;
				if (!(((Vector3)(ref val2)).get_sqrMagnitude() <= num3))
				{
					break;
				}
				num2 += num;
				nonUniformPoint = spline.GetNonUniformPoint(num2);
			}
			val = nonUniformPoint;
			list.Add(nonUniformPoint);
		}
		linearPoints = list.ToArray();
	}

	public Vector3 GetPoint(float t)
	{
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		int num = linearPoints.Length - ((!spline.closed) ? 3 : 0);
		int num2 = Mathf.Min(Mathf.FloorToInt(t * (float)num), num - 1);
		int num3 = linearPoints.Length;
		if (num2 < 0)
		{
			num2 += num3;
		}
		float u = t * (float)num - (float)num2;
		Vector3 a = linearPoints[num2 % num3];
		Vector3 b = linearPoints[(num2 + 1) % num3];
		Vector3 c = linearPoints[(num2 + 2) % num3];
		Vector3 d = linearPoints[(num2 + 3) % num3];
		return SplineComponent.Interpolate(a, b, c, d, u);
	}
}

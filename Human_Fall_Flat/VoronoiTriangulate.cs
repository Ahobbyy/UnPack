using System;
using System.Collections.Generic;
using UnityEngine;
using Voronoi2;

public class VoronoiTriangulate : MonoBehaviour, IReset
{
	public ShatterAxis thicknessLocalAxis;

	public Vector3[] vertices;

	private Vector3[] vertexBackup;

	public int[] triangles;

	public float depth = 0.05f;

	private BoxCollider collider;

	public int seed = 56;

	private float scale = 1f;

	private Mesh mesh;

	public bool regenerate;

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		collider = ((Component)this).GetComponent<BoxCollider>();
		scale = ((Component)this).get_transform().get_lossyScale().x;
	}

	private Vector3 To3D(Vector3 v)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(thicknessLocalAxis switch
		{
			ShatterAxis.X => new Vector3(v.z, v.x, v.y) / scale, 
			ShatterAxis.Y => new Vector3(v.y, v.z, v.x) / scale, 
			ShatterAxis.Z => new Vector3(v.x, v.y, v.z) / scale, 
			_ => throw new InvalidOperationException(), 
		});
	}

	private Vector3 To2D(Vector3 v)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(thicknessLocalAxis switch
		{
			ShatterAxis.X => new Vector3(v.y, v.z, v.x) * scale, 
			ShatterAxis.Y => new Vector3(v.z, v.x, v.y) * scale, 
			ShatterAxis.Z => new Vector3(v.x, v.y, v.z) * scale, 
			_ => throw new InvalidOperationException(), 
		});
	}

	public void Deform(Vector3 worldPos, Vector3 impact)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)this).get_transform().InverseTransformPoint(worldPos);
		Vector3 val2 = ((Component)this).get_transform().InverseTransformVector(-impact);
		for (int i = 0; i < vertices.Length; i++)
		{
			float num = depth;
			Vector3 val3 = vertices[i] - val;
			float num2 = num * Mathf.InverseLerp(1f, 0f, ((Vector3)(ref val3)).get_sqrMagnitude());
			if (num2 > 0f)
			{
				ref Vector3 reference = ref vertices[i];
				reference += val2 * num2;
			}
		}
		mesh.set_vertices(vertices);
		mesh.RecalculateNormals();
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
	}

	private void Start()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		if (vertices == null || vertices.Length == 0)
		{
			Generate();
		}
		mesh = new Mesh();
		((Object)mesh).set_name("impactlayer");
		mesh.set_vertices(vertices);
		mesh.set_triangles(triangles);
		mesh.RecalculateNormals();
		vertexBackup = (Vector3[])(object)new Vector3[vertices.Length];
		vertices.CopyTo(vertexBackup, 0);
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
	}

	private void Update()
	{
		if (regenerate)
		{
			regenerate = false;
			Generate();
			mesh.set_vertices(vertices);
			mesh.RecalculateNormals();
			((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
		}
	}

	private void Generate()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0162: Unknown result type (might be due to invalid IL or missing references)
		//IL_0175: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_0262: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0280: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_031e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		//IL_039b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03df: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_042d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0466: Unknown result type (might be due to invalid IL or missing references)
		//IL_046b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0470: Unknown result type (might be due to invalid IL or missing references)
		//IL_0478: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0532: Unknown result type (might be due to invalid IL or missing references)
		//IL_0547: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0571: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0690: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		To2D(collider.get_center());
		Vector3 val = To2D(collider.get_size());
		float num = (0f - val.x) / 2f;
		float num2 = val.x / 2f;
		float num3 = (0f - val.y) / 2f;
		float num4 = val.y / 2f;
		float num5 = (0f - val.z) / 2f;
		float num6 = val.z / 2f;
		Random.InitState(seed);
		int num7 = 100;
		Voronoi voronoi = new Voronoi(0.1f);
		float[] array = new float[num7];
		float[] array2 = new float[num7];
		for (int i = 0; i < num7; i++)
		{
			array[i] = Random.Range(num, num2);
			array2[i] = Random.Range(num3, num4);
		}
		List<GraphEdge> list = voronoi.generateVoronoi(array, array2, num, num2, num3, num4);
		List<Vector2>[] array3 = new List<Vector2>[num7];
		for (int j = 0; j < num7; j++)
		{
			array3[j] = new List<Vector2>();
		}
		int count = list.Count;
		Vector2 val2 = default(Vector2);
		Vector2 val3 = default(Vector2);
		for (int k = 0; k < count; k++)
		{
			GraphEdge graphEdge = list[k];
			((Vector2)(ref val2))._002Ector(graphEdge.x1, graphEdge.y1);
			((Vector2)(ref val3))._002Ector(graphEdge.x2, graphEdge.y2);
			if (!(val2 == val3))
			{
				if (!array3[graphEdge.site1].Contains(val2))
				{
					array3[graphEdge.site1].Add(val2);
				}
				if (!array3[graphEdge.site2].Contains(val2))
				{
					array3[graphEdge.site2].Add(val2);
				}
				if (!array3[graphEdge.site1].Contains(val3))
				{
					array3[graphEdge.site1].Add(val3);
				}
				if (!array3[graphEdge.site2].Contains(val3))
				{
					array3[graphEdge.site2].Add(val3);
				}
			}
		}
		float num8 = float.MaxValue;
		int num9 = 0;
		Vector2 val4 = default(Vector2);
		((Vector2)(ref val4))._002Ector(num, num3);
		float num10 = float.MaxValue;
		int num11 = 0;
		Vector2 val5 = default(Vector2);
		((Vector2)(ref val5))._002Ector(num, num4);
		float num12 = float.MaxValue;
		int num13 = 0;
		Vector2 val6 = default(Vector2);
		((Vector2)(ref val6))._002Ector(num2, num3);
		float num14 = float.MaxValue;
		int num15 = 0;
		Vector2 val7 = default(Vector2);
		((Vector2)(ref val7))._002Ector(num2, num4);
		Vector2 val8 = default(Vector2);
		for (int l = 0; l < num7; l++)
		{
			((Vector2)(ref val8))._002Ector(array[l], array2[l]);
			Vector2 val9 = val4 - val8;
			float sqrMagnitude = ((Vector2)(ref val9)).get_sqrMagnitude();
			if (sqrMagnitude < num8)
			{
				num8 = sqrMagnitude;
				num9 = l;
			}
			val9 = val5 - val8;
			float sqrMagnitude2 = ((Vector2)(ref val9)).get_sqrMagnitude();
			if (sqrMagnitude2 < num10)
			{
				num10 = sqrMagnitude2;
				num11 = l;
			}
			val9 = val6 - val8;
			float sqrMagnitude3 = ((Vector2)(ref val9)).get_sqrMagnitude();
			if (sqrMagnitude3 < num12)
			{
				num12 = sqrMagnitude3;
				num13 = l;
			}
			val9 = val7 - val8;
			float sqrMagnitude4 = ((Vector2)(ref val9)).get_sqrMagnitude();
			if (sqrMagnitude4 < num14)
			{
				num14 = sqrMagnitude4;
				num15 = l;
			}
		}
		array3[num9].Add(val4);
		array3[num11].Add(val5);
		array3[num13].Add(val6);
		array3[num15].Add(val7);
		List<Vector3> list2 = new List<Vector3>();
		List<int> list3 = new List<int>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector2> list5 = new List<Vector2>();
		List<Vector2> list6 = new List<Vector2>();
		List<float> list7 = new List<float>();
		for (int m = 0; m < num7; m++)
		{
			new Vector2(array[m], array2[m]);
			List<Vector2> list8 = array3[m];
			if (list8.Count < 3)
			{
				continue;
			}
			list7.Clear();
			list6.Clear();
			list5.Clear();
			int count2 = list8.Count;
			Vector2 val10 = Vector2.get_zero();
			for (int n = 0; n < count2; n++)
			{
				val10 += list8[n];
			}
			val10 /= (float)list8.Count;
			for (int num16 = 0; num16 < count2; num16++)
			{
				Vector2 val11 = list8[num16] - val10;
				float num17 = Mathf.Atan2(val11.x, val11.y);
				int num18;
				for (num18 = 0; num18 < list7.Count && num17 < list7[num18]; num18++)
				{
				}
				list6.Insert(num18, list8[num16]);
				list7.Insert(num18, num17);
			}
			int count3 = list2.Count;
			bool flag = false;
			for (int num19 = 0; num19 < count2; num19++)
			{
				Vector2 val12 = list6[num19];
				list2.Add(To3D(new Vector3(val12.x, val12.y, num6)) + collider.get_center());
				list2.Add(To3D(new Vector3(val12.x, val12.y, num5)) + collider.get_center());
				if (num19 >= 2)
				{
					list3.Add(count3);
					list3.Add(list2.Count - 4);
					list3.Add(list2.Count - 2);
					list3.Add(list2.Count - 2 + 1);
					list3.Add(list2.Count - 4 + 1);
					list3.Add(count3 + 1);
				}
				bool num20 = Mathf.Abs(val12.x - num) < 0.01f || Mathf.Abs(val12.x - num2) < 0.01f || Mathf.Abs(val12.y - num3) < 0.01f || Mathf.Abs(val12.y - num4) < 0.01f;
				if (num20 && flag)
				{
					list4.Add(list2[list2.Count - 4]);
					list4.Add(list2[list2.Count - 4 + 1]);
					list4.Add(list2[list2.Count - 2]);
					list4.Add(list2[list2.Count - 2 + 1]);
				}
				flag = num20;
			}
			for (int num21 = 0; num21 < list4.Count; num21 += 4)
			{
				list3.Add(list2.Count);
				list3.Add(list2.Count + 1);
				list3.Add(list2.Count + 2);
				list3.Add(list2.Count + 2);
				list3.Add(list2.Count + 1);
				list3.Add(list2.Count + 3);
				list2.Add(list4[num21]);
				list2.Add(list4[num21 + 1]);
				list2.Add(list4[num21 + 2]);
				list2.Add(list4[num21 + 3]);
			}
		}
		vertices = list2.ToArray();
		triangles = list3.ToArray();
	}

	public void ResetState(int checkpoint, int subObjectives)
	{
		vertexBackup.CopyTo(vertices, 0);
		mesh.set_vertices(vertices);
		mesh.RecalculateNormals();
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(mesh);
	}

	public VoronoiTriangulate()
		: this()
	{
	}
}

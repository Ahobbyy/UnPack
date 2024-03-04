using System;
using System.Collections.Generic;
using UnityEngine;

public static class BuildPaddedMesh
{
	private struct Edge
	{
		public int v1;

		public int v2;

		public int prev;

		public int next;

		public Vector2 normal;
	}

	private static List<int> openEdges;

	private static Vector3[] verts;

	private static Vector2[] uvs;

	private static Edge[] edges;

	public static Mesh GeneratePadded(Mesh source, float padding)
	{
		//IL_01f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0202: Unknown result type (might be due to invalid IL or missing references)
		//IL_0207: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_021a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0330: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_034d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0352: Unknown result type (might be due to invalid IL or missing references)
		//IL_0357: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0366: Unknown result type (might be due to invalid IL or missing references)
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0402: Unknown result type (might be due to invalid IL or missing references)
		//IL_0408: Unknown result type (might be due to invalid IL or missing references)
		//IL_040f: Expected O, but got Unknown
		verts = source.get_vertices();
		uvs = source.get_uv();
		int[] triangles = source.get_triangles();
		edges = new Edge[triangles.Length];
		openEdges = new List<int>();
		for (int i = 0; i < triangles.Length; i += 3)
		{
			AddEdge(triangles[i], triangles[i + 1], i, i + 2, i + 1);
			AddEdge(triangles[i + 1], triangles[i + 2], i + 1, i, i + 2);
			AddEdge(triangles[i + 2], triangles[i], i + 2, i + 1, i);
		}
		for (int j = 0; j < openEdges.Count; j++)
		{
			for (int k = j + 1; k < openEdges.Count; k++)
			{
				Edge edge = edges[openEdges[j]];
				Edge edge2 = edges[openEdges[k]];
				if (Match(edge.v1, edge2.v2) && Match(edge.v2, edge2.v1))
				{
					edges[edge.next].prev = edge2.prev;
					edges[edge.prev].next = edge2.next;
					edges[edge2.next].prev = edge.prev;
					edges[edge2.prev].next = edge.next;
					openEdges.RemoveAt(k);
					openEdges.RemoveAt(j);
					j--;
					break;
				}
			}
		}
		Vector2 val;
		for (int l = 0; l < openEdges.Count; l++)
		{
			Edge edge3 = edges[openEdges[l]];
			val = (uvs[edge3.v1] - uvs[edge3.v2]).RotateCW90();
			Vector2 normalized = ((Vector2)(ref val)).get_normalized();
			edges[openEdges[l]].normal = normalized;
		}
		Vector3[] array = (Vector3[])(object)new Vector3[verts.Length + openEdges.Count];
		Vector2[] array2 = (Vector2[])(object)new Vector2[uvs.Length + openEdges.Count];
		int[] array3 = new int[triangles.Length + openEdges.Count * 6];
		Array.Copy(verts, array, verts.Length);
		Array.Copy(uvs, array2, uvs.Length);
		Array.Copy(triangles, array3, triangles.Length);
		for (int m = 0; m < openEdges.Count; m++)
		{
			Edge edge4 = edges[openEdges[m]];
			int num = verts.Length + m;
			int num2 = verts.Length + openEdges.IndexOf(edge4.next);
			array[num] = verts[edge4.v1];
			Vector2 val2 = uvs[edge4.v1];
			val = edge4.normal + edges[edge4.next].normal;
			array2[num] = val2 + ((Vector2)(ref val)).get_normalized() * padding;
			array3[triangles.Length + m * 6] = edge4.v2;
			array3[triangles.Length + m * 6 + 1] = edge4.v1;
			array3[triangles.Length + m * 6 + 2] = num;
			array3[triangles.Length + m * 6 + 3] = num;
			array3[triangles.Length + m * 6 + 4] = num2;
			array3[triangles.Length + m * 6 + 5] = edge4.v2;
		}
		Mesh val3 = new Mesh();
		val3.set_vertices(array);
		val3.set_uv(array2);
		val3.set_triangles(array3);
		val3.RecalculateBounds();
		val3.RecalculateNormals();
		return val3;
	}

	private static void AddEdge(int v1, int v2, int current, int prev, int next)
	{
		openEdges.Add(current);
		edges[current] = new Edge
		{
			v1 = v1,
			v2 = v2,
			prev = prev,
			next = next
		};
	}

	private static bool Match(int v1, int v2)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		try
		{
			return uvs[v1] == uvs[v2] && verts[v1] == verts[v2];
		}
		catch
		{
			return true;
		}
	}
}

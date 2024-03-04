using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HumanAPI
{
	[CustomEditor(typeof(Rail))]
	public class RailEditor : Editor
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

		private static Edge[] edges;

		private static Vector3[] verts;

		private void FinishGeneration(ref Rail rail)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			EditorUtility.SetDirty(((Editor)this).get_target());
			if ((Object)(object)rail.start == (Object)null)
			{
				rail.start = new GameObject(((Object)rail).get_name() + "Start").AddComponent<RailEnd>();
			}
			((Component)rail.start).get_transform().SetParent(((Component)rail).get_transform(), false);
			((Component)rail.start).get_transform().set_position(((Component)rail).get_transform().TransformPoint(rail.points[0]));
			rail.start.rail = rail;
			if ((Object)(object)rail.end == (Object)null)
			{
				rail.end = new GameObject(((Object)rail).get_name() + "End").AddComponent<RailEnd>();
			}
			((Component)rail.end).get_transform().SetParent(((Component)rail).get_transform(), false);
			((Component)rail.end).get_transform().set_position(((Component)rail).get_transform().TransformPoint(rail.points[rail.points.Length - 1]));
			rail.end.rail = rail;
		}

		public override void OnInspectorGUI()
		{
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0286: Unknown result type (might be due to invalid IL or missing references)
			//IL_0350: Unknown result type (might be due to invalid IL or missing references)
			//IL_0355: Unknown result type (might be due to invalid IL or missing references)
			//IL_035a: Unknown result type (might be due to invalid IL or missing references)
			((Editor)this).DrawDefaultInspector();
			if (GUILayout.Button("ReadMesh", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				Rail rail = ((Editor)this).get_target() as Rail;
				Undo.RecordObject((Object)(object)rail, "reread mesh");
				Mesh sharedMesh = ((Component)rail).GetComponent<MeshFilter>().get_sharedMesh();
				verts = sharedMesh.get_vertices();
				int[] triangles = sharedMesh.get_triangles();
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
				Vector3[] array = (Vector3[])(object)new Vector3[openEdges.Count - rail.hide];
				int index = 0;
				for (int l = 0; l < rail.first; l++)
				{
					Edge edge3 = edges[openEdges[index]];
					index = openEdges.IndexOf(edge3.next);
				}
				for (int m = 0; m < openEdges.Count - rail.hide; m++)
				{
					Edge edge4 = edges[openEdges[index]];
					array[m] = verts[edge4.v1];
					index = openEdges.IndexOf(edge4.next);
				}
				array = (rail.points = Smooth(array, rail.precision));
				FinishGeneration(ref rail);
			}
			if (!GUILayout.Button("ReadSpline", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				return;
			}
			Rail rail2 = ((Editor)this).get_target() as Rail;
			Undo.RecordObject((Object)(object)rail2, "reread mesh");
			SplineComponent component = ((Component)rail2).GetComponent<SplineComponent>();
			if ((Object)(object)component != (Object)null)
			{
				float num = 2f;
				int num2 = (int)(component.GetLength() / num);
				Vector3[] array2 = (Vector3[])(object)new Vector3[num2];
				for (int n = 0; n < num2; n++)
				{
					array2[n] = ((Component)component).get_transform().InverseTransformPoint(component.GetDistance((float)n * num));
				}
				array2 = (rail2.points = Smooth(array2, rail2.precision));
				FinishGeneration(ref rail2);
			}
		}

		private Vector3[] Smooth(Vector3[] points, float precision)
		{
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_0122: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0136: Unknown result type (might be due to invalid IL or missing references)
			//IL_013d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0147: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_015b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0165: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_016e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0192: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> list = new List<Vector3>(points);
			float num = float.MaxValue;
			do
			{
				num = float.MaxValue;
				int index = 0;
				for (int i = 1; i < list.Count - 1; i++)
				{
					Vector3 linePoint = list[i - 1];
					Vector3 val = list[i + 1] - list[i - 1];
					val = Math3d.ProjectPointOnLine(linePoint, ((Vector3)(ref val)).get_normalized(), list[i]) - list[i];
					float magnitude = ((Vector3)(ref val)).get_magnitude();
					if (magnitude < num)
					{
						num = magnitude;
						index = i;
					}
				}
				list.RemoveAt(index);
			}
			while (num < precision);
			List<Vector3> list2 = new List<Vector3>();
			list2.Add(list[0]);
			for (int j = 1; j < list.Count - 2; j++)
			{
				list2.Add(list[j]);
				float num2 = 0.5f;
				float num3 = num2 * num2;
				float num4 = num3 * num2;
				float num5 = 2f * num4 - 3f * num3 + 1f;
				float num6 = num4 - 2f * num3 + num2;
				float num7 = -2f * num4 + 3f * num3;
				float num8 = num4 - num3;
				Vector3 val2 = (list[j] - list[j - 1]) / 2f;
				Vector3 val3 = (list[j + 2] - list[j + 1]) / 2f;
				Vector3 item = num5 * list[j] + num6 * val2 + num7 * list[j + 1] + num8 * val3;
				list2.Add(item);
			}
			list2.Add(list[list.Count - 2]);
			list2.Add(list[list.Count - 1]);
			return list2.ToArray();
		}

		private void AddEdge(int v1, int v2, int current, int prev, int next)
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

		private bool Match(int v1, int v2)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			try
			{
				return verts[v1] == verts[v2];
			}
			catch
			{
				return true;
			}
		}

		public RailEditor()
			: this()
		{
		}
	}
}

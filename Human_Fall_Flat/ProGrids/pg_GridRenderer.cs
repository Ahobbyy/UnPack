using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ProGrids
{
	public class pg_GridRenderer
	{
		private static readonly HideFlags PG_HIDE_FLAGS = (HideFlags)61;

		private const string PREVIEW_OBJECT_NAME = "ProGridsGridObject";

		private const string MATERIAL_OBJECT_NAME = "ProGridsMaterialObject";

		private const string MESH_OBJECT_NAME = "ProGridsMeshObject";

		private const string GRID_SHADER = "Hidden/ProGrids/pg_GridShader";

		private const int MAX_LINES = 256;

		private static GameObject gridObject;

		private static Mesh gridMesh;

		private static Material gridMaterial;

		public static int majorLineIncrement = 10;

		private static int tan_iter;

		private static int bit_iter;

		private static int max = 256;

		private static int div = 1;

		public static void Init()
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Expected O, but got Unknown
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Expected O, but got Unknown
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			Destroy();
			gridObject = EditorUtility.CreateGameObjectWithHideFlags("ProGridsGridObject", PG_HIDE_FLAGS, new Type[2]
			{
				typeof(MeshFilter),
				typeof(MeshRenderer)
			});
			majorLineIncrement = EditorPrefs.GetInt("pg_MajorLineIncrement", 10);
			if (majorLineIncrement < 2)
			{
				majorLineIncrement = 2;
			}
			pg_SceneMeshRender pg_SceneMeshRender = gridObject.AddComponent<pg_SceneMeshRender>();
			gridMesh = new Mesh();
			((Object)gridMesh).set_name("ProGridsMeshObject");
			((Object)gridMesh).set_hideFlags(PG_HIDE_FLAGS);
			gridMaterial = new Material(Shader.Find("Hidden/ProGrids/pg_GridShader"));
			((Object)gridMaterial).set_name("ProGridsMaterialObject");
			((Object)gridMaterial).set_hideFlags(PG_HIDE_FLAGS);
			pg_SceneMeshRender.mesh = gridMesh;
			pg_SceneMeshRender.material = gridMaterial;
		}

		public static void Destroy()
		{
			DestoryObjectsWithName("ProGridsMeshObject", typeof(Mesh));
			DestoryObjectsWithName("ProGridsMaterialObject", typeof(Material));
			DestoryObjectsWithName("ProGridsGridObject", typeof(GameObject));
		}

		private static void DestoryObjectsWithName(string Name, Type type)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Expected O, but got Unknown
			foreach (Object item in (IEnumerable)(from x in Resources.FindObjectsOfTypeAll(type)
				where x.get_name().Contains(Name)
				select x))
			{
				Object.DestroyImmediate(item);
			}
		}

		public static float DrawPlane(Camera cam, Vector3 pivot, Vector3 tangent, Vector3 bitangent, float snapValue, Color color, float alphaBump)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0174: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			if (!Object.op_Implicit((Object)(object)gridMesh) || !Object.op_Implicit((Object)(object)gridMaterial) || !Object.op_Implicit((Object)(object)gridObject))
			{
				Init();
			}
			gridMaterial.SetFloat("_AlphaCutoff", 0.1f);
			gridMaterial.SetFloat("_AlphaFade", 0.6f);
			pivot = pg_Util.SnapValue(pivot, snapValue);
			Vector3 val = cam.WorldToViewportPoint(pivot);
			bool num = val.x >= 0f && val.x <= 1f && val.y >= 0f && val.y <= 1f && val.z >= 0f;
			float[] distanceToFrustumPlanes = GetDistanceToFrustumPlanes(cam, pivot, tangent, bitangent, 24f);
			if (num)
			{
				tan_iter = (int)Mathf.Ceil((Mathf.Abs(distanceToFrustumPlanes[0]) + Mathf.Abs(distanceToFrustumPlanes[2])) / snapValue);
				bit_iter = (int)Mathf.Ceil((Mathf.Abs(distanceToFrustumPlanes[1]) + Mathf.Abs(distanceToFrustumPlanes[3])) / snapValue);
				max = Mathf.Max(tan_iter, bit_iter);
				if (max > Mathf.Min(tan_iter, bit_iter) * 2)
				{
					max = Mathf.Min(tan_iter, bit_iter) * 2;
				}
				div = 1;
				float num2 = Vector3.Dot(((Component)cam).get_transform().get_position() - pivot, Vector3.Cross(tangent, bitangent));
				if (max > 256)
				{
					if (Vector3.Distance(((Component)cam).get_transform().get_position(), pivot) > 50f * snapValue && Mathf.Abs(num2) > 0.8f)
					{
						while (max / div > 256)
						{
							div += div;
						}
					}
					else
					{
						max = 256;
					}
				}
			}
			DrawFullGrid(cam, pivot, tangent, bitangent, snapValue * (float)div, max / div, div, color, alphaBump);
			return snapValue * (float)div * (float)(max / div);
		}

		public static void DrawGridPerspective(Camera cam, Vector3 pivot, float snapValue, Color[] colors, float alphaBump)
		{
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0263: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0271: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0309: Unknown result type (might be due to invalid IL or missing references)
			if (!Object.op_Implicit((Object)(object)gridMesh) || !Object.op_Implicit((Object)(object)gridMaterial) || !Object.op_Implicit((Object)(object)gridObject))
			{
				Init();
			}
			gridMaterial.SetFloat("_AlphaCutoff", 0f);
			gridMaterial.SetFloat("_AlphaFade", 0f);
			Vector3 val = pivot - ((Component)cam).get_transform().get_position();
			Vector3 normalized = ((Vector3)(ref val)).get_normalized();
			pivot = pg_Util.SnapValue(pivot, snapValue);
			Vector3 val2 = ((normalized.x < 0f) ? Vector3.get_right() : (Vector3.get_right() * -1f));
			Vector3 val3 = ((normalized.y < 0f) ? Vector3.get_up() : (Vector3.get_up() * -1f));
			Vector3 val4 = ((normalized.z < 0f) ? Vector3.get_forward() : (Vector3.get_forward() * -1f));
			Ray val5 = default(Ray);
			((Ray)(ref val5))._002Ector(pivot, val2);
			Ray val6 = default(Ray);
			((Ray)(ref val6))._002Ector(pivot, val3);
			Ray val7 = default(Ray);
			((Ray)(ref val7))._002Ector(pivot, val4);
			float num = 10f;
			float num2 = 10f;
			float num3 = 10f;
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			Plane[] array = GeometryUtility.CalculateFrustumPlanes(cam);
			float num5 = default(float);
			for (int i = 0; i < array.Length; i++)
			{
				Plane val8 = array[i];
				float num4 = 0f;
				if (((Plane)(ref val8)).Raycast(val5, ref num5))
				{
					num4 = Vector3.Distance(pivot, ((Ray)(ref val5)).GetPoint(num5));
					if (num4 < num || !flag)
					{
						flag = true;
						num = num4;
					}
				}
				if (((Plane)(ref val8)).Raycast(val6, ref num5))
				{
					num4 = Vector3.Distance(pivot, ((Ray)(ref val6)).GetPoint(num5));
					if (num4 < num2 || !flag2)
					{
						flag2 = true;
						num2 = num4;
					}
				}
				if (((Plane)(ref val8)).Raycast(val7, ref num5))
				{
					num4 = Vector3.Distance(pivot, ((Ray)(ref val7)).GetPoint(num5));
					if (num4 < num3 || !flag3)
					{
						flag3 = true;
						num3 = num4;
					}
				}
			}
			int num6 = (int)(Mathf.Ceil(Mathf.Max(num, num2)) / snapValue);
			int num7 = (int)(Mathf.Ceil(Mathf.Max(num, num3)) / snapValue);
			int num8 = (int)(Mathf.Ceil(Mathf.Max(num3, num2)) / snapValue);
			int num9 = Mathf.Max(Mathf.Max(num6, num7), num8);
			int j;
			for (j = 1; num9 / j > 256; j++)
			{
			}
			Vector3[] vertices = null;
			Vector3[] normals = null;
			Color[] colors2 = null;
			int[] indices = null;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			List<Color> list3 = new List<Color>();
			List<int> list4 = new List<int>();
			DrawHalfGrid(cam, pivot, val3, val2, snapValue * (float)j, num6 / j, colors[0], alphaBump, out vertices, out normals, out colors2, out indices, 0);
			list.AddRange(vertices);
			list2.AddRange(normals);
			list3.AddRange(colors2);
			list4.AddRange(indices);
			DrawHalfGrid(cam, pivot, val2, val4, snapValue * (float)j, num7 / j, colors[1], alphaBump, out vertices, out normals, out colors2, out indices, list.Count);
			list.AddRange(vertices);
			list2.AddRange(normals);
			list3.AddRange(colors2);
			list4.AddRange(indices);
			DrawHalfGrid(cam, pivot, val4, val3, snapValue * (float)j, num8 / j, colors[2], alphaBump, out vertices, out normals, out colors2, out indices, list.Count);
			list.AddRange(vertices);
			list2.AddRange(normals);
			list3.AddRange(colors2);
			list4.AddRange(indices);
			gridMesh.Clear();
			gridMesh.set_vertices(list.ToArray());
			gridMesh.set_normals(list2.ToArray());
			gridMesh.set_subMeshCount(1);
			gridMesh.set_uv((Vector2[])(object)new Vector2[list.Count]);
			gridMesh.set_colors(list3.ToArray());
			gridMesh.SetIndices(list4.ToArray(), (MeshTopology)3, 0);
		}

		private static void DrawHalfGrid(Camera cam, Vector3 pivot, Vector3 tan, Vector3 bitan, float increment, int iterations, Color secondary, float alphaBump, out Vector3[] vertices, out Vector3[] normals, out Color[] colors, out int[] indices, int offset)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_009b: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0155: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_015a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0180: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0195: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0208: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0214: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_0225: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_0228: Unknown result type (might be due to invalid IL or missing references)
			//IL_022d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0242: Unknown result type (might be due to invalid IL or missing references)
			//IL_0247: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0253: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0261: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026f: Unknown result type (might be due to invalid IL or missing references)
			//IL_027b: Unknown result type (might be due to invalid IL or missing references)
			//IL_027d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0289: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0297: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_034a: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_034f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0363: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_0371: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_037f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0381: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03df: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e1: Unknown result type (might be due to invalid IL or missing references)
			Color val = secondary;
			val.a += alphaBump;
			float num = increment * (float)iterations;
			int num2 = (int)(pg_Util.ValueFromMask(pivot, tan) % (increment * (float)majorLineIncrement) / increment);
			int num3 = (int)(pg_Util.ValueFromMask(pivot, bitan) % (increment * (float)majorLineIncrement) / increment);
			iterations++;
			float num4 = 0.75f;
			float num5 = num * num4;
			Vector3 val2 = Vector3.Cross(tan, bitan);
			vertices = (Vector3[])(object)new Vector3[iterations * 6 - 3];
			normals = (Vector3[])(object)new Vector3[iterations * 6 - 3];
			indices = new int[iterations * 8 - 4];
			colors = (Color[])(object)new Color[iterations * 6 - 3];
			vertices[0] = pivot;
			vertices[1] = pivot + bitan * num5;
			vertices[2] = pivot + bitan * num;
			normals[0] = val2;
			normals[1] = val2;
			normals[2] = val2;
			indices[0] = offset;
			indices[1] = 1 + offset;
			indices[2] = 1 + offset;
			indices[3] = 2 + offset;
			colors[0] = val;
			colors[1] = val;
			colors[2] = val;
			colors[2].a = 0f;
			int num6 = 4;
			int num7 = 3;
			for (int i = 1; i < iterations; i++)
			{
				vertices[num7] = pivot + (float)i * tan * increment;
				vertices[num7 + 1] = pivot + bitan * num5 + (float)i * tan * increment;
				vertices[num7 + 2] = pivot + bitan * num + (float)i * tan * increment;
				vertices[num7 + 3] = pivot + (float)i * bitan * increment;
				vertices[num7 + 4] = pivot + tan * num5 + (float)i * bitan * increment;
				vertices[num7 + 5] = pivot + tan * num + (float)i * bitan * increment;
				normals[num7] = val2;
				normals[num7 + 1] = val2;
				normals[num7 + 2] = val2;
				normals[num7 + 3] = val2;
				normals[num7 + 4] = val2;
				normals[num7 + 5] = val2;
				indices[num6] = num7 + offset;
				indices[num6 + 1] = num7 + 1 + offset;
				indices[num6 + 2] = num7 + 1 + offset;
				indices[num6 + 3] = num7 + 2 + offset;
				indices[num6 + 4] = num7 + 3 + offset;
				indices[num6 + 5] = num7 + 4 + offset;
				indices[num6 + 6] = num7 + 4 + offset;
				indices[num6 + 7] = num7 + 5 + offset;
				float num8 = (float)i / (float)iterations;
				num8 = ((num8 < num4) ? 1f : (1f - (num8 - num4) / (1f - num4)));
				Color val3 = (((i + num2) % majorLineIncrement == 0) ? val : secondary);
				val3.a *= num8;
				colors[num7] = val3;
				colors[num7 + 1] = val3;
				colors[num7 + 2] = val3;
				colors[num7 + 2].a = 0f;
				val3 = (((i + num3) % majorLineIncrement == 0) ? val : secondary);
				val3.a *= num8;
				colors[num7 + 3] = val3;
				colors[num7 + 4] = val3;
				colors[num7 + 5] = val3;
				colors[num7 + 5].a = 0f;
				num6 += 8;
				num7 += 6;
			}
		}

		private static void DrawFullGrid(Camera cam, Vector3 pivot, Vector3 tan, Vector3 bitan, float increment, int iterations, int div, Color secondary, float alphaBump)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_006b: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0109: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_0118: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_016c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			//IL_0198: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			Color val = secondary;
			val.a += alphaBump;
			float num = (float)iterations * increment;
			iterations++;
			Vector3 val2 = pivot - tan * (num / 2f) - bitan * (num / 2f);
			val2 = pg_Util.SnapValue(val2, bitan + tan, increment);
			int num2 = (int)(pg_Util.ValueFromMask(val2, tan) % (increment * (float)majorLineIncrement) / increment);
			int num3 = (int)(pg_Util.ValueFromMask(val2, bitan) % (increment * (float)majorLineIncrement) / increment);
			Vector3[] array = (Vector3[])(object)new Vector3[iterations * 4];
			int[] array2 = new int[iterations * 4];
			Color[] array3 = (Color[])(object)new Color[iterations * 4];
			int num4 = 0;
			int num5 = 0;
			for (int i = 0; i < iterations; i++)
			{
				Vector3 val3 = val2 + tan * (float)i * increment;
				Vector3 val4 = val2 + bitan * (float)i * increment;
				array[num4] = val3;
				array[num4 + 1] = val3 + bitan * num;
				array[num4 + 2] = val4;
				array[num4 + 3] = val4 + tan * num;
				array2[num5++] = num4;
				array2[num5++] = num4 + 1;
				array2[num5++] = num4 + 2;
				array2[num5++] = num4 + 3;
				array3[num4 + 1] = (array3[num4] = (((i + num2) % majorLineIncrement == 0) ? val : secondary));
				array3[num4 + 3] = (array3[num4 + 2] = (((i + num3) % majorLineIncrement == 0) ? val : secondary));
				num4 += 4;
			}
			Vector3 val5 = Vector3.Cross(tan, bitan);
			Vector3[] array4 = (Vector3[])(object)new Vector3[array.Length];
			for (int j = 0; j < array.Length; j++)
			{
				array4[j] = val5;
			}
			gridMesh.Clear();
			gridMesh.set_vertices(array);
			gridMesh.set_normals(array4);
			gridMesh.set_subMeshCount(1);
			gridMesh.set_uv((Vector2[])(object)new Vector2[array.Length]);
			gridMesh.set_colors(array3);
			gridMesh.SetIndices(array2, (MeshTopology)3, 0);
		}

		private static float[] GetDistanceToFrustumPlanes(Camera cam, Vector3 pivot, Vector3 tan, Vector3 bitan, float minDist)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			Ray[] array = (Ray[])(object)new Ray[4]
			{
				new Ray(pivot, tan),
				new Ray(pivot, bitan),
				new Ray(pivot, -tan),
				new Ray(pivot, -bitan)
			};
			float[] array2 = new float[4] { minDist, minDist, minDist, minDist };
			bool[] array3 = new bool[4];
			Plane[] array4 = GeometryUtility.CalculateFrustumPlanes(cam);
			float num2 = default(float);
			for (int i = 0; i < array4.Length; i++)
			{
				Plane val = array4[i];
				float num = 0f;
				for (int j = 0; j < 4; j++)
				{
					if (((Plane)(ref val)).Raycast(array[j], ref num2))
					{
						num = Vector3.Distance(pivot, ((Ray)(ref array[j])).GetPoint(num2));
						if (num < array2[j] || !array3[j])
						{
							array3[j] = true;
							array2[j] = Mathf.Max(minDist, num);
						}
					}
				}
			}
			return array2;
		}
	}
}

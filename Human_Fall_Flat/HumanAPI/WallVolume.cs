using System;
using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
	public class WallVolume : MonoBehaviour
	{
		public float gridSize = 0.5f;

		public List<WallVolumeFace> faces = new List<WallVolumeFace>();

		private static Vector3[] vertices;

		private static Vector3[] normals;

		private static int[] tris;

		[NonSerialized]
		public Vector3 gizmoStart;

		[NonSerialized]
		public Vector3 gizmoOffset;

		[NonSerialized]
		public Vector3 gizmoSize;

		[NonSerialized]
		public Color gizmoColor;

		private void Resize<T>(ref T[] array, int minSize, int desiredSize)
		{
			int num = ((array != null) ? array.Length : minSize);
			int num2;
			for (num2 = num; num2 < desiredSize; num2 *= 2)
			{
			}
			while (num2 > minSize && num2 / 2 > desiredSize)
			{
				num2 /= 2;
			}
			if (num2 != num || array == null)
			{
				array = new T[num2];
			}
		}

		public void FillMesh(Mesh mesh, bool forceExact)
		{
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0102: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_016d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0173: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_019d: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0211: Unknown result type (might be due to invalid IL or missing references)
			//IL_0212: Unknown result type (might be due to invalid IL or missing references)
			//IL_0217: Unknown result type (might be due to invalid IL or missing references)
			//IL_0229: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_0246: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_0266: Unknown result type (might be due to invalid IL or missing references)
			//IL_0268: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0279: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_028e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0290: Unknown result type (might be due to invalid IL or missing references)
			//IL_029b: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_02af: Unknown result type (might be due to invalid IL or missing references)
			//IL_02be: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
			if (forceExact)
			{
				vertices = (Vector3[])(object)new Vector3[faces.Count * 4];
				normals = (Vector3[])(object)new Vector3[faces.Count * 4];
				tris = new int[faces.Count * 2 * 6];
			}
			else
			{
				Resize(ref vertices, 64, faces.Count * 4);
				Resize(ref normals, 64, faces.Count * 4);
				Resize(ref tris, 96, faces.Count * 6);
			}
			Vector3 val = default(Vector3);
			for (int i = 0; i < faces.Count; i++)
			{
				WallVolumeFace wallVolumeFace = faces[i];
				Vector3 val2;
				Vector3 val3;
				Vector3 val4;
				switch (wallVolumeFace.orientation)
				{
				case WallVolumeOrientaion.Back:
					((Vector3)(ref val))._002Ector((float)wallVolumeFace.posX, (float)wallVolumeFace.posY, (float)wallVolumeFace.posZ);
					val2 = Vector3.get_back();
					val3 = Vector3.get_up();
					val4 = Vector3.get_right();
					break;
				case WallVolumeOrientaion.Right:
					((Vector3)(ref val))._002Ector((float)(wallVolumeFace.posX + 1), (float)wallVolumeFace.posY, (float)wallVolumeFace.posZ);
					val2 = Vector3.get_right();
					val3 = Vector3.get_up();
					val4 = Vector3.get_forward();
					break;
				case WallVolumeOrientaion.Forward:
					((Vector3)(ref val))._002Ector((float)(wallVolumeFace.posX + 1), (float)wallVolumeFace.posY, (float)(wallVolumeFace.posZ + 1));
					val2 = Vector3.get_forward();
					val3 = Vector3.get_up();
					val4 = Vector3.get_left();
					break;
				case WallVolumeOrientaion.Left:
					((Vector3)(ref val))._002Ector((float)wallVolumeFace.posX, (float)wallVolumeFace.posY, (float)(wallVolumeFace.posZ + 1));
					val2 = Vector3.get_left();
					val3 = Vector3.get_up();
					val4 = Vector3.get_back();
					break;
				case WallVolumeOrientaion.Down:
					((Vector3)(ref val))._002Ector((float)wallVolumeFace.posX, (float)wallVolumeFace.posY, (float)(wallVolumeFace.posZ + 1));
					val2 = Vector3.get_down();
					val3 = Vector3.get_back();
					val4 = Vector3.get_right();
					break;
				case WallVolumeOrientaion.Up:
					((Vector3)(ref val))._002Ector((float)wallVolumeFace.posX, (float)(wallVolumeFace.posY + 1), (float)wallVolumeFace.posZ);
					val2 = Vector3.get_up();
					val3 = Vector3.get_forward();
					val4 = Vector3.get_right();
					break;
				default:
					throw new InvalidOperationException();
				}
				vertices[i * 4] = val * gridSize;
				vertices[i * 4 + 1] = (val + val3) * gridSize;
				vertices[i * 4 + 2] = (val + val4 + val3) * gridSize;
				vertices[i * 4 + 3] = (val + val4) * gridSize;
				normals[i * 4] = val2;
				normals[i * 4 + 1] = val2;
				normals[i * 4 + 2] = val2;
				normals[i * 4 + 3] = val2;
				tris[i * 6] = i * 4;
				tris[i * 6 + 1] = i * 4 + 1;
				tris[i * 6 + 2] = i * 4 + 2;
				tris[i * 6 + 3] = i * 4;
				tris[i * 6 + 4] = i * 4 + 2;
				tris[i * 6 + 5] = i * 4 + 3;
			}
			for (int j = faces.Count * 6; j < tris.Length; j++)
			{
				tris[j] = 0;
			}
			((Object)mesh).set_name(((Object)this).get_name());
			mesh.set_vertices(vertices);
			mesh.set_normals(normals);
			mesh.set_triangles(tris);
			mesh.RecalculateBounds();
		}

		private bool IsOpen(int x, int y, int z, WallVolumeOrientaion orientation)
		{
			bool flag = false;
			switch (orientation)
			{
			case WallVolumeOrientaion.Up:
			{
				for (int l = 0; l < faces.Count; l++)
				{
					if (faces[l].posX == x && faces[l].posZ == z && faces[l].posY > y && (faces[l].orientation == WallVolumeOrientaion.Up || faces[l].orientation == WallVolumeOrientaion.Down))
					{
						flag = !flag;
					}
				}
				break;
			}
			case WallVolumeOrientaion.Down:
			{
				for (int n = 0; n < faces.Count; n++)
				{
					if (faces[n].posX == x && faces[n].posZ == z && faces[n].posY < y && (faces[n].orientation == WallVolumeOrientaion.Up || faces[n].orientation == WallVolumeOrientaion.Down))
					{
						flag = !flag;
					}
				}
				break;
			}
			case WallVolumeOrientaion.Left:
			{
				for (int j = 0; j < faces.Count; j++)
				{
					if (faces[j].posY == y && faces[j].posZ == z && faces[j].posX < x && (faces[j].orientation == WallVolumeOrientaion.Left || faces[j].orientation == WallVolumeOrientaion.Right))
					{
						flag = !flag;
					}
				}
				break;
			}
			case WallVolumeOrientaion.Right:
			{
				for (int m = 0; m < faces.Count; m++)
				{
					if (faces[m].posY == y && faces[m].posZ == z && faces[m].posX > x && (faces[m].orientation == WallVolumeOrientaion.Left || faces[m].orientation == WallVolumeOrientaion.Right))
					{
						flag = !flag;
					}
				}
				break;
			}
			case WallVolumeOrientaion.Forward:
			{
				for (int k = 0; k < faces.Count; k++)
				{
					if (faces[k].posX == x && faces[k].posY == y && faces[k].posZ > z && (faces[k].orientation == WallVolumeOrientaion.Forward || faces[k].orientation == WallVolumeOrientaion.Back))
					{
						flag = !flag;
					}
				}
				break;
			}
			case WallVolumeOrientaion.Back:
			{
				for (int i = 0; i < faces.Count; i++)
				{
					if (faces[i].posX == x && faces[i].posY == y && faces[i].posZ < z && (faces[i].orientation == WallVolumeOrientaion.Forward || faces[i].orientation == WallVolumeOrientaion.Back))
					{
						flag = !flag;
					}
				}
				break;
			}
			default:
				throw new InvalidOperationException();
			}
			return flag;
		}

		private void ClearVoxel(int x, int y, int z, int brushX, int brushY, int brushZ)
		{
			for (int i = 0; i < faces.Count; i++)
			{
				WallVolumeFace wallVolumeFace = faces[i];
				if (((wallVolumeFace.posX >= x && wallVolumeFace.posX <= x + brushX - 1) || (wallVolumeFace.posX == x - 1 && wallVolumeFace.orientation == WallVolumeOrientaion.Right) || (wallVolumeFace.posX == x + brushX && wallVolumeFace.orientation == WallVolumeOrientaion.Left)) && ((wallVolumeFace.posY >= y && wallVolumeFace.posY <= y + brushY - 1) || (wallVolumeFace.posY == y - 1 && wallVolumeFace.orientation == WallVolumeOrientaion.Up) || (wallVolumeFace.posY == y + brushY && wallVolumeFace.orientation == WallVolumeOrientaion.Down)) && ((wallVolumeFace.posZ >= z && wallVolumeFace.posZ <= z + brushZ - 1) || (wallVolumeFace.posZ == z - 1 && wallVolumeFace.orientation == WallVolumeOrientaion.Forward) || (wallVolumeFace.posZ == z + brushZ && wallVolumeFace.orientation == WallVolumeOrientaion.Back)))
				{
					faces.RemoveAt(i);
					i--;
				}
			}
		}

		public void AddVoxel(int x, int y, int z, int brushX, int brushY, int brushZ)
		{
			ClearVoxel(x, y, z, brushX, brushY, brushZ);
			for (int i = 0; i < brushX; i++)
			{
				for (int j = 0; j < brushY; j++)
				{
					if (!IsOpen(x + i, y + j, z, WallVolumeOrientaion.Back))
					{
						faces.Add(new WallVolumeFace(x + i, y + j, z, WallVolumeOrientaion.Back));
					}
					if (!IsOpen(x + i, y + j, z + brushZ - 1, WallVolumeOrientaion.Forward))
					{
						faces.Add(new WallVolumeFace(x + i, y + j, z + brushZ - 1, WallVolumeOrientaion.Forward));
					}
				}
			}
			for (int k = 0; k < brushY; k++)
			{
				for (int l = 0; l < brushZ; l++)
				{
					if (!IsOpen(x, y + k, z + l, WallVolumeOrientaion.Left))
					{
						faces.Add(new WallVolumeFace(x, y + k, z + l, WallVolumeOrientaion.Left));
					}
					if (!IsOpen(x + brushX - 1, y + k, z + l, WallVolumeOrientaion.Right))
					{
						faces.Add(new WallVolumeFace(x + brushX - 1, y + k, z + l, WallVolumeOrientaion.Right));
					}
				}
			}
			for (int m = 0; m < brushX; m++)
			{
				for (int n = 0; n < brushZ; n++)
				{
					if (!IsOpen(x + m, y + brushY - 1, z + n, WallVolumeOrientaion.Up))
					{
						faces.Add(new WallVolumeFace(x + m, y + brushY - 1, z + n, WallVolumeOrientaion.Up));
					}
					if (!IsOpen(x + m, y, z + n, WallVolumeOrientaion.Down))
					{
						faces.Add(new WallVolumeFace(x + m, y, z + n, WallVolumeOrientaion.Down));
					}
				}
			}
		}

		public void RemoveVoxel(int x, int y, int z, int brushX, int brushY, int brushZ)
		{
			ClearVoxel(x, y, z, brushX, brushY, brushZ);
			for (int i = 0; i < brushX; i++)
			{
				for (int j = 0; j < brushY; j++)
				{
					if (IsOpen(x + i, y + j, z, WallVolumeOrientaion.Back))
					{
						faces.Add(new WallVolumeFace(x + i, y + j, z - 1, WallVolumeOrientaion.Forward));
					}
					if (IsOpen(x + i, y + j, z + brushZ - 1, WallVolumeOrientaion.Forward))
					{
						faces.Add(new WallVolumeFace(x + i, y + j, z + brushZ, WallVolumeOrientaion.Back));
					}
				}
			}
			for (int k = 0; k < brushY; k++)
			{
				for (int l = 0; l < brushZ; l++)
				{
					if (IsOpen(x, y + k, z + l, WallVolumeOrientaion.Left))
					{
						faces.Add(new WallVolumeFace(x - 1, y + k, z + l, WallVolumeOrientaion.Right));
					}
					if (IsOpen(x + brushX - 1, y + k, z + l, WallVolumeOrientaion.Right))
					{
						faces.Add(new WallVolumeFace(x + brushX, y + k, z + l, WallVolumeOrientaion.Left));
					}
				}
			}
			for (int m = 0; m < brushX; m++)
			{
				for (int n = 0; n < brushZ; n++)
				{
					if (IsOpen(x + m, y + brushY - 1, z + n, WallVolumeOrientaion.Up))
					{
						faces.Add(new WallVolumeFace(x + m, y + brushY, z + n, WallVolumeOrientaion.Down));
					}
					if (IsOpen(x + m, y, z + n, WallVolumeOrientaion.Down))
					{
						faces.Add(new WallVolumeFace(x + m, y - 1, z + n, WallVolumeOrientaion.Up));
					}
				}
			}
		}

		public void OnDrawGizmosSelected()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			if (!(gizmoSize == Vector3.get_zero()))
			{
				Gizmos.set_color(gizmoColor);
				Gizmos.set_matrix(((Component)this).get_transform().get_localToWorldMatrix());
				Gizmos.DrawCube(gizmoStart + gizmoSize / 2f, gizmoSize + gizmoOffset);
				Gizmos.DrawWireCube(gizmoStart + gizmoSize / 2f, gizmoSize + gizmoOffset);
			}
		}

		public WallVolume()
			: this()
		{
		}
	}
}

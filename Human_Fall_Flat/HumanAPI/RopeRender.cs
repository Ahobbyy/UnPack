using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	public class RopeRender : MonoBehaviour
	{
		public int meshSegments = 20;

		public int segmentsAround = 6;

		public float radius = 0.1f;

		private Vector2[] rotatedRadius;

		private Mesh mesh;

		private MeshFilter filter;

		private Vector3[] meshVerts;

		private bool forceUpdate;

		public bool visible;

		public bool isDirty;

		public virtual void OnEnable()
		{
			//IL_016f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0179: Expected O, but got Unknown
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0201: Unknown result type (might be due to invalid IL or missing references)
			int num = meshSegments + 1;
			meshVerts = (Vector3[])(object)new Vector3[num * segmentsAround + 2 * segmentsAround];
			int[] array = new int[meshSegments * segmentsAround * 6 + 6 * (segmentsAround - 2)];
			int num2 = 0;
			num2 = 0;
			for (int i = 0; i < num - 1; i++)
			{
				for (int j = 0; j < segmentsAround; j++)
				{
					int num3 = i * segmentsAround;
					int num4 = num3 + segmentsAround;
					int num5 = (j + 1) % segmentsAround;
					int num6 = num3 + j;
					int num7 = num3 + num5;
					int num8 = num4 + j;
					int num9 = num4 + num5;
					array[num2++] = num6;
					array[num2++] = num7;
					array[num2++] = num8;
					array[num2++] = num8;
					array[num2++] = num7;
					array[num2++] = num9;
				}
			}
			int num10 = num * segmentsAround;
			for (int k = 0; k < segmentsAround - 2; k++)
			{
				array[num2++] = num10;
				array[num2++] = num10 + k + 2;
				array[num2++] = num10 + k + 1;
			}
			int num11 = (num + 1) * segmentsAround;
			for (int l = 0; l < segmentsAround - 2; l++)
			{
				array[num2++] = num11;
				array[num2++] = num11 + l + 1;
				array[num2++] = num11 + l + 2;
			}
			mesh = new Mesh();
			((Object)mesh).set_name("rope " + ((Object)this).get_name());
			mesh.set_vertices(meshVerts);
			mesh.set_triangles(array);
			filter = ((Component)this).GetComponent<MeshFilter>();
			rotatedRadius = (Vector2[])(object)new Vector2[segmentsAround];
			for (int m = 0; m < segmentsAround; m++)
			{
				rotatedRadius[m] = VectorExtensions.Rotate(new Vector2(radius, 0f), (float)Math.PI * 2f * (float)m / (float)segmentsAround);
			}
		}

		protected virtual float GetLod()
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			GetPoint(0.5f, out var pos, out var _, out var _);
			pos = ((Component)this).get_transform().TransformPoint(pos);
			float num = float.MaxValue;
			for (int i = 0; i < NetGame.instance.local.players.Count; i++)
			{
				Human human = NetGame.instance.local.players[i].human;
				float num2 = num;
				Vector3 val = ((Component)human).get_transform().get_position() - pos;
				num = Mathf.Min(num2, ((Vector3)(ref val)).get_sqrMagnitude());
			}
			return 1f + Mathf.InverseLerp(100f, 1000f, num) * 4f;
		}

		protected void ForceUpdate()
		{
			forceUpdate = true;
		}

		public virtual void LateUpdate()
		{
			//IL_0093: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			if (!isDirty)
			{
				CheckDirty();
			}
			if (!forceUpdate && (!visible || !isDirty))
			{
				return;
			}
			forceUpdate = false;
			ReadData();
			float lod = GetLod();
			int idx = 0;
			int num = (int)((float)meshSegments / lod);
			int num2 = num + 1;
			for (int i = 0; i < num2; i++)
			{
				UpdateRing(1f * (float)i / (float)num, ref idx);
			}
			for (int j = 0; j < meshSegments - num; j++)
			{
				for (int k = 0; k < segmentsAround; k++)
				{
					meshVerts[idx++] = meshVerts[idx - segmentsAround];
				}
			}
			UpdateRing(0f, ref idx);
			UpdateRing(1f, ref idx);
			mesh.set_vertices(meshVerts);
			mesh.RecalculateNormals();
			mesh.RecalculateBounds();
			filter.set_sharedMesh(mesh);
			isDirty = false;
		}

		private void UpdateRing(float t, ref int idx)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			GetPoint(t, out var pos, out var normal, out var binormal);
			for (int i = 0; i < segmentsAround; i++)
			{
				Vector2 val = rotatedRadius[i];
				Vector3 val2 = pos + val.x * normal + val.y * binormal;
				meshVerts[idx++] = val2;
			}
		}

		public virtual void GetPoint(float dist, out Vector3 pos, out Vector3 normal, out Vector3 binormal)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			pos = (normal = (binormal = Vector3.get_zero()));
		}

		public void OnBecameInvisible()
		{
			visible = false;
		}

		public void OnBecameVisible()
		{
			visible = true;
		}

		public virtual void CheckDirty()
		{
		}

		public virtual void ReadData()
		{
		}

		public RopeRender()
			: this()
		{
		}
	}
}

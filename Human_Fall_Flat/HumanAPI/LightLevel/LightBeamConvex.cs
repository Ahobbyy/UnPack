using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI.LightLevel
{
	public class LightBeamConvex : LightBeam
	{
		public float focalLength = 5f;

		private MeshFilter meshFilter;

		private List<int> vertEndids;

		private Vector3[] startingVerts;

		private Color _color = Color.get_white();

		private float _range;

		public override Color color
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return _color;
			}
			set
			{
				//IL_0000: Unknown result type (might be due to invalid IL or missing references)
				//IL_0006: Unknown result type (might be due to invalid IL or missing references)
				//IL_000d: Unknown result type (might be due to invalid IL or missing references)
				//IL_0022: Unknown result type (might be due to invalid IL or missing references)
				if (value.r + value.g + value.b == 0f)
				{
					DisableLight();
				}
				else if (value.a == 0f)
				{
					value.a = 1f;
				}
			}
		}

		public override float range
		{
			get
			{
				return _range;
			}
			protected set
			{
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				//IL_002b: Unknown result type (might be due to invalid IL or missing references)
				_range = value;
				Vector3 localScale = ((Component)mycollider).get_transform().get_localScale();
				localScale.z = value;
				((Component)mycollider).get_transform().set_localScale(localScale);
				UpdateCone();
			}
		}

		protected override void Awake()
		{
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			base.Awake();
			meshFilter = ((Component)this).GetComponentInChildren<MeshFilter>();
			startingVerts = (Vector3[])(object)new Vector3[meshFilter.get_mesh().get_vertexCount()];
			meshFilter.get_mesh().get_vertices().CopyTo(startingVerts, 0);
			vertEndids = new List<int>();
			Vector3 val = startingVerts[0];
			for (int i = 0; i < startingVerts.Length; i++)
			{
				if (val.y != startingVerts[i].y)
				{
					vertEndids.Add(i);
				}
			}
		}

		private void UpdateCone()
		{
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0078: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)meshFilter == (Object)null))
			{
				Vector3[] vertices = meshFilter.get_mesh().get_vertices();
				float y = vertices[0].y;
				Vector3 val = default(Vector3);
				((Vector3)(ref val))._002Ector(0f, focalLength + y, 0f);
				float num = range / focalLength;
				for (int i = 0; i < vertEndids.Count; i++)
				{
					int num2 = vertEndids[i];
					vertices[num2] = Vector3.LerpUnclamped(startingVerts[num2], val, num);
					vertices[num2].y = range + y;
				}
				meshFilter.get_mesh().set_vertices(vertices);
				meshFilter.get_mesh().RecalculateBounds();
			}
		}

		public override void SetSize(Bounds b)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			Vector3 size = ((Bounds)(ref b)).get_size();
			size.z = 1f;
			((Component)this).get_transform().set_localScale(size);
		}

		public override void Reset()
		{
			base.Reset();
			focalLength = 5f;
			meshFilter.get_mesh().set_vertices(startingVerts);
		}
	}
}

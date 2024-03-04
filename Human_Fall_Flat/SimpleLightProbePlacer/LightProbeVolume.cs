using System.Collections.Generic;
using UnityEngine;

namespace SimpleLightProbePlacer
{
	[AddComponentMenu("Rendering/Light Probe Volume")]
	public class LightProbeVolume : TransformVolume
	{
		[SerializeField]
		private LightProbeVolumeType m_type;

		[SerializeField]
		private Vector3 m_densityFixed = Vector3.get_one();

		[SerializeField]
		private Vector3 m_densityFloat = Vector3.get_one();

		public LightProbeVolumeType Type
		{
			get
			{
				return m_type;
			}
			set
			{
				m_type = value;
			}
		}

		public Vector3 Density
		{
			get
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_0010: Unknown result type (might be due to invalid IL or missing references)
				if (m_type != 0)
				{
					return m_densityFloat;
				}
				return m_densityFixed;
			}
			set
			{
				//IL_0009: Unknown result type (might be due to invalid IL or missing references)
				//IL_000a: Unknown result type (might be due to invalid IL or missing references)
				//IL_0011: Unknown result type (might be due to invalid IL or missing references)
				//IL_0012: Unknown result type (might be due to invalid IL or missing references)
				if (m_type == LightProbeVolumeType.Fixed)
				{
					m_densityFixed = value;
				}
				else
				{
					m_densityFloat = value;
				}
			}
		}

		public static Color EditorColor => new Color(1f, 0.9f, 0.25f);

		public List<Vector3> CreatePositions()
		{
			return CreatePositions(m_type);
		}

		public List<Vector3> CreatePositions(LightProbeVolumeType type)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (type != 0)
			{
				return CreatePositionsFloat(((Component)this).get_transform(), base.Origin, base.Size, Density);
			}
			return CreatePositionsFixed(((Component)this).get_transform(), base.Origin, base.Size, Density);
		}

		public static List<Vector3> CreatePositionsFixed(Transform volumeTransform, Vector3 origin, Vector3 size, Vector3 density)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> list = new List<Vector3>();
			Vector3 val = origin;
			float num = size.x / (float)Mathf.FloorToInt(density.x);
			float num2 = size.y / (float)Mathf.FloorToInt(density.y);
			float num3 = size.z / (float)Mathf.FloorToInt(density.z);
			val -= size * 0.5f;
			for (int i = 0; (float)i <= density.x; i++)
			{
				for (int j = 0; (float)j <= density.y; j++)
				{
					for (int k = 0; (float)k <= density.z; k++)
					{
						Vector3 val2 = val + new Vector3((float)i * num, (float)j * num2, (float)k * num3);
						val2 = volumeTransform.TransformPoint(val2);
						list.Add(val2);
					}
				}
			}
			return list;
		}

		public static List<Vector3> CreatePositionsFloat(Transform volumeTransform, Vector3 origin, Vector3 size, Vector3 density)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_009f: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00df: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			List<Vector3> list = new List<Vector3>();
			Vector3 val = origin;
			int num = Mathf.FloorToInt(size.x / density.x);
			int num2 = Mathf.FloorToInt(size.y / density.y);
			int num3 = Mathf.FloorToInt(size.z / density.z);
			val -= size * 0.5f;
			val.x += (size.x - (float)num * density.x) * 0.5f;
			val.y += (size.y - (float)num2 * density.y) * 0.5f;
			val.z += (size.z - (float)num3 * density.z) * 0.5f;
			for (int i = 0; i <= num; i++)
			{
				for (int j = 0; j <= num2; j++)
				{
					for (int k = 0; k <= num3; k++)
					{
						Vector3 val2 = val + new Vector3((float)i * density.x, (float)j * density.y, (float)k * density.z);
						val2 = volumeTransform.TransformPoint(val2);
						list.Add(val2);
					}
				}
			}
			return list;
		}
	}
}

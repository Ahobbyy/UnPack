using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SimpleLightProbePlacer
{
	[RequireComponent(typeof(LightProbeGroup))]
	[AddComponentMenu("Rendering/Light Probe Group Control")]
	public class LightProbeGroupControl : MonoBehaviour
	{
		[SerializeField]
		private float m_mergeDistance = 0.5f;

		[SerializeField]
		private bool m_usePointLights = true;

		[SerializeField]
		private float m_pointLightRange = 1f;

		private int m_mergedProbes;

		private LightProbeGroup m_lightProbeGroup;

		public float MergeDistance
		{
			get
			{
				return m_mergeDistance;
			}
			set
			{
				m_mergeDistance = value;
			}
		}

		public int MergedProbes => m_mergedProbes;

		public bool UsePointLights
		{
			get
			{
				return m_usePointLights;
			}
			set
			{
				m_usePointLights = value;
			}
		}

		public float PointLightRange
		{
			get
			{
				return m_pointLightRange;
			}
			set
			{
				m_pointLightRange = value;
			}
		}

		public LightProbeGroup LightProbeGroup
		{
			get
			{
				if ((Object)(object)m_lightProbeGroup != (Object)null)
				{
					return m_lightProbeGroup;
				}
				return m_lightProbeGroup = ((Component)this).GetComponent<LightProbeGroup>();
			}
		}

		public void DeleteAll()
		{
			LightProbeGroup.set_probePositions((Vector3[])null);
			m_mergedProbes = 0;
		}

		public void Create()
		{
			DeleteAll();
			List<Vector3> list = CreatePositions();
			list.AddRange(CreateAroundPointLights(m_pointLightRange));
			list = MergeClosestPositions(list, m_mergeDistance, out m_mergedProbes);
			ApplyPositions(list);
		}

		public void Merge()
		{
			if (LightProbeGroup.get_probePositions() != null)
			{
				List<Vector3> source = MergeClosestPositions(LightProbeGroup.get_probePositions().ToList(), m_mergeDistance, out m_mergedProbes);
				source = source.Select((Vector3 x) => ((Component)this).get_transform().TransformPoint(x)).ToList();
				ApplyPositions(source);
			}
		}

		private void ApplyPositions(List<Vector3> positions)
		{
			LightProbeGroup.set_probePositions(positions.Select((Vector3 x) => ((Component)this).get_transform().InverseTransformPoint(x)).ToArray());
		}

		private static List<Vector3> CreatePositions()
		{
			LightProbeVolume[] array = Object.FindObjectsOfType<LightProbeVolume>();
			if (array.Length == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < array.Length; i++)
			{
				list.AddRange(array[i].CreatePositions());
			}
			return list;
		}

		private static List<Vector3> CreateAroundPointLights(float range)
		{
			List<Light> list = (from x in Object.FindObjectsOfType<Light>()
				where (int)x.get_type() == 2
				select x).ToList();
			if (list.Count == 0)
			{
				return new List<Vector3>();
			}
			List<Vector3> list2 = new List<Vector3>();
			for (int i = 0; i < list.Count; i++)
			{
				list2.AddRange(CreatePositionsAround(((Component)list[i]).get_transform(), range));
			}
			return list2;
		}

		private static List<Vector3> MergeClosestPositions(List<Vector3> positions, float distance, out int mergedCount)
		{
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0128: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_0156: Unknown result type (might be due to invalid IL or missing references)
			//IL_0160: Unknown result type (might be due to invalid IL or missing references)
			//IL_0170: Unknown result type (might be due to invalid IL or missing references)
			//IL_0175: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			if (positions == null)
			{
				mergedCount = 0;
				return new List<Vector3>();
			}
			int count = positions.Count;
			bool flag = false;
			while (!flag)
			{
				Dictionary<Vector3, List<Vector3>> dictionary = new Dictionary<Vector3, List<Vector3>>();
				int i;
				for (i = 0; i < positions.Count; i++)
				{
					List<Vector3> list = positions.Where(delegate(Vector3 x)
					{
						//IL_0000: Unknown result type (might be due to invalid IL or missing references)
						//IL_0012: Unknown result type (might be due to invalid IL or missing references)
						//IL_0017: Unknown result type (might be due to invalid IL or missing references)
						//IL_001c: Unknown result type (might be due to invalid IL or missing references)
						Vector3 val2 = x - positions[i];
						return ((Vector3)(ref val2)).get_magnitude() < distance;
					}).ToList();
					if (list.Count > 0 && !dictionary.ContainsKey(positions[i]))
					{
						dictionary.Add(positions[i], list);
					}
				}
				positions.Clear();
				List<Vector3> list2 = dictionary.Keys.ToList();
				for (int j = 0; j < list2.Count; j++)
				{
					Vector3 center = dictionary[list2[j]].Aggregate(Vector3.get_zero(), (Vector3 result, Vector3 target) => result + target) / (float)dictionary[list2[j]].Count;
					if (!positions.Exists((Vector3 x) => x == center))
					{
						positions.Add(center);
					}
				}
				flag = positions.Select((Vector3 x) => positions.Where(delegate(Vector3 y)
				{
					//IL_0000: Unknown result type (might be due to invalid IL or missing references)
					//IL_0002: Unknown result type (might be due to invalid IL or missing references)
					//IL_000e: Unknown result type (might be due to invalid IL or missing references)
					//IL_0010: Unknown result type (might be due to invalid IL or missing references)
					//IL_0015: Unknown result type (might be due to invalid IL or missing references)
					//IL_001a: Unknown result type (might be due to invalid IL or missing references)
					if (y != x)
					{
						Vector3 val = y - x;
						return ((Vector3)(ref val)).get_magnitude() < distance;
					}
					return false;
				})).All((IEnumerable<Vector3> x) => !x.Any());
			}
			mergedCount = count - positions.Count;
			return positions;
		}

		public static List<Vector3> CreatePositionsAround(Transform transform, float range)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			return ((IEnumerable<Vector3>)(object)new Vector3[8]
			{
				new Vector3(-0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, 0.5f),
				new Vector3(0.5f, 0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, -0.5f),
				new Vector3(-0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, 0.5f),
				new Vector3(0.5f, -0.5f, -0.5f)
			}).Select((Vector3 x) => transform.TransformPoint(x * range)).ToList();
		}

		public LightProbeGroupControl()
			: this()
		{
		}
	}
}

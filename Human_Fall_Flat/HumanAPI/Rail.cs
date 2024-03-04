using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class Rail : MonoBehaviour
	{
		private static List<Rail> all = new List<Rail>();

		public RailEnd start;

		public RailEnd end;

		public int first;

		public int hide;

		public Vector3[] points;

		public float precision = 0.1f;

		private void OnEnable()
		{
			all.Add(this);
		}

		private void OnDisable()
		{
			all.Remove(this);
		}

		private void Awake()
		{
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			if (points != null && points.Length != 0)
			{
				for (int i = 0; i < points.Length; i++)
				{
					points[i] = ((Component)this).get_transform().TransformPoint(points[i]);
				}
			}
		}

		private void Start()
		{
			if ((Object)(object)start == (Object)null || (Object)(object)end == (Object)null)
			{
				Debug.LogError((object)"Rail missing it's ends", (Object)(object)this);
			}
		}

		private void OnDrawGizmosSelected()
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Unknown result type (might be due to invalid IL or missing references)
			if (points != null && points.Length != 0)
			{
				Gizmos.set_color(Color.get_blue());
				for (int i = 0; i < points.Length - 1; i++)
				{
					Gizmos.DrawLine(((Component)this).get_transform().TransformPoint(points[i]), ((Component)this).get_transform().TransformPoint(points[(i + 1) % points.Length]));
				}
				for (int j = 0; j < points.Length; j++)
				{
					Gizmos.DrawSphere(((Component)this).get_transform().TransformPoint(points[j]), 0.1f);
				}
			}
		}

		public void ProjectSegment(Vector3 worldPos, int index, ref float bestDistSqr, ref Vector3 bestProjected, ref Rail bestRail, ref int bestIndex)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = Math3d.ProjectPointOnLineSegment(points[index], points[(index + 1) % points.Length], worldPos);
			Vector3 val2 = val - worldPos;
			float sqrMagnitude = ((Vector3)(ref val2)).get_sqrMagnitude();
			if (sqrMagnitude < bestDistSqr)
			{
				bestDistSqr = sqrMagnitude;
				bestProjected = val;
				bestRail = this;
				bestIndex = index;
			}
		}

		public static bool Next(ref Rail currentRail, ref int currentIndex, ref bool fwd)
		{
			RailEnd railEnd = null;
			if (fwd)
			{
				if (currentIndex < currentRail.points.Length - 2)
				{
					currentIndex++;
					return true;
				}
				railEnd = currentRail.end.connectedTo;
			}
			else
			{
				if (currentIndex > 0)
				{
					currentIndex--;
					return true;
				}
				railEnd = currentRail.start.connectedTo;
			}
			if ((Object)(object)railEnd == (Object)null)
			{
				currentRail = null;
				currentIndex = -1;
				return false;
			}
			currentRail = railEnd.rail;
			if ((Object)(object)currentRail.start == (Object)(object)railEnd)
			{
				fwd = true;
				currentIndex = 0;
			}
			else
			{
				fwd = false;
				currentIndex = currentRail.points.Length - 1;
			}
			return true;
		}

		public static bool Project(Vector3 worldPos, ref Vector3 projected, ref Rail currentRail, ref int currentIndex)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0130: Unknown result type (might be due to invalid IL or missing references)
			//IL_0131: Unknown result type (might be due to invalid IL or missing references)
			//IL_0137: Unknown result type (might be due to invalid IL or missing references)
			//IL_013c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0141: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			Debug.DrawRay(worldPos, Vector3.get_up(), Color.get_gray());
			float bestDistSqr = float.MaxValue;
			Vector3 bestProjected = Vector3.get_zero();
			Rail bestRail = null;
			int bestIndex = 0;
			if ((Object)(object)currentRail != (Object)null)
			{
				currentRail.ProjectSegment(worldPos, currentIndex, ref bestDistSqr, ref bestProjected, ref bestRail, ref bestIndex);
				Rail currentRail2 = currentRail;
				int currentIndex2 = currentIndex;
				bool fwd = true;
				for (int i = 0; i < 5; i++)
				{
					if (!Next(ref currentRail2, ref currentIndex2, ref fwd))
					{
						break;
					}
					currentRail2.ProjectSegment(worldPos, currentIndex2, ref bestDistSqr, ref bestProjected, ref bestRail, ref bestIndex);
				}
				currentRail2 = currentRail;
				currentIndex2 = currentIndex;
				fwd = false;
				for (int j = 0; j < 5; j++)
				{
					if (!Next(ref currentRail2, ref currentIndex2, ref fwd))
					{
						break;
					}
					currentRail2.ProjectSegment(worldPos, currentIndex2, ref bestDistSqr, ref bestProjected, ref bestRail, ref bestIndex);
				}
				if (bestDistSqr > 1f)
				{
					currentRail = null;
				}
			}
			if ((Object)(object)currentRail == (Object)null)
			{
				for (int k = 0; k < all.Count; k++)
				{
					for (int l = 0; l < all[k].points.Length - 1; l++)
					{
						all[k].ProjectSegment(worldPos, l, ref bestDistSqr, ref bestProjected, ref bestRail, ref bestIndex);
					}
				}
			}
			if (bestDistSqr <= 1f)
			{
				currentRail = bestRail;
				currentIndex = bestIndex;
				projected = bestProjected;
				Debug.DrawRay(projected, Vector3.get_up(), Color.get_green());
				return true;
			}
			currentRail = null;
			currentIndex = -1;
			projected = Vector3.get_zero();
			return false;
		}

		public Rail()
			: this()
		{
		}
	}
}

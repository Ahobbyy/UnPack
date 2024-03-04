using UnityEngine;

namespace ProGrids
{
	public static class PGExtensions
	{
		public static bool Contains(this Transform[] t_arr, Transform t)
		{
			for (int i = 0; i < t_arr.Length; i++)
			{
				if ((Object)(object)t_arr[i] == (Object)(object)t)
				{
					return true;
				}
			}
			return false;
		}

		public static float Sum(this Vector3 v)
		{
			return ((Vector3)(ref v)).get_Item(0) + ((Vector3)(ref v)).get_Item(1) + ((Vector3)(ref v)).get_Item(2);
		}

		public static bool InFrustum(this Camera cam, Vector3 point)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = cam.WorldToViewportPoint(point);
			if (val.x >= 0f && val.x <= 1f && val.y >= 0f && val.y <= 1f)
			{
				return val.z >= 0f;
			}
			return false;
		}
	}
}

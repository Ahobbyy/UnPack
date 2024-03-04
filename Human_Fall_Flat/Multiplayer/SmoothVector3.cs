using UnityEngine;

namespace Multiplayer
{
	public class SmoothVector3
	{
		private Vector3[] buffer = (Vector3[])(object)new Vector3[10];

		private int smoothingStart;

		private int smoothingCount;

		private const float tolerance = 0.01f;

		public Vector3 Next(Vector3 target)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0104: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Unknown result type (might be due to invalid IL or missing references)
			if (smoothingCount == buffer.Length)
			{
				smoothingStart = (smoothingStart + 1) % buffer.Length;
				smoothingCount--;
			}
			for (int num = smoothingCount - 1; num > 0; num--)
			{
				Vector3 val = target - buffer[(smoothingStart + num) % buffer.Length];
				if (((Vector3)(ref val)).get_sqrMagnitude() > 0.0001f)
				{
					smoothingStart = (smoothingStart + num + 1) % buffer.Length;
					smoothingCount -= num + 1;
					break;
				}
			}
			buffer[(smoothingStart + smoothingCount) % buffer.Length] = target;
			smoothingCount++;
			for (int i = 0; i < smoothingCount - 1; i++)
			{
				target = Vector3.Lerp(target, buffer[(smoothingStart + i) % buffer.Length], 1f / (float)(i + 1));
			}
			return target;
		}
	}
}

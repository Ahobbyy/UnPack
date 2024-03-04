using UnityEngine;

namespace Multiplayer
{
	public class SmoothQuaternion
	{
		private Quaternion[] buffer = (Quaternion[])(object)new Quaternion[10];

		private int smoothingStart;

		private int smoothingCount;

		private const float tolerance = 0.5f;

		public Quaternion Next(Quaternion target)
		{
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			if (smoothingCount == buffer.Length)
			{
				smoothingStart = (smoothingStart + 1) % buffer.Length;
				smoothingCount--;
			}
			for (int num = smoothingCount - 1; num > 0; num--)
			{
				if (Quaternion.Angle(target, buffer[(smoothingStart + num) % buffer.Length]) > 0.5f)
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
				target = Quaternion.Slerp(target, buffer[(smoothingStart + i) % buffer.Length], 1f / (float)(i + 1));
			}
			return target;
		}
	}
}

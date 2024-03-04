using UnityEngine;

namespace CurveExtended
{
	public class KeyframeUtil
	{
		public static Keyframe GetNew(float time, float value, TangentMode leftAndRight)
		{
			//IL_0004: Unknown result type (might be due to invalid IL or missing references)
			return GetNew(time, value, leftAndRight, leftAndRight);
		}

		public static Keyframe GetNew(float time, float value, TangentMode left, TangentMode right)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			Keyframe result = default(Keyframe);
			((Keyframe)(ref result))._002Ector(time, value);
			if (left == TangentMode.Stepped)
			{
				((Keyframe)(ref result)).set_inTangent(float.PositiveInfinity);
			}
			if (right == TangentMode.Stepped)
			{
				((Keyframe)(ref result)).set_outTangent(float.PositiveInfinity);
			}
			return result;
		}

		public static TangentMode GetKeyTangentMode(int tangentMode, int leftRight)
		{
			if (leftRight == 0)
			{
				return (TangentMode)((tangentMode & 6) >> 1);
			}
			return (TangentMode)((tangentMode & 0x18) >> 3);
		}
	}
}

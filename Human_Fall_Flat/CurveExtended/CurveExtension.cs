using UnityEngine;

namespace CurveExtended
{
	public static class CurveExtension
	{
		public static void UpdateAllLinearTangents(this AnimationCurve curve)
		{
			for (int i = 0; i < curve.get_keys().Length; i++)
			{
				UpdateTangentsFromMode(curve, i);
			}
		}

		public static void UpdateTangentsFromMode(AnimationCurve curve, int index)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			if (index >= 0 && index < curve.get_length())
			{
				Keyframe val = curve.get_Item(index);
				if (index >= 1)
				{
					((Keyframe)(ref val)).set_inTangent(CalculateLinearTangent(curve, index, index - 1));
					curve.MoveKey(index, val);
				}
				if (index + 1 < curve.get_length())
				{
					((Keyframe)(ref val)).set_outTangent(CalculateLinearTangent(curve, index, index + 1));
					curve.MoveKey(index, val);
				}
			}
		}

		private static float CalculateLinearTangent(AnimationCurve curve, int index, int toIndex)
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			Keyframe val = curve.get_Item(index);
			double num = ((Keyframe)(ref val)).get_value();
			val = curve.get_Item(toIndex);
			double num2 = num - (double)((Keyframe)(ref val)).get_value();
			val = curve.get_Item(index);
			double num3 = ((Keyframe)(ref val)).get_time();
			val = curve.get_Item(toIndex);
			return (float)(num2 / (num3 - (double)((Keyframe)(ref val)).get_time()));
		}
	}
}

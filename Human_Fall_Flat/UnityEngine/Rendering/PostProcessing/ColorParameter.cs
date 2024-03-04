using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class ColorParameter : ParameterOverride<Color>
	{
		public override void Interp(Color from, Color to, float t)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_006f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			value.r = from.r + (to.r - from.r) * t;
			value.g = from.g + (to.g - from.g) * t;
			value.b = from.b + (to.b - from.b) * t;
			value.a = from.a + (to.a - from.a) * t;
		}

		public static implicit operator Vector4(ColorParameter prop)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Color.op_Implicit(prop.value);
		}
	}
}

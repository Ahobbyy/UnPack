using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public class TMP_ColorGradient : ScriptableObject
	{
		public Color topLeft;

		public Color topRight;

		public Color bottomLeft;

		public Color bottomRight;

		private static Color k_defaultColor = Color.get_white();

		public TMP_ColorGradient()
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			topLeft = k_defaultColor;
			topRight = k_defaultColor;
			bottomLeft = k_defaultColor;
			bottomRight = k_defaultColor;
		}

		public TMP_ColorGradient(Color color)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			topLeft = color;
			topRight = color;
			bottomLeft = color;
			bottomRight = color;
		}

		public TMP_ColorGradient(Color color0, Color color1, Color color2, Color color3)
			: this()
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			topLeft = color0;
			topRight = color1;
			bottomLeft = color2;
			bottomRight = color3;
		}
	}
}

using System;
using UnityEngine;

namespace TMPro
{
	[Serializable]
	public struct VertexGradient
	{
		public Color topLeft;

		public Color topRight;

		public Color bottomLeft;

		public Color bottomRight;

		public VertexGradient(Color color)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			topLeft = color;
			topRight = color;
			bottomLeft = color;
			bottomRight = color;
		}

		public VertexGradient(Color color0, Color color1, Color color2, Color color3)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			//IL_0009: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			topLeft = color0;
			topRight = color1;
			bottomLeft = color2;
			bottomRight = color3;
		}
	}
}

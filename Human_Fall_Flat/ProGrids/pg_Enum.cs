using UnityEngine;

namespace ProGrids
{
	public static class pg_Enum
	{
		public static Vector3 InverseAxisMask(Vector3 v, Axis axis)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			switch (axis)
			{
			case Axis.X:
			case Axis.NegX:
				return Vector3.Scale(v, new Vector3(0f, 1f, 1f));
			case Axis.Y:
			case Axis.NegY:
				return Vector3.Scale(v, new Vector3(1f, 0f, 1f));
			case Axis.Z:
			case Axis.NegZ:
				return Vector3.Scale(v, new Vector3(1f, 1f, 0f));
			default:
				return v;
			}
		}

		public static Vector3 AxisMask(Vector3 v, Axis axis)
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0064: Unknown result type (might be due to invalid IL or missing references)
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			switch (axis)
			{
			case Axis.X:
			case Axis.NegX:
				return Vector3.Scale(v, new Vector3(1f, 0f, 0f));
			case Axis.Y:
			case Axis.NegY:
				return Vector3.Scale(v, new Vector3(0f, 1f, 0f));
			case Axis.Z:
			case Axis.NegZ:
				return Vector3.Scale(v, new Vector3(0f, 0f, 1f));
			default:
				return v;
			}
		}

		public static float SnapUnitValue(SnapUnit su)
		{
			return su switch
			{
				SnapUnit.Meter => 1f, 
				SnapUnit.Centimeter => 0.01f, 
				SnapUnit.Millimeter => 0.001f, 
				SnapUnit.Inch => 0.0253999867f, 
				SnapUnit.Foot => 0.3048f, 
				SnapUnit.Yard => 1.09361f, 
				SnapUnit.Parsec => 5f, 
				_ => 1f, 
			};
		}
	}
}

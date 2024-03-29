using UnityEngine;

public static class HexConverter
{
	public static int ColorToInt(Color32 color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		return (color.a << 24) + (color.r << 16) + (color.g << 8) + color.b;
	}

	public static Color32 IntToColor(int hex)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		byte b = (byte)((uint)hex & 0xFFu);
		hex >>= 8;
		byte b2 = (byte)((uint)hex & 0xFFu);
		hex >>= 8;
		byte num = (byte)(hex & 0xFF);
		hex >>= 8;
		byte b3 = (byte)((uint)hex & 0xFFu);
		return new Color32(num, b2, b, b3);
	}

	public static string ColorToHex(Color32 color)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		if (color.a == byte.MaxValue)
		{
			return color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		}
		return color.a.ToString("X2") + color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
	}

	public static Color32 HexToColor(string hex, Color defaultColor)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		if (string.IsNullOrEmpty(hex) || (hex.Length != 6 && hex.Length != 8))
		{
			return Color32.op_Implicit(defaultColor);
		}
		return HexToColor(hex);
	}

	public static Color32 HexToColor(string hex)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
		if (hex.Length == 6)
		{
			int num = (ParseChar(hex[0]) << 4) + ParseChar(hex[1]);
			int num2 = (ParseChar(hex[2]) << 4) + ParseChar(hex[3]);
			int num3 = (ParseChar(hex[4]) << 4) + ParseChar(hex[5]);
			return new Color32((byte)num, (byte)num2, (byte)num3, byte.MaxValue);
		}
		if (hex.Length == 8)
		{
			int num4 = (ParseChar(hex[0]) << 4) + ParseChar(hex[1]);
			int num5 = (ParseChar(hex[2]) << 4) + ParseChar(hex[3]);
			int num6 = (ParseChar(hex[4]) << 4) + ParseChar(hex[5]);
			int num7 = (ParseChar(hex[6]) << 4) + ParseChar(hex[7]);
			return new Color32((byte)num5, (byte)num6, (byte)num7, (byte)num4);
		}
		return default(Color32);
	}

	private static int ParseChar(char c)
	{
		switch (c)
		{
		case 'A':
		case 'a':
			return 10;
		case 'B':
		case 'b':
			return 11;
		case 'C':
		case 'c':
			return 12;
		case 'D':
		case 'd':
			return 13;
		case 'E':
		case 'e':
			return 14;
		case 'F':
		case 'f':
			return 15;
		default:
			return c - 48;
		}
	}
}

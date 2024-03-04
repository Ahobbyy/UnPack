using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	public static class TMPro_ExtensionMethods
	{
		public static string ArrayToString(this char[] chars)
		{
			string text = string.Empty;
			for (int i = 0; i < chars.Length && chars[i] != 0; i++)
			{
				text += chars[i];
			}
			return text;
		}

		public static int FindInstanceID<T>(this List<T> list, T target) where T : Object
		{
			int instanceID = ((Object)target).GetInstanceID();
			for (int i = 0; i < list.Count; i++)
			{
				if (((Object)list[i]).GetInstanceID() == instanceID)
				{
					return i;
				}
			}
			return -1;
		}

		public static bool Compare(this Color32 a, Color32 b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (a.r == b.r && a.g == b.g && a.b == b.b)
			{
				return a.a == b.a;
			}
			return false;
		}

		public static bool CompareRGB(this Color32 a, Color32 b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (a.r == b.r && a.g == b.g)
			{
				return a.b == b.b;
			}
			return false;
		}

		public static bool Compare(this Color a, Color b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			if (a.r == b.r && a.g == b.g && a.b == b.b)
			{
				return a.a == b.a;
			}
			return false;
		}

		public static bool CompareRGB(this Color a, Color b)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			if (a.r == b.r && a.g == b.g)
			{
				return a.b == b.b;
			}
			return false;
		}

		public static Color32 Multiply(this Color32 c1, Color32 c2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			byte num = (byte)((float)(int)c1.r / 255f * ((float)(int)c2.r / 255f) * 255f);
			byte b = (byte)((float)(int)c1.g / 255f * ((float)(int)c2.g / 255f) * 255f);
			byte b2 = (byte)((float)(int)c1.b / 255f * ((float)(int)c2.b / 255f) * 255f);
			byte b3 = (byte)((float)(int)c1.a / 255f * ((float)(int)c2.a / 255f) * 255f);
			return new Color32(num, b, b2, b3);
		}

		public static Color32 Tint(this Color32 c1, Color32 c2)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			byte num = (byte)((float)(int)c1.r / 255f * ((float)(int)c2.r / 255f) * 255f);
			byte b = (byte)((float)(int)c1.g / 255f * ((float)(int)c2.g / 255f) * 255f);
			byte b2 = (byte)((float)(int)c1.b / 255f * ((float)(int)c2.b / 255f) * 255f);
			byte b3 = (byte)((float)(int)c1.a / 255f * ((float)(int)c2.a / 255f) * 255f);
			return new Color32(num, b, b2, b3);
		}

		public static Color32 Tint(this Color32 c1, float tint)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			byte num = (byte)Mathf.Clamp((float)(int)c1.r / 255f * tint * 255f, 0f, 255f);
			byte b = (byte)Mathf.Clamp((float)(int)c1.g / 255f * tint * 255f, 0f, 255f);
			byte b2 = (byte)Mathf.Clamp((float)(int)c1.b / 255f * tint * 255f, 0f, 255f);
			byte b3 = (byte)Mathf.Clamp((float)(int)c1.a / 255f * tint * 255f, 0f, 255f);
			return new Color32(num, b, b2, b3);
		}

		public static bool Compare(this Vector3 v1, Vector3 v2, int accuracy)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			bool num = (int)(v1.x * (float)accuracy) == (int)(v2.x * (float)accuracy);
			bool flag = (int)(v1.y * (float)accuracy) == (int)(v2.y * (float)accuracy);
			bool flag2 = (int)(v1.z * (float)accuracy) == (int)(v2.z * (float)accuracy);
			return num && flag && flag2;
		}

		public static bool Compare(this Quaternion q1, Quaternion q2, int accuracy)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			bool num = (int)(q1.x * (float)accuracy) == (int)(q2.x * (float)accuracy);
			bool flag = (int)(q1.y * (float)accuracy) == (int)(q2.y * (float)accuracy);
			bool flag2 = (int)(q1.z * (float)accuracy) == (int)(q2.z * (float)accuracy);
			bool flag3 = (int)(q1.w * (float)accuracy) == (int)(q2.w * (float)accuracy);
			return num && flag && flag2 && flag3;
		}
	}
}

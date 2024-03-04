using ProBuilder2.Common;
using UnityEngine;

namespace ProBuilder2.Examples
{
	public class HueCube : MonoBehaviour
	{
		private pb_Object pb;

		private void Start()
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			pb = pb_ShapeGenerator.CubeGenerator(Vector3.get_one());
			int num = pb.get_sharedIndices().Length;
			Color[] array = (Color[])(object)new Color[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = HSVtoRGB((float)i / (float)num * 360f, 1f, 1f);
			}
			Color[] colors = pb.get_colors();
			for (int j = 0; j < pb.get_sharedIndices().Length; j++)
			{
				int[] array2 = pb.get_sharedIndices()[j].array;
				foreach (int num2 in array2)
				{
					colors[num2] = array[j];
				}
			}
			pb.SetColors(colors);
			pb.Refresh((RefreshMask)255);
		}

		private static Color HSVtoRGB(float h, float s, float v)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			if (s == 0f)
			{
				return new Color(v, v, v, 1f);
			}
			h /= 60f;
			int num = (int)Mathf.Floor(h);
			float num2 = h - (float)num;
			float num3 = v * (1f - s);
			float num4 = v * (1f - s * num2);
			float num5 = v * (1f - s * (1f - num2));
			float num6;
			float num7;
			float num8;
			switch (num)
			{
			case 0:
				num6 = v;
				num7 = num5;
				num8 = num3;
				break;
			case 1:
				num6 = num4;
				num7 = v;
				num8 = num3;
				break;
			case 2:
				num6 = num3;
				num7 = v;
				num8 = num5;
				break;
			case 3:
				num6 = num3;
				num7 = num4;
				num8 = v;
				break;
			case 4:
				num6 = num5;
				num7 = num3;
				num8 = v;
				break;
			default:
				num6 = v;
				num7 = num3;
				num8 = num4;
				break;
			}
			return new Color(num6, num7, num8, 1f);
		}

		public HueCube()
			: this()
		{
		}
	}
}

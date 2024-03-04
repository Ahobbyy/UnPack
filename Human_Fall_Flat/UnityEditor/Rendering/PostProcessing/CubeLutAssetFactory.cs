using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	internal static class CubeLutAssetFactory
	{
		private const int kVersion = 1;

		private const int kSize = 33;

		private static void CreateLuts()
		{
			Dump("Linear to Unity Log r" + 1, ColorUtilities.LinearToLogC);
			Dump("Unity Log to Linear r" + 1, ColorUtilities.LogCToLinear);
			Dump("sRGB to Unity Log r" + 1, (float x) => ColorUtilities.LinearToLogC(Mathf.GammaToLinearSpace(x)));
			Dump("Unity Log to sRGB r" + 1, (float x) => Mathf.LinearToGammaSpace(ColorUtilities.LogCToLinear(x)));
			Dump("Linear to sRGB r" + 1, (Func<float, float>)Mathf.LinearToGammaSpace);
			Dump("sRGB to Linear r" + 1, (Func<float, float>)Mathf.GammaToLinearSpace);
			AssetDatabase.Refresh();
		}

		private static void Dump(string title, Func<float, float> eval)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("TITLE \"{0}\"\n", title);
			stringBuilder.AppendFormat("LUT_3D_SIZE {0}\n", 33);
			stringBuilder.AppendFormat("DOMAIN_MIN {0} {0} {0}\n", 0f);
			stringBuilder.AppendFormat("DOMAIN_MAX {0} {0} {0}\n", 1f);
			for (int i = 0; i < 33; i++)
			{
				for (int j = 0; j < 33; j++)
				{
					for (int k = 0; k < 33; k++)
					{
						float num = eval((float)i / 32f);
						float num2 = eval((float)j / 32f);
						float num3 = eval((float)k / 32f);
						stringBuilder.AppendFormat("{0} {1} {2}\n", num3, num2, num);
					}
				}
			}
			string contents = stringBuilder.ToString();
			File.WriteAllText(Path.Combine(Application.get_dataPath(), $"{title}.cube"), contents);
		}
	}
}

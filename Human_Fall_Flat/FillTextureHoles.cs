using System.IO;
using UnityEditor;
using UnityEngine;

public static class FillTextureHoles
{
	[MenuItem("Assets/Fix Texture Holes")]
	public static void FixTextureHoles()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Expected O, but got Unknown
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Expected O, but got Unknown
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Expected O, but got Unknown
		//IL_0176: Unknown result type (might be due to invalid IL or missing references)
		//IL_017d: Expected O, but got Unknown
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0205: Unknown result type (might be due to invalid IL or missing references)
		Material val = new Material(Shader.Find("Unlit/FillTextureHoles"));
		int num = 0;
		Object[] objects = Selection.get_objects();
		for (int i = 0; i < objects.Length; i++)
		{
			if (AssetDatabase.GetAssetPath(objects[i]).Contains("Color"))
			{
				num++;
			}
		}
		bool flag = false;
		int num2 = 0;
		objects = Selection.get_objects();
		for (int i = 0; i < objects.Length; i++)
		{
			string assetPath = AssetDatabase.GetAssetPath(objects[i]);
			if (!assetPath.Contains("Color"))
			{
				continue;
			}
			for (int j = 0; j < 4; j++)
			{
				if (flag)
				{
					break;
				}
				flag |= EditorUtility.DisplayCancelableProgressBar("Padding file", assetPath, ((float)num2 + 0.25f * (float)j) / (float)num);
				if (!flag)
				{
					string path = Path.Combine(Path.GetDirectoryName(assetPath), Path.GetFileNameWithoutExtension(assetPath).Replace("Color", "Mask")) + ".png";
					Texture2D val2 = new Texture2D(1, 1);
					if (ImageConversion.LoadImage(val2, File.ReadAllBytes(path)))
					{
						Texture2D val3 = new Texture2D(1, 1);
						ImageConversion.LoadImage(val3, File.ReadAllBytes(assetPath));
						Debug.LogFormat("{0} {1} {2}", new object[3]
						{
							((Object)val3).get_name(),
							((Texture)val3).get_width(),
							((Texture)val3).get_height()
						});
						GL.set_sRGBWrite(true);
						val.SetTexture("_MaskTex", (Texture)(object)val2);
						Texture2D val4 = new Texture2D(((Texture)val3).get_width(), ((Texture)val3).get_height(), (TextureFormat)3, false);
						RenderTexture val5 = new RenderTexture(((Texture)val3).get_width(), ((Texture)val3).get_height(), 0, (RenderTextureFormat)0);
						val.set_mainTexture((Texture)(object)val3);
						Graphics.Blit((Texture)null, val5, val);
						RenderTexture.set_active(val5);
						val4.ReadPixels(new Rect(0f, 0f, (float)((Texture)val3).get_width(), (float)((Texture)val3).get_height()), 0, 0);
						RenderTexture.set_active((RenderTexture)null);
						File.WriteAllBytes(assetPath, ImageConversion.EncodeToPNG(val4));
						val.set_mainTexture((Texture)(object)val2);
						Graphics.Blit((Texture)null, val5, val);
						RenderTexture.set_active(val5);
						val4.ReadPixels(new Rect(0f, 0f, (float)((Texture)val3).get_width(), (float)((Texture)val3).get_height()), 0, 0);
						RenderTexture.set_active((RenderTexture)null);
						File.WriteAllBytes(path, ImageConversion.EncodeToPNG(val4));
					}
				}
			}
			num2++;
		}
		AssetDatabase.Refresh();
		Object.DestroyImmediate((Object)(object)val);
		EditorUtility.ClearProgressBar();
	}

	private static bool PadPixels(string path, Color32[] pixels, Color32[] target, int width, int height, int kernel, float progress)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < height; i++)
		{
			if (EditorUtility.DisplayCancelableProgressBar("Padding file", path, progress + 0.5f * (float)i / (float)height))
			{
				return true;
			}
			for (int j = 0; j < width; j++)
			{
				Color32 val = pixels[j + i * width];
				if (Color32.op_Implicit(val) == Color.get_black())
				{
					int num = 0;
					int num2 = 0;
					int num3 = 0;
					int num4 = 0;
					int num5 = 0;
					for (int k = -kernel; k <= kernel; k++)
					{
						for (int l = -kernel; l <= kernel; l++)
						{
							if (j + l >= 0 && j + l < width && i + k >= 0 && i + k < height)
							{
								Color32 val2 = pixels[j + l + (i + k) * width];
								if (!(Color32.op_Implicit(val2) == Color.get_black()))
								{
									num5++;
									num += val2.r;
									num2 += val2.g;
									num3 += val2.b;
									num4 += val2.a;
								}
							}
						}
					}
					if (num5 > 0)
					{
						((Color32)(ref val))._002Ector((byte)Mathf.RoundToInt((float)(num / num5)), (byte)Mathf.RoundToInt((float)(num2 / num5)), (byte)Mathf.RoundToInt((float)(num3 / num5)), (byte)Mathf.RoundToInt((float)(num4 / num5)));
					}
				}
				target[j + i * width] = val;
			}
		}
		return false;
	}
}

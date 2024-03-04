using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		public enum BlurType
		{
			StandardGauss,
			SgxGauss
		}

		[Range(0f, 2f)]
		public int downsample = 1;

		[Range(0f, 10f)]
		public float blurSize = 3f;

		[Range(1f, 4f)]
		public int blurIterations = 2;

		public BlurType blurType;

		public Shader blurShader;

		private Material blurMaterial;

		public override bool CheckResources()
		{
			CheckSupport(needDepth: false);
			blurMaterial = CheckShaderAndCreateMaterial(blurShader, blurMaterial);
			if (!isSupported)
			{
				ReportAutoDisable();
			}
			return isSupported;
		}

		public void OnDisable()
		{
			if (Object.op_Implicit((Object)(object)blurMaterial))
			{
				Object.DestroyImmediate((Object)(object)blurMaterial);
			}
		}

		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0134: Unknown result type (might be due to invalid IL or missing references)
			if (!CheckResources())
			{
				Graphics.Blit((Texture)(object)source, destination);
				return;
			}
			float num = 1f / (1f * (float)(1 << downsample));
			blurMaterial.SetVector("_Parameter", new Vector4(blurSize * num, (0f - blurSize) * num, 0f, 0f));
			((Texture)source).set_filterMode((FilterMode)1);
			int num2 = ((Texture)source).get_width() >> downsample;
			int num3 = ((Texture)source).get_height() >> downsample;
			RenderTexture val = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
			((Texture)val).set_filterMode((FilterMode)1);
			Graphics.Blit((Texture)(object)source, val, blurMaterial, 0);
			int num4 = ((blurType != 0) ? 2 : 0);
			for (int i = 0; i < blurIterations; i++)
			{
				float num5 = (float)i * 1f;
				blurMaterial.SetVector("_Parameter", new Vector4(blurSize * num + num5, (0f - blurSize) * num - num5, 0f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
				((Texture)temporary).set_filterMode((FilterMode)1);
				Graphics.Blit((Texture)(object)val, temporary, blurMaterial, 1 + num4);
				RenderTexture.ReleaseTemporary(val);
				val = temporary;
				temporary = RenderTexture.GetTemporary(num2, num3, 0, source.get_format());
				((Texture)temporary).set_filterMode((FilterMode)1);
				Graphics.Blit((Texture)(object)val, temporary, blurMaterial, 2 + num4);
				RenderTexture.ReleaseTemporary(val);
				val = temporary;
			}
			Graphics.Blit((Texture)(object)val, destination);
			RenderTexture.ReleaseTemporary(val);
		}
	}
}

using System.Linq;
using UnityEngine;

namespace TMPro
{
	public static class ShaderUtilities
	{
		public static int ID_MainTex;

		public static int ID_FaceTex;

		public static int ID_FaceColor;

		public static int ID_FaceDilate;

		public static int ID_Shininess;

		public static int ID_UnderlayColor;

		public static int ID_UnderlayOffsetX;

		public static int ID_UnderlayOffsetY;

		public static int ID_UnderlayDilate;

		public static int ID_UnderlaySoftness;

		public static int ID_WeightNormal;

		public static int ID_WeightBold;

		public static int ID_OutlineTex;

		public static int ID_OutlineWidth;

		public static int ID_OutlineSoftness;

		public static int ID_OutlineColor;

		public static int ID_GradientScale;

		public static int ID_ScaleX;

		public static int ID_ScaleY;

		public static int ID_PerspectiveFilter;

		public static int ID_TextureWidth;

		public static int ID_TextureHeight;

		public static int ID_BevelAmount;

		public static int ID_GlowColor;

		public static int ID_GlowOffset;

		public static int ID_GlowPower;

		public static int ID_GlowOuter;

		public static int ID_LightAngle;

		public static int ID_EnvMap;

		public static int ID_EnvMatrix;

		public static int ID_EnvMatrixRotation;

		public static int ID_MaskCoord;

		public static int ID_ClipRect;

		public static int ID_MaskSoftnessX;

		public static int ID_MaskSoftnessY;

		public static int ID_VertexOffsetX;

		public static int ID_VertexOffsetY;

		public static int ID_UseClipRect;

		public static int ID_StencilID;

		public static int ID_StencilOp;

		public static int ID_StencilComp;

		public static int ID_StencilReadMask;

		public static int ID_StencilWriteMask;

		public static int ID_ShaderFlags;

		public static int ID_ScaleRatio_A;

		public static int ID_ScaleRatio_B;

		public static int ID_ScaleRatio_C;

		public static string Keyword_Bevel;

		public static string Keyword_Glow;

		public static string Keyword_Underlay;

		public static string Keyword_Ratios;

		public static string Keyword_MASK_SOFT;

		public static string Keyword_MASK_HARD;

		public static string Keyword_MASK_TEX;

		public static string Keyword_Outline;

		public static string ShaderTag_ZTestMode;

		public static string ShaderTag_CullMode;

		private static float m_clamp;

		public static bool isInitialized;

		static ShaderUtilities()
		{
			Keyword_Bevel = "BEVEL_ON";
			Keyword_Glow = "GLOW_ON";
			Keyword_Underlay = "UNDERLAY_ON";
			Keyword_Ratios = "RATIOS_OFF";
			Keyword_MASK_SOFT = "MASK_SOFT";
			Keyword_MASK_HARD = "MASK_HARD";
			Keyword_MASK_TEX = "MASK_TEX";
			Keyword_Outline = "OUTLINE_ON";
			ShaderTag_ZTestMode = "unity_GUIZTestMode";
			ShaderTag_CullMode = "_CullMode";
			m_clamp = 1f;
			isInitialized = false;
			GetShaderPropertyIDs();
		}

		public static void GetShaderPropertyIDs()
		{
			if (!isInitialized)
			{
				isInitialized = true;
				ID_MainTex = Shader.PropertyToID("_MainTex");
				ID_FaceTex = Shader.PropertyToID("_FaceTex");
				ID_FaceColor = Shader.PropertyToID("_FaceColor");
				ID_FaceDilate = Shader.PropertyToID("_FaceDilate");
				ID_Shininess = Shader.PropertyToID("_FaceShininess");
				ID_UnderlayColor = Shader.PropertyToID("_UnderlayColor");
				ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
				ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
				ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
				ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");
				ID_WeightNormal = Shader.PropertyToID("_WeightNormal");
				ID_WeightBold = Shader.PropertyToID("_WeightBold");
				ID_OutlineTex = Shader.PropertyToID("_OutlineTex");
				ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth");
				ID_OutlineSoftness = Shader.PropertyToID("_OutlineSoftness");
				ID_OutlineColor = Shader.PropertyToID("_OutlineColor");
				ID_GradientScale = Shader.PropertyToID("_GradientScale");
				ID_ScaleX = Shader.PropertyToID("_ScaleX");
				ID_ScaleY = Shader.PropertyToID("_ScaleY");
				ID_PerspectiveFilter = Shader.PropertyToID("_PerspectiveFilter");
				ID_TextureWidth = Shader.PropertyToID("_TextureWidth");
				ID_TextureHeight = Shader.PropertyToID("_TextureHeight");
				ID_BevelAmount = Shader.PropertyToID("_Bevel");
				ID_LightAngle = Shader.PropertyToID("_LightAngle");
				ID_EnvMap = Shader.PropertyToID("_Cube");
				ID_EnvMatrix = Shader.PropertyToID("_EnvMatrix");
				ID_EnvMatrixRotation = Shader.PropertyToID("_EnvMatrixRotation");
				ID_GlowColor = Shader.PropertyToID("_GlowColor");
				ID_GlowOffset = Shader.PropertyToID("_GlowOffset");
				ID_GlowPower = Shader.PropertyToID("_GlowPower");
				ID_GlowOuter = Shader.PropertyToID("_GlowOuter");
				ID_MaskCoord = Shader.PropertyToID("_MaskCoord");
				ID_ClipRect = Shader.PropertyToID("_ClipRect");
				ID_UseClipRect = Shader.PropertyToID("_UseClipRect");
				ID_MaskSoftnessX = Shader.PropertyToID("_MaskSoftnessX");
				ID_MaskSoftnessY = Shader.PropertyToID("_MaskSoftnessY");
				ID_VertexOffsetX = Shader.PropertyToID("_VertexOffsetX");
				ID_VertexOffsetY = Shader.PropertyToID("_VertexOffsetY");
				ID_StencilID = Shader.PropertyToID("_Stencil");
				ID_StencilOp = Shader.PropertyToID("_StencilOp");
				ID_StencilComp = Shader.PropertyToID("_StencilComp");
				ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask");
				ID_StencilWriteMask = Shader.PropertyToID("_StencilWriteMask");
				ID_ShaderFlags = Shader.PropertyToID("_ShaderFlags");
				ID_ScaleRatio_A = Shader.PropertyToID("_ScaleRatioA");
				ID_ScaleRatio_B = Shader.PropertyToID("_ScaleRatioB");
				ID_ScaleRatio_C = Shader.PropertyToID("_ScaleRatioC");
			}
		}

		public static void UpdateShaderRatios(Material mat)
		{
			float num = 1f;
			float num2 = 1f;
			float num3 = 1f;
			bool flag = !mat.get_shaderKeywords().Contains(Keyword_Ratios);
			float @float = mat.GetFloat(ID_GradientScale);
			float float2 = mat.GetFloat(ID_FaceDilate);
			float float3 = mat.GetFloat(ID_OutlineWidth);
			float float4 = mat.GetFloat(ID_OutlineSoftness);
			float num4 = Mathf.Max(mat.GetFloat(ID_WeightNormal), mat.GetFloat(ID_WeightBold)) / 4f;
			float num5 = Mathf.Max(1f, num4 + float2 + float3 + float4);
			num = (flag ? ((@float - m_clamp) / (@float * num5)) : 1f);
			mat.SetFloat(ID_ScaleRatio_A, num);
			if (mat.HasProperty(ID_GlowOffset))
			{
				float float5 = mat.GetFloat(ID_GlowOffset);
				float float6 = mat.GetFloat(ID_GlowOuter);
				float num6 = (num4 + float2) * (@float - m_clamp);
				num5 = Mathf.Max(1f, float5 + float6);
				num2 = (flag ? (Mathf.Max(0f, @float - m_clamp - num6) / (@float * num5)) : 1f);
				mat.SetFloat(ID_ScaleRatio_B, num2);
			}
			if (mat.HasProperty(ID_UnderlayOffsetX))
			{
				float float7 = mat.GetFloat(ID_UnderlayOffsetX);
				float float8 = mat.GetFloat(ID_UnderlayOffsetY);
				float float9 = mat.GetFloat(ID_UnderlayDilate);
				float float10 = mat.GetFloat(ID_UnderlaySoftness);
				float num7 = (num4 + float2) * (@float - m_clamp);
				num5 = Mathf.Max(1f, Mathf.Max(Mathf.Abs(float7), Mathf.Abs(float8)) + float9 + float10);
				num3 = (flag ? (Mathf.Max(0f, @float - m_clamp - num7) / (@float * num5)) : 1f);
				mat.SetFloat(ID_ScaleRatio_C, num3);
			}
		}

		public static Vector4 GetFontExtent(Material material)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			return Vector4.get_zero();
		}

		public static bool IsMaskingEnabled(Material material)
		{
			if ((Object)(object)material == (Object)null || !material.HasProperty(ID_ClipRect))
			{
				return false;
			}
			if (material.get_shaderKeywords().Contains(Keyword_MASK_SOFT) || material.get_shaderKeywords().Contains(Keyword_MASK_HARD) || material.get_shaderKeywords().Contains(Keyword_MASK_TEX))
			{
				return true;
			}
			return false;
		}

		public static float GetPadding(Material material, bool enableExtraPadding, bool isBold)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0202: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_026a: Unknown result type (might be due to invalid IL or missing references)
			//IL_027e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0292: Unknown result type (might be due to invalid IL or missing references)
			//IL_02da: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0308: Unknown result type (might be due to invalid IL or missing references)
			//IL_031f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0336: Unknown result type (might be due to invalid IL or missing references)
			//IL_033c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0344: Unknown result type (might be due to invalid IL or missing references)
			//IL_034c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0359: Unknown result type (might be due to invalid IL or missing references)
			//IL_035f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0367: Unknown result type (might be due to invalid IL or missing references)
			//IL_036f: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
			if (!isInitialized)
			{
				GetShaderPropertyIDs();
			}
			if ((Object)(object)material == (Object)null)
			{
				return 0f;
			}
			int num = (enableExtraPadding ? 4 : 0);
			if (!material.HasProperty(ID_GradientScale))
			{
				return num;
			}
			Vector4 val = Vector4.get_zero();
			Vector4 zero = Vector4.get_zero();
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10 = 0f;
			UpdateShaderRatios(material);
			string[] shaderKeywords = material.get_shaderKeywords();
			if (material.HasProperty(ID_ScaleRatio_A))
			{
				num5 = material.GetFloat(ID_ScaleRatio_A);
			}
			if (material.HasProperty(ID_FaceDilate))
			{
				num2 = material.GetFloat(ID_FaceDilate) * num5;
			}
			if (material.HasProperty(ID_OutlineSoftness))
			{
				num3 = material.GetFloat(ID_OutlineSoftness) * num5;
			}
			if (material.HasProperty(ID_OutlineWidth))
			{
				num4 = material.GetFloat(ID_OutlineWidth) * num5;
			}
			num10 = num4 + num3 + num2;
			if (material.HasProperty(ID_GlowOffset) && shaderKeywords.Contains(Keyword_Glow))
			{
				if (material.HasProperty(ID_ScaleRatio_B))
				{
					num6 = material.GetFloat(ID_ScaleRatio_B);
				}
				num8 = material.GetFloat(ID_GlowOffset) * num6;
				num9 = material.GetFloat(ID_GlowOuter) * num6;
			}
			num10 = Mathf.Max(num10, num2 + num8 + num9);
			if (material.HasProperty(ID_UnderlaySoftness) && shaderKeywords.Contains(Keyword_Underlay))
			{
				if (material.HasProperty(ID_ScaleRatio_C))
				{
					num7 = material.GetFloat(ID_ScaleRatio_C);
				}
				float num11 = material.GetFloat(ID_UnderlayOffsetX) * num7;
				float num12 = material.GetFloat(ID_UnderlayOffsetY) * num7;
				float num13 = material.GetFloat(ID_UnderlayDilate) * num7;
				float num14 = material.GetFloat(ID_UnderlaySoftness) * num7;
				val.x = Mathf.Max(val.x, num2 + num13 + num14 - num11);
				val.y = Mathf.Max(val.y, num2 + num13 + num14 - num12);
				val.z = Mathf.Max(val.z, num2 + num13 + num14 + num11);
				val.w = Mathf.Max(val.w, num2 + num13 + num14 + num12);
			}
			val.x = Mathf.Max(val.x, num10);
			val.y = Mathf.Max(val.y, num10);
			val.z = Mathf.Max(val.z, num10);
			val.w = Mathf.Max(val.w, num10);
			val.x += num;
			val.y += num;
			val.z += num;
			val.w += num;
			val.x = Mathf.Min(val.x, 1f);
			val.y = Mathf.Min(val.y, 1f);
			val.z = Mathf.Min(val.z, 1f);
			val.w = Mathf.Min(val.w, 1f);
			zero.x = ((zero.x < val.x) ? val.x : zero.x);
			zero.y = ((zero.y < val.y) ? val.y : zero.y);
			zero.z = ((zero.z < val.z) ? val.z : zero.z);
			zero.w = ((zero.w < val.w) ? val.w : zero.w);
			float @float = material.GetFloat(ID_GradientScale);
			val *= @float;
			num10 = Mathf.Max(val.x, val.y);
			num10 = Mathf.Max(val.z, num10);
			num10 = Mathf.Max(val.w, num10);
			return num10 + 0.5f;
		}

		public static float GetPadding(Material[] materials, bool enableExtraPadding, bool isBold)
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_022c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0248: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_0280: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0320: Unknown result type (might be due to invalid IL or missing references)
			//IL_0337: Unknown result type (might be due to invalid IL or missing references)
			//IL_034e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0365: Unknown result type (might be due to invalid IL or missing references)
			//IL_037c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0382: Unknown result type (might be due to invalid IL or missing references)
			//IL_038a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0392: Unknown result type (might be due to invalid IL or missing references)
			//IL_039f: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_03d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_03e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_03eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0425: Unknown result type (might be due to invalid IL or missing references)
			//IL_0428: Unknown result type (might be due to invalid IL or missing references)
			//IL_042d: Unknown result type (might be due to invalid IL or missing references)
			//IL_042e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0434: Unknown result type (might be due to invalid IL or missing references)
			//IL_0441: Unknown result type (might be due to invalid IL or missing references)
			//IL_0450: Unknown result type (might be due to invalid IL or missing references)
			if (!isInitialized)
			{
				GetShaderPropertyIDs();
			}
			if (materials == null)
			{
				return 0f;
			}
			int num = (enableExtraPadding ? 4 : 0);
			if (!materials[0].HasProperty(ID_GradientScale))
			{
				return num;
			}
			Vector4 val = Vector4.get_zero();
			Vector4 zero = Vector4.get_zero();
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10 = 0f;
			for (int i = 0; i < materials.Length; i++)
			{
				UpdateShaderRatios(materials[i]);
				string[] shaderKeywords = materials[i].get_shaderKeywords();
				if (materials[i].HasProperty(ID_ScaleRatio_A))
				{
					num5 = materials[i].GetFloat(ID_ScaleRatio_A);
				}
				if (materials[i].HasProperty(ID_FaceDilate))
				{
					num2 = materials[i].GetFloat(ID_FaceDilate) * num5;
				}
				if (materials[i].HasProperty(ID_OutlineSoftness))
				{
					num3 = materials[i].GetFloat(ID_OutlineSoftness) * num5;
				}
				if (materials[i].HasProperty(ID_OutlineWidth))
				{
					num4 = materials[i].GetFloat(ID_OutlineWidth) * num5;
				}
				num10 = num4 + num3 + num2;
				if (materials[i].HasProperty(ID_GlowOffset) && shaderKeywords.Contains(Keyword_Glow))
				{
					if (materials[i].HasProperty(ID_ScaleRatio_B))
					{
						num6 = materials[i].GetFloat(ID_ScaleRatio_B);
					}
					num8 = materials[i].GetFloat(ID_GlowOffset) * num6;
					num9 = materials[i].GetFloat(ID_GlowOuter) * num6;
				}
				num10 = Mathf.Max(num10, num2 + num8 + num9);
				if (materials[i].HasProperty(ID_UnderlaySoftness) && shaderKeywords.Contains(Keyword_Underlay))
				{
					if (materials[i].HasProperty(ID_ScaleRatio_C))
					{
						num7 = materials[i].GetFloat(ID_ScaleRatio_C);
					}
					float num11 = materials[i].GetFloat(ID_UnderlayOffsetX) * num7;
					float num12 = materials[i].GetFloat(ID_UnderlayOffsetY) * num7;
					float num13 = materials[i].GetFloat(ID_UnderlayDilate) * num7;
					float num14 = materials[i].GetFloat(ID_UnderlaySoftness) * num7;
					val.x = Mathf.Max(val.x, num2 + num13 + num14 - num11);
					val.y = Mathf.Max(val.y, num2 + num13 + num14 - num12);
					val.z = Mathf.Max(val.z, num2 + num13 + num14 + num11);
					val.w = Mathf.Max(val.w, num2 + num13 + num14 + num12);
				}
				val.x = Mathf.Max(val.x, num10);
				val.y = Mathf.Max(val.y, num10);
				val.z = Mathf.Max(val.z, num10);
				val.w = Mathf.Max(val.w, num10);
				val.x += num;
				val.y += num;
				val.z += num;
				val.w += num;
				val.x = Mathf.Min(val.x, 1f);
				val.y = Mathf.Min(val.y, 1f);
				val.z = Mathf.Min(val.z, 1f);
				val.w = Mathf.Min(val.w, 1f);
				zero.x = ((zero.x < val.x) ? val.x : zero.x);
				zero.y = ((zero.y < val.y) ? val.y : zero.y);
				zero.z = ((zero.z < val.z) ? val.z : zero.z);
				zero.w = ((zero.w < val.w) ? val.w : zero.w);
			}
			float @float = materials[0].GetFloat(ID_GradientScale);
			val *= @float;
			num10 = Mathf.Max(val.x, val.y);
			num10 = Mathf.Max(val.z, num10);
			num10 = Mathf.Max(val.w, num10);
			return num10 + 0.25f;
		}
	}
}

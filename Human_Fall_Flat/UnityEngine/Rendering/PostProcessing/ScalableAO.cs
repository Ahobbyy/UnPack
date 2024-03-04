using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class ScalableAO : IAmbientOcclusionMethod
	{
		private enum Pass
		{
			OcclusionEstimationForward,
			OcclusionEstimationDeferred,
			HorizontalBlurForward,
			HorizontalBlurDeferred,
			VerticalBlur,
			CompositionForward,
			CompositionDeferred,
			DebugOverlay
		}

		private RenderTexture m_Result;

		private PropertySheet m_PropertySheet;

		private AmbientOcclusion m_Settings;

		private readonly RenderTargetIdentifier[] m_MRT = (RenderTargetIdentifier[])(object)new RenderTargetIdentifier[2]
		{
			RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)10),
			RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2)
		};

		private readonly int[] m_SampleCount = new int[5] { 4, 6, 10, 8, 12 };

		public ScalableAO(AmbientOcclusion settings)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			m_Settings = settings;
		}

		public DepthTextureMode GetCameraFlags()
		{
			return (DepthTextureMode)3;
		}

		private void DoLazyInitialization(PostProcessRenderContext context)
		{
			m_PropertySheet = context.propertySheets.Get(context.resources.shaders.scalableAO);
			bool flag = false;
			if ((Object)(object)m_Result == (Object)null || !m_Result.IsCreated())
			{
				m_Result = context.GetScreenSpaceTemporaryRT(0, (RenderTextureFormat)0, (RenderTextureReadWrite)1);
				((Object)m_Result).set_hideFlags((HideFlags)52);
				((Texture)m_Result).set_filterMode((FilterMode)1);
				flag = true;
			}
			else if (((Texture)m_Result).get_width() != context.width || ((Texture)m_Result).get_height() != context.height)
			{
				m_Result.Release();
				((Texture)m_Result).set_width(context.width);
				((Texture)m_Result).set_height(context.height);
				flag = true;
			}
			if (flag)
			{
				m_Result.Create();
			}
		}

		private void Render(PostProcessRenderContext context, CommandBuffer cmd, int occlusionSource)
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f4: Invalid comparison between Unknown and I4
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_016b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0172: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
			DoLazyInitialization(context);
			m_Settings.radius.value = Mathf.Max(m_Settings.radius.value, 0.0001f);
			bool num = m_Settings.quality.value < AmbientOcclusionQuality.High;
			float value = m_Settings.intensity.value;
			float value2 = m_Settings.radius.value;
			float num2 = (num ? 0.5f : 1f);
			float num3 = m_SampleCount[(int)m_Settings.quality.value];
			PropertySheet propertySheet = m_PropertySheet;
			propertySheet.ClearKeywords();
			propertySheet.properties.SetVector(ShaderIDs.AOParams, new Vector4(value, value2, num2, num3));
			propertySheet.properties.SetVector(ShaderIDs.AOColor, Color.op_Implicit(Color.get_white() - m_Settings.color.value));
			if ((int)context.camera.get_actualRenderingPath() == 1 && RenderSettings.get_fog())
			{
				propertySheet.EnableKeyword("APPLY_FORWARD_FOG");
				propertySheet.properties.SetVector(ShaderIDs.FogParams, Vector4.op_Implicit(new Vector3(RenderSettings.get_fogDensity(), RenderSettings.get_fogStartDistance(), RenderSettings.get_fogEndDistance())));
			}
			int num4 = ((!num) ? 1 : 2);
			int occlusionTexture = ShaderIDs.OcclusionTexture1;
			int widthOverride = context.width / num4;
			int heightOverride = context.height / num4;
			context.GetScreenSpaceTemporaryRT(cmd, occlusionTexture, 0, (RenderTextureFormat)0, (RenderTextureReadWrite)1, (FilterMode)1, widthOverride, heightOverride);
			cmd.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit(occlusionTexture), propertySheet, occlusionSource);
			int occlusionTexture2 = ShaderIDs.OcclusionTexture2;
			context.GetScreenSpaceTemporaryRT(cmd, occlusionTexture2, 0, (RenderTextureFormat)0, (RenderTextureReadWrite)1, (FilterMode)1);
			cmd.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(occlusionTexture), RenderTargetIdentifier.op_Implicit(occlusionTexture2), propertySheet, 2 + occlusionSource);
			cmd.ReleaseTemporaryRT(occlusionTexture);
			cmd.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(occlusionTexture2), RenderTargetIdentifier.op_Implicit((Texture)(object)m_Result), propertySheet, 4);
			cmd.ReleaseTemporaryRT(occlusionTexture2);
			if (context.IsDebugOverlayEnabled(DebugOverlay.AmbientOcclusion))
			{
				context.PushDebugOverlay(cmd, RenderTargetIdentifier.op_Implicit((Texture)(object)m_Result), propertySheet, 7);
			}
		}

		public void RenderAfterOpaque(PostProcessRenderContext context)
		{
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion");
			Render(context, command, 0);
			command.SetGlobalTexture(ShaderIDs.SAOcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture)(object)m_Result));
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2), m_PropertySheet, 5, (RenderBufferLoadAction)0);
			command.EndSample("Ambient Occlusion");
		}

		public void RenderAmbientOnly(PostProcessRenderContext context)
		{
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion Render");
			Render(context, command, 1);
			command.EndSample("Ambient Occlusion Render");
		}

		public void CompositeAmbientOnly(PostProcessRenderContext context)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion Composite");
			command.SetGlobalTexture(ShaderIDs.SAOcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture)(object)m_Result));
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), m_MRT, RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2), m_PropertySheet, 6);
			command.EndSample("Ambient Occlusion Composite");
		}

		public void Release()
		{
			RuntimeUtilities.Destroy((Object)(object)m_Result);
			m_Result = null;
		}
	}
}

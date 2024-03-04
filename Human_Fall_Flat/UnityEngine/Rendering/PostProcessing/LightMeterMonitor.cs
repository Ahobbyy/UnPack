using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class LightMeterMonitor : Monitor
	{
		public int width = 512;

		public int height = 256;

		public bool showCurves = true;

		internal override bool ShaderResourcesAvailable(PostProcessRenderContext context)
		{
			if (Object.op_Implicit((Object)(object)context.resources.shaders.lightMeter))
			{
				return context.resources.shaders.lightMeter.get_isSupported();
			}
			return false;
		}

		internal override void Render(PostProcessRenderContext context)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0176: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_019b: Unknown result type (might be due to invalid IL or missing references)
			CheckOutput(width, height);
			LogHistogram logHistogram = context.logHistogram;
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.lightMeter);
			propertySheet.ClearKeywords();
			propertySheet.properties.SetBuffer(ShaderIDs.HistogramBuffer, logHistogram.data);
			Vector4 histogramScaleOffsetRes = logHistogram.GetHistogramScaleOffsetRes(context);
			histogramScaleOffsetRes.z = 1f / (float)width;
			histogramScaleOffsetRes.w = 1f / (float)height;
			propertySheet.properties.SetVector(ShaderIDs.ScaleOffsetRes, histogramScaleOffsetRes);
			if ((Object)(object)context.logLut != (Object)null && showCurves)
			{
				propertySheet.EnableKeyword("COLOR_GRADING_HDR");
				propertySheet.properties.SetTexture(ShaderIDs.Lut3D, context.logLut);
			}
			AutoExposure autoExposure = context.autoExposure;
			if ((Object)(object)autoExposure != (Object)null)
			{
				float x = autoExposure.filtering.value.x;
				float y = autoExposure.filtering.value.y;
				y = Mathf.Clamp(y, 1.01f, 99f);
				x = Mathf.Clamp(x, 1f, y - 0.01f);
				Vector4 val = default(Vector4);
				((Vector4)(ref val))._002Ector(x * 0.01f, y * 0.01f, RuntimeUtilities.Exp2(autoExposure.minLuminance.value), RuntimeUtilities.Exp2(autoExposure.maxLuminance.value));
				propertySheet.EnableKeyword("AUTO_EXPOSURE");
				propertySheet.properties.SetVector(ShaderIDs.Params, val);
			}
			CommandBuffer command = context.command;
			command.BeginSample("LightMeter");
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit((Texture)(object)base.output), propertySheet, 0);
			command.EndSample("LightMeter");
		}
	}
}

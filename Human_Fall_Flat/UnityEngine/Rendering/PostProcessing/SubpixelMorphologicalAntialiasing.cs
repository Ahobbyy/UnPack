using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class SubpixelMorphologicalAntialiasing
	{
		private enum Pass
		{
			EdgeDetection = 0,
			BlendWeights = 3,
			NeighborhoodBlending = 6
		}

		public enum Quality
		{
			Low,
			Medium,
			High
		}

		[Tooltip("Lower quality is faster at the expense of visual quality (Low = ~60%, Medium = ~80%).")]
		public Quality quality = Quality.High;

		public bool IsSupported()
		{
			return !RuntimeUtilities.isSinglePassStereoEnabled;
		}

		internal void Render(PostProcessRenderContext context)
		{
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00db: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_0106: Unknown result type (might be due to invalid IL or missing references)
			//IL_010c: Unknown result type (might be due to invalid IL or missing references)
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.subpixelMorphologicalAntialiasing);
			propertySheet.properties.SetTexture("_AreaTex", (Texture)(object)context.resources.smaaLuts.area);
			propertySheet.properties.SetTexture("_SearchTex", (Texture)(object)context.resources.smaaLuts.search);
			CommandBuffer command = context.command;
			command.BeginSample("SubpixelMorphologicalAntialiasing");
			command.GetTemporaryRT(ShaderIDs.SMAA_Flip, context.width, context.height, 0, (FilterMode)1, context.sourceFormat, (RenderTextureReadWrite)1);
			command.GetTemporaryRT(ShaderIDs.SMAA_Flop, context.width, context.height, 0, (FilterMode)1, context.sourceFormat, (RenderTextureReadWrite)1);
			command.BlitFullscreenTriangle(context.source, RenderTargetIdentifier.op_Implicit(ShaderIDs.SMAA_Flip), propertySheet, (int)quality, clear: true);
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(ShaderIDs.SMAA_Flip), RenderTargetIdentifier.op_Implicit(ShaderIDs.SMAA_Flop), propertySheet, (int)(3 + quality));
			command.SetGlobalTexture("_BlendTex", RenderTargetIdentifier.op_Implicit(ShaderIDs.SMAA_Flop));
			command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 6);
			command.ReleaseTemporaryRT(ShaderIDs.SMAA_Flip);
			command.ReleaseTemporaryRT(ShaderIDs.SMAA_Flop);
			command.EndSample("SubpixelMorphologicalAntialiasing");
		}
	}
}

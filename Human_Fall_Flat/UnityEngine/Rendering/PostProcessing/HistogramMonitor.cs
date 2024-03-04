using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class HistogramMonitor : Monitor
	{
		public enum Channel
		{
			Red,
			Green,
			Blue,
			Master
		}

		public int width = 512;

		public int height = 256;

		public Channel channel = Channel.Master;

		private ComputeBuffer m_Data;

		private const int k_NumBins = 256;

		private const int k_ThreadGroupSizeX = 16;

		private const int k_ThreadGroupSizeY = 16;

		internal override void OnDisable()
		{
			base.OnDisable();
			if (m_Data != null)
			{
				m_Data.Release();
			}
			m_Data = null;
		}

		internal override bool NeedsHalfRes()
		{
			return true;
		}

		internal override bool ShaderResourcesAvailable(PostProcessRenderContext context)
		{
			return Object.op_Implicit((Object)(object)context.resources.computeShaders.gammaHistogram);
		}

		internal override void Render(PostProcessRenderContext context)
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Expected O, but got Unknown
			//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0159: Unknown result type (might be due to invalid IL or missing references)
			//IL_017b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			CheckOutput(width, height);
			if (m_Data == null)
			{
				m_Data = new ComputeBuffer(256, 4);
			}
			ComputeShader gammaHistogram = context.resources.computeShaders.gammaHistogram;
			CommandBuffer command = context.command;
			command.BeginSample("GammaHistogram");
			int num = gammaHistogram.FindKernel("KHistogramClear");
			command.SetComputeBufferParam(gammaHistogram, num, "_HistogramBuffer", m_Data);
			command.DispatchCompute(gammaHistogram, num, Mathf.CeilToInt(16f), 1, 1);
			num = gammaHistogram.FindKernel("KHistogramGather");
			Vector4 val = default(Vector4);
			((Vector4)(ref val))._002Ector((float)(context.width / 2), (float)(context.height / 2), (float)(RuntimeUtilities.isLinearColorSpace ? 1 : 0), (float)channel);
			command.SetComputeVectorParam(gammaHistogram, "_Params", val);
			command.SetComputeTextureParam(gammaHistogram, num, "_Source", RenderTargetIdentifier.op_Implicit(ShaderIDs.HalfResFinalCopy));
			command.SetComputeBufferParam(gammaHistogram, num, "_HistogramBuffer", m_Data);
			command.DispatchCompute(gammaHistogram, num, Mathf.CeilToInt(val.x / 16f), Mathf.CeilToInt(val.y / 16f), 1);
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.gammaHistogram);
			propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4((float)width, (float)height, 0f, 0f));
			propertySheet.properties.SetBuffer(ShaderIDs.HistogramBuffer, m_Data);
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit((Texture)(object)base.output), propertySheet, 0);
			command.EndSample("GammaHistogram");
		}
	}
}

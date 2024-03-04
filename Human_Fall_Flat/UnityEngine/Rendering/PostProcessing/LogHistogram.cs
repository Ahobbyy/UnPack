namespace UnityEngine.Rendering.PostProcessing
{
	public sealed class LogHistogram
	{
		public const int rangeMin = -9;

		public const int rangeMax = 9;

		private const int k_Bins = 128;

		private int m_ThreadX;

		private int m_ThreadY;

		public ComputeBuffer data { get; private set; }

		public void Generate(PostProcessRenderContext context)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00da: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			if (data == null)
			{
				m_ThreadX = 16;
				m_ThreadY = (RuntimeUtilities.isAndroidOpenGL ? 8 : 16);
				data = new ComputeBuffer(128, 4);
			}
			Vector4 histogramScaleOffsetRes = GetHistogramScaleOffsetRes(context);
			ComputeShader exposureHistogram = context.resources.computeShaders.exposureHistogram;
			CommandBuffer command = context.command;
			command.BeginSample("LogHistogram");
			int num = exposureHistogram.FindKernel("KEyeHistogramClear");
			command.SetComputeBufferParam(exposureHistogram, num, "_HistogramBuffer", data);
			command.DispatchCompute(exposureHistogram, num, Mathf.CeilToInt(128f / (float)m_ThreadX), 1, 1);
			num = exposureHistogram.FindKernel("KEyeHistogram");
			command.SetComputeBufferParam(exposureHistogram, num, "_HistogramBuffer", data);
			command.SetComputeTextureParam(exposureHistogram, num, "_Source", context.source);
			command.SetComputeVectorParam(exposureHistogram, "_ScaleOffsetRes", histogramScaleOffsetRes);
			command.DispatchCompute(exposureHistogram, num, Mathf.CeilToInt(histogramScaleOffsetRes.z / 2f / (float)m_ThreadX), Mathf.CeilToInt(histogramScaleOffsetRes.w / 2f / (float)m_ThreadY), 1);
			command.EndSample("LogHistogram");
		}

		public Vector4 GetHistogramScaleOffsetRes(PostProcessRenderContext context)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			float num = 18f;
			float num2 = 1f / num;
			float num3 = 9f * num2;
			return new Vector4(num2, num3, (float)context.width, (float)context.height);
		}

		public void Release()
		{
			if (data != null)
			{
				data.Release();
			}
			data = null;
		}
	}
}

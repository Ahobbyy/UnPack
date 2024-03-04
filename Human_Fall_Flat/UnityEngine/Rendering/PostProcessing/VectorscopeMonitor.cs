using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class VectorscopeMonitor : Monitor
	{
		public int size = 256;

		public float exposure = 0.12f;

		private ComputeBuffer m_Data;

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
			return Object.op_Implicit((Object)(object)context.resources.computeShaders.vectorscope);
		}

		internal override void Render(PostProcessRenderContext context)
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0069: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Expected O, but got Unknown
			//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0146: Unknown result type (might be due to invalid IL or missing references)
			//IL_0153: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
			CheckOutput(size, size);
			exposure = Mathf.Max(0f, exposure);
			int num = size * size;
			if (m_Data == null)
			{
				m_Data = new ComputeBuffer(num, 4);
			}
			else if (m_Data.get_count() != num)
			{
				m_Data.Release();
				m_Data = new ComputeBuffer(num, 4);
			}
			ComputeShader vectorscope = context.resources.computeShaders.vectorscope;
			CommandBuffer command = context.command;
			command.BeginSample("Vectorscope");
			Vector4 val = default(Vector4);
			((Vector4)(ref val))._002Ector((float)(context.width / 2), (float)(context.height / 2), (float)size, (float)(RuntimeUtilities.isLinearColorSpace ? 1 : 0));
			int num2 = vectorscope.FindKernel("KVectorscopeClear");
			command.SetComputeBufferParam(vectorscope, num2, "_VectorscopeBuffer", m_Data);
			command.SetComputeVectorParam(vectorscope, "_Params", val);
			command.DispatchCompute(vectorscope, num2, Mathf.CeilToInt((float)size / 16f), Mathf.CeilToInt((float)size / 16f), 1);
			num2 = vectorscope.FindKernel("KVectorscopeGather");
			command.SetComputeBufferParam(vectorscope, num2, "_VectorscopeBuffer", m_Data);
			command.SetComputeTextureParam(vectorscope, num2, "_Source", RenderTargetIdentifier.op_Implicit(ShaderIDs.HalfResFinalCopy));
			command.DispatchCompute(vectorscope, num2, Mathf.CeilToInt(val.x / 16f), Mathf.CeilToInt(val.y / 16f), 1);
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.vectorscope);
			propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4((float)size, (float)size, exposure, 0f));
			propertySheet.properties.SetBuffer(ShaderIDs.VectorscopeBuffer, m_Data);
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit((Texture)(object)base.output), propertySheet, 0);
			command.EndSample("Vectorscope");
		}
	}
}

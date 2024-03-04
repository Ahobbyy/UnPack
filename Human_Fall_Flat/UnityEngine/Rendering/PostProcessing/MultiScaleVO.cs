using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class MultiScaleVO : IAmbientOcclusionMethod
	{
		internal enum MipLevel
		{
			Original,
			L1,
			L2,
			L3,
			L4,
			L5,
			L6
		}

		private enum Pass
		{
			DepthCopy,
			CompositionDeferred,
			CompositionForward,
			DebugOverlay
		}

		private readonly float[] m_SampleThickness = new float[12]
		{
			Mathf.Sqrt(0.96f),
			Mathf.Sqrt(0.84f),
			Mathf.Sqrt(0.64f),
			Mathf.Sqrt(0.359999955f),
			Mathf.Sqrt(0.919999957f),
			Mathf.Sqrt(0.799999952f),
			Mathf.Sqrt(0.599999964f),
			Mathf.Sqrt(0.319999933f),
			Mathf.Sqrt(0.679999948f),
			Mathf.Sqrt(0.479999959f),
			Mathf.Sqrt(0.199999928f),
			Mathf.Sqrt(0.279999971f)
		};

		private readonly float[] m_InvThicknessTable = new float[12];

		private readonly float[] m_SampleWeightTable = new float[12];

		private readonly int[] m_Widths = new int[7];

		private readonly int[] m_Heights = new int[7];

		private AmbientOcclusion m_Settings;

		private PropertySheet m_PropertySheet;

		private PostProcessResources m_Resources;

		private RenderTexture m_AmbientOnlyAO;

		private readonly RenderTargetIdentifier[] m_MRT = (RenderTargetIdentifier[])(object)new RenderTargetIdentifier[2]
		{
			RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)10),
			RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2)
		};

		public MultiScaleVO(AmbientOcclusion settings)
		{
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			m_Settings = settings;
		}

		public DepthTextureMode GetCameraFlags()
		{
			return (DepthTextureMode)1;
		}

		public void SetResources(PostProcessResources resources)
		{
			m_Resources = resources;
		}

		private void Alloc(CommandBuffer cmd, int id, MipLevel size, RenderTextureFormat format, bool uav)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			RenderTextureDescriptor val = default(RenderTextureDescriptor);
			((RenderTextureDescriptor)(ref val)).set_width(m_Widths[(int)size]);
			((RenderTextureDescriptor)(ref val)).set_height(m_Heights[(int)size]);
			((RenderTextureDescriptor)(ref val)).set_colorFormat(format);
			((RenderTextureDescriptor)(ref val)).set_depthBufferBits(0);
			((RenderTextureDescriptor)(ref val)).set_volumeDepth(1);
			((RenderTextureDescriptor)(ref val)).set_autoGenerateMips(false);
			((RenderTextureDescriptor)(ref val)).set_msaaSamples(1);
			((RenderTextureDescriptor)(ref val)).set_enableRandomWrite(uav);
			((RenderTextureDescriptor)(ref val)).set_dimension((TextureDimension)2);
			((RenderTextureDescriptor)(ref val)).set_sRGB(false);
			cmd.GetTemporaryRT(id, val, (FilterMode)0);
		}

		private void AllocArray(CommandBuffer cmd, int id, MipLevel size, RenderTextureFormat format, bool uav)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_006d: Unknown result type (might be due to invalid IL or missing references)
			RenderTextureDescriptor val = default(RenderTextureDescriptor);
			((RenderTextureDescriptor)(ref val)).set_width(m_Widths[(int)size]);
			((RenderTextureDescriptor)(ref val)).set_height(m_Heights[(int)size]);
			((RenderTextureDescriptor)(ref val)).set_colorFormat(format);
			((RenderTextureDescriptor)(ref val)).set_depthBufferBits(0);
			((RenderTextureDescriptor)(ref val)).set_volumeDepth(16);
			((RenderTextureDescriptor)(ref val)).set_autoGenerateMips(false);
			((RenderTextureDescriptor)(ref val)).set_msaaSamples(1);
			((RenderTextureDescriptor)(ref val)).set_enableRandomWrite(uav);
			((RenderTextureDescriptor)(ref val)).set_dimension((TextureDimension)5);
			((RenderTextureDescriptor)(ref val)).set_sRGB(false);
			cmd.GetTemporaryRT(id, val, (FilterMode)0);
		}

		private void Release(CommandBuffer cmd, int id)
		{
			cmd.ReleaseTemporaryRT(id);
		}

		private Vector4 CalculateZBufferParams(Camera camera)
		{
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			float num = camera.get_farClipPlane() / camera.get_nearClipPlane();
			if (SystemInfo.get_usesReversedZBuffer())
			{
				return new Vector4(num - 1f, 1f, 0f, 0f);
			}
			return new Vector4(1f - num, num, 0f, 0f);
		}

		private float CalculateTanHalfFovHeight(Camera camera)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			Matrix4x4 projectionMatrix = camera.get_projectionMatrix();
			return 1f / ((Matrix4x4)(ref projectionMatrix)).get_Item(0, 0);
		}

		private Vector2 GetSize(MipLevel mip)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			return new Vector2((float)m_Widths[(int)mip], (float)m_Heights[(int)mip]);
		}

		private Vector3 GetSizeArray(MipLevel mip)
		{
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			return new Vector3((float)m_Widths[(int)mip], (float)m_Heights[(int)mip], 16f);
		}

		public void GenerateAOMap(CommandBuffer cmd, Camera camera, RenderTargetIdentifier destination, RenderTargetIdentifier? depthMap, bool invert, bool isMSAA)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0123: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_015c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0190: Unknown result type (might be due to invalid IL or missing references)
			//IL_0197: Unknown result type (might be due to invalid IL or missing references)
			//IL_019c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			m_Widths[0] = camera.get_pixelWidth() * ((!RuntimeUtilities.isSinglePassStereoEnabled) ? 1 : 2);
			m_Heights[0] = camera.get_pixelHeight();
			for (int i = 1; i < 7; i++)
			{
				int num = 1 << i;
				m_Widths[i] = (m_Widths[0] + (num - 1)) / num;
				m_Heights[i] = (m_Heights[0] + (num - 1)) / num;
			}
			PushAllocCommands(cmd, isMSAA);
			PushDownsampleCommands(cmd, camera, depthMap, isMSAA);
			float tanHalfFovH = CalculateTanHalfFovHeight(camera);
			PushRenderCommands(cmd, ShaderIDs.TiledDepth1, ShaderIDs.Occlusion1, GetSizeArray(MipLevel.L3), tanHalfFovH, isMSAA);
			PushRenderCommands(cmd, ShaderIDs.TiledDepth2, ShaderIDs.Occlusion2, GetSizeArray(MipLevel.L4), tanHalfFovH, isMSAA);
			PushRenderCommands(cmd, ShaderIDs.TiledDepth3, ShaderIDs.Occlusion3, GetSizeArray(MipLevel.L5), tanHalfFovH, isMSAA);
			PushRenderCommands(cmd, ShaderIDs.TiledDepth4, ShaderIDs.Occlusion4, GetSizeArray(MipLevel.L6), tanHalfFovH, isMSAA);
			PushUpsampleCommands(cmd, ShaderIDs.LowDepth4, ShaderIDs.Occlusion4, ShaderIDs.LowDepth3, ShaderIDs.Occlusion3, RenderTargetIdentifier.op_Implicit(ShaderIDs.Combined3), Vector2.op_Implicit(GetSize(MipLevel.L4)), GetSize(MipLevel.L3), isMSAA);
			PushUpsampleCommands(cmd, ShaderIDs.LowDepth3, ShaderIDs.Combined3, ShaderIDs.LowDepth2, ShaderIDs.Occlusion2, RenderTargetIdentifier.op_Implicit(ShaderIDs.Combined2), Vector2.op_Implicit(GetSize(MipLevel.L3)), GetSize(MipLevel.L2), isMSAA);
			PushUpsampleCommands(cmd, ShaderIDs.LowDepth2, ShaderIDs.Combined2, ShaderIDs.LowDepth1, ShaderIDs.Occlusion1, RenderTargetIdentifier.op_Implicit(ShaderIDs.Combined1), Vector2.op_Implicit(GetSize(MipLevel.L2)), GetSize(MipLevel.L1), isMSAA);
			PushUpsampleCommands(cmd, ShaderIDs.LowDepth1, ShaderIDs.Combined1, ShaderIDs.LinearDepth, null, destination, Vector2.op_Implicit(GetSize(MipLevel.L1)), GetSize(MipLevel.Original), isMSAA, invert);
			PushReleaseCommands(cmd);
		}

		private void PushAllocCommands(CommandBuffer cmd, bool isMSAA)
		{
			if (isMSAA)
			{
				Alloc(cmd, ShaderIDs.LinearDepth, MipLevel.Original, (RenderTextureFormat)13, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth1, MipLevel.L1, (RenderTextureFormat)12, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth2, MipLevel.L2, (RenderTextureFormat)12, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth3, MipLevel.L3, (RenderTextureFormat)12, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth4, MipLevel.L4, (RenderTextureFormat)12, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth1, MipLevel.L3, (RenderTextureFormat)13, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth2, MipLevel.L4, (RenderTextureFormat)13, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth3, MipLevel.L5, (RenderTextureFormat)13, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth4, MipLevel.L6, (RenderTextureFormat)13, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion1, MipLevel.L1, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion2, MipLevel.L2, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion3, MipLevel.L3, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion4, MipLevel.L4, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Combined1, MipLevel.L1, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Combined2, MipLevel.L2, (RenderTextureFormat)25, uav: true);
				Alloc(cmd, ShaderIDs.Combined3, MipLevel.L3, (RenderTextureFormat)25, uav: true);
			}
			else
			{
				Alloc(cmd, ShaderIDs.LinearDepth, MipLevel.Original, (RenderTextureFormat)15, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth1, MipLevel.L1, (RenderTextureFormat)14, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth2, MipLevel.L2, (RenderTextureFormat)14, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth3, MipLevel.L3, (RenderTextureFormat)14, uav: true);
				Alloc(cmd, ShaderIDs.LowDepth4, MipLevel.L4, (RenderTextureFormat)14, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth1, MipLevel.L3, (RenderTextureFormat)15, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth2, MipLevel.L4, (RenderTextureFormat)15, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth3, MipLevel.L5, (RenderTextureFormat)15, uav: true);
				AllocArray(cmd, ShaderIDs.TiledDepth4, MipLevel.L6, (RenderTextureFormat)15, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion1, MipLevel.L1, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion2, MipLevel.L2, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion3, MipLevel.L3, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Occlusion4, MipLevel.L4, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Combined1, MipLevel.L1, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Combined2, MipLevel.L2, (RenderTextureFormat)16, uav: true);
				Alloc(cmd, ShaderIDs.Combined3, MipLevel.L3, (RenderTextureFormat)16, uav: true);
			}
		}

		private void PushDownsampleCommands(CommandBuffer cmd, Camera camera, RenderTargetIdentifier? depthMap, bool isMSAA)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_018f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			bool flag = false;
			RenderTargetIdentifier val = default(RenderTargetIdentifier);
			if (depthMap.HasValue)
			{
				val = depthMap.Value;
			}
			else if (!RuntimeUtilities.IsResolvedDepthAvailable(camera))
			{
				Alloc(cmd, ShaderIDs.DepthCopy, MipLevel.Original, (RenderTextureFormat)14, uav: false);
				((RenderTargetIdentifier)(ref val))._002Ector(ShaderIDs.DepthCopy);
				cmd.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), val, m_PropertySheet, 0);
				flag = true;
			}
			else
			{
				val = RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)5);
			}
			ComputeShader multiScaleAODownsample = m_Resources.computeShaders.multiScaleAODownsample1;
			int num = multiScaleAODownsample.FindKernel(isMSAA ? "MultiScaleVODownsample1_MSAA" : "MultiScaleVODownsample1");
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "LinearZ", RenderTargetIdentifier.op_Implicit(ShaderIDs.LinearDepth));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS2x", RenderTargetIdentifier.op_Implicit(ShaderIDs.LowDepth1));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS4x", RenderTargetIdentifier.op_Implicit(ShaderIDs.LowDepth2));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS2xAtlas", RenderTargetIdentifier.op_Implicit(ShaderIDs.TiledDepth1));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS4xAtlas", RenderTargetIdentifier.op_Implicit(ShaderIDs.TiledDepth2));
			cmd.SetComputeVectorParam(multiScaleAODownsample, "ZBufferParams", CalculateZBufferParams(camera));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "Depth", val);
			cmd.DispatchCompute(multiScaleAODownsample, num, m_Widths[4], m_Heights[4], 1);
			if (flag)
			{
				Release(cmd, ShaderIDs.DepthCopy);
			}
			multiScaleAODownsample = m_Resources.computeShaders.multiScaleAODownsample2;
			num = (isMSAA ? multiScaleAODownsample.FindKernel("MultiScaleVODownsample2_MSAA") : multiScaleAODownsample.FindKernel("MultiScaleVODownsample2"));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS4x", RenderTargetIdentifier.op_Implicit(ShaderIDs.LowDepth2));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS8x", RenderTargetIdentifier.op_Implicit(ShaderIDs.LowDepth3));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS16x", RenderTargetIdentifier.op_Implicit(ShaderIDs.LowDepth4));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS8xAtlas", RenderTargetIdentifier.op_Implicit(ShaderIDs.TiledDepth3));
			cmd.SetComputeTextureParam(multiScaleAODownsample, num, "DS16xAtlas", RenderTargetIdentifier.op_Implicit(ShaderIDs.TiledDepth4));
			cmd.DispatchCompute(multiScaleAODownsample, num, m_Widths[6], m_Heights[6], 1);
		}

		private void PushRenderCommands(CommandBuffer cmd, int source, int destination, Vector3 sourceSize, float tanHalfFovH, bool isMSAA)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0258: Unknown result type (might be due to invalid IL or missing references)
			//IL_0265: Unknown result type (might be due to invalid IL or missing references)
			//IL_026d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0272: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0302: Unknown result type (might be due to invalid IL or missing references)
			//IL_0312: Unknown result type (might be due to invalid IL or missing references)
			float num = 2f * tanHalfFovH * 10f / sourceSize.x;
			if (RuntimeUtilities.isSinglePassStereoEnabled)
			{
				num *= 2f;
			}
			float num2 = 1f / num;
			for (int i = 0; i < 12; i++)
			{
				m_InvThicknessTable[i] = num2 / m_SampleThickness[i];
			}
			m_SampleWeightTable[0] = 4f * m_SampleThickness[0];
			m_SampleWeightTable[1] = 4f * m_SampleThickness[1];
			m_SampleWeightTable[2] = 4f * m_SampleThickness[2];
			m_SampleWeightTable[3] = 4f * m_SampleThickness[3];
			m_SampleWeightTable[4] = 4f * m_SampleThickness[4];
			m_SampleWeightTable[5] = 8f * m_SampleThickness[5];
			m_SampleWeightTable[6] = 8f * m_SampleThickness[6];
			m_SampleWeightTable[7] = 8f * m_SampleThickness[7];
			m_SampleWeightTable[8] = 4f * m_SampleThickness[8];
			m_SampleWeightTable[9] = 8f * m_SampleThickness[9];
			m_SampleWeightTable[10] = 8f * m_SampleThickness[10];
			m_SampleWeightTable[11] = 4f * m_SampleThickness[11];
			m_SampleWeightTable[0] = 0f;
			m_SampleWeightTable[2] = 0f;
			m_SampleWeightTable[5] = 0f;
			m_SampleWeightTable[7] = 0f;
			m_SampleWeightTable[9] = 0f;
			float num3 = 0f;
			float[] sampleWeightTable = m_SampleWeightTable;
			foreach (float num4 in sampleWeightTable)
			{
				num3 += num4;
			}
			for (int k = 0; k < m_SampleWeightTable.Length; k++)
			{
				m_SampleWeightTable[k] /= num3;
			}
			ComputeShader multiScaleAORender = m_Resources.computeShaders.multiScaleAORender;
			int num5 = (isMSAA ? multiScaleAORender.FindKernel("MultiScaleVORender_MSAA_interleaved") : multiScaleAORender.FindKernel("MultiScaleVORender_interleaved"));
			cmd.SetComputeFloatParams(multiScaleAORender, "gInvThicknessTable", m_InvThicknessTable);
			cmd.SetComputeFloatParams(multiScaleAORender, "gSampleWeightTable", m_SampleWeightTable);
			cmd.SetComputeVectorParam(multiScaleAORender, "gInvSliceDimension", Vector4.op_Implicit(new Vector2(1f / sourceSize.x, 1f / sourceSize.y)));
			cmd.SetComputeVectorParam(multiScaleAORender, "AdditionalParams", Vector4.op_Implicit(new Vector2(-1f / m_Settings.thicknessModifier.value, m_Settings.intensity.value)));
			cmd.SetComputeTextureParam(multiScaleAORender, num5, "DepthTex", RenderTargetIdentifier.op_Implicit(source));
			cmd.SetComputeTextureParam(multiScaleAORender, num5, "Occlusion", RenderTargetIdentifier.op_Implicit(destination));
			uint num6 = default(uint);
			uint num7 = default(uint);
			uint num8 = default(uint);
			multiScaleAORender.GetKernelThreadGroupSizes(num5, ref num6, ref num7, ref num8);
			cmd.DispatchCompute(multiScaleAORender, num5, ((int)sourceSize.x + (int)num6 - 1) / (int)num6, ((int)sourceSize.y + (int)num7 - 1) / (int)num7, ((int)sourceSize.z + (int)num8 - 1) / (int)num8);
		}

		private void PushUpsampleCommands(CommandBuffer cmd, int lowResDepth, int interleavedAO, int highResDepth, int? highResAO, RenderTargetIdentifier dest, Vector3 lowResDepthSize, Vector2 highResDepthSize, bool isMSAA, bool invert = false)
		{
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			//IL_0119: Unknown result type (might be due to invalid IL or missing references)
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_014a: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0171: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			ComputeShader multiScaleAOUpsample = m_Resources.computeShaders.multiScaleAOUpsample;
			int num = 0;
			num = (isMSAA ? multiScaleAOUpsample.FindKernel(highResAO.HasValue ? "MultiScaleVOUpSample_MSAA_blendout" : (invert ? "MultiScaleVOUpSample_MSAA_invert" : "MultiScaleVOUpSample_MSAA")) : multiScaleAOUpsample.FindKernel(highResAO.HasValue ? "MultiScaleVOUpSample_blendout" : (invert ? "MultiScaleVOUpSample_invert" : "MultiScaleVOUpSample")));
			float num2 = 1920f / lowResDepthSize.x;
			float num3 = 1f - Mathf.Pow(10f, m_Settings.blurTolerance.value) * num2;
			num3 *= num3;
			float num4 = Mathf.Pow(10f, m_Settings.upsampleTolerance.value);
			float num5 = 1f / (Mathf.Pow(10f, m_Settings.noiseFilterTolerance.value) + num4);
			cmd.SetComputeVectorParam(multiScaleAOUpsample, "InvLowResolution", Vector4.op_Implicit(new Vector2(1f / lowResDepthSize.x, 1f / lowResDepthSize.y)));
			cmd.SetComputeVectorParam(multiScaleAOUpsample, "InvHighResolution", Vector4.op_Implicit(new Vector2(1f / highResDepthSize.x, 1f / highResDepthSize.y)));
			cmd.SetComputeVectorParam(multiScaleAOUpsample, "AdditionalParams", new Vector4(num5, num2, num3, num4));
			cmd.SetComputeTextureParam(multiScaleAOUpsample, num, "LoResDB", RenderTargetIdentifier.op_Implicit(lowResDepth));
			cmd.SetComputeTextureParam(multiScaleAOUpsample, num, "HiResDB", RenderTargetIdentifier.op_Implicit(highResDepth));
			cmd.SetComputeTextureParam(multiScaleAOUpsample, num, "LoResAO1", RenderTargetIdentifier.op_Implicit(interleavedAO));
			if (highResAO.HasValue)
			{
				cmd.SetComputeTextureParam(multiScaleAOUpsample, num, "HiResAO", RenderTargetIdentifier.op_Implicit(highResAO.Value));
			}
			cmd.SetComputeTextureParam(multiScaleAOUpsample, num, "AoResult", dest);
			int num6 = ((int)highResDepthSize.x + 17) / 16;
			int num7 = ((int)highResDepthSize.y + 17) / 16;
			cmd.DispatchCompute(multiScaleAOUpsample, num, num6, num7, 1);
		}

		private void PushReleaseCommands(CommandBuffer cmd)
		{
			Release(cmd, ShaderIDs.LinearDepth);
			Release(cmd, ShaderIDs.LowDepth1);
			Release(cmd, ShaderIDs.LowDepth2);
			Release(cmd, ShaderIDs.LowDepth3);
			Release(cmd, ShaderIDs.LowDepth4);
			Release(cmd, ShaderIDs.TiledDepth1);
			Release(cmd, ShaderIDs.TiledDepth2);
			Release(cmd, ShaderIDs.TiledDepth3);
			Release(cmd, ShaderIDs.TiledDepth4);
			Release(cmd, ShaderIDs.Occlusion1);
			Release(cmd, ShaderIDs.Occlusion2);
			Release(cmd, ShaderIDs.Occlusion3);
			Release(cmd, ShaderIDs.Occlusion4);
			Release(cmd, ShaderIDs.Combined1);
			Release(cmd, ShaderIDs.Combined2);
			Release(cmd, ShaderIDs.Combined3);
		}

		private void PreparePropertySheet(PostProcessRenderContext context)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			PropertySheet propertySheet = context.propertySheets.Get(m_Resources.shaders.multiScaleAO);
			propertySheet.ClearKeywords();
			propertySheet.properties.SetVector(ShaderIDs.AOColor, Color.op_Implicit(Color.get_white() - m_Settings.color.value));
			m_PropertySheet = propertySheet;
		}

		private void CheckAOTexture(PostProcessRenderContext context)
		{
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Expected O, but got Unknown
			if ((Object)(object)m_AmbientOnlyAO == (Object)null || !m_AmbientOnlyAO.IsCreated() || ((Texture)m_AmbientOnlyAO).get_width() != context.width || ((Texture)m_AmbientOnlyAO).get_height() != context.height)
			{
				RuntimeUtilities.Destroy((Object)(object)m_AmbientOnlyAO);
				RenderTexture val = new RenderTexture(context.width, context.height, 0, (RenderTextureFormat)16, (RenderTextureReadWrite)1);
				((Object)val).set_hideFlags((HideFlags)52);
				((Texture)val).set_filterMode((FilterMode)0);
				val.set_enableRandomWrite(true);
				m_AmbientOnlyAO = val;
				m_AmbientOnlyAO.Create();
			}
		}

		private void PushDebug(PostProcessRenderContext context)
		{
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (context.IsDebugOverlayEnabled(DebugOverlay.AmbientOcclusion))
			{
				context.PushDebugOverlay(context.command, RenderTargetIdentifier.op_Implicit((Texture)(object)m_AmbientOnlyAO), m_PropertySheet, 3);
			}
		}

		public void RenderAfterOpaque(PostProcessRenderContext context)
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Invalid comparison between Unknown and I4
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_008d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion");
			SetResources(context.resources);
			PreparePropertySheet(context);
			CheckAOTexture(context);
			if ((int)context.camera.get_actualRenderingPath() == 1 && RenderSettings.get_fog())
			{
				m_PropertySheet.EnableKeyword("APPLY_FORWARD_FOG");
				m_PropertySheet.properties.SetVector(ShaderIDs.FogParams, Vector4.op_Implicit(new Vector3(RenderSettings.get_fogDensity(), RenderSettings.get_fogStartDistance(), RenderSettings.get_fogEndDistance())));
			}
			GenerateAOMap(command, context.camera, RenderTargetIdentifier.op_Implicit((Texture)(object)m_AmbientOnlyAO), null, invert: false, isMSAA: false);
			PushDebug(context);
			command.SetGlobalTexture(ShaderIDs.MSVOcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture)(object)m_AmbientOnlyAO));
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2), m_PropertySheet, 2, (RenderBufferLoadAction)0);
			command.EndSample("Ambient Occlusion");
		}

		public void RenderAmbientOnly(PostProcessRenderContext context)
		{
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion Render");
			SetResources(context.resources);
			PreparePropertySheet(context);
			CheckAOTexture(context);
			GenerateAOMap(command, context.camera, RenderTargetIdentifier.op_Implicit((Texture)(object)m_AmbientOnlyAO), null, invert: false, isMSAA: false);
			PushDebug(context);
			command.EndSample("Ambient Occlusion Render");
		}

		public void CompositeAmbientOnly(PostProcessRenderContext context)
		{
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("Ambient Occlusion Composite");
			command.SetGlobalTexture(ShaderIDs.MSVOcclusionTexture, RenderTargetIdentifier.op_Implicit((Texture)(object)m_AmbientOnlyAO));
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)0), m_MRT, RenderTargetIdentifier.op_Implicit((BuiltinRenderTextureType)2), m_PropertySheet, 1);
			command.EndSample("Ambient Occlusion Composite");
		}

		public void Release()
		{
			RuntimeUtilities.Destroy((Object)(object)m_AmbientOnlyAO);
			m_AmbientOnlyAO = null;
		}
	}
}

namespace UnityEngine.Rendering.PostProcessing
{
	public sealed class BloomRenderer : PostProcessEffectRenderer<Bloom>
	{
		private enum Pass
		{
			Prefilter13,
			Prefilter4,
			Downsample13,
			Downsample4,
			UpsampleTent,
			UpsampleBox,
			DebugOverlayThreshold,
			DebugOverlayTent,
			DebugOverlayBox
		}

		private struct Level
		{
			internal int down;

			internal int up;
		}

		private Level[] m_Pyramid;

		private const int k_MaxPyramidSize = 16;

		public override void Init()
		{
			m_Pyramid = new Level[16];
			for (int i = 0; i < 16; i++)
			{
				m_Pyramid[i] = new Level
				{
					down = Shader.PropertyToID("_BloomMipDown" + i),
					up = Shader.PropertyToID("_BloomMipUp" + i)
				};
			}
		}

		public override void Render(PostProcessRenderContext context)
		{
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_023a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024b: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_025f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0264: Unknown result type (might be due to invalid IL or missing references)
			//IL_02db: Unknown result type (might be due to invalid IL or missing references)
			//IL_02e8: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ef: Unknown result type (might be due to invalid IL or missing references)
			//IL_031e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0323: Unknown result type (might be due to invalid IL or missing references)
			//IL_0373: Unknown result type (might be due to invalid IL or missing references)
			//IL_0395: Unknown result type (might be due to invalid IL or missing references)
			//IL_039c: Unknown result type (might be due to invalid IL or missing references)
			//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_03ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0461: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_04e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_04f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0508: Unknown result type (might be due to invalid IL or missing references)
			//IL_052a: Unknown result type (might be due to invalid IL or missing references)
			CommandBuffer command = context.command;
			command.BeginSample("BloomPyramid");
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.bloom);
			propertySheet.properties.SetTexture(ShaderIDs.AutoExposureTex, context.autoExposureTexture);
			float num = Mathf.Clamp((float)base.settings.anamorphicRatio, -1f, 1f);
			float num2 = ((num < 0f) ? (0f - num) : 0f);
			float num3 = ((num > 0f) ? num : 0f);
			int num4 = Mathf.FloorToInt((float)context.screenWidth / (2f - num2));
			int num5 = Mathf.FloorToInt((float)context.screenHeight / (2f - num3));
			float num6 = Mathf.Log((float)Mathf.Max(num4, num5), 2f) + Mathf.Min(base.settings.diffusion.value, 10f) - 10f;
			int num7 = Mathf.FloorToInt(num6);
			int num8 = Mathf.Clamp(num7, 1, 16);
			float num9 = 0.5f + num6 - (float)num7;
			propertySheet.properties.SetFloat(ShaderIDs.SampleScale, num9);
			float num10 = Mathf.GammaToLinearSpace(base.settings.threshold.value);
			float num11 = num10 * base.settings.softKnee.value + 1E-05f;
			Vector4 val = default(Vector4);
			((Vector4)(ref val))._002Ector(num10, num10 - num11, num11 * 2f, 0.25f / num11);
			propertySheet.properties.SetVector(ShaderIDs.Threshold, val);
			float num12 = Mathf.GammaToLinearSpace(base.settings.clamp.value);
			propertySheet.properties.SetVector(ShaderIDs.Params, new Vector4(num12, 0f, 0f, 0f));
			int num13 = (base.settings.fastMode ? 1 : 0);
			RenderTargetIdentifier source = context.source;
			for (int i = 0; i < num8; i++)
			{
				int down = m_Pyramid[i].down;
				int up = m_Pyramid[i].up;
				int pass = ((i == 0) ? num13 : (2 + num13));
				context.GetScreenSpaceTemporaryRT(command, down, 0, context.sourceFormat, (RenderTextureReadWrite)0, (FilterMode)1, num4, num5);
				context.GetScreenSpaceTemporaryRT(command, up, 0, context.sourceFormat, (RenderTextureReadWrite)0, (FilterMode)1, num4, num5);
				command.BlitFullscreenTriangle(source, RenderTargetIdentifier.op_Implicit(down), propertySheet, pass);
				source = RenderTargetIdentifier.op_Implicit(down);
				num4 = Mathf.Max(num4 / 2, 1);
				num5 = Mathf.Max(num5 / 2, 1);
			}
			int num14 = m_Pyramid[num8 - 1].down;
			for (int num15 = num8 - 2; num15 >= 0; num15--)
			{
				int down2 = m_Pyramid[num15].down;
				int up2 = m_Pyramid[num15].up;
				command.SetGlobalTexture(ShaderIDs.BloomTex, RenderTargetIdentifier.op_Implicit(down2));
				command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(num14), RenderTargetIdentifier.op_Implicit(up2), propertySheet, 4 + num13);
				num14 = up2;
			}
			Color linear = ((Color)(ref base.settings.color.value)).get_linear();
			float num16 = RuntimeUtilities.Exp2(base.settings.intensity.value / 10f) - 1f;
			Vector4 val2 = default(Vector4);
			((Vector4)(ref val2))._002Ector(num9, num16, base.settings.dirtIntensity.value, (float)num8);
			if (context.IsDebugOverlayEnabled(DebugOverlay.BloomThreshold))
			{
				context.PushDebugOverlay(command, context.source, propertySheet, 6);
			}
			else if (context.IsDebugOverlayEnabled(DebugOverlay.BloomBuffer))
			{
				propertySheet.properties.SetVector(ShaderIDs.ColorIntensity, new Vector4(linear.r, linear.g, linear.b, num16));
				context.PushDebugOverlay(command, RenderTargetIdentifier.op_Implicit(m_Pyramid[0].up), propertySheet, 7 + num13);
			}
			Texture val3 = (Texture)(((Object)(object)base.settings.dirtTexture.value == (Object)null) ? ((object)RuntimeUtilities.blackTexture) : ((object)base.settings.dirtTexture.value));
			float num17 = (float)val3.get_width() / (float)val3.get_height();
			float num18 = (float)context.screenWidth / (float)context.screenHeight;
			Vector4 val4 = default(Vector4);
			((Vector4)(ref val4))._002Ector(1f, 1f, 0f, 0f);
			if (num17 > num18)
			{
				val4.x = num18 / num17;
				val4.z = (1f - val4.x) * 0.5f;
			}
			else if (num18 > num17)
			{
				val4.y = num17 / num18;
				val4.w = (1f - val4.y) * 0.5f;
			}
			PropertySheet uberSheet = context.uberSheet;
			if ((bool)base.settings.fastMode)
			{
				uberSheet.EnableKeyword("BLOOM_LOW");
			}
			else
			{
				uberSheet.EnableKeyword("BLOOM");
			}
			uberSheet.properties.SetVector(ShaderIDs.Bloom_DirtTileOffset, val4);
			uberSheet.properties.SetVector(ShaderIDs.Bloom_Settings, val2);
			uberSheet.properties.SetColor(ShaderIDs.Bloom_Color, linear);
			uberSheet.properties.SetTexture(ShaderIDs.Bloom_DirtTex, val3);
			command.SetGlobalTexture(ShaderIDs.BloomTex, RenderTargetIdentifier.op_Implicit(num14));
			for (int j = 0; j < num8; j++)
			{
				if (m_Pyramid[j].down != num14)
				{
					command.ReleaseTemporaryRT(m_Pyramid[j].down);
				}
				if (m_Pyramid[j].up != num14)
				{
					command.ReleaseTemporaryRT(m_Pyramid[j].up);
				}
			}
			command.EndSample("BloomPyramid");
			context.bloomBufferNameID = num14;
		}
	}
}

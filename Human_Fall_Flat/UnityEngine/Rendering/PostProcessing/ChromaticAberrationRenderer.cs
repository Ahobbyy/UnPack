namespace UnityEngine.Rendering.PostProcessing
{
	public sealed class ChromaticAberrationRenderer : PostProcessEffectRenderer<ChromaticAberration>
	{
		private Texture2D m_InternalSpectralLut;

		public override void Render(PostProcessRenderContext context)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
			Texture val = base.settings.spectralLut.value;
			if ((Object)(object)val == (Object)null)
			{
				if ((Object)(object)m_InternalSpectralLut == (Object)null)
				{
					Texture2D val2 = new Texture2D(3, 1, (TextureFormat)3, false);
					((Object)val2).set_name("Chromatic Aberration Spectrum Lookup");
					((Texture)val2).set_filterMode((FilterMode)1);
					((Texture)val2).set_wrapMode((TextureWrapMode)1);
					((Texture)val2).set_anisoLevel(0);
					((Object)val2).set_hideFlags((HideFlags)52);
					m_InternalSpectralLut = val2;
					m_InternalSpectralLut.SetPixels((Color[])(object)new Color[3]
					{
						new Color(1f, 0f, 0f),
						new Color(0f, 1f, 0f),
						new Color(0f, 0f, 1f)
					});
					m_InternalSpectralLut.Apply();
				}
				val = (Texture)(object)m_InternalSpectralLut;
			}
			PropertySheet uberSheet = context.uberSheet;
			uberSheet.EnableKeyword(base.settings.fastMode ? "CHROMATIC_ABERRATION_LOW" : "CHROMATIC_ABERRATION");
			uberSheet.properties.SetFloat(ShaderIDs.ChromaticAberration_Amount, (float)base.settings.intensity * 0.05f);
			uberSheet.properties.SetTexture(ShaderIDs.ChromaticAberration_SpectralLut, val);
		}

		public override void Release()
		{
			RuntimeUtilities.Destroy((Object)(object)m_InternalSpectralLut);
			m_InternalSpectralLut = null;
		}
	}
}

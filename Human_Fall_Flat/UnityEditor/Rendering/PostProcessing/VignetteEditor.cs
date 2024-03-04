using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(Vignette))]
	public sealed class VignetteEditor : PostProcessEffectEditor<Vignette>
	{
		private SerializedParameterOverride m_Mode;

		private SerializedParameterOverride m_Color;

		private SerializedParameterOverride m_Center;

		private SerializedParameterOverride m_Intensity;

		private SerializedParameterOverride m_Smoothness;

		private SerializedParameterOverride m_Roundness;

		private SerializedParameterOverride m_Rounded;

		private SerializedParameterOverride m_Mask;

		private SerializedParameterOverride m_Opacity;

		public override void OnEnable()
		{
			m_Mode = FindParameterOverride((Vignette x) => x.mode);
			m_Color = FindParameterOverride((Vignette x) => x.color);
			m_Center = FindParameterOverride((Vignette x) => x.center);
			m_Intensity = FindParameterOverride((Vignette x) => x.intensity);
			m_Smoothness = FindParameterOverride((Vignette x) => x.smoothness);
			m_Roundness = FindParameterOverride((Vignette x) => x.roundness);
			m_Rounded = FindParameterOverride((Vignette x) => x.rounded);
			m_Mask = FindParameterOverride((Vignette x) => x.mask);
			m_Opacity = FindParameterOverride((Vignette x) => x.opacity);
		}

		public override void OnInspectorGUI()
		{
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Invalid comparison between Unknown and I4
			//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0101: Unknown result type (might be due to invalid IL or missing references)
			//IL_0107: Invalid comparison between Unknown and I4
			PropertyField(m_Mode);
			PropertyField(m_Color);
			if (m_Mode.value.get_intValue() == 0)
			{
				PropertyField(m_Center);
				PropertyField(m_Intensity);
				PropertyField(m_Smoothness);
				PropertyField(m_Roundness);
				PropertyField(m_Rounded);
				return;
			}
			PropertyField(m_Mask);
			Texture value = (base.target as Vignette).mask.value;
			if ((Object)(object)value != (Object)null)
			{
				TextureImporter importer = default(TextureImporter);
				ref TextureImporter val = ref importer;
				AssetImporter atPath = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath((Object)(object)value));
				val = (TextureImporter)(object)((atPath is TextureImporter) ? atPath : null);
				if ((Object)(object)importer != (Object)null && (importer.get_anisoLevel() != 0 || importer.get_mipmapEnabled() || (int)importer.get_alphaSource() != 2 || (int)importer.get_textureCompression() != 0 || (int)importer.get_wrapMode() != 1))
				{
					EditorUtilities.DrawFixMeBox("Invalid mask import settings.", delegate
					{
						SetMaskImportSettings(importer);
					});
				}
			}
			PropertyField(m_Opacity);
		}

		private void SetMaskImportSettings(TextureImporter importer)
		{
			importer.set_textureType((TextureImporterType)10);
			importer.set_alphaSource((TextureImporterAlphaSource)2);
			importer.set_textureCompression((TextureImporterCompression)0);
			importer.set_anisoLevel(0);
			importer.set_mipmapEnabled(false);
			importer.set_wrapMode((TextureWrapMode)1);
			((AssetImporter)importer).SaveAndReimport();
			AssetDatabase.Refresh();
		}
	}
}

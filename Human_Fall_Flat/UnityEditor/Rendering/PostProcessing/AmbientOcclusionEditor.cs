using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(AmbientOcclusion))]
	public sealed class AmbientOcclusionEditor : PostProcessEffectEditor<AmbientOcclusion>
	{
		private SerializedParameterOverride m_Mode;

		private SerializedParameterOverride m_Intensity;

		private SerializedParameterOverride m_Color;

		private SerializedParameterOverride m_AmbientOnly;

		private SerializedParameterOverride m_ThicknessModifier;

		private SerializedParameterOverride m_DirectLightingStrength;

		private SerializedParameterOverride m_Quality;

		private SerializedParameterOverride m_Radius;

		public override void OnEnable()
		{
			m_Mode = FindParameterOverride((AmbientOcclusion x) => x.mode);
			m_Intensity = FindParameterOverride((AmbientOcclusion x) => x.intensity);
			m_Color = FindParameterOverride((AmbientOcclusion x) => x.color);
			m_AmbientOnly = FindParameterOverride((AmbientOcclusion x) => x.ambientOnly);
			m_ThicknessModifier = FindParameterOverride((AmbientOcclusion x) => x.thicknessModifier);
			m_DirectLightingStrength = FindParameterOverride((AmbientOcclusion x) => x.directLightingStrength);
			m_Quality = FindParameterOverride((AmbientOcclusion x) => x.quality);
			m_Radius = FindParameterOverride((AmbientOcclusion x) => x.radius);
		}

		public override void OnInspectorGUI()
		{
			PropertyField(m_Mode);
			int intValue = m_Mode.value.get_intValue();
			if (RuntimeUtilities.scriptableRenderPipelineActive && intValue == 0)
			{
				EditorGUILayout.HelpBox("Scalable ambient obscurance doesn't work with scriptable render pipelines.", (MessageType)2);
				return;
			}
			PropertyField(m_Intensity);
			switch (intValue)
			{
			case 0:
				PropertyField(m_Radius);
				PropertyField(m_Quality);
				break;
			case 1:
				if (!SystemInfo.get_supportsComputeShaders())
				{
					EditorGUILayout.HelpBox("Multi-scale volumetric obscurance requires compute shader support.", (MessageType)2);
				}
				PropertyField(m_ThicknessModifier);
				if (RuntimeUtilities.scriptableRenderPipelineActive)
				{
					PropertyField(m_DirectLightingStrength);
				}
				break;
			}
			PropertyField(m_Color);
			PropertyField(m_AmbientOnly);
			if (m_AmbientOnly.overrideState.get_boolValue() && m_AmbientOnly.value.get_boolValue() && !RuntimeUtilities.scriptableRenderPipelineActive)
			{
				EditorGUILayout.HelpBox("Ambient-only only works with cameras rendering in Deferred + HDR", (MessageType)1);
			}
		}
	}
}

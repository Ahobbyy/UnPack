using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(ScreenSpaceReflections))]
	public sealed class ScreenSpaceReflectionsEditor : PostProcessEffectEditor<ScreenSpaceReflections>
	{
		private SerializedParameterOverride m_Preset;

		private SerializedParameterOverride m_MaximumIterationCount;

		private SerializedParameterOverride m_Thickness;

		private SerializedParameterOverride m_Resolution;

		private SerializedParameterOverride m_MaximumMarchDistance;

		private SerializedParameterOverride m_DistanceFade;

		private SerializedParameterOverride m_Vignette;

		public override void OnEnable()
		{
			m_Preset = FindParameterOverride((ScreenSpaceReflections x) => x.preset);
			m_MaximumIterationCount = FindParameterOverride((ScreenSpaceReflections x) => x.maximumIterationCount);
			m_Thickness = FindParameterOverride((ScreenSpaceReflections x) => x.thickness);
			m_Resolution = FindParameterOverride((ScreenSpaceReflections x) => x.resolution);
			m_MaximumMarchDistance = FindParameterOverride((ScreenSpaceReflections x) => x.maximumMarchDistance);
			m_DistanceFade = FindParameterOverride((ScreenSpaceReflections x) => x.distanceFade);
			m_Vignette = FindParameterOverride((ScreenSpaceReflections x) => x.vignette);
		}

		public override void OnInspectorGUI()
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Invalid comparison between Unknown and I4
			if (RuntimeUtilities.scriptableRenderPipelineActive)
			{
				EditorGUILayout.HelpBox("This effect doesn't work with scriptable render pipelines yet.", (MessageType)2);
				return;
			}
			if ((Object)(object)Camera.get_main() != (Object)null && (int)Camera.get_main().get_actualRenderingPath() != 3)
			{
				EditorGUILayout.HelpBox("This effect only works with the deferred rendering path.", (MessageType)2);
			}
			if (!SystemInfo.get_supportsComputeShaders())
			{
				EditorGUILayout.HelpBox("This effect requires compute shader support.", (MessageType)2);
			}
			PropertyField(m_Preset);
			if (m_Preset.value.get_intValue() == 7)
			{
				PropertyField(m_MaximumIterationCount);
				PropertyField(m_Thickness);
				PropertyField(m_Resolution);
				EditorGUILayout.Space();
			}
			PropertyField(m_MaximumMarchDistance);
			PropertyField(m_DistanceFade);
			PropertyField(m_Vignette);
		}
	}
}

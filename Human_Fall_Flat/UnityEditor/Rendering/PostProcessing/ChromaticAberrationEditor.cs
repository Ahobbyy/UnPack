using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(ChromaticAberration))]
	public sealed class ChromaticAberrationEditor : PostProcessEffectEditor<ChromaticAberration>
	{
		private SerializedParameterOverride m_SpectralLut;

		private SerializedParameterOverride m_Intensity;

		private SerializedParameterOverride m_FastMode;

		public override void OnEnable()
		{
			m_SpectralLut = FindParameterOverride((ChromaticAberration x) => x.spectralLut);
			m_Intensity = FindParameterOverride((ChromaticAberration x) => x.intensity);
			m_FastMode = FindParameterOverride((ChromaticAberration x) => x.fastMode);
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			PropertyField(m_SpectralLut);
			PropertyField(m_Intensity);
			PropertyField(m_FastMode);
			if (m_FastMode.overrideState.get_boolValue() && !m_FastMode.value.get_boolValue() && EditorUtilities.isTargetingConsolesOrMobiles)
			{
				EditorGUILayout.HelpBox("For performance reasons it is recommended to use Fast Mode on mobile and console platforms.", (MessageType)2);
			}
		}
	}
}

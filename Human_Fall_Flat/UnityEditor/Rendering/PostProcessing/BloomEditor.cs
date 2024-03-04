using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(Bloom))]
	public sealed class BloomEditor : PostProcessEffectEditor<Bloom>
	{
		private SerializedParameterOverride m_Intensity;

		private SerializedParameterOverride m_Threshold;

		private SerializedParameterOverride m_SoftKnee;

		private SerializedParameterOverride m_Clamp;

		private SerializedParameterOverride m_Diffusion;

		private SerializedParameterOverride m_AnamorphicRatio;

		private SerializedParameterOverride m_Color;

		private SerializedParameterOverride m_FastMode;

		private SerializedParameterOverride m_DirtTexture;

		private SerializedParameterOverride m_DirtIntensity;

		public override void OnEnable()
		{
			m_Intensity = FindParameterOverride((Bloom x) => x.intensity);
			m_Threshold = FindParameterOverride((Bloom x) => x.threshold);
			m_SoftKnee = FindParameterOverride((Bloom x) => x.softKnee);
			m_Clamp = FindParameterOverride((Bloom x) => x.clamp);
			m_Diffusion = FindParameterOverride((Bloom x) => x.diffusion);
			m_AnamorphicRatio = FindParameterOverride((Bloom x) => x.anamorphicRatio);
			m_Color = FindParameterOverride((Bloom x) => x.color);
			m_FastMode = FindParameterOverride((Bloom x) => x.fastMode);
			m_DirtTexture = FindParameterOverride((Bloom x) => x.dirtTexture);
			m_DirtIntensity = FindParameterOverride((Bloom x) => x.dirtIntensity);
		}

		public override void OnInspectorGUI()
		{
			EditorUtilities.DrawHeaderLabel("Bloom");
			PropertyField(m_Intensity);
			PropertyField(m_Threshold);
			PropertyField(m_SoftKnee);
			PropertyField(m_Clamp);
			PropertyField(m_Diffusion);
			PropertyField(m_AnamorphicRatio);
			PropertyField(m_Color);
			PropertyField(m_FastMode);
			if (m_FastMode.overrideState.get_boolValue() && !m_FastMode.value.get_boolValue() && EditorUtilities.isTargetingConsolesOrMobiles)
			{
				EditorGUILayout.HelpBox("For performance reasons it is recommended to use Fast Mode on mobile and console platforms.", (MessageType)2);
			}
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("Dirtiness");
			PropertyField(m_DirtTexture);
			PropertyField(m_DirtIntensity);
			if (RuntimeUtilities.isVREnabled && ((m_DirtIntensity.overrideState.get_boolValue() && m_DirtIntensity.value.get_floatValue() > 0f) || (m_DirtTexture.overrideState.get_boolValue() && m_DirtTexture.value.get_objectReferenceValue() != (Object)null)))
			{
				EditorGUILayout.HelpBox("Using a dirt texture in VR is not recommended.", (MessageType)2);
			}
		}
	}
}

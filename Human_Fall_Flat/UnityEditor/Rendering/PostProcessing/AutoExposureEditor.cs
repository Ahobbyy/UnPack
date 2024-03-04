using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(AutoExposure))]
	public sealed class AutoExposureEditor : PostProcessEffectEditor<AutoExposure>
	{
		private SerializedParameterOverride m_Filtering;

		private SerializedParameterOverride m_MinLuminance;

		private SerializedParameterOverride m_MaxLuminance;

		private SerializedParameterOverride m_KeyValue;

		private SerializedParameterOverride m_EyeAdaptation;

		private SerializedParameterOverride m_SpeedUp;

		private SerializedParameterOverride m_SpeedDown;

		public override void OnEnable()
		{
			m_Filtering = FindParameterOverride((AutoExposure x) => x.filtering);
			m_MinLuminance = FindParameterOverride((AutoExposure x) => x.minLuminance);
			m_MaxLuminance = FindParameterOverride((AutoExposure x) => x.maxLuminance);
			m_KeyValue = FindParameterOverride((AutoExposure x) => x.keyValue);
			m_EyeAdaptation = FindParameterOverride((AutoExposure x) => x.eyeAdaptation);
			m_SpeedUp = FindParameterOverride((AutoExposure x) => x.speedUp);
			m_SpeedDown = FindParameterOverride((AutoExposure x) => x.speedDown);
		}

		public override void OnInspectorGUI()
		{
			if (!SystemInfo.get_supportsComputeShaders())
			{
				EditorGUILayout.HelpBox("Auto exposure requires compute shader support.", (MessageType)2);
			}
			EditorUtilities.DrawHeaderLabel("Exposure");
			PropertyField(m_Filtering);
			PropertyField(m_MinLuminance);
			PropertyField(m_MaxLuminance);
			float floatValue = m_MinLuminance.value.get_floatValue();
			float floatValue2 = m_MaxLuminance.value.get_floatValue();
			m_MinLuminance.value.set_floatValue(Mathf.Min(floatValue, floatValue2));
			m_MaxLuminance.value.set_floatValue(Mathf.Max(floatValue, floatValue2));
			PropertyField(m_KeyValue);
			EditorGUILayout.Space();
			EditorUtilities.DrawHeaderLabel("Adaptation");
			PropertyField(m_EyeAdaptation);
			if (m_EyeAdaptation.value.get_intValue() == 0)
			{
				PropertyField(m_SpeedUp);
				PropertyField(m_SpeedDown);
			}
		}
	}
}

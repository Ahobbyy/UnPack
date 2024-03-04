using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[CustomEditor(typeof(PostProcessDebug))]
	public sealed class PostProcessDebugEditor : BaseEditor<PostProcessDebug>
	{
		private SerializedProperty m_PostProcessLayer;

		private SerializedProperty m_LightMeterEnabled;

		private SerializedProperty m_HistogramEnabled;

		private SerializedProperty m_WaveformEnabled;

		private SerializedProperty m_VectorscopeEnabled;

		private SerializedProperty m_Overlay;

		private SerializedObject m_LayerObject;

		private SerializedProperty m_LightMeterShowCurves;

		private SerializedProperty m_HistogramChannel;

		private SerializedProperty m_WaveformExposure;

		private SerializedProperty m_VectorscopeExposure;

		private SerializedProperty m_LinearDepth;

		private SerializedProperty m_MotionColorIntensity;

		private SerializedProperty m_MotionGridSize;

		private SerializedProperty m_ColorBlindness;

		private SerializedProperty m_ColorBlindnessStrength;

		private void OnEnable()
		{
			m_PostProcessLayer = FindProperty((PostProcessDebug x) => x.postProcessLayer);
			m_LightMeterEnabled = FindProperty((PostProcessDebug x) => x.lightMeter);
			m_HistogramEnabled = FindProperty((PostProcessDebug x) => x.histogram);
			m_WaveformEnabled = FindProperty((PostProcessDebug x) => x.waveform);
			m_VectorscopeEnabled = FindProperty((PostProcessDebug x) => x.vectorscope);
			m_Overlay = FindProperty((PostProcessDebug x) => x.debugOverlay);
			if (m_PostProcessLayer.get_objectReferenceValue() != (Object)null)
			{
				RebuildProperties();
			}
		}

		private void RebuildProperties()
		{
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Expected O, but got Unknown
			if (!(m_PostProcessLayer.get_objectReferenceValue() == (Object)null))
			{
				m_LayerObject = new SerializedObject((Object)(object)base.m_Target.postProcessLayer);
				m_LightMeterShowCurves = m_LayerObject.FindProperty("debugLayer.lightMeter.showCurves");
				m_HistogramChannel = m_LayerObject.FindProperty("debugLayer.histogram.channel");
				m_WaveformExposure = m_LayerObject.FindProperty("debugLayer.waveform.exposure");
				m_VectorscopeExposure = m_LayerObject.FindProperty("debugLayer.vectorscope.exposure");
				m_LinearDepth = m_LayerObject.FindProperty("debugLayer.overlaySettings.linearDepth");
				m_MotionColorIntensity = m_LayerObject.FindProperty("debugLayer.overlaySettings.motionColorIntensity");
				m_MotionGridSize = m_LayerObject.FindProperty("debugLayer.overlaySettings.motionGridSize");
				m_ColorBlindness = m_LayerObject.FindProperty("debugLayer.overlaySettings.colorBlindnessType");
				m_ColorBlindnessStrength = m_LayerObject.FindProperty("debugLayer.overlaySettings.colorBlindnessStrength");
			}
		}

		public override void OnInspectorGUI()
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Expected O, but got Unknown
			((Editor)this).get_serializedObject().Update();
			ChangeCheckScope val = new ChangeCheckScope();
			try
			{
				EditorGUILayout.PropertyField(m_PostProcessLayer, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				((Editor)this).get_serializedObject().ApplyModifiedProperties();
				((Editor)this).get_serializedObject().Update();
				if (val.get_changed())
				{
					RebuildProperties();
				}
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
			if (m_PostProcessLayer.get_objectReferenceValue() != (Object)null)
			{
				m_LayerObject.Update();
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(EditorUtilities.GetContent("Overlay"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				EditorGUILayout.PropertyField(m_Overlay, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				DoOverlayGUI(DebugOverlay.Depth, m_LinearDepth);
				DoOverlayGUI(DebugOverlay.MotionVectors, m_MotionColorIntensity, m_MotionGridSize);
				DoOverlayGUI(DebugOverlay.ColorBlindnessSimulation, m_ColorBlindness, m_ColorBlindnessStrength);
				if (m_Overlay.get_intValue() == 4 && base.m_Target.postProcessLayer.stopNaNPropagation)
				{
					EditorGUILayout.HelpBox("Disable \"Stop NaN Propagation\" in the Post-process layer or NaNs will be overwritten!", (MessageType)2);
				}
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				EditorGUILayout.Space();
				EditorGUILayout.LabelField(EditorUtilities.GetContent("Monitors"), EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				DoMonitorGUI(EditorUtilities.GetContent("Light Meter"), m_LightMeterEnabled, m_LightMeterShowCurves);
				DoMonitorGUI(EditorUtilities.GetContent("Histogram"), m_HistogramEnabled, m_HistogramChannel);
				DoMonitorGUI(EditorUtilities.GetContent("Waveform"), m_WaveformEnabled, m_WaveformExposure);
				DoMonitorGUI(EditorUtilities.GetContent("Vectoscope"), m_VectorscopeEnabled, m_VectorscopeExposure);
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				m_LayerObject.ApplyModifiedProperties();
			}
			((Editor)this).get_serializedObject().ApplyModifiedProperties();
		}

		private void DoMonitorGUI(GUIContent content, SerializedProperty prop, params SerializedProperty[] settings)
		{
			EditorGUILayout.PropertyField(prop, content, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (settings != null && settings.Length != 0 && prop.get_boolValue())
			{
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				for (int i = 0; i < settings.Length; i++)
				{
					EditorGUILayout.PropertyField(settings[i], (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
			}
		}

		private void DoOverlayGUI(DebugOverlay overlay, params SerializedProperty[] settings)
		{
			if (m_Overlay.get_intValue() == (int)overlay && settings != null && settings.Length != 0)
			{
				for (int i = 0; i < settings.Length; i++)
				{
					EditorGUILayout.PropertyField(settings[i], (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
			}
		}
	}
}

using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(DepthOfField))]
	public sealed class DepthOfFieldEditor : PostProcessEffectEditor<DepthOfField>
	{
		private SerializedParameterOverride m_FocusDistance;

		private SerializedParameterOverride m_Aperture;

		private SerializedParameterOverride m_FocalLength;

		private SerializedParameterOverride m_KernelSize;

		public override void OnEnable()
		{
			m_FocusDistance = FindParameterOverride((DepthOfField x) => x.focusDistance);
			m_Aperture = FindParameterOverride((DepthOfField x) => x.aperture);
			m_FocalLength = FindParameterOverride((DepthOfField x) => x.focalLength);
			m_KernelSize = FindParameterOverride((DepthOfField x) => x.kernelSize);
		}

		public override void OnInspectorGUI()
		{
			if (SystemInfo.get_graphicsShaderLevel() < 35)
			{
				EditorGUILayout.HelpBox("Depth Of Field is only supported on the following platforms:\nDX11+, OpenGL 3.2+, OpenGL ES 3+, Metal, Vulkan, PS4/XB1 consoles.", (MessageType)2);
			}
			PropertyField(m_FocusDistance);
			PropertyField(m_Aperture);
			PropertyField(m_FocalLength);
			PropertyField(m_KernelSize);
		}
	}
}

using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	[PostProcessEditor(typeof(LensDistortion))]
	public sealed class LensDistortionEditor : DefaultPostProcessEffectEditor
	{
		public override void OnInspectorGUI()
		{
			if (RuntimeUtilities.isVREnabled)
			{
				EditorGUILayout.HelpBox("Lens Distortion is automatically disabled when VR is enabled.", (MessageType)2);
			}
			base.OnInspectorGUI();
		}
	}
}

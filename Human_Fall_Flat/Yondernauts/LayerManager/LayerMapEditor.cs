using UnityEditor;
using UnityEngine;

namespace Yondernauts.LayerManager
{
	[CustomEditor(typeof(LayerMap))]
	public class LayerMapEditor : Editor
	{
		private int m_IndexInput;

		private int m_IndexOutput;

		private int m_MaskInput;

		private int m_MaskOutput;

		private const float label_width = 20f;

		public override void OnInspectorGUI()
		{
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			LayerMap layerMap = ((Editor)this).get_serializedObject().get_targetObject() as LayerMap;
			EditorGUILayout.LabelField("Transform Layer Index", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			m_IndexInput = EditorGUILayout.IntField(m_IndexInput, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			m_IndexInput = Mathf.Clamp(m_IndexInput, 0, 31);
			m_IndexOutput = layerMap.TransformLayer(m_IndexInput);
			EditorGUILayout.LabelField("->", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) });
			EditorGUILayout.IntField(m_IndexOutput, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
			EditorGUILayout.LabelField("Transform Layer Mask", EditorStyles.get_boldLabel(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			m_MaskInput = EditorGUILayout.IntField(m_MaskInput, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			m_MaskOutput = layerMap.TransformMask(m_MaskInput);
			EditorGUILayout.LabelField("->", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.Width(20f) });
			EditorGUILayout.IntField(m_MaskOutput, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.EndHorizontal();
		}

		public LayerMapEditor()
			: this()
		{
		}
	}
}

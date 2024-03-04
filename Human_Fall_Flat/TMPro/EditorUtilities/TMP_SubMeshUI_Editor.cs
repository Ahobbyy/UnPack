using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_SubMeshUI))]
	[CanEditMultipleObjects]
	public class TMP_SubMeshUI_Editor : Editor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct m_foldout
		{
			public static bool fontSettings = true;
		}

		private static string[] uiStateLabel = new string[2] { "\t- <i>Click to expand</i> -", "\t- <i>Click to collapse</i> -" };

		private SerializedProperty fontAsset_prop;

		private SerializedProperty spriteAsset_prop;

		private TMP_SubMeshUI m_SubMeshComponent;

		private CanvasRenderer m_canvasRenderer;

		private Editor m_materialEditor;

		private Material m_targetMaterial;

		public void OnEnable()
		{
			TMP_UIStyleManager.GetUIStyles();
			fontAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontAsset");
			spriteAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_spriteAsset");
			m_SubMeshComponent = ((Editor)this).get_target() as TMP_SubMeshUI;
			m_canvasRenderer = m_SubMeshComponent.canvasRenderer;
			if ((Object)(object)m_canvasRenderer != (Object)null && (Object)(object)m_canvasRenderer.GetMaterial() != (Object)null)
			{
				m_materialEditor = Editor.CreateEditor((Object)(object)m_canvasRenderer.GetMaterial());
				m_targetMaterial = m_canvasRenderer.GetMaterial();
			}
		}

		public void OnDisable()
		{
			if ((Object)(object)m_materialEditor != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_materialEditor);
			}
		}

		public override void OnInspectorGUI()
		{
			if (GUILayout.Button("<b>SUB OBJECT SETTINGS</b>" + (m_foldout.fontSettings ? uiStateLabel[1] : uiStateLabel[0]), TMP_UIStyleManager.Section_Label, (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				m_foldout.fontSettings = !m_foldout.fontSettings;
			}
			if (m_foldout.fontSettings)
			{
				GUI.set_enabled(false);
				EditorGUILayout.PropertyField(fontAsset_prop, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				EditorGUILayout.PropertyField(spriteAsset_prop, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUI.set_enabled(true);
			}
			EditorGUILayout.Space();
			if ((Object)(object)m_canvasRenderer != (Object)null && (Object)(object)m_canvasRenderer.GetMaterial() != (Object)null)
			{
				Material material = m_canvasRenderer.GetMaterial();
				if ((Object)(object)material != (Object)(object)m_targetMaterial)
				{
					m_targetMaterial = material;
					Object.DestroyImmediate((Object)(object)m_materialEditor);
				}
				if ((Object)(object)m_materialEditor == (Object)null)
				{
					m_materialEditor = Editor.CreateEditor((Object)(object)material);
				}
				m_materialEditor.DrawHeader();
				m_materialEditor.OnInspectorGUI();
			}
		}

		public TMP_SubMeshUI_Editor()
			: this()
		{
		}
	}
}

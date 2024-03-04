using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

namespace TMPro.EditorUtilities
{
	[CustomEditor(typeof(TMP_SubMesh))]
	[CanEditMultipleObjects]
	public class TMP_SubMesh_Editor : Editor
	{
		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct m_foldout
		{
			public static bool fontSettings = true;
		}

		private static string[] uiStateLabel = new string[2] { "\t- <i>Click to expand</i> -", "\t- <i>Click to collapse</i> -" };

		private SerializedProperty fontAsset_prop;

		private SerializedProperty spriteAsset_prop;

		private TMP_SubMesh m_SubMeshComponent;

		private Renderer m_Renderer;

		public void OnEnable()
		{
			TMP_UIStyleManager.GetUIStyles();
			fontAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_fontAsset");
			spriteAsset_prop = ((Editor)this).get_serializedObject().FindProperty("m_spriteAsset");
			m_SubMeshComponent = ((Editor)this).get_target() as TMP_SubMesh;
			m_Renderer = m_SubMeshComponent.renderer;
		}

		public override void OnInspectorGUI()
		{
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			EditorGUI.set_indentLevel(0);
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
			EditorGUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.PrefixLabel("Sorting Layer");
			EditorGUI.BeginChangeCheck();
			string[] sortingLayerNames = SortingLayerHelper.sortingLayerNames;
			string sortingLayerNameFromID = SortingLayerHelper.GetSortingLayerNameFromID(m_Renderer.get_sortingLayerID());
			int num = Array.IndexOf(sortingLayerNames, sortingLayerNameFromID);
			EditorGUIUtility.set_fieldWidth(0f);
			int num2 = EditorGUILayout.Popup(string.Empty, num, sortingLayerNames, (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MinWidth(80f) });
			if (num2 != num)
			{
				m_Renderer.set_sortingLayerID(SortingLayerHelper.GetSortingLayerIDForIndex(num2));
			}
			EditorGUIUtility.set_labelWidth(40f);
			EditorGUIUtility.set_fieldWidth(80f);
			int num3 = EditorGUILayout.IntField("Order", m_Renderer.get_sortingOrder(), (GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (num3 != m_Renderer.get_sortingOrder())
			{
				m_Renderer.set_sortingOrder(num3);
			}
			EditorGUILayout.EndHorizontal();
		}

		public TMP_SubMesh_Editor()
			: this()
		{
		}
	}
}

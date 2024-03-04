using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	internal class SingleBundleInspector
	{
		private Editor m_Editor;

		private Rect m_Position;

		[SerializeField]
		private Vector2 m_ScrollPosition;

		private AssetBundleInspectTab m_assetBundleInspectTab;

		private AssetBundleInspectTab.InspectTabData m_inspectTabData;

		internal static string currentPath { get; set; }

		internal SingleBundleInspector()
		{
		}

		internal void SetBundle(AssetBundle bundle, string path = "", AssetBundleInspectTab.InspectTabData inspectTabData = null, AssetBundleInspectTab assetBundleInspectTab = null)
		{
			currentPath = path;
			m_inspectTabData = inspectTabData;
			m_assetBundleInspectTab = assetBundleInspectTab;
			m_Editor = null;
			if ((Object)(object)bundle != (Object)null)
			{
				m_Editor = Editor.CreateEditor((Object)(object)bundle);
			}
		}

		internal void OnGUI(Rect pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			m_Position = pos;
			DrawBundleData();
		}

		private void DrawBundleData()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_005f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_0074: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Expected O, but got Unknown
			//IL_009a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_Editor != (Object)null)
			{
				GUILayout.BeginArea(m_Position);
				m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				m_Editor.OnInspectorGUI();
				EditorGUILayout.EndScrollView();
				GUILayout.EndArea();
			}
			else
			{
				if (string.IsNullOrEmpty(currentPath))
				{
					return;
				}
				GUIStyle val = new GUIStyle(GUI.get_skin().get_label());
				val.set_alignment((TextAnchor)4);
				val.set_wordWrap(true);
				GUI.Label(m_Position, new GUIContent("Invalid bundle selected"), val);
				if (m_inspectTabData == null || !GUI.Button(new Rect(new Vector2(((Rect)(ref m_Position)).get_position().x + ((Rect)(ref m_Position)).get_width() / 2f - 37.5f, ((Rect)(ref m_Position)).get_position().y + ((Rect)(ref m_Position)).get_height() / 2f + 15f), new Vector2(75f, 30f)), "Ignore file"))
				{
					return;
				}
				AssetBundleInspectTab.InspectTabData.BundleFolderData bundleFolderData = m_inspectTabData.FolderDataContainingFilePath(currentPath);
				if (bundleFolderData != null)
				{
					if (!bundleFolderData.ignoredFiles.Contains(currentPath))
					{
						bundleFolderData.ignoredFiles.Add(currentPath);
					}
					if (m_assetBundleInspectTab != null)
					{
						m_assetBundleInspectTab.RefreshBundles();
					}
				}
			}
		}
	}
}

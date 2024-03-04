using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	[CustomEditor(typeof(AssetBundle))]
	internal class AssetBundleEditor : Editor
	{
		internal bool pathFoldout;

		internal bool advancedFoldout;

		public override void OnInspectorGUI()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Expected O, but got Unknown
			//IL_0086: Unknown result type (might be due to invalid IL or missing references)
			//IL_0097: Expected O, but got Unknown
			//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ba: Expected O, but got Unknown
			Object target = ((Editor)this).get_target();
			AssetBundle val = (AssetBundle)(object)((target is AssetBundle) ? target : null);
			DisabledScope val2 = default(DisabledScope);
			((DisabledScope)(ref val2))._002Ector(true);
			try
			{
				GUIStyle val3 = new GUIStyle(GUI.get_skin().GetStyle("Label"));
				val3.set_alignment((TextAnchor)0);
				GUILayout.Label(new GUIContent("Name: " + ((Object)val).get_name()), val3, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				long num = -1L;
				if (!string.IsNullOrEmpty(SingleBundleInspector.currentPath) && File.Exists(SingleBundleInspector.currentPath))
				{
					num = new FileInfo(SingleBundleInspector.currentPath).Length;
				}
				if (num < 0)
				{
					GUILayout.Label(new GUIContent("Size: unknown"), val3, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
				else
				{
					GUILayout.Label(new GUIContent("Size: " + EditorUtility.FormatBytes(num)), val3, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				}
				string[] allAssetNames = val.GetAllAssetNames();
				pathFoldout = EditorGUILayout.Foldout(pathFoldout, "Source Asset Paths");
				if (pathFoldout)
				{
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
					string[] array = allAssetNames;
					for (int i = 0; i < array.Length; i++)
					{
						EditorGUILayout.LabelField(array[i], (GUILayoutOption[])(object)new GUILayoutOption[0]);
					}
					EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
				}
				advancedFoldout = EditorGUILayout.Foldout(advancedFoldout, "Advanced Data");
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val2)).Dispose();
			}
			if (advancedFoldout)
			{
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() + 1);
				((Editor)this).OnInspectorGUI();
				EditorGUI.set_indentLevel(EditorGUI.get_indentLevel() - 1);
			}
		}

		public AssetBundleEditor()
			: this()
		{
		}
	}
}

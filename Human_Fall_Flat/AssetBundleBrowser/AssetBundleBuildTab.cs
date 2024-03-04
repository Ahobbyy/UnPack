using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using AssetBundleBrowser.AssetBundleDataSource;
using AssetBundleBrowser.AssetBundleModel;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser
{
	[Serializable]
	internal class AssetBundleBuildTab
	{
		private class ToggleData
		{
			internal bool state;

			internal GUIContent content;

			internal BuildAssetBundleOptions option;

			internal ToggleData(bool s, string title, string tooltip, List<string> onToggles, BuildAssetBundleOptions opt = 0)
			{
				//IL_0023: Unknown result type (might be due to invalid IL or missing references)
				//IL_002d: Expected O, but got Unknown
				//IL_002e: Unknown result type (might be due to invalid IL or missing references)
				//IL_0030: Unknown result type (might be due to invalid IL or missing references)
				if (onToggles.Contains(title))
				{
					state = true;
				}
				else
				{
					state = s;
				}
				content = new GUIContent(title, tooltip);
				option = opt;
			}
		}

		internal enum CompressOptions
		{
			Uncompressed,
			StandardCompression,
			ChunkBasedCompression
		}

		internal enum ValidBuildTarget
		{
			StandaloneOSXUniversal = 2,
			StandaloneOSXIntel = 4,
			StandaloneWindows = 5,
			WebPlayer = 6,
			WebPlayerStreamed = 7,
			iOS = 9,
			PS3 = 10,
			XBOX360 = 11,
			Android = 13,
			StandaloneLinux = 17,
			StandaloneWindows64 = 19,
			WebGL = 20,
			WSAPlayer = 21,
			StandaloneLinux64 = 24,
			StandaloneLinuxUniversal = 25,
			WP8Player = 26,
			StandaloneOSXIntel64 = 27,
			BlackBerry = 28,
			Tizen = 29,
			PSP2 = 30,
			PS4 = 0x1F,
			PSM = 0x20,
			XboxOne = 33,
			SamsungTV = 34,
			N3DS = 35,
			WiiU = 36,
			tvOS = 37,
			Switch = 38
		}

		[Serializable]
		internal class BuildTabData
		{
			internal List<string> m_OnToggles;

			internal ValidBuildTarget m_BuildTarget = ValidBuildTarget.StandaloneWindows;

			internal CompressOptions m_Compression = CompressOptions.StandardCompression;

			internal string m_OutputPath = string.Empty;

			internal bool m_UseDefaultPath = true;
		}

		private const string k_BuildPrefPrefix = "ABBBuild:";

		private string m_streamingPath = "Assets/StreamingAssets";

		[SerializeField]
		private bool m_AdvancedSettings;

		[SerializeField]
		private Vector2 m_ScrollPosition;

		private AssetBundleInspectTab m_InspectTab;

		[SerializeField]
		private BuildTabData m_UserData;

		private List<ToggleData> m_ToggleData;

		private ToggleData m_ForceRebuild;

		private ToggleData m_CopyToStreaming;

		private GUIContent m_TargetContent;

		private GUIContent m_CompressionContent;

		private GUIContent[] m_CompressionOptions = (GUIContent[])(object)new GUIContent[3]
		{
			new GUIContent("No Compression"),
			new GUIContent("Standard Compression (LZMA)"),
			new GUIContent("Chunk Based Compression (LZ4)")
		};

		private int[] m_CompressionValues = new int[3] { 0, 1, 2 };

		internal AssetBundleBuildTab()
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Expected O, but got Unknown
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0039: Expected O, but got Unknown
			m_AdvancedSettings = false;
			m_UserData = new BuildTabData();
			m_UserData.m_OnToggles = new List<string>();
			m_UserData.m_UseDefaultPath = true;
		}

		internal void OnDisable()
		{
			string path = Path.GetFullPath(".").Replace("\\", "/") + "/Library/AssetBundleBrowserBuild.dat";
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Create(path);
			binaryFormatter.Serialize(fileStream, m_UserData);
			fileStream.Close();
		}

		internal void OnEnable(EditorWindow parent)
		{
			//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Expected O, but got Unknown
			//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_01eb: Expected O, but got Unknown
			m_InspectTab = (parent as AssetBundleBrowserMain).m_InspectTab;
			string fullPath = Path.GetFullPath(".");
			fullPath = fullPath.Replace("\\", "/");
			fullPath += "/Library/AssetBundleBrowserBuild.dat";
			if (File.Exists(fullPath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(fullPath, FileMode.Open);
				BuildTabData buildTabData = binaryFormatter.Deserialize(fileStream) as BuildTabData;
				if (buildTabData != null)
				{
					m_UserData = buildTabData;
				}
				fileStream.Close();
			}
			m_ToggleData = new List<ToggleData>();
			m_ToggleData.Add(new ToggleData(s: false, "Exclude Type Information", "Do not include type information within the asset bundle (don't write type tree).", m_UserData.m_OnToggles, (BuildAssetBundleOptions)8));
			m_ToggleData.Add(new ToggleData(s: false, "Force Rebuild", "Force rebuild the asset bundles", m_UserData.m_OnToggles, (BuildAssetBundleOptions)32));
			m_ToggleData.Add(new ToggleData(s: false, "Ignore Type Tree Changes", "Ignore the type tree changes when doing the incremental build check.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)64));
			m_ToggleData.Add(new ToggleData(s: false, "Append Hash", "Append the hash to the assetBundle name.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)128));
			m_ToggleData.Add(new ToggleData(s: false, "Strict Mode", "Do not allow the build to succeed if any errors are reporting during it.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)512));
			m_ToggleData.Add(new ToggleData(s: false, "Dry Run Build", "Do a dry run build.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)1024));
			m_ForceRebuild = new ToggleData(s: false, "Clear Folders", "Will wipe out all contents of build directory as well as StreamingAssets/AssetBundles if you are choosing to copy build there.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)0);
			m_CopyToStreaming = new ToggleData(s: false, "Copy to StreamingAssets", "After build completes, will copy all build content to " + m_streamingPath + " for use in stand-alone player.", m_UserData.m_OnToggles, (BuildAssetBundleOptions)0);
			m_TargetContent = new GUIContent("Build Target", "Choose target platform to build for.");
			m_CompressionContent = new GUIContent("Compression", "Choose no compress, standard (LZMA), or chunk based (LZ4)");
			if (m_UserData.m_UseDefaultPath)
			{
				ResetPathToDefault();
			}
		}

		internal void OnGUI()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Expected O, but got Unknown
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Expected O, but got Unknown
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_046e: Expected O, but got Unknown
			//IL_046e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0478: Expected O, but got Unknown
			m_ScrollPosition = EditorGUILayout.BeginScrollView(m_ScrollPosition, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			bool flag = false;
			GUIStyle val = new GUIStyle(GUI.get_skin().GetStyle("Label"));
			val.set_alignment((TextAnchor)1);
			GUILayout.Label(new GUIContent("Example build setup"), val, (GUILayoutOption[])(object)new GUILayoutOption[0]);
			EditorGUILayout.Space();
			GUILayout.BeginVertical((GUILayoutOption[])(object)new GUILayoutOption[0]);
			DisabledScope val2 = default(DisabledScope);
			((DisabledScope)(ref val2))._002Ector(!Model.DataSource.CanSpecifyBuildTarget);
			try
			{
				ValidBuildTarget validBuildTarget = (ValidBuildTarget)(object)EditorGUILayout.EnumPopup(m_TargetContent, (Enum)m_UserData.m_BuildTarget, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (validBuildTarget != m_UserData.m_BuildTarget)
				{
					m_UserData.m_BuildTarget = validBuildTarget;
					if (m_UserData.m_UseDefaultPath)
					{
						m_UserData.m_OutputPath = "AssetBundles/";
						m_UserData.m_OutputPath += m_UserData.m_BuildTarget;
					}
				}
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val2)).Dispose();
			}
			((DisabledScope)(ref val2))._002Ector(!Model.DataSource.CanSpecifyBuildOutputDirectory);
			try
			{
				EditorGUILayout.Space();
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				string text = EditorGUILayout.TextField("Output Path", m_UserData.m_OutputPath, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (!string.IsNullOrEmpty(text) && text != m_UserData.m_OutputPath)
				{
					m_UserData.m_UseDefaultPath = false;
					m_UserData.m_OutputPath = text;
				}
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
				GUILayout.FlexibleSpace();
				if (GUILayout.Button("Browse", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(75f) }))
				{
					BrowseForFolder();
				}
				if (GUILayout.Button("Reset", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(75f) }))
				{
					ResetPathToDefault();
				}
				GUILayout.EndHorizontal();
				EditorGUILayout.Space();
				flag = GUILayout.Toggle(m_ForceRebuild.state, m_ForceRebuild.content, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (flag != m_ForceRebuild.state)
				{
					if (flag)
					{
						m_UserData.m_OnToggles.Add(m_ForceRebuild.content.get_text());
					}
					else
					{
						m_UserData.m_OnToggles.Remove(m_ForceRebuild.content.get_text());
					}
					m_ForceRebuild.state = flag;
				}
				flag = GUILayout.Toggle(m_CopyToStreaming.state, m_CopyToStreaming.content, (GUILayoutOption[])(object)new GUILayoutOption[0]);
				if (flag != m_CopyToStreaming.state)
				{
					if (flag)
					{
						m_UserData.m_OnToggles.Add(m_CopyToStreaming.content.get_text());
					}
					else
					{
						m_UserData.m_OnToggles.Remove(m_CopyToStreaming.content.get_text());
					}
					m_CopyToStreaming.state = flag;
				}
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val2)).Dispose();
			}
			((DisabledScope)(ref val2))._002Ector(!Model.DataSource.CanSpecifyBuildOptions);
			try
			{
				EditorGUILayout.Space();
				m_AdvancedSettings = EditorGUILayout.Foldout(m_AdvancedSettings, "Advanced Settings");
				if (m_AdvancedSettings)
				{
					int indentLevel = EditorGUI.get_indentLevel();
					EditorGUI.set_indentLevel(1);
					CompressOptions compressOptions = (CompressOptions)EditorGUILayout.IntPopup(m_CompressionContent, (int)m_UserData.m_Compression, m_CompressionOptions, m_CompressionValues, (GUILayoutOption[])(object)new GUILayoutOption[0]);
					if (compressOptions != m_UserData.m_Compression)
					{
						m_UserData.m_Compression = compressOptions;
					}
					foreach (ToggleData toggleDatum in m_ToggleData)
					{
						flag = EditorGUILayout.ToggleLeft(toggleDatum.content, toggleDatum.state, (GUILayoutOption[])(object)new GUILayoutOption[0]);
						if (flag != toggleDatum.state)
						{
							if (flag)
							{
								m_UserData.m_OnToggles.Add(toggleDatum.content.get_text());
							}
							else
							{
								m_UserData.m_OnToggles.Remove(toggleDatum.content.get_text());
							}
							toggleDatum.state = flag;
						}
					}
					EditorGUILayout.Space();
					EditorGUI.set_indentLevel(indentLevel);
				}
			}
			finally
			{
				((IDisposable)(DisabledScope)(ref val2)).Dispose();
			}
			EditorGUILayout.Space();
			if (GUILayout.Button("Build", (GUILayoutOption[])(object)new GUILayoutOption[0]))
			{
				EditorApplication.delayCall = (CallbackFunction)Delegate.Combine((Delegate)(object)EditorApplication.delayCall, (Delegate)new CallbackFunction(ExecuteBuild));
			}
			GUILayout.EndVertical();
			EditorGUILayout.EndScrollView();
		}

		private void ExecuteBuild()
		{
			//IL_012f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0149: Unknown result type (might be due to invalid IL or missing references)
			//IL_014b: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_015d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0163: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			//IL_0185: Unknown result type (might be due to invalid IL or missing references)
			//IL_0188: Unknown result type (might be due to invalid IL or missing references)
			//IL_018d: Unknown result type (might be due to invalid IL or missing references)
			//IL_018e: Unknown result type (might be due to invalid IL or missing references)
			//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
			if (Model.DataSource.CanSpecifyBuildOutputDirectory)
			{
				if (string.IsNullOrEmpty(m_UserData.m_OutputPath))
				{
					BrowseForFolder();
				}
				if (string.IsNullOrEmpty(m_UserData.m_OutputPath))
				{
					Debug.LogError((object)"AssetBundle Build: No valid output path for build.");
					return;
				}
				if (m_ForceRebuild.state)
				{
					string text = "Do you want to delete all files in the directory " + m_UserData.m_OutputPath;
					if (m_CopyToStreaming.state)
					{
						text = text + " and " + m_streamingPath;
					}
					text += "?";
					if (EditorUtility.DisplayDialog("File delete confirmation", text, "Yes", "No"))
					{
						try
						{
							if (Directory.Exists(m_UserData.m_OutputPath))
							{
								Directory.Delete(m_UserData.m_OutputPath, recursive: true);
							}
							if (m_CopyToStreaming.state && Directory.Exists(m_streamingPath))
							{
								Directory.Delete(m_streamingPath, recursive: true);
							}
						}
						catch (Exception ex)
						{
							Debug.LogException(ex);
						}
					}
				}
				if (!Directory.Exists(m_UserData.m_OutputPath))
				{
					Directory.CreateDirectory(m_UserData.m_OutputPath);
				}
			}
			BuildAssetBundleOptions val = (BuildAssetBundleOptions)0;
			if (Model.DataSource.CanSpecifyBuildOptions)
			{
				if (m_UserData.m_Compression == CompressOptions.Uncompressed)
				{
					val = (BuildAssetBundleOptions)(val | 1);
				}
				else if (m_UserData.m_Compression == CompressOptions.ChunkBasedCompression)
				{
					val = (BuildAssetBundleOptions)(val | 0x100);
				}
				foreach (ToggleData toggleDatum in m_ToggleData)
				{
					if (toggleDatum.state)
					{
						val = (BuildAssetBundleOptions)(val | toggleDatum.option);
					}
				}
			}
			ABBuildInfo buildInfo = new ABBuildInfo();
			buildInfo.outputDirectory = m_UserData.m_OutputPath;
			buildInfo.options = val;
			buildInfo.buildTarget = (BuildTarget)m_UserData.m_BuildTarget;
			buildInfo.onBuild = delegate
			{
				if (m_InspectTab != null)
				{
					m_InspectTab.AddBundleFolder(buildInfo.outputDirectory);
					m_InspectTab.RefreshBundles();
				}
			};
			Model.DataSource.BuildAssetBundles(buildInfo);
			AssetDatabase.Refresh((ImportAssetOptions)1);
			if (m_CopyToStreaming.state)
			{
				DirectoryCopy(m_UserData.m_OutputPath, m_streamingPath);
			}
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName)
		{
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}
			string[] directories = Directory.GetDirectories(sourceDirName, "*", SearchOption.AllDirectories);
			foreach (string text in directories)
			{
				if (!Directory.Exists(text.Replace(sourceDirName, destDirName)))
				{
					Directory.CreateDirectory(text.Replace(sourceDirName, destDirName));
				}
			}
			directories = Directory.GetFiles(sourceDirName, "*.*", SearchOption.AllDirectories);
			foreach (string obj in directories)
			{
				string text2 = Path.GetDirectoryName(obj).Replace("\\", "/");
				string destFileName = Path.Combine(path2: Path.GetFileName(obj), path1: text2.Replace(sourceDirName, destDirName));
				File.Copy(obj, destFileName, overwrite: true);
			}
		}

		private void BrowseForFolder()
		{
			m_UserData.m_UseDefaultPath = false;
			string text = EditorUtility.OpenFolderPanel("Bundle Folder", m_UserData.m_OutputPath, string.Empty);
			if (!string.IsNullOrEmpty(text))
			{
				string fullPath = Path.GetFullPath(".");
				fullPath = fullPath.Replace("\\", "/");
				if (text.StartsWith(fullPath) && text.Length > fullPath.Length)
				{
					text = text.Remove(0, fullPath.Length + 1);
				}
				m_UserData.m_OutputPath = text;
			}
		}

		private void ResetPathToDefault()
		{
			m_UserData.m_UseDefaultPath = true;
			m_UserData.m_OutputPath = "AssetBundles/";
			m_UserData.m_OutputPath += m_UserData.m_BuildTarget;
		}
	}
}

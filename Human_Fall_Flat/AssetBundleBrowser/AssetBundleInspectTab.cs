using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace AssetBundleBrowser
{
	[Serializable]
	internal class AssetBundleInspectTab
	{
		[Serializable]
		internal class InspectTabData
		{
			[Serializable]
			internal class BundleFolderData
			{
				[SerializeField]
				internal string path;

				[SerializeField]
				private List<string> m_ignoredFiles;

				internal List<string> ignoredFiles
				{
					get
					{
						if (m_ignoredFiles == null)
						{
							m_ignoredFiles = new List<string>();
						}
						return m_ignoredFiles;
					}
				}

				internal BundleFolderData(string p)
				{
					path = p;
				}
			}

			[SerializeField]
			private List<string> m_BundlePaths = new List<string>();

			[SerializeField]
			private List<BundleFolderData> m_BundleFolders = new List<BundleFolderData>();

			internal IList<string> BundlePaths => m_BundlePaths.AsReadOnly();

			internal IList<BundleFolderData> BundleFolders => m_BundleFolders.AsReadOnly();

			internal void AddPath(string newPath)
			{
				if (!m_BundlePaths.Contains(newPath))
				{
					BundleFolderData bundleFolderData = FolderDataContainingFilePath(newPath);
					if (bundleFolderData == null)
					{
						m_BundlePaths.Add(newPath);
					}
					else
					{
						bundleFolderData.ignoredFiles.Remove(newPath);
					}
				}
			}

			internal void AddFolder(string newPath)
			{
				if (!BundleFolderContains(newPath))
				{
					m_BundleFolders.Add(new BundleFolderData(newPath));
				}
			}

			internal void RemovePath(string pathToRemove)
			{
				m_BundlePaths.Remove(pathToRemove);
			}

			internal void RemoveFolder(string pathToRemove)
			{
				m_BundleFolders.Remove(BundleFolders.FirstOrDefault((BundleFolderData bfd) => bfd.path == pathToRemove));
			}

			internal bool FolderIgnoresFile(string folderPath, string filePath)
			{
				if (BundleFolders == null)
				{
					return false;
				}
				return BundleFolders.FirstOrDefault((BundleFolderData bfd) => bfd.path == folderPath)?.ignoredFiles.Contains(filePath) ?? false;
			}

			internal BundleFolderData FolderDataContainingFilePath(string filePath)
			{
				foreach (BundleFolderData bundleFolder in BundleFolders)
				{
					if (Path.GetFullPath(filePath).StartsWith(Path.GetFullPath(bundleFolder.path)))
					{
						return bundleFolder;
					}
				}
				return null;
			}

			private bool BundleFolderContains(string folderPath)
			{
				foreach (BundleFolderData bundleFolder in BundleFolders)
				{
					if (Path.GetFullPath(bundleFolder.path) == Path.GetFullPath(folderPath))
					{
						return true;
					}
				}
				return false;
			}
		}

		private Rect m_Position;

		[SerializeField]
		private InspectTabData m_Data;

		private Dictionary<string, List<string>> m_BundleList;

		private InspectBundleTree m_BundleTreeView;

		[SerializeField]
		private TreeViewState m_BundleTreeState;

		internal Editor m_Editor;

		private SingleBundleInspector m_SingleInspector;

		private Dictionary<string, AssetBundleRecord> m_loadedAssetBundles;

		internal Dictionary<string, List<string>> BundleList => m_BundleList;

		private AssetBundleRecord GetLoadedBundleRecordByName(string bundleName)
		{
			if (string.IsNullOrEmpty(bundleName))
			{
				return null;
			}
			if (!m_loadedAssetBundles.ContainsKey(bundleName))
			{
				return null;
			}
			return m_loadedAssetBundles[bundleName];
		}

		internal AssetBundleInspectTab()
		{
			m_BundleList = new Dictionary<string, List<string>>();
			m_SingleInspector = new SingleBundleInspector();
			m_loadedAssetBundles = new Dictionary<string, AssetBundleRecord>();
		}

		internal void OnEnable(Rect pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_008f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Expected O, but got Unknown
			m_Position = pos;
			if (m_Data == null)
			{
				m_Data = new InspectTabData();
			}
			string fullPath = Path.GetFullPath(".");
			fullPath = fullPath.Replace("\\", "/");
			fullPath += "/Library/AssetBundleBrowserInspect.dat";
			if (File.Exists(fullPath))
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				FileStream fileStream = File.Open(fullPath, FileMode.Open);
				InspectTabData inspectTabData = binaryFormatter.Deserialize(fileStream) as InspectTabData;
				if (inspectTabData != null)
				{
					m_Data = inspectTabData;
				}
				fileStream.Close();
			}
			if (m_BundleList == null)
			{
				m_BundleList = new Dictionary<string, List<string>>();
			}
			if (m_BundleTreeState == null)
			{
				m_BundleTreeState = new TreeViewState();
			}
			m_BundleTreeView = new InspectBundleTree(m_BundleTreeState, this);
			RefreshBundles();
		}

		internal void OnDisable()
		{
			ClearData();
			string path = Path.GetFullPath(".").Replace("\\", "/") + "/Library/AssetBundleBrowserInspect.dat";
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			FileStream fileStream = File.Create(path);
			binaryFormatter.Serialize(fileStream, m_Data);
			fileStream.Close();
		}

		internal void OnGUI(Rect pos)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Expected O, but got Unknown
			//IL_0070: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0085: Expected O, but got Unknown
			m_Position = pos;
			if (Application.get_isPlaying())
			{
				GUIStyle val = new GUIStyle(GUI.get_skin().get_label());
				val.set_alignment((TextAnchor)4);
				val.set_wordWrap(true);
				GUI.Label(new Rect(((Rect)(ref m_Position)).get_x() + 1f, ((Rect)(ref m_Position)).get_y() + 1f, ((Rect)(ref m_Position)).get_width() - 2f, ((Rect)(ref m_Position)).get_height() - 2f), new GUIContent("Inspector unavailable while in PLAY mode"), val);
			}
			else
			{
				OnGUIEditor();
			}
		}

		private void OnGUIEditor()
		{
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			EditorGUILayout.Space();
			GUILayout.BeginHorizontal((GUILayoutOption[])(object)new GUILayoutOption[0]);
			if (GUILayout.Button("Add File", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(75f) }))
			{
				BrowseForFile();
			}
			if (GUILayout.Button("Add Folder", (GUILayoutOption[])(object)new GUILayoutOption[1] { GUILayout.MaxWidth(75f) }))
			{
				BrowseForFolder();
			}
			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
			if (m_BundleList.Count > 0)
			{
				int num = (int)(((Rect)(ref m_Position)).get_width() / 2f);
				((TreeView)m_BundleTreeView).OnGUI(new Rect(((Rect)(ref m_Position)).get_x(), ((Rect)(ref m_Position)).get_y() + 30f, (float)num, ((Rect)(ref m_Position)).get_height() - 30f));
				m_SingleInspector.OnGUI(new Rect(((Rect)(ref m_Position)).get_x() + (float)num, ((Rect)(ref m_Position)).get_y() + 30f, (float)num, ((Rect)(ref m_Position)).get_height() - 30f));
			}
		}

		internal void RemoveBundlePath(string pathToRemove)
		{
			UnloadBundle(pathToRemove);
			m_Data.RemovePath(pathToRemove);
		}

		internal void RemoveBundleFolder(string pathToRemove)
		{
			List<string> value = null;
			if (m_BundleList.TryGetValue(pathToRemove, out value))
			{
				foreach (string item in value)
				{
					UnloadBundle(item);
				}
			}
			m_Data.RemoveFolder(pathToRemove);
		}

		private void BrowseForFile()
		{
			string text = EditorUtility.OpenFilePanelWithFilters("Bundle Folder", string.Empty, new string[0]);
			if (!string.IsNullOrEmpty(text))
			{
				string fullPath = Path.GetFullPath(".");
				fullPath = fullPath.Replace("\\", "/");
				if (text.StartsWith(fullPath))
				{
					text = text.Remove(0, fullPath.Length + 1);
				}
				m_Data.AddPath(text);
				RefreshBundles();
			}
		}

		private void BrowseForFolder(string folderPath = null)
		{
			folderPath = EditorUtility.OpenFolderPanel("Bundle Folder", string.Empty, string.Empty);
			if (!string.IsNullOrEmpty(folderPath))
			{
				string fullPath = Path.GetFullPath(".");
				fullPath = fullPath.Replace("\\", "/");
				if (folderPath.StartsWith(fullPath))
				{
					folderPath = folderPath.Remove(0, fullPath.Length + 1);
				}
				AddBundleFolder(folderPath);
				RefreshBundles();
			}
		}

		internal void AddBundleFolder(string folderPath)
		{
			m_Data.AddFolder(folderPath);
		}

		private void ClearData()
		{
			m_SingleInspector.SetBundle(null);
			if (m_loadedAssetBundles == null)
			{
				return;
			}
			foreach (AssetBundleRecord item in new List<AssetBundleRecord>(m_loadedAssetBundles.Values))
			{
				item.bundle.Unload(true);
			}
			m_loadedAssetBundles.Clear();
		}

		internal void RefreshBundles()
		{
			ClearData();
			if (m_Data.BundlePaths == null)
			{
				return;
			}
			if (m_BundleList == null)
			{
				m_BundleList = new Dictionary<string, List<string>>();
			}
			m_BundleList.Clear();
			List<string> list = new List<string>();
			foreach (string bundlePath in m_Data.BundlePaths)
			{
				if (File.Exists(bundlePath))
				{
					AddBundleToList(string.Empty, bundlePath);
					continue;
				}
				Debug.Log((object)("Expected bundle not found: " + bundlePath));
				list.Add(bundlePath);
			}
			foreach (string item in list)
			{
				m_Data.RemovePath(item);
			}
			list.Clear();
			foreach (InspectTabData.BundleFolderData bundleFolder in m_Data.BundleFolders)
			{
				if (Directory.Exists(bundleFolder.path))
				{
					AddFilePathToList(bundleFolder.path, bundleFolder.path);
					continue;
				}
				Debug.Log((object)("Expected folder not found: " + bundleFolder));
				list.Add(bundleFolder.path);
			}
			foreach (string item2 in list)
			{
				m_Data.RemoveFolder(item2);
			}
			((TreeView)m_BundleTreeView).Reload();
		}

		private void AddBundleToList(string parent, string bundlePath)
		{
			List<string> value = null;
			m_BundleList.TryGetValue(parent, out value);
			if (value == null)
			{
				value = new List<string>();
				m_BundleList.Add(parent, value);
			}
			value.Add(bundlePath);
		}

		private void AddFilePathToList(string rootPath, string path)
		{
			string[] source = new string[6] { ".meta", ".manifest", ".dll", ".cs", ".exe", ".js" };
			string[] files = Directory.GetFiles(path);
			foreach (string text in files)
			{
				string extension = Path.GetExtension(text);
				if (!source.Contains(extension))
				{
					string text2 = text.Replace('\\', '/');
					if (File.Exists(text) && !m_Data.FolderIgnoresFile(rootPath, text2))
					{
						AddBundleToList(rootPath, text2);
					}
				}
			}
			files = Directory.GetDirectories(path);
			foreach (string path2 in files)
			{
				AddFilePathToList(rootPath, path2);
			}
		}

		internal void SetBundleItem(IList<InspectTreeItem> selected)
		{
			if (selected == null || selected.Count == 0 || selected[0] == null)
			{
				m_SingleInspector.SetBundle(null);
			}
			else if (selected.Count == 1)
			{
				AssetBundle bundle = LoadBundle(selected[0].bundlePath);
				m_SingleInspector.SetBundle(bundle, selected[0].bundlePath, m_Data, this);
			}
			else
			{
				m_SingleInspector.SetBundle(null);
			}
		}

		private AssetBundle LoadBundle(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return null;
			}
			string extension = Path.GetExtension(path);
			string text = path.Substring(0, path.Length - extension.Length);
			AssetBundleRecord loadedBundleRecordByName = GetLoadedBundleRecordByName(text);
			AssetBundle val = null;
			if (loadedBundleRecordByName != null)
			{
				if (!loadedBundleRecordByName.path.Equals(path))
				{
					UnloadBundle(text);
				}
				else
				{
					val = loadedBundleRecordByName.bundle;
				}
			}
			if ((Object)null == (Object)(object)val)
			{
				val = AssetBundle.LoadFromFile(path);
				if ((Object)null == (Object)(object)val)
				{
					return null;
				}
				m_loadedAssetBundles[text] = new AssetBundleRecord(path, val);
				string[] allAssetNames = val.GetAllAssetNames();
				foreach (string text2 in allAssetNames)
				{
					val.LoadAsset(text2);
				}
			}
			return val;
		}

		private void UnloadBundle(string bundleName)
		{
			AssetBundleRecord loadedBundleRecordByName = GetLoadedBundleRecordByName(bundleName);
			if (loadedBundleRecordByName != null)
			{
				loadedBundleRecordByName.bundle.Unload(true);
				m_loadedAssetBundles.Remove(bundleName);
			}
		}
	}
}

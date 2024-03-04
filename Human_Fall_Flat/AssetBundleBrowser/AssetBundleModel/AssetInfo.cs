using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace AssetBundleBrowser.AssetBundleModel
{
	internal class AssetInfo
	{
		internal long fileSize;

		private HashSet<string> m_Parents;

		private string m_AssetName;

		private string m_DisplayName;

		private string m_BundleName;

		private MessageSystem.MessageState m_AssetMessages = new MessageSystem.MessageState();

		private List<AssetInfo> m_dependencies;

		internal bool isScene { get; set; }

		internal bool isFolder { get; set; }

		internal string fullAssetName
		{
			get
			{
				return m_AssetName;
			}
			set
			{
				m_AssetName = value;
				m_DisplayName = Path.GetFileNameWithoutExtension(m_AssetName);
				FileInfo fileInfo = new FileInfo(m_AssetName);
				if (fileInfo.Exists)
				{
					fileSize = fileInfo.Length;
				}
				else
				{
					fileSize = 0L;
				}
			}
		}

		internal string displayName => m_DisplayName;

		internal string bundleName
		{
			get
			{
				if (!string.IsNullOrEmpty(m_BundleName))
				{
					return m_BundleName;
				}
				return "auto";
			}
		}

		internal AssetInfo(string inName, string bundleName = "")
		{
			fullAssetName = inName;
			m_BundleName = bundleName;
			m_Parents = new HashSet<string>();
			isScene = false;
			isFolder = false;
		}

		internal Color GetColor()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (string.IsNullOrEmpty(m_BundleName))
			{
				return Model.k_LightGrey;
			}
			return Color.get_white();
		}

		internal bool IsMessageSet(MessageSystem.MessageFlag flag)
		{
			return m_AssetMessages.IsSet(flag);
		}

		internal void SetMessageFlag(MessageSystem.MessageFlag flag, bool on)
		{
			m_AssetMessages.SetFlag(flag, on);
		}

		internal MessageType HighestMessageLevel()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return m_AssetMessages.HighestMessageLevel();
		}

		internal IEnumerable<MessageSystem.Message> GetMessages()
		{
			List<MessageSystem.Message> list = new List<MessageSystem.Message>();
			if (IsMessageSet(MessageSystem.MessageFlag.SceneBundleConflict))
			{
				string text = displayName + "\n";
				text = ((!isScene) ? (text + "Is included in a bundle with a scene. Scene bundles must have only one or more scene assets.") : (text + "Is a scene that is in a bundle with non-scene assets. Scene bundles must have only one or more scene assets."));
				list.Add(new MessageSystem.Message(text, (MessageType)3));
			}
			if (IsMessageSet(MessageSystem.MessageFlag.DependencySceneConflict))
			{
				string text2 = displayName + "\n";
				text2 += MessageSystem.GetMessage(MessageSystem.MessageFlag.DependencySceneConflict).message;
				list.Add(new MessageSystem.Message(text2, (MessageType)3));
			}
			if (IsMessageSet(MessageSystem.MessageFlag.AssetsDuplicatedInMultBundles))
			{
				IEnumerable<string> enumerable = Model.CheckDependencyTracker(this);
				string text3 = displayName + "\nIs auto-included in multiple bundles:\n";
				foreach (string item in enumerable)
				{
					text3 = text3 + item + ", ";
				}
				text3 = text3.Substring(0, text3.Length - 2);
				list.Add(new MessageSystem.Message(text3, (MessageType)2));
			}
			if (string.IsNullOrEmpty(m_BundleName) && m_Parents.Count > 0)
			{
				string text4 = displayName + "\nIs auto included in bundle(s) due to parent(s): \n";
				foreach (string parent in m_Parents)
				{
					text4 = text4 + parent + ", ";
				}
				text4 = text4.Substring(0, text4.Length - 2);
				list.Add(new MessageSystem.Message(text4, (MessageType)1));
			}
			if (m_dependencies != null && m_dependencies.Count > 0)
			{
				string text5 = string.Empty;
				foreach (AssetInfo item2 in m_dependencies.OrderBy((AssetInfo d) => d.bundleName))
				{
					if (item2.bundleName != bundleName)
					{
						text5 = text5 + item2.bundleName + " : " + item2.displayName + "\n";
					}
				}
				if (!string.IsNullOrEmpty(text5))
				{
					text5 = text5.Insert(0, displayName + "\nIs dependent on other bundle's asset(s) or auto included asset(s): \n");
					text5 = text5.Substring(0, text5.Length - 1);
					list.Add(new MessageSystem.Message(text5, (MessageType)1));
				}
			}
			list.Add(new MessageSystem.Message(displayName + "\nPath: " + fullAssetName, (MessageType)1));
			return list;
		}

		internal void AddParent(string name)
		{
			m_Parents.Add(name);
		}

		internal void RemoveParent(string name)
		{
			m_Parents.Remove(name);
		}

		internal string GetSizeString()
		{
			if (fileSize == 0L)
			{
				return "--";
			}
			return EditorUtility.FormatBytes(fileSize);
		}

		internal List<AssetInfo> GetDependencies()
		{
			if (m_dependencies == null)
			{
				m_dependencies = new List<AssetInfo>();
				if (!AssetDatabase.IsValidFolder(m_AssetName))
				{
					string[] dependencies = AssetDatabase.GetDependencies(m_AssetName, true);
					foreach (string text in dependencies)
					{
						if (text != m_AssetName)
						{
							AssetInfo assetInfo = Model.CreateAsset(text, this);
							if (assetInfo != null)
							{
								m_dependencies.Add(assetInfo);
							}
						}
					}
				}
			}
			return m_dependencies;
		}
	}
}

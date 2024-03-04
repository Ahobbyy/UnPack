using UnityEditor;
using UnityEngine;

namespace PrefabSampler
{
	public class PrefabSamplerConfigData : ScriptableObject
	{
		public const string assetsRoot = "Assets/";

		private const string settingsFilePath = "Assets/PrefabSampler/PrefabSamplerSettings.asset";

		private static PrefabSamplerConfigData editorData;

		[Header("Saving prefabs location")]
		[SerializeField]
		private string destinationFolder = "Assets/";

		[SerializeField]
		private string appendName = "-HFFPrefab";

		[Header("Pivoting")]
		[SerializeField]
		private bool fixMeshPosition = true;

		[SerializeField]
		private MeshPivotType pivotType;

		[SerializeField]
		private bool includeNodeGraphObjects = true;

		[SerializeField]
		private bool allignWithChildren = true;

		[SerializeField]
		private bool fixScale = true;

		[SerializeField]
		[HideInInspector]
		private Texture2D configButton;

		public static PrefabSamplerConfigData EditorData
		{
			get
			{
				if ((Object)(object)editorData == (Object)null)
				{
					InitializeConfigFile();
				}
				return editorData;
			}
		}

		public string DestinationFolder
		{
			get
			{
				return destinationFolder;
			}
			set
			{
				destinationFolder = GetValidPath(value);
			}
		}

		public string AppendName
		{
			get
			{
				return appendName;
			}
			set
			{
				appendName = value;
			}
		}

		public bool FixMeshPosition
		{
			get
			{
				return fixMeshPosition;
			}
			set
			{
				fixMeshPosition = value;
			}
		}

		public MeshPivotType PivotType
		{
			get
			{
				return pivotType;
			}
			set
			{
				pivotType = value;
			}
		}

		public bool IncludeNodeGraphObjects
		{
			get
			{
				return includeNodeGraphObjects;
			}
			set
			{
				includeNodeGraphObjects = value;
			}
		}

		public bool AllignWithChildren
		{
			get
			{
				return allignWithChildren;
			}
			set
			{
				allignWithChildren = value;
			}
		}

		public bool FixScale
		{
			get
			{
				return fixScale;
			}
			set
			{
				fixScale = value;
			}
		}

		public Texture2D ConfigButton => configButton;

		private void OnValidate()
		{
			if ((Object)(object)editorData != (Object)null)
			{
				AssertValidSettings();
			}
		}

		private static void InitializeConfigFile()
		{
			PrefabSamplerConfigData prefabSamplerConfigData = AssetDatabase.LoadAssetAtPath<PrefabSamplerConfigData>("Assets/PrefabSampler/PrefabSamplerSettings.asset");
			if ((Object)(object)prefabSamplerConfigData == (Object)null)
			{
				Debug.Log((object)"Prefab Sampler: No settings file found. Creating new settings file");
				AssetDatabase.CreateAsset((Object)(object)ScriptableObject.CreateInstance<PrefabSamplerConfigData>(), "Assets/PrefabSampler/PrefabSamplerSettings.asset");
				AssetDatabase.SaveAssets();
				AssetDatabase.Refresh();
				editorData = AssetDatabase.LoadAssetAtPath<PrefabSamplerConfigData>("Assets/PrefabSampler/PrefabSamplerSettings.asset");
			}
			else
			{
				editorData = prefabSamplerConfigData;
			}
			AssertValidSettings();
		}

		public static string GetValidPath(string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				path = "Assets/";
			}
			else if (!path.StartsWith("Assets/"))
			{
				path = ((!path.Contains("Assets/")) ? "Assets/" : path.Substring(path.IndexOf("Assets/")));
			}
			path = (AssetDatabase.IsValidFolder(path) ? path : "Assets/");
			return path;
		}

		private static void AssertValidSettings()
		{
			editorData.destinationFolder = GetValidPath(editorData.destinationFolder);
		}

		public PrefabSamplerConfigData()
			: this()
		{
		}
	}
}

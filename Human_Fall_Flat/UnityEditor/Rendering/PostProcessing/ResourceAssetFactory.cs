using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace UnityEditor.Rendering.PostProcessing
{
	internal static class ResourceAssetFactory
	{
		private static void CreateAsset()
		{
			AssetDatabase.CreateAsset((Object)(object)ScriptableObject.CreateInstance<PostProcessResources>(), "Assets/PostProcessResources.asset");
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}
	}
}

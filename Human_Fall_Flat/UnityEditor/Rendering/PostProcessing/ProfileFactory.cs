using System.IO;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

namespace UnityEditor.Rendering.PostProcessing
{
	public class ProfileFactory
	{
		[MenuItem("Assets/Create/Post-processing Profile", priority = 201)]
		private static void CreatePostProcessProfile()
		{
			ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, (EndNameEditAction)(object)ScriptableObject.CreateInstance<DoCreatePostProcessProfile>(), "New Post-processing Profile.asset", (Texture2D)null, (string)null);
		}

		public static PostProcessProfile CreatePostProcessProfileAtPath(string path)
		{
			PostProcessProfile postProcessProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
			((Object)postProcessProfile).set_name(Path.GetFileName(path));
			AssetDatabase.CreateAsset((Object)(object)postProcessProfile, path);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return postProcessProfile;
		}

		public static PostProcessProfile CreatePostProcessProfile(Scene scene, string targetName)
		{
			string empty = string.Empty;
			if (string.IsNullOrEmpty(((Scene)(ref scene)).get_path()))
			{
				empty = "Assets/";
			}
			else
			{
				string directoryName = Path.GetDirectoryName(((Scene)(ref scene)).get_path());
				string text = ((Scene)(ref scene)).get_name() + "_Profiles";
				string text2 = directoryName + "/" + text;
				if (!AssetDatabase.IsValidFolder(text2))
				{
					AssetDatabase.CreateFolder(directoryName, text);
				}
				empty = text2 + "/";
			}
			empty = empty + targetName + " Profile.asset";
			empty = AssetDatabase.GenerateUniqueAssetPath(empty);
			PostProcessProfile postProcessProfile = ScriptableObject.CreateInstance<PostProcessProfile>();
			AssetDatabase.CreateAsset((Object)(object)postProcessProfile, empty);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			return postProcessProfile;
		}
	}
}

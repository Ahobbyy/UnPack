using UnityEditor;

namespace TMPro.EditorUtilities
{
	public class TMPro_PackageImportPostProcessor : AssetPostprocessor
	{
		private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			for (int i = 0; i < deletedAssets.Length; i++)
			{
				if (deletedAssets[i] == "Assets/TextMesh Pro")
				{
					string scriptingDefineSymbolsForGroup = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.get_selectedBuildTargetGroup());
					if (scriptingDefineSymbolsForGroup.Contains("TMP_PRESENT;"))
					{
						scriptingDefineSymbolsForGroup = scriptingDefineSymbolsForGroup.Replace("TMP_PRESENT;", "");
						PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.get_selectedBuildTargetGroup(), scriptingDefineSymbolsForGroup);
					}
					else if (scriptingDefineSymbolsForGroup.Contains("TMP_PRESENT"))
					{
						scriptingDefineSymbolsForGroup = scriptingDefineSymbolsForGroup.Replace("TMP_PRESENT", "");
						PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.get_selectedBuildTargetGroup(), scriptingDefineSymbolsForGroup);
					}
				}
			}
		}

		public TMPro_PackageImportPostProcessor()
			: this()
		{
		}
	}
}

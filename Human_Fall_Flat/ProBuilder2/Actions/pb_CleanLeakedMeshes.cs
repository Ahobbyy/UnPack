using UnityEditor;

namespace ProBuilder2.Actions
{
	public class pb_CleanLeakedMeshes : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Clean Leaked Meshes", false, 600)]
		public static void CleanUp()
		{
			EditorUtility.UnloadUnusedAssetsImmediate();
		}

		public pb_CleanLeakedMeshes()
			: this()
		{
		}
	}
}

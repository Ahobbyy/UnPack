using UnityEditor;

namespace ProBuilder2.Common
{
	[InitializeOnLoad]
	public static class pb_Fbx
	{
		private static bool m_FbxIsLoaded;

		public static bool FbxEnabled => m_FbxIsLoaded;
	}
}

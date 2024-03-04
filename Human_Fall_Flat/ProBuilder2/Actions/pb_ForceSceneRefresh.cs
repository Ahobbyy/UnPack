using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_ForceSceneRefresh : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Force Refresh Scene", false, 600)]
		public static void MenuForceSceneRefresh()
		{
			ForceRefresh(interactive: true);
		}

		private static void ForceRefresh(bool interactive)
		{
			pb_Object[] array = (pb_Object[])(object)Object.FindObjectsOfType(typeof(pb_Object));
			for (int i = 0; i < array.Length; i++)
			{
				if (interactive)
				{
					EditorUtility.DisplayProgressBar("Refreshing ProBuilder Objects", "Reshaping pb_Object " + array[i].get_id() + ".", (float)i / (float)array.Length);
				}
				try
				{
					array[i].ToMesh();
					array[i].Refresh((RefreshMask)255);
					pb_EditorMeshUtility.Optimize(array[i], false);
				}
				catch
				{
				}
			}
			if (interactive)
			{
				EditorUtility.ClearProgressBar();
				EditorUtility.DisplayDialog("Refresh ProBuilder Objects", "Successfully refreshed all ProBuilder objects in scene.", "Okay");
			}
		}

		public pb_ForceSceneRefresh()
			: this()
		{
		}
	}
}

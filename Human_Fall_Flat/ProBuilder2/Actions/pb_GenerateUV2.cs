using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_GenerateUV2 : Editor
	{
		[MenuItem("Tools/ProBuilder/Actions/Generate UV2 - Selection", true, 320)]
		public static bool VerifyGenerateUV2Selection()
		{
			return pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()).Length != 0;
		}

		[MenuItem("Tools/ProBuilder/Actions/Generate UV2 - Selection", false, 320)]
		public static void MenuGenerateUV2Selection()
		{
			pb_Object[] components = pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms());
			if (Menu_GenerateUV2(components))
			{
				if (components.Length != 0)
				{
					pb_EditorUtility.ShowNotification("Generated UV2 for " + components.Length + " Meshes");
				}
				else
				{
					pb_EditorUtility.ShowNotification("Nothing Selected");
				}
			}
		}

		[MenuItem("Tools/ProBuilder/Actions/Generate UV2 - Scene", false, 320)]
		public static void MenuGenerateUV2Scene()
		{
			pb_Object[] array = (pb_Object[])(object)Object.FindObjectsOfType(typeof(pb_Object));
			if (Menu_GenerateUV2(array))
			{
				if (array.Length != 0)
				{
					pb_EditorUtility.ShowNotification("Generated UV2 for " + array.Length + " Meshes");
				}
				else
				{
					pb_EditorUtility.ShowNotification("No ProBuilder Objects Found");
				}
			}
		}

		private static bool Menu_GenerateUV2(pb_Object[] selected)
		{
			for (int i = 0; i < selected.Length; i++)
			{
				if (selected.Length > 3 && EditorUtility.DisplayCancelableProgressBar("Generating UV2 Channel", "pb_Object: " + ((Object)selected[i]).get_name() + ".", ((float)i + 1f) / (float)selected.Length))
				{
					EditorUtility.ClearProgressBar();
					Debug.LogWarning((object)("User canceled UV2 generation.  " + (selected.Length - i) + " pb_Objects left without lightmap UVs."));
					return false;
				}
				pb_EditorMeshUtility.Optimize(selected[i], true);
			}
			EditorUtility.ClearProgressBar();
			return true;
		}

		public pb_GenerateUV2()
			: this()
		{
		}
	}
}

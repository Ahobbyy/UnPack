using System;
using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_RebuildSharedIndices : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Rebuild Shared Indices Cache", true, 600)]
		private static bool VertifyRebuildMeshes()
		{
			return pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()).Length != 0;
		}

		[MenuItem("Tools/ProBuilder/Repair/Rebuild Shared Indices Cache", false, 600)]
		public static void DoRebuildMeshes()
		{
			RebuildSharedIndices(pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()));
		}

		private static void RebuildSharedIndices(pb_Object[] targets, bool interactive = true)
		{
			for (int i = 0; i < targets.Length; i++)
			{
				if (interactive)
				{
					EditorUtility.DisplayProgressBar("Refreshing ProBuilder Objects", "Reshaping pb_Object " + targets[i].get_id() + ".", (float)i / (float)targets.Length);
				}
				pb_Object val = targets[i];
				try
				{
					val.SetSharedIndices(pb_IntArrayUtility.ExtractSharedIndices(val.get_vertices()));
					val.ToMesh();
					val.Refresh((RefreshMask)255);
					pb_EditorMeshUtility.Optimize(val, false);
				}
				catch (Exception ex)
				{
					Debug.LogError((object)("Failed rebuilding " + ((Object)val).get_name() + " shared indices cache.\n" + ex.ToString()));
				}
			}
			if (interactive)
			{
				EditorUtility.ClearProgressBar();
				EditorUtility.DisplayDialog("Rebuild Shared Index Cache", "Successfully rebuilt " + targets.Length + " shared index caches", "Okay");
			}
		}

		public pb_RebuildSharedIndices()
			: this()
		{
		}
	}
}

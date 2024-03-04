using System;
using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using ProBuilder2.MeshOperations;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_RebuildMeshes : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Rebuild ProBuilder Objects", true, 600)]
		private static bool VertifyRebuildMeshes()
		{
			return pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()).Length != 0;
		}

		[MenuItem("Tools/ProBuilder/Repair/Rebuild ProBuilder Objects", false, 600)]
		public static void DoRebuildMeshes()
		{
			StripAndProBuilderize(pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms()));
		}

		private static void StripAndProBuilderize(pb_Object[] targets, bool interactive = true)
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
					val.ToMesh();
					val.Refresh((RefreshMask)255);
					pb_EditorMeshUtility.Optimize(val, false);
				}
				catch
				{
					if ((Object)(object)val.get_msh() != (Object)null)
					{
						RebuildProBuilderMesh(val);
					}
				}
			}
			if (interactive)
			{
				EditorUtility.ClearProgressBar();
				EditorUtility.DisplayDialog("Rebuild ProBuilder Objects", "Successfully rebuilt " + targets.Length + " ProBuilder Objects", "Okay");
			}
		}

		private static void RebuildProBuilderMesh(pb_Object pb)
		{
			try
			{
				GameObject gameObject = ((Component)pb).get_gameObject();
				pb.dontDestroyMeshOnDelete = true;
				Undo.DestroyObjectImmediate((Object)(object)pb);
				pb = Undo.AddComponent<pb_Object>(gameObject);
				pbMeshOps.ResetPbObjectWithMeshFilter(pb, true);
				pb.ToMesh();
				pb.Refresh((RefreshMask)255);
				pb_EditorMeshUtility.Optimize(pb, false);
			}
			catch (Exception ex)
			{
				Debug.LogError((object)("Failed rebuilding ProBuilder mesh: " + ex.ToString()));
			}
		}

		public pb_RebuildMeshes()
			: this()
		{
		}
	}
}

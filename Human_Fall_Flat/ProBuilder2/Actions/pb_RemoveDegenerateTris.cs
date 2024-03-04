using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using ProBuilder2.MeshOperations;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_RemoveDegenerateTris : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Remove Degenerate Triangles", false, 600)]
		public static void MenuRemoveDegenerateTriangles()
		{
			int num = 0;
			pb_Object[] components = pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms());
			int[] array = default(int[]);
			foreach (pb_Object obj in components)
			{
				obj.ToMesh();
				pbTriangleOps.RemoveDegenerateTriangles(obj, ref array);
				num += array.Length;
				obj.ToMesh();
				obj.Refresh((RefreshMask)255);
				pb_EditorMeshUtility.Optimize(obj, false);
			}
			pb_EditorUtility.ShowNotification("Removed " + num / 3 + " degenerate triangles.");
		}

		public pb_RemoveDegenerateTris()
			: this()
		{
		}
	}
}

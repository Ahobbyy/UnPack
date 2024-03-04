using System.Collections.Generic;
using ProBuilder2.Common;
using ProBuilder2.EditorCommon;
using UnityEditor;
using UnityEngine;

namespace ProBuilder2.Actions
{
	public class pb_RepairColors : Editor
	{
		[MenuItem("Tools/ProBuilder/Repair/Rebuild Vertex Colors", false, 600)]
		public static void MenuRepairColors()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			int num = 0;
			pb_Object[] components = pbUtil.GetComponents<pb_Object>((IEnumerable<Transform>)Selection.get_transforms());
			foreach (pb_Object val in components)
			{
				if (val.get_colors() == null || val.get_colors().Length != val.get_vertexCount())
				{
					val.ToMesh();
					val.SetColors(pbUtil.FilledArray<Color>(Color.get_white(), val.get_vertexCount()));
					val.Refresh((RefreshMask)255);
					pb_EditorMeshUtility.Optimize(val, false);
					num++;
				}
			}
			pb_EditorUtility.ShowNotification("Rebuilt colors for " + num + " ProBuilder Objects.");
		}

		public pb_RepairColors()
			: this()
		{
		}
	}
}

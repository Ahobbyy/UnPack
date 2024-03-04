using System;
using System.Collections.Generic;
using System.Linq;
using ProBuilder2.Common;
using ProBuilder2.MeshOperations;
using UnityEngine;

public class ExtrudeRandomEdges : MonoBehaviour
{
	private pb_Object pb;

	private pb_Face lastExtrudedFace;

	public float distance = 1f;

	private void Start()
	{
		pb = pb_ShapeGenerator.PlaneGenerator(1f, 1f, 0, 0, (Axis)2, false);
		pb.SetFaceMaterial(pb.get_faces(), pb_Constant.get_DefaultMaterial());
		lastExtrudedFace = pb.get_faces()[0];
	}

	private void OnGUI()
	{
		if (GUILayout.Button("Extrude Random Edge", (GUILayoutOption[])(object)new GUILayoutOption[0]))
		{
			ExtrudeEdge();
		}
	}

	private void ExtrudeEdge()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		pb_Face sourceFace = lastExtrudedFace;
		List<pb_Edge> list = (from x in pb_WingedEdge.GetWingedEdges(pb, false)
			where x.face == sourceFace
			where x.opposite == null
			select x into y
			select y.edge.local).ToList();
		int index = Random.Range(0, list.Count);
		pb_Edge val = list[index];
		Vector3 val2 = (pb.get_vertices()[val.x] + pb.get_vertices()[val.y]) * 0.5f - pb_Math.Average<int>((IList<int>)sourceFace.get_distinctIndices(), (Func<int, Vector3>)((int x) => pb.get_vertices()[x]), (IList<int>)null);
		((Vector3)(ref val2)).Normalize();
		pb_Edge[] selectedEdges = default(pb_Edge[]);
		pbMeshOps.Extrude(pb, (pb_Edge[])(object)new pb_Edge[1] { val }, 0f, false, true, ref selectedEdges);
		lastExtrudedFace = pb.get_faces().Last();
		pb.SetSelectedEdges((IEnumerable<pb_Edge>)selectedEdges);
		pb_Object_Utility.TranslateVertices(pb, pb.get_SelectedTriangles(), val2 * distance);
		pb.ToMesh();
		pb.Refresh((RefreshMask)255);
	}

	public ExtrudeRandomEdges()
		: this()
	{
	}
}

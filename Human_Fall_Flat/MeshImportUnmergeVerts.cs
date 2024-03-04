using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MeshImportUnmergeVerts : AssetPostprocessor
{
	public override int GetPostprocessOrder()
	{
		return 2;
	}

	private void OnPostprocessModel(GameObject g)
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		if (!Path.GetFileName(((AssetPostprocessor)this).get_assetPath()).Contains("Water"))
		{
			return;
		}
		MeshFilter componentInChildren = g.GetComponentInChildren<MeshFilter>();
		if ((Object)(object)componentInChildren == (Object)null)
		{
			ProcessSkinned(g);
			return;
		}
		Mesh sharedMesh = componentInChildren.get_sharedMesh();
		int[] triangles = sharedMesh.get_triangles();
		Vector3[] vertices = sharedMesh.get_vertices();
		Vector2[] uv = sharedMesh.get_uv();
		List<Vector2> list = new List<Vector2>();
		sharedMesh.GetUVs(1, list);
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		new List<Vector3>();
		List<Vector3> list4 = new List<Vector3>();
		Vector3[] array = (Vector3[])(object)new Vector3[triangles.Length];
		for (int i = 0; i < triangles.Length; i++)
		{
			array[i] = vertices[triangles[i]];
			if (list.Count > 0)
			{
				list3.Add(list[triangles[i]]);
			}
			else if (uv.Length != 0)
			{
				list3.Add(uv[triangles[i]]);
			}
			int num = i / 3;
			int num2 = num * 3 + (i + 1) % 3;
			int num3 = num * 3 + (i + 2) % 3;
			list2.Add(vertices[triangles[num2]]);
			list4.Add(vertices[triangles[num3]]);
		}
		for (int j = 0; j < triangles.Length; j++)
		{
			triangles[j] = j;
		}
		sharedMesh.Clear();
		sharedMesh.set_vertices(array);
		sharedMesh.SetUVs(0, list2);
		if (list3.Count > 0)
		{
			sharedMesh.SetUVs(1, list3);
		}
		sharedMesh.SetUVs(3, list4);
		sharedMesh.set_triangles(triangles);
		sharedMesh.RecalculateNormals();
		componentInChildren.set_sharedMesh(sharedMesh);
		Debug.Log((object)("Processed " + ((AssetPostprocessor)this).get_assetPath()));
	}

	private void ProcessSkinned(GameObject g)
	{
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		SkinnedMeshRenderer componentInChildren = g.GetComponentInChildren<SkinnedMeshRenderer>();
		Mesh sharedMesh = componentInChildren.get_sharedMesh();
		int[] triangles = sharedMesh.get_triangles();
		Vector3[] vertices = sharedMesh.get_vertices();
		Vector2[] uv = sharedMesh.get_uv();
		List<Vector2> list = new List<Vector2>();
		BoneWeight[] boneWeights = sharedMesh.get_boneWeights();
		sharedMesh.GetUVs(1, list);
		List<Vector3> list2 = new List<Vector3>();
		List<Vector2> list3 = new List<Vector2>();
		List<Vector3> list4 = new List<Vector3>();
		List<Vector3> list5 = new List<Vector3>();
		List<BoneWeight> list6 = new List<BoneWeight>();
		Vector3[] array = (Vector3[])(object)new Vector3[triangles.Length];
		for (int i = 0; i < triangles.Length; i++)
		{
			array[i] = vertices[triangles[i]];
			if (list.Count > 0)
			{
				list3.Add(list[triangles[i]]);
			}
			else if (uv.Length != 0)
			{
				list3.Add(uv[triangles[i]]);
			}
			int num = i / 3;
			int num2 = num * 3 + (i + 1) % 3;
			int num3 = num * 3 + (i + 2) % 3;
			list2.Add(vertices[triangles[num2]]);
			list4.Add(vertices[triangles[i]]);
			list5.Add(vertices[triangles[num3]]);
			list6.Add(boneWeights[triangles[i]]);
		}
		for (int j = 0; j < triangles.Length; j++)
		{
			triangles[j] = j;
		}
		sharedMesh.Clear();
		sharedMesh.set_vertices(array);
		sharedMesh.SetUVs(0, list2);
		if (list3.Count > 0)
		{
			sharedMesh.SetUVs(1, list3);
		}
		sharedMesh.SetUVs(2, list4);
		sharedMesh.SetUVs(3, list5);
		sharedMesh.set_boneWeights(list6.ToArray());
		sharedMesh.set_triangles(triangles);
		sharedMesh.RecalculateNormals();
		componentInChildren.set_sharedMesh(sharedMesh);
		Debug.Log((object)("Processed skinned " + ((AssetPostprocessor)this).get_assetPath()));
	}

	public MeshImportUnmergeVerts()
		: this()
	{
	}
}

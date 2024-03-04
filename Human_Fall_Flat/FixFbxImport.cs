using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FixFbxImport : AssetPostprocessor
{
	public Queue<Vector3> positions = new Queue<Vector3>();

	public Queue<Quaternion> rotations = new Queue<Quaternion>();

	private List<Mesh> processedMeshes = new List<Mesh>();

	public override int GetPostprocessOrder()
	{
		return 1;
	}

	public void OnPostprocessModel(GameObject obj)
	{
		if (!((AssetPostprocessor)this).get_assetPath().Contains("PhotoRoom") && (((AssetPostprocessor)this).get_assetPath().Contains("WorkshopContent") || ((AssetPostprocessor)this).get_assetPath().Contains("HumanWorkshop") || ((AssetPostprocessor)this).get_assetPath().Contains("Aztec") || ((AssetPostprocessor)this).get_assetPath().Contains("Finale") || ((AssetPostprocessor)this).get_assetPath().Contains("Steam")))
		{
			processedMeshes.Clear();
			CollectPosRot(obj.get_transform());
			RotateObject(obj.get_transform());
		}
	}

	private void CollectPosRot(Transform obj)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		positions.Enqueue(obj.get_position());
		rotations.Enqueue(obj.get_rotation());
		foreach (Transform item in obj)
		{
			Transform obj2 = item;
			CollectPosRot(obj2);
		}
	}

	private void RotateObject(Transform obj)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Expected O, but got Unknown
		obj.set_position(positions.Dequeue());
		obj.set_rotation(rotations.Dequeue() * Quaternion.Euler(90f, 0f, 0f));
		obj.set_localScale(new Vector3(obj.get_localScale().x, obj.get_localScale().z, obj.get_localScale().y));
		Component component = ((Component)obj).GetComponent(typeof(MeshFilter));
		MeshFilter val = (MeshFilter)(object)((component is MeshFilter) ? component : null);
		if (Object.op_Implicit((Object)(object)val) && !processedMeshes.Contains(val.get_sharedMesh()))
		{
			RotateMesh(val.get_sharedMesh());
			processedMeshes.Add(val.get_sharedMesh());
		}
		foreach (Transform item in obj)
		{
			Transform obj2 = item;
			RotateObject(obj2);
		}
	}

	private Vector3 SnapEuler(Vector3 euler)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		float num = Mathf.Round(euler.x);
		float num2 = Mathf.Round(euler.y);
		float num3 = Mathf.Round(euler.z);
		if (Mathf.Abs(num - euler.x) < 0.1f)
		{
			euler.x = num;
		}
		if (Mathf.Abs(num2 - euler.y) < 0.1f)
		{
			euler.y = num2;
		}
		if (Mathf.Abs(num3 - euler.z) < 0.1f)
		{
			euler.z = num3;
		}
		return euler;
	}

	private void RotateMesh(Mesh mesh)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		Vector3[] vertices = mesh.get_vertices();
		Vector3[] array = mesh.get_normals();
		Vector4[] array2 = mesh.get_tangents();
		if (array != null && array.Length == 0)
		{
			array = null;
		}
		if (array2 != null && array2.Length == 0)
		{
			array2 = null;
		}
		for (num = 0; num < vertices.Length; num++)
		{
			vertices[num] = new Vector3(vertices[num].x, vertices[num].z, 0f - vertices[num].y);
			if (array != null)
			{
				array[num] = new Vector3(array[num].x, array[num].z, 0f - array[num].y);
			}
			if (array2 != null)
			{
				array2[num] = new Vector4(array2[num].x, array2[num].z, 0f - array2[num].y, array2[num].w);
			}
		}
		mesh.set_vertices(vertices);
		if (array != null)
		{
			mesh.set_normals(array);
		}
		if (array2 != null)
		{
			mesh.set_tangents(array2);
		}
		mesh.RecalculateBounds();
	}

	public FixFbxImport()
		: this()
	{
	}
}

using UnityEngine;

public class FantasticMissingMeshFinder
{
	private static void FindMissingMeshes()
	{
		MeshFilter[] array = Object.FindObjectsOfType<MeshFilter>();
		foreach (MeshFilter val in array)
		{
			if ((Object)(object)val.get_sharedMesh() == (Object)null)
			{
				Debug.Log((object)(((Object)((Component)val).get_gameObject()).get_name() + " has a missing mesh"), (Object)(object)((Component)val).get_gameObject());
			}
		}
	}

	private static void FindMissingMeshColliders()
	{
		MeshCollider[] array = Object.FindObjectsOfType<MeshCollider>();
		foreach (MeshCollider val in array)
		{
			if ((Object)(object)val.get_sharedMesh() == (Object)null)
			{
				Debug.Log((object)(((Object)((Component)val).get_gameObject()).get_name() + " has a missing mesh collider"), (Object)(object)((Component)val).get_gameObject());
			}
		}
	}

	private static void FindMeshesWithoutColliders()
	{
		MeshRenderer[] array = Object.FindObjectsOfType<MeshRenderer>();
		foreach (MeshRenderer val in array)
		{
			if ((Object)(object)((Component)val).get_gameObject().GetComponent<Collider>() == (Object)null)
			{
				Debug.Log((object)(((Object)((Component)val).get_gameObject()).get_name() + " has a mesh renderer but no collider. Player may clip  through it"), (Object)(object)((Component)val).get_gameObject());
			}
		}
	}
}

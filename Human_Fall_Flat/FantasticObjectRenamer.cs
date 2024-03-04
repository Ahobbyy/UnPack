using UnityEditor;
using UnityEngine;

public class FantasticObjectRenamer
{
	private static void RenameFromMeshName()
	{
		GameObject[] gameObjects = Selection.get_gameObjects();
		foreach (GameObject val in gameObjects)
		{
			if ((Object)(object)val.GetComponent<MeshFilter>() != (Object)null)
			{
				((Object)val).set_name(((Object)val.GetComponent<MeshFilter>().get_sharedMesh()).get_name());
			}
		}
	}
}

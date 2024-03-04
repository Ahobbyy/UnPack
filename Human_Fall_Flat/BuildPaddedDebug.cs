using UnityEngine;

public class BuildPaddedDebug : MonoBehaviour
{
	private void Start()
	{
		((Component)this).GetComponent<MeshFilter>().set_sharedMesh(BuildPaddedMesh.GeneratePadded(((Component)this).GetComponent<MeshFilter>().get_sharedMesh(), 0.1f));
	}

	public BuildPaddedDebug()
		: this()
	{
	}
}

using UnityEngine;

public class WaterRender : MonoBehaviour
{
	public float scale = 1f;

	[Space]
	public bool useTimeSinceLevelLoad;

	private Material material;

	private void Start()
	{
		MeshRenderer component = ((Component)this).GetComponent<MeshRenderer>();
		if ((Object)(object)component != (Object)null)
		{
			material = ((Renderer)component).get_material();
			((Renderer)component).set_sharedMaterial(material);
		}
		SkinnedMeshRenderer component2 = ((Component)this).GetComponent<SkinnedMeshRenderer>();
		if ((Object)(object)component2 != (Object)null)
		{
			material = ((Renderer)component2).get_material();
			((Renderer)component2).set_sharedMaterial(material);
		}
		material.SetFloat("_Scale", scale);
	}

	private void LateUpdate()
	{
		float num = (useTimeSinceLevelLoad ? Time.get_timeSinceLevelLoad() : ReplayRecorder.time);
		material.SetFloat("_Time2", num);
	}

	public WaterRender()
		: this()
	{
	}
}

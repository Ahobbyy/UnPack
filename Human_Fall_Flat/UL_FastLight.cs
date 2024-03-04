using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("UPGEN Lighting/UPGEN Fast Light")]
[ExecuteInEditMode]
public sealed class UL_FastLight : MonoBehaviour
{
	public Color color = Color.get_white();

	public float intensity = 1f;

	public float range = 10f;

	public static readonly List<UL_FastLight> all = new List<UL_FastLight>();

	private void OnEnable()
	{
		all.Add(this);
	}

	private void OnDisable()
	{
		all.Remove(this);
	}

	internal void GenerateRenderData()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		UL_Renderer.Add(((Component)this).get_transform().get_position(), range, intensity * intensity * ((Color)(ref color)).get_linear());
	}

	public UL_FastLight()
		: this()
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0006: Unknown result type (might be due to invalid IL or missing references)

}

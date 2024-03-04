using UnityEngine;

[ExecuteInEditMode]
public class pg_SceneMeshRender : MonoBehaviour
{
	private HideFlags SceneCameraHideFlags = (HideFlags)13;

	public Mesh mesh;

	public Material material;

	private void OnDestroy()
	{
		if (Object.op_Implicit((Object)(object)mesh))
		{
			Object.DestroyImmediate((Object)(object)mesh);
		}
		if (Object.op_Implicit((Object)(object)material))
		{
			Object.DestroyImmediate((Object)(object)material);
		}
	}

	private void OnRenderObject()
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		if ((HideFlags)(((Object)((Component)Camera.get_current()).get_gameObject()).get_hideFlags() & SceneCameraHideFlags) == SceneCameraHideFlags && !(((Object)Camera.get_current()).get_name() != "SceneCamera"))
		{
			if ((Object)(object)material == (Object)null || (Object)(object)mesh == (Object)null)
			{
				Object.DestroyImmediate((Object)(object)((Component)this).get_gameObject());
				return;
			}
			material.SetPass(0);
			Graphics.DrawMeshNow(mesh, Vector3.get_zero(), Quaternion.get_identity(), 0);
		}
	}

	public pg_SceneMeshRender()
		: this()
	{
	}//IL_0003: Unknown result type (might be due to invalid IL or missing references)

}

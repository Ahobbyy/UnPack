using UnityEngine;

public class RagdollPaddedModel : MonoBehaviour
{
	private Renderer[] renderers;

	public void CreatePaddedMesh(Renderer[] modelRenderers)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Expected O, but got Unknown
		renderers = (Renderer[])(object)new Renderer[modelRenderers.Length];
		for (int i = 0; i < modelRenderers.Length; i++)
		{
			Mesh val;
			if (modelRenderers[i] is SkinnedMeshRenderer)
			{
				val = new Mesh();
				Renderer obj = modelRenderers[i];
				((SkinnedMeshRenderer)((obj is SkinnedMeshRenderer) ? obj : null)).BakeMesh(val);
			}
			else
			{
				val = ((Component)modelRenderers[i]).GetComponent<MeshFilter>().get_sharedMesh();
			}
			val = BuildPaddedMesh.GeneratePadded(val, 0.00390625f);
			GameObject val2 = new GameObject("UnwrapMesh");
			val2.get_transform().SetParent(((Component)modelRenderers[i]).get_transform(), false);
			val2.set_layer(31);
			val2.AddComponent<MeshFilter>().set_sharedMesh(val);
			renderers[i] = (Renderer)(object)val2.AddComponent<MeshRenderer>();
			renderers[i].set_enabled(false);
		}
	}

	public void Teardown()
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			Object.Destroy((Object)(object)((Component)renderers[i]).get_gameObject());
		}
		renderers = null;
	}

	public void Enable(bool enable)
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].set_enabled(enable);
		}
	}

	public void SetMaterial(Material material)
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].set_sharedMaterial(material);
		}
	}

	public RagdollPaddedModel()
		: this()
	{
	}
}

using System.Collections.Generic;
using UnityEngine;

public class RagdollTransparency : MonoBehaviour
{
	private CameraController3 cameraController;

	public List<Material> materials = new List<Material>();

	private float cutoutTimer;

	private bool replaced;

	public void Initialize(CameraController3 cameraController, Ragdoll ragdoll)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		this.cameraController = cameraController;
		ClearMaterials();
		Dictionary<Material, Material> dictionary = new Dictionary<Material, Material>();
		Renderer[] componentsInChildren = ((Component)ragdoll).GetComponentsInChildren<Renderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			Material[] sharedMaterials = componentsInChildren[i].get_sharedMaterials();
			for (int j = 0; j < sharedMaterials.Length; j++)
			{
				if ("Standard".Equals(((Object)sharedMaterials[j].get_shader()).get_name()) && sharedMaterials[j].GetFloat("_Mode") != 3f)
				{
					Material value = null;
					if (!dictionary.TryGetValue(sharedMaterials[j], out value))
					{
						value = new Material(sharedMaterials[j]);
						dictionary[sharedMaterials[j]] = value;
						materials.Add(value);
					}
					sharedMaterials[j] = value;
				}
			}
			componentsInChildren[i].set_sharedMaterials(sharedMaterials);
		}
	}

	private void ClearMaterials()
	{
		for (int i = 0; i < materials.Count; i++)
		{
			Object.Destroy((Object)(object)materials[i]);
		}
		materials.Clear();
	}

	private void OnDestroy()
	{
		ClearMaterials();
	}

	public void OnPreRender()
	{
		if ((Object)(object)cameraController != (Object)null && (cameraController.offset < 0.8f || cutoutTimer > 0f))
		{
			replaced = true;
			if (cameraController.offset < 0.8f)
			{
				cutoutTimer = Mathf.Clamp01(cutoutTimer + Time.get_deltaTime());
			}
			else
			{
				cutoutTimer = Mathf.Clamp01(cutoutTimer - Time.get_deltaTime());
			}
			float num = ((cameraController.mode == CameraMode.FirstPerson) ? 1.2f : (cutoutTimer * 1.25f - 0.25f));
			for (int i = 0; i < materials.Count; i++)
			{
				if ((Object)(object)materials[i].GetTexture("_MetallicGlossMap") != (Object)null)
				{
					materials[i].set_shader(Shaders.instance.transparentHumanShaderMetal);
				}
				else
				{
					materials[i].set_shader(Shaders.instance.transparentHumanShader);
				}
				materials[i].SetFloat("_ClipDist", num);
			}
		}
		else
		{
			replaced = false;
		}
	}

	public void OnPostRender()
	{
		if (replaced)
		{
			for (int i = 0; i < materials.Count; i++)
			{
				materials[i].set_shader(Shaders.instance.opaqueHumanShader);
			}
		}
	}

	private void SetTransparentBlendMode(Material material)
	{
		material.SetOverrideTag("RenderType", "Transparent");
		material.SetInt("_SrcBlend", 1);
		material.SetInt("_DstBlend", 10);
		material.SetInt("_ZWrite", 0);
		material.DisableKeyword("_ALPHATEST_ON");
		material.DisableKeyword("_ALPHABLEND_ON");
		material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
		material.set_renderQueue(3000);
	}

	public RagdollTransparency()
		: this()
	{
	}
}

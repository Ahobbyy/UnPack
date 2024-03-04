using UnityEngine;
using UnityEngine.Rendering;

public class CaveShadowDisable : MonoBehaviour
{
	private bool shadowsEnabled = true;

	private MeshRenderer[] renderers;

	private void OnEnable()
	{
		renderers = ((Component)this).GetComponentsInChildren<MeshRenderer>();
	}

	public void DisableShadow()
	{
		if (shadowsEnabled)
		{
			shadowsEnabled = false;
			for (int i = 0; i < renderers.Length; i++)
			{
				((Renderer)renderers[i]).set_shadowCastingMode((ShadowCastingMode)0);
			}
		}
	}

	public void EnableShadow()
	{
		if (!shadowsEnabled)
		{
			shadowsEnabled = true;
			for (int i = 0; i < renderers.Length; i++)
			{
				((Renderer)renderers[i]).set_shadowCastingMode((ShadowCastingMode)1);
			}
		}
	}

	public CaveShadowDisable()
		: this()
	{
	}
}

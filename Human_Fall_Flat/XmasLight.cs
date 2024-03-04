using UnityEngine;
using UnityEngine.Rendering;

public class XmasLight : MonoBehaviour
{
	public int priority;

	private Light light;

	private int syncedShadows = -1;

	private void Awake()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Expected I4, but got Unknown
		light = ((Component)this).GetComponent<Light>();
		int num = Options.advancedVideoShadows - priority;
		if (priority > 0)
		{
			light.set_shadowResolution((LightShadowResolution)Mathf.Max(0, QualitySettings.get_shadowResolution() - 1));
		}
		if (num >= 2)
		{
			((Behaviour)light).set_enabled(true);
			light.set_shadows((LightShadows)2);
		}
		else if (num >= 1)
		{
			((Behaviour)light).set_enabled(true);
			light.set_shadows((LightShadows)1);
		}
		else if (num >= 0)
		{
			((Behaviour)light).set_enabled(true);
			light.set_shadows((LightShadows)0);
		}
		else
		{
			((Behaviour)light).set_enabled(false);
		}
	}

	public XmasLight()
		: this()
	{
	}
}

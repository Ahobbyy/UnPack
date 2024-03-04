using UnityEngine;

public class QualitySettingsOverride : MonoBehaviour
{
	[Header("Shadow Distance")]
	public bool overrideShadowDistance;

	public float maxShadowDistance = 50f;

	private float cachedShadowDistance;

	[Header("Shadow Cascade")]
	public bool overrideShadowCascade;

	public int maxShadowCascade = 2;

	private int cachedShadowCascade;

	[Header("LOD Bias")]
	public bool overrideLODBias;

	public float LODBias = 2f;

	private float cachedLODBias;

	private void OnEnable()
	{
		if (overrideShadowDistance)
		{
			cachedShadowDistance = QualitySettings.get_shadowDistance();
			if (maxShadowDistance < cachedShadowDistance)
			{
				QualitySettings.set_shadowDistance(maxShadowDistance);
			}
		}
		if (overrideShadowCascade)
		{
			cachedShadowCascade = QualitySettings.get_shadowCascades();
			if (maxShadowCascade < cachedShadowCascade)
			{
				QualitySettings.set_shadowCascades(maxShadowCascade);
			}
		}
		if (overrideLODBias)
		{
			cachedLODBias = QualitySettings.get_lodBias();
			QualitySettings.set_lodBias(LODBias);
		}
	}

	private void OnDisable()
	{
		if (overrideShadowDistance)
		{
			QualitySettings.set_shadowDistance(cachedShadowDistance);
		}
		if (overrideShadowCascade)
		{
			QualitySettings.set_shadowCascades(cachedShadowCascade);
		}
		if (overrideLODBias)
		{
			QualitySettings.set_lodBias(cachedLODBias);
		}
	}

	public QualitySettingsOverride()
		: this()
	{
	}
}

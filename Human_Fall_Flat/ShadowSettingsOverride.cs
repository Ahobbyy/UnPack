using UnityEngine;

public class ShadowSettingsOverride : MonoBehaviour
{
	private void Update()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Invalid comparison between Unknown and I4
		if ((int)QualitySettings.get_shadows() != 2)
		{
			QualitySettings.set_shadows((ShadowQuality)2);
		}
	}

	public ShadowSettingsOverride()
		: this()
	{
	}
}

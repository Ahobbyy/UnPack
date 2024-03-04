using UnityEngine;

public class SwitchBrightness : MonoBehaviour
{
	public Shader shaderBrightness;

	private static float brightnessPower = 1f;

	private Material materialBrightness;

	private const int kMaxBrightness = 20;

	private const int kMidBrightness = 10;

	private const int kMinBrightness = 0;

	private const float kMaxBrightnessFloat = 20f;

	private const float kMidBrightnessPower = 1f;

	private const float kMinBrightnessCoef = 0.25f;

	private const float kMaxBrightnessCoef = 1.75f;

	public static void SetSwitchBrightness(int brightnessLevel)
	{
		if (brightnessLevel == 10 || brightnessLevel < 0 || brightnessLevel > 20)
		{
			brightnessPower = 1f;
			return;
		}
		brightnessPower = Mathf.Lerp(0.25f, 1.75f, (float)brightnessLevel / 20f);
		Debug.Log((object)("SetSwitchBrightness: " + brightnessLevel + " " + brightnessPower));
	}

	public bool CheckResources()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if ((Object)(object)materialBrightness == (Object)null)
		{
			materialBrightness = new Material(shaderBrightness);
			((Object)materialBrightness).set_hideFlags((HideFlags)52);
		}
		return true;
	}

	public SwitchBrightness()
		: this()
	{
	}
}

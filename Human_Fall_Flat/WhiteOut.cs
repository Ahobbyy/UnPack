using UnityEngine;

public sealed class WhiteOut : MonoBehaviour
{
	public Shader shaderWhiteOut;

	private Material materialWhiteOut;

	private float mWhiteProportion = 1f;

	public void SetWhiteProportion(float proportion)
	{
		mWhiteProportion = proportion;
	}

	public bool CheckResources()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Expected O, but got Unknown
		if ((Object)(object)materialWhiteOut == (Object)null)
		{
			materialWhiteOut = new Material(shaderWhiteOut);
			((Object)materialWhiteOut).set_hideFlags((HideFlags)52);
		}
		return true;
	}

	public void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (CheckResources() && (Object)(object)materialWhiteOut != (Object)null)
		{
			materialWhiteOut.SetFloat("_WhiteProportion", mWhiteProportion);
			Graphics.Blit((Texture)(object)source, destination, materialWhiteOut);
		}
	}

	public WhiteOut()
		: this()
	{
	}
}

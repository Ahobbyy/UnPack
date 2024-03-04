using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : MonoBehaviour
	{
		public Shader shaderFXAAIII;

		public AAMode mode = AAMode.FXAA3Console;

		public float edgeThresholdMin = 0.05f;

		public float edgeThreshold = 0.2f;

		public float edgeSharpness = 4f;

		private Material materialFXAAIII;

		public Material CurrentAAMaterial()
		{
			Material val = null;
			if (mode == AAMode.FXAA3Console)
			{
				return materialFXAAIII;
			}
			return null;
		}

		public bool CheckResources()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Expected O, but got Unknown
			if ((Object)(object)materialFXAAIII == (Object)null)
			{
				materialFXAAIII = new Material(shaderFXAAIII);
				((Object)materialFXAAIII).set_hideFlags((HideFlags)52);
			}
			return true;
		}

		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (CheckResources() && mode == AAMode.FXAA3Console && (Object)(object)materialFXAAIII != (Object)null)
			{
				materialFXAAIII.SetFloat("_EdgeThresholdMin", edgeThresholdMin);
				materialFXAAIII.SetFloat("_EdgeThreshold", edgeThreshold);
				materialFXAAIII.SetFloat("_EdgeSharpness", edgeSharpness);
				Graphics.Blit((Texture)(object)source, destination, materialFXAAIII);
			}
		}

		public Antialiasing()
			: this()
		{
		}
	}
}

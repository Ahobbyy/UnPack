using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public sealed class UPGEN_Lighting_Renderer : PostProcessEffectRenderer<UPGEN_Lighting>
{
	private const string SHADER = "Hidden/Shader/UPGEN_Lighting";

	private static Shader _shader;

	public override void Render(PostProcessRenderContext context)
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_shader == (Object)null)
		{
			_shader = Shader.Find("Hidden/Shader/UPGEN_Lighting");
		}
		PropertySheet propertySheet = context.propertySheets.Get(_shader);
		Camera camera = context.camera;
		propertySheet.properties.SetFloat("_Intensity", base.settings.intensity.value);
		UL_Renderer.SetupForCamera(camera, propertySheet.properties);
		context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, 0);
	}
}

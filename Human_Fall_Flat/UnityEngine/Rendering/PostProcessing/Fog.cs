using System;

namespace UnityEngine.Rendering.PostProcessing
{
	[Serializable]
	public sealed class Fog
	{
		[Tooltip("Enables the internal deferred fog pass. Actual fog settings should be set in the Lighting panel.")]
		public bool enabled = true;

		[Tooltip("Should the fog affect the skybox?")]
		public bool excludeSkybox = true;

		internal DepthTextureMode GetCameraFlags()
		{
			return (DepthTextureMode)1;
		}

		internal bool IsEnabledAndSupported(PostProcessRenderContext context)
		{
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Invalid comparison between Unknown and I4
			if (enabled && RenderSettings.get_fog() && !RuntimeUtilities.scriptableRenderPipelineActive && Object.op_Implicit((Object)(object)context.resources.shaders.deferredFog) && context.resources.shaders.deferredFog.get_isSupported())
			{
				return (int)context.camera.get_actualRenderingPath() == 3;
			}
			return false;
		}

		internal void Render(PostProcessRenderContext context)
		{
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			PropertySheet propertySheet = context.propertySheets.Get(context.resources.shaders.deferredFog);
			propertySheet.ClearKeywords();
			Color val;
			if (!RuntimeUtilities.isLinearColorSpace)
			{
				val = RenderSettings.get_fogColor();
			}
			else
			{
				Color fogColor = RenderSettings.get_fogColor();
				val = ((Color)(ref fogColor)).get_linear();
			}
			Color val2 = val;
			propertySheet.properties.SetVector(ShaderIDs.FogColor, Color.op_Implicit(val2));
			propertySheet.properties.SetVector(ShaderIDs.FogParams, Vector4.op_Implicit(new Vector3(RenderSettings.get_fogDensity(), RenderSettings.get_fogStartDistance(), RenderSettings.get_fogEndDistance())));
			context.command.BlitFullscreenTriangle(context.source, context.destination, propertySheet, excludeSkybox ? 1 : 0);
		}
	}
}

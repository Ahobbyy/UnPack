using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;

public sealed class BlurRenderer : PostProcessEffectRenderer<Blur>
{
	private int[] ids;

	public override void Init()
	{
		base.Init();
		ids = new int[(int)base.settings.blurIterations * 2 + 1];
		int num = 0;
		ids[num++] = Shader.PropertyToID("_RT");
		for (int i = 0; i < (int)base.settings.blurIterations; i++)
		{
			ids[num++] = Shader.PropertyToID("_RT1" + i);
			ids[num++] = Shader.PropertyToID("_RT2" + i);
		}
	}

	public override void Render(PostProcessRenderContext context)
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019a: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023e: Unknown result type (might be due to invalid IL or missing references)
		CommandBuffer command = context.command;
		command.BeginSample("Blur");
		PropertySheet propertySheet = context.propertySheets.Get(Shader.Find("Hidden/Custom/Blur"));
		float num = 1f / (1f * (float)(1 << (int)base.settings.downsample));
		propertySheet.properties.SetVector("_Parameter", new Vector4((float)base.settings.blurSize * num, (0f - (float)base.settings.blurSize) * num, 0f, 0f));
		int widthOverride = context.screenWidth >> (int)base.settings.downsample;
		int heightOverride = context.screenHeight >> (int)base.settings.downsample;
		int num2 = 0;
		context.GetScreenSpaceTemporaryRT(command, ids[num2], 0, context.sourceFormat, (RenderTextureReadWrite)0, (FilterMode)1, widthOverride, heightOverride);
		command.BlitFullscreenTriangle(context.source, RenderTargetIdentifier.op_Implicit(ids[num2]), propertySheet, 0);
		for (int i = 0; i < (int)base.settings.blurIterations; i++)
		{
			float num3 = (float)i * 1f;
			propertySheet.properties.SetVector("_Parameter", new Vector4((float)base.settings.blurSize * num + num3, (0f - (float)base.settings.blurSize) * num - num3, 0f, 0f));
			context.GetScreenSpaceTemporaryRT(command, ids[++num2], 0, context.sourceFormat, (RenderTextureReadWrite)0, (FilterMode)1, widthOverride, heightOverride);
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(ids[num2 - 1]), RenderTargetIdentifier.op_Implicit(ids[num2]), propertySheet, 1);
			command.ReleaseTemporaryRT(ids[num2 - 1]);
			context.GetScreenSpaceTemporaryRT(command, ids[++num2], 0, context.sourceFormat, (RenderTextureReadWrite)0, (FilterMode)1, widthOverride, heightOverride);
			command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(ids[num2 - 1]), RenderTargetIdentifier.op_Implicit(ids[num2]), propertySheet, 2);
			command.ReleaseTemporaryRT(ids[num2 - 1]);
		}
		command.BlitFullscreenTriangle(RenderTargetIdentifier.op_Implicit(ids[num2]), context.destination, propertySheet, 0);
		command.ReleaseTemporaryRT(ids[num2]);
		command.EndSample("Blur");
	}
}

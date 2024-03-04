using System.Collections;
using System.Collections.Generic;
using HumanAPI;
using UnityEngine;

public class RagdollTexture : MonoBehaviour
{
	private WorkshopItemType part;

	private RagdollModel model;

	public Texture texture;

	public Texture2D baseTexture;

	public Texture2D bakedTexture;

	public RenderTexture renderTexture;

	private bool baseTextureIsAsset;

	private bool maskTextureIsAsset;

	public bool paintingEnabled;

	public int width;

	public int height;

	private string savePath;

	private bool textureLoadSuppressed;

	private Color appliedColor1;

	private Color appliedColor2;

	private Color appliedColor3;

	private Coroutine bakeCoroutine;

	private List<Texture2D> undo = new List<Texture2D>();

	private List<Texture2D> redo = new List<Texture2D>();

	public Texture paintBase;

	public RenderTexture paintSource;

	public RenderTexture paintTarget;

	private RenderBuffer[] buffers = (RenderBuffer[])(object)new RenderBuffer[2];

	private void ChangeBaseTexture(Texture2D newRes, bool isAsset)
	{
		if (!((Object)(object)newRes == (Object)(object)baseTexture))
		{
			if ((Object)(object)baseTexture != (Object)null)
			{
				ReleaseBaseTexture();
			}
			else if (baseTextureIsAsset)
			{
				TextureTracker.instance.RemoveMapping((Object)(object)this, null);
			}
			baseTexture = newRes;
			baseTextureIsAsset = isAsset;
			if (isAsset && (Object)(object)newRes != (Object)null)
			{
				TextureTracker.instance.AddMapping((Object)(object)this, (Object)(object)newRes, TextureTracker.DontUnloadAsset);
			}
		}
	}

	private void ChangeMaskTexture(Texture2D newRes, bool isAsset)
	{
		if (!((Object)(object)model == (Object)null) && !((Object)(object)newRes == (Object)(object)model.maskTexture))
		{
			ReleaseMaskTexture();
			model.maskTexture = newRes;
			maskTextureIsAsset = isAsset;
			if (isAsset && (Object)(object)newRes != (Object)null)
			{
				TextureTracker.instance.AddMapping((Object)(object)model, (Object)(object)newRes, TextureTracker.DontUnloadAsset);
			}
		}
	}

	public void BindToModel(RagdollModel model)
	{
		part = model.ragdollPart;
		this.model = model;
	}

	public void LoadFromPreset(RagdollPresetMetadata preset)
	{
		ReleaseBaseTexture();
		savePath = FileTools.Combine(preset.folder, part.ToString() + ".png");
		textureLoadSuppressed = preset.GetPart(part).suppressCustomTexture;
		if (!textureLoadSuppressed)
		{
			RagdollPresetPartMetadata ragdollPresetPartMetadata = preset.GetPart(part);
			if (ragdollPresetPartMetadata != null && ragdollPresetPartMetadata.bytes != null)
			{
				ChangeBaseTexture(FileTools.TextureFromBytes(part.ToString(), ragdollPresetPartMetadata.bytes), isAsset: false);
			}
			else if (!string.IsNullOrEmpty(savePath))
			{
				bool isAsset;
				Texture2D newRes = FileTools.ReadTexture(savePath, out isAsset);
				ChangeBaseTexture(newRes, isAsset);
			}
			if ((Object)(object)baseTexture != (Object)null)
			{
				baseTexture.Compress(true);
				baseTexture.Apply(true);
			}
		}
		if (model.meta.metaPath.StartsWith("builtin"))
		{
			if ((Object)(object)baseTexture == (Object)null)
			{
				ChangeBaseTexture(HFFResources.instance.FindTextureResource("SkinTextures/" + ((Object)model.meta.modelPrefab).get_name() + "Color"), isAsset: true);
			}
			if ((Object)(object)model.maskTexture == (Object)null)
			{
				ChangeMaskTexture(HFFResources.instance.FindTextureResource("SkinTextures/" + ((Object)model.meta.modelPrefab).get_name() + "Mask"), isAsset: true);
			}
		}
		if ((Object)(object)baseTexture != (Object)null)
		{
			width = ((Texture)baseTexture).get_width();
			height = ((Texture)baseTexture).get_height();
			paintingEnabled = true;
		}
		else if ((Object)(object)model.maskTexture != (Object)null)
		{
			width = ((Texture)model.maskTexture).get_width();
			height = ((Texture)model.maskTexture).get_height();
			paintingEnabled = true;
		}
		else
		{
			width = (height = 2048);
			paintingEnabled = true;
		}
	}

	public void SaveToPreset()
	{
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Expected O, but got Unknown
		for (int i = 0; i < undo.Count; i++)
		{
			Object.Destroy((Object)(object)undo[i]);
		}
		undo.Clear();
		for (int j = 0; j < redo.Count; j++)
		{
			Object.Destroy((Object)(object)redo[j]);
		}
		redo.Clear();
		FileTools.WriteTexture(savePath, bakedTexture);
		ReleaseBaseTexture();
		ChangeBaseTexture(bakedTexture, isAsset: false);
		bakedTexture = new Texture2D(width, height, (TextureFormat)3, false);
		((Object)bakedTexture).set_name("SavedClone" + ((Object)this).get_name());
		Graphics.CopyTexture((Texture)(object)baseTexture, (Texture)(object)bakedTexture);
	}

	private void ApplyColors(Color color1, Color color2, Color color3, bool bake, bool compress = false)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Expected O, but got Unknown
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		if (!paintingEnabled)
		{
			return;
		}
		appliedColor1 = color1;
		appliedColor2 = color2;
		appliedColor3 = color3;
		if ((Object)(object)renderTexture == (Object)null)
		{
			renderTexture = new RenderTexture(width, height, 0, (RenderTextureFormat)0);
			((Object)renderTexture).set_name("Colors" + ((Object)this).get_name());
		}
		Material val = new Material(Shaders.instance.applyMaskedColors);
		val.set_mainTexture((Texture)(object)baseTexture);
		val.SetTexture("_MaskTex", (Texture)(object)model.maskTexture);
		val.SetColor("_Color1", (Color)(model.mask1 ? ((Color)(ref color1)).get_linear() : default(Color)));
		val.SetColor("_Color2", (Color)(model.mask2 ? ((Color)(ref color2)).get_linear() : default(Color)));
		val.SetColor("_Color3", (Color)(model.mask3 ? ((Color)(ref color3)).get_linear() : default(Color)));
		Graphics.Blit((Texture)null, renderTexture, val);
		Object.Destroy((Object)(object)val);
		if (bake)
		{
			BakeTexture(renderTexture, compress);
			Object.Destroy((Object)(object)renderTexture);
			return;
		}
		ApplyTexture((Texture)(object)renderTexture);
		if (bakeCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(bakeCoroutine);
		}
		bakeCoroutine = ((MonoBehaviour)this).StartCoroutine(BakeDelayed());
	}

	private IEnumerator BakeDelayed()
	{
		yield return (object)new WaitForSeconds(1f);
		EndStrokeFinalize();
	}

	public void ApplyPresetColors(RagdollPresetMetadata preset, bool bake, bool compress)
	{
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		string text = FileTools.Combine(preset.folder, part.ToString() + ".png");
		bool suppressCustomTexture = preset.GetPart(part).suppressCustomTexture;
		if (savePath != text || textureLoadSuppressed != suppressCustomTexture)
		{
			LoadFromPreset(preset);
		}
		RagdollPresetPartMetadata ragdollPresetPartMetadata = preset.GetPart(part);
		ApplyColors(Color32.op_Implicit(HexConverter.HexToColor(ragdollPresetPartMetadata.color1, default(Color))), Color32.op_Implicit(HexConverter.HexToColor(ragdollPresetPartMetadata.color2, default(Color))), Color32.op_Implicit(HexConverter.HexToColor(ragdollPresetPartMetadata.color3, default(Color))), bake, compress);
	}

	private void ApplyTexture(Texture texture)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)texture != (Object)null && (int)texture.get_wrapMode() != 0)
		{
			texture.set_wrapMode((TextureWrapMode)0);
		}
		this.texture = texture;
		model.SetTexture(texture);
	}

	public void BakeTexture(RenderTexture rt, bool compress)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)bakedTexture == (Object)null)
		{
			bakedTexture = new Texture2D(width, height, (TextureFormat)3, true);
			((Object)bakedTexture).set_name("Baked" + ((Object)this).get_name());
		}
		RenderTexture.set_active(rt);
		bakedTexture.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0, true);
		if (compress)
		{
			bakedTexture.Compress(true);
			bakedTexture.Apply(true, true);
			ReleaseBaseTexture();
			ReleaseMaskTexture();
		}
		else
		{
			bakedTexture.Apply(true);
		}
		RenderTexture.set_active((RenderTexture)null);
		ApplyTexture((Texture)(object)bakedTexture);
	}

	public void BakeTextureNoMip(RenderTexture rt)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Expected O, but got Unknown
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)bakedTexture == (Object)null)
		{
			bakedTexture = new Texture2D(width, height, (TextureFormat)3, false);
			((Object)bakedTexture).set_name("BakedNoMIP" + ((Object)this).get_name());
		}
		RenderTexture.set_active(rt);
		bakedTexture.ReadPixels(new Rect(0f, 0f, (float)width, (float)height), 0, 0, true);
		bakedTexture.Apply(true);
		RenderTexture.set_active((RenderTexture)null);
	}

	public void EnterPaint()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		paintSource = new RenderTexture(width, height, 0, (RenderTextureFormat)0);
		paintTarget = new RenderTexture(width, height, 0, (RenderTextureFormat)0);
		((Object)paintSource).set_name("PaintSource" + ((Object)this).get_name());
		((Object)paintTarget).set_name("PaintTarget" + ((Object)this).get_name());
		ClearPaintTextures();
	}

	public void BeginStroke()
	{
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Expected O, but got Unknown
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Expected O, but got Unknown
		EnsureBaked();
		paintBase = (Texture)(object)bakedTexture;
		undo.Add(bakedTexture);
		bakedTexture = null;
		if (undo.Count > 8)
		{
			Object.Destroy((Object)(object)undo[0]);
			undo.RemoveAt(0);
		}
		if (redo.Count > 0)
		{
			for (int i = 0; i < redo.Count; i++)
			{
				Object.Destroy((Object)(object)redo[i]);
			}
			redo.Clear();
		}
		renderTexture = new RenderTexture(width, height, 0, (RenderTextureFormat)0);
		while ((Object)(object)renderTexture == (Object)null && undo.Count > 0)
		{
			Object.Destroy((Object)(object)undo[0]);
			undo.RemoveAt(0);
			renderTexture = new RenderTexture(width, height, 0, (RenderTextureFormat)0);
		}
		((Object)renderTexture).set_name("Stroke" + ((Object)this).get_name());
		ApplyTexture((Texture)(object)renderTexture);
		if (!paintSource.IsCreated())
		{
			paintSource.Create();
			ClearPaintTextures();
		}
		Graphics.Blit(paintBase, renderTexture);
	}

	private void EnsureBaked()
	{
		if (bakeCoroutine != null)
		{
			EndStrokeFinalize();
		}
	}

	private void ClearPaintTextures()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Graphics.SetRenderTarget(paintSource);
		GL.Clear(false, true, default(Color));
		Graphics.SetRenderTarget((RenderTexture)null);
	}

	public void EndStroke()
	{
		ClearPaintTextures();
		bakeCoroutine = ((MonoBehaviour)this).StartCoroutine(EndStrokeFinalizeWait());
	}

	public void CancelStroke()
	{
		ClearPaintTextures();
		bakedTexture = undo[undo.Count - 1];
		ApplyTexture((Texture)(object)bakedTexture);
		undo.RemoveAt(undo.Count - 1);
		Object.Destroy((Object)(object)renderTexture);
		renderTexture = null;
	}

	private IEnumerator EndStrokeFinalizeWait()
	{
		for (int i = 0; i < (int)part; i++)
		{
			yield return null;
		}
		EndStrokeFinalize();
	}

	private void EndStrokeFinalize()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		if (!renderTexture.IsCreated())
		{
			ApplyColors(appliedColor1, appliedColor2, appliedColor3, bake: true);
			return;
		}
		BakeTextureNoMip(renderTexture);
		ApplyTexture((Texture)(object)bakedTexture);
		Object.Destroy((Object)(object)renderTexture);
		renderTexture = null;
		if (bakeCoroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(bakeCoroutine);
		}
		bakeCoroutine = null;
	}

	public void PaintStep(Camera camera, Material unprojectMaterial, int mask)
	{
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		unprojectMaterial.SetFloat("_TexSize", (float)width);
		unprojectMaterial.SetTexture("_ModelTex", paintBase);
		unprojectMaterial.SetTexture("_BaseTex", (Texture)(object)paintSource);
		unprojectMaterial.SetTexture("_MaskTex", (Texture)(object)model.maskTexture);
		unprojectMaterial.SetFloat("_Mask1", (float)(((mask & 1) == 1) ? 1 : 0));
		unprojectMaterial.SetFloat("_Mask2", (float)(((mask & 2) == 2) ? 1 : 0));
		unprojectMaterial.SetFloat("_Mask3", (float)(((mask & 4) == 4) ? 1 : 0));
		buffers[0] = renderTexture.get_colorBuffer();
		buffers[1] = paintTarget.get_colorBuffer();
		camera.SetTargetBuffers(buffers, renderTexture.get_depthBuffer());
		RenderTexture val = paintSource;
		paintSource = paintTarget;
		paintTarget = val;
	}

	public void Undo()
	{
		if (undo.Count != 0)
		{
			EnsureBaked();
			redo.Add(bakedTexture);
			bakedTexture = undo[undo.Count - 1];
			ApplyTexture((Texture)(object)bakedTexture);
			undo.RemoveAt(undo.Count - 1);
		}
	}

	public void Redo()
	{
		if (redo.Count != 0)
		{
			undo.Add(bakedTexture);
			bakedTexture = redo[redo.Count - 1];
			ApplyTexture((Texture)(object)bakedTexture);
			redo.RemoveAt(redo.Count - 1);
		}
	}

	public void LeavePaint()
	{
		for (int i = 0; i < undo.Count; i++)
		{
			Object.Destroy((Object)(object)undo[i]);
		}
		undo.Clear();
		for (int j = 0; j < redo.Count; j++)
		{
			Object.Destroy((Object)(object)redo[j]);
		}
		redo.Clear();
		if ((Object)(object)paintSource != (Object)null)
		{
			Object.Destroy((Object)(object)paintSource);
		}
		if ((Object)(object)paintTarget != (Object)null)
		{
			Object.Destroy((Object)(object)paintTarget);
		}
	}

	private void ReleaseBaseTexture()
	{
		if (baseTextureIsAsset)
		{
			TextureTracker.instance.RemoveMapping((Object)(object)this, (Object)(object)baseTexture);
		}
		else if ((Object)(object)baseTexture != (Object)null)
		{
			Object.Destroy((Object)(object)baseTexture);
		}
		baseTexture = null;
		baseTextureIsAsset = false;
	}

	private void ReleaseMaskTexture()
	{
		if (maskTextureIsAsset)
		{
			TextureTracker.instance.RemoveMapping((Object)(object)model, (Object)(object)model.maskTexture);
		}
		model.maskTexture = null;
		maskTextureIsAsset = false;
	}

	private void OnDestroy()
	{
		LeavePaint();
		ReleaseBaseTexture();
		ReleaseMaskTexture();
		if ((Object)(object)bakedTexture != (Object)null)
		{
			Object.Destroy((Object)(object)bakedTexture);
		}
		if ((Object)(object)renderTexture != (Object)null)
		{
			Object.Destroy((Object)(object)renderTexture);
		}
	}

	public RagdollTexture()
		: this()
	{
	}
}

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(CanvasRenderer))]
	[AddComponentMenu("UI/TextMeshPro - Text (UI)", 11)]
	[SelectionBase]
	public class TextMeshProUGUI : TMP_Text, ILayoutElement
	{
		private bool m_isRebuildingLayout;

		[SerializeField]
		private bool m_hasFontAssetChanged;

		[SerializeField]
		protected TMP_SubMeshUI[] m_subTextObjects = new TMP_SubMeshUI[8];

		private float m_previousLossyScaleY = -1f;

		private Vector3[] m_RectTransformCorners = (Vector3[])(object)new Vector3[4];

		private CanvasRenderer m_canvasRenderer;

		private Canvas m_canvas;

		private bool m_isFirstAllocation;

		private int m_max_characters = 8;

		private bool m_isMaskingEnabled;

		[SerializeField]
		private Material m_baseMaterial;

		private bool m_isScrollRegionSet;

		private int m_stencilID;

		[SerializeField]
		private Vector4 m_maskOffset;

		private Matrix4x4 m_EnvMapMatrix;

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private int m_recursiveCountA;

		private int loopCountA;

		public override Material materialForRendering => TMP_MaterialManager.GetMaterialForRendering((MaskableGraphic)(object)this, m_sharedMaterial);

		public override bool autoSizeTextContainer
		{
			get
			{
				return m_autoSizeTextContainer;
			}
			set
			{
				if (m_autoSizeTextContainer != value)
				{
					m_autoSizeTextContainer = value;
					if (m_autoSizeTextContainer)
					{
						CanvasUpdateRegistry.RegisterCanvasElementForLayoutRebuild((ICanvasElement)(object)this);
						((Graphic)this).SetLayoutDirty();
					}
				}
			}
		}

		public override Mesh mesh => m_mesh;

		public CanvasRenderer canvasRenderer
		{
			get
			{
				if ((Object)(object)m_canvasRenderer == (Object)null)
				{
					m_canvasRenderer = ((Component)this).GetComponent<CanvasRenderer>();
				}
				return m_canvasRenderer;
			}
		}

		public Vector4 maskOffset
		{
			get
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				return m_maskOffset;
			}
			set
			{
				//IL_0001: Unknown result type (might be due to invalid IL or missing references)
				//IL_0002: Unknown result type (might be due to invalid IL or missing references)
				m_maskOffset = value;
				UpdateMask();
				m_havePropertiesChanged = true;
			}
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (((Component)this).get_gameObject().get_activeInHierarchy() && (m_isCalculateSizeRequired || ((Transform)m_rectTransform).get_hasChanged()))
			{
				m_preferredWidth = GetPreferredWidth();
				ComputeMarginSize();
				m_isLayoutDirty = true;
			}
		}

		public void CalculateLayoutInputVertical()
		{
			if (((Component)this).get_gameObject().get_activeInHierarchy())
			{
				if (m_isCalculateSizeRequired || ((Transform)m_rectTransform).get_hasChanged())
				{
					m_preferredHeight = GetPreferredHeight();
					ComputeMarginSize();
					m_isLayoutDirty = true;
				}
				m_isCalculateSizeRequired = false;
			}
		}

		public override void SetVerticesDirty()
		{
			if (!m_verticesAlreadyDirty && !((Object)(object)this == (Object)null) && ((UIBehaviour)this).IsActive() && !CanvasUpdateRegistry.IsRebuildingGraphics())
			{
				m_verticesAlreadyDirty = true;
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild((ICanvasElement)(object)this);
				if (((Graphic)this).m_OnDirtyVertsCallback != null)
				{
					((Graphic)this).m_OnDirtyVertsCallback.Invoke();
				}
			}
		}

		public override void SetLayoutDirty()
		{
			m_isPreferredWidthDirty = true;
			m_isPreferredHeightDirty = true;
			if (!m_layoutAlreadyDirty && !((Object)(object)this == (Object)null) && ((UIBehaviour)this).IsActive())
			{
				m_layoutAlreadyDirty = true;
				LayoutRebuilder.MarkLayoutForRebuild(base.rectTransform);
				m_isLayoutDirty = true;
				if (((Graphic)this).m_OnDirtyLayoutCallback != null)
				{
					((Graphic)this).m_OnDirtyLayoutCallback.Invoke();
				}
			}
		}

		public override void SetMaterialDirty()
		{
			if (!((Object)(object)this == (Object)null) && ((UIBehaviour)this).IsActive() && !CanvasUpdateRegistry.IsRebuildingGraphics())
			{
				m_isMaterialDirty = true;
				CanvasUpdateRegistry.RegisterCanvasElementForGraphicRebuild((ICanvasElement)(object)this);
				if (((Graphic)this).m_OnDirtyMaterialCallback != null)
				{
					((Graphic)this).m_OnDirtyMaterialCallback.Invoke();
				}
			}
		}

		public override void SetAllDirty()
		{
			((Graphic)this).SetLayoutDirty();
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetMaterialDirty();
		}

		public override void Rebuild(CanvasUpdate update)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Invalid comparison between Unknown and I4
			if ((Object)(object)this == (Object)null)
			{
				return;
			}
			if ((int)update == 0)
			{
				if (m_autoSizeTextContainer)
				{
					m_rectTransform.set_sizeDelta(GetPreferredValues(float.PositiveInfinity, float.PositiveInfinity));
				}
			}
			else if ((int)update == 3)
			{
				OnPreRenderCanvas();
				m_verticesAlreadyDirty = false;
				m_layoutAlreadyDirty = false;
				if (m_isMaterialDirty)
				{
					((Graphic)this).UpdateMaterial();
					m_isMaterialDirty = false;
				}
			}
		}

		private void UpdateSubObjectPivot()
		{
			if (m_textInfo != null)
			{
				for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
				{
					m_subTextObjects[i].SetPivotDirty();
				}
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material val = baseMaterial;
			if (((MaskableGraphic)this).m_ShouldRecalculateStencil)
			{
				m_stencilID = TMP_MaterialManager.GetStencilID(((Component)this).get_gameObject());
				((MaskableGraphic)this).m_ShouldRecalculateStencil = false;
			}
			if (m_stencilID > 0)
			{
				val = TMP_MaterialManager.GetStencilMaterial(baseMaterial, m_stencilID);
				if ((Object)(object)((MaskableGraphic)this).m_MaskMaterial != (Object)null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(((MaskableGraphic)this).m_MaskMaterial);
				}
				((MaskableGraphic)this).m_MaskMaterial = val;
			}
			return val;
		}

		protected override void UpdateMaterial()
		{
			if (!((Object)(object)m_sharedMaterial == (Object)null))
			{
				if ((Object)(object)m_canvasRenderer == (Object)null)
				{
					m_canvasRenderer = canvasRenderer;
				}
				m_canvasRenderer.set_materialCount(1);
				m_canvasRenderer.SetMaterial(((Graphic)this).get_materialForRendering(), 0);
			}
		}

		public override void RecalculateClipping()
		{
			((MaskableGraphic)this).RecalculateClipping();
		}

		public override void RecalculateMasking()
		{
			((MaskableGraphic)this).m_ShouldRecalculateStencil = true;
			((Graphic)this).SetMaterialDirty();
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			if (!m_ignoreRectMaskCulling)
			{
				((MaskableGraphic)this).Cull(clipRect, validRect);
			}
		}

		public override void UpdateMeshPadding()
		{
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_havePropertiesChanged = true;
			checkPaddingRequired = false;
			if (m_textInfo != null)
			{
				for (int i = 1; i < m_textInfo.materialCount; i++)
				{
					m_subTextObjects[i].UpdateMeshPadding(m_enableExtraPadding, m_isUsingBold);
				}
			}
		}

		protected override void InternalCrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			int materialCount = m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				((Graphic)m_subTextObjects[i]).CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
			}
		}

		protected override void InternalCrossFadeAlpha(float alpha, float duration, bool ignoreTimeScale)
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 1; i < materialCount; i++)
			{
				((Graphic)m_subTextObjects[i]).CrossFadeAlpha(alpha, duration, ignoreTimeScale);
			}
		}

		public override void ForceMeshUpdate()
		{
			m_havePropertiesChanged = true;
			OnPreRenderCanvas();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			m_havePropertiesChanged = true;
			m_ignoreActiveState = true;
			OnPreRenderCanvas();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			m_renderMode = TextRenderFlags.DontRender;
			ComputeMarginSize();
			if ((Object)(object)m_canvas == (Object)null)
			{
				m_canvas = ((Graphic)this).get_canvas();
			}
			GenerateTextMesh();
			m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh()
		{
			m_canvasRenderer.SetMesh((Mesh)null);
			for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
			{
				m_subTextObjects[i].canvasRenderer.SetMesh((Mesh)null);
			}
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
			if (index == 0)
			{
				m_canvasRenderer.SetMesh(mesh);
			}
			else
			{
				m_subTextObjects[index].canvasRenderer.SetMesh(mesh);
			}
		}

		public override void UpdateVertexData(TMP_VertexDataUpdateFlags flags)
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh val = ((i != 0) ? m_subTextObjects[i].mesh : m_mesh);
				if ((flags & TMP_VertexDataUpdateFlags.Vertices) == TMP_VertexDataUpdateFlags.Vertices)
				{
					val.set_vertices(m_textInfo.meshInfo[i].vertices);
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv0) == TMP_VertexDataUpdateFlags.Uv0)
				{
					val.set_uv(m_textInfo.meshInfo[i].uvs0);
				}
				if ((flags & TMP_VertexDataUpdateFlags.Uv2) == TMP_VertexDataUpdateFlags.Uv2)
				{
					val.set_uv2(m_textInfo.meshInfo[i].uvs2);
				}
				if ((flags & TMP_VertexDataUpdateFlags.Colors32) == TMP_VertexDataUpdateFlags.Colors32)
				{
					val.set_colors32(m_textInfo.meshInfo[i].colors32);
				}
				val.RecalculateBounds();
				if (i == 0)
				{
					m_canvasRenderer.SetMesh(val);
				}
				else
				{
					m_subTextObjects[i].canvasRenderer.SetMesh(val);
				}
			}
		}

		public override void UpdateVertexData()
		{
			int materialCount = m_textInfo.materialCount;
			for (int i = 0; i < materialCount; i++)
			{
				Mesh val;
				if (i == 0)
				{
					val = m_mesh;
				}
				else
				{
					m_textInfo.meshInfo[i].ClearUnusedVertices();
					val = m_subTextObjects[i].mesh;
				}
				val.set_vertices(m_textInfo.meshInfo[i].vertices);
				val.set_uv(m_textInfo.meshInfo[i].uvs0);
				val.set_uv2(m_textInfo.meshInfo[i].uvs2);
				val.set_colors32(m_textInfo.meshInfo[i].colors32);
				val.RecalculateBounds();
				if (i == 0)
				{
					m_canvasRenderer.SetMesh(val);
				}
				else
				{
					m_subTextObjects[i].canvasRenderer.SetMesh(val);
				}
			}
		}

		public void UpdateFontAsset()
		{
			LoadFontAsset();
		}

		protected override void Awake()
		{
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Expected O, but got Unknown
			m_canvas = ((Graphic)this).get_canvas();
			m_isOrthographic = true;
			m_rectTransform = ((Component)this).get_gameObject().GetComponent<RectTransform>();
			if ((Object)(object)m_rectTransform == (Object)null)
			{
				m_rectTransform = ((Component)this).get_gameObject().AddComponent<RectTransform>();
			}
			m_canvasRenderer = ((Component)this).GetComponent<CanvasRenderer>();
			if ((Object)(object)m_canvasRenderer == (Object)null)
			{
				m_canvasRenderer = ((Component)this).get_gameObject().AddComponent<CanvasRenderer>();
			}
			if ((Object)(object)m_mesh == (Object)null)
			{
				m_mesh = new Mesh();
				((Object)m_mesh).set_hideFlags((HideFlags)61);
			}
			LoadDefaultSettings();
			LoadFontAsset();
			TMP_StyleSheet.LoadDefaultStyleSheet();
			if (m_char_buffer == null)
			{
				m_char_buffer = new int[m_max_characters];
			}
			m_cached_TextElement = new TMP_Glyph();
			m_isFirstAllocation = true;
			if (m_textInfo == null)
			{
				m_textInfo = new TMP_TextInfo(this);
			}
			if ((Object)(object)m_fontAsset == (Object)null)
			{
				Debug.LogWarning((object)("Please assign a Font Asset to this " + ((Object)base.transform).get_name() + " gameobject."), (Object)(object)this);
				return;
			}
			TMP_SubMeshUI[] componentsInChildren = ((Component)this).GetComponentsInChildren<TMP_SubMeshUI>();
			if (componentsInChildren.Length != 0)
			{
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					m_subTextObjects[i + 1] = componentsInChildren[i];
				}
			}
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			m_isAwake = true;
		}

		protected override void OnEnable()
		{
			if (!m_isRegisteredForEvents)
			{
				TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Add(ON_MATERIAL_PROPERTY_CHANGED);
				TMPro_EventManager.FONT_PROPERTY_EVENT.Add(ON_FONT_PROPERTY_CHANGED);
				TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Add(ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED);
				TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Add(ON_DRAG_AND_DROP_MATERIAL);
				TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Add(ON_TEXT_STYLE_CHANGED);
				TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Add(ON_COLOR_GRADIENT_CHANGED);
				TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Add(ON_TMP_SETTINGS_CHANGED);
				m_isRegisteredForEvents = true;
			}
			m_canvas = GetCanvas();
			SetActiveSubMeshes(state: true);
			GraphicRegistry.RegisterGraphicForCanvas(m_canvas, (Graphic)(object)this);
			ComputeMarginSize();
			m_verticesAlreadyDirty = false;
			m_layoutAlreadyDirty = false;
			((MaskableGraphic)this).m_ShouldRecalculateStencil = true;
			m_isInputParsingRequired = true;
			((Graphic)this).SetAllDirty();
			((MaskableGraphic)this).RecalculateClipping();
		}

		protected override void OnDisable()
		{
			if ((Object)(object)((MaskableGraphic)this).m_MaskMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(((MaskableGraphic)this).m_MaskMaterial);
				((MaskableGraphic)this).m_MaskMaterial = null;
			}
			GraphicRegistry.UnregisterGraphicForCanvas(m_canvas, (Graphic)(object)this);
			CanvasUpdateRegistry.UnRegisterCanvasElementForRebuild((ICanvasElement)(object)this);
			if ((Object)(object)m_canvasRenderer != (Object)null)
			{
				m_canvasRenderer.Clear();
			}
			SetActiveSubMeshes(state: false);
			LayoutRebuilder.MarkLayoutForRebuild(m_rectTransform);
			((MaskableGraphic)this).RecalculateClipping();
		}

		protected override void OnDestroy()
		{
			GraphicRegistry.UnregisterGraphicForCanvas(m_canvas, (Graphic)(object)this);
			if ((Object)(object)m_mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_mesh);
			}
			if ((Object)(object)((MaskableGraphic)this).m_MaskMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(((MaskableGraphic)this).m_MaskMaterial);
				((MaskableGraphic)this).m_MaskMaterial = null;
			}
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Remove(ON_MATERIAL_PROPERTY_CHANGED);
			TMPro_EventManager.FONT_PROPERTY_EVENT.Remove(ON_FONT_PROPERTY_CHANGED);
			TMPro_EventManager.TEXTMESHPRO_UGUI_PROPERTY_EVENT.Remove(ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED);
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Remove(ON_DRAG_AND_DROP_MATERIAL);
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Remove(ON_TEXT_STYLE_CHANGED);
			TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Remove(ON_COLOR_GRADIENT_CHANGED);
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Remove(ON_TMP_SETTINGS_CHANGED);
			m_isRegisteredForEvents = false;
		}

		protected override void Reset()
		{
			LoadDefaultSettings();
			LoadFontAsset();
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
		}

		protected override void OnValidate()
		{
			if ((Object)(object)m_fontAsset == (Object)null || m_hasFontAssetChanged)
			{
				LoadFontAsset();
				m_isCalculateSizeRequired = true;
				m_hasFontAssetChanged = false;
			}
			if ((Object)(object)m_canvasRenderer == (Object)null || (Object)(object)m_canvasRenderer.GetMaterial() == (Object)null || (Object)(object)m_canvasRenderer.GetMaterial().GetTexture(ShaderUtilities.ID_MainTex) == (Object)null || (Object)(object)m_fontAsset == (Object)null || ((Object)m_fontAsset.atlas).GetInstanceID() != ((Object)m_canvasRenderer.GetMaterial().GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
			{
				LoadFontAsset();
				m_isCalculateSizeRequired = true;
				m_hasFontAssetChanged = false;
			}
			m_padding = GetPaddingForMaterial();
			m_isInputParsingRequired = true;
			m_inputSource = TextInputSources.Text;
			m_havePropertiesChanged = true;
			m_isCalculateSizeRequired = true;
			m_isPreferredWidthDirty = true;
			m_isPreferredHeightDirty = true;
			((Graphic)this).SetAllDirty();
		}

		private void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			int instanceID = ((Object)mat).GetInstanceID();
			int instanceID2 = ((Object)m_sharedMaterial).GetInstanceID();
			int num = ((!((Object)(object)((MaskableGraphic)this).m_MaskMaterial == (Object)null)) ? ((Object)((MaskableGraphic)this).m_MaskMaterial).GetInstanceID() : 0);
			if ((Object)(object)m_canvasRenderer == (Object)null || (Object)(object)m_canvasRenderer.GetMaterial() == (Object)null)
			{
				if ((Object)(object)m_canvasRenderer == (Object)null)
				{
					return;
				}
				if ((Object)(object)m_fontAsset != (Object)null)
				{
					m_canvasRenderer.SetMaterial(m_fontAsset.material, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
				}
				else
				{
					Debug.LogWarning((object)("No Font Asset assigned to " + ((Object)this).get_name() + ". Please assign a Font Asset."), (Object)(object)this);
				}
			}
			if ((Object)(object)m_canvasRenderer.GetMaterial() != (Object)(object)m_sharedMaterial && (Object)(object)m_fontAsset == (Object)null)
			{
				m_sharedMaterial = m_canvasRenderer.GetMaterial();
			}
			if ((Object)(object)((MaskableGraphic)this).m_MaskMaterial != (Object)null)
			{
				Undo.RecordObject((Object)(object)((MaskableGraphic)this).m_MaskMaterial, "Material Property Changes");
				Undo.RecordObject((Object)(object)m_sharedMaterial, "Material Property Changes");
				if (instanceID == instanceID2)
				{
					float @float = ((MaskableGraphic)this).m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilID);
					float float2 = ((MaskableGraphic)this).m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilComp);
					((MaskableGraphic)this).m_MaskMaterial.CopyPropertiesFromMaterial(mat);
					((MaskableGraphic)this).m_MaskMaterial.set_shaderKeywords(mat.get_shaderKeywords());
					((MaskableGraphic)this).m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilID, @float);
					((MaskableGraphic)this).m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilComp, float2);
				}
				else if (instanceID == num)
				{
					GetPaddingForMaterial(mat);
					m_sharedMaterial.CopyPropertiesFromMaterial(mat);
					m_sharedMaterial.set_shaderKeywords(mat.get_shaderKeywords());
					m_sharedMaterial.SetFloat(ShaderUtilities.ID_StencilID, 0f);
					m_sharedMaterial.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
				}
			}
			m_padding = GetPaddingForMaterial();
			m_havePropertiesChanged = true;
			((Graphic)this).SetVerticesDirty();
		}

		private void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
		{
			if (MaterialReference.Contains(m_materialReferences, font))
			{
				m_isInputParsingRequired = true;
				m_havePropertiesChanged = true;
				((Graphic)this).SetLayoutDirty();
				((Graphic)this).SetVerticesDirty();
			}
		}

		private void ON_TEXTMESHPRO_UGUI_PROPERTY_CHANGED(bool isChanged, TextMeshProUGUI obj)
		{
			Debug.Log((object)("Event Received by " + (object)obj));
			if ((Object)(object)obj == (Object)(object)this)
			{
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				((Graphic)this).SetVerticesDirty();
			}
		}

		private void ON_DRAG_AND_DROP_MATERIAL(GameObject obj, Material currentMaterial, Material newMaterial)
		{
			if ((Object)(object)obj == (Object)(object)((Component)this).get_gameObject() || PrefabUtility.GetPrefabParent((Object)(object)((Component)this).get_gameObject()) == (Object)(object)obj)
			{
				Undo.RecordObject((Object)(object)this, "Material Assignment");
				Undo.RecordObject((Object)(object)m_canvasRenderer, "Material Assignment");
				m_sharedMaterial = newMaterial;
				m_padding = GetPaddingForMaterial();
				m_havePropertiesChanged = true;
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetMaterialDirty();
			}
		}

		private void ON_TEXT_STYLE_CHANGED(bool isChanged)
		{
			m_havePropertiesChanged = true;
			m_isInputParsingRequired = true;
			((Graphic)this).SetVerticesDirty();
		}

		private void ON_COLOR_GRADIENT_CHANGED(TMP_ColorGradient gradient)
		{
			if ((Object)(object)m_fontColorGradientPreset != (Object)null && ((Object)gradient).GetInstanceID() == ((Object)m_fontColorGradientPreset).GetInstanceID())
			{
				m_havePropertiesChanged = true;
				((Graphic)this).SetVerticesDirty();
			}
		}

		private void ON_TMP_SETTINGS_CHANGED()
		{
			m_defaultSpriteAsset = null;
			m_havePropertiesChanged = true;
			m_isInputParsingRequired = true;
			((Graphic)this).SetAllDirty();
		}

		protected override void LoadFontAsset()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if ((Object)(object)m_fontAsset == (Object)null)
			{
				if ((Object)(object)TMP_Settings.defaultFontAsset != (Object)null)
				{
					m_fontAsset = TMP_Settings.defaultFontAsset;
				}
				else
				{
					m_fontAsset = Resources.Load("Fonts & Materials/LiberationSans SDF", typeof(TMP_FontAsset)) as TMP_FontAsset;
				}
				if ((Object)(object)m_fontAsset == (Object)null)
				{
					Debug.LogWarning((object)("The LiberationSans SDF Font Asset was not found. There is no Font Asset assigned to " + ((Object)((Component)this).get_gameObject()).get_name() + "."), (Object)(object)this);
					return;
				}
				if (m_fontAsset.characterDictionary == null)
				{
					Debug.Log((object)"Dictionary is Null!");
				}
				m_sharedMaterial = m_fontAsset.material;
			}
			else
			{
				if (m_fontAsset.characterDictionary == null)
				{
					m_fontAsset.ReadFontDefinition();
				}
				if ((Object)(object)m_sharedMaterial == (Object)null && (Object)(object)m_baseMaterial != (Object)null)
				{
					m_sharedMaterial = m_baseMaterial;
					m_baseMaterial = null;
				}
				if ((Object)(object)m_sharedMaterial == (Object)null || (Object)(object)m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex) == (Object)null || ((Object)m_fontAsset.atlas).GetInstanceID() != ((Object)m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
				{
					if ((Object)(object)m_fontAsset.material == (Object)null)
					{
						Debug.LogWarning((object)("The Font Atlas Texture of the Font Asset " + ((Object)m_fontAsset).get_name() + " assigned to " + ((Object)((Component)this).get_gameObject()).get_name() + " is missing."), (Object)(object)this);
					}
					else
					{
						m_sharedMaterial = m_fontAsset.material;
					}
				}
			}
			GetSpecialCharacters(m_fontAsset);
			m_padding = GetPaddingForMaterial();
			((Graphic)this).SetMaterialDirty();
		}

		private Canvas GetCanvas()
		{
			Canvas result = null;
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			((Component)this).get_gameObject().GetComponentsInParent<Canvas>(false, list);
			if (list.Count > 0)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (((Behaviour)list[i]).get_isActiveAndEnabled())
					{
						result = list[i];
						break;
					}
				}
			}
			TMP_ListPool<Canvas>.Release(list);
			return result;
		}

		private void UpdateEnvMapMatrix()
		{
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Unknown result type (might be due to invalid IL or missing references)
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_EnvMap) && !((Object)(object)m_sharedMaterial.GetTexture(ShaderUtilities.ID_EnvMap) == (Object)null))
			{
				Vector3 val = Vector4.op_Implicit(m_sharedMaterial.GetVector(ShaderUtilities.ID_EnvMatrixRotation));
				m_EnvMapMatrix = Matrix4x4.TRS(Vector3.get_zero(), Quaternion.Euler(val), Vector3.get_one());
				m_sharedMaterial.SetMatrix(ShaderUtilities.ID_EnvMatrix, m_EnvMapMatrix);
			}
		}

		private void EnableMasking()
		{
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
				m_canvasRenderer.SetMaterial(m_fontMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			m_sharedMaterial = m_fontMaterial;
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				UpdateMask();
			}
			m_isMaskingEnabled = true;
		}

		private void DisableMasking()
		{
			if ((Object)(object)m_fontMaterial != (Object)null)
			{
				if (m_stencilID > 0)
				{
					m_sharedMaterial = ((MaskableGraphic)this).m_MaskMaterial;
				}
				m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
				Object.DestroyImmediate((Object)(object)m_fontMaterial);
			}
			m_isMaskingEnabled = false;
		}

		private void UpdateMask()
		{
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0140: Unknown result type (might be due to invalid IL or missing references)
			//IL_0150: Unknown result type (might be due to invalid IL or missing references)
			//IL_0161: Unknown result type (might be due to invalid IL or missing references)
			//IL_0166: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_01be: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0200: Unknown result type (might be due to invalid IL or missing references)
			//IL_0205: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_020f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0233: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_rectTransform != (Object)null)
			{
				if (!ShaderUtilities.isInitialized)
				{
					ShaderUtilities.GetShaderPropertyIDs();
				}
				m_isScrollRegionSet = true;
				float num = Mathf.Min(Mathf.Min(m_margin.x, m_margin.z), m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessX));
				float num2 = Mathf.Min(Mathf.Min(m_margin.y, m_margin.w), m_sharedMaterial.GetFloat(ShaderUtilities.ID_MaskSoftnessY));
				num = ((num > 0f) ? num : 0f);
				num2 = ((num2 > 0f) ? num2 : 0f);
				Rect rect = m_rectTransform.get_rect();
				float num3 = (((Rect)(ref rect)).get_width() - Mathf.Max(m_margin.x, 0f) - Mathf.Max(m_margin.z, 0f)) / 2f + num;
				rect = m_rectTransform.get_rect();
				float num4 = (((Rect)(ref rect)).get_height() - Mathf.Max(m_margin.y, 0f) - Mathf.Max(m_margin.w, 0f)) / 2f + num2;
				Vector3 localPosition = ((Transform)m_rectTransform).get_localPosition();
				float num5 = 0.5f - m_rectTransform.get_pivot().x;
				rect = m_rectTransform.get_rect();
				float num6 = num5 * ((Rect)(ref rect)).get_width() + (Mathf.Max(m_margin.x, 0f) - Mathf.Max(m_margin.z, 0f)) / 2f;
				float num7 = 0.5f - m_rectTransform.get_pivot().y;
				rect = m_rectTransform.get_rect();
				Vector2 val = Vector2.op_Implicit(localPosition + new Vector3(num6, num7 * ((Rect)(ref rect)).get_height() + (0f - Mathf.Max(m_margin.y, 0f) + Mathf.Max(m_margin.w, 0f)) / 2f));
				Vector4 val2 = default(Vector4);
				((Vector4)(ref val2))._002Ector(val.x, val.y, num3, num4);
				m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, val2);
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			ShaderUtilities.GetShaderPropertyIDs();
			if ((Object)(object)m_fontMaterial == (Object)null || ((Object)m_fontMaterial).GetInstanceID() != ((Object)mat).GetInstanceID())
			{
				m_fontMaterial = CreateMaterialInstance(mat);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			((MaskableGraphic)this).m_ShouldRecalculateStencil = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetMaterialDirty();
			return m_sharedMaterial;
		}

		protected override Material[] GetMaterials(Material[] mats)
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontMaterials == null)
			{
				m_fontMaterials = (Material[])(object)new Material[materialCount];
			}
			else if (m_fontMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontMaterials, materialCount, isBlockAllocated: false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					m_fontMaterials[i] = base.fontMaterial;
				}
				else
				{
					m_fontMaterials[i] = ((Graphic)m_subTextObjects[i]).get_material();
				}
			}
			m_fontSharedMaterials = m_fontMaterials;
			return m_fontMaterials;
		}

		protected override void SetSharedMaterial(Material mat)
		{
			m_sharedMaterial = mat;
			m_padding = GetPaddingForMaterial();
			((Graphic)this).SetMaterialDirty();
		}

		protected override Material[] GetSharedMaterials()
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontSharedMaterials == null)
			{
				m_fontSharedMaterials = (Material[])(object)new Material[materialCount];
			}
			else if (m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, isBlockAllocated: false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					m_fontSharedMaterials[i] = m_sharedMaterial;
				}
				else
				{
					m_fontSharedMaterials[i] = m_subTextObjects[i].sharedMaterial;
				}
			}
			return m_fontSharedMaterials;
		}

		protected override void SetSharedMaterials(Material[] materials)
		{
			int materialCount = m_textInfo.materialCount;
			if (m_fontSharedMaterials == null)
			{
				m_fontSharedMaterials = (Material[])(object)new Material[materialCount];
			}
			else if (m_fontSharedMaterials.Length != materialCount)
			{
				TMP_TextInfo.Resize(ref m_fontSharedMaterials, materialCount, isBlockAllocated: false);
			}
			for (int i = 0; i < materialCount; i++)
			{
				if (i == 0)
				{
					if (!((Object)(object)materials[i].GetTexture(ShaderUtilities.ID_MainTex) == (Object)null) && ((Object)materials[i].GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() == ((Object)m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
					{
						m_sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
						m_padding = GetPaddingForMaterial(m_sharedMaterial);
					}
				}
				else if (!((Object)(object)materials[i].GetTexture(ShaderUtilities.ID_MainTex) == (Object)null) && ((Object)materials[i].GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() == ((Object)m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() && m_subTextObjects[i].isDefaultMaterial)
				{
					m_subTextObjects[i].sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			if ((Object)(object)m_fontMaterial != (Object)null && ((Object)m_sharedMaterial).GetInstanceID() != ((Object)m_fontMaterial).GetInstanceID())
			{
				m_sharedMaterial = m_fontMaterial;
				m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			else if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
				m_sharedMaterial = m_fontMaterial;
				m_canvasRenderer.SetMaterial(m_sharedMaterial, m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex));
			}
			thickness = Mathf.Clamp01(thickness);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			m_padding = GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			m_sharedMaterial.SetColor(ShaderUtilities.ID_FaceColor, Color32.op_Implicit(color));
		}

		protected override void SetOutlineColor(Color32 color)
		{
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = CreateMaterialInstance(m_sharedMaterial);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
			m_sharedMaterial.SetColor(ShaderUtilities.ID_OutlineColor, Color32.op_Implicit(color));
		}

		protected override void SetShaderDepth()
		{
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)m_canvas == (Object)null) && !((Object)(object)m_sharedMaterial == (Object)null))
			{
				if ((int)m_canvas.get_renderMode() == 0 || m_isOverlay)
				{
					m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
				}
				else
				{
					m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				}
			}
		}

		protected override void SetCulling()
		{
			if (m_isCullingEnabled)
			{
				Material val = ((Graphic)this).get_materialForRendering();
				if ((Object)(object)val != (Object)null)
				{
					val.SetFloat("_CullMode", 2f);
				}
				for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
				{
					val = ((Graphic)m_subTextObjects[i]).get_materialForRendering();
					if ((Object)(object)val != (Object)null)
					{
						val.SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
					}
				}
				return;
			}
			Material val2 = ((Graphic)this).get_materialForRendering();
			if ((Object)(object)val2 != (Object)null)
			{
				val2.SetFloat("_CullMode", 0f);
			}
			for (int j = 1; j < m_subTextObjects.Length && (Object)(object)m_subTextObjects[j] != (Object)null; j++)
			{
				val2 = ((Graphic)m_subTextObjects[j]).get_materialForRendering();
				if ((Object)(object)val2 != (Object)null)
				{
					val2.SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
				}
			}
		}

		private void SetPerspectiveCorrection()
		{
			if (m_isOrthographic)
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0f);
			}
			else
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ID_PerspectiveFilter, 0.875f);
			}
		}

		protected override float GetPaddingForMaterial(Material mat)
		{
			m_padding = ShaderUtilities.GetPadding(mat, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = mat.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
		}

		protected override float GetPaddingForMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
		}

		private void SetMeshArrays(int size)
		{
			m_textInfo.meshInfo[0].ResizeMeshInfo(size);
			m_canvasRenderer.SetMesh(m_textInfo.meshInfo[0].mesh);
		}

		protected override int SetArraySizes(int[] chars)
		{
			//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cd: Expected O, but got Unknown
			//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aec: Unknown result type (might be due to invalid IL or missing references)
			int endIndex = 0;
			int num = 0;
			m_totalCharacterCount = 0;
			m_isUsingBold = false;
			m_isParsingText = false;
			tag_NoParsing = false;
			m_style = m_fontStyle;
			m_fontWeightInternal = (((m_style & FontStyles.Bold) == FontStyles.Bold) ? 700 : m_fontWeight);
			m_fontWeightStack.SetDefault(m_fontWeightInternal);
			m_currentFontAsset = m_fontAsset;
			m_currentMaterial = m_sharedMaterial;
			m_currentMaterialIndex = 0;
			m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
			m_materialReferenceIndexLookup.Clear();
			MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
			if (m_textInfo == null)
			{
				m_textInfo = new TMP_TextInfo();
			}
			m_textElementType = TMP_TextElementType.Character;
			if ((Object)(object)m_linkedTextComponent != (Object)null)
			{
				m_linkedTextComponent.text = string.Empty;
				m_linkedTextComponent.ForceMeshUpdate();
			}
			for (int i = 0; i < chars.Length && chars[i] != 0; i++)
			{
				if (m_textInfo.characterInfo == null || m_totalCharacterCount >= m_textInfo.characterInfo.Length)
				{
					TMP_TextInfo.Resize(ref m_textInfo.characterInfo, m_totalCharacterCount + 1, isBlockAllocated: true);
				}
				int num2 = chars[i];
				if (m_isRichText && num2 == 60)
				{
					int currentMaterialIndex = m_currentMaterialIndex;
					if (ValidateHtmlTag(chars, i + 1, out endIndex))
					{
						i = endIndex;
						if ((m_style & FontStyles.Bold) == FontStyles.Bold)
						{
							m_isUsingBold = true;
						}
						if (m_textElementType == TMP_TextElementType.Sprite)
						{
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
							m_textInfo.characterInfo[m_totalCharacterCount].character = (char)(57344 + m_spriteIndex);
							m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = m_spriteIndex;
							m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = m_currentSpriteAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
							m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
							m_textElementType = TMP_TextElementType.Character;
							m_currentMaterialIndex = currentMaterialIndex;
							num++;
							m_totalCharacterCount++;
						}
						continue;
					}
				}
				bool flag = false;
				bool isUsingAlternateTypeface = false;
				TMP_FontAsset currentFontAsset = m_currentFontAsset;
				Material currentMaterial = m_currentMaterial;
				int currentMaterialIndex2 = m_currentMaterialIndex;
				if (m_textElementType == TMP_TextElementType.Character)
				{
					if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num2))
						{
							num2 = char.ToUpper((char)num2);
						}
					}
					else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num2))
						{
							num2 = char.ToLower((char)num2);
						}
					}
					else if (((m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num2))
					{
						num2 = char.ToUpper((char)num2);
					}
				}
				TMP_FontAsset fontAssetForWeight = GetFontAssetForWeight(m_fontWeightInternal);
				if ((Object)(object)fontAssetForWeight != (Object)null)
				{
					flag = true;
					isUsingAlternateTypeface = true;
					m_currentFontAsset = fontAssetForWeight;
				}
				fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out var glyph);
				if (glyph == null)
				{
					TMP_SpriteAsset tMP_SpriteAsset = base.spriteAsset;
					if ((Object)(object)tMP_SpriteAsset != (Object)null)
					{
						int spriteIndex = -1;
						tMP_SpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(tMP_SpriteAsset, num2, out spriteIndex);
						if (spriteIndex != -1)
						{
							m_textElementType = TMP_TextElementType.Sprite;
							m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
							m_currentMaterialIndex = MaterialReference.AddMaterialReference(tMP_SpriteAsset.material, tMP_SpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
							m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = spriteIndex;
							m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = tMP_SpriteAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
							m_textElementType = TMP_TextElementType.Character;
							m_currentMaterialIndex = currentMaterialIndex2;
							num++;
							m_totalCharacterCount++;
							continue;
						}
					}
				}
				if (glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
				{
					fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num2, out glyph);
				}
				if (glyph == null && (Object)(object)TMP_Settings.defaultFontAsset != (Object)null)
				{
					fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num2, out glyph);
				}
				if (glyph == null)
				{
					TMP_SpriteAsset defaultSpriteAsset = TMP_Settings.defaultSpriteAsset;
					if ((Object)(object)defaultSpriteAsset != (Object)null)
					{
						int spriteIndex2 = -1;
						defaultSpriteAsset = TMP_SpriteAsset.SearchFallbackForSprite(defaultSpriteAsset, num2, out spriteIndex2);
						if (spriteIndex2 != -1)
						{
							m_textElementType = TMP_TextElementType.Sprite;
							m_textInfo.characterInfo[m_totalCharacterCount].elementType = m_textElementType;
							m_currentMaterialIndex = MaterialReference.AddMaterialReference(defaultSpriteAsset.material, defaultSpriteAsset, m_materialReferences, m_materialReferenceIndexLookup);
							m_materialReferences[m_currentMaterialIndex].referenceCount++;
							m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteIndex = spriteIndex2;
							m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].spriteAsset = defaultSpriteAsset;
							m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
							m_textElementType = TMP_TextElementType.Character;
							m_currentMaterialIndex = currentMaterialIndex2;
							num++;
							m_totalCharacterCount++;
							continue;
						}
					}
				}
				if (glyph == null)
				{
					int num3 = num2;
					num2 = (chars[i] = ((TMP_Settings.missingGlyphCharacter == 0) ? 9633 : TMP_Settings.missingGlyphCharacter));
					fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
					if (glyph == null && TMP_Settings.fallbackFontAssets != null && TMP_Settings.fallbackFontAssets.Count > 0)
					{
						fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.fallbackFontAssets, num2, out glyph);
					}
					if (glyph == null && (Object)(object)TMP_Settings.defaultFontAsset != (Object)null)
					{
						fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(TMP_Settings.defaultFontAsset, num2, out glyph);
					}
					if (glyph == null)
					{
						num2 = (chars[i] = 32);
						fontAssetForWeight = TMP_FontUtilities.SearchForGlyph(m_currentFontAsset, num2, out glyph);
						if (!TMP_Settings.warningsDisabled)
						{
							Debug.LogWarning((object)("Character with ASCII value of " + num3 + " was not found in the Font Asset Glyph Table. It was replaced by a space."), (Object)(object)this);
						}
					}
				}
				if ((Object)(object)fontAssetForWeight != (Object)null && ((Object)fontAssetForWeight).GetInstanceID() != ((Object)m_currentFontAsset).GetInstanceID())
				{
					flag = true;
					isUsingAlternateTypeface = false;
					m_currentFontAsset = fontAssetForWeight;
				}
				m_textInfo.characterInfo[m_totalCharacterCount].elementType = TMP_TextElementType.Character;
				m_textInfo.characterInfo[m_totalCharacterCount].textElement = glyph;
				m_textInfo.characterInfo[m_totalCharacterCount].isUsingAlternateTypeface = isUsingAlternateTypeface;
				m_textInfo.characterInfo[m_totalCharacterCount].character = (char)num2;
				m_textInfo.characterInfo[m_totalCharacterCount].fontAsset = m_currentFontAsset;
				if (flag)
				{
					if (TMP_Settings.matchMaterialPreset)
					{
						m_currentMaterial = TMP_MaterialManager.GetFallbackMaterial(m_currentMaterial, m_currentFontAsset.material);
					}
					else
					{
						m_currentMaterial = m_currentFontAsset.material;
					}
					m_currentMaterialIndex = MaterialReference.AddMaterialReference(m_currentMaterial, m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
				}
				if (!char.IsWhiteSpace((char)num2) && num2 != 8203)
				{
					if (m_materialReferences[m_currentMaterialIndex].referenceCount < 16383)
					{
						m_materialReferences[m_currentMaterialIndex].referenceCount++;
					}
					else
					{
						m_currentMaterialIndex = MaterialReference.AddMaterialReference(new Material(m_currentMaterial), m_currentFontAsset, m_materialReferences, m_materialReferenceIndexLookup);
						m_materialReferences[m_currentMaterialIndex].referenceCount++;
					}
				}
				m_textInfo.characterInfo[m_totalCharacterCount].material = m_currentMaterial;
				m_textInfo.characterInfo[m_totalCharacterCount].materialReferenceIndex = m_currentMaterialIndex;
				m_materialReferences[m_currentMaterialIndex].isFallbackMaterial = flag;
				if (flag)
				{
					m_materialReferences[m_currentMaterialIndex].fallbackMaterial = currentMaterial;
					m_currentFontAsset = currentFontAsset;
					m_currentMaterial = currentMaterial;
					m_currentMaterialIndex = currentMaterialIndex2;
				}
				m_totalCharacterCount++;
			}
			if (m_isCalculatingPreferredValues)
			{
				m_isCalculatingPreferredValues = false;
				m_isInputParsingRequired = true;
				return m_totalCharacterCount;
			}
			m_textInfo.spriteCount = num;
			int num4 = (m_textInfo.materialCount = m_materialReferenceIndexLookup.Count);
			if (num4 > m_textInfo.meshInfo.Length)
			{
				TMP_TextInfo.Resize(ref m_textInfo.meshInfo, num4, isBlockAllocated: false);
			}
			if (num4 > m_subTextObjects.Length)
			{
				TMP_TextInfo.Resize(ref m_subTextObjects, Mathf.NextPowerOfTwo(num4 + 1));
			}
			if (m_textInfo.characterInfo.Length - m_totalCharacterCount > 256)
			{
				TMP_TextInfo.Resize(ref m_textInfo.characterInfo, Mathf.Max(m_totalCharacterCount + 1, 256), isBlockAllocated: true);
			}
			for (int j = 0; j < num4; j++)
			{
				if (j > 0)
				{
					if ((Object)(object)m_subTextObjects[j] == (Object)null)
					{
						m_subTextObjects[j] = TMP_SubMeshUI.AddSubTextObject(this, m_materialReferences[j]);
						m_textInfo.meshInfo[j].vertices = null;
					}
					if (m_rectTransform.get_pivot() != ((Graphic)m_subTextObjects[j]).get_rectTransform().get_pivot())
					{
						((Graphic)m_subTextObjects[j]).get_rectTransform().set_pivot(m_rectTransform.get_pivot());
					}
					if ((Object)(object)m_subTextObjects[j].sharedMaterial == (Object)null || ((Object)m_subTextObjects[j].sharedMaterial).GetInstanceID() != ((Object)m_materialReferences[j].material).GetInstanceID())
					{
						bool isDefaultMaterial = m_materialReferences[j].isDefaultMaterial;
						m_subTextObjects[j].isDefaultMaterial = isDefaultMaterial;
						if (!isDefaultMaterial || (Object)(object)m_subTextObjects[j].sharedMaterial == (Object)null || ((Object)m_subTextObjects[j].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() != ((Object)m_materialReferences[j].material.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
						{
							m_subTextObjects[j].sharedMaterial = m_materialReferences[j].material;
							m_subTextObjects[j].fontAsset = m_materialReferences[j].fontAsset;
							m_subTextObjects[j].spriteAsset = m_materialReferences[j].spriteAsset;
						}
					}
					if (m_materialReferences[j].isFallbackMaterial)
					{
						m_subTextObjects[j].fallbackMaterial = m_materialReferences[j].material;
						m_subTextObjects[j].fallbackSourceMaterial = m_materialReferences[j].fallbackMaterial;
					}
				}
				int referenceCount = m_materialReferences[j].referenceCount;
				if (m_textInfo.meshInfo[j].vertices == null || m_textInfo.meshInfo[j].vertices.Length < referenceCount * 4)
				{
					if (m_textInfo.meshInfo[j].vertices == null)
					{
						if (j == 0)
						{
							m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_mesh, referenceCount + 1);
						}
						else
						{
							m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_subTextObjects[j].mesh, referenceCount + 1);
						}
					}
					else
					{
						m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.NextPowerOfTwo(referenceCount));
					}
				}
				else if (m_textInfo.meshInfo[j].vertices.Length - referenceCount * 4 > 1024)
				{
					m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 256));
				}
			}
			for (int k = num4; k < m_subTextObjects.Length && (Object)(object)m_subTextObjects[k] != (Object)null; k++)
			{
				if (k < m_textInfo.meshInfo.Length)
				{
					m_subTextObjects[k].canvasRenderer.SetMesh((Mesh)null);
				}
			}
			return m_totalCharacterCount;
		}

		protected override void ComputeMarginSize()
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)base.rectTransform != (Object)null)
			{
				Rect rect = m_rectTransform.get_rect();
				m_marginWidth = ((Rect)(ref rect)).get_width() - m_margin.x - m_margin.z;
				rect = m_rectTransform.get_rect();
				m_marginHeight = ((Rect)(ref rect)).get_height() - m_margin.y - m_margin.w;
				m_RectTransformCorners = GetTextContainerLocalCorners();
			}
		}

		protected override void OnDidApplyAnimationProperties()
		{
			m_havePropertiesChanged = true;
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		protected override void OnCanvasHierarchyChanged()
		{
			((MaskableGraphic)this).OnCanvasHierarchyChanged();
			m_canvas = ((Graphic)this).get_canvas();
		}

		protected override void OnTransformParentChanged()
		{
			((MaskableGraphic)this).OnTransformParentChanged();
			m_canvas = ((Graphic)this).get_canvas();
			ComputeMarginSize();
			m_havePropertiesChanged = true;
		}

		protected override void OnRectTransformDimensionsChange()
		{
			if (((Component)this).get_gameObject().get_activeInHierarchy())
			{
				ComputeMarginSize();
				UpdateSubObjectPivot();
				((Graphic)this).SetVerticesDirty();
				((Graphic)this).SetLayoutDirty();
			}
		}

		private void LateUpdate()
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			if (((Transform)m_rectTransform).get_hasChanged())
			{
				float y = ((Transform)m_rectTransform).get_lossyScale().y;
				if (!m_havePropertiesChanged && y != m_previousLossyScaleY && m_text != string.Empty && m_text != null)
				{
					UpdateSDFScale(y);
					m_previousLossyScaleY = y;
				}
				((Transform)m_rectTransform).set_hasChanged(false);
			}
			if (m_isUsingLegacyAnimationComponent)
			{
				m_havePropertiesChanged = true;
				OnPreRenderCanvas();
			}
		}

		private void OnPreRenderCanvas()
		{
			if (!m_isAwake || (!m_ignoreActiveState && !((UIBehaviour)this).IsActive()))
			{
				return;
			}
			if ((Object)(object)m_canvas == (Object)null)
			{
				m_canvas = ((Graphic)this).get_canvas();
				if ((Object)(object)m_canvas == (Object)null)
				{
					return;
				}
			}
			loopCountA = 0;
			if (m_havePropertiesChanged || m_isLayoutDirty)
			{
				if (checkPaddingRequired)
				{
					UpdateMeshPadding();
				}
				if (m_isInputParsingRequired || m_isTextTruncated)
				{
					ParseInputText();
				}
				if (m_enableAutoSizing)
				{
					m_fontSize = Mathf.Clamp(m_fontSize, m_fontSizeMin, m_fontSizeMax);
				}
				m_maxFontSize = m_fontSizeMax;
				m_minFontSize = m_fontSizeMin;
				m_lineSpacingDelta = 0f;
				m_charWidthAdjDelta = 0f;
				m_isCharacterWrappingEnabled = false;
				m_isTextTruncated = false;
				m_havePropertiesChanged = false;
				m_isLayoutDirty = false;
				m_ignoreActiveState = false;
				GenerateTextMesh();
			}
		}

		protected override void GenerateTextMesh()
		{
			//IL_020e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0213: Unknown result type (might be due to invalid IL or missing references)
			//IL_0215: Unknown result type (might be due to invalid IL or missing references)
			//IL_021a: Unknown result type (might be due to invalid IL or missing references)
			//IL_021f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0224: Unknown result type (might be due to invalid IL or missing references)
			//IL_0226: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0230: Unknown result type (might be due to invalid IL or missing references)
			//IL_0235: Unknown result type (might be due to invalid IL or missing references)
			//IL_0237: Unknown result type (might be due to invalid IL or missing references)
			//IL_023c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0269: Unknown result type (might be due to invalid IL or missing references)
			//IL_026e: Unknown result type (might be due to invalid IL or missing references)
			//IL_027a: Unknown result type (might be due to invalid IL or missing references)
			//IL_028b: Unknown result type (might be due to invalid IL or missing references)
			//IL_029c: Unknown result type (might be due to invalid IL or missing references)
			//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_044f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0454: Unknown result type (might be due to invalid IL or missing references)
			//IL_045f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0464: Unknown result type (might be due to invalid IL or missing references)
			//IL_0732: Unknown result type (might be due to invalid IL or missing references)
			//IL_0737: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
			//IL_0a98: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ab9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ad5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0d80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0db1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dec: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e14: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ec9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ece: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ed9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0edf: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ee8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0eef: Unknown result type (might be due to invalid IL or missing references)
			//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0efe: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f00: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f17: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f19: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f20: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f25: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f36: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f38: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f42: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f49: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f51: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f53: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f55: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f5f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f61: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f66: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f6e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f70: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f72: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f77: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f83: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff2: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ff4: Unknown result type (might be due to invalid IL or missing references)
			//IL_105e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1065: Unknown result type (might be due to invalid IL or missing references)
			//IL_106d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1074: Unknown result type (might be due to invalid IL or missing references)
			//IL_17b7: Unknown result type (might be due to invalid IL or missing references)
			//IL_17bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1803: Unknown result type (might be due to invalid IL or missing references)
			//IL_1cfe: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d03: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d08: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_278c: Unknown result type (might be due to invalid IL or missing references)
			//IL_2791: Unknown result type (might be due to invalid IL or missing references)
			//IL_27d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_27d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_30ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_30b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3303: Unknown result type (might be due to invalid IL or missing references)
			//IL_330d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3321: Unknown result type (might be due to invalid IL or missing references)
			//IL_332e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3333: Unknown result type (might be due to invalid IL or missing references)
			//IL_3338: Unknown result type (might be due to invalid IL or missing references)
			//IL_3342: Unknown result type (might be due to invalid IL or missing references)
			//IL_334c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3371: Unknown result type (might be due to invalid IL or missing references)
			//IL_337e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3383: Unknown result type (might be due to invalid IL or missing references)
			//IL_3388: Unknown result type (might be due to invalid IL or missing references)
			//IL_339b: Unknown result type (might be due to invalid IL or missing references)
			//IL_33a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_33a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_33b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_33bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_33cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_33da: Unknown result type (might be due to invalid IL or missing references)
			//IL_33ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_33f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_33f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_3402: Unknown result type (might be due to invalid IL or missing references)
			//IL_340a: Unknown result type (might be due to invalid IL or missing references)
			//IL_340f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3419: Unknown result type (might be due to invalid IL or missing references)
			//IL_3423: Unknown result type (might be due to invalid IL or missing references)
			//IL_3447: Unknown result type (might be due to invalid IL or missing references)
			//IL_3467: Unknown result type (might be due to invalid IL or missing references)
			//IL_347b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3480: Unknown result type (might be due to invalid IL or missing references)
			//IL_3485: Unknown result type (might be due to invalid IL or missing references)
			//IL_3498: Unknown result type (might be due to invalid IL or missing references)
			//IL_34a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_34b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_34bf: Unknown result type (might be due to invalid IL or missing references)
			//IL_34c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_34c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_34d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_34dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3502: Unknown result type (might be due to invalid IL or missing references)
			//IL_350f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3514: Unknown result type (might be due to invalid IL or missing references)
			//IL_3519: Unknown result type (might be due to invalid IL or missing references)
			//IL_3523: Unknown result type (might be due to invalid IL or missing references)
			//IL_352b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3530: Unknown result type (might be due to invalid IL or missing references)
			//IL_353a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3544: Unknown result type (might be due to invalid IL or missing references)
			//IL_3556: Unknown result type (might be due to invalid IL or missing references)
			//IL_355b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3560: Unknown result type (might be due to invalid IL or missing references)
			//IL_356a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3572: Unknown result type (might be due to invalid IL or missing references)
			//IL_3577: Unknown result type (might be due to invalid IL or missing references)
			//IL_3581: Unknown result type (might be due to invalid IL or missing references)
			//IL_358b: Unknown result type (might be due to invalid IL or missing references)
			//IL_35a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_35c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_35d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_35da: Unknown result type (might be due to invalid IL or missing references)
			//IL_35df: Unknown result type (might be due to invalid IL or missing references)
			//IL_35e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_35ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_35f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_35fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3607: Unknown result type (might be due to invalid IL or missing references)
			//IL_361a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3622: Unknown result type (might be due to invalid IL or missing references)
			//IL_3636: Unknown result type (might be due to invalid IL or missing references)
			//IL_363b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3640: Unknown result type (might be due to invalid IL or missing references)
			//IL_3642: Unknown result type (might be due to invalid IL or missing references)
			//IL_3647: Unknown result type (might be due to invalid IL or missing references)
			//IL_3649: Unknown result type (might be due to invalid IL or missing references)
			//IL_364e: Unknown result type (might be due to invalid IL or missing references)
			//IL_368b: Unknown result type (might be due to invalid IL or missing references)
			//IL_36a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_36ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_36bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_36c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_36c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_36c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_36cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_36d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a30: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c49: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c53: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c5a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c75: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ca3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ca8: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cad: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ccb: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cd0: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d16: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d18: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d1f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e4e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3e67: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ea8: Unknown result type (might be due to invalid IL or missing references)
			//IL_3eb5: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ec1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f02: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f1b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f5c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f69: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f75: Unknown result type (might be due to invalid IL or missing references)
			//IL_3fb9: Unknown result type (might be due to invalid IL or missing references)
			//IL_4027: Unknown result type (might be due to invalid IL or missing references)
			//IL_4095: Unknown result type (might be due to invalid IL or missing references)
			//IL_4103: Unknown result type (might be due to invalid IL or missing references)
			//IL_4176: Unknown result type (might be due to invalid IL or missing references)
			//IL_41e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_4252: Unknown result type (might be due to invalid IL or missing references)
			//IL_42c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_43d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_43df: Unknown result type (might be due to invalid IL or missing references)
			//IL_43eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_442c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4439: Unknown result type (might be due to invalid IL or missing references)
			//IL_4445: Unknown result type (might be due to invalid IL or missing references)
			//IL_4b67: Unknown result type (might be due to invalid IL or missing references)
			//IL_4b7a: Expected I4, but got Unknown
			//IL_4d6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d71: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d76: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d8f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d94: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d96: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_4db4: Unknown result type (might be due to invalid IL or missing references)
			//IL_4db9: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dc0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dde: Unknown result type (might be due to invalid IL or missing references)
			//IL_4de0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4de5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e4b: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e50: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e52: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e57: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e70: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e75: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e77: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e95: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e9a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ea1: Unknown result type (might be due to invalid IL or missing references)
			//IL_4eba: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ebf: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ec1: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ec6: Unknown result type (might be due to invalid IL or missing references)
			//IL_4edb: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ee0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ef3: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ef8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f10: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f23: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f28: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f73: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f78: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f9c: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fa1: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fa3: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fca: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fcc: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fee: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ff3: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ff5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ffa: Unknown result type (might be due to invalid IL or missing references)
			//IL_5018: Unknown result type (might be due to invalid IL or missing references)
			//IL_503a: Unknown result type (might be due to invalid IL or missing references)
			//IL_505c: Unknown result type (might be due to invalid IL or missing references)
			//IL_507e: Unknown result type (might be due to invalid IL or missing references)
			//IL_50a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_50e3: Unknown result type (might be due to invalid IL or missing references)
			//IL_5105: Unknown result type (might be due to invalid IL or missing references)
			//IL_5127: Unknown result type (might be due to invalid IL or missing references)
			//IL_518f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5194: Unknown result type (might be due to invalid IL or missing references)
			//IL_51f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_51fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_522a: Unknown result type (might be due to invalid IL or missing references)
			//IL_524c: Unknown result type (might be due to invalid IL or missing references)
			//IL_526e: Unknown result type (might be due to invalid IL or missing references)
			//IL_52d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_52db: Unknown result type (might be due to invalid IL or missing references)
			//IL_533f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5344: Unknown result type (might be due to invalid IL or missing references)
			//IL_5793: Unknown result type (might be due to invalid IL or missing references)
			//IL_5798: Unknown result type (might be due to invalid IL or missing references)
			//IL_57ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_57f0: Unknown result type (might be due to invalid IL or missing references)
			//IL_57fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_58dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_58df: Unknown result type (might be due to invalid IL or missing references)
			//IL_58eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_5958: Unknown result type (might be due to invalid IL or missing references)
			//IL_595a: Unknown result type (might be due to invalid IL or missing references)
			//IL_5966: Unknown result type (might be due to invalid IL or missing references)
			//IL_5996: Unknown result type (might be due to invalid IL or missing references)
			//IL_59ac: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a02: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a04: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a10: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a76: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a78: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a84: Unknown result type (might be due to invalid IL or missing references)
			//IL_5bde: Unknown result type (might be due to invalid IL or missing references)
			//IL_5be3: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c54: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c56: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c62: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d30: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d3c: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d92: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e45: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e47: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e53: Unknown result type (might be due to invalid IL or missing references)
			//IL_5eda: Unknown result type (might be due to invalid IL or missing references)
			//IL_5edc: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ee8: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f4f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f51: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f5d: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fb7: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fb9: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_6073: Unknown result type (might be due to invalid IL or missing references)
			//IL_6078: Unknown result type (might be due to invalid IL or missing references)
			//IL_607d: Unknown result type (might be due to invalid IL or missing references)
			//IL_607f: Unknown result type (might be due to invalid IL or missing references)
			//IL_6084: Unknown result type (might be due to invalid IL or missing references)
			//IL_6089: Unknown result type (might be due to invalid IL or missing references)
			//IL_609d: Unknown result type (might be due to invalid IL or missing references)
			//IL_60a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_60bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_60c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_60c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_60c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_60d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_6108: Unknown result type (might be due to invalid IL or missing references)
			//IL_6132: Unknown result type (might be due to invalid IL or missing references)
			//IL_615b: Unknown result type (might be due to invalid IL or missing references)
			//IL_615d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6161: Unknown result type (might be due to invalid IL or missing references)
			//IL_616b: Unknown result type (might be due to invalid IL or missing references)
			//IL_616d: Unknown result type (might be due to invalid IL or missing references)
			//IL_61c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_61c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_61d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_6202: Unknown result type (might be due to invalid IL or missing references)
			//IL_622c: Unknown result type (might be due to invalid IL or missing references)
			//IL_625b: Unknown result type (might be due to invalid IL or missing references)
			//IL_6294: Unknown result type (might be due to invalid IL or missing references)
			//IL_6296: Unknown result type (might be due to invalid IL or missing references)
			//IL_629a: Unknown result type (might be due to invalid IL or missing references)
			//IL_62c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_62c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_62c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_62dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_62de: Unknown result type (might be due to invalid IL or missing references)
			//IL_62e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_62f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_62f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_62f9: Unknown result type (might be due to invalid IL or missing references)
			//IL_638c: Unknown result type (might be due to invalid IL or missing references)
			//IL_6393: Invalid comparison between Unknown and I4
			//IL_639c: Unknown result type (might be due to invalid IL or missing references)
			//IL_63a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_6479: Unknown result type (might be due to invalid IL or missing references)
			//IL_647e: Unknown result type (might be due to invalid IL or missing references)
			//IL_65bd: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)m_fontAsset == (Object)null || m_fontAsset.characterDictionary == null)
			{
				Debug.LogWarning((object)("Can't Generate Mesh! No Font Asset has been assigned to Object ID: " + ((Object)this).GetInstanceID()));
				return;
			}
			if (m_textInfo != null)
			{
				m_textInfo.Clear();
			}
			if (m_char_buffer == null || m_char_buffer.Length == 0 || m_char_buffer[0] == 0)
			{
				ClearMesh();
				m_preferredWidth = 0f;
				m_preferredHeight = 0f;
				TMPro_EventManager.ON_TEXT_CHANGED((Object)(object)this);
				return;
			}
			m_currentFontAsset = m_fontAsset;
			m_currentMaterial = m_sharedMaterial;
			m_currentMaterialIndex = 0;
			m_materialReferenceStack.SetDefault(new MaterialReference(m_currentMaterialIndex, m_currentFontAsset, null, m_currentMaterial, m_padding));
			m_currentSpriteAsset = m_spriteAsset;
			if ((Object)(object)m_spriteAnimator != (Object)null)
			{
				m_spriteAnimator.StopAllAnimations();
			}
			int totalCharacterCount = m_totalCharacterCount;
			m_fontScale = m_fontSize / m_currentFontAsset.fontInfo.PointSize;
			float num = m_fontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
			float num2 = m_fontScale;
			m_fontScaleMultiplier = 1f;
			m_currentFontSize = m_fontSize;
			m_sizeStack.SetDefault(m_currentFontSize);
			float num3 = 0f;
			int num4 = 0;
			m_style = m_fontStyle;
			m_fontWeightInternal = (((m_style & FontStyles.Bold) == FontStyles.Bold) ? 700 : m_fontWeight);
			m_fontWeightStack.SetDefault(m_fontWeightInternal);
			m_fontStyleStack.Clear();
			m_lineJustification = m_textAlignment;
			m_lineJustificationStack.SetDefault(m_lineJustification);
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 1f;
			m_baselineOffset = 0f;
			m_baselineOffsetStack.Clear();
			bool flag = false;
			Vector3 zero = Vector3.get_zero();
			Vector3 zero2 = Vector3.get_zero();
			bool flag2 = false;
			Vector3 zero3 = Vector3.get_zero();
			Vector3 zero4 = Vector3.get_zero();
			bool flag3 = false;
			Vector3 val = Vector3.get_zero();
			Vector3 val2 = Vector3.get_zero();
			m_fontColor32 = Color32.op_Implicit(m_fontColor);
			m_htmlColor = m_fontColor32;
			m_underlineColor = m_htmlColor;
			m_strikethroughColor = m_htmlColor;
			m_colorStack.SetDefault(m_htmlColor);
			m_underlineColorStack.SetDefault(m_htmlColor);
			m_strikethroughColorStack.SetDefault(m_htmlColor);
			m_highlightColorStack.SetDefault(m_htmlColor);
			m_colorGradientPreset = null;
			m_colorGradientStack.SetDefault(null);
			m_actionStack.Clear();
			m_isFXMatrixSet = false;
			m_lineOffset = 0f;
			m_lineHeight = -32767f;
			float num8 = m_currentFontAsset.fontInfo.LineHeight - (m_currentFontAsset.fontInfo.Ascender - m_currentFontAsset.fontInfo.Descender);
			m_cSpacing = 0f;
			m_monoSpacing = 0f;
			float num9 = 0f;
			m_xAdvance = 0f;
			tag_LineIndent = 0f;
			tag_Indent = 0f;
			m_indentStack.SetDefault(0f);
			tag_NoParsing = false;
			m_characterCount = 0;
			m_firstCharacterOfLine = 0;
			m_lastCharacterOfLine = 0;
			m_firstVisibleCharacterOfLine = 0;
			m_lastVisibleCharacterOfLine = 0;
			m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
			m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
			m_lineNumber = 0;
			m_lineVisibleCharacterCount = 0;
			bool flag4 = true;
			m_firstOverflowCharacterIndex = -1;
			m_pageNumber = 0;
			int num10 = Mathf.Clamp(m_pageToDisplay - 1, 0, m_textInfo.pageInfo.Length - 1);
			int num11 = 0;
			int num12 = 0;
			Vector4 val3 = m_margin;
			float marginWidth = m_marginWidth;
			float marginHeight = m_marginHeight;
			m_marginLeft = 0f;
			m_marginRight = 0f;
			m_width = -1f;
			float num13 = marginWidth + 0.0001f - m_marginLeft - m_marginRight;
			m_meshExtents.min = TMP_Text.k_LargePositiveVector2;
			m_meshExtents.max = TMP_Text.k_LargeNegativeVector2;
			m_textInfo.ClearLineInfo();
			m_maxCapHeight = 0f;
			m_maxAscender = 0f;
			m_maxDescender = 0f;
			float num14 = 0f;
			float num15 = 0f;
			bool flag5 = false;
			m_isNewPage = false;
			bool flag6 = true;
			m_isNonBreakingSpace = false;
			bool flag7 = false;
			bool flag8 = false;
			int num16 = 0;
			SaveWordWrappingState(ref m_SavedWordWrapState, -1, -1);
			SaveWordWrappingState(ref m_SavedLineState, -1, -1);
			loopCountA++;
			int endIndex = 0;
			Vector3 val4 = default(Vector3);
			Vector3 val5 = default(Vector3);
			Vector3 val6 = default(Vector3);
			Vector3 val7 = default(Vector3);
			Vector3 val8 = default(Vector3);
			Vector3 val9 = default(Vector3);
			for (int i = 0; i < m_char_buffer.Length && m_char_buffer[i] != 0; i++)
			{
				num4 = m_char_buffer[i];
				m_textElementType = m_textInfo.characterInfo[m_characterCount].elementType;
				m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
				m_currentFontAsset = m_materialReferences[m_currentMaterialIndex].fontAsset;
				int currentMaterialIndex = m_currentMaterialIndex;
				if (m_isRichText && num4 == 60)
				{
					m_isParsingText = true;
					m_textElementType = TMP_TextElementType.Character;
					if (ValidateHtmlTag(m_char_buffer, i + 1, out endIndex))
					{
						i = endIndex;
						if (m_textElementType == TMP_TextElementType.Character)
						{
							continue;
						}
					}
				}
				m_isParsingText = false;
				bool isUsingAlternateTypeface = m_textInfo.characterInfo[m_characterCount].isUsingAlternateTypeface;
				if (m_characterCount < m_firstVisibleCharacter)
				{
					m_textInfo.characterInfo[m_characterCount].isVisible = false;
					m_textInfo.characterInfo[m_characterCount].character = '\u200b';
					m_characterCount++;
					continue;
				}
				float num17 = 1f;
				if (m_textElementType == TMP_TextElementType.Character)
				{
					if ((m_style & FontStyles.UpperCase) == FontStyles.UpperCase)
					{
						if (char.IsLower((char)num4))
						{
							num4 = char.ToUpper((char)num4);
						}
					}
					else if ((m_style & FontStyles.LowerCase) == FontStyles.LowerCase)
					{
						if (char.IsUpper((char)num4))
						{
							num4 = char.ToLower((char)num4);
						}
					}
					else if (((m_fontStyle & FontStyles.SmallCaps) == FontStyles.SmallCaps || (m_style & FontStyles.SmallCaps) == FontStyles.SmallCaps) && char.IsLower((char)num4))
					{
						num17 = 0.8f;
						num4 = char.ToUpper((char)num4);
					}
				}
				if (m_textElementType == TMP_TextElementType.Sprite)
				{
					m_currentSpriteAsset = m_textInfo.characterInfo[m_characterCount].spriteAsset;
					m_spriteIndex = m_textInfo.characterInfo[m_characterCount].spriteIndex;
					TMP_Sprite tMP_Sprite = m_currentSpriteAsset.spriteInfoList[m_spriteIndex];
					if (tMP_Sprite == null)
					{
						continue;
					}
					if (num4 == 60)
					{
						num4 = 57344 + m_spriteIndex;
					}
					else
					{
						m_spriteColor = TMP_Text.s_colorWhite;
					}
					m_currentFontAsset = m_fontAsset;
					float num18 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale;
					num2 = m_fontAsset.fontInfo.Ascender / tMP_Sprite.height * tMP_Sprite.scale * num18;
					m_cached_TextElement = tMP_Sprite;
					m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Sprite;
					m_textInfo.characterInfo[m_characterCount].scale = num18;
					m_textInfo.characterInfo[m_characterCount].spriteAsset = m_currentSpriteAsset;
					m_textInfo.characterInfo[m_characterCount].fontAsset = m_currentFontAsset;
					m_textInfo.characterInfo[m_characterCount].materialReferenceIndex = m_currentMaterialIndex;
					m_currentMaterialIndex = currentMaterialIndex;
					num5 = 0f;
				}
				else if (m_textElementType == TMP_TextElementType.Character)
				{
					m_cached_TextElement = m_textInfo.characterInfo[m_characterCount].textElement;
					if (m_cached_TextElement == null)
					{
						continue;
					}
					m_currentFontAsset = m_textInfo.characterInfo[m_characterCount].fontAsset;
					m_currentMaterial = m_textInfo.characterInfo[m_characterCount].material;
					m_currentMaterialIndex = m_textInfo.characterInfo[m_characterCount].materialReferenceIndex;
					m_fontScale = m_currentFontSize * num17 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale;
					num2 = m_fontScale * m_fontScaleMultiplier * m_cached_TextElement.scale;
					m_textInfo.characterInfo[m_characterCount].elementType = TMP_TextElementType.Character;
					m_textInfo.characterInfo[m_characterCount].scale = num2;
					num5 = ((m_currentMaterialIndex == 0) ? m_padding : m_subTextObjects[m_currentMaterialIndex].padding);
				}
				float num19 = num2;
				if (num4 == 173)
				{
					num2 = 0f;
				}
				if (m_isRightToLeft)
				{
					m_xAdvance -= ((m_cached_TextElement.xAdvance * num7 + m_characterSpacing + m_wordSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num4) || num4 == 8203)
					{
						m_xAdvance -= m_wordSpacing * num2;
					}
				}
				m_textInfo.characterInfo[m_characterCount].character = (char)num4;
				m_textInfo.characterInfo[m_characterCount].pointSize = m_currentFontSize;
				m_textInfo.characterInfo[m_characterCount].color = m_htmlColor;
				m_textInfo.characterInfo[m_characterCount].underlineColor = m_underlineColor;
				m_textInfo.characterInfo[m_characterCount].strikethroughColor = m_strikethroughColor;
				m_textInfo.characterInfo[m_characterCount].highlightColor = m_highlightColor;
				m_textInfo.characterInfo[m_characterCount].style = m_style;
				m_textInfo.characterInfo[m_characterCount].index = i;
				if (m_enableKerning && m_characterCount >= 1)
				{
					int character = m_textInfo.characterInfo[m_characterCount - 1].character;
					KerningPairKey kerningPairKey = new KerningPairKey(character, num4);
					m_currentFontAsset.kerningDictionary.TryGetValue(kerningPairKey.key, out var value);
					if (value != null)
					{
						m_xAdvance += value.XadvanceOffset * num2;
					}
				}
				float num20 = 0f;
				if (m_monoSpacing != 0f)
				{
					num20 = (m_monoSpacing / 2f - (m_cached_TextElement.width / 2f + m_cached_TextElement.xOffset) * num2) * (1f - m_charWidthAdjDelta);
					m_xAdvance += num20;
				}
				if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && ((m_style & FontStyles.Bold) == FontStyles.Bold || (m_fontStyle & FontStyles.Bold) == FontStyles.Bold))
				{
					if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
					{
						float @float = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
						num6 = m_currentFontAsset.boldStyle / 4f * @float * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
						if (num6 + num5 > @float)
						{
							num5 = @float - num6;
						}
					}
					else
					{
						num6 = 0f;
					}
					num7 = 1f + m_currentFontAsset.boldSpacing * 0.01f;
				}
				else
				{
					if (m_currentMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
					{
						float float2 = m_currentMaterial.GetFloat(ShaderUtilities.ID_GradientScale);
						num6 = m_currentFontAsset.normalStyle / 4f * float2 * m_currentMaterial.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
						if (num6 + num5 > float2)
						{
							num5 = float2 - num6;
						}
					}
					else
					{
						num6 = 0f;
					}
					num7 = 1f;
				}
				float baseline = m_currentFontAsset.fontInfo.Baseline;
				val4.x = m_xAdvance + (m_cached_TextElement.xOffset - num5 - num6) * num2 * (1f - m_charWidthAdjDelta);
				val4.y = (baseline + m_cached_TextElement.yOffset + num5) * num2 - m_lineOffset + m_baselineOffset;
				val4.z = 0f;
				val5.x = val4.x;
				val5.y = val4.y - (m_cached_TextElement.height + num5 * 2f) * num2;
				val5.z = 0f;
				val6.x = val5.x + (m_cached_TextElement.width + num5 * 2f + num6 * 2f) * num2 * (1f - m_charWidthAdjDelta);
				val6.y = val4.y;
				val6.z = 0f;
				val7.x = val6.x;
				val7.y = val5.y;
				val7.z = 0f;
				if (m_textElementType == TMP_TextElementType.Character && !isUsingAlternateTypeface && ((m_style & FontStyles.Italic) == FontStyles.Italic || (m_fontStyle & FontStyles.Italic) == FontStyles.Italic))
				{
					float num21 = (float)(int)m_currentFontAsset.italicStyle * 0.01f;
					((Vector3)(ref val8))._002Ector(num21 * ((m_cached_TextElement.yOffset + num5 + num6) * num2), 0f, 0f);
					((Vector3)(ref val9))._002Ector(num21 * ((m_cached_TextElement.yOffset - m_cached_TextElement.height - num5 - num6) * num2), 0f, 0f);
					val4 += val8;
					val5 += val9;
					val6 += val8;
					val7 += val9;
				}
				if (m_isFXMatrixSet)
				{
					Vector3 val10 = (val6 + val5) / 2f;
					val4 = ((Matrix4x4)(ref m_FXMatrix)).MultiplyPoint3x4(val4 - val10) + val10;
					val5 = ((Matrix4x4)(ref m_FXMatrix)).MultiplyPoint3x4(val5 - val10) + val10;
					val6 = ((Matrix4x4)(ref m_FXMatrix)).MultiplyPoint3x4(val6 - val10) + val10;
					val7 = ((Matrix4x4)(ref m_FXMatrix)).MultiplyPoint3x4(val7 - val10) + val10;
				}
				m_textInfo.characterInfo[m_characterCount].bottomLeft = val5;
				m_textInfo.characterInfo[m_characterCount].topLeft = val4;
				m_textInfo.characterInfo[m_characterCount].topRight = val6;
				m_textInfo.characterInfo[m_characterCount].bottomRight = val7;
				m_textInfo.characterInfo[m_characterCount].origin = m_xAdvance;
				m_textInfo.characterInfo[m_characterCount].baseLine = 0f - m_lineOffset + m_baselineOffset;
				m_textInfo.characterInfo[m_characterCount].aspectRatio = (val6.x - val5.x) / (val4.y - val5.y);
				float num22 = m_currentFontAsset.fontInfo.Ascender * ((m_textElementType == TMP_TextElementType.Character) ? (num2 / num17) : m_textInfo.characterInfo[m_characterCount].scale) + m_baselineOffset;
				m_textInfo.characterInfo[m_characterCount].ascender = num22 - m_lineOffset;
				m_maxLineAscender = ((num22 > m_maxLineAscender) ? num22 : m_maxLineAscender);
				float num23 = m_currentFontAsset.fontInfo.Descender * ((m_textElementType == TMP_TextElementType.Character) ? (num2 / num17) : m_textInfo.characterInfo[m_characterCount].scale) + m_baselineOffset;
				float num24 = (m_textInfo.characterInfo[m_characterCount].descender = num23 - m_lineOffset);
				m_maxLineDescender = ((num23 < m_maxLineDescender) ? num23 : m_maxLineDescender);
				if ((m_style & FontStyles.Subscript) == FontStyles.Subscript || (m_style & FontStyles.Superscript) == FontStyles.Superscript)
				{
					float num25 = (num22 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
					num22 = m_maxLineAscender;
					m_maxLineAscender = ((num25 > m_maxLineAscender) ? num25 : m_maxLineAscender);
					float num26 = (num23 - m_baselineOffset) / m_currentFontAsset.fontInfo.SubSize;
					num23 = m_maxLineDescender;
					m_maxLineDescender = ((num26 < m_maxLineDescender) ? num26 : m_maxLineDescender);
				}
				if (m_lineNumber == 0 || m_isNewPage)
				{
					m_maxAscender = ((m_maxAscender > num22) ? m_maxAscender : num22);
					m_maxCapHeight = Mathf.Max(m_maxCapHeight, m_currentFontAsset.fontInfo.CapHeight * num2 / num17);
				}
				if (m_lineOffset == 0f)
				{
					num14 = ((num14 > num22) ? num14 : num22);
				}
				m_textInfo.characterInfo[m_characterCount].isVisible = false;
				if (num4 == 9 || (!char.IsWhiteSpace((char)num4) && num4 != 8203) || m_textElementType == TMP_TextElementType.Sprite)
				{
					m_textInfo.characterInfo[m_characterCount].isVisible = true;
					num13 = ((m_width != -1f) ? Mathf.Min(marginWidth + 0.0001f - m_marginLeft - m_marginRight, m_width) : (marginWidth + 0.0001f - m_marginLeft - m_marginRight));
					m_textInfo.lineInfo[m_lineNumber].marginLeft = m_marginLeft;
					bool flag9 = (m_lineJustification & (TextAlignmentOptions)16) == (TextAlignmentOptions)16 || (m_lineJustification & (TextAlignmentOptions)8) == (TextAlignmentOptions)8;
					if (Mathf.Abs(m_xAdvance) + ((!m_isRightToLeft) ? m_cached_TextElement.xAdvance : 0f) * (1f - m_charWidthAdjDelta) * ((num4 != 173) ? num2 : num19) > num13 * (flag9 ? 1.05f : 1f))
					{
						num12 = m_characterCount - 1;
						if (base.enableWordWrapping && m_characterCount != m_firstCharacterOfLine)
						{
							if (num16 == m_SavedWordWrapState.previous_WordBreak || flag6)
							{
								if (m_enableAutoSizing && m_fontSize > m_fontSizeMin)
								{
									if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
									{
										loopCountA = 0;
										m_charWidthAdjDelta += 0.01f;
										GenerateTextMesh();
										return;
									}
									m_maxFontSize = m_fontSize;
									m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
									m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
									if (loopCountA <= 20)
									{
										GenerateTextMesh();
									}
									return;
								}
								if (!m_isCharacterWrappingEnabled)
								{
									if (!flag7)
									{
										flag7 = true;
									}
									else
									{
										m_isCharacterWrappingEnabled = true;
									}
								}
								else
								{
									flag8 = true;
								}
							}
							i = RestoreWordWrappingState(ref m_SavedWordWrapState);
							num16 = i;
							if (m_char_buffer[i] == 173)
							{
								m_isTextTruncated = true;
								m_char_buffer[i] = 45;
								GenerateTextMesh();
								return;
							}
							if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f && !m_isNewPage)
							{
								float num27 = m_maxLineAscender - m_startOfLineAscender;
								AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num27);
								m_lineOffset += num27;
								m_SavedWordWrapState.lineOffset = m_lineOffset;
								m_SavedWordWrapState.previousLineAscender = m_maxLineAscender;
							}
							m_isNewPage = false;
							float num28 = m_maxLineAscender - m_lineOffset;
							float num29 = m_maxLineDescender - m_lineOffset;
							m_maxDescender = ((m_maxDescender < num29) ? m_maxDescender : num29);
							if (!flag5)
							{
								num15 = m_maxDescender;
							}
							if (m_useMaxVisibleDescender && (m_characterCount >= m_maxVisibleCharacters || m_lineNumber >= m_maxVisibleLines))
							{
								flag5 = true;
							}
							m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
							m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = (m_firstVisibleCharacterOfLine = ((m_firstCharacterOfLine > m_firstVisibleCharacterOfLine) ? m_firstCharacterOfLine : m_firstVisibleCharacterOfLine));
							m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = ((m_characterCount - 1 > 0) ? (m_characterCount - 1) : 0));
							m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = (m_lastVisibleCharacterOfLine = ((m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine) ? m_firstVisibleCharacterOfLine : m_lastVisibleCharacterOfLine));
							m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
							m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
							m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num29);
							m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num28);
							m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x;
							m_textInfo.lineInfo[m_lineNumber].width = num13;
							m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
							m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
							m_textInfo.lineInfo[m_lineNumber].ascender = num28;
							m_textInfo.lineInfo[m_lineNumber].descender = num29;
							m_textInfo.lineInfo[m_lineNumber].lineHeight = num28 - num29 + num8 * num;
							m_firstCharacterOfLine = m_characterCount;
							m_lineVisibleCharacterCount = 0;
							SaveWordWrappingState(ref m_SavedLineState, i, m_characterCount - 1);
							m_lineNumber++;
							flag4 = true;
							if (m_lineNumber >= m_textInfo.lineInfo.Length)
							{
								ResizeLineExtents(m_lineNumber);
							}
							if (m_lineHeight == -32767f)
							{
								float num30 = m_textInfo.characterInfo[m_characterCount].ascender - m_textInfo.characterInfo[m_characterCount].baseLine;
								num9 = 0f - m_maxLineDescender + num30 + (num8 + m_lineSpacing + m_lineSpacingDelta) * num;
								m_lineOffset += num9;
								m_startOfLineAscender = num30;
							}
							else
							{
								m_lineOffset += m_lineHeight + m_lineSpacing * num;
							}
							m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
							m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
							m_xAdvance = 0f + tag_Indent;
							continue;
						}
						if (m_enableAutoSizing && m_fontSize > m_fontSizeMin)
						{
							if (m_charWidthAdjDelta < m_charWidthMaxAdj / 100f)
							{
								loopCountA = 0;
								m_charWidthAdjDelta += 0.01f;
								GenerateTextMesh();
								return;
							}
							m_maxFontSize = m_fontSize;
							m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
							m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
							if (loopCountA <= 20)
							{
								GenerateTextMesh();
							}
							return;
						}
						switch (m_overflowMode)
						{
						case TextOverflowModes.Overflow:
							if (m_isMaskingEnabled)
							{
								DisableMasking();
							}
							break;
						case TextOverflowModes.Ellipsis:
							if (m_isMaskingEnabled)
							{
								DisableMasking();
							}
							m_isTextTruncated = true;
							if (m_characterCount < 1)
							{
								m_textInfo.characterInfo[m_characterCount].isVisible = false;
								break;
							}
							m_char_buffer[i - 1] = 8230;
							m_char_buffer[i] = 0;
							if (m_cached_Ellipsis_GlyphInfo != null)
							{
								m_textInfo.characterInfo[num12].character = '…';
								m_textInfo.characterInfo[num12].textElement = m_cached_Ellipsis_GlyphInfo;
								m_textInfo.characterInfo[num12].fontAsset = m_materialReferences[0].fontAsset;
								m_textInfo.characterInfo[num12].material = m_materialReferences[0].material;
								m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
							}
							else
							{
								Debug.LogWarning((object)("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + ((Object)m_fontAsset).get_name() + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file."), (Object)(object)this);
							}
							m_totalCharacterCount = num12 + 1;
							GenerateTextMesh();
							return;
						case TextOverflowModes.Masking:
							if (!m_isMaskingEnabled)
							{
								EnableMasking();
							}
							break;
						case TextOverflowModes.ScrollRect:
							if (!m_isMaskingEnabled)
							{
								EnableMasking();
							}
							break;
						case TextOverflowModes.Truncate:
							if (m_isMaskingEnabled)
							{
								DisableMasking();
							}
							m_textInfo.characterInfo[m_characterCount].isVisible = false;
							break;
						}
					}
					if (num4 != 9)
					{
						Color32 vertexColor = ((!m_overrideHtmlColors) ? m_htmlColor : m_fontColor32);
						if (m_textElementType == TMP_TextElementType.Character)
						{
							SaveGlyphVertexInfo(num5, num6, vertexColor);
						}
						else if (m_textElementType == TMP_TextElementType.Sprite)
						{
							SaveSpriteVertexInfo(vertexColor);
						}
					}
					else
					{
						m_textInfo.characterInfo[m_characterCount].isVisible = false;
						m_lastVisibleCharacterOfLine = m_characterCount;
						m_textInfo.lineInfo[m_lineNumber].spaceCount++;
						m_textInfo.spaceCount++;
					}
					if (m_textInfo.characterInfo[m_characterCount].isVisible && num4 != 173)
					{
						if (flag4)
						{
							flag4 = false;
							m_firstVisibleCharacterOfLine = m_characterCount;
						}
						m_lineVisibleCharacterCount++;
						m_lastVisibleCharacterOfLine = m_characterCount;
					}
				}
				else if ((num4 == 10 || char.IsSeparator((char)num4)) && num4 != 173 && num4 != 8203 && num4 != 8288)
				{
					m_textInfo.lineInfo[m_lineNumber].spaceCount++;
					m_textInfo.spaceCount++;
				}
				if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f && !m_isNewPage)
				{
					float num31 = m_maxLineAscender - m_startOfLineAscender;
					AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num31);
					num24 -= num31;
					m_lineOffset += num31;
					m_startOfLineAscender += num31;
					m_SavedWordWrapState.lineOffset = m_lineOffset;
					m_SavedWordWrapState.previousLineAscender = m_startOfLineAscender;
				}
				m_textInfo.characterInfo[m_characterCount].lineNumber = m_lineNumber;
				m_textInfo.characterInfo[m_characterCount].pageNumber = m_pageNumber;
				if ((num4 != 10 && num4 != 13 && num4 != 8230) || m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
				{
					m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
				}
				if (m_maxAscender - num24 > marginHeight + 0.0001f)
				{
					if (m_enableAutoSizing && m_lineSpacingDelta > m_lineSpacingMax && m_lineNumber > 0)
					{
						loopCountA = 0;
						m_lineSpacingDelta -= 1f;
						GenerateTextMesh();
						return;
					}
					if (m_enableAutoSizing && m_fontSize > m_fontSizeMin)
					{
						m_maxFontSize = m_fontSize;
						m_fontSize -= Mathf.Max((m_fontSize - m_minFontSize) / 2f, 0.05f);
						m_fontSize = (float)(int)(Mathf.Max(m_fontSize, m_fontSizeMin) * 20f + 0.5f) / 20f;
						if (loopCountA <= 20)
						{
							GenerateTextMesh();
						}
						return;
					}
					if (m_firstOverflowCharacterIndex == -1)
					{
						m_firstOverflowCharacterIndex = m_characterCount;
					}
					switch (m_overflowMode)
					{
					case TextOverflowModes.Overflow:
						if (m_isMaskingEnabled)
						{
							DisableMasking();
						}
						break;
					case TextOverflowModes.Ellipsis:
						if (m_isMaskingEnabled)
						{
							DisableMasking();
						}
						if (m_lineNumber > 0)
						{
							m_char_buffer[m_textInfo.characterInfo[num12].index] = 8230;
							m_char_buffer[m_textInfo.characterInfo[num12].index + 1] = 0;
							if (m_cached_Ellipsis_GlyphInfo != null)
							{
								m_textInfo.characterInfo[num12].character = '…';
								m_textInfo.characterInfo[num12].textElement = m_cached_Ellipsis_GlyphInfo;
								m_textInfo.characterInfo[num12].fontAsset = m_materialReferences[0].fontAsset;
								m_textInfo.characterInfo[num12].material = m_materialReferences[0].material;
								m_textInfo.characterInfo[num12].materialReferenceIndex = 0;
							}
							else
							{
								Debug.LogWarning((object)("Unable to use Ellipsis character since it wasn't found in the current Font Asset [" + ((Object)m_fontAsset).get_name() + "]. Consider regenerating this font asset to include the Ellipsis character (u+2026).\nNote: Warnings can be disabled in the TMP Settings file."), (Object)(object)this);
							}
							m_totalCharacterCount = num12 + 1;
							GenerateTextMesh();
							m_isTextTruncated = true;
						}
						else
						{
							ClearMesh();
						}
						return;
					case TextOverflowModes.Masking:
						if (!m_isMaskingEnabled)
						{
							EnableMasking();
						}
						break;
					case TextOverflowModes.ScrollRect:
						if (!m_isMaskingEnabled)
						{
							EnableMasking();
						}
						break;
					case TextOverflowModes.Truncate:
						if (m_isMaskingEnabled)
						{
							DisableMasking();
						}
						if (m_lineNumber > 0)
						{
							m_char_buffer[m_textInfo.characterInfo[num12].index + 1] = 0;
							m_totalCharacterCount = num12 + 1;
							GenerateTextMesh();
							m_isTextTruncated = true;
						}
						else
						{
							ClearMesh();
						}
						return;
					case TextOverflowModes.Page:
						if (m_isMaskingEnabled)
						{
							DisableMasking();
						}
						if (num4 != 13 && num4 != 10)
						{
							if (i == 0)
							{
								ClearMesh();
								return;
							}
							if (num11 == i)
							{
								m_char_buffer[i] = 0;
								m_isTextTruncated = true;
							}
							num11 = i;
							i = RestoreWordWrappingState(ref m_SavedLineState);
							m_isNewPage = true;
							m_xAdvance = 0f + tag_Indent;
							m_lineOffset = 0f;
							m_maxAscender = 0f;
							num14 = 0f;
							m_lineNumber++;
							m_pageNumber++;
							continue;
						}
						break;
					case TextOverflowModes.Linked:
						if ((Object)(object)m_linkedTextComponent != (Object)null)
						{
							m_linkedTextComponent.text = base.text;
							m_linkedTextComponent.firstVisibleCharacter = m_characterCount;
							m_linkedTextComponent.ForceMeshUpdate();
						}
						if (m_lineNumber > 0)
						{
							m_char_buffer[i] = 0;
							m_totalCharacterCount = m_characterCount;
							GenerateTextMesh();
							m_isTextTruncated = true;
						}
						else
						{
							ClearMesh();
						}
						return;
					}
				}
				if (num4 == 9)
				{
					float num32 = m_currentFontAsset.fontInfo.TabWidth * num2;
					float num33 = Mathf.Ceil(m_xAdvance / num32) * num32;
					m_xAdvance = ((num33 > m_xAdvance) ? num33 : (m_xAdvance + num32));
				}
				else if (m_monoSpacing != 0f)
				{
					m_xAdvance += (m_monoSpacing - num20 + (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num4) || num4 == 8203)
					{
						m_xAdvance += m_wordSpacing * num2;
					}
				}
				else if (!m_isRightToLeft)
				{
					m_xAdvance += ((m_cached_TextElement.xAdvance * num7 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
					if (char.IsWhiteSpace((char)num4) || num4 == 8203)
					{
						m_xAdvance += m_wordSpacing * num2;
					}
				}
				m_textInfo.characterInfo[m_characterCount].xAdvance = m_xAdvance;
				if (num4 == 13)
				{
					m_xAdvance = 0f + tag_Indent;
				}
				if (num4 == 10 || m_characterCount == totalCharacterCount - 1)
				{
					if (m_lineNumber > 0 && !TMP_Math.Approximately(m_maxLineAscender, m_startOfLineAscender) && m_lineHeight == -32767f && !m_isNewPage)
					{
						float num34 = m_maxLineAscender - m_startOfLineAscender;
						AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num34);
						num24 -= num34;
						m_lineOffset += num34;
					}
					m_isNewPage = false;
					float num35 = m_maxLineAscender - m_lineOffset;
					float num36 = m_maxLineDescender - m_lineOffset;
					m_maxDescender = ((m_maxDescender < num36) ? m_maxDescender : num36);
					if (!flag5)
					{
						num15 = m_maxDescender;
					}
					if (m_useMaxVisibleDescender && (m_characterCount >= m_maxVisibleCharacters || m_lineNumber >= m_maxVisibleLines))
					{
						flag5 = true;
					}
					m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex = m_firstCharacterOfLine;
					m_textInfo.lineInfo[m_lineNumber].firstVisibleCharacterIndex = (m_firstVisibleCharacterOfLine = ((m_firstCharacterOfLine > m_firstVisibleCharacterOfLine) ? m_firstCharacterOfLine : m_firstVisibleCharacterOfLine));
					m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex = (m_lastCharacterOfLine = m_characterCount);
					m_textInfo.lineInfo[m_lineNumber].lastVisibleCharacterIndex = (m_lastVisibleCharacterOfLine = ((m_lastVisibleCharacterOfLine < m_firstVisibleCharacterOfLine) ? m_firstVisibleCharacterOfLine : m_lastVisibleCharacterOfLine));
					m_textInfo.lineInfo[m_lineNumber].characterCount = m_textInfo.lineInfo[m_lineNumber].lastCharacterIndex - m_textInfo.lineInfo[m_lineNumber].firstCharacterIndex + 1;
					m_textInfo.lineInfo[m_lineNumber].visibleCharacterCount = m_lineVisibleCharacterCount;
					m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num36);
					m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num35);
					m_textInfo.lineInfo[m_lineNumber].length = m_textInfo.lineInfo[m_lineNumber].lineExtents.max.x - num5 * num2;
					m_textInfo.lineInfo[m_lineNumber].width = num13;
					if (m_textInfo.lineInfo[m_lineNumber].characterCount == 1)
					{
						m_textInfo.lineInfo[m_lineNumber].alignment = m_lineJustification;
					}
					if (m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].isVisible)
					{
						m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
					}
					else
					{
						m_textInfo.lineInfo[m_lineNumber].maxAdvance = m_textInfo.characterInfo[m_lastCharacterOfLine].xAdvance - (m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 - m_cSpacing;
					}
					m_textInfo.lineInfo[m_lineNumber].baseline = 0f - m_lineOffset;
					m_textInfo.lineInfo[m_lineNumber].ascender = num35;
					m_textInfo.lineInfo[m_lineNumber].descender = num36;
					m_textInfo.lineInfo[m_lineNumber].lineHeight = num35 - num36 + num8 * num;
					m_firstCharacterOfLine = m_characterCount + 1;
					m_lineVisibleCharacterCount = 0;
					if (num4 == 10)
					{
						SaveWordWrappingState(ref m_SavedLineState, i, m_characterCount);
						SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
						m_lineNumber++;
						flag4 = true;
						flag7 = false;
						if (m_lineNumber >= m_textInfo.lineInfo.Length)
						{
							ResizeLineExtents(m_lineNumber);
						}
						if (m_lineHeight == -32767f)
						{
							num9 = 0f - m_maxLineDescender + num22 + (num8 + m_lineSpacing + m_paragraphSpacing + m_lineSpacingDelta) * num;
							m_lineOffset += num9;
						}
						else
						{
							m_lineOffset += m_lineHeight + (m_lineSpacing + m_paragraphSpacing) * num;
						}
						m_maxLineAscender = TMP_Text.k_LargeNegativeFloat;
						m_maxLineDescender = TMP_Text.k_LargePositiveFloat;
						m_startOfLineAscender = num22;
						m_xAdvance = 0f + tag_LineIndent + tag_Indent;
						num12 = m_characterCount - 1;
						m_characterCount++;
						continue;
					}
				}
				if (m_textInfo.characterInfo[m_characterCount].isVisible)
				{
					m_meshExtents.min.x = Mathf.Min(m_meshExtents.min.x, m_textInfo.characterInfo[m_characterCount].bottomLeft.x);
					m_meshExtents.min.y = Mathf.Min(m_meshExtents.min.y, m_textInfo.characterInfo[m_characterCount].bottomLeft.y);
					m_meshExtents.max.x = Mathf.Max(m_meshExtents.max.x, m_textInfo.characterInfo[m_characterCount].topRight.x);
					m_meshExtents.max.y = Mathf.Max(m_meshExtents.max.y, m_textInfo.characterInfo[m_characterCount].topRight.y);
				}
				if (m_overflowMode == TextOverflowModes.Page && num4 != 13 && num4 != 10)
				{
					if (m_pageNumber + 1 > m_textInfo.pageInfo.Length)
					{
						TMP_TextInfo.Resize(ref m_textInfo.pageInfo, m_pageNumber + 1, isBlockAllocated: true);
					}
					m_textInfo.pageInfo[m_pageNumber].ascender = num14;
					m_textInfo.pageInfo[m_pageNumber].descender = ((num23 < m_textInfo.pageInfo[m_pageNumber].descender) ? num23 : m_textInfo.pageInfo[m_pageNumber].descender);
					if (m_pageNumber == 0 && m_characterCount == 0)
					{
						m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
					}
					else if (m_characterCount > 0 && m_pageNumber != m_textInfo.characterInfo[m_characterCount - 1].pageNumber)
					{
						m_textInfo.pageInfo[m_pageNumber - 1].lastCharacterIndex = m_characterCount - 1;
						m_textInfo.pageInfo[m_pageNumber].firstCharacterIndex = m_characterCount;
					}
					else if (m_characterCount == totalCharacterCount - 1)
					{
						m_textInfo.pageInfo[m_pageNumber].lastCharacterIndex = m_characterCount;
					}
				}
				if (m_enableWordWrapping || m_overflowMode == TextOverflowModes.Truncate || m_overflowMode == TextOverflowModes.Ellipsis)
				{
					if ((char.IsWhiteSpace((char)num4) || num4 == 8203 || num4 == 45 || num4 == 173) && (!m_isNonBreakingSpace || flag7) && num4 != 160 && num4 != 8209 && num4 != 8239 && num4 != 8288)
					{
						SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
						m_isCharacterWrappingEnabled = false;
						flag6 = false;
					}
					else if (((num4 > 4352 && num4 < 4607) || (num4 > 11904 && num4 < 40959) || (num4 > 43360 && num4 < 43391) || (num4 > 44032 && num4 < 55295) || (num4 > 63744 && num4 < 64255) || (num4 > 65072 && num4 < 65103) || (num4 > 65280 && num4 < 65519)) && !m_isNonBreakingSpace)
					{
						if (flag6 || flag8 || (!TMP_Settings.linebreakingRules.leadingCharacters.ContainsKey(num4) && m_characterCount < totalCharacterCount - 1 && !TMP_Settings.linebreakingRules.followingCharacters.ContainsKey(m_textInfo.characterInfo[m_characterCount + 1].character)))
						{
							SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
							m_isCharacterWrappingEnabled = false;
							flag6 = false;
						}
					}
					else if (flag6 || m_isCharacterWrappingEnabled || flag8)
					{
						SaveWordWrappingState(ref m_SavedWordWrapState, i, m_characterCount);
					}
				}
				m_characterCount++;
			}
			num3 = m_maxFontSize - m_minFontSize;
			if (!m_isCharacterWrappingEnabled && m_enableAutoSizing && num3 > 0.051f && m_fontSize < m_fontSizeMax)
			{
				m_minFontSize = m_fontSize;
				m_fontSize += Mathf.Max((m_maxFontSize - m_fontSize) / 2f, 0.05f);
				m_fontSize = (float)(int)(Mathf.Min(m_fontSize, m_fontSizeMax) * 20f + 0.5f) / 20f;
				if (loopCountA <= 20)
				{
					GenerateTextMesh();
				}
				return;
			}
			m_isCharacterWrappingEnabled = false;
			if (m_characterCount == 0)
			{
				ClearMesh();
				TMPro_EventManager.ON_TEXT_CHANGED((Object)(object)this);
				return;
			}
			int index = m_materialReferences[0].referenceCount * 4;
			m_textInfo.meshInfo[0].Clear(uploadChanges: false);
			Vector3 val11 = Vector3.get_zero();
			Vector3[] rectTransformCorners = m_RectTransformCorners;
			switch (m_textAlignment)
			{
			case TextAlignmentOptions.TopLeft:
			case TextAlignmentOptions.Top:
			case TextAlignmentOptions.TopRight:
			case TextAlignmentOptions.TopJustified:
			case TextAlignmentOptions.TopFlush:
			case TextAlignmentOptions.TopGeoAligned:
				val11 = ((m_overflowMode == TextOverflowModes.Page) ? (rectTransformCorners[1] + new Vector3(0f + val3.x, 0f - m_textInfo.pageInfo[num10].ascender - val3.y, 0f)) : (rectTransformCorners[1] + new Vector3(0f + val3.x, 0f - m_maxAscender - val3.y, 0f)));
				break;
			case TextAlignmentOptions.Left:
			case TextAlignmentOptions.Center:
			case TextAlignmentOptions.Right:
			case TextAlignmentOptions.Justified:
			case TextAlignmentOptions.Flush:
			case TextAlignmentOptions.CenterGeoAligned:
				val11 = ((m_overflowMode == TextOverflowModes.Page) ? ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + val3.x, 0f - (m_textInfo.pageInfo[num10].ascender + val3.y + m_textInfo.pageInfo[num10].descender - val3.w) / 2f, 0f)) : ((rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + val3.x, 0f - (m_maxAscender + val3.y + num15 - val3.w) / 2f, 0f)));
				break;
			case TextAlignmentOptions.BottomLeft:
			case TextAlignmentOptions.Bottom:
			case TextAlignmentOptions.BottomRight:
			case TextAlignmentOptions.BottomJustified:
			case TextAlignmentOptions.BottomFlush:
			case TextAlignmentOptions.BottomGeoAligned:
				val11 = ((m_overflowMode == TextOverflowModes.Page) ? (rectTransformCorners[0] + new Vector3(0f + val3.x, 0f - m_textInfo.pageInfo[num10].descender + val3.w, 0f)) : (rectTransformCorners[0] + new Vector3(0f + val3.x, 0f - num15 + val3.w, 0f)));
				break;
			case TextAlignmentOptions.BaselineLeft:
			case TextAlignmentOptions.Baseline:
			case TextAlignmentOptions.BaselineRight:
			case TextAlignmentOptions.BaselineJustified:
			case TextAlignmentOptions.BaselineFlush:
			case TextAlignmentOptions.BaselineGeoAligned:
				val11 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + val3.x, 0f, 0f);
				break;
			case TextAlignmentOptions.MidlineLeft:
			case TextAlignmentOptions.Midline:
			case TextAlignmentOptions.MidlineRight:
			case TextAlignmentOptions.MidlineJustified:
			case TextAlignmentOptions.MidlineFlush:
			case TextAlignmentOptions.MidlineGeoAligned:
				val11 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + val3.x, 0f - (m_meshExtents.max.y + val3.y + m_meshExtents.min.y - val3.w) / 2f, 0f);
				break;
			case TextAlignmentOptions.CaplineLeft:
			case TextAlignmentOptions.Capline:
			case TextAlignmentOptions.CaplineRight:
			case TextAlignmentOptions.CaplineJustified:
			case TextAlignmentOptions.CaplineFlush:
			case TextAlignmentOptions.CaplineGeoAligned:
				val11 = (rectTransformCorners[0] + rectTransformCorners[1]) / 2f + new Vector3(0f + val3.x, 0f - (m_maxCapHeight - val3.y - val3.w) / 2f, 0f);
				break;
			}
			Vector3 val12 = Vector3.get_zero();
			Vector3 zero5 = Vector3.get_zero();
			int index_X = 0;
			int index_X2 = 0;
			int num37 = 0;
			int lineCount = 0;
			int num38 = 0;
			bool flag10 = false;
			bool flag11 = false;
			int num39 = 0;
			int num40 = 0;
			bool flag12 = ((!((Object)(object)m_canvas.get_worldCamera() == (Object)null)) ? true : false);
			float num41 = (m_previousLossyScaleY = base.transform.get_lossyScale().y);
			RenderMode val13 = m_canvas.get_renderMode();
			float scaleFactor = m_canvas.get_scaleFactor();
			Color32 val14 = Color32.op_Implicit(Color.get_white());
			Color32 underlineColor = Color32.op_Implicit(Color.get_white());
			Color32 highlightColor = default(Color32);
			((Color32)(ref highlightColor))._002Ector(byte.MaxValue, byte.MaxValue, (byte)0, (byte)64);
			float num42 = 0f;
			float num43 = 0f;
			float num44 = 0f;
			float num45 = 0f;
			float num46 = TMP_Text.k_LargePositiveFloat;
			int num47 = 0;
			float num48 = 0f;
			float num49 = 0f;
			float b = 0f;
			TMP_CharacterInfo[] characterInfo = m_textInfo.characterInfo;
			for (int j = 0; j < m_characterCount; j++)
			{
				TMP_FontAsset fontAsset = characterInfo[j].fontAsset;
				char character2 = characterInfo[j].character;
				int lineNumber = characterInfo[j].lineNumber;
				TMP_LineInfo tMP_LineInfo = m_textInfo.lineInfo[lineNumber];
				lineCount = lineNumber + 1;
				TextAlignmentOptions textAlignmentOptions = tMP_LineInfo.alignment;
				switch (textAlignmentOptions)
				{
				case TextAlignmentOptions.TopLeft:
				case TextAlignmentOptions.Left:
				case TextAlignmentOptions.BottomLeft:
				case TextAlignmentOptions.BaselineLeft:
				case TextAlignmentOptions.MidlineLeft:
				case TextAlignmentOptions.CaplineLeft:
					if (!m_isRightToLeft)
					{
						((Vector3)(ref val12))._002Ector(0f + tMP_LineInfo.marginLeft, 0f, 0f);
					}
					else
					{
						((Vector3)(ref val12))._002Ector(0f - tMP_LineInfo.maxAdvance, 0f, 0f);
					}
					break;
				case TextAlignmentOptions.Top:
				case TextAlignmentOptions.Center:
				case TextAlignmentOptions.Bottom:
				case TextAlignmentOptions.Baseline:
				case TextAlignmentOptions.Midline:
				case TextAlignmentOptions.Capline:
					((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - tMP_LineInfo.maxAdvance / 2f, 0f, 0f);
					break;
				case TextAlignmentOptions.TopGeoAligned:
				case TextAlignmentOptions.CenterGeoAligned:
				case TextAlignmentOptions.BottomGeoAligned:
				case TextAlignmentOptions.BaselineGeoAligned:
				case TextAlignmentOptions.MidlineGeoAligned:
				case TextAlignmentOptions.CaplineGeoAligned:
					((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width / 2f - (tMP_LineInfo.lineExtents.min.x + tMP_LineInfo.lineExtents.max.x) / 2f, 0f, 0f);
					break;
				case TextAlignmentOptions.TopRight:
				case TextAlignmentOptions.Right:
				case TextAlignmentOptions.BottomRight:
				case TextAlignmentOptions.BaselineRight:
				case TextAlignmentOptions.MidlineRight:
				case TextAlignmentOptions.CaplineRight:
					if (!m_isRightToLeft)
					{
						((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width - tMP_LineInfo.maxAdvance, 0f, 0f);
					}
					else
					{
						((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
					}
					break;
				case TextAlignmentOptions.TopJustified:
				case TextAlignmentOptions.TopFlush:
				case TextAlignmentOptions.Justified:
				case TextAlignmentOptions.Flush:
				case TextAlignmentOptions.BottomJustified:
				case TextAlignmentOptions.BottomFlush:
				case TextAlignmentOptions.BaselineJustified:
				case TextAlignmentOptions.BaselineFlush:
				case TextAlignmentOptions.MidlineJustified:
				case TextAlignmentOptions.MidlineFlush:
				case TextAlignmentOptions.CaplineJustified:
				case TextAlignmentOptions.CaplineFlush:
				{
					if (character2 == '­' || character2 == '\u200b' || character2 == '\u2060')
					{
						break;
					}
					char character3 = characterInfo[tMP_LineInfo.lastCharacterIndex].character;
					bool flag13 = (textAlignmentOptions & (TextAlignmentOptions)16) == (TextAlignmentOptions)16;
					if ((!char.IsControl(character3) && lineNumber < m_lineNumber) || flag13 || tMP_LineInfo.maxAdvance > tMP_LineInfo.width)
					{
						if (lineNumber != num38 || j == 0 || j == m_firstVisibleCharacter)
						{
							if (!m_isRightToLeft)
							{
								((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft, 0f, 0f);
							}
							else
							{
								((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
							}
							flag10 = (char.IsSeparator(character2) ? true : false);
							break;
						}
						float num50 = ((!m_isRightToLeft) ? (tMP_LineInfo.width - tMP_LineInfo.maxAdvance) : (tMP_LineInfo.width + tMP_LineInfo.maxAdvance));
						int num51 = tMP_LineInfo.visibleCharacterCount - 1;
						int num52 = (characterInfo[tMP_LineInfo.lastCharacterIndex].isVisible ? tMP_LineInfo.spaceCount : (tMP_LineInfo.spaceCount - 1));
						if (flag10)
						{
							num52--;
							num51++;
						}
						float num53 = ((num52 > 0) ? m_wordWrappingRatios : 1f);
						if (num52 < 1)
						{
							num52 = 1;
						}
						val12 = ((character2 != '\t' && !char.IsSeparator(character2)) ? (m_isRightToLeft ? (val12 - new Vector3(num50 * num53 / (float)num51, 0f, 0f)) : (val12 + new Vector3(num50 * num53 / (float)num51, 0f, 0f))) : (m_isRightToLeft ? (val12 - new Vector3(num50 * (1f - num53) / (float)num52, 0f, 0f)) : (val12 + new Vector3(num50 * (1f - num53) / (float)num52, 0f, 0f))));
					}
					else if (!m_isRightToLeft)
					{
						((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft, 0f, 0f);
					}
					else
					{
						((Vector3)(ref val12))._002Ector(tMP_LineInfo.marginLeft + tMP_LineInfo.width, 0f, 0f);
					}
					break;
				}
				}
				zero5 = val11 + val12;
				if (characterInfo[j].isVisible)
				{
					TMP_TextElementType elementType = characterInfo[j].elementType;
					switch (elementType)
					{
					case TMP_TextElementType.Character:
					{
						Extents lineExtents = tMP_LineInfo.lineExtents;
						float num54 = m_uvLineOffset * (float)lineNumber % 1f;
						switch (m_horizontalMapping)
						{
						case TextureMappingOptions.Character:
							characterInfo[j].vertex_BL.uv2.x = 0f;
							characterInfo[j].vertex_TL.uv2.x = 0f;
							characterInfo[j].vertex_TR.uv2.x = 1f;
							characterInfo[j].vertex_BR.uv2.x = 1f;
							break;
						case TextureMappingOptions.Line:
							if (m_textAlignment != TextAlignmentOptions.Justified)
							{
								characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num54;
								characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num54;
								characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num54;
								characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num54;
							}
							else
							{
								characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
								characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
								characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
								characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
							}
							break;
						case TextureMappingOptions.Paragraph:
							characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
							characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
							characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
							characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num54;
							break;
						case TextureMappingOptions.MatchAspect:
						{
							switch (m_verticalMapping)
							{
							case TextureMappingOptions.Character:
								characterInfo[j].vertex_BL.uv2.y = 0f;
								characterInfo[j].vertex_TL.uv2.y = 1f;
								characterInfo[j].vertex_TR.uv2.y = 0f;
								characterInfo[j].vertex_BR.uv2.y = 1f;
								break;
							case TextureMappingOptions.Line:
								characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num54;
								characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num54;
								characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
								characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num54;
								characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num54;
								characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
								characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
								break;
							case TextureMappingOptions.MatchAspect:
								Debug.Log((object)"ERROR: Cannot Match both Vertical & Horizontal.");
								break;
							}
							float num55 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
							characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num55 + num54;
							characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
							characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num55 + num54;
							characterInfo[j].vertex_BR.uv2.x = characterInfo[j].vertex_TR.uv2.x;
							break;
						}
						}
						switch (m_verticalMapping)
						{
						case TextureMappingOptions.Character:
							characterInfo[j].vertex_BL.uv2.y = 0f;
							characterInfo[j].vertex_TL.uv2.y = 1f;
							characterInfo[j].vertex_TR.uv2.y = 1f;
							characterInfo[j].vertex_BR.uv2.y = 0f;
							break;
						case TextureMappingOptions.Line:
							characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
							characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - tMP_LineInfo.descender) / (tMP_LineInfo.ascender - tMP_LineInfo.descender);
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							break;
						case TextureMappingOptions.Paragraph:
							characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
							characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y);
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							break;
						case TextureMappingOptions.MatchAspect:
						{
							float num56 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
							characterInfo[j].vertex_BL.uv2.y = num56 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
							characterInfo[j].vertex_TL.uv2.y = num56 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							break;
						}
						}
						num42 = characterInfo[j].scale * (1f - m_charWidthAdjDelta);
						if (!characterInfo[j].isUsingAlternateTypeface && (characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
						{
							num42 *= -1f;
						}
						switch ((int)val13)
						{
						case 0:
							num42 *= num41 / scaleFactor;
							break;
						case 1:
							num42 *= (flag12 ? num41 : 1f);
							break;
						case 2:
							num42 *= num41;
							break;
						}
						float x = characterInfo[j].vertex_BL.uv2.x;
						float y = characterInfo[j].vertex_BL.uv2.y;
						float x2 = characterInfo[j].vertex_TR.uv2.x;
						float y2 = characterInfo[j].vertex_TR.uv2.y;
						float num57 = (int)x;
						float num58 = (int)y;
						x -= num57;
						x2 -= num57;
						y -= num58;
						y2 -= num58;
						characterInfo[j].vertex_BL.uv2.x = PackUV(x, y);
						characterInfo[j].vertex_BL.uv2.y = num42;
						characterInfo[j].vertex_TL.uv2.x = PackUV(x, y2);
						characterInfo[j].vertex_TL.uv2.y = num42;
						characterInfo[j].vertex_TR.uv2.x = PackUV(x2, y2);
						characterInfo[j].vertex_TR.uv2.y = num42;
						characterInfo[j].vertex_BR.uv2.x = PackUV(x2, y);
						characterInfo[j].vertex_BR.uv2.y = num42;
						break;
					}
					}
					if (j < m_maxVisibleCharacters && num37 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode != TextOverflowModes.Page)
					{
						ref Vector3 position = ref characterInfo[j].vertex_BL.position;
						position += zero5;
						ref Vector3 position2 = ref characterInfo[j].vertex_TL.position;
						position2 += zero5;
						ref Vector3 position3 = ref characterInfo[j].vertex_TR.position;
						position3 += zero5;
						ref Vector3 position4 = ref characterInfo[j].vertex_BR.position;
						position4 += zero5;
					}
					else if (j < m_maxVisibleCharacters && num37 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode == TextOverflowModes.Page && characterInfo[j].pageNumber == num10)
					{
						ref Vector3 position5 = ref characterInfo[j].vertex_BL.position;
						position5 += zero5;
						ref Vector3 position6 = ref characterInfo[j].vertex_TL.position;
						position6 += zero5;
						ref Vector3 position7 = ref characterInfo[j].vertex_TR.position;
						position7 += zero5;
						ref Vector3 position8 = ref characterInfo[j].vertex_BR.position;
						position8 += zero5;
					}
					else
					{
						characterInfo[j].vertex_BL.position = Vector3.get_zero();
						characterInfo[j].vertex_TL.position = Vector3.get_zero();
						characterInfo[j].vertex_TR.position = Vector3.get_zero();
						characterInfo[j].vertex_BR.position = Vector3.get_zero();
						characterInfo[j].isVisible = false;
					}
					switch (elementType)
					{
					case TMP_TextElementType.Character:
						FillCharacterVertexBuffers(j, index_X);
						break;
					case TMP_TextElementType.Sprite:
						FillSpriteVertexBuffers(j, index_X2);
						break;
					}
				}
				ref Vector3 bottomLeft = ref m_textInfo.characterInfo[j].bottomLeft;
				bottomLeft += zero5;
				ref Vector3 topLeft = ref m_textInfo.characterInfo[j].topLeft;
				topLeft += zero5;
				ref Vector3 topRight = ref m_textInfo.characterInfo[j].topRight;
				topRight += zero5;
				ref Vector3 bottomRight = ref m_textInfo.characterInfo[j].bottomRight;
				bottomRight += zero5;
				m_textInfo.characterInfo[j].origin += zero5.x;
				m_textInfo.characterInfo[j].xAdvance += zero5.x;
				m_textInfo.characterInfo[j].ascender += zero5.y;
				m_textInfo.characterInfo[j].descender += zero5.y;
				m_textInfo.characterInfo[j].baseLine += zero5.y;
				if (lineNumber != num38 || j == m_characterCount - 1)
				{
					if (lineNumber != num38)
					{
						m_textInfo.lineInfo[num38].baseline += zero5.y;
						m_textInfo.lineInfo[num38].ascender += zero5.y;
						m_textInfo.lineInfo[num38].descender += zero5.y;
						m_textInfo.lineInfo[num38].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num38].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[num38].descender);
						m_textInfo.lineInfo[num38].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num38].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[num38].ascender);
					}
					if (j == m_characterCount - 1)
					{
						m_textInfo.lineInfo[lineNumber].baseline += zero5.y;
						m_textInfo.lineInfo[lineNumber].ascender += zero5.y;
						m_textInfo.lineInfo[lineNumber].descender += zero5.y;
						m_textInfo.lineInfo[lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[lineNumber].descender);
						m_textInfo.lineInfo[lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[lineNumber].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[lineNumber].ascender);
					}
				}
				if (char.IsLetterOrDigit(character2) || character2 == '-' || character2 == '­' || character2 == '‐' || character2 == '‑')
				{
					if (!flag11)
					{
						flag11 = true;
						num39 = j;
					}
					if (flag11 && j == m_characterCount - 1)
					{
						int num59 = m_textInfo.wordInfo.Length;
						int wordCount = m_textInfo.wordCount;
						if (m_textInfo.wordCount + 1 > num59)
						{
							TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num59 + 1);
						}
						num40 = j;
						m_textInfo.wordInfo[wordCount].firstCharacterIndex = num39;
						m_textInfo.wordInfo[wordCount].lastCharacterIndex = num40;
						m_textInfo.wordInfo[wordCount].characterCount = num40 - num39 + 1;
						m_textInfo.wordInfo[wordCount].textComponent = this;
						num37++;
						m_textInfo.wordCount++;
						m_textInfo.lineInfo[lineNumber].wordCount++;
					}
				}
				else if ((flag11 || (j == 0 && (!char.IsPunctuation(character2) || char.IsWhiteSpace(character2) || character2 == '\u200b' || j == m_characterCount - 1))) && (j <= 0 || j >= characterInfo.Length - 1 || j >= m_characterCount || (character2 != '\'' && character2 != '’') || !char.IsLetterOrDigit(characterInfo[j - 1].character) || !char.IsLetterOrDigit(characterInfo[j + 1].character)))
				{
					num40 = ((j == m_characterCount - 1 && char.IsLetterOrDigit(character2)) ? j : (j - 1));
					flag11 = false;
					int num60 = m_textInfo.wordInfo.Length;
					int wordCount2 = m_textInfo.wordCount;
					if (m_textInfo.wordCount + 1 > num60)
					{
						TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num60 + 1);
					}
					m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num39;
					m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num40;
					m_textInfo.wordInfo[wordCount2].characterCount = num40 - num39 + 1;
					m_textInfo.wordInfo[wordCount2].textComponent = this;
					num37++;
					m_textInfo.wordCount++;
					m_textInfo.lineInfo[lineNumber].wordCount++;
				}
				if ((m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
				{
					bool flag14 = true;
					int pageNumber = m_textInfo.characterInfo[j].pageNumber;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != m_pageToDisplay))
					{
						flag14 = false;
					}
					if (!char.IsWhiteSpace(character2) && character2 != '\u200b')
					{
						num45 = Mathf.Max(num45, m_textInfo.characterInfo[j].scale);
						num46 = Mathf.Min((pageNumber == num47) ? num46 : TMP_Text.k_LargePositiveFloat, m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num45);
						num47 = pageNumber;
					}
					if (!flag && flag14 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag = true;
						num43 = m_textInfo.characterInfo[j].scale;
						if (num45 == 0f)
						{
							num45 = num43;
						}
						((Vector3)(ref zero))._002Ector(m_textInfo.characterInfo[j].bottomLeft.x, num46, 0f);
						val14 = m_textInfo.characterInfo[j].underlineColor;
					}
					if (flag && m_characterCount == 1)
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num46, 0f);
						num44 = m_textInfo.characterInfo[j].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num43, num44, num45, num42, val14);
						num45 = 0f;
						num46 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex))
					{
						if (char.IsWhiteSpace(character2) || character2 == '\u200b')
						{
							int lastVisibleCharacterIndex = tMP_LineInfo.lastVisibleCharacterIndex;
							((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num46, 0f);
							num44 = m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
						}
						else
						{
							((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num46, 0f);
							num44 = m_textInfo.characterInfo[j].scale;
						}
						flag = false;
						DrawUnderlineMesh(zero, zero2, ref index, num43, num44, num45, num42, val14);
						num45 = 0f;
						num46 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && !flag14)
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, num46, 0f);
						num44 = m_textInfo.characterInfo[j - 1].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num43, num44, num45, num42, val14);
						num45 = 0f;
						num46 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && j < m_characterCount - 1 && !val14.Compare(m_textInfo.characterInfo[j + 1].underlineColor))
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num46, 0f);
						num44 = m_textInfo.characterInfo[j].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num43, num44, num45, num42, val14);
						num45 = 0f;
						num46 = TMP_Text.k_LargePositiveFloat;
					}
				}
				else if (flag)
				{
					flag = false;
					((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, num46, 0f);
					num44 = m_textInfo.characterInfo[j - 1].scale;
					DrawUnderlineMesh(zero, zero2, ref index, num43, num44, num45, num42, val14);
					num45 = 0f;
					num46 = TMP_Text.k_LargePositiveFloat;
				}
				bool num61 = (m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
				float strikethrough = fontAsset.fontInfo.strikethrough;
				if (num61)
				{
					bool flag15 = true;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && m_textInfo.characterInfo[j].pageNumber + 1 != m_pageToDisplay))
					{
						flag15 = false;
					}
					if (!flag2 && flag15 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag2 = true;
						num48 = m_textInfo.characterInfo[j].pointSize;
						num49 = m_textInfo.characterInfo[j].scale;
						((Vector3)(ref zero3))._002Ector(m_textInfo.characterInfo[j].bottomLeft.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num49, 0f);
						underlineColor = m_textInfo.characterInfo[j].strikethroughColor;
						b = m_textInfo.characterInfo[j].baseLine;
					}
					if (flag2 && m_characterCount == 1)
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num49, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
					}
					else if (flag2 && j == tMP_LineInfo.lastCharacterIndex)
					{
						if (char.IsWhiteSpace(character2) || character2 == '\u200b')
						{
							int lastVisibleCharacterIndex2 = tMP_LineInfo.lastVisibleCharacterIndex;
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num49, 0f);
						}
						else
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num49, 0f);
						}
						flag2 = false;
						DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
					}
					else if (flag2 && j < m_characterCount && (m_textInfo.characterInfo[j + 1].pointSize != num48 || !TMP_Math.Approximately(m_textInfo.characterInfo[j + 1].baseLine + zero5.y, b)))
					{
						flag2 = false;
						int lastVisibleCharacterIndex3 = tMP_LineInfo.lastVisibleCharacterIndex;
						if (j > lastVisibleCharacterIndex3)
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num49, 0f);
						}
						else
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num49, 0f);
						}
						DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
					}
					else if (flag2 && j < m_characterCount && ((Object)fontAsset).GetInstanceID() != ((Object)characterInfo[j + 1].fontAsset).GetInstanceID())
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num49, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
					}
					else if (flag2 && !flag15)
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num49, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
					}
				}
				else if (flag2)
				{
					flag2 = false;
					((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num49, 0f);
					DrawUnderlineMesh(zero3, zero4, ref index, num49, num49, num49, num42, underlineColor);
				}
				if ((m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
				{
					bool flag16 = true;
					int pageNumber2 = m_textInfo.characterInfo[j].pageNumber;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber2 + 1 != m_pageToDisplay))
					{
						flag16 = false;
					}
					if (!flag3 && flag16 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag3 = true;
						val = Vector2.op_Implicit(TMP_Text.k_LargePositiveVector2);
						val2 = Vector2.op_Implicit(TMP_Text.k_LargeNegativeVector2);
						highlightColor = m_textInfo.characterInfo[j].highlightColor;
					}
					if (flag3)
					{
						Color32 highlightColor2 = m_textInfo.characterInfo[j].highlightColor;
						bool flag17 = false;
						if (!highlightColor.Compare(highlightColor2))
						{
							val2.x = (val2.x + m_textInfo.characterInfo[j].bottomLeft.x) / 2f;
							val.y = Mathf.Min(val.y, m_textInfo.characterInfo[j].descender);
							val2.y = Mathf.Max(val2.y, m_textInfo.characterInfo[j].ascender);
							DrawTextHighlight(val, val2, ref index, highlightColor);
							flag3 = true;
							val = val2;
							((Vector3)(ref val2))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].descender, 0f);
							highlightColor = m_textInfo.characterInfo[j].highlightColor;
							flag17 = true;
						}
						if (!flag17)
						{
							val.x = Mathf.Min(val.x, m_textInfo.characterInfo[j].bottomLeft.x);
							val.y = Mathf.Min(val.y, m_textInfo.characterInfo[j].descender);
							val2.x = Mathf.Max(val2.x, m_textInfo.characterInfo[j].topRight.x);
							val2.y = Mathf.Max(val2.y, m_textInfo.characterInfo[j].ascender);
						}
					}
					if (flag3 && m_characterCount == 1)
					{
						flag3 = false;
						DrawTextHighlight(val, val2, ref index, highlightColor);
					}
					else if (flag3 && (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex))
					{
						flag3 = false;
						DrawTextHighlight(val, val2, ref index, highlightColor);
					}
					else if (flag3 && !flag16)
					{
						flag3 = false;
						DrawTextHighlight(val, val2, ref index, highlightColor);
					}
				}
				else if (flag3)
				{
					flag3 = false;
					DrawTextHighlight(val, val2, ref index, highlightColor);
				}
				num38 = lineNumber;
			}
			m_textInfo.characterCount = m_characterCount;
			m_textInfo.spriteCount = m_spriteCount;
			m_textInfo.lineCount = lineCount;
			m_textInfo.wordCount = ((num37 == 0 || m_characterCount <= 0) ? 1 : num37);
			m_textInfo.pageCount = m_pageNumber + 1;
			if (m_renderMode == TextRenderFlags.Render)
			{
				if ((int)m_canvas.get_additionalShaderChannels() != 25)
				{
					Canvas canvas = m_canvas;
					canvas.set_additionalShaderChannels((AdditionalCanvasShaderChannels)(canvas.get_additionalShaderChannels() | 0x19));
				}
				if (m_geometrySortingOrder != 0)
				{
					m_textInfo.meshInfo[0].SortGeometry(VertexSortingOrder.Reverse);
				}
				m_mesh.MarkDynamic();
				m_mesh.set_vertices(m_textInfo.meshInfo[0].vertices);
				m_mesh.set_uv(m_textInfo.meshInfo[0].uvs0);
				m_mesh.set_uv2(m_textInfo.meshInfo[0].uvs2);
				m_mesh.set_colors32(m_textInfo.meshInfo[0].colors32);
				m_mesh.RecalculateBounds();
				m_canvasRenderer.SetMesh(m_mesh);
				Color val15 = m_canvasRenderer.GetColor();
				for (int k = 1; k < m_textInfo.materialCount; k++)
				{
					m_textInfo.meshInfo[k].ClearUnusedVertices();
					if (!((Object)(object)m_subTextObjects[k] == (Object)null))
					{
						if (m_geometrySortingOrder != 0)
						{
							m_textInfo.meshInfo[k].SortGeometry(VertexSortingOrder.Reverse);
						}
						m_subTextObjects[k].mesh.set_vertices(m_textInfo.meshInfo[k].vertices);
						m_subTextObjects[k].mesh.set_uv(m_textInfo.meshInfo[k].uvs0);
						m_subTextObjects[k].mesh.set_uv2(m_textInfo.meshInfo[k].uvs2);
						m_subTextObjects[k].mesh.set_colors32(m_textInfo.meshInfo[k].colors32);
						m_subTextObjects[k].mesh.RecalculateBounds();
						m_subTextObjects[k].canvasRenderer.SetMesh(m_subTextObjects[k].mesh);
						m_subTextObjects[k].canvasRenderer.SetColor(val15);
					}
				}
			}
			TMPro_EventManager.ON_TEXT_CHANGED((Object)(object)this);
		}

		protected override Vector3[] GetTextContainerLocalCorners()
		{
			if ((Object)(object)m_rectTransform == (Object)null)
			{
				m_rectTransform = base.rectTransform;
			}
			m_rectTransform.GetLocalCorners(m_RectTransformCorners);
			return m_RectTransformCorners;
		}

		protected override void SetActiveSubMeshes(bool state)
		{
			for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
			{
				if (((Behaviour)m_subTextObjects[i]).get_enabled() != state)
				{
					((Behaviour)m_subTextObjects[i]).set_enabled(state);
				}
			}
		}

		protected override Bounds GetCompoundBounds()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_001b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_006a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0080: Unknown result type (might be due to invalid IL or missing references)
			//IL_008c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_011b: Unknown result type (might be due to invalid IL or missing references)
			//IL_011c: Unknown result type (might be due to invalid IL or missing references)
			//IL_011d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_012c: Unknown result type (might be due to invalid IL or missing references)
			//IL_012d: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0133: Unknown result type (might be due to invalid IL or missing references)
			//IL_0138: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			//IL_013a: Unknown result type (might be due to invalid IL or missing references)
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			Bounds val = m_mesh.get_bounds();
			Vector3 min = ((Bounds)(ref val)).get_min();
			Vector3 max = ((Bounds)(ref val)).get_max();
			for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
			{
				Bounds val2 = m_subTextObjects[i].mesh.get_bounds();
				min.x = ((min.x < ((Bounds)(ref val2)).get_min().x) ? min.x : ((Bounds)(ref val2)).get_min().x);
				min.y = ((min.y < ((Bounds)(ref val2)).get_min().y) ? min.y : ((Bounds)(ref val2)).get_min().y);
				max.x = ((max.x > ((Bounds)(ref val2)).get_max().x) ? max.x : ((Bounds)(ref val2)).get_max().x);
				max.y = ((max.y > ((Bounds)(ref val2)).get_max().y) ? max.y : ((Bounds)(ref val2)).get_max().y);
			}
			Vector3 val3 = (min + max) / 2f;
			Vector2 val4 = Vector2.op_Implicit(max - min);
			return new Bounds(val3, Vector2.op_Implicit(val4));
		}

		private void UpdateSDFScale(float lossyScale)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Invalid comparison between Unknown and I4
			if ((Object)(object)m_canvas == (Object)null)
			{
				m_canvas = GetCanvas();
				if ((Object)(object)m_canvas == (Object)null)
				{
					return;
				}
			}
			lossyScale = ((lossyScale == 0f) ? 1f : lossyScale);
			float num = 0f;
			float scaleFactor = m_canvas.get_scaleFactor();
			num = (((int)m_canvas.get_renderMode() == 0) ? (lossyScale / scaleFactor) : (((int)m_canvas.get_renderMode() != 1) ? lossyScale : (((Object)(object)m_canvas.get_worldCamera() != (Object)null) ? lossyScale : 1f)));
			for (int i = 0; i < m_textInfo.characterCount; i++)
			{
				if (m_textInfo.characterInfo[i].isVisible && m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
				{
					float num2 = num * m_textInfo.characterInfo[i].scale * (1f - m_charWidthAdjDelta);
					if (!m_textInfo.characterInfo[i].isUsingAlternateTypeface && (m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num2 *= -1f;
					}
					int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = m_textInfo.characterInfo[i].vertexIndex;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num2;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num2;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num2;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num2;
				}
			}
			for (int j = 0; j < m_textInfo.materialCount; j++)
			{
				if (j == 0)
				{
					m_mesh.set_uv2(m_textInfo.meshInfo[0].uvs2);
					m_canvasRenderer.SetMesh(m_mesh);
				}
				else
				{
					m_subTextObjects[j].mesh.set_uv2(m_textInfo.meshInfo[j].uvs2);
					m_subTextObjects[j].canvasRenderer.SetMesh(m_subTextObjects[j].mesh);
				}
			}
		}

		protected override void AdjustLineOffset(int startIndex, int endIndex, float offset)
		{
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_0036: Unknown result type (might be due to invalid IL or missing references)
			//IL_003b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0062: Unknown result type (might be due to invalid IL or missing references)
			//IL_007e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0089: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_010d: Unknown result type (might be due to invalid IL or missing references)
			//IL_014c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0151: Unknown result type (might be due to invalid IL or missing references)
			//IL_0152: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0178: Unknown result type (might be due to invalid IL or missing references)
			//IL_017d: Unknown result type (might be due to invalid IL or missing references)
			//IL_017e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0183: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_01db: Unknown result type (might be due to invalid IL or missing references)
			Vector3 val = default(Vector3);
			((Vector3)(ref val))._002Ector(0f, offset, 0f);
			for (int i = startIndex; i <= endIndex; i++)
			{
				ref Vector3 bottomLeft = ref m_textInfo.characterInfo[i].bottomLeft;
				bottomLeft -= val;
				ref Vector3 topLeft = ref m_textInfo.characterInfo[i].topLeft;
				topLeft -= val;
				ref Vector3 topRight = ref m_textInfo.characterInfo[i].topRight;
				topRight -= val;
				ref Vector3 bottomRight = ref m_textInfo.characterInfo[i].bottomRight;
				bottomRight -= val;
				m_textInfo.characterInfo[i].ascender -= val.y;
				m_textInfo.characterInfo[i].baseLine -= val.y;
				m_textInfo.characterInfo[i].descender -= val.y;
				if (m_textInfo.characterInfo[i].isVisible)
				{
					ref Vector3 position = ref m_textInfo.characterInfo[i].vertex_BL.position;
					position -= val;
					ref Vector3 position2 = ref m_textInfo.characterInfo[i].vertex_TL.position;
					position2 -= val;
					ref Vector3 position3 = ref m_textInfo.characterInfo[i].vertex_TR.position;
					position3 -= val;
					ref Vector3 position4 = ref m_textInfo.characterInfo[i].vertex_BR.position;
					position4 -= val;
				}
			}
		}
	}
}

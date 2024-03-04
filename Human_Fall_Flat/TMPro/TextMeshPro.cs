using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	[AddComponentMenu("Mesh/TextMeshPro - Text")]
	[SelectionBase]
	public class TextMeshPro : TMP_Text, ILayoutElement
	{
		private bool m_currentAutoSizeMode;

		[SerializeField]
		private bool m_hasFontAssetChanged;

		private float m_previousLossyScaleY = -1f;

		[SerializeField]
		private Renderer m_renderer;

		private MeshFilter m_meshFilter;

		private bool m_isFirstAllocation;

		private int m_max_characters = 8;

		private int m_max_numberOfLines = 4;

		private Bounds m_default_bounds = new Bounds(Vector3.get_zero(), new Vector3(1000f, 1000f, 0f));

		[SerializeField]
		protected TMP_SubMesh[] m_subTextObjects = new TMP_SubMesh[8];

		private bool m_isMaskingEnabled;

		private bool isMaskUpdateRequired;

		[SerializeField]
		private MaskingTypes m_maskType;

		private Matrix4x4 m_EnvMapMatrix;

		private Vector3[] m_RectTransformCorners = (Vector3[])(object)new Vector3[4];

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private int loopCountA;

		public int sortingLayerID
		{
			get
			{
				return m_renderer.get_sortingLayerID();
			}
			set
			{
				m_renderer.set_sortingLayerID(value);
			}
		}

		public int sortingOrder
		{
			get
			{
				return m_renderer.get_sortingOrder();
			}
			set
			{
				m_renderer.set_sortingOrder(value);
			}
		}

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
						TMP_UpdateManager.RegisterTextElementForLayoutRebuild(this);
						((Graphic)this).SetLayoutDirty();
					}
				}
			}
		}

		[Obsolete("The TextContainer is now obsolete. Use the RectTransform instead.")]
		public TextContainer textContainer => null;

		public new Transform transform
		{
			get
			{
				if ((Object)(object)m_transform == (Object)null)
				{
					m_transform = ((Component)this).GetComponent<Transform>();
				}
				return m_transform;
			}
		}

		public Renderer renderer
		{
			get
			{
				if ((Object)(object)m_renderer == (Object)null)
				{
					m_renderer = ((Component)this).GetComponent<Renderer>();
				}
				return m_renderer;
			}
		}

		public override Mesh mesh
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				if ((Object)(object)m_mesh == (Object)null)
				{
					m_mesh = new Mesh();
					((Object)m_mesh).set_hideFlags((HideFlags)61);
					meshFilter.set_mesh(m_mesh);
				}
				return m_mesh;
			}
		}

		public MeshFilter meshFilter
		{
			get
			{
				if ((Object)(object)m_meshFilter == (Object)null)
				{
					m_meshFilter = ((Component)this).GetComponent<MeshFilter>();
				}
				return m_meshFilter;
			}
		}

		public MaskingTypes maskType
		{
			get
			{
				return m_maskType;
			}
			set
			{
				m_maskType = value;
				SetMask(m_maskType);
			}
		}

		public void SetMask(MaskingTypes type, Vector4 maskCoords)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			SetMask(type);
			SetMaskCoordinates(maskCoords);
		}

		public void SetMask(MaskingTypes type, Vector4 maskCoords, float softnessX, float softnessY)
		{
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			SetMask(type);
			SetMaskCoordinates(maskCoords, softnessX, softnessY);
		}

		public override void SetVerticesDirty()
		{
			if (!m_verticesAlreadyDirty && !((Object)(object)this == (Object)null) && ((UIBehaviour)this).IsActive())
			{
				TMP_UpdateManager.RegisterTextElementForGraphicRebuild(this);
				m_verticesAlreadyDirty = true;
			}
		}

		public override void SetLayoutDirty()
		{
			m_isPreferredWidthDirty = true;
			m_isPreferredHeightDirty = true;
			if (!m_layoutAlreadyDirty && !((Object)(object)this == (Object)null) && ((UIBehaviour)this).IsActive())
			{
				m_layoutAlreadyDirty = true;
				m_isLayoutDirty = true;
			}
		}

		public override void SetMaterialDirty()
		{
			((Graphic)this).UpdateMaterial();
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
				OnPreRenderObject();
				m_verticesAlreadyDirty = false;
				m_layoutAlreadyDirty = false;
				if (m_isMaterialDirty)
				{
					((Graphic)this).UpdateMaterial();
					m_isMaterialDirty = false;
				}
			}
		}

		protected override void UpdateMaterial()
		{
			if (!((Object)(object)m_sharedMaterial == (Object)null))
			{
				if ((Object)(object)m_renderer == (Object)null)
				{
					m_renderer = renderer;
				}
				if (((Object)m_renderer.get_sharedMaterial()).GetInstanceID() != ((Object)m_sharedMaterial).GetInstanceID())
				{
					m_renderer.set_sharedMaterial(m_sharedMaterial);
				}
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

		public override void ForceMeshUpdate()
		{
			m_havePropertiesChanged = true;
			OnPreRenderObject();
		}

		public override void ForceMeshUpdate(bool ignoreInactive)
		{
			m_havePropertiesChanged = true;
			m_ignoreActiveState = true;
			OnPreRenderObject();
		}

		public override TMP_TextInfo GetTextInfo(string text)
		{
			StringToCharArray(text, ref m_char_buffer);
			SetArraySizes(m_char_buffer);
			m_renderMode = TextRenderFlags.DontRender;
			ComputeMarginSize();
			GenerateTextMesh();
			m_renderMode = TextRenderFlags.Render;
			return base.textInfo;
		}

		public override void ClearMesh(bool updateMesh)
		{
			if ((Object)(object)m_textInfo.meshInfo[0].mesh == (Object)null)
			{
				m_textInfo.meshInfo[0].mesh = m_mesh;
			}
			m_textInfo.ClearMeshInfo(updateMesh);
		}

		public override void UpdateGeometry(Mesh mesh, int index)
		{
			mesh.RecalculateBounds();
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
			}
		}

		public void UpdateFontAsset()
		{
			LoadFontAsset();
		}

		public void CalculateLayoutInputHorizontal()
		{
			if (!((Component)this).get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			m_currentAutoSizeMode = m_enableAutoSizing;
			if (m_isCalculateSizeRequired || ((Transform)m_rectTransform).get_hasChanged())
			{
				m_minWidth = 0f;
				m_flexibleWidth = 0f;
				if (m_enableAutoSizing)
				{
					m_fontSize = m_fontSizeMax;
				}
				m_marginWidth = TMP_Text.k_LargePositiveFloat;
				m_marginHeight = TMP_Text.k_LargePositiveFloat;
				if (m_isInputParsingRequired || m_isTextTruncated)
				{
					ParseInputText();
				}
				GenerateTextMesh();
				m_renderMode = TextRenderFlags.Render;
				ComputeMarginSize();
				m_isLayoutDirty = true;
			}
		}

		public void CalculateLayoutInputVertical()
		{
			if (!((Component)this).get_gameObject().get_activeInHierarchy())
			{
				return;
			}
			if (m_isCalculateSizeRequired || ((Transform)m_rectTransform).get_hasChanged())
			{
				m_minHeight = 0f;
				m_flexibleHeight = 0f;
				if (m_enableAutoSizing)
				{
					m_currentAutoSizeMode = true;
					m_enableAutoSizing = false;
				}
				m_marginHeight = TMP_Text.k_LargePositiveFloat;
				GenerateTextMesh();
				m_enableAutoSizing = m_currentAutoSizeMode;
				m_renderMode = TextRenderFlags.Render;
				ComputeMarginSize();
				m_isLayoutDirty = true;
			}
			m_isCalculateSizeRequired = false;
		}

		protected override void Awake()
		{
			//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b4: Expected O, but got Unknown
			m_renderer = ((Component)this).GetComponent<Renderer>();
			if ((Object)(object)m_renderer == (Object)null)
			{
				m_renderer = ((Component)this).get_gameObject().AddComponent<Renderer>();
			}
			if ((Object)(object)((Graphic)this).get_canvasRenderer() != (Object)null)
			{
				((Object)((Graphic)this).get_canvasRenderer()).set_hideFlags((HideFlags)2);
			}
			else
			{
				((Object)((Component)this).get_gameObject().AddComponent<CanvasRenderer>()).set_hideFlags((HideFlags)2);
			}
			m_rectTransform = base.rectTransform;
			m_transform = transform;
			m_meshFilter = ((Component)this).GetComponent<MeshFilter>();
			if ((Object)(object)m_meshFilter == (Object)null)
			{
				m_meshFilter = ((Component)this).get_gameObject().AddComponent<MeshFilter>();
			}
			if ((Object)(object)m_mesh == (Object)null)
			{
				m_mesh = new Mesh();
				((Object)m_mesh).set_hideFlags((HideFlags)61);
				m_meshFilter.set_mesh(m_mesh);
			}
			((Object)m_meshFilter).set_hideFlags((HideFlags)2);
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
				Debug.LogWarning((object)("Please assign a Font Asset to this " + ((Object)transform).get_name() + " gameobject."), (Object)(object)this);
				return;
			}
			TMP_SubMesh[] componentsInChildren = ((Component)this).GetComponentsInChildren<TMP_SubMesh>();
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
				TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.Add(ON_TEXTMESHPRO_PROPERTY_CHANGED);
				TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Add(ON_DRAG_AND_DROP_MATERIAL);
				TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Add(ON_TEXT_STYLE_CHANGED);
				TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Add(ON_COLOR_GRADIENT_CHANGED);
				TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Add(ON_TMP_SETTINGS_CHANGED);
				m_isRegisteredForEvents = true;
			}
			meshFilter.set_sharedMesh(mesh);
			SetActiveSubMeshes(state: true);
			ComputeMarginSize();
			m_isInputParsingRequired = true;
			m_havePropertiesChanged = true;
			m_verticesAlreadyDirty = false;
			((Graphic)this).SetVerticesDirty();
		}

		protected override void OnDisable()
		{
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
			m_meshFilter.set_sharedMesh((Mesh)null);
			SetActiveSubMeshes(state: false);
		}

		protected override void OnDestroy()
		{
			if ((Object)(object)m_mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_mesh);
			}
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Remove(ON_MATERIAL_PROPERTY_CHANGED);
			TMPro_EventManager.FONT_PROPERTY_EVENT.Remove(ON_FONT_PROPERTY_CHANGED);
			TMPro_EventManager.TEXTMESHPRO_PROPERTY_EVENT.Remove(ON_TEXTMESHPRO_PROPERTY_CHANGED);
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Remove(ON_DRAG_AND_DROP_MATERIAL);
			TMPro_EventManager.TEXT_STYLE_PROPERTY_EVENT.Remove(ON_TEXT_STYLE_CHANGED);
			TMPro_EventManager.COLOR_GRADIENT_PROPERTY_EVENT.Remove(ON_COLOR_GRADIENT_CHANGED);
			TMPro_EventManager.TMP_SETTINGS_PROPERTY_EVENT.Remove(ON_TMP_SETTINGS_CHANGED);
			m_isRegisteredForEvents = false;
			TMP_UpdateManager.UnRegisterTextElementForRebuild(this);
		}

		protected override void Reset()
		{
			if ((Object)(object)m_mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_mesh);
			}
			((UIBehaviour)this).Awake();
		}

		protected override void OnValidate()
		{
			if ((Object)(object)m_fontAsset == (Object)null || m_hasFontAssetChanged)
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
			if ((Object)(object)m_renderer.get_sharedMaterial() == (Object)null)
			{
				if ((Object)(object)m_fontAsset != (Object)null)
				{
					m_renderer.set_sharedMaterial(m_fontAsset.material);
					Debug.LogWarning((object)("No Material was assigned to " + ((Object)this).get_name() + ". " + ((Object)m_fontAsset.material).get_name() + " was assigned."), (Object)(object)this);
				}
				else
				{
					Debug.LogWarning((object)("No Font Asset assigned to " + ((Object)this).get_name() + ". Please assign a Font Asset."), (Object)(object)this);
				}
			}
			if (((Object)m_fontAsset.atlas).GetInstanceID() != ((Object)m_renderer.get_sharedMaterial().GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
			{
				m_renderer.set_sharedMaterial(m_sharedMaterial);
				Debug.LogWarning((object)"Font Asset Atlas doesn't match the Atlas in the newly assigned material. Select a matching material or a different font asset.", (Object)(object)this);
			}
			if ((Object)(object)m_renderer.get_sharedMaterial() != (Object)(object)m_sharedMaterial)
			{
				m_sharedMaterial = m_renderer.get_sharedMaterial();
			}
			m_padding = GetPaddingForMaterial();
			UpdateMask();
			UpdateEnvMapMatrix();
			m_havePropertiesChanged = true;
			((Graphic)this).SetVerticesDirty();
		}

		private void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
		{
			if (MaterialReference.Contains(m_materialReferences, font))
			{
				m_isInputParsingRequired = true;
				m_havePropertiesChanged = true;
				((Graphic)this).SetMaterialDirty();
				((Graphic)this).SetVerticesDirty();
			}
		}

		private void ON_TEXTMESHPRO_PROPERTY_CHANGED(bool isChanged, TextMeshPro obj)
		{
			if ((Object)(object)obj == (Object)(object)this)
			{
				m_havePropertiesChanged = true;
				m_isInputParsingRequired = true;
				m_padding = GetPaddingForMaterial();
				((Graphic)this).SetVerticesDirty();
			}
		}

		private void ON_DRAG_AND_DROP_MATERIAL(GameObject obj, Material currentMaterial, Material newMaterial)
		{
			if ((Object)(object)obj == (Object)(object)((Component)this).get_gameObject() || PrefabUtility.GetPrefabParent((Object)(object)((Component)this).get_gameObject()) == (Object)(object)obj)
			{
				Undo.RecordObject((Object)(object)this, "Material Assignment");
				Undo.RecordObject((Object)(object)m_renderer, "Material Assignment");
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
				m_renderer.set_sharedMaterial(m_fontAsset.material);
				m_sharedMaterial = m_fontAsset.material;
				m_sharedMaterial.SetFloat("_CullMode", 0f);
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				m_renderer.set_receiveShadows(false);
				m_renderer.set_shadowCastingMode((ShadowCastingMode)0);
			}
			else
			{
				if (m_fontAsset.characterDictionary == null)
				{
					m_fontAsset.ReadFontDefinition();
				}
				if ((Object)(object)m_renderer.get_sharedMaterial() == (Object)null || (Object)(object)m_renderer.get_sharedMaterial().GetTexture(ShaderUtilities.ID_MainTex) == (Object)null || ((Object)m_fontAsset.atlas).GetInstanceID() != ((Object)m_renderer.get_sharedMaterial().GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
				{
					m_renderer.set_sharedMaterial(m_fontAsset.material);
					m_sharedMaterial = m_fontAsset.material;
				}
				else
				{
					m_sharedMaterial = m_renderer.get_sharedMaterial();
				}
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				if (m_sharedMaterial.get_passCount() == 1)
				{
					m_renderer.set_receiveShadows(false);
					m_renderer.set_shadowCastingMode((ShadowCastingMode)0);
				}
			}
			m_padding = GetPaddingForMaterial();
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			GetSpecialCharacters(m_fontAsset);
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

		private void SetMask(MaskingTypes maskType)
		{
			switch (maskType)
			{
			case MaskingTypes.MaskOff:
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				break;
			case MaskingTypes.MaskSoft:
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				break;
			case MaskingTypes.MaskHard:
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				break;
			}
		}

		private void SetMaskCoordinates(Vector4 coords)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
		}

		private void SetMaskCoordinates(Vector4 coords, float softX, float softY)
		{
			//IL_000b: Unknown result type (might be due to invalid IL or missing references)
			m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, coords);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessX, softX);
			m_sharedMaterial.SetFloat(ShaderUtilities.ID_MaskSoftnessY, softY);
		}

		private void EnableMasking()
		{
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				m_sharedMaterial.EnableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				m_isMaskingEnabled = true;
				UpdateMask();
			}
		}

		private void DisableMasking()
		{
			if (m_sharedMaterial.HasProperty(ShaderUtilities.ID_ClipRect))
			{
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_SOFT);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_HARD);
				m_sharedMaterial.DisableKeyword(ShaderUtilities.Keyword_MASK_TEX);
				m_isMaskingEnabled = false;
				UpdateMask();
			}
		}

		private void UpdateMask()
		{
			if (m_isMaskingEnabled && m_isMaskingEnabled && (Object)(object)m_fontMaterial == (Object)null)
			{
				CreateMaterialInstance();
			}
		}

		protected override Material GetMaterial(Material mat)
		{
			if ((Object)(object)m_fontMaterial == (Object)null || ((Object)m_fontMaterial).GetInstanceID() != ((Object)mat).GetInstanceID())
			{
				m_fontMaterial = CreateMaterialInstance(mat);
			}
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
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
					m_fontMaterials[i] = m_subTextObjects[i].material;
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
				Texture texture = materials[i].GetTexture(ShaderUtilities.ID_MainTex);
				if (i == 0)
				{
					if (!((Object)(object)texture == (Object)null) && ((Object)texture).GetInstanceID() == ((Object)m_sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID())
					{
						m_sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
						m_padding = GetPaddingForMaterial(m_sharedMaterial);
					}
				}
				else if (!((Object)(object)texture == (Object)null) && ((Object)texture).GetInstanceID() == ((Object)m_subTextObjects[i].sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex)).GetInstanceID() && m_subTextObjects[i].isDefaultMaterial)
				{
					m_subTextObjects[i].sharedMaterial = (m_fontSharedMaterials[i] = materials[i]);
				}
			}
		}

		protected override void SetOutlineThickness(float thickness)
		{
			thickness = Mathf.Clamp01(thickness);
			m_renderer.get_material().SetFloat(ShaderUtilities.ID_OutlineWidth, thickness);
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = m_renderer.get_material();
			}
			m_fontMaterial = m_renderer.get_material();
			m_sharedMaterial = m_fontMaterial;
			m_padding = GetPaddingForMaterial();
		}

		protected override void SetFaceColor(Color32 color)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			m_renderer.get_material().SetColor(ShaderUtilities.ID_FaceColor, Color32.op_Implicit(color));
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = m_renderer.get_material();
			}
			m_sharedMaterial = m_fontMaterial;
		}

		protected override void SetOutlineColor(Color32 color)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			m_renderer.get_material().SetColor(ShaderUtilities.ID_OutlineColor, Color32.op_Implicit(color));
			if ((Object)(object)m_fontMaterial == (Object)null)
			{
				m_fontMaterial = m_renderer.get_material();
			}
			m_sharedMaterial = m_fontMaterial;
		}

		private void CreateMaterialInstance()
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_000c: Expected O, but got Unknown
			Material val = new Material(m_sharedMaterial);
			val.set_shaderKeywords(m_sharedMaterial.get_shaderKeywords());
			((Object)val).set_name(((Object)val).get_name() + " Instance");
			m_fontMaterial = val;
		}

		protected override void SetShaderDepth()
		{
			if (m_isOverlay)
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 0f);
				m_renderer.get_material().set_renderQueue(4000);
				m_sharedMaterial = m_renderer.get_material();
			}
			else
			{
				m_sharedMaterial.SetFloat(ShaderUtilities.ShaderTag_ZTestMode, 4f);
				m_renderer.get_material().set_renderQueue(-1);
				m_sharedMaterial = m_renderer.get_material();
			}
		}

		protected override void SetCulling()
		{
			if (m_isCullingEnabled)
			{
				m_renderer.get_material().SetFloat("_CullMode", 2f);
				for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
				{
					Renderer val = m_subTextObjects[i].renderer;
					if ((Object)(object)val != (Object)null)
					{
						val.get_material().SetFloat(ShaderUtilities.ShaderTag_CullMode, 2f);
					}
				}
				return;
			}
			m_renderer.get_material().SetFloat("_CullMode", 0f);
			for (int j = 1; j < m_subTextObjects.Length && (Object)(object)m_subTextObjects[j] != (Object)null; j++)
			{
				Renderer val2 = m_subTextObjects[j].renderer;
				if ((Object)(object)val2 != (Object)null)
				{
					val2.get_material().SetFloat(ShaderUtilities.ShaderTag_CullMode, 0f);
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
			if ((Object)(object)m_sharedMaterial == (Object)null)
			{
				return 0f;
			}
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, m_enableExtraPadding, m_isUsingBold);
			m_isMaskingEnabled = ShaderUtilities.IsMaskingEnabled(m_sharedMaterial);
			m_isSDFShader = m_sharedMaterial.HasProperty(ShaderUtilities.ID_WeightNormal);
			return m_padding;
		}

		protected override int SetArraySizes(int[] chars)
		{
			//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
			//IL_08cd: Expected O, but got Unknown
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
						m_subTextObjects[j] = TMP_SubMesh.AddSubTextObject(this, m_materialReferences[j]);
						m_textInfo.meshInfo[j].vertices = null;
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
				if (m_textInfo.meshInfo[j].vertices == null || m_textInfo.meshInfo[j].vertices.Length < referenceCount * ((!m_isVolumetricText) ? 4 : 8))
				{
					if (m_textInfo.meshInfo[j].vertices == null)
					{
						if (j == 0)
						{
							m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_mesh, referenceCount + 1, m_isVolumetricText);
						}
						else
						{
							m_textInfo.meshInfo[j] = new TMP_MeshInfo(m_subTextObjects[j].mesh, referenceCount + 1, m_isVolumetricText);
						}
					}
					else
					{
						m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.NextPowerOfTwo(referenceCount), m_isVolumetricText);
					}
				}
				else if (m_textInfo.meshInfo[j].vertices.Length - referenceCount * ((!m_isVolumetricText) ? 4 : 8) > 1024)
				{
					m_textInfo.meshInfo[j].ResizeMeshInfo((referenceCount > 1024) ? (referenceCount + 256) : Mathf.Max(Mathf.NextPowerOfTwo(referenceCount), 256), m_isVolumetricText);
				}
			}
			for (int k = num4; k < m_subTextObjects.Length && (Object)(object)m_subTextObjects[k] != (Object)null; k++)
			{
				if (k < m_textInfo.meshInfo.Length)
				{
					m_textInfo.meshInfo[k].ClearUnusedVertices(0, updateMesh: true);
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
			isMaskUpdateRequired = true;
			((Graphic)this).SetVerticesDirty();
		}

		protected override void OnTransformParentChanged()
		{
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
		}

		protected override void OnRectTransformDimensionsChange()
		{
			ComputeMarginSize();
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetLayoutDirty();
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
			}
			if (m_isUsingLegacyAnimationComponent)
			{
				m_havePropertiesChanged = true;
				OnPreRenderObject();
			}
		}

		private void OnPreRenderObject()
		{
			if (!m_isAwake || (!m_ignoreActiveState && !((UIBehaviour)this).IsActive()))
			{
				return;
			}
			loopCountA = 0;
			if (m_havePropertiesChanged || m_isLayoutDirty)
			{
				if (isMaskUpdateRequired)
				{
					UpdateMask();
					isMaskUpdateRequired = false;
				}
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
			//IL_0239: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0240: Unknown result type (might be due to invalid IL or missing references)
			//IL_0245: Unknown result type (might be due to invalid IL or missing references)
			//IL_024a: Unknown result type (might be due to invalid IL or missing references)
			//IL_024f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0251: Unknown result type (might be due to invalid IL or missing references)
			//IL_0256: Unknown result type (might be due to invalid IL or missing references)
			//IL_025b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0260: Unknown result type (might be due to invalid IL or missing references)
			//IL_0262: Unknown result type (might be due to invalid IL or missing references)
			//IL_0267: Unknown result type (might be due to invalid IL or missing references)
			//IL_026b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0270: Unknown result type (might be due to invalid IL or missing references)
			//IL_0275: Unknown result type (might be due to invalid IL or missing references)
			//IL_027c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0281: Unknown result type (might be due to invalid IL or missing references)
			//IL_0288: Unknown result type (might be due to invalid IL or missing references)
			//IL_028d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0294: Unknown result type (might be due to invalid IL or missing references)
			//IL_0299: Unknown result type (might be due to invalid IL or missing references)
			//IL_02a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_02c7: Unknown result type (might be due to invalid IL or missing references)
			//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0424: Unknown result type (might be due to invalid IL or missing references)
			//IL_0429: Unknown result type (might be due to invalid IL or missing references)
			//IL_047a: Unknown result type (might be due to invalid IL or missing references)
			//IL_047f: Unknown result type (might be due to invalid IL or missing references)
			//IL_048a: Unknown result type (might be due to invalid IL or missing references)
			//IL_048f: Unknown result type (might be due to invalid IL or missing references)
			//IL_075d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0762: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ac7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0acc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0ae8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0aed: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b09: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b0e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0dd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e06: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e41: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e5b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0e69: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f1e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f23: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f25: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f27: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f29: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f30: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f32: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f34: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f39: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f44: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f63: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f65: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f67: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f71: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f80: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f82: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f87: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f8e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f93: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0f9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fa9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fab: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fb8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fba: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fbc: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc1: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fc8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fcd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fd9: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fde: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe3: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fe5: Unknown result type (might be due to invalid IL or missing references)
			//IL_0fea: Unknown result type (might be due to invalid IL or missing references)
			//IL_1002: Unknown result type (might be due to invalid IL or missing references)
			//IL_1004: Unknown result type (might be due to invalid IL or missing references)
			//IL_101f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1021: Unknown result type (might be due to invalid IL or missing references)
			//IL_103c: Unknown result type (might be due to invalid IL or missing references)
			//IL_103e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1059: Unknown result type (might be due to invalid IL or missing references)
			//IL_105b: Unknown result type (might be due to invalid IL or missing references)
			//IL_10c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_10cc: Unknown result type (might be due to invalid IL or missing references)
			//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_10db: Unknown result type (might be due to invalid IL or missing references)
			//IL_181e: Unknown result type (might be due to invalid IL or missing references)
			//IL_1823: Unknown result type (might be due to invalid IL or missing references)
			//IL_1865: Unknown result type (might be due to invalid IL or missing references)
			//IL_186a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d65: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d6f: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d74: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d83: Unknown result type (might be due to invalid IL or missing references)
			//IL_1d96: Unknown result type (might be due to invalid IL or missing references)
			//IL_281b: Unknown result type (might be due to invalid IL or missing references)
			//IL_2820: Unknown result type (might be due to invalid IL or missing references)
			//IL_2862: Unknown result type (might be due to invalid IL or missing references)
			//IL_2867: Unknown result type (might be due to invalid IL or missing references)
			//IL_3149: Unknown result type (might be due to invalid IL or missing references)
			//IL_314e: Unknown result type (might be due to invalid IL or missing references)
			//IL_339e: Unknown result type (might be due to invalid IL or missing references)
			//IL_33a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_33bc: Unknown result type (might be due to invalid IL or missing references)
			//IL_33c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_33ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_33d3: Unknown result type (might be due to invalid IL or missing references)
			//IL_33dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_33e7: Unknown result type (might be due to invalid IL or missing references)
			//IL_340c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3419: Unknown result type (might be due to invalid IL or missing references)
			//IL_341e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3423: Unknown result type (might be due to invalid IL or missing references)
			//IL_3436: Unknown result type (might be due to invalid IL or missing references)
			//IL_343e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3443: Unknown result type (might be due to invalid IL or missing references)
			//IL_344d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3457: Unknown result type (might be due to invalid IL or missing references)
			//IL_346a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3475: Unknown result type (might be due to invalid IL or missing references)
			//IL_3489: Unknown result type (might be due to invalid IL or missing references)
			//IL_348e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3493: Unknown result type (might be due to invalid IL or missing references)
			//IL_349d: Unknown result type (might be due to invalid IL or missing references)
			//IL_34a5: Unknown result type (might be due to invalid IL or missing references)
			//IL_34aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_34b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_34be: Unknown result type (might be due to invalid IL or missing references)
			//IL_34e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_3502: Unknown result type (might be due to invalid IL or missing references)
			//IL_3516: Unknown result type (might be due to invalid IL or missing references)
			//IL_351b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3520: Unknown result type (might be due to invalid IL or missing references)
			//IL_3533: Unknown result type (might be due to invalid IL or missing references)
			//IL_353d: Unknown result type (might be due to invalid IL or missing references)
			//IL_354d: Unknown result type (might be due to invalid IL or missing references)
			//IL_355a: Unknown result type (might be due to invalid IL or missing references)
			//IL_355f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3564: Unknown result type (might be due to invalid IL or missing references)
			//IL_356e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3578: Unknown result type (might be due to invalid IL or missing references)
			//IL_359d: Unknown result type (might be due to invalid IL or missing references)
			//IL_35aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_35af: Unknown result type (might be due to invalid IL or missing references)
			//IL_35b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_35be: Unknown result type (might be due to invalid IL or missing references)
			//IL_35c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_35cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_35d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_35df: Unknown result type (might be due to invalid IL or missing references)
			//IL_35f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_35f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_35fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_3605: Unknown result type (might be due to invalid IL or missing references)
			//IL_360d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3612: Unknown result type (might be due to invalid IL or missing references)
			//IL_361c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3626: Unknown result type (might be due to invalid IL or missing references)
			//IL_3643: Unknown result type (might be due to invalid IL or missing references)
			//IL_365c: Unknown result type (might be due to invalid IL or missing references)
			//IL_3670: Unknown result type (might be due to invalid IL or missing references)
			//IL_3675: Unknown result type (might be due to invalid IL or missing references)
			//IL_367a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3681: Unknown result type (might be due to invalid IL or missing references)
			//IL_3689: Unknown result type (might be due to invalid IL or missing references)
			//IL_368e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3698: Unknown result type (might be due to invalid IL or missing references)
			//IL_36a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_36b5: Unknown result type (might be due to invalid IL or missing references)
			//IL_36bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_36d1: Unknown result type (might be due to invalid IL or missing references)
			//IL_36d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_36db: Unknown result type (might be due to invalid IL or missing references)
			//IL_36dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_36e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_36e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_36e9: Unknown result type (might be due to invalid IL or missing references)
			//IL_370d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3723: Unknown result type (might be due to invalid IL or missing references)
			//IL_3728: Unknown result type (might be due to invalid IL or missing references)
			//IL_372d: Unknown result type (might be due to invalid IL or missing references)
			//IL_372f: Unknown result type (might be due to invalid IL or missing references)
			//IL_3734: Unknown result type (might be due to invalid IL or missing references)
			//IL_3739: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a87: Unknown result type (might be due to invalid IL or missing references)
			//IL_3a98: Unknown result type (might be due to invalid IL or missing references)
			//IL_3c96: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cb1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cbb: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cc2: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ce2: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ce7: Unknown result type (might be due to invalid IL or missing references)
			//IL_3cf6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d0b: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d10: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d15: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d19: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d2e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d33: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d38: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d7e: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d80: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d82: Unknown result type (might be due to invalid IL or missing references)
			//IL_3d87: Unknown result type (might be due to invalid IL or missing references)
			//IL_3eb6: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ec3: Unknown result type (might be due to invalid IL or missing references)
			//IL_3ecf: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f10: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f29: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f6a: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f77: Unknown result type (might be due to invalid IL or missing references)
			//IL_3f83: Unknown result type (might be due to invalid IL or missing references)
			//IL_3fc4: Unknown result type (might be due to invalid IL or missing references)
			//IL_3fd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_3fdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_4021: Unknown result type (might be due to invalid IL or missing references)
			//IL_408f: Unknown result type (might be due to invalid IL or missing references)
			//IL_40fd: Unknown result type (might be due to invalid IL or missing references)
			//IL_416b: Unknown result type (might be due to invalid IL or missing references)
			//IL_41de: Unknown result type (might be due to invalid IL or missing references)
			//IL_424c: Unknown result type (might be due to invalid IL or missing references)
			//IL_42ba: Unknown result type (might be due to invalid IL or missing references)
			//IL_4328: Unknown result type (might be due to invalid IL or missing references)
			//IL_443a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4447: Unknown result type (might be due to invalid IL or missing references)
			//IL_4453: Unknown result type (might be due to invalid IL or missing references)
			//IL_4494: Unknown result type (might be due to invalid IL or missing references)
			//IL_44a1: Unknown result type (might be due to invalid IL or missing references)
			//IL_44ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d99: Unknown result type (might be due to invalid IL or missing references)
			//IL_4d9e: Unknown result type (might be due to invalid IL or missing references)
			//IL_4da0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4da5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dbe: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dc3: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dc5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dca: Unknown result type (might be due to invalid IL or missing references)
			//IL_4de3: Unknown result type (might be due to invalid IL or missing references)
			//IL_4de8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4dea: Unknown result type (might be due to invalid IL or missing references)
			//IL_4def: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e08: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e0d: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e14: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e7f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e81: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e86: Unknown result type (might be due to invalid IL or missing references)
			//IL_4e9f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ea4: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ea6: Unknown result type (might be due to invalid IL or missing references)
			//IL_4eab: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ec4: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ec9: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ecb: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ed0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ee9: Unknown result type (might be due to invalid IL or missing references)
			//IL_4eee: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ef0: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ef5: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f0a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f22: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f27: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f3a: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f3f: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f52: Unknown result type (might be due to invalid IL or missing references)
			//IL_4f57: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fa8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fad: Unknown result type (might be due to invalid IL or missing references)
			//IL_4faf: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fb4: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fd1: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fd6: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fd8: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fdd: Unknown result type (might be due to invalid IL or missing references)
			//IL_4ffa: Unknown result type (might be due to invalid IL or missing references)
			//IL_4fff: Unknown result type (might be due to invalid IL or missing references)
			//IL_5001: Unknown result type (might be due to invalid IL or missing references)
			//IL_5006: Unknown result type (might be due to invalid IL or missing references)
			//IL_5023: Unknown result type (might be due to invalid IL or missing references)
			//IL_5028: Unknown result type (might be due to invalid IL or missing references)
			//IL_502a: Unknown result type (might be due to invalid IL or missing references)
			//IL_502f: Unknown result type (might be due to invalid IL or missing references)
			//IL_504d: Unknown result type (might be due to invalid IL or missing references)
			//IL_506f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5091: Unknown result type (might be due to invalid IL or missing references)
			//IL_50b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_50d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_5118: Unknown result type (might be due to invalid IL or missing references)
			//IL_513a: Unknown result type (might be due to invalid IL or missing references)
			//IL_515c: Unknown result type (might be due to invalid IL or missing references)
			//IL_51c4: Unknown result type (might be due to invalid IL or missing references)
			//IL_51c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_522d: Unknown result type (might be due to invalid IL or missing references)
			//IL_5232: Unknown result type (might be due to invalid IL or missing references)
			//IL_525f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5281: Unknown result type (might be due to invalid IL or missing references)
			//IL_52a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_530b: Unknown result type (might be due to invalid IL or missing references)
			//IL_5310: Unknown result type (might be due to invalid IL or missing references)
			//IL_5374: Unknown result type (might be due to invalid IL or missing references)
			//IL_5379: Unknown result type (might be due to invalid IL or missing references)
			//IL_57c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_57cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_5823: Unknown result type (might be due to invalid IL or missing references)
			//IL_5825: Unknown result type (might be due to invalid IL or missing references)
			//IL_5831: Unknown result type (might be due to invalid IL or missing references)
			//IL_5912: Unknown result type (might be due to invalid IL or missing references)
			//IL_5914: Unknown result type (might be due to invalid IL or missing references)
			//IL_5920: Unknown result type (might be due to invalid IL or missing references)
			//IL_598d: Unknown result type (might be due to invalid IL or missing references)
			//IL_598f: Unknown result type (might be due to invalid IL or missing references)
			//IL_599b: Unknown result type (might be due to invalid IL or missing references)
			//IL_59cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_59e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a37: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a39: Unknown result type (might be due to invalid IL or missing references)
			//IL_5a45: Unknown result type (might be due to invalid IL or missing references)
			//IL_5aab: Unknown result type (might be due to invalid IL or missing references)
			//IL_5aad: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ab9: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c13: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c18: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c89: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c8b: Unknown result type (might be due to invalid IL or missing references)
			//IL_5c97: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d63: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d65: Unknown result type (might be due to invalid IL or missing references)
			//IL_5d71: Unknown result type (might be due to invalid IL or missing references)
			//IL_5dc7: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e7a: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e7c: Unknown result type (might be due to invalid IL or missing references)
			//IL_5e88: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f0f: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f11: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f1d: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f84: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f86: Unknown result type (might be due to invalid IL or missing references)
			//IL_5f92: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fec: Unknown result type (might be due to invalid IL or missing references)
			//IL_5fee: Unknown result type (might be due to invalid IL or missing references)
			//IL_5ffa: Unknown result type (might be due to invalid IL or missing references)
			//IL_60a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_60ad: Unknown result type (might be due to invalid IL or missing references)
			//IL_60b2: Unknown result type (might be due to invalid IL or missing references)
			//IL_60b4: Unknown result type (might be due to invalid IL or missing references)
			//IL_60b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_60be: Unknown result type (might be due to invalid IL or missing references)
			//IL_60d2: Unknown result type (might be due to invalid IL or missing references)
			//IL_60d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_60f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_60f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_60fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_60fe: Unknown result type (might be due to invalid IL or missing references)
			//IL_610c: Unknown result type (might be due to invalid IL or missing references)
			//IL_613d: Unknown result type (might be due to invalid IL or missing references)
			//IL_6167: Unknown result type (might be due to invalid IL or missing references)
			//IL_6190: Unknown result type (might be due to invalid IL or missing references)
			//IL_6192: Unknown result type (might be due to invalid IL or missing references)
			//IL_6196: Unknown result type (might be due to invalid IL or missing references)
			//IL_61a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_61a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_61f5: Unknown result type (might be due to invalid IL or missing references)
			//IL_61fa: Unknown result type (might be due to invalid IL or missing references)
			//IL_6208: Unknown result type (might be due to invalid IL or missing references)
			//IL_6237: Unknown result type (might be due to invalid IL or missing references)
			//IL_6261: Unknown result type (might be due to invalid IL or missing references)
			//IL_6290: Unknown result type (might be due to invalid IL or missing references)
			//IL_62c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_62cb: Unknown result type (might be due to invalid IL or missing references)
			//IL_62cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_62f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_62f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_62fc: Unknown result type (might be due to invalid IL or missing references)
			//IL_6311: Unknown result type (might be due to invalid IL or missing references)
			//IL_6313: Unknown result type (might be due to invalid IL or missing references)
			//IL_6317: Unknown result type (might be due to invalid IL or missing references)
			//IL_6328: Unknown result type (might be due to invalid IL or missing references)
			//IL_632a: Unknown result type (might be due to invalid IL or missing references)
			//IL_632e: Unknown result type (might be due to invalid IL or missing references)
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
				ClearMesh(updateMesh: true);
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
			m_fontScale = m_fontSize / m_currentFontAsset.fontInfo.PointSize * (m_isOrthographic ? 1f : 0.1f);
			float num = m_fontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
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
					float num18 = m_currentFontSize / m_fontAsset.fontInfo.PointSize * m_fontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
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
					m_fontScale = m_currentFontSize * num17 / m_currentFontAsset.fontInfo.PointSize * m_currentFontAsset.fontInfo.Scale * (m_isOrthographic ? 1f : 0.1f);
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
					_ = m_FXMatrix.m00;
					_ = 1f;
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
							ClearMesh(updateMesh: false);
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
							ClearMesh(updateMesh: false);
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
							ClearMesh(updateMesh: true);
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
					float num34 = 1f;
					if (m_isFXMatrixSet)
					{
						num34 = m_FXMatrix.m00;
					}
					m_xAdvance += ((m_cached_TextElement.xAdvance * num34 * num7 + m_characterSpacing + m_currentFontAsset.normalSpacingOffset) * num2 + m_cSpacing) * (1f - m_charWidthAdjDelta);
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
						float num35 = m_maxLineAscender - m_startOfLineAscender;
						AdjustLineOffset(m_firstCharacterOfLine, m_characterCount, num35);
						num24 -= num35;
						m_lineOffset += num35;
					}
					m_isNewPage = false;
					float num36 = m_maxLineAscender - m_lineOffset;
					float num37 = m_maxLineDescender - m_lineOffset;
					m_maxDescender = ((m_maxDescender < num37) ? m_maxDescender : num37);
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
					m_textInfo.lineInfo[m_lineNumber].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_firstVisibleCharacterOfLine].bottomLeft.x, num37);
					m_textInfo.lineInfo[m_lineNumber].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_lastVisibleCharacterOfLine].topRight.x, num36);
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
					m_textInfo.lineInfo[m_lineNumber].ascender = num36;
					m_textInfo.lineInfo[m_lineNumber].descender = num37;
					m_textInfo.lineInfo[m_lineNumber].lineHeight = num36 - num37 + num8 * num;
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
				ClearMesh(updateMesh: true);
				TMPro_EventManager.ON_TEXT_CHANGED((Object)(object)this);
				return;
			}
			int index = m_materialReferences[0].referenceCount * ((!m_isVolumetricText) ? 4 : 8);
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
			int num38 = 0;
			int lineCount = 0;
			int num39 = 0;
			bool flag10 = false;
			bool flag11 = false;
			int num40 = 0;
			int num41 = 0;
			float num42 = (m_previousLossyScaleY = transform.get_lossyScale().y);
			Color32 val13 = Color32.op_Implicit(Color.get_white());
			Color32 underlineColor = Color32.op_Implicit(Color.get_white());
			Color32 highlightColor = default(Color32);
			((Color32)(ref highlightColor))._002Ector(byte.MaxValue, byte.MaxValue, (byte)0, (byte)64);
			float num43 = 0f;
			float num44 = 0f;
			float num45 = 0f;
			float num46 = 0f;
			float num47 = TMP_Text.k_LargePositiveFloat;
			int num48 = 0;
			float num49 = 0f;
			float num50 = 0f;
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
					bool flag12 = (textAlignmentOptions & (TextAlignmentOptions)16) == (TextAlignmentOptions)16;
					if ((!char.IsControl(character3) && lineNumber < m_lineNumber) || flag12 || tMP_LineInfo.maxAdvance > tMP_LineInfo.width)
					{
						if (lineNumber != num39 || j == 0 || j == m_firstVisibleCharacter)
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
						float num51 = ((!m_isRightToLeft) ? (tMP_LineInfo.width - tMP_LineInfo.maxAdvance) : (tMP_LineInfo.width + tMP_LineInfo.maxAdvance));
						int num52 = tMP_LineInfo.visibleCharacterCount - 1;
						int num53 = (characterInfo[tMP_LineInfo.lastCharacterIndex].isVisible ? tMP_LineInfo.spaceCount : (tMP_LineInfo.spaceCount - 1));
						if (flag10)
						{
							num53--;
							num52++;
						}
						float num54 = ((num53 > 0) ? m_wordWrappingRatios : 1f);
						if (num53 < 1)
						{
							num53 = 1;
						}
						val12 = ((character2 != '\t' && !char.IsSeparator(character2)) ? (m_isRightToLeft ? (val12 - new Vector3(num51 * num54 / (float)num52, 0f, 0f)) : (val12 + new Vector3(num51 * num54 / (float)num52, 0f, 0f))) : (m_isRightToLeft ? (val12 - new Vector3(num51 * (1f - num54) / (float)num53, 0f, 0f)) : (val12 + new Vector3(num51 * (1f - num54) / (float)num53, 0f, 0f))));
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
						float num55 = m_uvLineOffset * (float)lineNumber % 1f;
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
								characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55;
								characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55;
								characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55;
								characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x - lineExtents.min.x) / (lineExtents.max.x - lineExtents.min.x) + num55;
							}
							else
							{
								characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
								characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
								characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
								characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
							}
							break;
						case TextureMappingOptions.Paragraph:
							characterInfo[j].vertex_BL.uv2.x = (characterInfo[j].vertex_BL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
							characterInfo[j].vertex_TL.uv2.x = (characterInfo[j].vertex_TL.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
							characterInfo[j].vertex_TR.uv2.x = (characterInfo[j].vertex_TR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
							characterInfo[j].vertex_BR.uv2.x = (characterInfo[j].vertex_BR.position.x + val12.x - m_meshExtents.min.x) / (m_meshExtents.max.x - m_meshExtents.min.x) + num55;
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
								characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num55;
								characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - lineExtents.min.y) / (lineExtents.max.y - lineExtents.min.y) + num55;
								characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
								characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
								break;
							case TextureMappingOptions.Paragraph:
								characterInfo[j].vertex_BL.uv2.y = (characterInfo[j].vertex_BL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num55;
								characterInfo[j].vertex_TL.uv2.y = (characterInfo[j].vertex_TL.position.y - m_meshExtents.min.y) / (m_meshExtents.max.y - m_meshExtents.min.y) + num55;
								characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
								characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
								break;
							case TextureMappingOptions.MatchAspect:
								Debug.Log((object)"ERROR: Cannot Match both Vertical & Horizontal.");
								break;
							}
							float num56 = (1f - (characterInfo[j].vertex_BL.uv2.y + characterInfo[j].vertex_TL.uv2.y) * characterInfo[j].aspectRatio) / 2f;
							characterInfo[j].vertex_BL.uv2.x = characterInfo[j].vertex_BL.uv2.y * characterInfo[j].aspectRatio + num56 + num55;
							characterInfo[j].vertex_TL.uv2.x = characterInfo[j].vertex_BL.uv2.x;
							characterInfo[j].vertex_TR.uv2.x = characterInfo[j].vertex_TL.uv2.y * characterInfo[j].aspectRatio + num56 + num55;
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
							float num57 = (1f - (characterInfo[j].vertex_BL.uv2.x + characterInfo[j].vertex_TR.uv2.x) / characterInfo[j].aspectRatio) / 2f;
							characterInfo[j].vertex_BL.uv2.y = num57 + characterInfo[j].vertex_BL.uv2.x / characterInfo[j].aspectRatio;
							characterInfo[j].vertex_TL.uv2.y = num57 + characterInfo[j].vertex_TR.uv2.x / characterInfo[j].aspectRatio;
							characterInfo[j].vertex_BR.uv2.y = characterInfo[j].vertex_BL.uv2.y;
							characterInfo[j].vertex_TR.uv2.y = characterInfo[j].vertex_TL.uv2.y;
							break;
						}
						}
						num43 = characterInfo[j].scale * num42 * (1f - m_charWidthAdjDelta);
						if (!characterInfo[j].isUsingAlternateTypeface && (characterInfo[j].style & FontStyles.Bold) == FontStyles.Bold)
						{
							num43 *= -1f;
						}
						float x = characterInfo[j].vertex_BL.uv2.x;
						float y = characterInfo[j].vertex_BL.uv2.y;
						float x2 = characterInfo[j].vertex_TR.uv2.x;
						float y2 = characterInfo[j].vertex_TR.uv2.y;
						float num58 = (int)x;
						float num59 = (int)y;
						x -= num58;
						x2 -= num58;
						y -= num59;
						y2 -= num59;
						characterInfo[j].vertex_BL.uv2.x = PackUV(x, y);
						characterInfo[j].vertex_BL.uv2.y = num43;
						characterInfo[j].vertex_TL.uv2.x = PackUV(x, y2);
						characterInfo[j].vertex_TL.uv2.y = num43;
						characterInfo[j].vertex_TR.uv2.x = PackUV(x2, y2);
						characterInfo[j].vertex_TR.uv2.y = num43;
						characterInfo[j].vertex_BR.uv2.x = PackUV(x2, y);
						characterInfo[j].vertex_BR.uv2.y = num43;
						break;
					}
					}
					if (j < m_maxVisibleCharacters && num38 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode != TextOverflowModes.Page)
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
					else if (j < m_maxVisibleCharacters && num38 < m_maxVisibleWords && lineNumber < m_maxVisibleLines && m_overflowMode == TextOverflowModes.Page && characterInfo[j].pageNumber == num10)
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
						FillCharacterVertexBuffers(j, index_X, m_isVolumetricText);
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
				if (lineNumber != num39 || j == m_characterCount - 1)
				{
					if (lineNumber != num39)
					{
						m_textInfo.lineInfo[num39].baseline += zero5.y;
						m_textInfo.lineInfo[num39].ascender += zero5.y;
						m_textInfo.lineInfo[num39].descender += zero5.y;
						m_textInfo.lineInfo[num39].lineExtents.min = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num39].firstCharacterIndex].bottomLeft.x, m_textInfo.lineInfo[num39].descender);
						m_textInfo.lineInfo[num39].lineExtents.max = new Vector2(m_textInfo.characterInfo[m_textInfo.lineInfo[num39].lastVisibleCharacterIndex].topRight.x, m_textInfo.lineInfo[num39].ascender);
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
						num40 = j;
					}
					if (flag11 && j == m_characterCount - 1)
					{
						int num60 = m_textInfo.wordInfo.Length;
						int wordCount = m_textInfo.wordCount;
						if (m_textInfo.wordCount + 1 > num60)
						{
							TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num60 + 1);
						}
						num41 = j;
						m_textInfo.wordInfo[wordCount].firstCharacterIndex = num40;
						m_textInfo.wordInfo[wordCount].lastCharacterIndex = num41;
						m_textInfo.wordInfo[wordCount].characterCount = num41 - num40 + 1;
						m_textInfo.wordInfo[wordCount].textComponent = this;
						num38++;
						m_textInfo.wordCount++;
						m_textInfo.lineInfo[lineNumber].wordCount++;
					}
				}
				else if ((flag11 || (j == 0 && (!char.IsPunctuation(character2) || char.IsWhiteSpace(character2) || character2 == '\u200b' || j == m_characterCount - 1))) && (j <= 0 || j >= characterInfo.Length - 1 || j >= m_characterCount || (character2 != '\'' && character2 != '’') || !char.IsLetterOrDigit(characterInfo[j - 1].character) || !char.IsLetterOrDigit(characterInfo[j + 1].character)))
				{
					num41 = ((j == m_characterCount - 1 && char.IsLetterOrDigit(character2)) ? j : (j - 1));
					flag11 = false;
					int num61 = m_textInfo.wordInfo.Length;
					int wordCount2 = m_textInfo.wordCount;
					if (m_textInfo.wordCount + 1 > num61)
					{
						TMP_TextInfo.Resize(ref m_textInfo.wordInfo, num61 + 1);
					}
					m_textInfo.wordInfo[wordCount2].firstCharacterIndex = num40;
					m_textInfo.wordInfo[wordCount2].lastCharacterIndex = num41;
					m_textInfo.wordInfo[wordCount2].characterCount = num41 - num40 + 1;
					m_textInfo.wordInfo[wordCount2].textComponent = this;
					num38++;
					m_textInfo.wordCount++;
					m_textInfo.lineInfo[lineNumber].wordCount++;
				}
				if ((m_textInfo.characterInfo[j].style & FontStyles.Underline) == FontStyles.Underline)
				{
					bool flag13 = true;
					int pageNumber = m_textInfo.characterInfo[j].pageNumber;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber + 1 != m_pageToDisplay))
					{
						flag13 = false;
					}
					if (!char.IsWhiteSpace(character2) && character2 != '\u200b')
					{
						num46 = Mathf.Max(num46, m_textInfo.characterInfo[j].scale);
						num47 = Mathf.Min((pageNumber == num48) ? num47 : TMP_Text.k_LargePositiveFloat, m_textInfo.characterInfo[j].baseLine + base.font.fontInfo.Underline * num46);
						num48 = pageNumber;
					}
					if (!flag && flag13 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag = true;
						num44 = m_textInfo.characterInfo[j].scale;
						if (num46 == 0f)
						{
							num46 = num44;
						}
						((Vector3)(ref zero))._002Ector(m_textInfo.characterInfo[j].bottomLeft.x, num47, 0f);
						val13 = m_textInfo.characterInfo[j].underlineColor;
					}
					if (flag && m_characterCount == 1)
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num47, 0f);
						num45 = m_textInfo.characterInfo[j].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num44, num45, num46, num43, val13);
						num46 = 0f;
						num47 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && (j == tMP_LineInfo.lastCharacterIndex || j >= tMP_LineInfo.lastVisibleCharacterIndex))
					{
						if (char.IsWhiteSpace(character2) || character2 == '\u200b')
						{
							int lastVisibleCharacterIndex = tMP_LineInfo.lastVisibleCharacterIndex;
							((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex].topRight.x, num47, 0f);
							num45 = m_textInfo.characterInfo[lastVisibleCharacterIndex].scale;
						}
						else
						{
							((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num47, 0f);
							num45 = m_textInfo.characterInfo[j].scale;
						}
						flag = false;
						DrawUnderlineMesh(zero, zero2, ref index, num44, num45, num46, num43, val13);
						num46 = 0f;
						num47 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && !flag13)
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, num47, 0f);
						num45 = m_textInfo.characterInfo[j - 1].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num44, num45, num46, num43, val13);
						num46 = 0f;
						num47 = TMP_Text.k_LargePositiveFloat;
					}
					else if (flag && j < m_characterCount - 1 && !val13.Compare(m_textInfo.characterInfo[j + 1].underlineColor))
					{
						flag = false;
						((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j].topRight.x, num47, 0f);
						num45 = m_textInfo.characterInfo[j].scale;
						DrawUnderlineMesh(zero, zero2, ref index, num44, num45, num46, num43, val13);
						num46 = 0f;
						num47 = TMP_Text.k_LargePositiveFloat;
					}
				}
				else if (flag)
				{
					flag = false;
					((Vector3)(ref zero2))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, num47, 0f);
					num45 = m_textInfo.characterInfo[j - 1].scale;
					DrawUnderlineMesh(zero, zero2, ref index, num44, num45, num46, num43, val13);
					num46 = 0f;
					num47 = TMP_Text.k_LargePositiveFloat;
				}
				bool num62 = (m_textInfo.characterInfo[j].style & FontStyles.Strikethrough) == FontStyles.Strikethrough;
				float strikethrough = fontAsset.fontInfo.strikethrough;
				if (num62)
				{
					bool flag14 = true;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && m_textInfo.characterInfo[j].pageNumber + 1 != m_pageToDisplay))
					{
						flag14 = false;
					}
					if (!flag2 && flag14 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag2 = true;
						num49 = m_textInfo.characterInfo[j].pointSize;
						num50 = m_textInfo.characterInfo[j].scale;
						((Vector3)(ref zero3))._002Ector(m_textInfo.characterInfo[j].bottomLeft.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num50, 0f);
						underlineColor = m_textInfo.characterInfo[j].strikethroughColor;
						b = m_textInfo.characterInfo[j].baseLine;
					}
					if (flag2 && m_characterCount == 1)
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num50, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
					}
					else if (flag2 && j == tMP_LineInfo.lastCharacterIndex)
					{
						if (char.IsWhiteSpace(character2) || character2 == '\u200b')
						{
							int lastVisibleCharacterIndex2 = tMP_LineInfo.lastVisibleCharacterIndex;
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex2].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex2].baseLine + strikethrough * num50, 0f);
						}
						else
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num50, 0f);
						}
						flag2 = false;
						DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
					}
					else if (flag2 && j < m_characterCount && (m_textInfo.characterInfo[j + 1].pointSize != num49 || !TMP_Math.Approximately(m_textInfo.characterInfo[j + 1].baseLine + zero5.y, b)))
					{
						flag2 = false;
						int lastVisibleCharacterIndex3 = tMP_LineInfo.lastVisibleCharacterIndex;
						if (j > lastVisibleCharacterIndex3)
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[lastVisibleCharacterIndex3].topRight.x, m_textInfo.characterInfo[lastVisibleCharacterIndex3].baseLine + strikethrough * num50, 0f);
						}
						else
						{
							((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num50, 0f);
						}
						DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
					}
					else if (flag2 && j < m_characterCount && ((Object)fontAsset).GetInstanceID() != ((Object)characterInfo[j + 1].fontAsset).GetInstanceID())
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j].topRight.x, m_textInfo.characterInfo[j].baseLine + strikethrough * num50, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
					}
					else if (flag2 && !flag14)
					{
						flag2 = false;
						((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num50, 0f);
						DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
					}
				}
				else if (flag2)
				{
					flag2 = false;
					((Vector3)(ref zero4))._002Ector(m_textInfo.characterInfo[j - 1].topRight.x, m_textInfo.characterInfo[j - 1].baseLine + strikethrough * num50, 0f);
					DrawUnderlineMesh(zero3, zero4, ref index, num50, num50, num50, num43, underlineColor);
				}
				if ((m_textInfo.characterInfo[j].style & FontStyles.Highlight) == FontStyles.Highlight)
				{
					bool flag15 = true;
					int pageNumber2 = m_textInfo.characterInfo[j].pageNumber;
					if (j > m_maxVisibleCharacters || lineNumber > m_maxVisibleLines || (m_overflowMode == TextOverflowModes.Page && pageNumber2 + 1 != m_pageToDisplay))
					{
						flag15 = false;
					}
					if (!flag3 && flag15 && j <= tMP_LineInfo.lastVisibleCharacterIndex && character2 != '\n' && character2 != '\r' && (j != tMP_LineInfo.lastVisibleCharacterIndex || !char.IsSeparator(character2)))
					{
						flag3 = true;
						val = Vector2.op_Implicit(TMP_Text.k_LargePositiveVector2);
						val2 = Vector2.op_Implicit(TMP_Text.k_LargeNegativeVector2);
						highlightColor = m_textInfo.characterInfo[j].highlightColor;
					}
					if (flag3)
					{
						Color32 highlightColor2 = m_textInfo.characterInfo[j].highlightColor;
						bool flag16 = false;
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
							flag16 = true;
						}
						if (!flag16)
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
					else if (flag3 && !flag15)
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
				num39 = lineNumber;
			}
			m_textInfo.characterCount = m_characterCount;
			m_textInfo.spriteCount = m_spriteCount;
			m_textInfo.lineCount = lineCount;
			m_textInfo.wordCount = ((num38 == 0 || m_characterCount <= 0) ? 1 : num38);
			m_textInfo.pageCount = m_pageNumber + 1;
			if (m_renderMode == TextRenderFlags.Render)
			{
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

		private void SetMeshFilters(bool state)
		{
			if ((Object)(object)m_meshFilter != (Object)null)
			{
				if (state)
				{
					m_meshFilter.set_sharedMesh(m_mesh);
				}
				else
				{
					m_meshFilter.set_sharedMesh((Mesh)null);
				}
			}
			for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
			{
				if ((Object)(object)m_subTextObjects[i].meshFilter != (Object)null)
				{
					if (state)
					{
						m_subTextObjects[i].meshFilter.set_sharedMesh(m_subTextObjects[i].mesh);
					}
					else
					{
						m_subTextObjects[i].meshFilter.set_sharedMesh((Mesh)null);
					}
				}
			}
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

		protected override void ClearSubMeshObjects()
		{
			for (int i = 1; i < m_subTextObjects.Length && (Object)(object)m_subTextObjects[i] != (Object)null; i++)
			{
				Debug.Log((object)("Destroying Sub Text object[" + i + "]."));
				Object.DestroyImmediate((Object)(object)m_subTextObjects[i]);
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
			for (int i = 0; i < m_textInfo.characterCount; i++)
			{
				if (m_textInfo.characterInfo[i].isVisible && m_textInfo.characterInfo[i].elementType == TMP_TextElementType.Character)
				{
					float num = lossyScale * m_textInfo.characterInfo[i].scale * (1f - m_charWidthAdjDelta);
					if (!m_textInfo.characterInfo[i].isUsingAlternateTypeface && (m_textInfo.characterInfo[i].style & FontStyles.Bold) == FontStyles.Bold)
					{
						num *= -1f;
					}
					int materialReferenceIndex = m_textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = m_textInfo.characterInfo[i].vertexIndex;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex].y = num;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 1].y = num;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 2].y = num;
					m_textInfo.meshInfo[materialReferenceIndex].uvs2[vertexIndex + 3].y = num;
				}
			}
			for (int j = 0; j < m_textInfo.meshInfo.Length; j++)
			{
				if (j == 0)
				{
					m_mesh.set_uv2(m_textInfo.meshInfo[0].uvs2);
				}
				else
				{
					m_subTextObjects[j].mesh.set_uv2(m_textInfo.meshInfo[j].uvs2);
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

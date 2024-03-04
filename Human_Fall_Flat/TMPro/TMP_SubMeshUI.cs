using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	public class TMP_SubMeshUI : MaskableGraphic, IClippable, IMaskable, IMaterialModifier
	{
		[SerializeField]
		private TMP_FontAsset m_fontAsset;

		[SerializeField]
		private TMP_SpriteAsset m_spriteAsset;

		[SerializeField]
		private Material m_material;

		[SerializeField]
		private Material m_sharedMaterial;

		private Material m_fallbackMaterial;

		private Material m_fallbackSourceMaterial;

		[SerializeField]
		private bool m_isDefaultMaterial;

		[SerializeField]
		private float m_padding;

		[SerializeField]
		private CanvasRenderer m_canvasRenderer;

		private Mesh m_mesh;

		[SerializeField]
		private TextMeshProUGUI m_TextComponent;

		[NonSerialized]
		private bool m_isRegisteredForEvents;

		private bool m_materialDirty;

		[SerializeField]
		private int m_materialReferenceIndex;

		public TMP_FontAsset fontAsset
		{
			get
			{
				return m_fontAsset;
			}
			set
			{
				m_fontAsset = value;
			}
		}

		public TMP_SpriteAsset spriteAsset
		{
			get
			{
				return m_spriteAsset;
			}
			set
			{
				m_spriteAsset = value;
			}
		}

		public override Texture mainTexture
		{
			get
			{
				if ((Object)(object)sharedMaterial != (Object)null)
				{
					return sharedMaterial.GetTexture(ShaderUtilities.ID_MainTex);
				}
				return null;
			}
		}

		public override Material material
		{
			get
			{
				return GetMaterial(m_sharedMaterial);
			}
			set
			{
				if (!((Object)(object)m_sharedMaterial != (Object)null) || ((Object)m_sharedMaterial).GetInstanceID() != ((Object)value).GetInstanceID())
				{
					m_sharedMaterial = (m_material = value);
					m_padding = GetPaddingForMaterial();
					((Graphic)this).SetVerticesDirty();
					((Graphic)this).SetMaterialDirty();
				}
			}
		}

		public Material sharedMaterial
		{
			get
			{
				return m_sharedMaterial;
			}
			set
			{
				SetSharedMaterial(value);
			}
		}

		public Material fallbackMaterial
		{
			get
			{
				return m_fallbackMaterial;
			}
			set
			{
				if (!((Object)(object)m_fallbackMaterial == (Object)(object)value))
				{
					if ((Object)(object)m_fallbackMaterial != (Object)null && (Object)(object)m_fallbackMaterial != (Object)(object)value)
					{
						TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
					}
					m_fallbackMaterial = value;
					TMP_MaterialManager.AddFallbackMaterialReference(m_fallbackMaterial);
					SetSharedMaterial(m_fallbackMaterial);
				}
			}
		}

		public Material fallbackSourceMaterial
		{
			get
			{
				return m_fallbackSourceMaterial;
			}
			set
			{
				m_fallbackSourceMaterial = value;
			}
		}

		public override Material materialForRendering => TMP_MaterialManager.GetMaterialForRendering((MaskableGraphic)(object)this, m_sharedMaterial);

		public bool isDefaultMaterial
		{
			get
			{
				return m_isDefaultMaterial;
			}
			set
			{
				m_isDefaultMaterial = value;
			}
		}

		public float padding
		{
			get
			{
				return m_padding;
			}
			set
			{
				m_padding = value;
			}
		}

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

		public Mesh mesh
		{
			get
			{
				//IL_000f: Unknown result type (might be due to invalid IL or missing references)
				//IL_0019: Expected O, but got Unknown
				if ((Object)(object)m_mesh == (Object)null)
				{
					m_mesh = new Mesh();
					((Object)m_mesh).set_hideFlags((HideFlags)61);
				}
				return m_mesh;
			}
			set
			{
				m_mesh = value;
			}
		}

		public static TMP_SubMeshUI AddSubTextObject(TextMeshProUGUI textComponent, MaterialReference materialReference)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0055: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_0082: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = new GameObject("TMP UI SubObject [" + ((Object)materialReference.material).get_name() + "]", new Type[1] { typeof(RectTransform) });
			val.get_transform().SetParent(textComponent.transform, false);
			val.set_layer(((Component)textComponent).get_gameObject().get_layer());
			RectTransform component = val.GetComponent<RectTransform>();
			component.set_anchorMin(Vector2.get_zero());
			component.set_anchorMax(Vector2.get_one());
			component.set_sizeDelta(Vector2.get_zero());
			component.set_pivot(textComponent.rectTransform.get_pivot());
			TMP_SubMeshUI tMP_SubMeshUI = val.AddComponent<TMP_SubMeshUI>();
			tMP_SubMeshUI.m_canvasRenderer = tMP_SubMeshUI.canvasRenderer;
			tMP_SubMeshUI.m_TextComponent = textComponent;
			tMP_SubMeshUI.m_materialReferenceIndex = materialReference.index;
			tMP_SubMeshUI.m_fontAsset = materialReference.fontAsset;
			tMP_SubMeshUI.m_spriteAsset = materialReference.spriteAsset;
			tMP_SubMeshUI.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			tMP_SubMeshUI.SetSharedMaterial(materialReference.material);
			return tMP_SubMeshUI;
		}

		protected override void OnEnable()
		{
			if (!m_isRegisteredForEvents)
			{
				TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Add(ON_MATERIAL_PROPERTY_CHANGED);
				TMPro_EventManager.FONT_PROPERTY_EVENT.Add(ON_FONT_PROPERTY_CHANGED);
				TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Add(ON_DRAG_AND_DROP_MATERIAL);
				TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Add(ON_SPRITE_ASSET_PROPERTY_CHANGED);
				m_isRegisteredForEvents = true;
			}
			base.m_ShouldRecalculateStencil = true;
			((MaskableGraphic)this).RecalculateClipping();
			((MaskableGraphic)this).RecalculateMasking();
		}

		protected override void OnDisable()
		{
			TMP_UpdateRegistry.UnRegisterCanvasElementForRebuild((ICanvasElement)(object)this);
			if ((Object)(object)base.m_MaskMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(base.m_MaskMaterial);
				base.m_MaskMaterial = null;
			}
			if ((Object)(object)m_fallbackMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				m_fallbackMaterial = null;
			}
			((MaskableGraphic)this).OnDisable();
		}

		protected override void OnDestroy()
		{
			if ((Object)(object)m_mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_mesh);
			}
			if ((Object)(object)base.m_MaskMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseStencilMaterial(base.m_MaskMaterial);
			}
			if ((Object)(object)m_fallbackMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				m_fallbackMaterial = null;
			}
			TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Remove(ON_MATERIAL_PROPERTY_CHANGED);
			TMPro_EventManager.FONT_PROPERTY_EVENT.Remove(ON_FONT_PROPERTY_CHANGED);
			TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Remove(ON_DRAG_AND_DROP_MATERIAL);
			TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Remove(ON_SPRITE_ASSET_PROPERTY_CHANGED);
			m_isRegisteredForEvents = false;
			((MaskableGraphic)this).RecalculateClipping();
		}

		private void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			int instanceID = ((Object)mat).GetInstanceID();
			int instanceID2 = ((Object)m_sharedMaterial).GetInstanceID();
			int num = ((!((Object)(object)base.m_MaskMaterial == (Object)null)) ? ((Object)base.m_MaskMaterial).GetInstanceID() : 0);
			int num2 = ((!((Object)(object)m_fallbackSourceMaterial == (Object)null)) ? ((Object)m_fallbackSourceMaterial).GetInstanceID() : 0);
			if ((Object)(object)m_fallbackMaterial != (Object)null && num2 == instanceID)
			{
				TMP_MaterialManager.CopyMaterialPresetProperties(mat, m_fallbackMaterial);
			}
			if ((Object)(object)m_TextComponent == (Object)null)
			{
				m_TextComponent = ((Component)this).GetComponentInParent<TextMeshProUGUI>();
			}
			if ((Object)(object)base.m_MaskMaterial != (Object)null)
			{
				Undo.RecordObject((Object)(object)base.m_MaskMaterial, "Material Property Changes");
				Undo.RecordObject((Object)(object)m_sharedMaterial, "Material Property Changes");
				if (instanceID == instanceID2)
				{
					float @float = base.m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilID);
					float float2 = base.m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilComp);
					base.m_MaskMaterial.CopyPropertiesFromMaterial(mat);
					base.m_MaskMaterial.set_shaderKeywords(mat.get_shaderKeywords());
					base.m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilID, @float);
					base.m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilComp, float2);
				}
				else if (instanceID == num)
				{
					GetPaddingForMaterial(mat);
					m_sharedMaterial.CopyPropertiesFromMaterial(mat);
					m_sharedMaterial.set_shaderKeywords(mat.get_shaderKeywords());
					m_sharedMaterial.SetFloat(ShaderUtilities.ID_StencilID, 0f);
					m_sharedMaterial.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
				}
				else if (num2 == instanceID)
				{
					float float3 = base.m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilID);
					float float4 = base.m_MaskMaterial.GetFloat(ShaderUtilities.ID_StencilComp);
					base.m_MaskMaterial.CopyPropertiesFromMaterial(m_fallbackMaterial);
					base.m_MaskMaterial.set_shaderKeywords(m_fallbackMaterial.get_shaderKeywords());
					base.m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilID, float3);
					base.m_MaskMaterial.SetFloat(ShaderUtilities.ID_StencilComp, float4);
				}
			}
			m_padding = GetPaddingForMaterial();
			((Graphic)this).SetVerticesDirty();
			base.m_ShouldRecalculateStencil = true;
			((MaskableGraphic)this).RecalculateClipping();
			((MaskableGraphic)this).RecalculateMasking();
		}

		private void ON_DRAG_AND_DROP_MATERIAL(GameObject obj, Material currentMaterial, Material newMaterial)
		{
			if (((Object)(object)obj == (Object)(object)((Component)this).get_gameObject() || PrefabUtility.GetPrefabParent((Object)(object)((Component)this).get_gameObject()) == (Object)(object)obj) && m_isDefaultMaterial)
			{
				if ((Object)(object)m_canvasRenderer == (Object)null)
				{
					m_canvasRenderer = ((Component)this).GetComponent<CanvasRenderer>();
				}
				Undo.RecordObject((Object)(object)this, "Material Assignment");
				Undo.RecordObject((Object)(object)m_canvasRenderer, "Material Assignment");
				SetSharedMaterial(newMaterial);
				m_TextComponent.havePropertiesChanged = true;
			}
		}

		private void ON_SPRITE_ASSET_PROPERTY_CHANGED(bool isChanged, Object obj)
		{
			if ((Object)(object)m_TextComponent != (Object)null)
			{
				m_TextComponent.havePropertiesChanged = true;
			}
		}

		private void ON_FONT_PROPERTY_CHANGED(bool isChanged, TMP_FontAsset font)
		{
			if (((Object)font).GetInstanceID() == ((Object)m_fontAsset).GetInstanceID() && (Object)(object)m_fallbackMaterial != (Object)null)
			{
				m_fallbackMaterial.SetFloat(ShaderUtilities.ID_WeightNormal, m_fontAsset.normalStyle);
				m_fallbackMaterial.SetFloat(ShaderUtilities.ID_WeightBold, m_fontAsset.boldStyle);
			}
		}

		private void ON_TMP_SETTINGS_CHANGED()
		{
		}

		protected override void OnTransformParentChanged()
		{
			if (((UIBehaviour)this).IsActive())
			{
				base.m_ShouldRecalculateStencil = true;
				((MaskableGraphic)this).RecalculateClipping();
				((MaskableGraphic)this).RecalculateMasking();
			}
		}

		public override Material GetModifiedMaterial(Material baseMaterial)
		{
			Material val = baseMaterial;
			if (base.m_ShouldRecalculateStencil)
			{
				base.m_StencilValue = TMP_MaterialManager.GetStencilID(((Component)this).get_gameObject());
				base.m_ShouldRecalculateStencil = false;
			}
			if (base.m_StencilValue > 0)
			{
				val = TMP_MaterialManager.GetStencilMaterial(baseMaterial, base.m_StencilValue);
				if ((Object)(object)base.m_MaskMaterial != (Object)null)
				{
					TMP_MaterialManager.ReleaseStencilMaterial(base.m_MaskMaterial);
				}
				base.m_MaskMaterial = val;
			}
			return val;
		}

		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(m_sharedMaterial, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
		}

		public float GetPaddingForMaterial(Material mat)
		{
			return ShaderUtilities.GetPadding(mat, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
		}

		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		public override void SetAllDirty()
		{
		}

		public override void SetVerticesDirty()
		{
			if (((UIBehaviour)this).IsActive() && (Object)(object)m_TextComponent != (Object)null)
			{
				m_TextComponent.havePropertiesChanged = true;
				((Graphic)m_TextComponent).SetVerticesDirty();
			}
		}

		public override void SetLayoutDirty()
		{
		}

		public override void SetMaterialDirty()
		{
			m_materialDirty = true;
			((Graphic)this).UpdateMaterial();
			if (((Graphic)this).m_OnDirtyMaterialCallback != null)
			{
				((Graphic)this).m_OnDirtyMaterialCallback.Invoke();
			}
		}

		public void SetPivotDirty()
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			if (((UIBehaviour)this).IsActive())
			{
				((Graphic)this).get_rectTransform().set_pivot(m_TextComponent.rectTransform.get_pivot());
			}
		}

		public override void Cull(Rect clipRect, bool validRect)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			if (!m_TextComponent.ignoreRectMaskCulling)
			{
				((MaskableGraphic)this).Cull(clipRect, validRect);
			}
		}

		protected override void UpdateGeometry()
		{
			Debug.Log((object)"UpdateGeometry()");
		}

		public override void Rebuild(CanvasUpdate update)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Invalid comparison between Unknown and I4
			if ((int)update == 3 && m_materialDirty)
			{
				((Graphic)this).UpdateMaterial();
				m_materialDirty = false;
			}
		}

		public void RefreshMaterial()
		{
			((Graphic)this).UpdateMaterial();
		}

		protected override void UpdateMaterial()
		{
			if ((Object)(object)m_canvasRenderer == (Object)null)
			{
				m_canvasRenderer = canvasRenderer;
			}
			m_canvasRenderer.set_materialCount(1);
			m_canvasRenderer.SetMaterial(((Graphic)this).get_materialForRendering(), 0);
			m_canvasRenderer.SetTexture(((Graphic)this).get_mainTexture());
			if ((Object)(object)m_sharedMaterial != (Object)null && ((Object)((Component)this).get_gameObject()).get_name() != "TMP SubMeshUI [" + ((Object)m_sharedMaterial).get_name() + "]")
			{
				((Object)((Component)this).get_gameObject()).set_name("TMP SubMeshUI [" + ((Object)m_sharedMaterial).get_name() + "]");
			}
		}

		public override void RecalculateClipping()
		{
			((MaskableGraphic)this).RecalculateClipping();
		}

		public override void RecalculateMasking()
		{
			base.m_ShouldRecalculateStencil = true;
			((Graphic)this).SetMaterialDirty();
		}

		private Material GetMaterial()
		{
			return m_sharedMaterial;
		}

		private Material GetMaterial(Material mat)
		{
			if ((Object)(object)m_material == (Object)null || ((Object)m_material).GetInstanceID() != ((Object)mat).GetInstanceID())
			{
				m_material = CreateMaterialInstance(mat);
			}
			m_sharedMaterial = m_material;
			m_padding = GetPaddingForMaterial();
			((Graphic)this).SetVerticesDirty();
			((Graphic)this).SetMaterialDirty();
			return m_sharedMaterial;
		}

		private Material CreateMaterialInstance(Material source)
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Expected O, but got Unknown
			Material val = new Material(source);
			val.set_shaderKeywords(source.get_shaderKeywords());
			((Object)val).set_name(((Object)val).get_name() + " (Instance)");
			return val;
		}

		private Material GetSharedMaterial()
		{
			if ((Object)(object)m_canvasRenderer == (Object)null)
			{
				m_canvasRenderer = ((Component)this).GetComponent<CanvasRenderer>();
			}
			return m_canvasRenderer.GetMaterial();
		}

		private void SetSharedMaterial(Material mat)
		{
			m_sharedMaterial = mat;
			((Graphic)this).m_Material = m_sharedMaterial;
			m_padding = GetPaddingForMaterial();
			((Graphic)this).SetMaterialDirty();
		}

		public TMP_SubMeshUI()
			: this()
		{
		}
	}
}

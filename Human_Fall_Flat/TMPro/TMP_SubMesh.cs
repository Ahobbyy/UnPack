using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	[ExecuteInEditMode]
	[RequireComponent(typeof(MeshRenderer))]
	[RequireComponent(typeof(MeshFilter))]
	public class TMP_SubMesh : MonoBehaviour
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
		private Renderer m_renderer;

		[SerializeField]
		private MeshFilter m_meshFilter;

		private Mesh m_mesh;

		[SerializeField]
		private BoxCollider m_boxCollider;

		[SerializeField]
		private TextMeshPro m_TextComponent;

		[NonSerialized]
		private bool m_isRegisteredForEvents;

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

		public Material material
		{
			get
			{
				return GetMaterial(m_sharedMaterial);
			}
			set
			{
				if (((Object)m_sharedMaterial).GetInstanceID() != ((Object)value).GetInstanceID())
				{
					m_sharedMaterial = (m_material = value);
					m_padding = GetPaddingForMaterial();
					SetVerticesDirty();
					SetMaterialDirty();
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
					meshFilter.set_mesh(m_mesh);
				}
				return m_mesh;
			}
			set
			{
				m_mesh = value;
			}
		}

		public BoxCollider boxCollider
		{
			get
			{
				if ((Object)(object)m_boxCollider == (Object)null)
				{
					m_boxCollider = ((Component)this).GetComponent<BoxCollider>();
					if ((Object)(object)m_boxCollider == (Object)null)
					{
						m_boxCollider = ((Component)this).get_gameObject().AddComponent<BoxCollider>();
						((Component)this).get_gameObject().AddComponent<Rigidbody>();
					}
				}
				return m_boxCollider;
			}
		}

		private void OnEnable()
		{
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			if (!m_isRegisteredForEvents)
			{
				TMPro_EventManager.MATERIAL_PROPERTY_EVENT.Add(ON_MATERIAL_PROPERTY_CHANGED);
				TMPro_EventManager.FONT_PROPERTY_EVENT.Add(ON_FONT_PROPERTY_CHANGED);
				TMPro_EventManager.DRAG_AND_DROP_MATERIAL_EVENT.Add(ON_DRAG_AND_DROP_MATERIAL);
				TMPro_EventManager.SPRITE_ASSET_PROPERTY_EVENT.Add(ON_SPRITE_ASSET_PROPERTY_CHANGED);
				m_isRegisteredForEvents = true;
			}
			meshFilter.set_sharedMesh(mesh);
			if ((Object)(object)m_sharedMaterial != (Object)null)
			{
				m_sharedMaterial.SetVector(ShaderUtilities.ID_ClipRect, new Vector4(-32767f, -32767f, 32767f, 32767f));
			}
		}

		private void OnDisable()
		{
			m_meshFilter.set_sharedMesh((Mesh)null);
			if ((Object)(object)m_fallbackMaterial != (Object)null)
			{
				TMP_MaterialManager.ReleaseFallbackMaterial(m_fallbackMaterial);
				m_fallbackMaterial = null;
			}
		}

		private void OnDestroy()
		{
			if ((Object)(object)m_mesh != (Object)null)
			{
				Object.DestroyImmediate((Object)(object)m_mesh);
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
		}

		private void ON_MATERIAL_PROPERTY_CHANGED(bool isChanged, Material mat)
		{
			int instanceID = ((Object)mat).GetInstanceID();
			int instanceID2 = ((Object)m_sharedMaterial).GetInstanceID();
			int num = ((!((Object)(object)m_fallbackSourceMaterial == (Object)null)) ? ((Object)m_fallbackSourceMaterial).GetInstanceID() : 0);
			if (instanceID != instanceID2)
			{
				if (!((Object)(object)m_fallbackMaterial != (Object)null) || num != instanceID)
				{
					return;
				}
				TMP_MaterialManager.CopyMaterialPresetProperties(mat, m_fallbackMaterial);
			}
			if ((Object)(object)m_TextComponent == (Object)null)
			{
				m_TextComponent = ((Component)this).GetComponentInParent<TextMeshPro>();
			}
			m_padding = GetPaddingForMaterial();
			m_TextComponent.havePropertiesChanged = true;
			((Graphic)m_TextComponent).SetVerticesDirty();
		}

		private void ON_DRAG_AND_DROP_MATERIAL(GameObject obj, Material currentMaterial, Material newMaterial)
		{
			if (((Object)(object)obj == (Object)(object)((Component)this).get_gameObject() || PrefabUtility.GetPrefabParent((Object)(object)((Component)this).get_gameObject()) == (Object)(object)obj) && m_isDefaultMaterial)
			{
				if ((Object)(object)m_renderer == (Object)null)
				{
					m_renderer = ((Component)this).GetComponent<Renderer>();
				}
				Undo.RecordObject((Object)(object)this, "Material Assignment");
				Undo.RecordObject((Object)(object)m_renderer, "Material Assignment");
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

		public static TMP_SubMesh AddSubTextObject(TextMeshPro textComponent, MaterialReference materialReference)
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Expected O, but got Unknown
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			GameObject val = new GameObject("TMP SubMesh [" + ((Object)materialReference.material).get_name() + "]", new Type[1] { typeof(TMP_SubMesh) });
			TMP_SubMesh component = val.GetComponent<TMP_SubMesh>();
			val.get_transform().SetParent(textComponent.transform, false);
			val.get_transform().set_localPosition(Vector3.get_zero());
			val.get_transform().set_localRotation(Quaternion.get_identity());
			val.get_transform().set_localScale(Vector3.get_one());
			val.set_layer(((Component)textComponent).get_gameObject().get_layer());
			component.m_meshFilter = val.GetComponent<MeshFilter>();
			component.m_TextComponent = textComponent;
			component.m_fontAsset = materialReference.fontAsset;
			component.m_spriteAsset = materialReference.spriteAsset;
			component.m_isDefaultMaterial = materialReference.isDefaultMaterial;
			component.SetSharedMaterial(materialReference.material);
			component.renderer.set_sortingLayerID(textComponent.renderer.get_sortingLayerID());
			component.renderer.set_sortingOrder(textComponent.renderer.get_sortingOrder());
			return component;
		}

		public void DestroySelf()
		{
			Object.Destroy((Object)(object)((Component)this).get_gameObject(), 1f);
		}

		private Material GetMaterial(Material mat)
		{
			if ((Object)(object)m_renderer == (Object)null)
			{
				m_renderer = ((Component)this).GetComponent<Renderer>();
			}
			if ((Object)(object)m_material == (Object)null || ((Object)m_material).GetInstanceID() != ((Object)mat).GetInstanceID())
			{
				m_material = CreateMaterialInstance(mat);
			}
			m_sharedMaterial = m_material;
			m_padding = GetPaddingForMaterial();
			SetVerticesDirty();
			SetMaterialDirty();
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
			if ((Object)(object)m_renderer == (Object)null)
			{
				m_renderer = ((Component)this).GetComponent<Renderer>();
			}
			return m_renderer.get_sharedMaterial();
		}

		private void SetSharedMaterial(Material mat)
		{
			m_sharedMaterial = mat;
			m_padding = GetPaddingForMaterial();
			SetMaterialDirty();
			if ((Object)(object)m_sharedMaterial != (Object)null)
			{
				((Object)((Component)this).get_gameObject()).set_name("TMP SubMesh [" + ((Object)m_sharedMaterial).get_name() + "]");
			}
		}

		public float GetPaddingForMaterial()
		{
			return ShaderUtilities.GetPadding(m_sharedMaterial, m_TextComponent.extraPadding, m_TextComponent.isUsingBold);
		}

		public void UpdateMeshPadding(bool isExtraPadding, bool isUsingBold)
		{
			m_padding = ShaderUtilities.GetPadding(m_sharedMaterial, isExtraPadding, isUsingBold);
		}

		public void SetVerticesDirty()
		{
			if (((Behaviour)this).get_enabled() && (Object)(object)m_TextComponent != (Object)null)
			{
				m_TextComponent.havePropertiesChanged = true;
				((Graphic)m_TextComponent).SetVerticesDirty();
			}
		}

		public void SetMaterialDirty()
		{
			UpdateMaterial();
		}

		protected void UpdateMaterial()
		{
			if ((Object)(object)m_renderer == (Object)null)
			{
				m_renderer = renderer;
			}
			m_renderer.set_sharedMaterial(m_sharedMaterial);
			if ((Object)(object)m_sharedMaterial != (Object)null && ((Object)((Component)this).get_gameObject()).get_name() != "TMP SubMesh [" + ((Object)m_sharedMaterial).get_name() + "]")
			{
				((Object)((Component)this).get_gameObject()).set_name("TMP SubMesh [" + ((Object)m_sharedMaterial).get_name() + "]");
			}
		}

		public void UpdateColliders(int vertexCount)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0077: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0117: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)boxCollider == (Object)null))
			{
				Vector2 mAX_16BIT = TMP_Math.MAX_16BIT;
				Vector2 mIN_16BIT = TMP_Math.MIN_16BIT;
				for (int i = 0; i < vertexCount; i++)
				{
					mAX_16BIT.x = Mathf.Min(mAX_16BIT.x, m_mesh.get_vertices()[i].x);
					mAX_16BIT.y = Mathf.Min(mAX_16BIT.y, m_mesh.get_vertices()[i].y);
					mIN_16BIT.x = Mathf.Max(mIN_16BIT.x, m_mesh.get_vertices()[i].x);
					mIN_16BIT.y = Mathf.Max(mIN_16BIT.y, m_mesh.get_vertices()[i].y);
				}
				Vector3 center = Vector2.op_Implicit((mAX_16BIT + mIN_16BIT) / 2f);
				Vector3 size = Vector2.op_Implicit(mIN_16BIT - mAX_16BIT);
				size.z = 0.1f;
				boxCollider.set_center(center);
				boxCollider.set_size(size);
			}
		}

		public TMP_SubMesh()
			: this()
		{
		}
	}
}

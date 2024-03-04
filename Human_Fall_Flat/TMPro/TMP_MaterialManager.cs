using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TMPro
{
	public static class TMP_MaterialManager
	{
		private class FallbackMaterial
		{
			public int baseID;

			public Material baseMaterial;

			public long fallbackID;

			public Material fallbackMaterial;

			public int count;
		}

		private class MaskingMaterial
		{
			public Material baseMaterial;

			public Material stencilMaterial;

			public int count;

			public int stencilID;
		}

		private static List<MaskingMaterial> m_materialList;

		private static Dictionary<long, FallbackMaterial> m_fallbackMaterials;

		private static Dictionary<int, long> m_fallbackMaterialLookup;

		private static List<FallbackMaterial> m_fallbackCleanupList;

		private static bool isFallbackListDirty;

		static TMP_MaterialManager()
		{
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Expected O, but got Unknown
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Expected O, but got Unknown
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Expected O, but got Unknown
			m_materialList = new List<MaskingMaterial>();
			m_fallbackMaterials = new Dictionary<long, FallbackMaterial>();
			m_fallbackMaterialLookup = new Dictionary<int, long>();
			m_fallbackCleanupList = new List<FallbackMaterial>();
			Camera.onPreRender = (CameraCallback)Delegate.Combine((Delegate)(object)Camera.onPreRender, (Delegate)new CameraCallback(OnPreRender));
			Canvas.add_willRenderCanvases(new WillRenderCanvases(OnPreRenderCanvas));
		}

		private static void OnPreRender(Camera cam)
		{
			if (isFallbackListDirty)
			{
				CleanupFallbackMaterials();
				isFallbackListDirty = false;
			}
		}

		private static void OnPreRenderCanvas()
		{
			if (isFallbackListDirty)
			{
				CleanupFallbackMaterials();
				isFallbackListDirty = false;
			}
		}

		public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
		{
			//IL_008a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Expected O, but got Unknown
			if (!baseMaterial.HasProperty(ShaderUtilities.ID_StencilID))
			{
				Debug.LogWarning((object)"Selected Shader does not support Stencil Masking. Please select the Distance Field or Mobile Distance Field Shader.");
				return baseMaterial;
			}
			int instanceID = ((Object)baseMaterial).GetInstanceID();
			for (int i = 0; i < m_materialList.Count; i++)
			{
				if (((Object)m_materialList[i].baseMaterial).GetInstanceID() == instanceID && m_materialList[i].stencilID == stencilID)
				{
					m_materialList[i].count++;
					return m_materialList[i].stencilMaterial;
				}
			}
			Material val = new Material(baseMaterial);
			((Object)val).set_hideFlags((HideFlags)61);
			((Object)val).set_name(((Object)val).get_name() + " Masking ID:" + stencilID);
			val.set_shaderKeywords(baseMaterial.get_shaderKeywords());
			ShaderUtilities.GetShaderPropertyIDs();
			val.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			val.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			MaskingMaterial maskingMaterial = new MaskingMaterial();
			maskingMaterial.baseMaterial = baseMaterial;
			maskingMaterial.stencilMaterial = val;
			maskingMaterial.stencilID = stencilID;
			maskingMaterial.count = 1;
			m_materialList.Add(maskingMaterial);
			return val;
		}

		public static void ReleaseStencilMaterial(Material stencilMaterial)
		{
			int instanceID = ((Object)stencilMaterial).GetInstanceID();
			for (int i = 0; i < m_materialList.Count; i++)
			{
				if (((Object)m_materialList[i].stencilMaterial).GetInstanceID() == instanceID)
				{
					if (m_materialList[i].count > 1)
					{
						m_materialList[i].count--;
						break;
					}
					Object.DestroyImmediate((Object)(object)m_materialList[i].stencilMaterial);
					m_materialList.RemoveAt(i);
					stencilMaterial = null;
					break;
				}
			}
		}

		public static Material GetBaseMaterial(Material stencilMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => (Object)(object)item.stencilMaterial == (Object)(object)stencilMaterial);
			if (num == -1)
			{
				return null;
			}
			return m_materialList[num].baseMaterial;
		}

		public static Material SetStencil(Material material, int stencilID)
		{
			material.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			if (stencilID == 0)
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
			}
			else
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			}
			return material;
		}

		public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => (Object)(object)item.stencilMaterial == (Object)(object)stencilMaterial);
			if (num == -1)
			{
				MaskingMaterial maskingMaterial = new MaskingMaterial();
				maskingMaterial.baseMaterial = baseMaterial;
				maskingMaterial.stencilMaterial = stencilMaterial;
				maskingMaterial.stencilID = stencilID;
				maskingMaterial.count = 1;
				m_materialList.Add(maskingMaterial);
			}
			else
			{
				stencilMaterial = m_materialList[num].stencilMaterial;
				m_materialList[num].count++;
			}
		}

		public static void RemoveStencilMaterial(Material stencilMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => (Object)(object)item.stencilMaterial == (Object)(object)stencilMaterial);
			if (num != -1)
			{
				m_materialList.RemoveAt(num);
			}
		}

		public static void ReleaseBaseMaterial(Material baseMaterial)
		{
			int num = m_materialList.FindIndex((MaskingMaterial item) => (Object)(object)item.baseMaterial == (Object)(object)baseMaterial);
			if (num == -1)
			{
				Debug.Log((object)("No Masking Material exists for " + ((Object)baseMaterial).get_name()));
			}
			else if (m_materialList[num].count > 1)
			{
				m_materialList[num].count--;
				Debug.Log((object)("Removed (1) reference to " + ((Object)m_materialList[num].stencilMaterial).get_name() + ". There are " + m_materialList[num].count + " references left."));
			}
			else
			{
				Debug.Log((object)("Removed last reference to " + ((Object)m_materialList[num].stencilMaterial).get_name() + " with ID " + ((Object)m_materialList[num].stencilMaterial).GetInstanceID()));
				Object.DestroyImmediate((Object)(object)m_materialList[num].stencilMaterial);
				m_materialList.RemoveAt(num);
			}
		}

		public static void ClearMaterials()
		{
			if (m_materialList.Count == 0)
			{
				Debug.Log((object)"Material List has already been cleared.");
				return;
			}
			for (int i = 0; i < m_materialList.Count; i++)
			{
				Object.DestroyImmediate((Object)(object)m_materialList[i].stencilMaterial);
				m_materialList.RemoveAt(i);
			}
		}

		public static int GetStencilID(GameObject obj)
		{
			int num = 0;
			Transform transform = obj.get_transform();
			Transform val = FindRootSortOverrideCanvas(transform);
			if ((Object)(object)transform == (Object)(object)val)
			{
				return num;
			}
			Transform parent = transform.get_parent();
			List<Mask> list = TMP_ListPool<Mask>.Get();
			while ((Object)(object)parent != (Object)null)
			{
				((Component)parent).GetComponents<Mask>(list);
				for (int i = 0; i < list.Count; i++)
				{
					Mask val2 = list[i];
					if ((Object)(object)val2 != (Object)null && val2.MaskEnabled() && ((UIBehaviour)val2.get_graphic()).IsActive())
					{
						num++;
						break;
					}
				}
				if ((Object)(object)parent == (Object)(object)val)
				{
					break;
				}
				parent = parent.get_parent();
			}
			TMP_ListPool<Mask>.Release(list);
			return Mathf.Min((1 << num) - 1, 255);
		}

		public static Material GetMaterialForRendering(MaskableGraphic graphic, Material baseMaterial)
		{
			if ((Object)(object)baseMaterial == (Object)null)
			{
				return null;
			}
			List<IMaterialModifier> list = TMP_ListPool<IMaterialModifier>.Get();
			((Component)graphic).GetComponents<IMaterialModifier>(list);
			Material val = baseMaterial;
			for (int i = 0; i < list.Count; i++)
			{
				val = list[i].GetModifiedMaterial(val);
			}
			TMP_ListPool<IMaterialModifier>.Release(list);
			return val;
		}

		private static Transform FindRootSortOverrideCanvas(Transform start)
		{
			List<Canvas> list = TMP_ListPool<Canvas>.Get();
			((Component)start).GetComponentsInParent<Canvas>(false, list);
			Canvas val = null;
			for (int i = 0; i < list.Count; i++)
			{
				val = list[i];
				if (val.get_overrideSorting())
				{
					break;
				}
			}
			TMP_ListPool<Canvas>.Release(list);
			if (!((Object)(object)val != (Object)null))
			{
				return null;
			}
			return ((Component)val).get_transform();
		}

		public static Material GetFallbackMaterial(Material sourceMaterial, Material targetMaterial)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0065: Expected O, but got Unknown
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0115: Expected O, but got Unknown
			int instanceID = ((Object)sourceMaterial).GetInstanceID();
			Texture texture = targetMaterial.GetTexture(ShaderUtilities.ID_MainTex);
			int instanceID2 = ((Object)texture).GetInstanceID();
			long num = ((long)instanceID << 32) | (uint)instanceID2;
			if (m_fallbackMaterials.TryGetValue(num, out var value))
			{
				return value.fallbackMaterial;
			}
			Material val = null;
			if (sourceMaterial.HasProperty(ShaderUtilities.ID_GradientScale) && targetMaterial.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				val = new Material(sourceMaterial);
				((Object)val).set_hideFlags((HideFlags)61);
				Material obj = val;
				((Object)obj).set_name(((Object)obj).get_name() + " + " + ((Object)texture).get_name());
				val.SetTexture(ShaderUtilities.ID_MainTex, texture);
				val.SetFloat(ShaderUtilities.ID_GradientScale, targetMaterial.GetFloat(ShaderUtilities.ID_GradientScale));
				val.SetFloat(ShaderUtilities.ID_TextureWidth, targetMaterial.GetFloat(ShaderUtilities.ID_TextureWidth));
				val.SetFloat(ShaderUtilities.ID_TextureHeight, targetMaterial.GetFloat(ShaderUtilities.ID_TextureHeight));
				val.SetFloat(ShaderUtilities.ID_WeightNormal, targetMaterial.GetFloat(ShaderUtilities.ID_WeightNormal));
				val.SetFloat(ShaderUtilities.ID_WeightBold, targetMaterial.GetFloat(ShaderUtilities.ID_WeightBold));
			}
			else
			{
				val = new Material(targetMaterial);
			}
			value = new FallbackMaterial();
			value.baseID = instanceID;
			value.baseMaterial = sourceMaterial;
			value.fallbackID = num;
			value.fallbackMaterial = val;
			value.count = 0;
			m_fallbackMaterials.Add(num, value);
			m_fallbackMaterialLookup.Add(((Object)val).GetInstanceID(), num);
			return val;
		}

		public static void AddFallbackMaterialReference(Material targetMaterial)
		{
			if (!((Object)(object)targetMaterial == (Object)null))
			{
				int instanceID = ((Object)targetMaterial).GetInstanceID();
				if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
				{
					value2.count++;
				}
			}
		}

		public static void RemoveFallbackMaterialReference(Material targetMaterial)
		{
			if ((Object)(object)targetMaterial == (Object)null)
			{
				return;
			}
			int instanceID = ((Object)targetMaterial).GetInstanceID();
			if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
			{
				value2.count--;
				if (value2.count < 1)
				{
					m_fallbackCleanupList.Add(value2);
				}
			}
		}

		public static void CleanupFallbackMaterials()
		{
			if (m_fallbackCleanupList.Count == 0)
			{
				return;
			}
			for (int i = 0; i < m_fallbackCleanupList.Count; i++)
			{
				FallbackMaterial fallbackMaterial = m_fallbackCleanupList[i];
				if (fallbackMaterial.count < 1)
				{
					Material fallbackMaterial2 = fallbackMaterial.fallbackMaterial;
					m_fallbackMaterials.Remove(fallbackMaterial.fallbackID);
					m_fallbackMaterialLookup.Remove(((Object)fallbackMaterial2).GetInstanceID());
					Object.DestroyImmediate((Object)(object)fallbackMaterial2);
					fallbackMaterial2 = null;
				}
			}
			m_fallbackCleanupList.Clear();
		}

		public static void ReleaseFallbackMaterial(Material fallackMaterial)
		{
			if ((Object)(object)fallackMaterial == (Object)null)
			{
				return;
			}
			int instanceID = ((Object)fallackMaterial).GetInstanceID();
			if (m_fallbackMaterialLookup.TryGetValue(instanceID, out var value) && m_fallbackMaterials.TryGetValue(value, out var value2))
			{
				value2.count--;
				if (value2.count < 1)
				{
					m_fallbackCleanupList.Add(value2);
				}
			}
			isFallbackListDirty = true;
		}

		public static void CopyMaterialPresetProperties(Material source, Material destination)
		{
			if (source.HasProperty(ShaderUtilities.ID_GradientScale) && destination.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				Texture texture = destination.GetTexture(ShaderUtilities.ID_MainTex);
				float @float = destination.GetFloat(ShaderUtilities.ID_GradientScale);
				float float2 = destination.GetFloat(ShaderUtilities.ID_TextureWidth);
				float float3 = destination.GetFloat(ShaderUtilities.ID_TextureHeight);
				float float4 = destination.GetFloat(ShaderUtilities.ID_WeightNormal);
				float float5 = destination.GetFloat(ShaderUtilities.ID_WeightBold);
				destination.CopyPropertiesFromMaterial(source);
				destination.set_shaderKeywords(source.get_shaderKeywords());
				destination.SetTexture(ShaderUtilities.ID_MainTex, texture);
				destination.SetFloat(ShaderUtilities.ID_GradientScale, @float);
				destination.SetFloat(ShaderUtilities.ID_TextureWidth, float2);
				destination.SetFloat(ShaderUtilities.ID_TextureHeight, float3);
				destination.SetFloat(ShaderUtilities.ID_WeightNormal, float4);
				destination.SetFloat(ShaderUtilities.ID_WeightBold, float5);
			}
		}
	}
}

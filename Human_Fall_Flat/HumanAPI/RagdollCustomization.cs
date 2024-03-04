using System;
using System.Collections.Generic;
using UnityEngine;

namespace HumanAPI
{
	public class RagdollCustomization : MonoBehaviour
	{
		public Ragdoll ragdoll;

		public RagdollModel main;

		public RagdollModel head;

		public RagdollModel upper;

		public RagdollModel lower;

		public RagdollPresetMetadata preset;

		internal List<RigClipVolume> cachedClipVolumes = new List<RigClipVolume>();

		public bool allowHead
		{
			get
			{
				if (((Object)(object)main == (Object)null || main.allowHead) && ((Object)(object)upper == (Object)null || upper.allowHead))
				{
					if (!((Object)(object)lower == (Object)null))
					{
						return lower.allowHead;
					}
					return true;
				}
				return false;
			}
		}

		public bool allowUpper
		{
			get
			{
				if (((Object)(object)main == (Object)null || main.allowUpperBody) && ((Object)(object)lower == (Object)null || lower.allowUpperBody))
				{
					if (!((Object)(object)head == (Object)null))
					{
						return head.allowUpperBody;
					}
					return true;
				}
				return false;
			}
		}

		public bool allowLower
		{
			get
			{
				if (((Object)(object)main == (Object)null || main.allowLowerBody) && ((Object)(object)upper == (Object)null || upper.allowLowerBody))
				{
					if (!((Object)(object)head == (Object)null))
					{
						return head.allowLowerBody;
					}
					return true;
				}
				return false;
			}
		}

		private void OnEnable()
		{
			ragdoll = ((Component)this).GetComponent<Ragdoll>();
		}

		private RagdollModel BindToSkin(WorkshopItemMetadata wsMeta)
		{
			if (wsMeta == null)
			{
				return null;
			}
			RagdollModelMetadata ragdollModelMetadata = wsMeta as RagdollModelMetadata;
			if (ragdollModelMetadata != null)
			{
				RagdollModel component;
				if ((Object)(object)ragdollModelMetadata.modelPrefab == (Object)null)
				{
					AssetBundle obj = WorkshopRepository.LoadBundle(ragdollModelMetadata);
					obj.GetAllAssetNames();
					Object obj2 = obj.LoadAsset(ragdollModelMetadata.model);
					component = Object.Instantiate<GameObject>((GameObject)(object)((obj2 is GameObject) ? obj2 : null)).GetComponent<RagdollModel>();
					obj.Unload(false);
				}
				else
				{
					component = Object.Instantiate<GameObject>(((Component)ragdollModelMetadata.modelPrefab).get_gameObject()).GetComponent<RagdollModel>();
				}
				component.meta = ragdollModelMetadata;
				component.BindToRagdoll(ragdoll);
				component.texture.LoadFromPreset(preset);
				return component;
			}
			return null;
		}

		internal List<RigClipVolume> GetClipVolumes(bool forceClip)
		{
			List<RigClipVolume> list = new List<RigClipVolume>();
			if ((Object)(object)main != (Object)null && (main.clipInCustomize || forceClip))
			{
				list.AddRange(((Component)main).GetComponentsInChildren<RigClipVolume>());
			}
			if ((Object)(object)head != (Object)null && (head.clipInCustomize || forceClip || ((Object)(object)main != (Object)null && main.clipInCustomize)))
			{
				list.AddRange(((Component)head).GetComponentsInChildren<RigClipVolume>());
			}
			if ((Object)(object)upper != (Object)null && (upper.clipInCustomize || forceClip || ((Object)(object)main != (Object)null && main.clipInCustomize)))
			{
				list.AddRange(((Component)upper).GetComponentsInChildren<RigClipVolume>());
			}
			if ((Object)(object)lower != (Object)null && (lower.clipInCustomize || forceClip || ((Object)(object)main != (Object)null && main.clipInCustomize)))
			{
				list.AddRange(((Component)lower).GetComponentsInChildren<RigClipVolume>());
			}
			return list;
		}

		public void ClearOutCachedClipVolumes()
		{
			cachedClipVolumes.Clear();
		}

		internal void ApplyPreset(RagdollPresetMetadata preset, bool forceRebuild = false)
		{
			if (!forceRebuild && this.preset != null && preset != null && this.preset.folder != preset.folder)
			{
				forceRebuild = true;
			}
			this.preset = preset;
			bool flag = false;
			if (!forceRebuild)
			{
				string obj = (((Object)(object)main != (Object)null && main.meta != null && main.meta.folder != null) ? main.meta.folder : string.Empty);
				string text = ((preset != null && preset.main != null && preset.main.modelPath != null) ? preset.main.modelPath : string.Empty);
				flag = obj != text;
			}
			bool flag2 = RebindMain(preset, forceRebuild);
			RebindHead(preset, forceRebuild);
			RebindUpper(preset, forceRebuild);
			RebindLower(preset, forceRebuild);
			List<RigClipVolume> clipVolumes = GetClipVolumes(forceRebuild);
			if (!forceRebuild)
			{
				bool flag3 = flag || cachedClipVolumes.Count != clipVolumes.Count;
				if (!flag3)
				{
					for (int i = 0; i < cachedClipVolumes.Count; i++)
					{
						flag3 |= (Object)(object)cachedClipVolumes[i] != (Object)(object)clipVolumes[i];
					}
				}
				if (flag3)
				{
					forceRebuild = true;
					if (!flag2)
					{
						flag2 = RebindMain(preset, forceRebuild);
					}
				}
			}
			if (forceRebuild && (Object)(object)main != (Object)null)
			{
				RagdollModelSkinnedMesh[] componentsInChildren = ((Component)main).GetComponentsInChildren<RagdollModelSkinnedMesh>();
				RigClipVolume[] clipVolumes2 = clipVolumes.ToArray();
				cachedClipVolumes = clipVolumes;
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].Clip(clipVolumes2);
				}
			}
			else if ((Object)(object)main == (Object)null)
			{
				cachedClipVolumes.Clear();
			}
		}

		private bool RebindMain(RagdollPresetMetadata preset, bool forceRebuild)
		{
			if ((Object)(object)main != (Object)null && (forceRebuild || preset == null || preset.main == null || main.meta.folder != preset.main.modelPath))
			{
				main.Unbind(ragdoll);
				Object.DestroyImmediate((Object)(object)((Component)main).get_gameObject());
				main = null;
			}
			if ((Object)(object)main == (Object)null && preset != null && preset.main != null)
			{
				RagdollModelMetadata item = WorkshopRepository.instance.GetPartRepository(WorkshopItemType.ModelFull).GetItem(preset.main.modelPath);
				if (item != null)
				{
					main = BindToSkin(item);
					return true;
				}
			}
			return false;
		}

		private bool RebindHead(RagdollPresetMetadata preset, bool forceRebuild)
		{
			if ((Object)(object)head != (Object)null && (forceRebuild || !allowHead || preset == null || preset.head == null || preset.head.modelPath != head.meta.folder))
			{
				head.Unbind(ragdoll);
				Object.DestroyImmediate((Object)(object)((Component)head).get_gameObject());
				head = null;
			}
			if ((Object)(object)head == (Object)null && allowHead && preset != null && preset.head != null)
			{
				RagdollModelMetadata item = WorkshopRepository.instance.GetPartRepository(WorkshopItemType.ModelHead).GetItem(preset.head.modelPath);
				if (item != null)
				{
					head = BindToSkin(item);
					return true;
				}
			}
			return false;
		}

		private bool RebindUpper(RagdollPresetMetadata preset, bool forceRebuild)
		{
			if ((Object)(object)upper != (Object)null && (forceRebuild || !allowUpper || preset == null || preset.upperBody == null || preset.upperBody.modelPath != upper.meta.folder))
			{
				upper.Unbind(ragdoll);
				Object.DestroyImmediate((Object)(object)((Component)upper).get_gameObject());
				upper = null;
			}
			if ((Object)(object)upper == (Object)null && allowUpper && preset != null && preset.upperBody != null)
			{
				RagdollModelMetadata item = WorkshopRepository.instance.GetPartRepository(WorkshopItemType.ModelUpperBody).GetItem(preset.upperBody.modelPath);
				if (item != null)
				{
					upper = BindToSkin(item);
					return true;
				}
			}
			return false;
		}

		private bool RebindLower(RagdollPresetMetadata preset, bool forceRebuild)
		{
			if ((Object)(object)lower != (Object)null && (forceRebuild || !allowLower || preset == null || preset.lowerBody == null || preset.lowerBody.modelPath != lower.meta.folder))
			{
				lower.Unbind(ragdoll);
				Object.DestroyImmediate((Object)(object)((Component)lower).get_gameObject());
				lower = null;
			}
			if ((Object)(object)lower == (Object)null && allowLower && preset != null && preset.lowerBody != null)
			{
				RagdollModelMetadata item = WorkshopRepository.instance.GetPartRepository(WorkshopItemType.ModelLowerBody).GetItem(preset.lowerBody.modelPath);
				if (item != null)
				{
					lower = BindToSkin(item);
					return true;
				}
			}
			return false;
		}

		public void RebindColors(bool bake, bool compress = false)
		{
			if ((Object)(object)main != (Object)null)
			{
				RebindColors(main, bake, compress);
			}
			if ((Object)(object)head != (Object)null)
			{
				RebindColors(head, bake, compress);
			}
			if ((Object)(object)upper != (Object)null)
			{
				RebindColors(upper, bake, compress);
			}
			if ((Object)(object)lower != (Object)null)
			{
				RebindColors(lower, bake, compress);
			}
		}

		public RagdollModel GetModel(WorkshopItemType part)
		{
			return part switch
			{
				WorkshopItemType.ModelFull => main, 
				WorkshopItemType.ModelHead => head, 
				WorkshopItemType.ModelUpperBody => upper, 
				WorkshopItemType.ModelLowerBody => lower, 
				_ => throw new InvalidOperationException(), 
			};
		}

		public void RebindColors(RagdollModel model, bool bake, bool compress)
		{
			model.texture.ApplyPresetColors(preset, bake, compress);
		}

		public RagdollCustomization()
			: this()
		{
		}
	}
}

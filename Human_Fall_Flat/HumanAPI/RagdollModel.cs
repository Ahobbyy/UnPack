using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("HumanRagdoll/Ragdoll Model", 10)]
	public class RagdollModel : MonoBehaviour
	{
		public enum Rigging
		{
			AutoRig,
			BoneMap
		}

		public WorkshopItemType ragdollPart;

		public Rigging rigType;

		public bool replacesCharacter;

		public bool clipInCustomize;

		public bool allowHead = true;

		public bool allowUpperBody = true;

		public bool allowLowerBody = true;

		public Texture2D baseTexture;

		public Texture2D maskTexture;

		public bool mask1;

		public bool mask2;

		public bool mask3;

		public Color color1;

		public Color color2;

		public Color color3;

		public Transform partHead;

		public Transform partChest;

		public Transform partWaist;

		public Transform partHips;

		public Transform partLeftArm;

		public Transform partLeftForearm;

		public Transform partLeftHand;

		public Transform partLeftThigh;

		public Transform partLeftLeg;

		public Transform partLeftFoot;

		public Transform partRightArm;

		public Transform partRightForearm;

		public Transform partRightHand;

		public Transform partRightThigh;

		public Transform partRightLeg;

		public Transform partRightFoot;

		internal RagdollModelMetadata meta;

		public RagdollTexture texture;

		public RagdollPaddedModel _padded;

		private Renderer[] texturedRenderers;

		public RagdollPaddedModel padded
		{
			get
			{
				if ((Object)(object)_padded == (Object)null)
				{
					_padded = ((Component)this).get_gameObject().AddComponent<RagdollPaddedModel>();
					_padded.CreatePaddedMesh(texturedRenderers);
				}
				return _padded;
			}
		}

		protected void OnDestroy()
		{
			if ((Object)(object)maskTexture != (Object)null)
			{
				TextureTracker.instance.RemoveMapping((Object)(object)this, (Object)(object)maskTexture);
				maskTexture = null;
			}
		}

		public void BindToRagdoll(Ragdoll ragdoll)
		{
			((Component)this).get_transform().SetParent(((Component)ragdoll).get_transform());
			texturedRenderers = ((Component)this).GetComponentsInChildren<Renderer>();
			if (rigType == Rigging.AutoRig && (Object)(object)((Component)this).GetComponentInChildren<RagdollModelSkinnedMesh>() == (Object)null)
			{
				MeshRenderer[] componentsInChildren = ((Component)this).GetComponentsInChildren<MeshRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					if (((Renderer)componentsInChildren[i]).get_enabled())
					{
						((Component)componentsInChildren[i]).get_gameObject().AddComponent<RagdollModelSkinnedMesh>();
					}
				}
				SkinnedMeshRenderer[] componentsInChildren2 = ((Component)this).GetComponentsInChildren<SkinnedMeshRenderer>();
				for (int j = 0; j < componentsInChildren2.Length; j++)
				{
					if (((Renderer)componentsInChildren2[j]).get_enabled())
					{
						((Component)componentsInChildren2[j]).get_gameObject().AddComponent<RagdollModelSkinnedMesh>();
					}
				}
			}
			RagdollModelSkinnedMesh[] componentsInChildren3 = ((Component)this).GetComponentsInChildren<RagdollModelSkinnedMesh>();
			if (componentsInChildren3.Length != 0)
			{
				for (int k = 0; k < componentsInChildren3.Length; k++)
				{
					componentsInChildren3[k].Reskin2(ragdoll);
				}
			}
			BindBone(ragdoll.partHead, partHead);
			BindBone(ragdoll.partChest, partChest);
			BindBone(ragdoll.partWaist, partWaist);
			BindBone(ragdoll.partHips, partHips);
			BindBone(ragdoll.partLeftArm, partLeftArm);
			BindBone(ragdoll.partLeftForearm, partLeftForearm);
			BindBone(ragdoll.partLeftHand, partLeftHand);
			BindBone(ragdoll.partLeftThigh, partLeftThigh);
			BindBone(ragdoll.partLeftLeg, partLeftLeg);
			BindBone(ragdoll.partLeftFoot, partLeftFoot);
			BindBone(ragdoll.partRightArm, partRightArm);
			BindBone(ragdoll.partRightForearm, partRightForearm);
			BindBone(ragdoll.partRightHand, partRightHand);
			BindBone(ragdoll.partRightThigh, partRightThigh);
			BindBone(ragdoll.partRightLeg, partRightLeg);
			BindBone(ragdoll.partRightFoot, partRightFoot);
			for (int l = 0; l < componentsInChildren3.Length; l++)
			{
				for (int m = 0; m < texturedRenderers.Length; m++)
				{
					if ((Object)(object)texturedRenderers[m] == (Object)(object)componentsInChildren3[l].originalRenderer)
					{
						texturedRenderers[m] = componentsInChildren3[l].reskinnedRenderer;
					}
				}
			}
			texture = ((Component)this).get_gameObject().AddComponent<RagdollTexture>();
			texture.BindToModel(this);
		}

		private void BindBone(HumanSegment segment, Transform bone)
		{
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			//IL_003f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_004c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0051: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)bone != (Object)null)
			{
				Matrix4x4 val = segment.bindPose * bone.get_localToWorldMatrix();
				bone.SetParent(segment.transform, false);
				bone.set_localPosition(Vector4.op_Implicit(((Matrix4x4)(ref val)).GetColumn(3)));
				bone.set_localRotation(Quaternion.LookRotation(Vector4.op_Implicit(((Matrix4x4)(ref val)).GetColumn(2)), Vector4.op_Implicit(((Matrix4x4)(ref val)).GetColumn(1))));
			}
		}

		public void Unbind(Ragdoll ragdoll)
		{
			UnbindBone(ragdoll.partHead, partHead);
			UnbindBone(ragdoll.partChest, partChest);
			UnbindBone(ragdoll.partWaist, partWaist);
			UnbindBone(ragdoll.partHips, partHips);
			UnbindBone(ragdoll.partLeftArm, partLeftArm);
			UnbindBone(ragdoll.partLeftForearm, partLeftForearm);
			UnbindBone(ragdoll.partLeftHand, partLeftHand);
			UnbindBone(ragdoll.partLeftThigh, partLeftThigh);
			UnbindBone(ragdoll.partLeftLeg, partLeftLeg);
			UnbindBone(ragdoll.partLeftFoot, partLeftFoot);
			UnbindBone(ragdoll.partRightArm, partRightArm);
			UnbindBone(ragdoll.partRightForearm, partRightForearm);
			UnbindBone(ragdoll.partRightHand, partRightHand);
			UnbindBone(ragdoll.partRightThigh, partRightThigh);
			UnbindBone(ragdoll.partRightLeg, partRightLeg);
			UnbindBone(ragdoll.partRightFoot, partRightFoot);
		}

		private void UnbindBone(HumanSegment segment, Transform bone)
		{
			if ((Object)(object)bone != (Object)null)
			{
				bone.SetParent(((Component)this).get_transform(), false);
			}
		}

		public void SetTexture(Texture texture)
		{
			for (int i = 0; i < texturedRenderers.Length; i++)
			{
				texturedRenderers[i].get_material().set_mainTexture(texture);
			}
		}

		public void ShowMask(bool show)
		{
			for (int i = 0; i < texturedRenderers.Length; i++)
			{
				if (!((Object)(object)texturedRenderers[i].get_material().get_shader() != (Object)(object)Shaders.instance.showMaskShader) || texturedRenderers[i].get_material().GetFloat("_Mode") != 3f)
				{
					texturedRenderers[i].get_material().set_shader(show ? Shaders.instance.showMaskShader : Shaders.instance.opaqueHumanShader);
					texturedRenderers[i].get_material().SetTexture("_MaskTex", (Texture)(object)maskTexture);
				}
			}
		}

		public void SetMask(int mask)
		{
			for (int i = 0; i < texturedRenderers.Length; i++)
			{
				texturedRenderers[i].get_material().SetFloat("_Mask1", (float)(((mask & 1) == 1) ? 1 : 0));
				texturedRenderers[i].get_material().SetFloat("_Mask2", (float)(((mask & 2) == 2) ? 1 : 0));
				texturedRenderers[i].get_material().SetFloat("_Mask3", (float)(((mask & 4) == 4) ? 1 : 0));
			}
		}

		public RagdollModel()
			: this()
		{
		}
	}
}

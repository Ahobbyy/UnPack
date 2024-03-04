using System;
using Multiplayer;
using UnityEngine;

namespace HumanAPI
{
	[AddComponentMenu("HumanRagdoll/Ragdoll Skinned Mesh", 10)]
	public class RagdollModelSkinnedMesh : MonoBehaviour
	{
		public Renderer originalRenderer;

		public Renderer reskinnedRenderer;

		private Mesh meshToDestroy;

		internal void Reskin2(Ragdoll ragdoll)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0061: Unknown result type (might be due to invalid IL or missing references)
			//IL_0066: Unknown result type (might be due to invalid IL or missing references)
			//IL_0068: Expected O, but got Unknown
			//IL_006d: Expected O, but got Unknown
			//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Expected O, but got Unknown
			//IL_00d5: Expected O, but got Unknown
			//IL_013f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0144: Unknown result type (might be due to invalid IL or missing references)
			//IL_0184: Unknown result type (might be due to invalid IL or missing references)
			//IL_0189: Unknown result type (might be due to invalid IL or missing references)
			//IL_019a: Unknown result type (might be due to invalid IL or missing references)
			//IL_019f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_01de: Unknown result type (might be due to invalid IL or missing references)
			//IL_020b: Unknown result type (might be due to invalid IL or missing references)
			Matrix4x4[] bindposes = (Matrix4x4[])(object)new Matrix4x4[RagdollTemplate.instance.bindposes.Length];
			Matrix4x4 localToWorldMatrix = ((Component)this).get_transform().get_localToWorldMatrix();
			for (int i = 0; i < bindposes.Length; i++)
			{
				bindposes[i] = RagdollTemplate.instance.bindposes[i] * localToWorldMatrix;
			}
			Mesh val = null;
			SkinnedMeshRenderer val2 = ((Component)this).GetComponent<SkinnedMeshRenderer>();
			if (Object.op_Implicit((Object)(object)val2))
			{
				Mesh val3 = new Mesh();
				val = val3;
				meshToDestroy = val3;
				val2.BakeMesh(val);
				originalRenderer = (Renderer)(object)val2;
				reskinnedRenderer = (Renderer)(object)val2;
			}
			else
			{
				MeshFilter component = ((Component)this).GetComponent<MeshFilter>();
				MeshRenderer component2 = ((Component)component).GetComponent<MeshRenderer>();
				if ((Object)(object)component == (Object)null || (Object)(object)component2 == (Object)null)
				{
					Debug.LogError((object)"Trying to resking gameobject without mesh renderer", (Object)(object)this);
					return;
				}
				((Renderer)component2).set_enabled(false);
				originalRenderer = (Renderer)(object)component2;
				Mesh val4 = new Mesh();
				val = val4;
				meshToDestroy = val4;
				val.set_vertices(component.get_sharedMesh().get_vertices());
				val.set_triangles(component.get_sharedMesh().get_triangles());
				val.set_uv(component.get_sharedMesh().get_uv());
				val.set_uv2(component.get_sharedMesh().get_uv2());
				val.set_normals(component.get_sharedMesh().get_normals());
				GameObject val5 = new GameObject(((Object)this).get_name() + "Skinned");
				val5.get_transform().SetParent(((Component)this).get_transform(), false);
				val2 = val5.AddComponent<SkinnedMeshRenderer>();
				((Renderer)val2).set_sharedMaterials(((Renderer)component2).get_sharedMaterials());
				reskinnedRenderer = (Renderer)(object)val2;
			}
			Time.get_realtimeSinceStartup();
			Vector3[] vertices = val.get_vertices();
			Matrix4x4 localToWorldMatrix2 = ((Component)this).get_transform().get_localToWorldMatrix();
			for (int j = 0; j < vertices.Length; j++)
			{
				vertices[j] = ((Matrix4x4)(ref localToWorldMatrix2)).MultiplyPoint3x4(vertices[j]);
			}
			BoneWeight[] array = (BoneWeight[])(object)new BoneWeight[vertices.Length];
			for (int k = 0; k < array.Length; k++)
			{
				array[k] = RagdollTemplate.instance.Map(vertices[k]);
			}
			Transform[] bones = ragdoll.bones;
			if (App.state != AppSate.Customize)
			{
				Override(ragdoll, vertices, array, ref bones, ref bindposes, localToWorldMatrix);
			}
			val.set_boneWeights(array);
			val2.set_sharedMesh(val);
			val.set_bindposes(bindposes);
			val2.set_bones(bones);
			val2.set_rootBone(bones[0]);
			val.RecalculateBounds();
		}

		private void Override(Ragdoll ragdoll, Vector3[] targetPositions, BoneWeight[] targetWeights, ref Transform[] bones, ref Matrix4x4[] bindposes, Matrix4x4 targetMeshToWorld)
		{
			//IL_0057: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0063: Unknown result type (might be due to invalid IL or missing references)
			//IL_009d: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_0111: Unknown result type (might be due to invalid IL or missing references)
			//IL_0116: Unknown result type (might be due to invalid IL or missing references)
			//IL_011a: Unknown result type (might be due to invalid IL or missing references)
			//IL_011f: Unknown result type (might be due to invalid IL or missing references)
			SkinOverrideVolume[] componentsInChildren = ((Component)this).GetComponentsInChildren<SkinOverrideVolume>();
			if (componentsInChildren.Length == 0)
			{
				return;
			}
			int num = bindposes.Length;
			Array.Resize(ref bindposes, bindposes.Length + componentsInChildren.Length);
			Array.Resize(ref bones, bones.Length + componentsInChildren.Length);
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				SkinOverrideVolume skinOverrideVolume = componentsInChildren[i];
				bones[num + i] = ((Component)skinOverrideVolume).get_transform();
				bindposes[num + i] = skinOverrideVolume.GetBindPose(((Component)ragdoll).get_transform()) * targetMeshToWorld;
			}
			float[] array = new float[componentsInChildren.Length];
			for (int j = 0; j < targetPositions.Length; j++)
			{
				float num2 = 0f;
				for (int k = 0; k < componentsInChildren.Length; k++)
				{
					SkinOverrideVolume skinOverrideVolume2 = componentsInChildren[k];
					array[k] = skinOverrideVolume2.GetWeight(targetPositions[j]);
					num2 += array[k];
				}
				if (num2 != 0f)
				{
					BoneWeight b = BoneWeightUtils.FindBestBones(array);
					((BoneWeight)(ref b)).set_boneIndex0(((BoneWeight)(ref b)).get_boneIndex0() + num);
					((BoneWeight)(ref b)).set_boneIndex1(((BoneWeight)(ref b)).get_boneIndex1() + num);
					((BoneWeight)(ref b)).set_boneIndex2(((BoneWeight)(ref b)).get_boneIndex2() + num);
					((BoneWeight)(ref b)).set_boneIndex3(((BoneWeight)(ref b)).get_boneIndex3() + num);
					targetWeights[j] = BoneWeightUtils.Lerp(targetWeights[j], b, num2);
				}
			}
		}

		private void OnDestroy()
		{
			if ((Object)(object)meshToDestroy != (Object)null)
			{
				Object.Destroy((Object)(object)meshToDestroy);
			}
		}

		public void Clip(RigClipVolume[] clipVolumes)
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			if (clipVolumes.Length == 0)
			{
				return;
			}
			Renderer obj = reskinnedRenderer;
			Mesh sharedMesh = ((SkinnedMeshRenderer)((obj is SkinnedMeshRenderer) ? obj : null)).get_sharedMesh();
			Vector3[] vertices = sharedMesh.get_vertices();
			for (int i = 0; i < vertices.Length; i++)
			{
				vertices[i] = ((Component)reskinnedRenderer).get_transform().TransformPoint(vertices[i]);
			}
			bool[] array = new bool[vertices.Length];
			for (int j = 0; j < clipVolumes.Length; j++)
			{
				clipVolumes[j].Clip(array, vertices);
			}
			int[] triangles = sharedMesh.get_triangles();
			for (int k = 0; k < triangles.Length; k += 3)
			{
				if (array[triangles[k]] && array[triangles[k + 1]] && array[triangles[k + 2]])
				{
					triangles[k + 1] = (triangles[k + 2] = triangles[k]);
				}
			}
			sharedMesh.set_triangles(triangles);
			Renderer obj2 = reskinnedRenderer;
			((SkinnedMeshRenderer)((obj2 is SkinnedMeshRenderer) ? obj2 : null)).set_sharedMesh(sharedMesh);
		}

		public RagdollModelSkinnedMesh()
			: this()
		{
		}
	}
}

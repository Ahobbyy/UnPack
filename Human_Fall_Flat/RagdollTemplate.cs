using System;
using UnityEngine;

public class RagdollTemplate : MonoBehaviour, IDependency
{
	public static RagdollTemplate instance;

	public Ragdoll ragdoll;

	public RigVolume[] rigVolumes;

	[NonSerialized]
	public Matrix4x4[] bindposes;

	private float[] weights;

	public void Initialize()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		instance = this;
		Transform[] bones = ragdoll.bones;
		weights = new float[bones.Length];
		for (int i = 0; i < rigVolumes.Length; i++)
		{
			rigVolumes[i].Build(bones);
		}
		bindposes = (Matrix4x4[])(object)new Matrix4x4[bones.Length];
		for (int j = 0; j < bindposes.Length; j++)
		{
			bindposes[j] = bones[j].get_worldToLocalMatrix() * ((Component)ragdoll).get_transform().get_localToWorldMatrix();
		}
		Dependencies.OnInitialized(this);
	}

	public void PaintWeights(Vector3 pos, float[] weights)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		float num = 1f;
		for (int num2 = rigVolumes.Length - 1; num2 >= 0; num2--)
		{
			num *= 1f - rigVolumes[num2].PaintWeights(pos, num, weights, mirror: false);
			if (rigVolumes[num2].mirrorBoneIndex >= 0)
			{
				num *= 1f - rigVolumes[num2].PaintWeights(pos, num, weights, mirror: true);
			}
		}
	}

	public BoneWeight Map(Vector3 pos)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < weights.Length; i++)
		{
			weights[i] = 0f;
		}
		PaintWeights(pos, weights);
		return BoneWeightUtils.FindBestBones(weights);
	}

	public RagdollTemplate()
		: this()
	{
	}
}

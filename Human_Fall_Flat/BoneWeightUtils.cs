using UnityEngine;

public static class BoneWeightUtils
{
	public static BoneWeight FindBestBones(float[] weights)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		BoneWeight result = default(BoneWeight);
		((BoneWeight)(ref result)).set_boneIndex0(FindBestBone(weights));
		((BoneWeight)(ref result)).set_weight0(weights[((BoneWeight)(ref result)).get_boneIndex0()]);
		weights[((BoneWeight)(ref result)).get_boneIndex0()] = 0f;
		((BoneWeight)(ref result)).set_boneIndex1(FindBestBone(weights));
		((BoneWeight)(ref result)).set_weight1(weights[((BoneWeight)(ref result)).get_boneIndex1()]);
		weights[((BoneWeight)(ref result)).get_boneIndex1()] = 0f;
		((BoneWeight)(ref result)).set_boneIndex2(FindBestBone(weights));
		((BoneWeight)(ref result)).set_weight2(weights[((BoneWeight)(ref result)).get_boneIndex2()]);
		weights[((BoneWeight)(ref result)).get_boneIndex2()] = 0f;
		((BoneWeight)(ref result)).set_boneIndex3(FindBestBone(weights));
		((BoneWeight)(ref result)).set_weight3(weights[((BoneWeight)(ref result)).get_boneIndex3()]);
		weights[((BoneWeight)(ref result)).get_boneIndex3()] = 0f;
		float num = ((BoneWeight)(ref result)).get_weight0() + ((BoneWeight)(ref result)).get_weight1() + ((BoneWeight)(ref result)).get_weight2() + ((BoneWeight)(ref result)).get_weight3();
		if (num != 0f)
		{
			((BoneWeight)(ref result)).set_weight0(((BoneWeight)(ref result)).get_weight0() / num);
			((BoneWeight)(ref result)).set_weight1(((BoneWeight)(ref result)).get_weight1() / num);
			((BoneWeight)(ref result)).set_weight2(((BoneWeight)(ref result)).get_weight2() / num);
			((BoneWeight)(ref result)).set_weight3(((BoneWeight)(ref result)).get_weight3() / num);
		}
		return result;
	}

	private static int FindBestBone(float[] weights)
	{
		float num = 0f;
		int result = 0;
		for (int i = 0; i < weights.Length; i++)
		{
			if (num < weights[i])
			{
				num = weights[i];
				result = i;
			}
		}
		return result;
	}

	public static BoneWeight Lerp(BoneWeight a, BoneWeight b, float mix)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_025c: Unknown result type (might be due to invalid IL or missing references)
		if (mix <= 0f)
		{
			return a;
		}
		if (mix >= 1f)
		{
			return b;
		}
		((BoneWeight)(ref a)).set_weight0(((BoneWeight)(ref a)).get_weight0() * (1f - mix));
		((BoneWeight)(ref a)).set_weight1(((BoneWeight)(ref a)).get_weight1() * (1f - mix));
		((BoneWeight)(ref a)).set_weight2(((BoneWeight)(ref a)).get_weight2() * (1f - mix));
		((BoneWeight)(ref a)).set_weight3(((BoneWeight)(ref a)).get_weight3() * (1f - mix));
		((BoneWeight)(ref b)).set_weight0(((BoneWeight)(ref b)).get_weight0() * mix);
		((BoneWeight)(ref b)).set_weight1(((BoneWeight)(ref b)).get_weight1() * mix);
		((BoneWeight)(ref b)).set_weight2(((BoneWeight)(ref b)).get_weight2() * mix);
		((BoneWeight)(ref b)).set_weight3(((BoneWeight)(ref b)).get_weight3() * mix);
		BoneWeight result = default(BoneWeight);
		int num = 0;
		int num2 = 0;
		if (a.GetWeight(num) > b.GetWeight(num2))
		{
			((BoneWeight)(ref result)).set_weight0(a.GetWeight(num));
			((BoneWeight)(ref result)).set_boneIndex0(a.GetBoneIndex(num));
			num++;
		}
		else
		{
			((BoneWeight)(ref result)).set_weight0(b.GetWeight(num2));
			((BoneWeight)(ref result)).set_boneIndex0(b.GetBoneIndex(num2));
			num2++;
		}
		if (a.GetWeight(num) > b.GetWeight(num2))
		{
			((BoneWeight)(ref result)).set_weight1(a.GetWeight(num));
			((BoneWeight)(ref result)).set_boneIndex1(a.GetBoneIndex(num));
			num++;
		}
		else
		{
			((BoneWeight)(ref result)).set_weight1(b.GetWeight(num2));
			((BoneWeight)(ref result)).set_boneIndex1(b.GetBoneIndex(num2));
			num2++;
		}
		if (a.GetWeight(num) > b.GetWeight(num2))
		{
			((BoneWeight)(ref result)).set_weight2(a.GetWeight(num));
			((BoneWeight)(ref result)).set_boneIndex2(a.GetBoneIndex(num));
			num++;
		}
		else
		{
			((BoneWeight)(ref result)).set_weight2(b.GetWeight(num2));
			((BoneWeight)(ref result)).set_boneIndex2(b.GetBoneIndex(num2));
			num2++;
		}
		if (a.GetWeight(num) > b.GetWeight(num2))
		{
			((BoneWeight)(ref result)).set_weight3(a.GetWeight(num));
			((BoneWeight)(ref result)).set_boneIndex3(a.GetBoneIndex(num));
			num++;
		}
		else
		{
			((BoneWeight)(ref result)).set_weight3(b.GetWeight(num2));
			((BoneWeight)(ref result)).set_boneIndex3(b.GetBoneIndex(num2));
			num2++;
		}
		float num3 = ((BoneWeight)(ref result)).get_weight0() + ((BoneWeight)(ref result)).get_weight1() + ((BoneWeight)(ref result)).get_weight2() + ((BoneWeight)(ref result)).get_weight3();
		if (num3 != 0f)
		{
			((BoneWeight)(ref result)).set_weight0(((BoneWeight)(ref result)).get_weight0() / num3);
			((BoneWeight)(ref result)).set_weight1(((BoneWeight)(ref result)).get_weight1() / num3);
			((BoneWeight)(ref result)).set_weight2(((BoneWeight)(ref result)).get_weight2() / num3);
			((BoneWeight)(ref result)).set_weight3(((BoneWeight)(ref result)).get_weight3() / num3);
		}
		return result;
	}

	public static int GetBoneIndex(this BoneWeight w, int i)
	{
		return i switch
		{
			0 => ((BoneWeight)(ref w)).get_boneIndex0(), 
			1 => ((BoneWeight)(ref w)).get_boneIndex1(), 
			2 => ((BoneWeight)(ref w)).get_boneIndex2(), 
			_ => ((BoneWeight)(ref w)).get_boneIndex3(), 
		};
	}

	public static float GetWeight(this BoneWeight w, int i)
	{
		return i switch
		{
			0 => ((BoneWeight)(ref w)).get_weight0(), 
			1 => ((BoneWeight)(ref w)).get_weight1(), 
			2 => ((BoneWeight)(ref w)).get_weight2(), 
			_ => ((BoneWeight)(ref w)).get_weight3(), 
		};
	}
}

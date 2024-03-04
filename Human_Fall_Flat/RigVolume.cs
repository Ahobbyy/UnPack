using UnityEngine;

public class RigVolume : MonoBehaviour
{
	public string boneName;

	public string mirrorBoneName;

	public int boneIndex = -1;

	public int mirrorBoneIndex = -1;

	public RigVolumeMesh inner;

	public RigVolumeMesh outer;

	public void Build(Transform[] bones)
	{
		boneIndex = -1;
		for (int i = 0; i < bones.Length; i++)
		{
			if (((Object)bones[i]).get_name() == boneName)
			{
				boneIndex = i;
				break;
			}
		}
		mirrorBoneIndex = -1;
		for (int j = 0; j < bones.Length; j++)
		{
			if (((Object)bones[j]).get_name() == mirrorBoneName)
			{
				mirrorBoneIndex = j;
				break;
			}
		}
		if ((Object)(object)inner != (Object)null)
		{
			inner.Build(-1f);
		}
		if ((Object)(object)outer != (Object)null)
		{
			outer.Build(1f);
		}
	}

	public float PaintWeights(Vector3 pos, float opacity, float[] weights, bool mirror)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		if (mirror)
		{
			pos.x *= -1f;
		}
		int num = (mirror ? mirrorBoneIndex : boneIndex);
		float weight = GetWeight(pos);
		weights[num] = Mathf.Lerp(weights[num], 1f, weight * opacity);
		return weight;
	}

	private float GetWeight(Vector3 pos)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)inner == (Object)null || (Object)(object)outer == (Object)null)
		{
			return 1f;
		}
		float distOutside = inner.GetDistOutside(pos);
		float distInside = outer.GetDistInside(pos);
		if (distInside <= 0f)
		{
			return 0f;
		}
		if (distOutside <= 0f)
		{
			return 1f;
		}
		return distInside / (distOutside + distInside);
	}

	public RigVolume()
		: this()
	{
	}
}

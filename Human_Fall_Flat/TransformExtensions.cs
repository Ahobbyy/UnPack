using UnityEngine;

public static class TransformExtensions
{
	public static Transform FindRecursive(this Transform transform, string name)
	{
		if (((Object)transform).get_name().Equals(name))
		{
			return transform;
		}
		for (int i = 0; i < transform.get_childCount(); i++)
		{
			Transform val = transform.GetChild(i).FindRecursive(name);
			if ((Object)(object)val != (Object)null)
			{
				return val;
			}
		}
		return null;
	}
}

using UnityEngine;

public class RigClipVolume : RigVolumeMesh
{
	public void Clip(bool[] canClip, Vector3[] vertices)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Build(1f);
		for (int i = 0; i < vertices.Length; i++)
		{
			if (!canClip[i])
			{
				float distInside = GetDistInside(vertices[i]);
				canClip[i] = distInside > 0f;
			}
		}
	}
}

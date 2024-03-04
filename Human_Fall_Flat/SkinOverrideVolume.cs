using UnityEngine;

public class SkinOverrideVolume : MonoBehaviour
{
	public float weight = 1f;

	public bool isBox;

	public float innerRadius = 0.49f;

	public float outerRadius = 0.5f;

	public Vector3 boxScale = new Vector3(0.25f, 0.25f, 0.25f);

	public Color colour = new Color(1f, 0f, 0f, 0.5f);

	private bool initedBox;

	private Bounds boxBounds;

	public Matrix4x4 GetBindPose(Transform ragdoll)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)this).get_transform().get_worldToLocalMatrix();
	}

	public float GetWeight(Vector3 pos)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (isBox)
		{
			if (!initedBox)
			{
				boxBounds = new Bounds(((Component)this).get_transform().get_position(), boxScale);
				initedBox = true;
			}
			if (((Bounds)(ref boxBounds)).Contains(pos))
			{
				return weight;
			}
			return 0f;
		}
		float num = outerRadius;
		float num2 = innerRadius;
		Vector3 val = ((Component)this).get_transform().InverseTransformPoint(pos);
		return Mathf.InverseLerp(num, num2, ((Vector3)(ref val)).get_magnitude());
	}

	private void OnDrawGizmosSelected()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		if (isBox)
		{
			Gizmos.set_color(colour);
			Gizmos.DrawCube(((Component)this).get_transform().get_position(), boxScale);
			return;
		}
		Gizmos.set_color(new Color(1f, 1f, 0f, 0.5f));
		Gizmos.DrawSphere(((Component)this).get_transform().get_position(), innerRadius);
		Gizmos.set_color(new Color(1f, 0f, 0f, 0.5f));
		Gizmos.DrawSphere(((Component)this).get_transform().get_position(), outerRadius);
	}

	public SkinOverrideVolume()
		: this()
	{
	}//IL_0031: Unknown result type (might be due to invalid IL or missing references)
	//IL_0036: Unknown result type (might be due to invalid IL or missing references)
	//IL_0050: Unknown result type (might be due to invalid IL or missing references)
	//IL_0055: Unknown result type (might be due to invalid IL or missing references)

}

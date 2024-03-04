using UnityEngine;

public class SkinOverrideBox : MonoBehaviour
{
	public float weight = 1f;

	public Vector3 scale = new Vector3(0.25f, 0.25f, 0.25f);

	public Color colour = new Color(1f, 0f, 0f, 0.5f);

	public Matrix4x4 GetBindPose(Transform ragdoll)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return ((Component)this).get_transform().get_worldToLocalMatrix();
	}

	public float GetWeight(Vector3 pos)
	{
		return 1f;
	}

	private void OnDrawGizmosSelected()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(colour);
		Gizmos.DrawCube(((Component)this).get_transform().get_position(), scale);
	}

	public SkinOverrideBox()
		: this()
	{
	}//IL_001b: Unknown result type (might be due to invalid IL or missing references)
	//IL_0020: Unknown result type (might be due to invalid IL or missing references)
	//IL_003a: Unknown result type (might be due to invalid IL or missing references)
	//IL_003f: Unknown result type (might be due to invalid IL or missing references)

}

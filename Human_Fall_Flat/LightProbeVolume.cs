using UnityEngine;

public class LightProbeVolume : MonoBehaviour
{
	public RaycastHit hitInfo;

	public Vector3 offset;

	public void OnDrawGizmosSelected()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_yellow());
		Gizmos.DrawLine(((RaycastHit)(ref hitInfo)).get_point(), ((RaycastHit)(ref hitInfo)).get_point() + offset);
		Gizmos.DrawSphere(((RaycastHit)(ref hitInfo)).get_point() + offset, 0.1f);
	}

	public LightProbeVolume()
		: this()
	{
	}
}

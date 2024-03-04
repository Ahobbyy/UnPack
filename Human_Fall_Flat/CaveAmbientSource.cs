using UnityEngine;

public class CaveAmbientSource : MonoBehaviour
{
	public float radius = 2f;

	public float minRadius = 1f;

	public Color color;

	public float intensity = 0.1f;

	public void OnDrawGizmosSelected()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Gizmos.set_color(Color.get_yellow());
		Gizmos.DrawWireSphere(((Component)this).get_transform().get_position(), minRadius);
		Gizmos.DrawWireSphere(((Component)this).get_transform().get_position(), radius);
	}

	public CaveAmbientSource()
		: this()
	{
	}
}
